using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Kola.Helpers;

namespace Kola.Helpers.Wiki
{
    public class WikiPage
    {
        public string ID { get; private set; }
        public string Title { get; private set; }
        public string Lang { get; private set; }
        public string ProjectName => project.ProjectName;
        public string ProjectIcon => project.IconString;
        private WikiProject project;

        public WikiPage(WikiProject project, string pageID, string pageTitle, string lang)
        {
            this.project = project;
            this.ID = pageID;
            this.Title = pageTitle;
            this.Lang = lang;
        }

        public async Task<string> GetContent(bool fullHTML = true)
        {
            string content = await project.GetPageContent(ID, Lang);
            if(fullHTML)
            {
                content = HtmlHelper.GetHtml(content);
            }
            return content;
        }

        public override string ToString()
        {
            return $"{Title} [{ProjectName}]";
        }
    }
}
