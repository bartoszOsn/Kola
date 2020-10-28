using Kola.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kola.Model
{
    class ImageBook : ComicBook
    {
        public static readonly string[] supportedImageExtensions = new string[]
        {
            ".bmp", ".png", ".jpg", ".gif"
        };

        public override int PageNumber
        {
            get => currentPage;
            set
            {
                currentPage = MathHelper.Clamp(value, 0, PageCount - 1);
                GenerateImageSource();
                Changed(nameof(PageNumber));
            }
        }

        public override int PageCount => pages.Length;

        public override ImageSource PageImage => currentPageImage;

        private int currentPage = 0;
        private ImageSource currentPageImage;
        private string[] pages;

        public ImageBook(string path) : base(System.IO.Path.GetDirectoryName(path))
        {
            pages = System.IO.Directory.GetFiles(Path).Where(t => supportedImageExtensions.Contains(System.IO.Path.GetExtension(t))).ToArray();
            PageNumber = 0;
        }

        public override void Close()
        {
        }

        public override void GainFocus()
        {
        }

        public override void LostFocus()
        {
        }

        private void GenerateImageSource()
        {
            currentPageImage = new BitmapImage(new Uri(pages[currentPage]));
            Changed(nameof(PageImage));
        }
    }
}
