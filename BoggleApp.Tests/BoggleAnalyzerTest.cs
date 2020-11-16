using System;
using System.Collections.Generic;
using System.IO;
using BoggleApp.Shared.Analysis;
using Newtonsoft.Json;
using Xunit;

namespace BoggleApp.Tests
{
    public class BoggleAnalyzerTest
    {
        [Fact]
        public void TestsolutionMatrixComposer()
        {
            var array = new string[16] { "00", "01", "02", "03", "10", "11", "12", "13", "20", "21", "22", "23", "30", "31", "32", "33" };

            int rowSize = 4;
            int columnSize = 4;
            var matrix = SolutionMatrixComposer.ComposeMatrix(array, rowSize, columnSize);

            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < columnSize; j++)
                {
                    Assert.Equal($"{i}{j}", matrix[i, j]);
                }
            }
        }

        [Fact]
        public void TestBoggleAnalyzer()
        {
            var array = new string[16] { "a", "b", "c", "d",
                                         "e", "f", "g", "h",
                                         "i", "j", "k", "l",
                                         "m", "n", "o", "p" };

            AnalyzerBuilder bulder = AnalyzerBuilder.GetInstance();
            var analyzer = bulder.ForSolution(array);

            Assert.False(analyzer.IsSolution("afgg"));
            Assert.True(analyzer.IsSolution("aefjkgcd"));
            Assert.True(analyzer.IsSolution("gcdhkonmiea"));
        }

        [Fact]
        public void DeserializeTree()
        {
            var json = File.ReadAllText("boggleTree.txt");
            var node = JsonConvert.DeserializeObject<List<Node>>(json);
            Assert.NotNull(node);
        }
    }
}
