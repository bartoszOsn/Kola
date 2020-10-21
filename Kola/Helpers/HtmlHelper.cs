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
        public static string GetHtml(string content = "", Color backgroundColor = default, Color fontColor = default, Color scrollbarColor = default)
        {
            string customStyle = Properties.Resources.Custom;
            customStyle = customStyle.Replace("@bcg", backgroundColor.ToCSSString());
            customStyle = customStyle.Replace("@fnt", fontColor.ToCSSString());
            customStyle = customStyle.Replace("@scroll-color", scrollbarColor.ToCSSString());
            string fullstyle = $"<style>{customStyle}</style>";
            return $"<html><head>{fullstyle}</head><body>{content}</body></html>";
        }
    }
}
