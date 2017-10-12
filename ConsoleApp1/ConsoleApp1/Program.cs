using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
TO-DO
Version 0.6.1

-needs turn counter implementation
-needs AI
-needs win condition implementation
-needs list for moves
-needs undo/redo function
-needs save game function
-needs load game function
*/


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set title and console window display 
            Console.Title = "Checkers";
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.SetWindowSize(110, 30);


            // Set variables for game


            // define the board structure (default value for a given board space is 0 - empty)
            /*
             In order to properly map the game pieces a state for each 'square' on the board is defined as:
                0 - empty
                1 - occupied (Red)
                2 - occupied (Black)
                3 - occupied (Red King)
                4 - occupied (Black King)
             */

            int[,] board = { { 0, 1, 0, 1, 0, 1, 0, 1 },
                             { 1, 0, 1, 0, 1, 0, 1, 0 },
                             { 0, 1, 0, 1, 0, 1, 0, 1 },
                             { 0, 0, 0, 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0, 0, 0, 0 },
                             { 2, 0, 2, 0, 2, 0, 2, 0 },
                             { 0, 2, 0, 2, 0, 2, 0, 2 },
                             { 2, 0, 2, 0, 2, 0, 2, 0 } };

            // define variables for cursor movement
            // define start position (x offset by +3 & y offset by +1 to account for the board layout)
            int x = 4;
            int y = 1;

            // variables for moving through the board
            int moveX = 0;
            int moveY = 0;

            // define variables for locating pieces in board array (remember to adjust offsets so you dont fly off the end of the array!!)
            int boardColumn = 0;
            int boardRow = 0;

            // variables used to define the offsets that need to be adjusted as per previous comments
            int boardArrayX = 0;
            int boardArrayY = 0;

            // int for held pieces
            int holding = 0;

            // variables for move list (used for undo/redo and comparison of origin square versus destination square)
            int[] origXY = { 0, 0 };
            int[] destXY = { 0, 0 };
            //List<int> masterMove = new List<int>();
            //List<int> moveList1 = new List<int>();
            //List<int> moveList2 = new List<int>();
            //to add an array to a list use
            // moveList.AddRange(arrayName);(

            // player score variables
            int player1score = 0;
            int player2score = 0;

            // draw the board
            DrawBoard();

            // draw the pieces
            DrawPieces(board);

            // play the game
            Console.SetCursorPosition(4, 1);

            while (player1score + player2score < 16)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo info = Console.ReadKey(true);
                    ConsoleKey key = info.Key;

                    if (key == ConsoleKey.UpArrow)
                    {
                        moveX = 0; moveY = -2;
                        boardArrayX = 0; boardArrayY = -1;
                    }
                    if (key == ConsoleKey.DownArrow)
                    {
                        moveX = 0; moveY = 2;
                        boardArrayX = 0; boardArrayY = 1;
                    }
                    if (key == ConsoleKey.RightArrow)
                    {
                        moveX = 4; moveY = 0;
                        boardArrayX = 1; boardArrayY = 0;
                    }
                    if (key == ConsoleKey.LeftArrow)
                    {
                        moveX = -4; moveY = 0;
                        boardArrayX = -1; boardArrayY = 0;
                    }

                    /* this code allows you to see the values stored in all spaces of the array
                     * this portion of code can be uncommented for testing purposes
                     */
                    if (key == ConsoleKey.Enter)
                    {
                        for (int xCount2 = 0; xCount2 < 8; xCount2++)
                        {

                            for (int yCount2 = 0; yCount2 < 8; yCount2++)
                            {
                                switch (board[xCount2, yCount2])
                                {
                                    case 0:
                                        Console.SetCursorPosition(yCount2 + 2, xCount2 + 22);
                                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                                        Console.Write("0");
                                        break;
                                    case 1:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write("1");
                                        break;
                                    case 2:
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.Write("2");
                                        break;
                                    case 3:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write("3");
                                        break;
                                    case 4:
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.Write("4");
                                        break;
                                    default:
                                        break;
                                }
                            }

                        }


                        Console.SetCursorPosition(x, y);

                    }

                    // Player interaction code
                    if (key == ConsoleKey.Spacebar)
                    {
                        int boardPiece = board[boardRow, boardColumn];

                        if (holding == 0)
                        {
                            origXY[0] = boardRow;
                            origXY[1] = boardColumn;
                            holding = boardPiece;
                            board[boardRow, boardColumn] = 0;
                            DrawPieces(board);
                        }
                        else
                        {   
                            destXY[0] = boardRow;
                            destXY[1] = boardColumn;
                            if ((ValidMove(holding, boardPiece, origXY, destXY) == true) && board[boardRow,boardColumn] == 0)
                            {
                                if((holding == 1 && boardRow == 7)||(holding == 2 && boardRow == 0))
                                {
                                    board[boardRow, boardColumn] = holding+2;
                                    holding = 0;
                                    DrawPieces(board);
                                }
                                else
                                {
                                    board[boardRow, boardColumn] = holding;
                                    holding = 0;
                                    DrawPieces(board);
                                }
                                
                            }
                            else if ((ValidMove(holding, boardPiece, origXY, destXY) == true) && board[boardRow, boardColumn] != 0 && ((board[boardRow,boardColumn] != holding) || board[boardRow, boardColumn] != (holding-2)))
                            {
                                int spaceX = (destXY[0] + (destXY[0] - origXY[0]));
                                int spaceY = (destXY[1] + (destXY[1] - origXY[1]));

                                if (board[spaceX,spaceY] == 0)
                                {
                                    board[spaceX, spaceY] = holding;
                                    board[boardRow, boardColumn] = 0;
                                    holding = 0;
                                    DrawPieces(board);
                                }
                                
                            }
                        }
                    }


                    //take input from move and adjust cursor position for output and array position for board
                    if (moveX != 0 || moveY != 0)
                    {
                        x = x + moveX;
                        y = y + moveY;
                        boardColumn = boardColumn + boardArrayX;
                        boardRow = boardRow + boardArrayY;
                    }
                    // ensure the cursor cannot move outwith the bounds of the board
                    if (x < 3 || x > 33 || y < 0 || y > 15)
                    {
                        x = x - moveX;
                        y = y - moveY;
                    }
                    // ensure the pieces dont move out of the board array
                    if (boardColumn < 0 || boardRow < 0 || boardColumn > 7 || boardRow > 7)
                    {
                        boardColumn = boardColumn - boardArrayX;
                        boardRow = boardRow - boardArrayY;
                    }
                    Console.SetCursorPosition(x, y);
                }
            }

            // win conditions
            if (player1score > player2score)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(0, 13);

                Console.Write("Player 1 Wins!");
            }
            if (player2score > player1score)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(0, 13);

                Console.Write("Player 2 Wins!");
            }
            if (player1score == player2score)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(0, 13);

                Console.Write("It's a draw.");
            }
        }

        static bool ValidMove(int holding, int boardPiece, int[] origXY, int[] destXY)
        {
            bool isValid = false;
            if (holding == 1)
            {
                if (((origXY[0] + 1) == destXY[0] && ((origXY[1] - 1) == destXY[1] || (origXY[1] + 1) == destXY[1])) || (origXY[0] == destXY[0] && origXY[1] == destXY[1]))
                {
                    isValid = true;
                }
            }
            else if (holding == 2)
            {
                if (((origXY[0] - 1) == destXY[0] && ((origXY[1] - 1) == destXY[1] || (origXY[1] + 1) == destXY[1])) || (origXY[0] == destXY[0] && origXY[1] == destXY[1]))
                {
                    isValid = true;
                }
            }
            else
            {
                if ((((origXY[0] - 1) == destXY[0]) || ((origXY[0] + 1) == destXY[0])) && (((origXY[1] - 1) == destXY[1]) || ((origXY[1] + 1) == destXY[1])) || ((origXY[0] == destXY[0]) && (origXY[1] == destXY[1]))) 
                {
                    isValid = true;
                }
            }

            return isValid;
        }


        static void DrawBoard()
        {
            // draw play area

            Console.WriteLine("  ╔═══╦═══╦═══╦═══╦═══╦═══╦═══╦═══╗");
            Console.WriteLine("  ║   ║░░░║   ║░░░║   ║░░░║   ║░░░║         ╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣         ║  Grid Like Arrayed Draughts Organizing System v:0.6.1  ║");
            Console.WriteLine("  ║░░░║   ║░░░║   ║░░░║   ║░░░║   ║         ╚════════════════════════════════════════════════════════╝");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣            - Move the cursor with the arrow keys.");
            Console.WriteLine("  ║   ║░░░║   ║░░░║   ║░░░║   ║░░░║            - Press space to select/move.");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣");
            Console.WriteLine("  ║░░░║   ║░░░║   ║░░░║   ║░░░║   ║            - Select a piece.");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣            - Select the space you want to move it to.");
            Console.WriteLine("  ║   ║░░░║   ║░░░║   ║░░░║   ║░░░║            - ???");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣            - Profit!");
            Console.WriteLine("  ║░░░║   ║░░░║   ║░░░║   ║░░░║   ║");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣");
            Console.WriteLine("  ║   ║░░░║   ║░░░║   ║░░░║   ║░░░║");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣");
            Console.WriteLine("  ║░░░║   ║░░░║   ║░░░║   ║░░░║   ║");
            Console.WriteLine("  ╚═══╩═══╩═══╩═══╩═══╩═══╩═══╩═══╝            - Made by Brian 'BranFlakes' Cleland 2017");
            Console.WriteLine();


            // set player score and turn tracker
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("     Player 1: 0");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("     player 2: 0");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(0, 18);
            Console.Write(" -->");
        }


        static void DrawPieces(int[,] arr)
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
    }
}