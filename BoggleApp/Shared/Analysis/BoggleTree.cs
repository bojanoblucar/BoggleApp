using System;
using System.Collections.Generic;
using System.Linq;

namespace BoggleApp.Shared.Analysis
{
    public class BoggleTree
    {
        private readonly int rowSize = 4;
        private readonly int columnSize = 4;

        public BoggleTree()
        {
            SolutionPaths = new List<Node>();
            InitializeBoggleTree(4, 4);
        }

        private void InitializeBoggleTree(int rowSize, int columnSize)
        {
            List<Node> nodes = new List<Node>();
            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < columnSize; j++)
                {
                    nodes.Add(new Node(i, j));
                }
            }

            SolutionPaths = nodes;
            CreatePaths();
        }


        private void CreatePaths()
        {
            foreach (var node in SolutionPaths)
            {
                int row = node.Row;
                int column = node.Column;

                List<Node> Children = new List<Node>();
                Children.Add(FindChild(row, column + 1));
                Children.Add(FindChild(row, column - 1));
                Children.Add(FindChild(row - 1, column + 1));
                Children.Add(FindChild(row - 1, column));
                Children.Add(FindChild(row - 1, column - 1));
                Children.Add(FindChild(row + 1, column + 1));
                Children.Add(FindChild(row + 1, column));
                Children.Add(FindChild(row + 1, column - 1));

                node.Children = Children.Where(c => c != null).ToList();
            }
        }

        private Node FindChild(int row, int column)
        {
            return SolutionPaths.Where(n => n.Row == row && n.Column == column).FirstOrDefault();
        }

        public List<Node> SolutionPaths { get; private set; }

        public void InitializeTree(string [] solution)
        {
            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < columnSize; j++)
                {
                    GetNode(i,j).Value = solution[i * rowSize + j];
                }
            }
        }


        public Node GetNode(int row, int column)
        {
            return SolutionPaths.Where(n => n.Row == row && n.Column == column).First();
        }
        
        
    }
}
