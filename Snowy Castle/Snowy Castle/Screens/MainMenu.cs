using Microsoft.Xna.Framework;

namespace Snowy_Castle
{
    class MainMenu : menuScreen
    {
       public MainMenu() : base("Main Menu")
        {
            menuItem playL1 = new menuItem("Play Level 1");
            menuItem playL2 = new menuItem("Play Level 2");
            menuItem exitMenuEntry = new menuItem("Exit");

            playL1.chosen += PlayL1Selected;
            playL2.chosen += PlayL2Selected;
            exitMenuEntry.chosen += Exit;

            MenuEntries.Add(playL1);
            MenuEntries.Add(playL2);
            MenuEntries.Add(exitMenuEntry);
        }

        void PlayL1Selected(object sender, playerEvent e)
        {
            SManager.AddScreen(new Level1(), e.pIndex);
        }

        void PlayL2Selected(object sender, playerEvent e)
        {
            SManager.AddScreen(new Level2(), e.pIndex);
        }

        void Exit(object sender, playerEvent e)
        {
            SManager.Game.Exit();
        }
    }
}
