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
    public class Model : INotifyPropertyChanged
    {
        public Model()
        {
            Tabs = new ObservableCollection<ComicBook>();

            //Tabs don't get notified of collection changing because of converter. It musst be notified manually.
            Tabs.CollectionChanged += (s, e) => Changed(nameof(Tabs));
            selectedTabIndex = -1;
        }

        ~Model()
        {
            while(Tabs.Count > 0)
            {
                Close(Tabs.Count - 1);
            }
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
                Tabs.ElementAtOrDefault(selectedTabIndex)?.LostFocus();
                selectedTabIndex = value;
                Tabs.ElementAtOrDefault(selectedTabIndex)?.GainFocus();
                Changed(nameof(SelectedTabIndex));
                Changed(nameof(SelectedTab));
            }
        }
        public ComicBook SelectedTab
        {
            get
            {
                return Tabs.ElementAtOrDefault(SelectedTabIndex);
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

        public void Add(string[] paths)
        {
            foreach(string path in paths)
            {
                Add(path);
            }
        }
        public void Close(int index)
        {
            Tabs[index].LostFocus();
            Tabs[index].Close();
            Tabs.RemoveAt(index);
            if(index < SelectedTabIndex)
            {
                SelectedTabIndex--;
            }
            if(index == SelectedTabIndex)
            {
                //Trigger INotifyPropertyChanged
                SelectedTabIndex = SelectedTabIndex;
            }
        }

        private int selectedTabIndex;

        private void Changed(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
