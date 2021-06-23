using System.Windows;

namespace OthelloServer
{
    /// <summary>
    /// Window showing which player won the game
    /// </summary>
    public partial class WinnerDialog : Window
    {
        public WinnerDialog(string winnerName)
        {
            InitializeComponent();
            Winner.Text = $"{winnerName} WON";
        }

        private void Button_Ok(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}