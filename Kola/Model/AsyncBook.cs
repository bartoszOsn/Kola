using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kola.Model
{
    //TODO: load many pages to memory at once, to avoid delay in displaing then when page is changed.
    /// <summary>
    /// Base class for books with asynchronic generation of page images. 
    /// </summary>
    public abstract class AsyncBook : ComicBook
    {
        public AsyncBook(string path) : base(path)
        {
            PageNumber = 0;
        }

        private Task unpackingTask;
        private CancellationTokenSource cts;
        private int pageNumber;
        private ImageSource pageImage;
        private Dictionary<int, ImageSource> pages = new Dictionary<int, ImageSource>();

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
                    p = (PageCount == 0)? 0 : PageCount - 1;
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
                Changed(nameof(PageImage));
            }
        }

        public override ImageSource PageImage => pages.ContainsKey(PageNumber) ? pages[PageNumber] : null;

        public override void GainFocus()
        {
            cts = new CancellationTokenSource();
            unpackingTask = new Task(UnpackingMethod, cts.Token, TaskCreationOptions.LongRunning);
            unpackingTask.Start();
        }

        public override void LostFocus()
        {
            cts?.Cancel();
        }

        private async void UnpackingMethod()
        {
            int currentPage;
            try
            {
                while (true)
                {
                    currentPage = pageNumber;
                    pageImage = null;
                    cts.Token.ThrowIfCancellationRequested();
                    
                    for(int i = 0; i < Properties.Settings.Default.PagesInMemoryCount; i++)
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        ClearPages();
                        int key = PageNumber + (i + 1) / 2 * (i % 2 == 0 ? -1 : 1);

                        if (key < 0 || key >= PageCount)
                            continue;

                        ImageSource source = await GetImage(key, cts.Token);
                        pages[key] = source;
                        Console.WriteLine($"Generated page: [{key}]\tPageNumber: [{PageNumber}]\tAll pages: [{string.Join(", ", pages.Keys)}]");
                        if(currentPage != PageNumber) // if page was changed, start again.
                        {
                            Console.WriteLine("Page Changed, I start again");
                            i = -1;
                            currentPage = PageNumber;
                        }
                        if(key == PageNumber)
                        {
                            Changed(nameof(PageImage));
                        }
                    }
                    while (currentPage == pageNumber) //Wait while page is not changed.
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        await Task.Delay(10);
                    }
                }
            }
            catch(OperationCanceledException e)
            {

            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Removes pages which are out of range.
        /// </summary>
        private void ClearPages()
        {
            int min = PageNumber + (int)-Math.Round(Properties.Settings.Default.PagesInMemoryCount / 2.0) + 1;
            int max = PageNumber + (int)Math.Floor(Properties.Settings.Default.PagesInMemoryCount / 2.0);

            Console.WriteLine($"Clearing, min: [{min}], max: [{max}]");

            if (min < 0)
                min = 0;
            if (max >= PageCount)
                max = PageCount - 1;

            foreach(var key in pages.Keys.ToArray())
            {
                if(key < min || key > max)
                {
                    pages.Remove(key);
                }
            }
        }

        abstract protected Task<ImageSource> GetImage(int pageNumber, CancellationToken cancellationToken);
    }
}
