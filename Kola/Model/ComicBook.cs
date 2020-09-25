using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kola.Model
{
    public abstract class ComicBook
    {
        public ComicBook(string path)
        {
            Path = path;
        }
        public string Path { get; protected set; }
        public string Name { get => System.IO.Path.GetFileName(Path); }
        public abstract int PageNumber { get; set; }
        public abstract int PageCount { get; }
        public abstract ImageSource PageImage { get; }


        public abstract void Close();
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
