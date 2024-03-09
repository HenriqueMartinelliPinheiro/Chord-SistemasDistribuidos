using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chord
{
    internal class Node
    {
        public int Id { get; }
        public bool Status { get; set; }
        public Node? NextNode { get; set; }
        public Node? PreviousNode { get; set; }
        public Node? NextActiveNode { get; set; }
        public Node? PreviousActiveNode { get; set; }

        public Dictionary<string, int> Resource { get; } //hash, value
        public List<Node> NodeReference { get; set; } // hashNode, idNode6

        public Node(int id, bool status)
        {
            Id = id;
            Resource = new Dictionary<string, int>();
            Status = status;
            NodeReference = new List<Node>();
            NextNode = null;
            PreviousNode = null;
            PreviousActiveNode = null;
            NextActiveNode = null;
        }

        public void AddResource(int hash, string resource)
        {
            Resource.Add(resource, hash);
        }

        public override string ToString()
        {
            return "ID: " + Id + " Status: " + Status;
        }

        public void AddNodeReferencence(Node node)
        {
            NodeReference.Add(node);
        }
    }
}
