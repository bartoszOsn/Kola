using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kola.Model
{
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
            }
        }

        public override ImageSource PageImage { get { return pageImage; } }

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
                    Changed(nameof(PageImage));
                    cts.Token.ThrowIfCancellationRequested();
                    ImageSource source = await GetImage(pageNumber, cts.Token);
                    cts.Token.ThrowIfCancellationRequested();
                    if (currentPage != pageNumber)
                    {
                        continue;
                    }
                    pageImage = source;
                    Changed(nameof(PageImage));
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

        abstract protected Task<ImageSource> GetImage(int pageNumber, CancellationToken cancellationToken);
    }
}
