using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/*
TO-DO
Version 0.8.4

-needs AI
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
            Console.SetWindowSize(110, 35);

            // define the board structure (default value for a given board space is 0 - empty)
            /*
             In order to properly map the game pieces a state for each 'square' on the board is defined as:
                0 - empty
                1 - occupied (Red)
                2 - occupied (Black)
                3 - occupied (Red King)
                4 - occupied (Black King)
             */

            int[,] board =  { { 0, 1, 0, 1, 0, 1, 0, 1 },
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
            //move list
            Dictionary<int, int[,]> moveList = new Dictionary<int, int[,]>();

            int[,] redoMove = new int[8, 8];

            moveList.Add(1, (int[,])board.Clone());
            //statelist
            int[] states = new int[3];
            int[] redoStates = new int[3];

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

                    if (key == ConsoleKey.S)
                    {
                        using (StreamWriter outputFile = new StreamWriter(@"C:\Users\Home\Documents\CheckersSave.csv"))
                        {
                            foreach (KeyValuePair<int, int[,]> pair in moveList)
                            {
                                outputFile.WriteLine(pair.Key + "," + player1score + "," + player2score + "," + String.Join(",",pair.Value.Cast<int>())); 
                            }
                        }
                    }

                    if (key == ConsoleKey.U)
                    {
                        foreach (KeyValuePair<int, int[,]> pair in moveList)
                        {
                            if (moveList.ContainsKey(turn - 1) == true)
                            {
                                redoMove = board;
                                redoStates[0] = turn;
                                redoStates[1] = player1score;
                                redoStates[2] = player2score;
                                board = (int[,])moveList[states[0]].Clone();
                                turn = states[0];
                                player1score = states[1];
                                player2score = states[2];
                            }
                        }
                        DrawPieces(board);
                        WriteScores(player1score, player2score, turn);
                    }

                    if (key == ConsoleKey.R)
                    {
                        board = redoMove;
                        turn = redoStates[0];
                        player1score = redoStates[1];
                        player2score = redoStates[2];
                        DrawPieces(board);
                        WriteScores(player1score, player2score, turn);
                    }


                    // Player interaction code
                    if (key == ConsoleKey.Spacebar)
                    {
                        states[0] = turn;
                        states[1] = player1score;
                        states[2] = player2score;

                        int boardPiece = board[boardRow, boardColumn];

                        //pick
                        if (holding == 0)
                        {
                            if ((turn % 2 == 0) && (boardPiece % 2 == 0))
                            {
                                // set origin Black
                                hop = true;// required to stop game turn updating on keypress
                                origXY[0] = boardRow;
                                origXY[1] = boardColumn;
                                holding = boardPiece;
                                board[boardRow, boardColumn] = 0;
                                DrawPieces(board);
                            }
                            else
                            if ((turn % 2 != 0) && (boardPiece % 2 != 0))
                            {
                                //set origin Red
                                hop = true;
                                origXY[0] = boardRow;
                                origXY[1] = boardColumn;
                                holding = boardPiece;
                                board[boardRow, boardColumn] = 0;
                                DrawPieces(board);
                            }
                        }
                        //place
                        else
                        if (holding != 0)
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
                                        hop = false;
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
                                        hop = false;
                                    }
                                }
                            }
                            // taking a piece
                            else
                            if (ValidMove(holding, boardPiece, origXY, destXY, turn) == true
                                    && boardPiece != 0)
                            {
                                int spaceX = (destXY[0] + (destXY[0] - origXY[0]));
                                int spaceY = (destXY[1] + (destXY[1] - origXY[1]));

                                /* stops spaceX & spaceY from running of the ends of the array */
                                if (spaceX < 0 || spaceX > 7)
                                {
                                    spaceX = destXY[0];
                                }
                                if (spaceY < 0 || spaceY > 7)
                                {
                                    spaceY = destXY[1];
                                }

                                if (board[spaceX, spaceY] == 0)
                                {

                                    if ((holding == 1 && spaceX == 7) || (holding == 2 && spaceX == 0))
                                    {

                                        if (holding == 1)
                                        {
                                            player1score++;
                                        }
                                        else
                                        if (holding == 2)
                                        {
                                            player2score++;
                                        }

                                        holding = holding + 2;
                                        board[spaceX, spaceY] = holding;
                                        board[boardRow, boardColumn] = 0;
                                        hop = false;
                                        board[origXY[0], origXY[1]] = 0;
                                        origXY[0] = spaceX;
                                        origXY[1] = spaceY;

                                        DrawPieces(board);

                                    }
                                    else
                                    {
                                        int adderA = 0;
                                        int adderB = 0;
                                        board[origXY[0], origXY[1]] = 0;
                                        board[spaceX, spaceY] = holding;
                                        board[boardRow, boardColumn] = 0;
                                        switch (holding)
                                        {
                                            case 1:
                                                adderA = 1;
                                                adderB = 1;
                                                player1score++;
                                                break;
                                            case 2:
                                                adderA = -1;
                                                adderB = -1;
                                                player2score++;
                                                break;
                                            case 3:
                                                player1score++;
                                                adderA = 1;
                                                adderB = -1;
                                                break;
                                            case 4:
                                                player2score++;
                                                adderA = -1;
                                                adderB = 1;
                                                break;
                                        }

                                        if ((spaceY == 0 || spaceY == 1) && spaceX >= 2 && spaceX <= 5)
                                        {
                                            if (((board[spaceX + adderA, spaceY + 1] != 0 && board[spaceX + adderA, spaceY + 1] % 2 != holding % 2) && board[spaceX + (adderA + adderA), spaceY + 2] == 0)
                                             || ((board[spaceX + adderB, spaceY + 1] != 0 && board[spaceX + adderB, spaceY + 1] % 2 != holding % 2) && board[spaceX + (adderB + adderB), spaceY + 2] == 0))
                                            {
                                                origXY[0] = spaceX;
                                                origXY[1] = spaceY;

                                                hop = true;
                                            }
                                            else
                                            {
                                                hop = false;
                                                holding = 0;
                                            }
                                        }
                                        else
                                        if ((spaceY == 6 || spaceY == 7) && spaceX >= 2 && spaceX <= 5)
                                        {
                                            if (board[spaceX + adderA, spaceY - 1] != 0 && board[spaceX + adderA, spaceY - 1] % 2 != holding % 2 && board[spaceX + (adderA + adderA), spaceY - 2] == 0
                                             || board[spaceX + adderB, spaceY - 1] != 0 && board[spaceX + adderB, spaceY - 1] % 2 != holding % 2 && board[spaceX + (adderB + adderB), spaceY - 2] == 0)
                                            {
                                                origXY[0] = spaceX;
                                                origXY[1] = spaceY;

                                                hop = true;
                                            }
                                            else
                                            {
                                                hop = false;
                                                holding = 0;
                                            }
                                        }
                                        else
                                        if ((spaceY == 2 || spaceY == 3 || spaceY == 4 || spaceY == 5) && spaceX >= 2 && spaceX <= 5)
                                        {
                                            if ((board[spaceX + adderA, spaceY - 1] != 0 && board[spaceX + adderA, spaceY - 1] % 2 != holding % 2 && board[spaceX + (adderA + adderA), spaceY - 2] == 0)
                                             || (board[spaceX + adderB, spaceY - 1] != 0 && board[spaceX + adderB, spaceY - 1] % 2 != holding % 2 && board[spaceX + (adderB + adderB), spaceY - 2] == 0)
                                             || (board[spaceX + adderA, spaceY + 1] != 0 && board[spaceX + adderA, spaceY + 1] % 2 != holding % 2 && board[spaceX + (adderA + adderA), spaceY + 2] == 0)
                                             || (board[spaceX + adderB, spaceY + 1] != 0 && board[spaceX + adderB, spaceY + 1] % 2 != holding % 2 && board[spaceX + (adderB + adderB), spaceY + 2] == 0))
                                            {
                                                origXY[0] = spaceX;
                                                origXY[1] = spaceY;

                                                hop = true;
                                            }
                                            else
                                            {
                                                hop = false;
                                                holding = 0;
                                            }
                                        }

                                        // hop checks for board extremes
                                        else
                                        if (spaceX == 0 || spaceX == 1)
                                        {
                                            if (spaceY == 0 || spaceY == 1)
                                            {
                                                if (board[spaceX + 1, spaceY + 1] != 0 && board[spaceX + 1, spaceY + 1] % 2 != holding % 2 && board[spaceX + 2, spaceY + 2] == 0)
                                                {
                                                    origXY[0] = spaceX;
                                                    origXY[1] = spaceY;

                                                    hop = true;
                                                }
                                                else
                                                {
                                                    hop = false;
                                                    holding = 0;
                                                }
                                            }
                                            else
                                            if (spaceY == 6 || spaceY == 7)
                                            {
                                                if (board[spaceX + 1, spaceY - 1] != 0 && board[spaceX + 1, spaceY - 1] % 2 != holding % 2 && board[spaceX + 2, spaceY - 2] == 0)
                                                {
                                                    origXY[0] = spaceX;
                                                    origXY[1] = spaceY;

                                                    hop = true;
                                                }
                                                else
                                                {
                                                    hop = false;
                                                    holding = 0;
                                                }
                                            }
                                            else
                                            {
                                                hop = false;
                                                holding = 0;
                                            }
                                        }
                                        else
                                        if (spaceX == 6 || spaceX == 7)
                                        {
                                            if (spaceY == 0 || spaceY == 1)
                                            {
                                                if (board[spaceX - 1, spaceY + 1] != 0 && board[spaceX - 1, spaceY + 1] % 2 != holding % 2 && board[spaceX - 2, spaceY + 2] == 0)
                                                {
                                                    origXY[0] = spaceX;
                                                    origXY[1] = spaceY;

                                                    hop = true;
                                                }
                                                else
                                                {
                                                    hop = false;
                                                    holding = 0;
                                                }
                                            }
                                            else
                                            if (spaceY == 6 || spaceY == 7)
                                            {
                                                if (board[spaceX - 1, spaceY - 1] != 0 && board[spaceX - 1, spaceY - 1] % 2 != holding % 2 && board[spaceX - 2, spaceY - 2] == 0)
                                                {
                                                    origXY[0] = spaceX;
                                                    origXY[1] = spaceY;

                                                    hop = true;
                                                }
                                                else
                                                {
                                                    hop = false;
                                                    holding = 0;
                                                }
                                            }
                                            else
                                            {
                                                hop = false;
                                                holding = 0;
                                            }
                                        }
                                        else
                                        {
                                            hop = false;
                                            holding = 0;
                                        }
                                        DrawPieces(board);
                                    }
                                }
                            }

                            if (hop == false)
                            {
                                holding = 0;
                                turn++;

                                if (moveList.ContainsKey(turn) == true)
                                {
                                    moveList[turn] = (int[,])board.Clone();
                                }
                                else
                                {
                                    moveList.Add(turn, board);
                                }
                            }
                        }

                        WriteScores(player1score, player2score, turn);
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

        static void WriteScores(int player1score, int player2score, int turn)
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

        static void Thinking(int wait)
        {
            System.Threading.Thread.Sleep(wait * 1000);
        }

        static bool ValidMove(int holding, int boardPiece, int[] origXY, int[] destXY, int turn)
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


        static void DrawBoard()
        {
            // draw play area

            Console.WriteLine("  ╔═══╦═══╦═══╦═══╦═══╦═══╦═══╦═══╗");
            Console.WriteLine("  ║   ║░░░║   ║░░░║   ║░░░║   ║░░░║         ╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣         ║  Grid Like Arrayed Draughts Organizing System v:0.8.4  ║");
            Console.WriteLine("  ║░░░║   ║░░░║   ║░░░║   ║░░░║   ║         ╚════════════════════════════════════════════════════════╝");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣            - Move the cursor with the arrow keys.");
            Console.WriteLine("  ║   ║░░░║   ║░░░║   ║░░░║   ║░░░║            - Press space to select/move.");
            Console.WriteLine("  ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣            - Press U to undo your move");
            Console.WriteLine("  ║░░░║   ║░░░║   ║░░░║   ║░░░║   ║            - Press R to redo your move");
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








