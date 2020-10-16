using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                if(selectedTabIndex >= 0 && selectedTabIndex < Tabs.Count)
                {
                    Tabs[selectedTabIndex].LostFocus();
                    Tabs[selectedTabIndex].IsSelected = false;
                }
                
                selectedTabIndex = value;

                if (selectedTabIndex >= 0 && selectedTabIndex < Tabs.Count)
                {
                    Tabs[selectedTabIndex].GainFocus();
                    Tabs[selectedTabIndex].IsSelected = true;
                }
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
            catch(FileNotFoundException e)
            {
                MessageBox.Show("No file", String.Format("There is no file {0}", e.FileName), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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
            ComicBook tab = Tabs[index];
            tab.Close();
            //TODO: Make sure it works properly.
            App.Current?.Dispatcher.Invoke(() => Tabs.RemoveAt(index));
            //Tabs.RemoveAt(index);
            if(index < SelectedTabIndex)
            {
                SelectedTabIndex--;
            }
            if(index == SelectedTabIndex)
            {
                tab.LostFocus();

                //Trigger INotifyPropertyChanged
                SelectedTabIndex = SelectedTabIndex;
            }
        }

        public void Close(ComicBook book)
        {
            int index = Tabs.IndexOf(book);
            Close(index);
        }

        private int selectedTabIndex;

        private void Changed(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
