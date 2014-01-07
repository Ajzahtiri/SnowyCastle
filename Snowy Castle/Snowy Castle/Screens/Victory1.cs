using Microsoft.Xna.Framework;

namespace Snowy_Castle
{
    class Victory1 : MenuScreen
    {
        public Victory1() : base("Congratulations! You survived! But then...", 1)
        {
            MenuItem i1 = new MenuItem("Proceed to Level 2");
            MenuItem i2 = new MenuItem("Flee! (to the Main Menu)");

            i1.Selected += toLevel2;
            i2.Selected += exit;

            MenuEntries.Add(i1);
            MenuEntries.Add(i2);
        }

        void toLevel2(object s, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new Level2(), e.PlayerIndex);
        }

        void exit(object s, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new MainMenu(), e.PlayerIndex);
        }
    }
}
