using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
TO-DO
Version 0.7.2

-needs AI
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
            Console.Title = "G.L.A.D.O.S.";
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.SetWindowSize(110, 30);

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

            /* define variables for cursor movement */
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
            bool hop = false;
            /* Various Variables */
            // int for held pieces
            int holding = 0;
            // int for turn
            int turn = 1;
            // variables for move list (used for undo/redo and comparison of origin square versus destination square)
            int[] origXY = { 0, 0 };
            int[] destXY = { 0, 0 };

            // player score variables
            int player1score = 0;
            int player2score = 0;

            // Start the game
            DrawBoard();
            DrawPieces(board);
            bool play = true;
            Console.SetCursorPosition(4, 1);
            while (play == true)
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

                    /* this code allows you to see background stuff (for testing purposes only need to remve in final release) */
                    if (key == ConsoleKey.Enter)
                    {
                        Console.SetCursorPosition(24, 25);
                        Console.Write("turn: ");
                        Console.SetCursorPosition(31, 25);
                        Console.Write(turn);

                        Console.SetCursorPosition(24, 26);
                        Console.Write("holding: ");
                        Console.SetCursorPosition(31, 26);
                        Console.Write(holding);

                        Console.SetCursorPosition(24, 27);
                        Console.Write("dest x: ");
                        Console.SetCursorPosition(31, 27);
                        Console.Write(destXY[0] + 1);

                        Console.SetCursorPosition(24, 28);
                        Console.Write("dest y: ");
                        Console.SetCursorPosition(31, 28);
                        Console.Write(destXY[1] + 1);

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

                        Console.SetCursorPosition(15, 20);

                        Console.Write(player1score);
                        Console.Write(player2score);
                        Console.SetCursorPosition(x, y);

                    }

                    // Player interaction code
                    if (key == ConsoleKey.Spacebar)
                    {
                        int boardPiece = board[boardRow, boardColumn];

                        if (holding == 0)
                        {
                            if ((turn % 2 == 0) && (boardPiece % 2 == 0))
                            {
                                // set origin Black
                                origXY[0] = boardRow;
                                origXY[1] = boardColumn;
                                holding = boardPiece;
                                board[boardRow, boardColumn] = 0;
                                DrawPieces(board);
                            }
                            else if ((turn % 2 != 0) && (boardPiece % 2 != 0))
                            {
                                //set origin Red
                                origXY[0] = boardRow;
                                origXY[1] = boardColumn;
                                holding = boardPiece;
                                board[boardRow, boardColumn] = 0;
                                DrawPieces(board);
                            }
                        }
                        else if (holding != 0)
                        {
                            //set the destination
                            destXY[0] = boardRow;
                            destXY[1] = boardColumn;

                            // placing the piece
                            if ((ValidMove(holding, boardPiece, origXY, destXY, turn) == true) && board[boardRow, boardColumn] == 0)
                            {
                                // king move
                                if ((holding == 1 && boardRow == 7) || (holding == 2 && boardRow == 0))
                                {
                                    board[boardRow, boardColumn] = holding + 2;
                                    holding = 0;
                                    DrawPieces(board);
                                    if (origXY[0] != destXY[0])
                                    {
                                        turn++;
                                    }
                                }
                                // standard move
                                else
                                {
                                    board[boardRow, boardColumn] = holding;
                                    holding = 0;
                                    DrawPieces(board);
                                    if (origXY[0] != destXY[0])
                                    {
                                        turn++;
                                    }
                                }
                            }
                            // taking a piece
                            else if (ValidMove(holding, boardPiece, origXY, destXY, turn) == true
                                && board[boardRow, boardColumn] != 0
                                && board[boardRow, boardColumn] != holding
                                && board[boardRow, boardColumn] != (holding + 2)
                                && board[boardRow, boardColumn] != (holding - 2))
                            {
                                int spaceX = (destXY[0] + (destXY[0] - origXY[0]));
                                int spaceY = (destXY[1] + (destXY[1] - origXY[1]));

                                /* stops spaceX & spaceY from running of the ends of the array */
                                if (spaceX < 0 || spaceY < 0 || spaceX > 7 || spaceY > 7)
                                {
                                    spaceX = destXY[0];
                                    spaceY = destXY[1];
                                }

                                if (board[spaceX, spaceY] == 0)
                                {

                                    if ((holding == 1 && spaceX == 7) || (holding == 2 && spaceX == 0))
                                    {
                                        board[spaceX, spaceY] = holding + 2;
                                        board[boardRow, boardColumn] = 0;
                                        if (hop == true)
                                        {
                                            board[origXY[0], origXY[1]] = 0;
                                        }
                                        holding = 0;
                                        DrawPieces(board);
                                        if (origXY[0] != destXY[0])
                                        {
                                            turn++;
                                        }
                                    }
                                    else
                                    {
                                        board[spaceX, spaceY] = holding;
                                        board[boardRow, boardColumn] = 0;

                                        if (holding == 1 || holding == 3)
                                        {

                                            player1score++;
                                            try
                                            {
                                                if ((board[spaceX + 1, spaceY + 1] == 2 && board[spaceX + 2, spaceY + 2] == 0)
                                                     || (board[spaceX + 1, spaceY - 1] == 2 && board[spaceX + 2, spaceY - 2] == 0))
                                                {
                                                    origXY[0] = spaceX;
                                                    origXY[1] = spaceY;
                                                    hop = true;
                                                    turn--;
                                                }
                                                else
                                                {
                                                    holding = 0;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                holding = 0;
                                                hop = true;
                                            }

                                        }
                                        else if (holding == 2 || holding == 4)
                                        {
                                            player2score++;
                                            try
                                            {
                                                if ((board[spaceX - 1, spaceY + 1] == 1 && board[spaceX - 2, spaceY + 2] == 0)
                                                 || (board[spaceX - 1, spaceY - 1] == 1 && board[spaceX - 2, spaceY - 2] == 0))
                                                {
                                                    origXY[0] = spaceX;
                                                    origXY[1] = spaceY;
                                                    hop = true;
                                                    turn--;
                                                }
                                                else
                                                {
                                                    holding = 0;
                                                    hop = true;
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                holding = 0;
                                            }

                                        }
                                        if (origXY[0] != destXY[0])
                                        {
                                            turn++;
                                        }
                                        DrawPieces(board);
                                    }
                                }
                            }
                        }
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(15, 18);
                        Console.Write(player1score);

                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.SetCursorPosition(15, 19);
                        Console.Write(player2score);
                    }

                    /* Move Mechanics */
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
                    /* Win Conditions */
                    if (player1score == 12)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(47, 13);

                        Console.Write("Player 1 Wins!");
                        // 5 second pause timer
                        Thinking(5);
                        play = false;
                    }
                    if (player2score == 12)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.SetCursorPosition(47, 13);

                        Console.Write("Player 2 Wins!");
                        // 5 second pause timer
                        Thinking(5);
                        play = false;
                    }

                    Console.SetCursorPosition(x, y);
                }
            }
        }



        static void Thinking(int wait)
        {
            System.Threading.Thread.Sleep(wait * 1000);
        }

        static bool ValidMove(int holding, int boardPiece, int[] origXY, int[] destXY, int turn)
        {
            bool isValid = false;
            if (holding == 1 && boardPiece != holding)
            {
                if (((origXY[0] + 1) == destXY[0] && ((origXY[1] - 1) == destXY[1] || (origXY[1] + 1) == destXY[1])) || (origXY[0] == destXY[0] && origXY[1] == destXY[1]))
                {
                    isValid = true;
                }
            }
            else if (holding == 2 && boardPiece != holding)
            {
                if (((origXY[0] - 1) == destXY[0] && ((origXY[1] - 1) == destXY[1] || (origXY[1] + 1) == destXY[1])) || (origXY[0] == destXY[0] && origXY[1] == destXY[1]))
                {
                    isValid = true;
                }
            }
            else if (boardPiece != holding || boardPiece != holding - 2)
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
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣         ║  Grid Like Arrayed Draughts Organizing System v:0.7.2  ║");
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
            Console.SetCursorPosition(1, 18);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(" ->");
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








