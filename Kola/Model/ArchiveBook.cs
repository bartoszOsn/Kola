using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kola.Model
{
    class ArchiveBook : ComicBook, INotifyPropertyChanged
    {
        public ArchiveBook(string path) : base(path)
        {
            archive = SharpCompress.Archives.ArchiveFactory.Open(path);
            FilterEntries();
            PageNumber = 0;
        }

        public override int PageNumber
        {
            get
            {
                return pageNumber;
            }
            set
            {
                if (value < 0)
                {
                    pageNumber = 0;
                }
                else if (value >= PageCount)
                {
                    pageNumber = PageCount - 1;
                }
                else
                {
                    pageNumber = value;
                }
                GenerateImage();
                Changed(nameof(PageNumber));
                Changed(nameof(PageImage));
            }
        }

        public override int PageCount
        {
            get
            {
                return imageEntries.Length;
            }
        }

        public override ImageSource PageImage { get { return pageImage; } }

        public override void Close()
        {
            archive.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private SharpCompress.Archives.IArchive archive;
        private SharpCompress.Archives.IArchiveEntry[] imageEntries;

        private int pageNumber;
        private ImageSource pageImage;

        private void Changed(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void FilterEntries()
        {
            imageEntries = archive.Entries.Where(t => {
                string ext = System.IO.Path.GetExtension(t.Key).ToLower();
                return ext == ".png" || ext == ".jpg" || ext == ".bmp";
            }).ToArray();
        }
        private void GenerateImage()
        {
            BitmapImage image = new BitmapImage();
            Stream imgStream = imageEntries[PageNumber].OpenEntryStream();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;

            image.UriSource = null;

            using (MemoryStream ms = new MemoryStream())
            {
                imgStream.CopyTo(ms, (int)imageEntries[PageNumber].Size);
                ms.Position = 0;
                image.StreamSource = ms;
                image.EndInit();
            }

            image.Freeze();
            imgStream.Dispose();
            pageImage = image;
        }
    }
}
