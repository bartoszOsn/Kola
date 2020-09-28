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
    class ArchiveBook : ComicBook, INotifyPropertyChanged
    {
        public ArchiveBook(string path) : base(path)
        {
            archive = SharpCompress.Archives.ArchiveFactory.Open(path);
            FilterEntries();
            PageNumber = 0;
            cts = new CancellationTokenSource();
            unpackingTask = new Task(UnpackingMethod, cts.Token, TaskCreationOptions.LongRunning);
            unpackingTask.Start();
        }

        public override int PageNumber
        {
            get
            {
                return pageNumber;
            }
            set
            {
                int p;
                if (value < 0)
                {
                    p = 0;
                }
                else if (value >= PageCount)
                {
                    p = PageCount - 1;
                }
                else
                {
                    p = value;
                }
                if (p == PageNumber)
                {
                    return;
                }
                pageNumber = p;
                Changed(nameof(PageNumber));
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

        private Task unpackingTask;
        private CancellationTokenSource cts;

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
        private ImageSource GenerateImage()
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
            return image;
        }

        private void UnpackingMethod()
        {
            int currentPage = pageNumber;
            while(true)
            {
                currentPage = pageNumber;
                pageImage = null;
                Changed(nameof(PageImage));
                ImageSource source = GenerateImage();
                if(cts.IsCancellationRequested)
                {
                    break;
                }
                if(currentPage != pageNumber)
                {
                    continue;
                }
                pageImage = source;
                Changed(nameof(PageImage));
                while (currentPage == pageNumber) { } //Wait while page is not changed.
            }
        }
    }
}
