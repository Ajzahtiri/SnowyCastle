using Microsoft.Xna.Framework;

namespace Snowy_Castle
{
    class PauseMenu : menuScreen
    {
        public PauseMenu() : base("Paused")
        {
            menuItem resume = new menuItem("Resume Game");
            menuItem quit = new menuItem("Quit Game");

            resume.chosen += OnCancel;
            quit.chosen += quitSelected;

            MenuEntries.Add(resume);
            MenuEntries.Add(quit);
        }

        void quitSelected(object sender, playerEvent e)
        {
            SManager.AddScreen(new backgroundScreen(), thisPlayer);
            SManager.AddScreen(new MainMenu(), thisPlayer);
        }
    }
}
