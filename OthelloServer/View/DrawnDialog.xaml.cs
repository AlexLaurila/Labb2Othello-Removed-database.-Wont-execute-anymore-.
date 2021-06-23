using System.Windows;

namespace OthelloServer
{
    /// <summary>
    /// Window showing players if the game ended in a draw.
    /// </summary>
    public partial class DrawnDialog : Window
    {
        public DrawnDialog()
        {
            InitializeComponent();
        }

        private void Button_Ok(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
