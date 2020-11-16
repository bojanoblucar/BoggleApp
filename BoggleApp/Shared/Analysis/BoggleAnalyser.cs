using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BoggleApp.Shared.Analysis
{
    public static class SolutionMatrixComposer
    {
        public static string[,] ComposeMatrix(string [] array, int rowSize, int columnSize)
        {
            var solutionMatrix = new string[rowSize, columnSize];

            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < columnSize; j++)
                {
                    solutionMatrix[i, j] = array[i * rowSize + j];
                }
            }

            return solutionMatrix;
        }

        public static ReferencedNode[,] ComposeMatrix(int rowSize, int columnSize)
        {
            var solutionMatrix = new ReferencedNode[rowSize, columnSize];

            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < columnSize; j++)
                {
                    solutionMatrix[i, j] = new ReferencedNode();
                }
            }

            return solutionMatrix;
        }
    }


    public class ReferencedNode
    {
        public string Value { get; set; }
    }


    public class AnalyzerBuilder
    {
        private ReferencedNode[,] solutionMatrix;

        private readonly BoggleTree boggleTree;

        readonly int columnSize = 4;
        readonly int rowSize = 4;

        private AnalyzerBuilder()
        {
            solutionMatrix = SolutionMatrixComposer.ComposeMatrix(4,4);
            this.boggleTree = new BoggleTree();
            InitializeBoggleTree();
        }

        private static AnalyzerBuilder _instance = null;

        public static AnalyzerBuilder GetInstance()
        {
            if (_instance == null)
                _instance = new AnalyzerBuilder();

            return _instance;
        }

        public BoggleAnalyser ForSolution(string[] solution)
        {
            InitializeMatrix(solution);
            return new BoggleAnalyser(boggleTree);
        }

        private void InitializeMatrix(string [] solution)
        {
            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < columnSize; j++)
                {
                    solutionMatrix[i, j].Value = solution[i * rowSize + j];
                }
            }
        }


        private void InitializeBoggleTree()
        {
            List<(int, int)> loop = new List<(int, int)>();
            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < columnSize; j++)
                {
                    loop.Add((i, j));
                }
            }

            Parallel.For(0, loop.Count, (index) =>
            {
                int i = loop[index].Item1;
                int j = loop[index].Item2;
                Node mainNode = new Node(solutionMatrix[i,j], i, j);
                CreatePath(mainNode);
                boggleTree.AddSolutionPath(mainNode);
            });

            //var jsonNode = JsonConvert.SerializeObject(boggleTree.SolutionPaths);
            //File.WriteAllText("boggleTree.txt", jsonNode);

        }


        private void CreatePath(Node parent)
        {
            int row = parent.Row;
            int column = parent.Column;

            List<Node> Children = new List<Node>();
            FindChild(row, column + 1, parent, Children);
            FindChild(row, column - 1, parent, Children);
            FindChild(row - 1, column + 1, parent, Children);
            FindChild(row - 1, column, parent, Children);
            FindChild(row - 1, column - 1, parent, Children);
            FindChild(row + 1, column + 1, parent, Children);
            FindChild(row + 1, column, parent, Children);
            FindChild(row + 1, column - 1, parent, Children);

            parent.Children = Children;
            //path.Add(parent);
            foreach (var child in Children)
            {
                child.Parent = parent;
                //if (parent.Value == "a")
                //Console.Write("a");
                CreatePath(child);
            }
        }

        private void FindChild(int row, int column, Node parent, List<Node> Children)
        {
            Node node = null;
            if (column < columnSize && column >= 0
                && row < rowSize && row >= 0
                && !IsInPath(parent, row, column))
            {
                node = new Node(solutionMatrix[row, column], row, column);
            }

            if (node != null)
                Children.Add(node);
        }


        private bool IsInPath(Node parent, int row, int column)
        {
            if (parent == null) return false;

            if (parent.Column == column && parent.Row == row)
                return true;

            return IsInPath(parent.Parent, row, column);
        }
    }



    public class BoggleAnalyser
    {
        private readonly BoggleTree boggleTree;

        public BoggleAnalyser(BoggleTree boggleTree)
        {
            this.boggleTree = boggleTree;
        }

        public bool IsSolution(string candidate)
        {
            return boggleTree.IsSolution(candidate);
        }
    }
}
