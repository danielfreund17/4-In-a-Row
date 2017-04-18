using System;
using System.Collections.Generic;
using System.Text;

namespace B16_Ex06_1
{
    public class Program
    {
        public static void Main()
        {

            // $G$ SFN-003 (-5) Changing the game settings should cause a new game round and not a new tournament.
            UserInterface userInterface = new UserInterface();
            userInterface.RunUserInterface();
        }
    }
}
