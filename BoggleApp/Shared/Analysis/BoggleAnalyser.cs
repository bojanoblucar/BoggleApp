using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BoggleApp.Shared.Analysis
{
 
    public interface ISolutionChecker
    {
        bool IsSolution(string candidate);
    }


    public class BoggleAnalyser : ISolutionChecker
    {
        private readonly BoggleTree boggleTree;

        public static BoggleAnalyser CreateForSolution(string[] solution)
        {
            return new BoggleAnalyser(solution);
        }

        private BoggleAnalyser(string[] solution)
        {
            if (boggleTree == null)
                boggleTree = new BoggleTree();

            boggleTree.InitializeTree(solution);
        }

        public bool IsSolution(string candidate)
        {
            return IsSolution(new SolutionCandidate(candidate), boggleTree.SolutionPaths, new List<Node>());
        }

        private bool IsSolution(SolutionCandidate candidate, List<Node> paths, List<Node> visited)
        {
            if (!candidate.HasNextLetter())
                return true;

            var letter = candidate.GetNextLetter();
            var candidates = paths.Where(n => n.Value.Equals(letter, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!candidates.Any())
                return false;

            foreach (var c in candidates)
            {
                var newVisited = new List<Node>(visited)
                {
                    c
                };

                var availablePaths = c.Children.Where(p => !newVisited.Any(v => v.IsEqualPositionAs(p))).ToList();
                if (IsSolution(candidate, availablePaths, newVisited)) return true;
            }

            return false;
        }
    }
}
