using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowy_Castle
{
    class Loss1 : menuScreen
    {
        public Loss1() : base("You lose!")
        {
            menuItem i1 = new menuItem("Retry (from Level 1)");
            menuItem i2 = new menuItem("Tunnel out! (to the Main Menu)");

            i1.chosen += toLevel1;
            i2.chosen += exit;

            MenuEntries.Add(i1);
            MenuEntries.Add(i2);
        }

        void toLevel1(object s, playerEvent e)
        {
            SManager.AddScreen(new Level1(), e.pIndex);
        }

        void exit(object s, playerEvent e)
        {
            SManager.AddScreen(new MainMenu(), e.pIndex);
        }
    }
}
