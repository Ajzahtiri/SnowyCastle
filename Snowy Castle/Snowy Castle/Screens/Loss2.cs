using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowy_Castle
{
    class Loss2 : menuScreen
    {
        public Loss2() : base("You died!")
        {
            menuItem i1 = new menuItem("Retry (from Level 2)");
            menuItem i2 = new menuItem("Tunnel out! (to the Main Menu)");

            i1.chosen += toLevel2;
            i2.chosen += exit;

            MenuEntries.Add(i1);
            MenuEntries.Add(i2);
        }

        void toLevel2(object s, playerEvent e)
        {
            SManager.AddScreen(new Level2(), e.pIndex);
        }

        void exit(object s, playerEvent e)
        {
            SManager.AddScreen(new MainMenu(), e.pIndex);
        }
    }
}
