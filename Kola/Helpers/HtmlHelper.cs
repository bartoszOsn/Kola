using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kola.Helpers
{
    static class HtmlHelper
    {
        public static string GetHtml(string content = "")
        {
            string customStyle = Properties.Resources.Custom;
            customStyle = customStyle.Replace("@background-color", (App.Current.Resources["BackgroundColorVariant"] as SolidColorBrush).Color.ToCSSString());
            customStyle = customStyle.Replace("@font-color", (App.Current.Resources["FontColor"] as SolidColorBrush).Color.ToCSSString());
            customStyle = customStyle.Replace("@primary-color", (App.Current.Resources["PrimaryColorVariant"] as SolidColorBrush).Color.ToCSSString());
            string fullstyle = $"<style>{customStyle}</style>";
            return $"<html><head>{fullstyle}</head><body>{content}</body></html>";
        }
    }
}
