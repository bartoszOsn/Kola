using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kola.Model
{
    abstract class ComicBook
    {
        public string Name { get; set; }
        public abstract int PageNumber { get; set; }
        public abstract int PageCount { get; }
        public abstract ImageSource Page { get; }

        public void NextPage()
        {
            PageNumber++;
        }
        public void PreviousPage()
        {
            PageNumber--;
        }
    }
}
