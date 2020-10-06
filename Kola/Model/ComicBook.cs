using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kola.Model
{
    /// <summary>
    /// Base class representing a file on disc which can be open by Kola
    /// </summary>
    public abstract class ComicBook
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="path">Path to file.</param>
        public ComicBook(string path)
        {
            Path = path;
        }
        /// <summary>
        /// Path to file.
        /// </summary>
        public string Path { get; protected set; }
        /// <summary>
        /// Name of file, including extension.
        /// </summary>
        public string Name { get => System.IO.Path.GetFileName(Path); }
        /// <summary>
        /// Number of the page that comic is opened at.
        /// </summary>
        public abstract int PageNumber { get; set; }
        /// <summary>
        /// Number of pages in comic.
        /// </summary>
        public abstract int PageCount { get; }
        /// <summary>
        /// ImageSource of current page.
        /// </summary>
        public abstract ImageSource PageImage { get; }

        /// <summary>
        /// Invoked when tab is being closed.
        /// </summary>
        public abstract void Close();
        /// <summary>
        /// Invoked when tab is selected, for example when user clicks at tab or opens new tab.
        /// </summary>
        public abstract void GainFocus();
        /// <summary>
        /// Invokes when user selects another tab.
        /// </summary>
        public abstract void LostFocus();

        /// <summary>
        /// Turns the page forward.
        /// </summary>
        public void NextPage()
        {
            PageNumber++;
        }
        /// <summary>
        /// Turns the page backward.
        /// </summary>
        public void PreviousPage()
        {
            PageNumber--;
        }
    }
}
