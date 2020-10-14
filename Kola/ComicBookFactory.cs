﻿using Kola.Model;
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
        struct FormatInfo
        {
            public string[] Formats { get; set; }
            public string Description { get; set; }
            public Func<string, ComicBook> Opener { get; set; }
            public FormatInfo(string[] formats, string desc, Func<string, ComicBook> opener)
            {
                Formats = formats;
                Description = desc;
                Opener = opener;
            }
        }
        static public ComicBook Create(string path)
        {
            string extension = Path.GetExtension(path);
            if(hasFormat(extension))
            {
                return GetFormat(extension).Opener.Invoke(path);
            }
            else
            {
                throw new FileFormatException(String.Format("Can't open file with extension \"{0}\".", extension));
            }
        }
        
        static public string GetFilter()
        {
            return string.Join("|",
                creators.Select(t =>
                {
                    string filter = string.Join(";", t.Formats.Select(u=> "*" + u));
                    return $"{t.Description} ({filter})|{filter}";
                })) + "|All files (*.*)|*.*";
        }

        private static FormatInfo[] creators;
        static ComicBookFactory()
        {
            creators = new FormatInfo[]
            {
                new FormatInfo(new string[]{".cbr", ".cbz"}, "Comic book archives", CreateArchiveBook),
                new FormatInfo(new string[]{".rar", ".zip"}, "Archives", CreateArchiveBook)
            };
        }

        private static bool hasFormat(string f)
        {
            return creators.Any(t => t.Formats.Contains(f));
        }

        private static FormatInfo GetFormat(string f)
        {
            return creators.First(t => t.Formats.Contains(f));
        }

        private static ComicBook CreateArchiveBook(string path)
        {
            return new ArchiveBook(path);
        }
    }
}
