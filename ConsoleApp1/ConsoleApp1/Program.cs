using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
TO-DO
refactor code and create some seperate classes for common tasks inc.:
- Redrawing the board
*/


namespace ConsoleApplication1
{
    class Program
    {

        //public variables
        public bool boolPickPlace = false;


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

            int[,] board = { { 0, 1, 0, 3, 0, 4, 0, 1 },
                             { 1, 0, 1, 0, 1, 0, 1, 0 },
                             { 0, 1, 0, 1, 0, 1, 0, 1 },
                             { 0, 0, 0, 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0, 0, 0, 0 },
                             { 2, 0, 2, 0, 2, 0, 2, 0 },
                             { 0, 2, 0, 2, 0, 2, 0, 2 },
                             { 2, 0, 4, 0, 3, 0, 2, 0 } };

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
            int originX = 0;
            int originY = 0;
            //List<int> masterMove = new List<int>();
            //List<int> moveList1 = new List<int>();
            //List<int> moveList2 = new List<int>();
            //to add an array to a list use
            // moveList.AddRange(arrayName);(

            // player score variables
            int player1score = 0;
            int player2score = 0;

            // draw the board
            drawBoard();
     
            // draw the pieces
            drawPieces(board);
           
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
                    /*
                    //End of test code*/


                    // Player interaction code
                    if (key == ConsoleKey.Spacebar)
                    {
                        int boardPiece = board[boardRow,boardColumn];
                        if (boolPickPlace == false && ((((boardRow + 1) % 2 != 0) && ((boardColumn + 1) % 2 == 0)) || (((boardRow + 1) % 2 == 0) && ((boardColumn + 1) % 2 != 0))))
                        {
                            pickPlace(boardPiece, x, y, ref boolPickPlace);

                        }


                        // When spacebar is pressed check the board array item at the representative cursor location 
                        switch (board[boardRow, boardColumn])
                        {
                            case 0:
                                // If holding is not null while interacting with a blank square
                                // determine if the square is valid (if the row is even the column must be odd and vice versa)
                                // then put down held piece


                                if (holding != 0 && ((((boardRow + 1) % 2 != 0) && ((boardColumn + 1) % 2 == 0)) || (((boardRow + 1) % 2 == 0) && ((boardColumn + 1) % 2 != 0))))
                                {
                                      Console.SetCursorPosition(x, y);
                                        if (holding == 1)
                                        {

                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.Write("■");
                                            board[boardRow, boardColumn] = holding;
                                        }
                                        else if (holding == 2)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.Write("■");
                                            board[boardRow, boardColumn] = holding;
                                        }
                                        else if (holding == 3)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.Write("K");
                                            board[boardRow, boardColumn] = holding;
                                        }
                                        else if (holding == 4)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.Write("K");
                                            board[boardRow, boardColumn] = holding;
                                        }
                                        holding = 0;

                                    }
                                
                                break;

                            // If holding is not null while interacting with a Red Piece
                            case 1:
                                // determine if the square is valid (if the row is even the column must be odd and vice versa)

                                if (holding != 0 && ((((boardRow + 1) % 2 != 0) && ((boardColumn + 1) % 2 == 0)) || (((boardRow + 1) % 2 == 0) && ((boardColumn + 1) % 2 != 0))))
                                
