using System.Windows;

namespace OthelloServer
{
    /// <summary>
    /// Window asking for player name and player type.
    /// </summary>
    public partial class SetUpGameDialog : Window
    {
        //Attributer
        private string playerType;
        private string playerName;

        //Konstruktor
        public SetUpGameDialog()
        {
            InitializeComponent();
        }

        //Metoder
        private void Button_OK(object sender, RoutedEventArgs e)
        {
            playerName = textBox.Text.Trim();
            this.Close();
        }

        private void RadioButton_Human(object sender, RoutedEventArgs e)
        {
            this.playerType = "Human";
        }

        private void RadioButton_Local(object sender, RoutedEventArgs e)
        {
            this.playerType = "Local";
        }

        private void RadioButton_Remote(object sender, RoutedEventArgs e)
        {
            this.playerType = "Remote";
        }

        public string GetPlayerType()
        {
            return playerType;
        }

        public string GetPlayerName()
        {
            return playerName;
        }
    }
}
