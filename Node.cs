using System;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleGame
{
    class AStarNodeCosts
    {
        private string heuristicFunctionName;
        public int f, h, g;


        public AStarNodeCosts(int h, int g, string heuristicFunctionName)
        {
            this.h = h;
            this.g = g;
            this.f = this.h + this.g;
            this.heuristicFunctionName = heuristicFunctionName;
        }

        public void PrintConsole()
        {
            Console.WriteLine(String.Format("\ng = {0}, {1} = {2}, f = {3}",
                this.g, this.heuristicFunctionName, this.h, this.f));
            Console.WriteLine();
        }
    }

    class Node
    {
        private int[] goal = { 1, 2, 3, 4, 5, 6, 7, 8, 0 };

        public List<Node> children = new List<Node>();
        public Node parent = default;
        public int[] state = new int[9];
        public int emptyTilePosition = 0;  
        public int stateSize = 3;
        public AStarNodeCosts nodeCosts = default;


        public Node(int[] state, int g, string heuristicFunction)
        {
            this.state = state;

            if (heuristicFunction == "1")
            {
                this.nodeCosts = new AStarNodeCosts(this.H1(), g, "h1");
            }
            else
            {
                this.nodeCosts = new AStarNodeCosts(this.H2(), g, "h2");
            }
        }

        public bool GoalTest()
        {
            return this.state.SequenceEqual(this.goal);
        }

        public void Expand(string heuristicFunction)
        {
            for (int i = 0; i < this.state.Length; i++)
            {
                if (this.state[i] == 0)
                {
                    this.emptyTilePosition = i;
                    break;
                }
            }

            int[] directions = { 1, this.stateSize, -1, -this.stateSize };

            foreach (int direction in directions)
            {
                int newTilePosition = this.emptyTilePosition + direction;

                if (this.IsValidMove(this.emptyTilePosition, newTilePosition, direction))
                {
                    int[] newState = new int[this.state.Length];
                    Array.Copy(this.state, newState, this.state.Length);

                    this.Swap(newState, this.emptyTilePosition, newTilePosition);

                    Node child = new Node(newState, this.nodeCosts.g + 1, heuristicFunction) 
                    { 
                        parent = this
                    };

                    this.children.Add(child);
                }
            }
        }

        public void PrintConsole()
        {
            this.nodeCosts.PrintConsole();

            for (int i = 0; i < this.stateSize; i++)
            {
                for (int j = 0; j < this.stateSize; j++)
                {
                    Console.Write(this.state[i * this.stateSize + j] + " ");
                }
                Console.WriteLine();
            }
        }

        private int H1()
        {
            int numberOfWrongTiles = default;
            for (int tilePosition = 0; tilePosition<this.state.Length; tilePosition ++)
            {
                if (this.state[tilePosition] != this.goal[tilePosition])
                {
                    numberOfWrongTiles++;
                }
            }
            return numberOfWrongTiles;
        }

        private int H2()
        {
            int result = 0;
            for (int currentStateTilePos = 0; currentStateTilePos < this.stateSize * this.stateSize; currentStateTilePos++)
            {
                int currentStateTilePosX = currentStateTilePos / this.stateSize;
                int currentStateTilePosY = currentStateTilePos % this.stateSize;

                int golStateTilePos = Array.IndexOf(this.goal, this.state[currentStateTilePos]);
                int goalStateTilePosX = golStateTilePos / this.stateSize;
                int goalStateTilePosY = golStateTilePos % this.stateSize;

                result += Math.Abs(goalStateTilePosX - currentStateTilePosX) + Math.Abs(goalStateTilePosY - currentStateTilePosY);
            }

            return result;
        }


        private bool IsValidMove(int tilePosition, int newTilePosition, int offset)
        {
            if (offset == 1 && (tilePosition % this.stateSize == this.stateSize - 1) ||
            (offset == -1 && tilePosition % this.stateSize == 0) ||
            (newTilePosition < 0 || newTilePosition >= this.stateSize * this.stateSize))
            { 
                return false;
            }
            return true;
        }

        private void Swap(int[] state, int tilePosition, int newTilePosition)
        {
            int temp = state[tilePosition];
            state[tilePosition] = state[newTilePosition];
            state[newTilePosition] = temp;
        }
    }
}
