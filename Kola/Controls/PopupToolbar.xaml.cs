using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

namespace Kola.Controls
{
    public class PopupContentCollection : List<PopupContentControl> { }
    public class PopupContentControl : ContentControl
    {

        /// <summary>
        /// Tooltip to be set to associated button.
        /// </summary>
        public string TooltipText
        {
            get { return (string)GetValue(TooltipTextProperty); }
            set { SetValue(TooltipTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Tooltip.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TooltipTextProperty =
            DependencyProperty.Register("TooltipText", typeof(string), typeof(PopupContentControl), new PropertyMetadata(""));


        /// <summary>
        /// Icon of button, as string value with character from FontAwesome.
        /// </summary>
        public string ButtonIcon
        {
            get { return (string)GetValue(ButtonIconProperty); }
            set { SetValue(ButtonIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonIconProperty =
            DependencyProperty.Register("ButtonIcon", typeof(string), typeof(PopupContentControl));



        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(PopupContentControl), new PropertyMetadata(false));


    }

    /// <summary>
    /// Interaction logic for PopupToolbar.xaml
    /// </summary>
    public partial class PopupToolbar : UserControl, INotifyPropertyChanged
    {


        public IEnumerable<PopupContentControl> Popups
        {
            get { return (IEnumerable<PopupContentControl>)GetValue(PopupsProperty); }
            set { SetValue(PopupsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Popups.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupsProperty =
            DependencyProperty.Register("Popups", typeof(IEnumerable<PopupContentControl>), typeof(PopupToolbar), new PropertyMetadata(new PopupContentControl[0], OnPopupsChange));

        private static void OnPopupsChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PopupToolbar toolbar = d as PopupToolbar;
            toolbar.buttonContainer.Children.Clear();
            DataTemplate template = toolbar.FindResource("ButtonTemplate") as DataTemplate;
            IEnumerable<PopupContentControl> contents = e.NewValue as IEnumerable<PopupContentControl>;
            foreach(PopupContentControl content in contents)
            {
                ContentPresenter presenter = new ContentPresenter();
                presenter.ContentTemplate = template;
                presenter.Content = content;
                toolbar.buttonContainer.Children.Add(presenter);
            }
        }

        public PopupContentControl CheckedContent => Popups.FirstOrDefault(t => t.IsChecked);

        public event PropertyChangedEventHandler PropertyChanged;

        public PopupToolbar()
        {
            InitializeComponent();
        }

        public void SetChecked(PopupContentControl control)
        {
            foreach (var c in Popups)
            {
                c.IsChecked = c == control;
            }
        }

        public void SetChecked(int index)
        {
            int i = 0;
            foreach(var c in Popups)
            {
                c.IsChecked = i == index;
                i++;
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            PopupContentControl content = (sender as FrameworkElement).DataContext as PopupContentControl;
            SetChecked(content);
            Changed(nameof(CheckedContent));
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Changed(nameof(CheckedContent));
        }

        private void Changed(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
