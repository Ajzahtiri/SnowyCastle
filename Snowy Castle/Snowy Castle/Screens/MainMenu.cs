using Microsoft.Xna.Framework;

namespace Snowy_Castle
{
    class MainMenu : MenuScreen
    {
       public MainMenu() : base("Main Menu")
        {
            MenuItem playGameMenuEntry = new MenuItem("Play Game");
            MenuItem exitMenuEntry = new MenuItem("Exit");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            exitMenuEntry.Selected += Exit;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new Level1(), e.PlayerIndex);
        }

        void Exit(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
