using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kola.Model
{
    class Model : INotifyPropertyChanged
    {
        public Model()
        {
            Tabs = new ObservableCollection<ComicBook>();
        }
        public ObservableCollection<ComicBook> Tabs { get; private set; }
        public int SelectedTabIndex
        {
            get
            {
                return selectedTabIndex;
            }
            set
            {
                selectedTabIndex = value;
                Changed(nameof(SelectedTabIndex));
                Changed(nameof(SelectedTab));
            }
        }
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void Add(string path)
        {
            ComicBook book;
            try
            {
                book = ComicBookFactory.Create(path);
            }
            catch(FileFormatException e)
            {
                throw e;
            }
            Tabs.Add(book);
            if(Tabs.Count == 1)
            {
                SelectedTabIndex = 0;
            }
        }
        public void Close(int index)
        {
            Tabs[index].Close();
            Tabs.RemoveAt(index);
        }

        private int selectedTabIndex;

        private void Changed(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
