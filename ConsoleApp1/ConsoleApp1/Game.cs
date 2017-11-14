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
            Console.Clear();

            //Sound.Play(1);
            int[,] gameBoard = board.Clone() as int[,];
            int[,] boardReset = board.Clone() as int[,];
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

            //AI Variable
            bool play = true;
            bool AITurn = false;
            int[] AIMove = { 0, 0, 0, 0 };

            //set move list
            moveList.Add(turn, gameBoard);

            // Start the game
            Draw.DrawBoard();
            Draw.DrawPieces(gameBoard);
            Console.SetCursorPosition(4, 1);

            // main game loop
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
                else if (AIPlayer == 0)
                {
                    AITurn = false;
                }

                if (AITurn == true)
                {
                    AIMove = (int[])AI.AIMove(gameBoard, turn).Clone();
                    AI.Thinking(1);
                    if (AIMove[0] == 0 && AIMove[1] == 0 && AIMove[2] == 0 && AIMove[3] == 0)
                    {
                        play = false;
                    }
                    states[0] = turn;
                    states[1] = player1score;
                    states[2] = player2score;
                    holding = gameBoard[AIMove[0], AIMove[1]];
                    int boardPiece = gameBoard[AIMove[2], AIMove[3]];

                    // set origin AI
                    hop = true;// required to stop game turn updating erroneously
                    origXY[0] = AIMove[0];
                    origXY[1] = AIMove[1];
                    gameBoard[origXY[0], origXY[1]] = 0;

                    // set destination AI
                    destXY[0] = AIMove[2];
                    destXY[1] = AIMove[3];
                    // placing the piece
                    if ((Validate.ValidMove(holding, boardPiece, origXY, destXY, turn) == true) && boardPiece == 0)
                    {
                        // king move
                        if ((holding == 1 && destXY[0] == 7) || (holding == 2 && destXY[0] == 0))
                        {
                            gameBoard[destXY[0], destXY[1]] = holding + 2;
                            holding = 0;
                            Draw.DrawPieces(gameBoard);
                            if (origXY[0] != destXY[0])
                            {
                                hop = false;
                            }
                        }
                        // standard move
                        else
                        {
                            gameBoard[destXY[0], destXY[1]] = holding;
                            holding = 0;
                            Draw.DrawPieces(gameBoard);
                            if (origXY[0] != destXY[0])
                            {
                                hop = false;
                            }
                        }
                    }
                    // taking a piece
                    else
                    if (Validate.ValidMove(holding, boardPiece, origXY, destXY, turn) == true
                            && boardPiece != 0 && (boardPiece!=holding && boardPiece != holding-2))
                    {
                        int spaceX = (destXY[0] + (destXY[0] - origXY[0]));
                        int spaceY = (destXY[1] + (destXY[1] - origXY[1]));

                        /* stops spaceX & spaceY from running of the ends of the array */
                        if (spaceX < 0 || spaceX > 7 || spaceY < 0 || spaceY > 7)
                        {
                        }
                        else
                        {
                            if (gameBoard[spaceX, spaceY] == 0)
                            {

                                if ((holding == 1 && spaceX == 7) || (holding == 2 && spaceX == 0))
                                {


                                    holding = holding + 2;
                                    gameBoard[spaceX, spaceY] = holding;
                                    gameBoard[destXY[0], destXY[1]] = 0;
                                    hop = false;
                                    gameBoard[origXY[0], origXY[1]] = 0;
                                    origXY[0] = spaceX;
                                    origXY[1] = spaceY;

                                    Draw.DrawPieces(gameBoard);
                                }
                                else
                                {
                                    int adderA = 0;
                                    int adderB = 0;
                                    gameBoard[origXY[0], origXY[1]] = 0;
                                    gameBoard[spaceX, spaceY] = holding;
                                    gameBoard[destXY[0], destXY[1]] = 0;
                                    switch (holding)
                                    {
                                        case 1:
                                            adderA = 1;
                                            adderB = 1;
                                            break;
                                        case 2:
                                            adderA = -1;
                                            adderB = -1;
                                            break;
                                        case 3:
                                            adderA = 1;
                                            adderB = -1;
                                            break;
                                        case 4:
                                            adderA = -1;
                                            adderB = 1;
                                            break;
                                    }

                                    if ((spaceY == 0 || spaceY == 1) && spaceX >= 2 && spaceX <= 5)
                                    {
                                        if (((gameBoard[spaceX + adderA, spaceY + 1] != 0 && gameBoard[spaceX + adderA, spaceY + 1] % 2 != holding % 2) && gameBoard[spaceX + (adderA + adderA), spaceY + 2] == 0)
                                         || ((gameBoard[spaceX + adderB, spaceY + 1] != 0 && gameBoard[spaceX + adderB, spaceY + 1] % 2 != holding % 2) && gameBoard[spaceX + (adderB + adderB), spaceY + 2] == 0))
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
                                        if (gameBoard[spaceX + adderA, spaceY - 1] != 0 && gameBoard[spaceX + adderA, spaceY - 1] % 2 != holding % 2 && gameBoard[spaceX + (adderA + adderA), spaceY - 2] == 0
                                         || gameBoard[spaceX + adderB, spaceY - 1] != 0 && gameBoard[spaceX + adderB, spaceY - 1] % 2 != holding % 2 && gameBoard[spaceX + (adderB + adderB), spaceY - 2] == 0)
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
                                        if ((gameBoard[spaceX + adderA, spaceY - 1] != 0 && gameBoard[spaceX + adderA, spaceY - 1] % 2 != holding % 2 && gameBoard[spaceX + (adderA + adderA), spaceY - 2] == 0)
                                         || (gameBoard[spaceX + adderB, spaceY - 1] != 0 && gameBoard[spaceX + adderB, spaceY - 1] % 2 != holding % 2 && gameBoard[spaceX + (adderB + adderB), spaceY - 2] == 0)
                                         || (gameBoard[spaceX + adderA, spaceY + 1] != 0 && gameBoard[spaceX + adderA, spaceY + 1] % 2 != holding % 2 && gameBoard[spaceX + (adderA + adderA), spaceY + 2] == 0)
                                         || (gameBoard[spaceX + adderB, spaceY + 1] != 0 && gameBoard[spaceX + adderB, spaceY + 1] % 2 != holding % 2 && gameBoard[spaceX + (adderB + adderB), spaceY + 2] == 0))
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
                                            if (gameBoard[spaceX + 1, spaceY + 1] != 0 && gameBoard[spaceX + 1, spaceY + 1] % 2 != holding % 2 && gameBoard[spaceX + 2, spaceY + 2] == 0)
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
                                            if (gameBoard[spaceX + 1, spaceY - 1] != 0 && gameBoard[spaceX + 1, spaceY - 1] % 2 != holding % 2 && gameBoard[spaceX + 2, spaceY - 2] == 0)
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
                                            if (gameBoard[spaceX - 1, spaceY + 1] != 0 && gameBoard[spaceX - 1, spaceY + 1] % 2 != holding % 2 && gameBoard[spaceX - 2, spaceY + 2] == 0)
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
                                            if (gameBoard[spaceX - 1, spaceY - 1] != 0 && gameBoard[spaceX - 1, spaceY - 1] % 2 != holding % 2 && gameBoard[spaceX - 2, spaceY - 2] == 0)
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
                                    Draw.DrawPieces(gameBoard);
                                }
                            }
                        }

                        player1score = Draw.GetScore1(gameBoard);
                        player2score = Draw.GetScore2(gameBoard);
                        Draw.WriteScores(player1score, player2score, turn);
                    }
                    if (hop == false)
                    {
                        holding = 0;
                        turn++;
                        if (moveList.ContainsKey(turn) != true)
                        {
                            moveList.Add(turn, (int[,])gameBoard.Clone());
                        }
                        moveList[turn] = (int[,])gameBoard.Clone();

                    }

                    /* Win Conditions */
                   
                    if (player1score == 12 || player2score == 12)
                    {
                        play=Validate.WinChecks(play,gameBoard,player1score,player2score);
                        moveList.Clear();
                    }

                    Draw.WriteScores(player1score, player2score, turn);
                }
               


                Console.SetCursorPosition(x, y);
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
                        redoMove = gameBoard;
                        redoStates[0] = turn;
                        redoStates[1] = player1score;
                        redoStates[2] = player2score;
                        foreach (KeyValuePair<int, int[,]> pair in moveList)
                        {
                            if (moveList.ContainsKey(turn - 1) == true)
                            {
                                gameBoard = (int[,])moveList[states[0]].Clone();
                                turn = states[0];
                                player1score = states[1];
                                player2score = states[2];
                            }
                        }
                        Draw.DrawPieces(gameBoard);
                    }

                    if (key == ConsoleKey.R)
                    {
                        gameBoard = redoMove;
                        turn = redoStates[0];
                        player1score = redoStates[1];
                        player2score = redoStates[2];
                        Draw.DrawPieces(gameBoard);
                    }
                    
                    // Player interaction code
                    if (key == ConsoleKey.Spacebar)
                    {
                        player1score = Draw.GetScore1(gameBoard);
                        player2score = Draw.GetScore2(gameBoard);
                        Draw.WriteScores(player1score, player2score, turn);
                        states[0] = turn;
                        states[1] = player1score;
                        states[2] = player2score;
                        int boardPiece = gameBoard[boardRow, boardColumn];

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
                                gameBoard[boardRow, boardColumn] = 0;
                                Draw.DrawPieces(gameBoard);

                            }
                            else
                            if ((turn % 2 != 0) && (boardPiece % 2 != 0))
                            {
                                //set origin Red
                                hop = true;
                                origXY[0] = boardRow;
                                origXY[1] = boardColumn;
                                holding = boardPiece;
                                gameBoard[boardRow, boardColumn] = 0;
                                Draw.DrawPieces(gameBoard);

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
                            if ((Validate.ValidMove(holding, boardPiece, origXY, destXY, turn) == true) && gameBoard[boardRow, boardColumn] == 0)
                            {
                                // king move

                                if ((holding == 1 && boardRow == 7) || (holding == 2 && boardRow == 0))
                                {
                                    gameBoard[boardRow, boardColumn] = holding + 2;
                                    holding = 0;
                                    Draw.DrawPieces(gameBoard);
                                    if (origXY[0] != destXY[0])
                                    {
                                        hop = false;
                                    }
                                }
                                // standard move
                                else
                                {
                                    gameBoard[boardRow, boardColumn] = holding;
                                    holding = 0;
                                    Draw.DrawPieces(gameBoard);
                                    if (origXY[0] != destXY[0])
                                    {
                                        hop = false;
                                    }
                                }
                            }
                            // taking a piece
                            else
                            if (Validate.ValidMove(holding, boardPiece, origXY, destXY, turn) == true
                                    && boardPiece != 0 && (boardPiece != holding && boardPiece != holding - 2))
                            {
                                int spaceX = (destXY[0] + (destXY[0] - origXY[0]));
                                int spaceY = (destXY[1] + (destXY[1] - origXY[1]));

                                /* stops spaceX & spaceY from running of the ends of the array */
                                if (spaceX < 0 || spaceX > 7 || spaceY < 0 || spaceY > 7)
                                {
                                    
                                }
                                else
                                {
                                    if (gameBoard[spaceX, spaceY] == 0)
                                    {

                                        if ((holding == 1 && spaceX == 7) || (holding == 2 && spaceX == 0))
                                        {
                                            holding = holding + 2;
                                            gameBoard[spaceX, spaceY] = holding;
                                            gameBoard[boardRow, boardColumn] = 0;
                                            hop = false;
                                            gameBoard[origXY[0], origXY[1]] = 0;
                                            origXY[0] = spaceX;
                                            origXY[1] = spaceY;

                                            Draw.DrawPieces(gameBoard);
                                        }
                                        else
                                        {
                                            int adderA = 0;
                                            int adderB = 0;
                                            gameBoard[origXY[0], origXY[1]] = 0;
                                            gameBoard[spaceX, spaceY] = holding;
                                            gameBoard[boardRow, boardColumn] = 0;
                                            switch (holding)
                                            {
                                                case 1:
                                                    adderA = 1;
                                                    adderB = 1;
                                                    break;
                                                case 2:
                                                    adderA = -1;
                                                    adderB = -1;
                                                    break;
                                                case 3:
                                                    adderA = 1;
                                                    adderB = -1;
                                                    break;
                                                case 4:
                                                    adderA = -1;
                                                    adderB = 1;
                                                    break;
                                            }

                                            if ((spaceY == 0 || spaceY == 1) && spaceX >= 2 && spaceX <= 5)
                                            {
                                                if (((gameBoard[spaceX + adderA, spaceY + 1] != 0 && gameBoard[spaceX + adderA, spaceY + 1] % 2 != holding % 2) && gameBoard[spaceX + (adderA + adderA), spaceY + 2] == 0)
                                                 || ((gameBoard[spaceX + adderB, spaceY + 1] != 0 && gameBoard[spaceX + adderB, spaceY + 1] % 2 != holding % 2) && gameBoard[spaceX + (adderB + adderB), spaceY + 2] == 0))
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
                                                if (gameBoard[spaceX + adderA, spaceY - 1] != 0 && gameBoard[spaceX + adderA, spaceY - 1] % 2 != holding % 2 && gameBoard[spaceX + (adderA + adderA), spaceY - 2] == 0
                                                 || gameBoard[spaceX + adderB, spaceY - 1] != 0 && gameBoard[spaceX + adderB, spaceY - 1] % 2 != holding % 2 && gameBoard[spaceX + (adderB + adderB), spaceY - 2] == 0)
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
                                                if ((gameBoard[spaceX + adderA, spaceY - 1] != 0 && gameBoard[spaceX + adderA, spaceY - 1] % 2 != holding % 2 && gameBoard[spaceX + (adderA + adderA), spaceY - 2] == 0)
                                                 || (gameBoard[spaceX + adderB, spaceY - 1] != 0 && gameBoard[spaceX + adderB, spaceY - 1] % 2 != holding % 2 && gameBoard[spaceX + (adderB + adderB), spaceY - 2] == 0)
                                                 || (gameBoard[spaceX + adderA, spaceY + 1] != 0 && gameBoard[spaceX + adderA, spaceY + 1] % 2 != holding % 2 && gameBoard[spaceX + (adderA + adderA), spaceY + 2] == 0)
                                                 || (gameBoard[spaceX + adderB, spaceY + 1] != 0 && gameBoard[spaceX + adderB, spaceY + 1] % 2 != holding % 2 && gameBoard[spaceX + (adderB + adderB), spaceY + 2] == 0))
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
                                                    if (gameBoard[spaceX + 1, spaceY + 1] != 0 && gameBoard[spaceX + 1, spaceY + 1] % 2 != holding % 2 && gameBoard[spaceX + 2, spaceY + 2] == 0)
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
                                                    if (gameBoard[spaceX + 1, spaceY - 1] != 0 && gameBoard[spaceX + 1, spaceY - 1] % 2 != holding % 2 && gameBoard[spaceX + 2, spaceY - 2] == 0)
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
                                                    if (gameBoard[spaceX - 1, spaceY + 1] != 0 && gameBoard[spaceX - 1, spaceY + 1] % 2 != holding % 2 && gameBoard[spaceX - 2, spaceY + 2] == 0)
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
                                                    if (gameBoard[spaceX - 1, spaceY - 1] != 0 && gameBoard[spaceX - 1, spaceY - 1] % 2 != holding % 2 && gameBoard[spaceX - 2, spaceY - 2] == 0)
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
                                            Draw.DrawPieces(gameBoard);
                                        }
                                    }
                                }
                            }

                            if (hop == false)
                            {
                                holding = 0;
                                turn++;
                                if (moveList.ContainsKey(turn) != true)
                                {
                                    moveList.Add(turn, (int[,])gameBoard.Clone());
                                }
                                moveList[turn] = (int[,])gameBoard.Clone();
                            }
                        }

                        if (holding == 0)
                        {
                            player1score = Draw.GetScore1(gameBoard);
                            player2score = Draw.GetScore2(gameBoard);
                            Draw.WriteScores(player1score, player2score, turn);
                        }
                        /* Win Conditions */
                        if (player1score == 12)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.SetCursorPosition(47, 13);
                            Console.Write("Player 1 Wins!");
                            Draw.DrawPieces(gameBoard);
                            // 5 second pause timer
                            AI.Thinking(5);

                            moveList.Clear();
                            play = false;
                        }
                        if (player2score == 12)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(47, 13);
                            Console.Write("Player 2 Wins!");
                            Draw.DrawPieces(gameBoard);
                            // 5 second pause timer
                            AI.Thinking(5);

                            moveList.Clear();
                            play = false;
                        }
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


                   
                }
            }


            gameBoard = boardReset.Clone() as int[,];

            AI.Thinking(2);
            moveList.Clear();
            Draw.DrawTitle(AIPlayer);
        }
    }
}



