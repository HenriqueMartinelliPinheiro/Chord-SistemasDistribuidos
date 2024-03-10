using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public void AddNodeReferencence(Node node)
        {
            NodeReference.Add(node);
        }

        public List<Node> GetPreviousNodeReference(List<Node> nodeReference)
        {
            if (this != null && !Status)
            {
                if (this.PreviousNode!=null)
                {
                    nodeReference = this.PreviousNode.GetPreviousNodeReference(nodeReference);
                }
                nodeReference.Add(this);
            }
            return nodeReference;
        }

        public int GetNodeReference(Resource resource)
        {
            foreach (Node nodeReference in NodeReference)
            {
                if (nodeReference.Id == resource.Hash)
                {
                    return nodeReference.Id;
                }
            }
            return 0;
        }

        public void GetNodesReference()
        {
            if (Status && PreviousNode != null)
            {
                AddNodeReferencence(this);
                NodeReference =PreviousNode.GetPreviousNodeReference(NodeReference);
            }
        }
    }
}