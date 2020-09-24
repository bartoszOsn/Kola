using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kola.Model
{
    class Model
    {
        public ObservableCollection<ComicBook> Tabs { get; set; }
        public int SelectedTabIndex { get; set; }
        public ComicBook SelectedTab
        {
            get
            {
                return Tabs[SelectedTabIndex];
            }
            set
            {
                SelectedTabIndex = Tabs.IndexOf(value);
            }
        }
    }
}
