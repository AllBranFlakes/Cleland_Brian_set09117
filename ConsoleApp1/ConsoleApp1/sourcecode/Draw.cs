using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class Draw
    {

        public static void DrawTitle(int AItype, int Speed)
        {
            // include save game slots? show three boxes scan the csv's if the csv is empty show the box as empty otherwise show number of turns
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            string GameType = "";
            string GameSpeed = "";

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

            Console.WriteLine(@"                  ___           ___       ___           ___           ___           ___     ");
            Console.WriteLine(@"                 /\  \         /\__\     /\  \         /\  \         /\  \         /\  \    ");
            Console.WriteLine(@"                /::\  \       /:/  /    /::\  \       /::\  \       /::\  \       /::\  \   ");
            Console.WriteLine(@"               /:/\:\  \     /:/  /    /:/\:\  \     /:/\:\  \     /:/\:\  \     /:/\ \  \  ");
            Console.WriteLine(@"              /:/  \:\  \   /:/  /    /::\~\:\  \   /:/  \:\__\   /:/  \:\  \   _\:\~\ \  \ ");
            Console.WriteLine(@"             /:/__/_\:\__\ /:/__/    /:/\:\ \:\__\ /:/__/ \:|__| /:/__/ \:\__\ /\ \:\ \ \__\");
            Console.WriteLine(@"             \:\  /\ \/__/ \:\  \    \/__\:\/:/  / \:\  \ /:/  / \:\  \ /:/  / \:\ \:\ \/__/");
            Console.WriteLine(@"              \:\ \:\__\    \:\  \        \::/  /   \:\  /:/  /   \:\  /:/  /   \:\ \:\__\  ");
            Console.WriteLine(@"               \:\/:/  /     \:\  \       /:/  /     \:\/:/  /     \:\/:/  /     \:\/:/  /  ");
            Console.WriteLine(@"                \::/  /       \:\__\     /:/  /       \::/__/       \::/  /       \::/  /   ");
            Console.WriteLine(@"                 \/__/         \/__/     \/__/         ~~            \/__/         \/__/    ");
            Console.WriteLine("");
            Console.WriteLine("                                   ╔═══════════════════════════════╗");
            Console.WriteLine("                                   ║       Select Game Type:       ║");
            Console.WriteLine("                                   ╚═══════════════════════════════╝");
            Console.WriteLine("");
            Console.WriteLine("                                        1: Player Vs Player");
            Console.WriteLine("                                        2: Player Vs CPU (Red)");
            Console.WriteLine("                                        3: Player Vs CPU (Black)");
            Console.WriteLine("                                        4: CPU Vs CPU");
            Console.WriteLine("");
            Console.WriteLine("                                  Current Game Type: " + GameType);
            Console.WriteLine("");
            Console.WriteLine("                                   [L: Load game]      [N: New Game]");
            Console.WriteLine("");
            Console.WriteLine("                                              Game Speed");
            Console.WriteLine("                                 (adjust with left/right arrow keys)");
            Console.WriteLine("");
            if (Speed == 10)
            {
                GameSpeed = "Slow Potatoes";
                Console.SetCursorPosition(44, 27); Console.WriteLine(GameSpeed);
            }
            else if (Speed == 5)
            {
                GameSpeed = "Casual chips";
                Console.SetCursorPosition(45, 27); Console.WriteLine(GameSpeed);
            }
            else if (Speed == 1)
            {
                GameSpeed = "WHOAH THERE SPUDDY!";
                Console.SetCursorPosition(42, 27); Console.WriteLine(GameSpeed);
            }
            Console.WriteLine("");
            Console.WriteLine("                        - Made by B.Cleland 2017 (A Division of Aperture Science)");
        }

        public static void DrawBoard()
        {
            // draw play area

            Console.WriteLine("  ╔═══╦═══╦═══╦═══╦═══╦═══╦═══╦═══╗");
            Console.WriteLine("  ║   ║░░░║   ║░░░║   ║░░░║   ║░░░║         ╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣         ║  Grid Like Arrayed Draughts Organizing System v:1.0.1  ║");
            Console.WriteLine("  ║░░░║   ║░░░║   ║░░░║   ║░░░║   ║         ╚════════════════════════════════════════════════════════╝");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣            [Arrow Keys] Move the cursor");
            Console.WriteLine("  ║   ║░░░║   ║░░░║   ║░░░║   ║░░░║            [Space]      Select / Move");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣");
            Console.WriteLine("  ║░░░║   ║░░░║   ║░░░║   ║░░░║   ║            [U]ndo       [R]edo");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣            [S]ave       [Q]uit");
            Console.WriteLine("  ║   ║░░░║   ║░░░║   ║░░░║   ║░░░║");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣            How to play:");
            Console.WriteLine("  ║░░░║   ║░░░║   ║░░░║   ║░░░║   ║            - Select a piece.");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣            - Select the space you want to move it to.");
            Console.WriteLine("  ║   ║░░░║   ║░░░║   ║░░░║   ║░░░║            - ???");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣            - Profit!");
            Console.WriteLine("  ║░░░║   ║░░░║   ║░░░║   ║░░░║   ║");
            Console.WriteLine("  ╚═══╩═══╩═══╩═══╩═══╩═══╩═══╩═══╝");
            Console.WriteLine();


            // set player score and turn tracker
            Console.ForegroundColor = ConsoleColor.DarkRed;
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
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write("■");
                            break;
                        case 2:
                            Console.SetCursorPosition((yCount * 4) + 4, ((xCount * 2) + 1));
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("■");
                            break;
                        case 3:
                            Console.SetCursorPosition((yCount * 4) + 4, (xCount * 2) + 1);
                            Console.ForegroundColor = ConsoleColor.DarkRed;
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
        public static int GetScore1(int[,] board)
        {
            int player1score = 12;
            int adjust1score = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {

                    if (board[x, y] == 2 || board[x, y] == 4)
                    {
                        adjust1score++;
                    }

                }
            }
            player1score = player1score - adjust1score;
            return player1score;
        }

        public static int GetScore2(int[,] board)
        {
            int player2score = 12;
            int adjust2score = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {

                    if (board[x, y] == 1 || board[x, y] == 3)
                    {
                        adjust2score++;
                    }

                }
            }
            player2score = player2score - adjust2score;
            return player2score;
        }

        public static void SaveLoad()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(35, 9); Console.Write("                                  ");
            Console.SetCursorPosition(35, 10); Console.Write(" ╔═════════════════════════════╗  ");
            Console.SetCursorPosition(35, 11); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 12); Console.Write(" ║        Select a slot        ║░ ");
            Console.SetCursorPosition(35, 13); Console.Write(" ║    NumPad 1,2 or 3 choose   ║░ ");
            Console.SetCursorPosition(35, 14); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 15); Console.Write(" ║   [Slot 1]                  ║░ ");
            Console.SetCursorPosition(35, 16); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 17); Console.Write(" ║   [Slot 2]                  ║░ ");
            Console.SetCursorPosition(35, 18); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 19); Console.Write(" ║   [Slot 3]                  ║░ ");
            Console.SetCursorPosition(35, 20); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 21); Console.Write(" ╚═════════════════════════════╝░ ");
            Console.SetCursorPosition(35, 22); Console.Write("    ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░ ");
            using (StreamReader sr1 = new StreamReader(@".\\CheckersSave1.csv"))
            {
                int lineCount1 = File.ReadLines(@".\\CheckersSave1.csv").Count();
                if (lineCount1 != 0)
                {

                    Console.SetCursorPosition(51, 15); Console.Write(" - Turn " + lineCount1 + "-");
                }
                else
                {
                    Console.SetCursorPosition(51, 15); Console.Write(" - empty - ");
                }
            }
            using (StreamReader sr2 = new StreamReader(@".\\CheckersSave2.csv"))
            {
                int lineCount2 = File.ReadLines(@".\\CheckersSave2.csv").Count();
                if (lineCount2 != 0)
                {

                    Console.SetCursorPosition(51, 17); Console.Write(" - Turn " + lineCount2 + "-");
                }
                else
                {
                    Console.SetCursorPosition(51, 17); Console.Write(" - empty - ");
                }
            }
            using (StreamReader sr3 = new StreamReader(@".\\CheckersSave3.csv"))
            {
                int lineCount3 = File.ReadLines(@".\\CheckersSave3.csv").Count();
                if (lineCount3 != 0)
                {
                    Console.SetCursorPosition(51, 19); Console.Write(" - Turn " + lineCount3 + "-");
                }
                else
                {
                    Console.SetCursorPosition(51, 19); Console.Write(" - empty - ");
                }
            }
        }
        public static void LoadSuccess()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(35, 9); Console.Write("                                  ");
            Console.SetCursorPosition(35, 10); Console.Write(" ╔═════════════════════════════╗  ");
            Console.SetCursorPosition(35, 11); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 12); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 13); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 14); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 15); Console.Write(" ║      Save Game Loading      ║░ ");
            Console.SetCursorPosition(35, 16); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 17); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 18); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 19); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 20); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 21); Console.Write(" ╚═════════════════════════════╝░ ");
            Console.SetCursorPosition(35, 22); Console.Write("    ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░ ");
        }

        public static void SaveSuccess()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(35, 9); Console.Write("                                  ");
            Console.SetCursorPosition(35, 10); Console.Write(" ╔═════════════════════════════╗  ");
            Console.SetCursorPosition(35, 11); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 12); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 13); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 14); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 15); Console.Write(" ║         Game  Saved         ║░ ");
            Console.SetCursorPosition(35, 16); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 17); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 18); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 19); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 20); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 21); Console.Write(" ╚═════════════════════════════╝░ ");
            Console.SetCursorPosition(35, 22); Console.Write("    ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░ ");
        }

        public static void PlayerWin(int player)
        {
            if (player == 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.SetCursorPosition(35, 9); Console.Write("                                  ");
            Console.SetCursorPosition(35, 10); Console.Write(" ╔═════════════════════════════╗  ");
            Console.SetCursorPosition(35, 11); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 12); Console.Write(" ║     Player "+player+" Wins       ║░ ");
            Console.SetCursorPosition(35, 13); Console.Write(" ║                             ║░ ");
            Console.SetCursorPosition(35, 14); Console.Write(" ╚═════════════════════════════╝░ ");
            Console.SetCursorPosition(35, 15); Console.Write("    ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░ ");
        }

        public static void WriteScores(int player1score, int player2score, int turn)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.SetCursorPosition(15, 18);
            Console.Write(player1score);

            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(15, 19);
            Console.Write(player2score);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(15, 20);
            Console.Write(turn);

            /* Player Turn Indicator */
            if ((turn % 2) != 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.SetCursorPosition(1, 18);
                Console.Write(" ->");
                Console.SetCursorPosition(1, 19);
                Console.Write("   ");
            }
            else if ((turn % 2) == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.SetCursorPosition(1, 18);
                Console.Write("   ");
                Console.SetCursorPosition(1, 19);
                Console.Write(" ->");
            }
        }
    }
}

