using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kola.Helpers.Wiki
{
    static class Wiki
    {
        private static WikiProject[] projects = new WikiProject[]
        {
            new Wikipedia(),
            new Wiktionary()
        };

        public static async Task<IEnumerable<WikiPage>> Search(string query)
        {
            List<WikiPage> result = new System.Collections.Generic.List<WikiPage>();
            foreach(WikiProject project in projects)
            {
                result.AddRange(await project.Search(query));
            }
            return result;
        }
    }
}
