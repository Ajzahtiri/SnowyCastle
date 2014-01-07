using Microsoft.Xna.Framework;

namespace Snowy_Castle
{
    class Victory2 : MenuScreen
    {
        public Victory2(int s, int phs) : base("Congratulations! You actually won!", 1)
        {
            int thisScore = s;
            int prevHS = phs;

            MenuItem i1 = new MenuItem("Your score: " + thisScore);
            MenuItem i3 = new MenuItem("High score: " + phs);
            MenuItem i2 = new MenuItem("Leave! (to the Main Menu)");

            i2.Selected += exit;

            MenuEntries.Add(i1);
            MenuEntries.Add(i3);
            MenuEntries.Add(i2);
        }

        void exit(object s, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new MainMenu(), e.PlayerIndex);
        }
    }
}
