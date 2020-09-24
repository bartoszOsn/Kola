using Kola.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kola
{
    static class ComicBookFactory
    {
        static public ComicBook Create(string path)
        {
            string extension = Path.GetExtension(path);
            if(creators.ContainsKey(extension))
            {
                return creators[extension].Invoke(path);
            }
            else
            {
                throw new FileFormatException(String.Format("Can't open file with extension \"{0}\".", extension));
            }
        }

        private static Dictionary<string, Func<string, ComicBook>> creators;
        static ComicBookFactory()
        {
            creators = new Dictionary<string, Func<string, ComicBook>>()
            {
                {".cbz",  CreateCBZ},
                {".zip",  CreateCBZ}
            };
        }

        private static ComicBook CreateCBZ(string path)
        {
            return new CBZBook(path);
        }
    }
}
