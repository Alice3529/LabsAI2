using System;
using System.Collections.Generic;

namespace PuzzleGame
{
    class UniformedSearch
    {
        bool isStepByStepMode = default;

        public UniformedSearch(bool stepByStepMode)
        {
            this.isStepByStepMode = stepByStepMode;
        }

        public List<Node> AStar(Node root, string heuristicFunction, out int stepCount, out int memoryComplexity)
        {
            stepCount = 0;
            memoryComplexity = 1;
            List<Node> pathToSolution = new List<Node>();
            List<Node> visitedNodes = new List<Node>();
            List<Node> newNodes = new List<Node>();
            SortedList<int, List<Node>> priorityQueue = new SortedList<int, List<Node>>();
            HashSet<string> visitedStates = new HashSet<string>();

            this.AddToPriorityQueue(priorityQueue, root, root.nodeCosts.f);

            bool isGoalFound = false;

            while (priorityQueue.Count > 0 && !isGoalFound)
            {
                visitedNodes.Clear();
                newNodes.Clear();

                Node currentNode = ExtractMin(priorityQueue);
                stepCount++;

                string currentState = string.Join(",", currentNode.state);
                visitedStates.Add(currentState);

                if (currentNode.GoalTest())
                {
                    Console.WriteLine("Целевое состояние достигнуто");
                    isGoalFound = true;
                    this.PathTracer(pathToSolution, currentNode);
                    break;
                }

                currentNode.Expand(heuristicFunction);

                memoryComplexity += currentNode.children.Count;

                foreach (Node child in currentNode.children)
                {
                    string childState = string.Join(",", child.state);

                    if (visitedStates.Contains(childState))
                    {
                        visitedNodes.Add(child);
                        continue;
                    }
                    else
                    {
                        newNodes.Add(child);
                    }

                    this.AddToPriorityQueue(priorityQueue, child, child.nodeCosts.f);
                }

                this.LogInfo(newNodes, visitedNodes, priorityQueue, stepCount, currentNode);
            }

            return pathToSolution;
        }

        private void AddToPriorityQueue(SortedList<int, List<Node>> priorityQueue, Node node, int cost)
        {
            if (!priorityQueue.ContainsKey(cost))
            {
                priorityQueue[cost] = new List<Node>();
            }
            priorityQueue[cost].Add(node);
        }

        private Node ExtractMin(SortedList<int, List<Node>> priorityQueue)
        {
            int minKey = priorityQueue.Keys[0];
            List<Node> nodesWithMinKey = priorityQueue[minKey];

            Node minNode = nodesWithMinKey[0];
            nodesWithMinKey.RemoveAt(0);

            if (nodesWithMinKey.Count == 0)
            {
                priorityQueue.Remove(minKey);
            }

            return minNode;
        }


        private void LogInfo(List<Node> newNodes, List<Node> repeatNodes, SortedList<int, List<Node>> border, int stepCount, Node currentNode)
        {
            if (!this.isStepByStepMode) { return; }

            Console.WriteLine("\nШаг " + stepCount.ToString());
            Console.WriteLine("\nТекущая вершина:");

            currentNode.PrintConsole();

            Console.WriteLine("");

            this.LogNodesInfo("Вновь найденные вершины: ", newNodes);
            this.LogNodesInfo("Выявленные повторные вершины: ", repeatNodes);

            Console.WriteLine("\nТекущее состояние каймы:");

            foreach (var key in border.Keys)
            {
                foreach (var node in border[key])
                {
                    node.PrintConsole();
                }
                Console.WriteLine();
            }

            Console.WriteLine("\nЦелевое состояние не достигнуто");
            Console.WriteLine("\nНажмите любую клавишу для следующего шага...\n");

            Console.ReadKey();

        }

        private void LogNodesInfo(string message, List<Node> nodes)
        {
            if (nodes.Count == 0)
            {
                Console.WriteLine(message + "не найдено");
                return;
            }

            Console.WriteLine(message);

            foreach (var node in nodes)
            {
                node.PrintConsole();
            }

            Console.WriteLine();
        }

        private void PathTracer(List<Node> path, Node endNode)
        {
            Node node = endNode;
            path.Add(node);

            while (node.parent != null)
            {
                node = node.parent;
                path.Add(node);
            }

            path.Reverse();
        }
    }
}

