using System.Windows;

namespace OthelloServer
{
    /// <summary>
    /// This applications starting point
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            GameManager main = new GameManager();
        }
    }
}
