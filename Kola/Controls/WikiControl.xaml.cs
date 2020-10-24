using CefSharp;
using Kola.Helpers;
using Kola.Helpers.Wiki;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Kola.Controls
{
    /// <summary>
    /// Interaction logic for WikiControl.xaml
    /// </summary>
    public partial class WikiControl : UserControl, INotifyPropertyChanged
    {
        readonly TimeSpan DelayTime;
        public ObservableCollection<WikiPage> Pages { get; set; }
        public string Lang
        {
            get => lang;
            set
            {
                lang = value;
                Timer_Tick(this, EventArgs.Empty);
                Changed(nameof(Lang));
            }
        }

        DispatcherTimer timer;
        string lang = Wiki.Langs.First();

        public event PropertyChangedEventHandler PropertyChanged;

        public WikiControl()
        {
            DelayTime = TimeSpan.FromSeconds(0.3);
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = DelayTime;
            
            Pages = new ObservableCollection<WikiPage>();
            DataContext = this;
            InitializeComponent();
        }

        
        //TODO: When using this event, deletion doesn't work. 
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                Keyboard.ClearFocus();

            timer.Stop();
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            string text = textBox.Text;
            Task.Run(() =>
            {
                var result = Wiki.Search(text, Lang).Result;
                Dispatcher.Invoke(() =>
                {
                    Pages.Clear();
                    foreach (WikiPage page in result)
                    {
                        Pages.Add(page);
                    }
                    Console.WriteLine($"Count: {Pages.Count}");
                });
            });
        }

        private void listBox_Selected(object sender, RoutedEventArgs e)
        {
            WikiPage page = listBox.SelectedItem as WikiPage;
            if(page != null)
            {
                //Loads blank page to avoid white rectangle while loading
                browser.LoadHtml(HtmlHelper.GetHtml());
                Task.Run(() =>
                {
                    var content = page.GetContent().Result;
                    Dispatcher.Invoke(() => browser.LoadHtml(content, "http://wiki/" + page.ID));
                });
                //e.Handled = true;
            }
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;
        }

        private void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
