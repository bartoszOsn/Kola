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
    class CBZBook : ComicBook, INotifyPropertyChanged
    {
        public CBZBook(string path) : base(path)
        {
            zip = new ZipArchive(File.OpenRead(path), ZipArchiveMode.Read, false);
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
                else if(value >= PageCount)
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
            zip.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ZipArchive zip;
        private ZipArchiveEntry[] imageEntries;

        private int pageNumber;
        private ImageSource pageImage;

        private void Changed(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void FilterEntries()
        {
            imageEntries = zip.Entries.Where(t => {
                string ext = System.IO.Path.GetExtension(t.FullName).ToLower();
                return ext == ".png" || ext == ".jpg" || ext == ".bmp";
                }).ToArray();
        }
        private void GenerateImage()
        {
            BitmapImage image = new BitmapImage();
            Stream imgStream = imageEntries[PageNumber].Open();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            
            image.UriSource = null;

            using(MemoryStream ms = new MemoryStream())
            {
                imgStream.CopyTo(ms, (int)imageEntries[PageNumber].Length);
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