                                    {
                                        Console.SetCursorPosition(x, y);
                                        // if holding a red piece already take no action
                                        if (holding == 1)
                                        {
                                            holding = 1;
                                        }
                                        // if holding a black piece
                                        else if (holding == 2)
                                        {
                                            // determines if the selected space is at the boundries of the board
                                            // if not proceed with the move
                                            if ((boardRow != 7 && boardColumn != 0) && (boardRow != 7 && boardColumn != 7) && (boardRow != 0 && boardColumn != 0) && (boardRow != 0 && boardColumn != 7))
                                            {
                                                // check for an empty space beyond the piece you wish to take (-1 row,-1 column)
                                                if (board[boardRow - 1, boardColumn - 1] == 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("░");
                                                    board[boardRow, boardColumn] = 0;
                                                    board[boardRow - 1, boardColumn - 1] = 2;
                                                    Console.SetCursorPosition(x - 4, y - 2);
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("■");
                                                    // having succesfully taken the piece drop the piece
                                                    holding = 0;
                                                }

                                                // determines if the selected space is at the boundries of the board
                                                // if not proceed with the move
                                                else if (board[boardRow - 1, boardColumn + 1] == 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("░");
                                                    board[boardRow, boardColumn] = 0;
                                                    board[boardRow - 1, boardColumn + 1] = 2;
                                                    Console.SetCursorPosition(x + 4, y - 2);
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("■");
                                                    // having succesfully taken the piece drop the piece
                                                    holding = 0;
                                                }
                                            }
                                            else
                                            {
                                                holding = 2;
                                            }
                                        }
                                        // if holding a red piece already take no action
                                        else if (holding == 3)
                                        {
                                            holding = 3;
                                        }
                                        // if holding a black king
                                        else if (holding == 4)
                                        {
                                            // determines if the selected space is at the boundries of the board
                                            // if not proceed with the move
                                            if ((boardRow != 7 && boardColumn != 0) && (boardRow != 7 && boardColumn != 7) && (boardRow != 0 && boardColumn != 0) && (boardRow != 0 && boardColumn != 7))
                                            {
                                                // check for an empty space beyond the piece you wish to take (-1 row,-1 column)
                                                if (board[boardRow - 1, boardColumn - 1] == 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("░");
                                                    board[boardRow, boardColumn] = 0;
                                                    board[boardRow - 1, boardColumn - 1] = 4;
                                                    Console.SetCursorPosition(x - 4, y - 2);
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("K");
                                                    // having succesfully taken the piece drop the piece
                                                    holding = 0;
                                                }

                                                // determines if the selected space is at the boundries of the board
                                                // if not proceed with the move
                                                else if (board[boardRow - 1, boardColumn + 1] == 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("░");
                                                    board[boardRow, boardColumn] = 0;
                                                    board[boardRow - 1, boardColumn + 1] = 4;
                                                    Console.SetCursorPosition(x + 4, y - 2);
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("K");
                                                    // having succesfully taken the piece drop the piece
                                                    holding = 0;
                                                }
                                            }
                                            else
                                            {
                                                holding = 4;
                                            }
                                        }
                                    }
                                
                                else
                                {
                                    Console.SetCursorPosition(x, y);
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.Write("░");
                                    board[boardRow, boardColumn] = 0;
                                    holding = 1;
                                    originX = boardRow;
                                    originY = boardColumn;
                                }
                                
                                break;

                            // If you are holding any piece and interact with a black piece
                            case 2:
                              
                                    // determine if the square is valid (if the row is even the column must be odd and vice versa)
                                    if (holding != 0 && ((((boardRow + 1) % 2 != 0) && ((boardColumn + 1) % 2 == 0)) || (((boardRow + 1) % 2 == 0) && ((boardColumn + 1) % 2 != 0))))

