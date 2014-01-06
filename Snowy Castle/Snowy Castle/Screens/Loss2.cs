using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowy_Castle
{
    class Loss2 : MenuScreen
    {
        public Loss2()
            : base("You didn't make it! Space was too great for you.", 2)
        {
            MenuItem i1 = new MenuItem("Retry (from Level 2)");
            MenuItem i2 = new MenuItem("Tunnel out! (to the Main Menu)");

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
