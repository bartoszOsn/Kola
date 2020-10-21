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
                string customStyle = Properties.Resources.Custom;
                customStyle = customStyle.Replace("@bcg", (App.Current.Resources["BackgroundColorVariant"] as SolidColorBrush).Color.ToCSSString());
                customStyle = customStyle.Replace("@fnt", (App.Current.Resources["FontColor"] as SolidColorBrush).Color.ToCSSString());
                string fullstyle = $"<style>{customStyle}</style>";
                content = HtmlHelper.GetHtml(content,
                    (App.Current.Resources["BackgroundColorVariant"] as SolidColorBrush).Color,
                    (App.Current.Resources["FontColor"] as SolidColorBrush).Color,
                    (App.Current.Resources["PrimaryColorVariant"] as SolidColorBrush).Color);
            }
            return content;
        }

        public override string ToString()
        {
            return $"{Title} [{ProjectName}]";
        }
    }
}