                                    {
                                        Console.SetCursorPosition(x, y);
                                        if (holding == 1)
                                        {
                                            // determines if the selected space is at the boundries of the board
                                            // if not proceed with the move
                                            if ((boardRow != 7 && boardColumn != 0) && (boardRow != 7 && boardColumn != 7))
                                            {
                                                // check for an empty space beyond the piece you wish to take (-1 row,-1 column)
                                                if (board[boardRow + 1, boardColumn - 1] == 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("░");
                                                    board[boardRow, boardColumn] = 0;
                                                    board[boardRow + 1, boardColumn - 1] = 1;
                                                    Console.SetCursorPosition(x - 4, y + 2);
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.Write("■");
                                                    // having succesfully taken the piece drop the piece
                                                    holding = 0;
                                                }

                                                // determines if the selected space is at the boundries of the board
                                                // if not proceed with the move
                                                else if (board[boardRow + 1, boardColumn + 1] == 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("░");
                                                    board[boardRow, boardColumn] = 0;
                                                    board[boardRow + 1, boardColumn + 1] = 1;
                                                    Console.SetCursorPosition(x + 4, y + 2);
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.Write("■");
                                                    // having succesfully taken the piece drop the piece
                                                    holding = 0;
                                                }
                                            }
                                            else
                                            {
                                                holding = 1;
                                            }
                                        }
                                        else if (holding == 2)
                                        {
                                            holding = 2;
                                        }
                                        else if (holding == 3)
                                        {
                                            // determines if the selected space is at the boundries of the board
                                            // if not proceed with the move
                                            if ((boardRow != 7 && boardColumn != 0) && (boardRow != 7 && boardColumn != 7))
                                            {
                                                // check for an empty space beyond the piece you wish to take (-1 row,-1 column)
                                                if (board[boardRow + 1, boardColumn - 1] == 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("░");
                                                    board[boardRow, boardColumn] = 0;
                                                    board[boardRow + 1, boardColumn - 1] = 3;
                                                    Console.SetCursorPosition(x - 4, y + 2);
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.Write("K");
                                                    // having succesfully taken the piece drop the piece
                                                    holding = 0;
                                                }

                                                // determines if the selected space is at the boundries of the board
                                                // if not proceed with the move
                                                else if (board[boardRow + 1, boardColumn + 1] == 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("░");
                                                    board[boardRow, boardColumn] = 0;
                                                    board[boardRow + 1, boardColumn + 1] = 3;
                                                    Console.SetCursorPosition(x + 4, y + 2);
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.Write("K");
                                                    // having succesfully taken the piece drop the piece
                                                    holding = 0;
                                                }
                                            }
                                            else
                                            {
                                                holding = 3;
                                            }
                                        }
                                        else if (holding == 4)
                                        {
                                            holding = 4;
                                        }
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(x, y);
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.Write("░");
                                        board[boardRow, boardColumn] = 0;
                                        holding = 2;
                                        originX = boardRow;
                                        originY = boardColumn;
                                    }
                                
                                break;

                            // If you are holding any piece and you interact with a Red King
                            case 3:
                                
                                    // determine if the square is valid (if the row is even the column must be odd and vice versa)
                                    if (holding != 0 && ((((boardRow + 1) % 2 != 0) && ((boardColumn + 1) % 2 == 0)) || (((boardRow + 1) % 2 == 0) && ((boardColumn + 1) % 2 != 0))))

