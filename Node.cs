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
        /// <summary>
        /// adiciona recurso ao nodo
        /// </summary>
        /// <param name="hash">hash do recurso</param>
        /// <param name="resource">valor do recursos</param>
        public void AddResource(int hash, string resource)
        {
            Resource.Add(resource, hash);
        }

        /// <summary>
        /// adiciona uma referencia a um nodo pelo qual o nodo ativo é reposnável
        /// </summary>
        /// <param name="node">nodo a ser referenciado</param>
        public void AddNodeReferencence(Node node)
        {
            NodeReference.Add(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeReference">lista de nodos pelos quais o nodo ativo é reposnável</param>
        /// <returns> mapeia os nodos pelo qual o nodo ativo atual é reponsável</returns>
        public List<Node> GetPreviousNodeReference(List<Node> nodeReference)
        {
            if (this != null && !Status)
            {
                if (PreviousNode!=null)
                {
                    nodeReference = PreviousNode.GetPreviousNodeReference(nodeReference);
                }
                nodeReference.Add(this);
            }
            return nodeReference;
        }

        /// <summary>
        /// verifica se o nodo atual é responsável pelo nodo que contém o recurso procurado
        /// </summary>
        /// <param name="resource">recurso a ser procurado</param>
        /// <returns>id do nodo que contém o recurso</returns>
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

        /// <summary>
        /// define o próximo nodo ativo de um nodo ativo
        /// </summary>
        /// <returns>retorna o próximo nodo ativo do nodo ativo atual</returns>
        public Node DefineNextActiveNode()
        {
            if (NextNode != null && NextNode.Status)
            {
                return NextNode;
            }
            else if (NextNode != null)
            {
                return NextNode.DefineNextActiveNode();
            }
            return null;
        }

        /// <summary>
        /// deine o nodo ativo anterior do nodo atual
        /// </summary>
        /// <returns>retorna o nodo ativo anterior do nodo atual</returns>
        public Node DefinePreviousActiveNode()
        {
            if (PreviousNode != null && PreviousNode.Status)
            {
                return PreviousNode;
            }
            else if (NextNode != null)
            {
                return PreviousNode.DefinePreviousActiveNode();
            }
            return null;
        }
    }
}