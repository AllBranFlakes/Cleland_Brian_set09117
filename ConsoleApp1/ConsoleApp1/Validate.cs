using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Validate
    {
        public static bool ValidMove(int holding, int boardPiece, int[] origXY, int[] destXY, int turn)
        {
            bool isValid = false;
            if (holding == 1 && boardPiece != holding)
            {
                if (((origXY[0] + 1) == destXY[0] && ((origXY[1] - 1) == destXY[1] || (origXY[1] + 1) == destXY[1]))
                    || (origXY[0] == destXY[0] && origXY[1] == destXY[1]))
                {
                    isValid = true;
                }
            }
            else
            if (holding == 2 && boardPiece != holding)
            {
                if (((origXY[0] - 1) == destXY[0] && ((origXY[1] - 1) == destXY[1] || (origXY[1] + 1) == destXY[1]))
                    || (origXY[0] == destXY[0] && origXY[1] == destXY[1]))
                {
                    isValid = true;
                }
            }
            else
            if ((holding == 3 || holding == 4) && boardPiece != holding && boardPiece != holding - 2)
            {
                if ((((origXY[0] - 1) == destXY[0]) || ((origXY[0] + 1) == destXY[0]))
                    && (((origXY[1] - 1) == destXY[1]) || ((origXY[1] + 1) == destXY[1]))
                    || ((origXY[0] == destXY[0]) && (origXY[1] == destXY[1])))
                {
                    isValid = true;
                }
            }

            return isValid;
        }
        public static bool WinChecks(bool play, int[,] gameBoard, int player1score, int player2score)
        {
            if (player2score == 12)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(35, 10); Console.Write("                                  ");
                Console.SetCursorPosition(35, 11); Console.Write(" ╔═════════════════════════════╗  ");
                Console.SetCursorPosition(35, 12); Console.Write(" ║                             ║  ");
                Console.SetCursorPosition(35, 13); Console.Write(" ║        Player 2 Wins        ║░ ");
                Console.SetCursorPosition(35, 14); Console.Write(" ║                             ║░ ");
                Console.SetCursorPosition(35, 15); Console.Write(" ╚═════════════════════════════╝░ ");
                Console.SetCursorPosition(35, 16); Console.Write("    ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░ ");
            }
            if (player1score == 12)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition(35, 10); Console.Write("                                  ");
                Console.SetCursorPosition(35, 11); Console.Write(" ╔═════════════════════════════╗  ");
                Console.SetCursorPosition(35, 12); Console.Write(" ║                             ║  ");
                Console.SetCursorPosition(35, 13); Console.Write(" ║        Player 1 Wins        ║░ ");
                Console.SetCursorPosition(35, 14); Console.Write(" ║                             ║░ ");
                Console.SetCursorPosition(35, 15); Console.Write(" ╚═════════════════════════════╝░ ");
                Console.SetCursorPosition(35, 16); Console.Write("    ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░ ");
            }
            Draw.DrawPieces(gameBoard);
            // 5 second pause timer
            AI.Thinking(50);
            play = false;
            return play;
        }
    }
}