                                    {
                                        Console.SetCursorPosition(x, y);
                                        // if holding a red piece already take no action
                                        if (holding == 1)
                                        {
                                            holding = 1;
                                        }
                                        // if holding a black piece
                                        else if (holding == 2)
                                        {
                                            // determines if the selected space is at the boundries of the board
                                            // if not proceed with the move
                                            if (boardRow != 0 && boardColumn != 0)
                                            {
                                                // check for an empty space beyond the piece you wish to take (-1 row,-1 column)
                                                if (board[boardRow - 1, boardColumn - 1] == 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("░");
                                                    board[boardRow, boardColumn] = 0;
                                                    board[boardRow - 1, boardColumn - 1] = 2;
                                                    Console.SetCursorPosition(x - 4, y - 2);
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("■");
                                                    // having succesfully taken the piece drop the piece
                                                    holding = 0;
                                                }
                                            }
                                            else if (boardRow != 0 && boardColumn != 7)
                                            {
                                                // determines if the selected space is at the boundries of the board
                                                // if not proceed with the move
                                                if (board[boardRow - 1, boardColumn + 1] == 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("░");
                                                    board[boardRow, boardColumn] = 0;
                                                    board[boardRow - 1, boardColumn + 1] = 2;
                                                    Console.SetCursorPosition(x + 4, y - 2);
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("■");
                                                    // having succesfully taken the piece drop the piece
                                                    holding = 0;
                                                }
                                            }
                                            else
                                            {
                                                holding = 2;
                                            }
                                        }
                                        // if holding a red piece already take no action
                                        else if (holding == 3)
                                        {
                                            holding = 3;
                                        }
                                        // if holding a black king
                                        else if (holding == 4)
                                        {
                                            // determines if the selected space is at the boundries of the board
                                            // if not proceed with the move
                                            if (boardRow != 0 && boardColumn != 0)
                                            {
                                                // check for an empty space beyond the piece you wish to take (-1 row,-1 column)
                                                if (board[boardRow - 1, boardColumn - 1] == 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("░");
                                                    board[boardRow, boardColumn] = 0;
                                                    board[boardRow - 1, boardColumn - 1] = 4;
                                                    Console.SetCursorPosition(x - 4, y - 2);
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("K");
                                                    // having succesfully taken the piece drop the piece
                                                    holding = 0;
                                                }
                                            }
                                            else if (boardRow != 0 && boardColumn != 7)
                                            {
                                                // determines if the selected space is at the boundries of the board
                                                // if not proceed with the move
                                                if (board[boardRow - 1, boardColumn + 1] == 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("░");
                                                    board[boardRow, boardColumn] = 0;
                                                    board[boardRow - 1, boardColumn + 1] = 4;
                                                    Console.SetCursorPosition(x + 4, y - 2);
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("K");
                                                    // having succesfully taken the piece drop the piece
                                                    holding = 0;
                                                }
                                            }
                                            else
                                            {
                                                holding = 4;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(x, y);
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.Write("░");
                                        board[boardRow, boardColumn] = 0;
                                        holding = 3;
                                        originX = boardRow;
                                        originY = boardColumn;
                                    }
                                
                                break;

                            // If you are holding any piece and you interact with a Black King
                            case 4:
                                
                                    // determine if the square is valid (if the row is even the column must be odd and vice versa)
                                    if (holding != 0 && ((((boardRow + 1) % 2 != 0) && ((boardColumn + 1) % 2 == 0)) || (((boardRow + 1) % 2 == 0) && ((boardColumn + 1) % 2 != 0))))

                                    {
                                        Console.SetCursorPosition(x, y);
                                        if (holding == 1)
                                        {
                                            // determines if the selected space is at the boundries of the board
                                            // if not proceed with the move
                                            if ((boardRow != 7 && boardColumn != 0) && (boardRow != 7 && boardColumn != 7))
                                            {
                                                // check for an empty space beyond the piece you wish to take (-1 row,-1 column)
                                                if (board[boardRow + 1, boardColumn - 1] == 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("░");
                                                    board[boardRow, boardColumn] = 0;
                                                    board[boardRow + 1, boardColumn - 1] = 1;
                                                    Console.SetCursorPosition(x - 4, y + 2);
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.Write("■");
                                                    // having succesfully taken the piece drop the piece
                                                    holding = 0;
                                                }

                                                // determines if the selected space is at the boundries of the board
                                                // if not proceed with the move
                                                if (board[boardRow + 1, boardColumn + 1] == 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("░");
                                                    board[boardRow, boardColumn] = 0;
                                                    board[boardRow + 1, boardColumn + 1] = 1;
                                                    Console.SetCursorPosition(x + 4, y + 2);
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.Write("■");
                                                    // having succesfully taken the piece drop the piece
                                                    holding = 0;
                                                }
                                            }
                                            else
                                            {
                                                holding = 1;
                                            }
                                        }
                                        else if (holding == 2)
                                        {
                                            holding = 2;
                                        }
                                        else if (holding == 3)
                                        {
                                            // determines if the selected space is at the boundries of the board
                                            // if not proceed with the move
                                            if ((boardRow != 7 && boardColumn != 0) && (boardRow != 7 && boardColumn != 7))
                                            {
                                                // check for an empty space beyond the piece you wish to take (-1 row,-1 column)
                                                if (board[boardRow + 1, boardColumn - 1] == 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("░");
                                                    board[boardRow, boardColumn] = 0;
                                                    board[boardRow + 1, boardColumn - 1] = 3;
                                                    Console.SetCursorPosition(x - 4, y + 2);
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.Write("K");
                                                    // having succesfully taken the piece drop the piece
                                                    holding = 0;
                                                }

                                                // determines if the selected space is at the boundries of the board
                                                // if not proceed with the move
                                                if (board[boardRow + 1, boardColumn + 1] == 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Black;
                                                    Console.Write("░");
                                                    board[boardRow, boardColumn] = 0;
                                                    board[boardRow + 1, boardColumn + 1] = 3;
                                                    Console.SetCursorPosition(x + 4, y + 2);
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.Write("K");
                                                    // having succesfully taken the piece drop the piece
                                                    holding = 0;
                                                }
                                            }
                                            else
                                            {
                                                holding = 3;
                                            }
                                        }
                                        else if (holding == 4)
                                        {
                                            holding = 4;
                                        }
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(x, y);
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.Write("░");
                                        board[boardRow, boardColumn] = 0;
                                        holding = 4;
                                        originX = boardRow;
                                        originY = boardColumn;
                                    }
                                
                                break;
                            default:
                                break;
                        }

                    }


