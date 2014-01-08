using Microsoft.Xna.Framework;

namespace Snowy_Castle
{
    class Victory2 : menuScreen
    {
        public Victory2(int s) : base("You actually won! Wow!")
        {
            int thisScore = s;

            menuItem i1 = new menuItem("Your score: " + thisScore);
            menuItem i2 = new menuItem("Leave! (to the Main Menu)");

            i2.chosen += exit;

            MenuEntries.Add(i1);
            MenuEntries.Add(i2);
        }

        void exit(object s, playerEvent e)
        {
            SManager.AddScreen(new MainMenu(), e.pIndex);
        }
    }
}
