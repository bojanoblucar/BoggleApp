using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BoggleApp.Shared.Analysis
{
    public class Node
    {
        private readonly ReferencedNode referencedNode;

        public Node(ReferencedNode referencedNode, int row, int column)
        {
            this.referencedNode = referencedNode;
            Row = row;
            Column = column;
            Children = new List<Node>();
        }

        [JsonIgnore]
        public string Value => referencedNode.Value;

        public List<Node> Children { get; set; }

        [JsonIgnore]
        public Node Parent { get; set; }    

        public void AddChild(Node child)
        {
            Children.Add(child);
        }

        public int Row { get; set; }

        public int Column { get; set; }

    }
}
