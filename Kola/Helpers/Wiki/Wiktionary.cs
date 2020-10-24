using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kola.Helpers.Wiki
{
    class Wiktionary : WikiProject
    {
        public override string ProjectName => "Wiktionary";
        public override string IconString => ((char)(0xf02d)).ToString();
        protected override string HostName => "wiktionary.org";

        protected override string GetContent(XmlDocument doc)
        {
            return doc.SelectSingleNode("/api/query/pages/page/extract").InnerText;
        }

        protected override IEnumerable<WikiPage> GetPages(XmlDocument doc, string lang)
        {
            return doc
                .SelectNodes("/api/query/prefixsearch/ps")
                .Cast<XmlNode>()
                .Select(t => new WikiPage(this, t.Attributes["pageid"].Value, t.Attributes["title"].Value, lang))
                .ToList();
        }

        protected override StringDictionary GetSearchVariables(string query)
        {
            return new StringDictionary()
            {
                { "action", "query" },
                { "list", "prefixsearch" },
                { "pssearch", query }
            };
        }

        protected override StringDictionary GetVariablesToGetContent(string pageId)
        {
            return new StringDictionary()
            {
                { "action", "query" },
                { "pageids", pageId },
                { "prop", "extracts" },
            };
        }
    }
}
