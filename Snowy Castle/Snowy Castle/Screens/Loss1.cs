using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowy_Castle
{
    class Loss1 : MenuScreen
    {
        public Loss1() : base("You didn't make it! Swamped by Snowballs!", 1)
        {
            MenuItem i1 = new MenuItem("Retry (from Level 1)");
            MenuItem i2 = new MenuItem("Tunnel out! (to the Main Menu)");

            i1.Selected += toLevel1;
            i2.Selected += exit;

            MenuEntries.Add(i1);
            MenuEntries.Add(i2);
        }

        void toLevel1(object s, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new Level1(), e.PlayerIndex);
        }

        void exit(object s, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new MainMenu(), e.PlayerIndex);
        }
    }
}
