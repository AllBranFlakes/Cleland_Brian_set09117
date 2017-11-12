using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/*
TO-DO
Version 0.9.0

-needs AI
*/


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //set title and console window display 
            Console.Title = "G.L.A.D.O.S.";
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.SetWindowSize(110, 35);
            Sound.Play(1);

            //define the board structure (0 = empty | 1 = Red | 2 = Black | 3 = Red King | 4 = Black King)
            int[,] board =  {{ 0, 1, 0, 1, 0, 1, 0, 1 },
                             { 1, 0, 1, 0, 1, 0, 1, 0 },
                             { 0, 1, 0, 1, 0, 1, 0, 1 },
                             { 0, 0, 0, 0, 0, 0, 0, 0 },
                             { 0, 0, 0, 0, 0, 0, 0, 0 },
                             { 2, 0, 2, 0, 2, 0, 2, 0 },
                             { 0, 2, 0, 2, 0, 2, 0, 2 },
                             { 2, 0, 2, 0, 2, 0, 2, 0 }};

            //move list 
            Dictionary<int, int[,]> moveList = new Dictionary<int, int[,]>
            {
                { 0, (int[,])board.Clone() }
            };

            bool play = true;

            //bool for ai
            bool AIPlayer = false;
            int AIColour = 0;


            Draw.DrawTitle(AIPlayer, AIColour);
            while (play == true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo info = Console.ReadKey(true);
                    ConsoleKey key = info.Key;

                    if (key == ConsoleKey.S)
                    {
                        Console.Clear();
                        Game.MainGame(AIPlayer, AIColour, board, moveList);
                    }
                    if (key == ConsoleKey.Q)
                    {
                        Console.Clear();
                        Console.SetCursorPosition(45, 10);
                        Console.Write("Thanks and goodbye!");
                        Game.Thinking(5);
                        play = false;
                    }
                    if (key == ConsoleKey.D1)
                    {
                        // Player Vs Player
                        Console.Clear();
                        AIPlayer = false;
                        AIColour = 0;
                        Draw.DrawTitle(AIPlayer, AIColour);
                    }
                    if (key == ConsoleKey.D2)
                    {
                        // Player Vs CPU(Red)
                        Console.Clear();
                        AIPlayer = true;
                        AIColour = 1;
                        Draw.DrawTitle(AIPlayer, AIColour);
                    }

                    if (key == ConsoleKey.D3)
                    {
                        // Player Vs CPU(Black)
                        Console.Clear();
                        AIPlayer = true;
                        AIColour = 2;
                        Draw.DrawTitle(AIPlayer, AIColour);
                    }

                    if (key == ConsoleKey.D4)
                    {
                        // CPU Vs CPU
                        Console.Clear();
                        AIPlayer = true;
                        AIColour = 3;
                        Draw.DrawTitle(AIPlayer, AIColour);
                    }

                    if (key == ConsoleKey.L)
                    {
                        //Load previous game
                        using (StreamReader sr = new StreamReader(@".\\CheckersSave.csv"))
                        {

                            // Get the file's text.
                            string whole_file = System.IO.File.ReadAllText(@".\\CheckersSave.csv");

                            // Split into lines.
                            whole_file = whole_file.Replace('\n', '\r');
                            string[] lines = whole_file.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);

                            // See how many turns and items there are.
                            int num_turns = lines.Length;
                            int num_items = lines[0].Split(',').Length;

                            //
                            int[,] newRow = new int[8, 8];
                            var lineCount = File.ReadLines(@".\\CheckersSave.csv").Count();
                            string line;
                            int moveCount = 0;
                            while ((line = sr.ReadLine()) != null)
                            {
                                while (moveCount != lineCount)
                                {
                                    int[] ia = line.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                                    int counter = 0;
                                    while (counter < 64)
                                    {
                                        for (int j = 0; j < 8; j++)
                                        {
                                            for (int k = 0; k < 8; k++)
                                            {
                                                newRow[j, k] = ia[counter];
                                                counter++;
                                            }
                                        }
                                    }
                                    if (moveList.ContainsKey(moveCount) != true)
                                    {
                                        moveList.Add(moveCount, newRow);
                                    }
                                    moveList[moveCount] = (int[,])newRow.Clone();
                                    moveCount++;
                                }
                            }
                            Draw.DrawPieces(newRow);
                        }
                    }
                }
            }
        }
    }
}








