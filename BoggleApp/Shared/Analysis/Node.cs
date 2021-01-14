using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BoggleApp.Shared.Analysis
{
    public class Node
    {
        public Node(int row, int column)
        {
            Row = row;
            Column = column;
            Children = new List<Node>();
        }

        [JsonIgnore]
        public string Value { get; set; }

        public List<Node> Children { get; set; } 

        public void AddChild(Node child)
        {
            Children.Add(child);
        }

        public int Row { get; set; }

        public int Column { get; set; }

        public bool IsEqualPositionAs(Node node)
        {
            return Row == node.Row && Column == node.Column;
        }

    }
}
