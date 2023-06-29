using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace SpamBotRemaster.Infrastructure.CustomElements
{
    internal class TextBoxWithPlaceholder : TextBox
    {
        public static readonly DependencyProperty PlaceholderVisibilityProperty;
        public static readonly DependencyProperty PlaceholderTextProperty;
        public static readonly DependencyProperty PlaceholderColorProperty;
        public static readonly DependencyProperty TextColorProperty;

        

        public Visibility PlaceholderVisibility
        {
            get => (Visibility)GetValue(PlaceholderVisibilityProperty);
            set => SetValue(PlaceholderVisibilityProperty, value);
        }

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        public Brush PlaceholderColor
        {
            get => (Brush)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        public Brush TextColor
        {
            get => (Brush)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        static TextBoxWithPlaceholder()
        {
            PlaceholderVisibilityProperty = DependencyProperty.Register("PlaceholderVisibility", typeof(Visibility), typeof(TextBoxWithPlaceholder),
                new FrameworkPropertyMetadata(Visibility.Visible){BindsTwoWayByDefault=true});

            PlaceholderTextProperty = DependencyProperty.Register("PlaceholderText", typeof(string), typeof(TextBoxWithPlaceholder),
                new FrameworkPropertyMetadata(""));

            PlaceholderColorProperty = DependencyProperty.Register("PlaceholderColor", typeof(Brush), typeof(TextBoxWithPlaceholder),
                new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 109, 109, 109))));

            TextColorProperty = DependencyProperty.Register("TextColor", typeof(Brush), typeof(TextBoxWithPlaceholder),
                new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 109, 109, 109))));
        }


        public TextBoxWithPlaceholder()
        {
            base.LostFocus += TryToPastePlaceholder;
            base.GotFocus += TryToRemovePlaceholder;
            base.Loaded += TryToPastePlaceholder;
        }

        private void TryToPastePlaceholder(object sender, RoutedEventArgs e)
        {
            if (base.Text == null || base.Text.Replace(" ", "").Length == 0)
            {
                PlaceholderVisibility = Visibility.Visible;
            }

            
        }


        private void TryToRemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (PlaceholderVisibility == Visibility.Visible)
            {
                PlaceholderVisibility = Visibility.Collapsed;
            }
        }
    }
}
