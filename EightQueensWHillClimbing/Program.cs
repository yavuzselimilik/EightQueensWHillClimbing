﻿using System;
using System.Diagnostics;



//source: https://www.geeksforgeeks.org/n-queen-problem-local-search-using-hill-climbing-with-random-neighbour/
namespace EightQueensWHillClimbing
{
    class Program
    {
        static Random rnd = new Random();
        static int N = 8;
        static int REPEAT = 25;
        static int count_restart;
        static int count_displacement;
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();

            int[] count_restart_array = new int[REPEAT];
            int[] count_jump_array = new int[REPEAT];
            int[] time_array = new int[REPEAT];

            for (int i = 0; i < REPEAT; i++)
            {
                watch.Start();

                count_restart = 0;
                count_displacement = 0;
                Console.WriteLine("\n*****************");

                Console.WriteLine("      "+ (i + 1) + "/25");

                int[] state = new int[N];
                int[,] board = new int[N,N];

                // Getting a starting point by
                // randomly configuring the board
                configureRandomly(board, state);

                // Do hill climbing on 
                // board obtained
                hillClimbing(board, state);

                watch.Stop();

                count_restart_array[i] = count_restart;
                count_jump_array[i] = count_displacement;
                time_array[i] = (int)watch.Elapsed.TotalMilliseconds;

                Console.WriteLine("\nCalculation time: {0} ms", (int)watch.Elapsed.TotalMilliseconds);
                Console.WriteLine("Random restarts number: {0}",count_restart);
                Console.WriteLine("Displacement number: {0}", count_displacement);

                watch.Restart();
            }

            Console.WriteLine("\nSTATISTICS");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("| Restarts   -   Displacement  -  Time |");
            Console.WriteLine("----------------------------------------");

            double average_restart = 0;
            double average_jump = 0;
            double average_time = 0;
            for (int i = 0; i < REPEAT; i++)
            {
                average_restart += count_restart_array[i];
                average_jump += count_jump_array[i];
                average_time += time_array[i];
                Console.WriteLine(String.Format("|    {0,-3}     |       {1,-5}     |   {2,-3} |", count_restart_array[i], count_jump_array[i], time_array[i]));
            }
            Console.WriteLine("----------------------------------------");

            average_restart /= REPEAT;
            average_jump /= REPEAT;
            average_time /= REPEAT;

            Console.WriteLine("avg.res:{0} avg.jump:{1} avg.time:{2}", average_restart, average_jump, average_time);
            Console.WriteLine("----------------------------------------");

        }
        // A utility function that configures
        // the 2D array "board" and
        // array "state" randomly to provide
        // a starting point for the algorithm.
        static void configureRandomly(int[,] board, int[] state)
        {
            // Iterating through the
            // column indices
            for (int i = 0; i < N; i++)
            {
                // Getting a random row index
                state[i] = rnd.Next(N);

                // Placing a queen on the
                // obtained place in
                // chessboard.
                board[state[i], i] = 1;
            }
        }

