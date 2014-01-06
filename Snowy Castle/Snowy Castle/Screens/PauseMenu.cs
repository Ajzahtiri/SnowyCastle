using Microsoft.Xna.Framework;

namespace Snowy_Castle
{
    class PauseMenu : MenuScreen
    {
        public PauseMenu() : base("Paused", 1)
        {
            MenuItem resumeGameMenuEntry = new MenuItem("Resume Game");
            MenuItem quitGameMenuEntry = new MenuItem("Quit Game");

            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new BackgroundScreen(), ControllingPlayer);
            ScreenManager.AddScreen(new MainMenu(), ControllingPlayer);
        }
    }
}
