using System;
using System.Collections.Generic;

namespace PuzzleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] initialPuzzleState =
            {
                5, 8, 3,
                4, 0, 2,
                7, 6, 1,
            };

            string heuristicFunction = ChooseHeuristicFunction();
            string modeKey = ChooseMode();

            bool isStepByStepMode = modeKey.Equals("y");

            Node root = new Node(initialPuzzleState, 0, heuristicFunction);
            UniformedSearch uniformedSearch = new UniformedSearch(isStepByStepMode);

            List<Node> solution = new List<Node>();
            int stepCount = 0;
            int memoryComplexity = 0;

            solution = uniformedSearch.AStar(root, heuristicFunction, out stepCount, out memoryComplexity);

            if (solution.Count > 0)
            {
                Console.WriteLine($"Стоимость пути: {solution.Count - 1}");
                Console.WriteLine($"Количество шагов (временная сложность): {stepCount}");
                Console.WriteLine($"Количество единиц памяти (ёмкостная сложность): {memoryComplexity}");
            }
            else
            {
                Console.WriteLine("Решение не найдено.");
            }

            Console.ReadLine();
        }

        private static string ChooseHeuristicFunction()
        {
            while (true)
            {
                Console.Write("Выберите эвристическую функцию 1 - h1, 2 - h2?: ");
                string key = Console.ReadLine();

                if (key.ToLower() == "1"
                    || key.ToLower() == "2")
                {
                    return key.ToLower();
                }

                Console.WriteLine("Неверный ввод. Попробуйте снова.");
            }
        }

        private static string ChooseMode()
        {
            while (true)
            {
                Console.Write("Включить пошаговый режим (y/n)?: ");
                string key = Console.ReadLine();

                if (key.ToLower() == "y"
                    || key.ToLower() == "n")
                {
                    return key.ToLower();
                }

                Console.WriteLine("Неверный ввод. Попробуйте снова.");
            }
        }
    }
}