        // A utility function that prints
        // the 2D array "board".
        static void printBoard(int[,] board)
        {

            for (int i = 0; i < N; i++)
            {
                Console.Write(" ");
                for (int j = 0; j < N; j++)
                {
                    Console.Write(board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        // A utility function that prints
        // the array "state".
        static void printState(int[] state)
        {
            for (int i = 0; i < N; i++)
            {
                Console.WriteLine(" " + state[i] + " ");
            }
            Console.WriteLine();
        }

        // A utility function that compares
        // two arrays, state1 and state2 and
        // returns true if equal
        // and false otherwise.
        static bool compareStates(int[] state1, int[] state2)
        {
            for (int i = 0; i < N; i++)
            {
                if (state1[i] != state2[i])
                {
                    return false;
                }
            }
            return true;
        }

        // A utility function that fills
        // the 2D array "board" with
        // values "value"
        static void fill(int[,] board, int value)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    board[i, j] = value;
                }
            }
        }

        // This function calculates the
        // objective value of the
        // state(queens attacking each other)
        // using the board by the
        // following logic.
        static int calculateObjective(int[,] board, int[] state)
        {

            // For each queen in a column, we check
            // for other queens falling in the line
            // of our current queen and if found,
            // any, then we increment the variable
            // attacking count.

            // Number of queens attacking each other,
            // initially zero.
            int attacking = 0;

            // Variables to index a particular
            // row and column on board.
            int row, col;

            for (int i = 0; i < N; i++)
            {

                // At each column 'i', the queen is
                // placed at row 'state[i]', by the
                // definition of our state.

                // To the left of same row
                // (row remains constant
                // and col decreases)
                row = state[i];
                col = i - 1;
                while (col >= 0 && board[row, col] != 1)
                {
                    col--;
                }
                if (col >= 0 && board[row, col] == 1)
                {
                    attacking++;
                }

                // To the right of same row
                // (row remains constant
                // and col increases)
                row = state[i];
                col = i + 1;
                while (col < N && board[row, col] != 1)
                {
                    col++;
                }
                if (col < N && board[row, col] == 1)
                {
                    attacking++;
                }

                // Diagonally to the left up
                // (row and col simoultaneously
                // decrease)
                row = state[i] - 1;
                col = i - 1;
                while (col >= 0 && row >= 0 && board[row, col] != 1)
                {
                    col--;
                    row--;
                }
                if (col >= 0 && row >= 0 && board[row, col] == 1)
                {
                    attacking++;
                }

                // Diagonally to the right down
                // (row and col simoultaneously
                // increase)
                row = state[i] + 1;
                col = i + 1;
                while (col < N && row < N && board[row, col] != 1)
                {
                    col++;
                    row++;
                }
                if (col < N && row < N && board[row, col] == 1)
                {
                    attacking++;
                }

                // Diagonally to the left down
                // (col decreases and row
                // increases)
                row = state[i] + 1;
                col = i - 1;
                while (col >= 0 && row < N && board[row, col] != 1)
                {
                    col--;
                    row++;
                }
                if (col >= 0 && row < N && board[row, col] == 1)
                {
                    attacking++;
                }

                // Diagonally to the right up
                // (col increases and row
                // decreases)
                row = state[i] - 1;
                col = i + 1;
                while (col < N && row >= 0 && board[row, col] != 1)
                {
                    col++;
                    row--;
                }
                if (col < N && row >= 0 && board[row, col] == 1)
                {
                    attacking++;
                }
            }
            // Return pairs.
            return (int)(attacking / 2);
        }

        // A utility function that
        // generates a board configuration
        // given the state.
        static void generateBoard(int[,] board, int[] state)
        {
            fill(board, 0);
            for (int i = 0; i < N; i++)
            {
                board[state[i], i] = 1;
            }
        }

        // A utility function that copies
        // contents of state2 to state1.
        static void copyState(int[] state1, int[] state2)
        {
            for (int i = 0; i < N; i++)
            {
                state1[i] = state2[i];
            }
        }

        // This function gets the neighbour
        // of the current state having
        // the least objective value
        // amongst all neighbours as
        // well as the current state.
        static void getNeighbour(int[,] board, int[] state)
        {
            // Declaring and initializing the
            // optimal board and state with
            // the current board and the state
            // as the starting point.

            int[,] opBoard = new int[N,N];
            int[] opState = new int[N];

            copyState(opState, state);
            generateBoard(opBoard, opState);

            // Initializing the optimal
            // objective value

            int opObjective = calculateObjective(opBoard, opState);

            // Declaring and initializing
            // the temporary board and
            // state for the purpose
            // of computation.

            int[,] NeighbourBoard = new int[N,N];
            int[] NeighbourState = new int[N];

            copyState(NeighbourState, state);
            generateBoard(NeighbourBoard, NeighbourState);

            // Iterating through all
            // possible neighbours
            // of the board.

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {

                    // Condition for skipping the
                    // current state

                    if (j != state[i])
                    {

                        // Initializing temporary
                        // neighbour with the
                        // current neighbour.

                        NeighbourState[i] = j;
                        NeighbourBoard[NeighbourState[i],i]= 1;
                        NeighbourBoard[state[i],i]= 0;

                        // Calculating the objective
                        // value of the neighbour.

                        int temp = calculateObjective(NeighbourBoard, NeighbourState);

                        // Comparing temporary and optimal
                        // neighbour objectives and if
                        // temporary is less than optimal
                        // then updating accordingly.

                        if (temp <= opObjective)
                        {
                            opObjective = temp;
                            copyState(opState, NeighbourState);
                            generateBoard(opBoard, opState);
                            count_displacement++;
                        }

                        // Going back to the original
                        // configuration for the next
                        // iteration.

                        NeighbourBoard[NeighbourState[i],i] = 0;
                        NeighbourState[i] = state[i];
                        NeighbourBoard[state[i],i] = 1;
                    }
                }
            }
            copyState(state, opState);
            fill(board, 0);
            generateBoard(board, state);
        }

        static void hillClimbing(int[,] board, int[] state)
        {
            // Declaring  and initializing the
            // neighbour board and state with
            // the current board and the state
            // as the starting point.

            int[,] neighbourBoard = new int[N, N];
            int[] neighbourState = new int[N];

            copyState(neighbourState, state);
            generateBoard(neighbourBoard, neighbourState);

            while (true)
            {
                // Copying the neighbour board and
                // state to the current board and
                // state, since a neighbour
                // becomes current after the jump.
                
                copyState(state, neighbourState);
                generateBoard(board, state);

                // Getting the optimal neighbour
                getNeighbour(neighbourBoard, neighbourState);

                if (compareStates(state, neighbourState))
                {

                    // If neighbour and current are
                    // equal then no optimal neighbour
                    // exists and therefore output the
                    // result and break the loop.

                    printBoard(board);
                    break;
                }
                else if (calculateObjective(board,state) == calculateObjective(neighbourBoard,neighbourState))
                {
                    // If neighbour and current are
                    // not equal but their objectives
                    // are equal then we are either
                    // approaching a shoulder or a
                    // local optimum, in any case,
                    // jump to a random neighbour
                    // to escape it.

                    count_restart++;

                    // Random neighbour
                    neighbourState[rnd.Next(N)] = rnd.Next(N);
                    generateBoard(neighbourBoard, neighbourState);
                }
            };
        }
    }
    
}
