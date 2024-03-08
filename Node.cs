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
        public int Status { get; set; }
        public Node? NextNode { get; set; }
        public Node? PreviousNode { get; set; }

        public Dictionary<string, int> Resource { get; } //hash, value
        private List<Node> NodeReference { get;} // hashNode, idNode6

        public Node(int id, int status)
        {
            Id = id;
            Resource = new Dictionary<string, int>();
            Status = status;
            NodeReference = new List<Node>();
            NextNode = null;
            PreviousNode = null;
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
