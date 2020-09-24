using System;
using System.Collections.Generic;
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
    /// <summary>
    /// A control which displays an arrow and fades in and out on mouse hover.
    /// </summary>
    public partial class PageChanger : UserControl
    {
        public enum Direction { Left, Right}
        public PageChanger()
        {
            InitializeComponent();
        }

        public Direction ArrowDirection
        {
            get => (Direction)GetValue(ArrowDirectionProperty);
            set => SetValue(ArrowDirectionProperty, value);
        }

        public static readonly DependencyProperty ArrowDirectionProperty =
            DependencyProperty.Register(nameof(ArrowDirection), typeof(Direction), typeof(PageChanger), new PropertyMetadata(Direction.Left));
    }
}
