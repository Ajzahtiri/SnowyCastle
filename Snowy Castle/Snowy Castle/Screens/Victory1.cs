using Microsoft.Xna.Framework;

namespace Snowy_Castle
{
    class Victory1 : menuScreen
    {
        public Victory1() : base("You win! But then...")
        {
            menuItem i1 = new menuItem("Proceed to Level 2");
            menuItem i2 = new menuItem("Flee! (to the Main Menu)");

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
