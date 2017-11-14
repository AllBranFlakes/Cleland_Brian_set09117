using System;

namespace ConsoleApp1
{
    class Draw
    {

        public static void DrawTitle(int AItype)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            string GameType = "";

            if (AItype == 0)
            {
                GameType = "Player Vs Player";
            }
            else if (AItype == 1)
            {
                GameType = "Player Vs CPU (Red)";
            }
            else if (AItype == 2)
            {
                GameType = "Player Vs CPU (Black)";
            }
            else if (AItype == 3)
            {
                GameType = "CPU Vs CPU";
            }


            Console.WriteLine(@"            ________      ___           ________      ________      ________      ________      ");
            Console.WriteLine(@"           |\   ____\    |\  \         |\   __  \    |\   ___ \    |\   __  \    |\   ____\     ");
            Console.WriteLine(@"           \ \  \___|    \ \  \        \ \  \|\  \   \ \  \_|\ \   \ \  \|\  \   \ \  \___|_    ");
            Console.WriteLine(@"            \ \  \  ___   \ \  \        \ \   __  \   \ \  \ \\ \   \ \  \\\  \   \ \_____  \   ");
            Console.WriteLine(@"             \ \  \|\  \ __\ \  \____  __\ \  \ \  \ __\ \  \_\\ \ __\ \  \\\  \ __\|____|\  \  ");
            Console.WriteLine(@"              \ \_______\\__\ \_______\\__\ \__\ \__\\__\ \_______\\__\ \_______\\__\____\_\  \ ");
            Console.WriteLine(@"               \|_______\|__|\|_______\|__|\|__|\|__\|__|\|_______\|__|\|_______\|__|\_________\");
            Console.WriteLine(@"                                                                                    \|_________|");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("                   Select Game Type:");
            Console.WriteLine("                     1: Player Vs Player");
            Console.WriteLine("                     2: Player Vs CPU (Red)");
            Console.WriteLine("                     3: Player Vs CPU (Black)");
            Console.WriteLine("                     4: CPU Vs CPU");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("                   Current Game Type: " + GameType);
            Console.WriteLine("");
            Console.WriteLine("                   L: Load previous game");
            Console.WriteLine("                   S: Start Game");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("                                         - Made by B.Cleland 2017 (A division of Aperture Science)");
            Console.WriteLine();
        }
        public static void DrawBoard()
        {
            // draw play area

            Console.WriteLine("  ╔═══╦═══╦═══╦═══╦═══╦═══╦═══╦═══╗");
            Console.WriteLine("  ║   ║░░░║   ║░░░║   ║░░░║   ║░░░║         ╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣         ║  Grid Like Arrayed Draughts Organizing System v:0.9.0  ║");
            Console.WriteLine("  ║░░░║   ║░░░║   ║░░░║   ║░░░║   ║         ╚════════════════════════════════════════════════════════╝");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣            - Move the cursor with the arrow keys.");
            Console.WriteLine("  ║   ║░░░║   ║░░░║   ║░░░║   ║░░░║            - Press space to select/move.");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣            - Press U to undo or R to redo your move");
            Console.WriteLine("  ║░░░║   ║░░░║   ║░░░║   ║░░░║   ║            - Press S to save or L to load");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣");
            Console.WriteLine("  ║   ║░░░║   ║░░░║   ║░░░║   ║░░░║");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣            - Select a piece.");
            Console.WriteLine("  ║░░░║   ║░░░║   ║░░░║   ║░░░║   ║            - Select the space you want to move it to.");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣            - ???");
            Console.WriteLine("  ║   ║░░░║   ║░░░║   ║░░░║   ║░░░║            - Profit!");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣");
            Console.WriteLine("  ║░░░║   ║░░░║   ║░░░║   ║░░░║   ║");
            Console.WriteLine("  ╚═══╩═══╩═══╩═══╩═══╩═══╩═══╩═══╝            - Made by Brian 'BranFlakes' Cleland 2017");
            Console.WriteLine();


            // set player score and turn tracker
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("     Player 1: 0");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("     Player 2: 0");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("     Turn    : 1");
            Console.SetCursorPosition(1, 18);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(" ->");
        }

        public static void DrawPieces(int[,] arr)
        {
            for (int xCount = 0; xCount < 8; xCount++)
            {
                for (int yCount = 0; yCount < 8; yCount++)
                {
                    switch (arr[xCount, yCount])
                    {
                        case 0:
                            if ((((xCount + 1) % 2 != 0) && ((yCount + 1) % 2 == 0)) || (((xCount + 1) % 2 == 0) && ((yCount + 1) % 2 != 0)))
                            {
                                Console.SetCursorPosition((yCount * 4) + 4, (xCount * 2) + 1);
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write("░");
                            }
                            break;
                        case 1:
                            Console.SetCursorPosition((yCount * 4) + 4, (xCount * 2) + 1);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("■");
                            break;
                        case 2:
                            Console.SetCursorPosition((yCount * 4) + 4, ((xCount * 2) + 1));
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("■");
                            break;
                        case 3:
                            Console.SetCursorPosition((yCount * 4) + 4, (xCount * 2) + 1);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("K");
                            break;
                        case 4:
                            Console.SetCursorPosition((yCount * 4) + 4, (xCount * 2) + 1);
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("K");
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public static void WriteScores(int player1score, int player2score, int turn)
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(15, 18);
            Console.Write(player1score);

            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(15, 19);
            Console.Write(player2score);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(15, 20);
            Console.Write(turn);
        }
    }
}
