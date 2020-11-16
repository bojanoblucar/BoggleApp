using System;
using System.Collections.Generic;
using System.Linq;

namespace BoggleApp.Shared.Analysis
{
    public class BoggleTree
    {
        public BoggleTree()
        {
            SolutionPaths = new List<Node>();
        }

        public List<Node> SolutionPaths { get; set; }

        public void AddSolutionPath(Node solutionPath)
        {
            SolutionPaths.Add(solutionPath);
        }

        public List<Node> GetSolutionPaths(string firstLetter)
        {
            return SolutionPaths.Where(n => n.Value == firstLetter).ToList();
        }
        
        public bool IsSolution(string candidate)
        {
            return IsSolution(0, candidate, SolutionPaths);
        }

        private bool IsSolution(int index, string candidate, List<Node> nodes)
        {
            if (!nodes.Any()) return false;

            var letter = candidate[index];
            var solutionPaths = nodes.Where(c => c.Value == letter.ToString()).ToList();
            if (solutionPaths.Any())
            { 
                if (index == candidate.Length - 1)
                    return true;
                else
                {
                    bool isSolution = false;
                    foreach (var path in solutionPaths)
                    {
                        isSolution = isSolution || IsSolution(index + 1, candidate, path.Children);
                    }

                    return isSolution;
                }
            }

            return false;

            
        }
    }
}
