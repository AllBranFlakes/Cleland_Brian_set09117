using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(String[] args)
        {
            //set title and console window display 
            Console.Title = "G.L.A.D.O.S.";
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.SetWindowSize(105, 32);
            Sound.Play(2);

            //define the board structure (0 = empty | 1 = Red | 2 = Black | 3 = Red King | 4 = Black King)
            int[,] board =  {{ 0, 1, 0, 1, 0, 1, 0, 1 },
                             { 1, 0, 1, 0, 1, 0, 1, 0 },
                             { 0, 1, 0, 1, 0, 1, 0, 1 },
                             { 0, 0, 0, 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0, 0, 0, 0 },
                             { 2, 0, 2, 0, 2, 0, 2, 0 },
                             { 0, 2, 0, 2, 0, 2, 0, 2 },
                             { 2, 0, 2, 0, 2, 0, 2, 0 }};


            int[,] newBoard =  {{ 0, 1, 0, 1, 0, 1, 0, 1 },
                             { 1, 0, 1, 0, 1, 0, 1, 0 },
                             { 0, 1, 0, 1, 0, 1, 0, 1 },
                             { 0, 0, 0, 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0, 0, 0, 0 },
                             { 2, 0, 2, 0, 2, 0, 2, 0 },
                             { 0, 2, 0, 2, 0, 2, 0, 2 },
                             { 2, 0, 2, 0, 2, 0, 2, 0 }};

            int speed = 5;

            //move list 
            Dictionary<int, int[,]> moveList = new Dictionary<int, int[,]>
            {
                { 0, (int[,])board.Clone() }
            };

            bool play = true;

            //int for AItype
            int AIGameType = 0;


            Draw.DrawTitle(AIGameType, speed);
            while (play == true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo info = Console.ReadKey(true);
                    ConsoleKey key = info.Key;

                    if (key == ConsoleKey.N)
                    {
                        Console.Clear();
                        moveList.Clear();
                        Game.MainGame(AIGameType, newBoard, moveList, speed);
                    }
                    if (key == ConsoleKey.Q)
                    {
                        Console.Clear();
                        Console.SetCursorPosition(45, 10);
                        Console.Write("Thanks and goodbye!");
                        Sound.Play(3);
                        AI.Thinking(74);
                        play = false;
                    }
                    if (key == ConsoleKey.D1)
                    {
                        // Player Vs Player
                        Console.Clear();
                        AIGameType = 0;
                        Draw.DrawTitle(AIGameType, speed);
                    }
                    if (key == ConsoleKey.D2)
                    {
                        // Player Vs CPU(Red)
                        Console.Clear();
                        AIGameType = 1;
                        Draw.DrawTitle(AIGameType, speed);
                    }

                    if (key == ConsoleKey.D3)
                    {
                        // Player Vs CPU(Black)
                        Console.Clear();
                        AIGameType = 2;
                        Draw.DrawTitle(AIGameType, speed);
                    }

                    if (key == ConsoleKey.D4)
                    {
                        // CPU Vs CPU
                        Console.Clear();
                        AIGameType = 3;
                        Draw.DrawTitle(AIGameType, speed);
                    }

                    if (key == ConsoleKey.LeftArrow)
                    {
                        if (speed == 1)
                        {
                            speed = 5;
                        }
                        else
                        {
                            speed = 10;
                        }
                        Console.Clear();
                        Draw.DrawTitle(AIGameType, speed);
                    }
                    if (key == ConsoleKey.RightArrow)
                    {
                        if (speed == 10)
                        {
                            speed = 5;
                        }
                        else
                        {
                            speed = 1;
                        }
                        Console.Clear();
                        Draw.DrawTitle(AIGameType, speed);
                    }

                    if (key == ConsoleKey.L)
                    {
                        Draw.SaveLoad();
                    }

                    if (key == ConsoleKey.NumPad1)
                    {
                        using (StreamReader sr = new StreamReader(@".\\CheckersSave1.csv"))
                        {

                            string file = System.IO.File.ReadAllText(@".\\CheckersSave1.csv");
                            file = file.Replace('\n', '\r');
                            string[] lines = file.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);

                            int[,] tempBoard = new int[8, 8];
                            var saveCount = File.ReadLines(@".\\CheckersSave1.csv").Count();
                            string line;
                            int lineCount = 0;
                            while ((line = sr.ReadLine()) != null && saveCount != lineCount)
                            {

                                int[] tempPieceValue = line.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                                int counter = 0;
                                while (counter < 64)
                                {
                                    for (int j = 0; j < 8; j++)
                                    {
                                        for (int k = 0; k < 8; k++)
                                        {
                                            tempBoard[j, k] = tempPieceValue[counter];
                                            counter++;
                                        }
                                    }
                                }
                                if (moveList.ContainsKey(lineCount) != true)
                                {
                                    moveList.Add(lineCount, (int[,])tempBoard.Clone());
                                }
                                moveList[lineCount] = (int[,])tempBoard.Clone();
                                board = tempBoard;
                                lineCount++;
                            }
                            Draw.LoadSuccess();
                            AI.Thinking(10);
                            Console.Clear();
                            Game.MainGame(AIGameType, board, moveList, speed);
                        }
                    }
                    if (key == ConsoleKey.NumPad2)
                    {
                        using (StreamReader sr = new StreamReader(@".\\CheckersSave2.csv"))
                        {

                            string file = System.IO.File.ReadAllText(@".\\CheckersSave2.csv");
                            file = file.Replace('\n', '\r');
                            string[] lines = file.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);

                            int[,] tempBoard = new int[8, 8];
                            var saveCount = File.ReadLines(@".\\CheckersSave2.csv").Count();
                            string line;
                            int lineCount = 0;
                            while ((line = sr.ReadLine()) != null && saveCount != lineCount)
                            {
                                int[] tempPieceValue = line.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                                int counter = 0;
                                while (counter < 64)
                                {
                                    for (int j = 0; j < 8; j++)
                                    {
                                        for (int k = 0; k < 8; k++)
                                        {
                                            tempBoard[j, k] = tempPieceValue[counter];
                                            counter++;
                                        }
                                    }
                                }
                                if (moveList.ContainsKey(lineCount) != true)
                                {
                                    moveList.Add(lineCount, (int[,])tempBoard.Clone());
                                }
                                moveList[lineCount] = (int[,])tempBoard.Clone();
                                board = tempBoard;
                                lineCount++;
                            }
                            Draw.LoadSuccess();
                            AI.Thinking(10);
                            Console.Clear();
                            Game.MainGame(AIGameType, board, moveList, speed);
                        }
                    }

                    if (key == ConsoleKey.NumPad3)
                    {
                        using (StreamReader sr = new StreamReader(@".\\CheckersSave3.csv"))
                        {
                            string file = System.IO.File.ReadAllText(@".\\CheckersSave3.csv");
                            file = file.Replace('\n', '\r');
                            string[] lines = file.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);

                            int[,] tempBoard = new int[8, 8];
                            var saveCount = File.ReadLines(@".\\CheckersSave3.csv").Count();
                            string line;
                            int lineCount = 0;
                            while ((line = sr.ReadLine()) != null && saveCount != lineCount)
                            {
                                int[] tempPieceValue = line.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                                int counter = 0;
                                while (counter < 64)
                                {
                                    for (int j = 0; j < 8; j++)
                                    {
                                        for (int k = 0; k < 8; k++)
                                        {
                                            tempBoard[j, k] = tempPieceValue[counter];
                                            counter++;
                                        }
                                    }
                                }
                                if (moveList.ContainsKey(lineCount) != true)
                                {
                                    moveList.Add(lineCount, (int[,])tempBoard.Clone());
                                }
                                moveList[lineCount] = (int[,])tempBoard.Clone();
                                board = tempBoard;
                                lineCount++;
                            }
                        }
                        Draw.LoadSuccess();
                        AI.Thinking(10);
                        Console.Clear();
                        Game.MainGame(AIGameType, board, moveList, speed);
                    }
                }
            }
        }
    }
}








