using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Kola
{
    static class ToggleListEx
    {


        public static System.Collections.IList GetList(DependencyObject obj)
        {
            return (IList)obj.GetValue(ListProperty);
        }

        public static void SetList(DependencyObject obj, IList value)
        {
            obj.SetValue(ListProperty, value);
        }

        // Using a DependencyProperty as the backing store for List.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ListProperty =
            DependencyProperty.RegisterAttached("List", typeof(IList), typeof(ToggleListEx), new PropertyMetadata(null, ListChanged));



        public static string GetElement(DependencyObject obj)
        {
            return (string)obj.GetValue(ElementProperty);
        }

        public static void SetElement(DependencyObject obj, string value)
        {
            obj.SetValue(ElementProperty, value);
        }

        // Using a DependencyProperty as the backing store for Element.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.RegisterAttached("Element", typeof(string), typeof(ToggleListEx), new PropertyMetadata("", ElementChanged));

        static private void ListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToggleButton toggle = d as ToggleButton;
            toggle.Checked -= ToggleChecked;
            toggle.Unchecked -= ToggleUnchecked;

            toggle.IsChecked = GetList(d).Contains(GetElement(d));

            toggle.Checked += ToggleChecked;
            toggle.Unchecked += ToggleUnchecked;
        }

        static private void ElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(GetList(d) != null)
            {
                ToggleButton toggle = d as ToggleButton;
                toggle.Checked -= ToggleChecked;
                toggle.Unchecked -= ToggleUnchecked;

                toggle.IsChecked = GetList(d).Contains(GetElement(d));

                toggle.Checked += ToggleChecked;
                toggle.Unchecked += ToggleUnchecked;
            }
        }

        private static void ToggleUnchecked(object sender, RoutedEventArgs e)
        {
            IList list = GetList(sender as ToggleButton);
            string Element = GetElement(sender as ToggleButton);

            while(list.Contains(Element))
            {
                list.Remove(Element);
            }
        }

        private static void ToggleChecked(object sender, RoutedEventArgs e)
        {
            GetList(sender as ToggleButton).Add(GetElement(sender as ToggleButton));
        }
    }
}
