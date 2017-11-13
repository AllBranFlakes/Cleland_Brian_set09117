using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class Game
    {
        public static void MainGame(int AIPlayer, int[,] board, Dictionary<int, int[,]> moveList)
        {
            //set title and console window display 
            Console.Title = "G.L.A.D.O.S.";
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.SetWindowSize(110, 35);

            Sound.Play(2);

            //variables for moving through the board
            int moveX = 0;
            int moveY = 0;
            int x = 4;
            int y = 1;

            //define variables for locating pieces in board array (and offsets so you dont fly off the end of the array!!)
            int boardColumn = 0;
            int boardRow = 0;
            int boardArrayX = 0;
            int boardArrayY = 0;

            //set the hop boolean
            bool hop = false;

            //int for game states
            int holding = 0;
            int turn = 1;
            int player1score = 0;
            int player2score = 0;
            int[] states = new int[3];

            //variables for move list (used for undo/redo and comparison of origin square versus destination square)
            int[] origXY = { 0, 0 };
            int[] destXY = { 0, 0 };

            //redo variables
            int[,] redoMove = new int[8, 8];
            int[] redoStates = new int[3];

            //set move list
            moveList.Add(turn, board);

            // Start the game
            Draw.DrawBoard();
            Draw.DrawPieces(board);
            bool play = true;
            bool AITurn = false;
            Console.SetCursorPosition(4, 1);
            while (play == true)
            {
                if (turn % 2 == 0 && (AIPlayer == 2 || AIPlayer == 3))
                {
                    AITurn = true;
                }
                else if (turn % 2 == 0 && AIPlayer == 1)
                {
                    AITurn = false;
                }
                else if (turn % 2 != 0 && ((AIPlayer == 1 || AIPlayer == 3)))
                {
                    AITurn = true;
                }
                else if (turn % 2 != 0 && AIPlayer == 2)
                {
                    AITurn = false;
                }

                if (AITurn == true)
                {
                    int[,] AIMove = new int[2, 2];
                    AIMove = (int[,])AI.AIMove(board, turn, AIPlayer).Clone();
                    Console.Write(AIMove[0, 0]);
                    Console.Write(AIMove[0, 1]);
                    Console.Write(AIMove[1, 0]);
                    Console.Write(AIMove[1, 1]);
                    states[0] = turn;
                    states[1] = player1score;
                    states[2] = player2score;
                    int boardPiece = board[AIMove[0, 0], AIMove[0, 1]];
                    AI.Thinking(3);
                    //pick
                    if (holding == 0)
                    {
                        Console.WriteLine("clause 1");
                        if ((turn % 2 == 0) && (boardPiece % 2 == 0))
                        {
                            // set origin Black
                            hop = true;// required to stop game turn updating on keypress
                            origXY[0] = AIMove[0, 0];
                            origXY[1] = AIMove[0, 1];
                            holding = boardPiece;
                            board[AIMove[0, 0], AIMove[0, 1]] = 0;
                            Draw.DrawPieces(board);

                        }
                        else
                        if ((turn % 2 != 0) && (boardPiece % 2 != 0))
                        {
                            //set origin Red
                            hop = true;
                            origXY[0] = AIMove[0, 0];
                            origXY[1] = AIMove[0, 1];
                            holding = boardPiece;
                            board[AIMove[0, 0], AIMove[0, 1]] = 0;
                            Draw.DrawPieces(board);

                        }
                    }
                    //place
                    else
                    if (holding != 0)
                    {
                        Console.WriteLine("clause 2");
                        //set the destination
                        destXY[0] = AIMove[1, 0];
                        destXY[1] = AIMove[1, 1];
                        // placing the piece
                        if ((Validate.ValidMove(holding, boardPiece, origXY, destXY, turn) == true) && boardPiece == 0)
                        {


                            // king move

                            if ((holding == 1 && destXY[1] == 7) || (holding == 2 && destXY[1] == 0))
                            {
                                board[destXY[0], destXY[1]] = holding + 2;
                                holding = 0;
                                Draw.DrawPieces(board);
                                if (origXY[0] != destXY[0])
                                {
                                    hop = false;
                                }
                            }
                            // standard move
                            else
                            {
                                board[destXY[0], destXY[1]] = holding;
                                holding = 0;
                                Draw.DrawPieces(board);
                                if (origXY[0] != destXY[0])
                                {
                                    hop = false;
                                }
                            }
                        }
                        // taking a piece
                        else
                        if (Validate.ValidMove(holding, boardPiece, origXY, destXY, turn) == true && boardPiece != 0)
                        {
                            int spaceX = (destXY[0] + (destXY[0] - origXY[0]));
                            int spaceY = (destXY[1] + (destXY[1] - origXY[1]));

                            Console.WriteLine("clause 3");
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
                                    board[destXY[0], destXY[1]] = 0;
                                    hop = false;
                                    board[origXY[0], origXY[1]] = 0;
                                    origXY[0] = spaceX;
                                    origXY[1] = spaceY;

                                    Draw.DrawPieces(board);
                                }
                                else
                                {
                                    int adderA = 0;
                                    int adderB = 0;
                                    board[origXY[0], origXY[1]] = 0;
                                    board[spaceX, spaceY] = holding;
                                    board[destXY[0], destXY[1]] = 0;
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
                                    Draw.DrawPieces(board);
                                }
                            }
                        }

                        if (hop == false)
                        {

                            Console.WriteLine("clause 4");
                            holding = 0;
                            turn++;

                            if (moveList.ContainsKey(turn) != true)
                            {
                                moveList.Add(turn, (int[,])board.Clone());
                            }
                            moveList[turn] = (int[,])board.Clone();
                        }
                    }

                    Draw.WriteScores(player1score, player2score, turn);
                }



                else if (AITurn == false)
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
                            using (StreamWriter outputFile = new StreamWriter(@".\\CheckersSave.csv"))
                            {
                                //outputFile.WriteLine(turn);
                                //outputFile.WriteLine(player1score);
                                //outputFile.WriteLine(player2score);
                                foreach (KeyValuePair<int, int[,]> pair in moveList)
                                {
                                    outputFile.WriteLine(String.Join(",", pair.Value.Cast<int>()));
                                }
                            }
                        }


                        if (key == ConsoleKey.U)
                        {
                            redoMove = board;
                            redoStates[0] = turn;
                            redoStates[1] = player1score;
                            redoStates[2] = player2score;
                            foreach (KeyValuePair<int, int[,]> pair in moveList)
                            {
                                if (moveList.ContainsKey(turn - 1) == true)
                                {
                                    board = (int[,])moveList[states[0]].Clone();
                                    turn = states[0];
                                    player1score = states[1];
                                    player2score = states[2];
                                }
                            }
                            Draw.DrawPieces(board);
                            Draw.WriteScores(player1score, player2score, turn);
                        }

                        if (key == ConsoleKey.R)
                        {
                            board = redoMove;
                            turn = redoStates[0];
                            player1score = redoStates[1];
                            player2score = redoStates[2];
                            Draw.DrawPieces(board);
                            Draw.WriteScores(player1score, player2score, turn);
                        }
                        // debug
                        if (key == ConsoleKey.Enter)
                        {

                            Console.SetCursorPosition(0, 24);
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine("\n\n");
                            Console.WriteLine("x" + x + " y" + y + " Holding:" + holding + " Turn:" + turn + " s1:" + player1score + " s2:" + player2score + "  " + "states [0]: " + states[0]);

                            for (int p = 0; p < 8; p++)
                            {
                                for (int l = 0; l < 8; l++)
                                {
                                    switch (board[p, l])
                                    {
                                        case 0:
                                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                                            Console.Write("0 ");
                                            break;
                                        case 1:
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.Write("1 ");
                                            break;
                                        case 2:
                                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                                            Console.Write("2 ");
                                            break;
                                        case 3:
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.Write("K ");
                                            break;
                                        case 4:
                                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                                            Console.Write("K ");
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                Console.Write("\n");
                            }

                            foreach (KeyValuePair<int, int[,]> pair in moveList)
                            {
                                Console.WriteLine(pair.Key);

                                int[,] temp = new int[8, 8];

                                temp = (int[,])moveList[pair.Key].Clone();

                                for (int t = 0; t < 8; t++)
                                {
                                    for (int g = 0; g < 8; g++)
                                    {
                                        switch (temp[t, g])
                                        {
                                            case 0:
                                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                                Console.Write("0 ");
                                                break;
                                            case 1:
                                                Console.ForegroundColor = ConsoleColor.Black;
                                                Console.Write("1 ");
                                                break;
                                            case 2:
                                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                                Console.Write("2 ");
                                                break;
                                            case 3:
                                                Console.ForegroundColor = ConsoleColor.White;
                                                Console.Write("K ");
                                                break;
                                            case 4:
                                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                                Console.Write("K ");
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    Console.Write("\n");
                                }
                            }
                        }
                        //

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
                                    Draw.DrawPieces(board);

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
                                    Draw.DrawPieces(board);

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
                                if ((Validate.ValidMove(holding, boardPiece, origXY, destXY, turn) == true) && board[boardRow, boardColumn] == 0)
                                {
                                    // king move

                                    if ((holding == 1 && boardRow == 7) || (holding == 2 && boardRow == 0))
                                    {
                                        board[boardRow, boardColumn] = holding + 2;
                                        holding = 0;
                                        Draw.DrawPieces(board);
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
                                        Draw.DrawPieces(board);
                                        if (origXY[0] != destXY[0])
                                        {
                                            hop = false;
                                        }
                                    }
                                }
                                // taking a piece
                                else
                                if (Validate.ValidMove(holding, boardPiece, origXY, destXY, turn) == true
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

                                            Draw.DrawPieces(board);
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
                                            Draw.DrawPieces(board);
                                        }
                                    }
                                }

                                if (hop == false)
                                {
                                    holding = 0;
                                    turn++;
                                    if (moveList.ContainsKey(turn) != true)
                                    {
                                        moveList.Add(turn, (int[,])board.Clone());
                                    }
                                    moveList[turn] = (int[,])board.Clone();
                                }
                            }

                            Draw.WriteScores(player1score, player2score, turn);
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
                            AI.Thinking(5);
                            play = false;
                        }
                        if (player2score == 12)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(47, 13);

                            Console.Write("Player 2 Wins!");
                            // 5 second pause timer
                            AI.Thinking(5);
                            play = false;
                        }

                        Console.SetCursorPosition(x, y);
                    }
                }
            }
        }
    }
}

