using Microsoft.Xna.Framework;

namespace Snowy_Castle
{
    class MainMenu : MenuScreen
    {
       public MainMenu() : base("Main Menu")
        {
            MenuItem playL1 = new MenuItem("Play Level 1");
            MenuItem playL2 = new MenuItem("Play Level 2");
            MenuItem exitMenuEntry = new MenuItem("Exit");

            playL1.Selected += PlayL1Selected;
            playL2.Selected += PlayL2Selected;
            exitMenuEntry.Selected += Exit;

            MenuEntries.Add(playL1);
            MenuEntries.Add(playL2);
            MenuEntries.Add(exitMenuEntry);
        }

        void PlayL1Selected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new Level1(), e.PlayerIndex);
        }

        void PlayL2Selected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new Level2(), e.PlayerIndex);
        }

        void Exit(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
