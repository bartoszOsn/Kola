using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kola.Helpers.Wiki
{
    public class WikiPage
    {
        public string ID { get; private set; }
        public string Title { get; private set; }
        public string ProjectName => project.ProjectName;
        private WikiProject project;

        public WikiPage(WikiProject project, string pageID, string pageTitle)
        {
            this.project = project;
            this.ID = pageID;
            this.Title = pageTitle;
        }

        public async Task<string> GetContent(bool fullHTML = true)
        {
            string content = await project.GetPageContent(ID);
            if(fullHTML)
            {
                content = $"<html><head><style>{Properties.Resources.load}</style></head><body style=\"margin: 20px;\">{content}</body></html>";
            }
            return content;
        }

        public override string ToString()
        {
            return $"{Title} [{ProjectName}]";
        }
    }
}
