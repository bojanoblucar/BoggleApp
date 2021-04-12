using System;
using System.Collections.Generic;
using System.IO;
using BoggleApp.Game.Analysis;
using Newtonsoft.Json;
using Xunit;

namespace BoggleApp.Tests
{
    public class BoggleAnalyzerTest
    {
        [Fact]
        public void TestBoggleAnalyzer()
        {
            var array = new string[16] { "a", "b", "c", "d",
                                         "e", "f", "g", "h",
                                         "i", "j", "k", "l",
                                         "m", "nj", "o", "lj" };

            var analyzer = BoggleAnalyser.CreateForSolution(array);

            Assert.False(analyzer.IsSolution("afgg"));
            Assert.True(analyzer.IsSolution("aefjkgcd"));
            Assert.True(analyzer.IsSolution("gcdhkonjmiea"));
            Assert.False(analyzer.IsSolution("gcdhkonmiea"));
            Assert.False(analyzer.IsSolution("gcdhkonmieae"));
            Assert.True(analyzer.IsSolution("ljonj"));
            Assert.True(analyzer.IsSolution("ljol"));
            Assert.True(analyzer.IsSolution("mie"));
        }


        [Fact]
        public void TestBoggleAnalyzer2()
        {
            var array = new string[16] { "l", "m", "j", "t",
                                         "v", "h", "u", "o",
                                         "o", "s", "d", "e",
                                         "o", "i", "s", "i" };

            var analyzer = BoggleAnalyser.CreateForSolution(array);

            Assert.True(analyzer.IsSolution("oov"));
 
        }

        [Fact]
        public void TestBoggleTree()
        {
            var array = new string[16] { "a", "b", "c", "d",
                                         "e", "f", "g", "h",
                                         "i", "j", "k", "l",
                                         "m", "n", "o", "p" };

            var boggleTree = new BoggleTree();
            boggleTree.InitializeTree(array);

            var node = boggleTree.GetNode(0, 0);
            Assert.Equal("a", node.Value);
            Assert.Equal(3, node.Children.Count);
        }
    }
}
