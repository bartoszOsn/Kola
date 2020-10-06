using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kola.Model
{
    class ArchiveBook : AsyncBook
    {
        public ArchiveBook(string path) : base(path)
        {
            archive = SharpCompress.Archives.ArchiveFactory.Open(path);
            FilterEntries();
        }

        public override int PageCount
        {
            get
            {
                return imageEntries?.Length ?? 0;
            }
        }

        public override void Close()
        {
            archive.Dispose();
        }

        private SharpCompress.Archives.IArchive archive;
        private SharpCompress.Archives.IArchiveEntry[] imageEntries;

        private void FilterEntries()
        {
            imageEntries = archive.Entries.Where(t => {
                string ext = System.IO.Path.GetExtension(t.Key).ToLower();
                return ext == ".png" || ext == ".jpg" || ext == ".bmp";
            }).ToArray();
        }

        protected override async Task<ImageSource> GetImage(int pageNumber, CancellationToken cancellationToken)
        {
            BitmapImage image = new BitmapImage();
            Stream imgStream = imageEntries[pageNumber].OpenEntryStream();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;

            image.UriSource = null;

            using (MemoryStream ms = new MemoryStream())
            {
                int size = (int)imageEntries[PageNumber].Size;
                imgStream.CopyToAsync(ms, size, cancellationToken).Wait();
                ms.Position = 0;
                image.StreamSource = ms;
                image.EndInit();
            }

            image.Freeze();
            imgStream.Dispose();
            return image;
        }
    }
}