                    // writing player1 scores
                    // Console.SetCursorPosition(13, 18);
                    // Console.Write(player1score);

                    // writing player2 scores
                    // Console.SetCursorPosition(13, 19);
                    // Console.Write(player1score);

                    // two second pause timer
                    // System.Threading.Thread.Sleep(2000);

                    // Change player indicator (to player two swap the console write for the cursor and blank space or swap the cursor values)
                    // Console.ForegroundColor = ConsoleColor.Green;
                    // Console.SetCursorPosition(0, 18);
                    // Console.Write("   ");
                    // Console.SetCursorPosition(0, 19);
                    // Console.Write("-->");



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


        static void pickPlace(int boardPiece,int x,int y, bool holding)
        {
            if (holding == true)
            {
                if (boardPiece == 0)
                {
                    //record destination
                    //compare destination and origin
                    //is move valid? validMove(orig,dest,x)
                    //if yes do move
                    //else do nothing
                   

                }
                else if (boardPiece != 0)
                {
                    //record destination
                    //determine boardpiece at destination
                    //if boardpiece same as held piece do nothing
                    //else determine if "take space" is available
                    //if yes do take
                    //if no do nothing
                }
            }
            else if (holding == false)
            {
                if (boardPiece != 0)
                {
                    //pick up
                    //record starting position
                    holding = true;
                }
                else
                {
                    //do nothing
                }
            }
           
        }


        static void validMove(int[,] orig, int[,] dest, int x)
        {
            //switch/case with x as trigger?
            //case (x%2==0) would it determine black pieces?
            //if x>=3 it's a king and can move in x+/-
            //  break;
            //case (x%2!=0) would it determine red pieces?
            //if x>=3 it's a king and can move in x+/-
            //  break;
        }


        static void drawBoard()
        {
            // draw play area

            Console.WriteLine("  ╔═══╦═══╦═══╦═══╦═══╦═══╦═══╦═══╗");
            Console.WriteLine("  ║   ║░░░║   ║░░░║   ║░░░║   ║░░░║         ╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣         ║   Grid Like Arrayed Draughts Organizing System v:0.5   ║");
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


        static void drawPieces(int[,] arr)
        {
            for (int xCount = 0; xCount < 8; xCount++)
            {
                for (int yCount = 0; yCount < 8; yCount++)
                {
                    switch (arr[xCount, yCount])
                    {
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
