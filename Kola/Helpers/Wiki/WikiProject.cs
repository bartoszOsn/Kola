using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kola.Helpers.Wiki
{
    /// <summary>
    /// Base class for Wikimedia Projects.
    /// Derrive from it to Search pages and to get page content.
    /// </summary>
    public abstract class WikiProject
    {
        /// <summary>
        /// Name of the project, for example <c>Wikipedia</c> or <c>Wiktionary</c>.
        /// </summary>
        public abstract string ProjectName { get; }

        /// <summary>
        /// Icon of project, as string representing icon in FontAwesome
        /// </summary>
        public abstract string IconString { get; }

        /// <summary>
        /// Host name, included domain name, for example <c>wikipedia.org</c>
        /// </summary>
        protected abstract string HostName { get; }

        private HttpClient httpClient = new HttpClient();

        /// <summary>
        /// returns content of a page.
        /// </summary>
        /// <param name="id">id of page</param>
        /// <returns>page content, more specifically children of the body of html document.</returns>
        public async Task<string> GetPageContent(string id, string lang)
        {
            XmlDocument doc;
            try
            {
                doc = await GetDocument(GetVariablesToGetContent(id), lang);
            }
            catch(HttpRequestException)
            {
                return "<h2>Network error</h2><p>Check your internet connection and try again.</p>";
            }
            return GetContent(doc);
        }

        /// <summary>
        /// Returns pages that match given query
        /// </summary>
        /// <param name="query">String to be searched.</param>
        /// <param name="lang">Language.</param>
        public async Task<IEnumerable<WikiPage>> Search(string query, string lang)
        {
            XmlDocument doc;
            try
            {
                doc = await GetDocument(GetSearchVariables(query), lang);
            }
            catch(HttpRequestException)
            {
                return new WikiPage[0];
            }
            return GetPages(doc, lang);
        }

        /// <summary>
        /// returns dictionary with url variables which should be appended to url to search page. 
        /// </summary>
        protected abstract StringDictionary GetSearchVariables(string query);
        /// <summary>
        /// returns dictionary with url variables which should be appended to url to get page content. 
        /// </summary>
        protected abstract StringDictionary GetVariablesToGetContent(string pageId);

        /// <summary>
        /// returns content of a page.
        /// </summary>
        /// <param name="doc">xml document returned by wikimedia API.</param>
        /// <returns>page content extracted from API response, more specifically children of the body of html document.</returns>
        protected abstract string GetContent(XmlDocument doc);
        /// <summary>
        /// returns pages returned by wikimedia API.
        /// </summary>
        /// <param name="doc">xml document returned by wikimedia API.</param>
        /// <param name="lang">Language of document.</param>
        /// <returns>pages extracted from API response.</returns>
        protected abstract IEnumerable<WikiPage> GetPages(XmlDocument doc, string lang);

        /// <summary>
        /// Returns API url with given variables, and appends variable <c>format</c> with value <c>xml</c>
        /// </summary>
        /// <param name="variables">Variables</param>
        /// <param name="lang">language site to be used.</param>
        private string GetUrl(StringDictionary variables, string lang)
        {
            string url = String.Format("https://{0}.{1}/w/api.php", lang, HostName);
            if(variables.Count > 0)
            {
                url += "?";
            }
            foreach(DictionaryEntry entry in variables)
            {
                url += entry.Key + "=" + entry.Value + "&";
            }
            url += "format=xml";
            return url;
        }

        /// <summary>
        /// Returns document received from API call with given variables.
        /// </summary>
        private async Task<XmlDocument> GetDocument(StringDictionary variables, string lang)
        {
            string response;
            XmlDocument doc = new XmlDocument();
            try
            {
                string url = GetUrl(variables, lang);
                response = await httpClient.GetStringAsync(url);
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
            
            doc.LoadXml(response);
            return doc;
        }
    }
}
