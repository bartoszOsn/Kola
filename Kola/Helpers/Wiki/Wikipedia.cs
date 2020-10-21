using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kola.Helpers.Wiki
{
    class Wikipedia : WikiProject
    {
        protected override string HostName => "wikipedia.org";
        public override string ProjectName => "Wikipedia";
        public override string IconString => ((char)(0xf0ac)).ToString();

        protected override StringDictionary GetSearchVariables(string query)
        {
            return new StringDictionary()
            {
                { "action", "query" },
                { "list", "prefixsearch" },
                { "pssearch", query },
                { "prop", "extracts" },
                { "exchars", "10" }
            };
        }
        protected override StringDictionary GetVariablesToGetContent(string pageId)
        {
            return new StringDictionary()
            {
                { "action", "query" },
                { "pageids", pageId },
                { "prop", "extracts" },
                { "exintro", "true" },
                { "redirects", "true" }
            };
        }

        protected override string GetContent(XmlDocument doc)
        {
            return doc.SelectSingleNode("/api/query/pages/page/extract").InnerText;
        }

        protected override IEnumerable<WikiPage> GetPages(XmlDocument doc)
        {
            return doc
                .SelectNodes("/api/query/prefixsearch/ps")
                .Cast<XmlNode>()
                .Select(t => new WikiPage(this, t.Attributes["pageid"].Value, t.Attributes["title"].Value))
                .ToList();
        }
    }
}
