using System.Globalization;
using System.Resources;
using System.Xml.Linq;

namespace Chord
{
    public class Program
    {
        static void Main()
        {
            int numberNodes = 10;
            List<Node> nodes;

            InitializateList(out nodes, numberNodes);
            DefineActiveNodes(nodes);
            GetNodesReferences(nodes);

            foreach (Node node in nodes)
            {
                Console.WriteLine(" ID: "+ node.Id);
                foreach (var nodeReference in node.NodeReference)
                {
                    Console.WriteLine(" Node Reference ID: " + nodeReference.Id);
                }
                Console.WriteLine("\n");
                //foreach (var resource in node.Resource)
                //{

                //    //Console.Write("ID: " + node.Id + " Status: " + node.Status +
                //    //" Recurso: " + resource.Key + " Hash: " + resource.Value + " Next Node ID: " + node.NextNode.Id + " Previous Node ID: " + node.PreviousNode);

                //    //if (node.NextActiveNode != null)
                //    //{
                //    //    Console.Write(" Next Active Node ID: " + node.NextActiveNode.Id);
                //    //}

                //    //if (node.PreviousActiveNode != null)
                //    //{
                //    //    Console.Write(" Previous Active Node ID: " + node.PreviousActiveNode.Id);
                //    }
                //    Console.WriteLine("\n");
                //}
                //Console.WriteLine("\n\n\n");
            }
        }

        static void InitializateList(out List<Node> nodes, int numberNodes)
        {
            nodes = new List<Node>();

            string[] animals = {
                "Cachorro", "Gato", "Cavalo", "Vaca", "Porco",
                "Ovelha", "Galinha", "Pato", "Coelho", "Rato",
                "Tigre", "Leão", "Elefante", "Girafa", "Macaco",
                "Panda", "Lobo", "Raposa", "Urso", "Coala",
                "Hipopótamo", "Crocodilo", "Tartaruga", "Cobra",
                "Salamandra", "Sapo", "Rã", "Peixe", "Tubarão",
                "Golfinho", "Baleia", "Pinguim", "Avestruz",
                "Morcego", "Borboleta", "Abelha", "Formiga",
                "Joaninha", "Libélula", "Lagarta", "Serpente", "Lontra"
            };
            Node previousNode = null;
            Node firstNode = null;
            for (int i = 0; i < numberNodes; i++)
            {
                bool status = false;

                if ((i + 1) % 3 == 0)
                {
                    status = true;
                }
                Node node = new Node(i + 1, status);
                nodes.Add(node);
                node.PreviousNode = previousNode;

                if (previousNode != null)
                {
                    previousNode.NextNode = node;
                }
                previousNode = node;

                if (i == 0)
                {
                    firstNode = node;
                }

                if (i == numberNodes - 1 && firstNode != null)
                {
                    node.NextNode = firstNode;
                    firstNode.PreviousNode = node;
                }
            }

            List<Resource> resources = FillResourcesDictionary(animals, numberNodes);
            distributeResources(nodes, resources);
        }

        static void DefineActiveNodes(List<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                if (node.Status)
                {
                    node.NextActiveNode = DefineNextActiveNode(node);
                    node.PreviousActiveNode = DefinePreviousActiveNode(node);
                }
            }
        }

        static Node DefineNextActiveNode(Node node)
        {
            if (node.NextNode != null && node.NextNode.Status)
            {
                return node.NextNode;
            }
            else if (node.NextNode != null)
            {
                return DefineNextActiveNode(node.NextNode);
            }
            return null;
        }

        static Node DefinePreviousActiveNode(Node node)
        {
            if (node.PreviousNode != null && node.PreviousNode.Status)
            {
                return node.PreviousNode;
            }
            else if (node.NextNode != null)
            {
                return DefinePreviousActiveNode(node.PreviousNode);
            }
            return null;
        }

        static void distributeResources(List<Node> nodes, List<Resource> resources)
        {
            foreach (Resource resource in resources)
            {
                Node node = GetNodeById(resource.Hash, nodes);
                node.AddResource(resource.Hash, resource.Value);
                //Console.WriteLine("ID: " + node.Id + " Status: " + node.Status +
                //" Recurso: " + resource.Value + " Hash: " + resource.Hash + " Next Node ID: " + node.NextNode.Id + " Previous Node ID: " + node.PreviousNode);
            }
        }

        static void GetNodesReferences(List<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                if (node.Status && node.PreviousNode!=null)
                {
                    node.AddNodeReferencence(node);
                    node.NodeReference = GetPreviousNodeReference(node.NodeReference,node.PreviousNode);
                }
            }
        }

        static List<Node> GetPreviousNodeReference(List<Node> nodeReference, Node node)
        {
            if (node!= null && !node.Status)
            {
                nodeReference.Add(node);
                nodeReference = GetPreviousNodeReference(nodeReference, node.PreviousNode);
            }
            return nodeReference;
        }

        static Node GetNodeById(int id, List<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                if (node.Id == id)
                {
                    return node;
                }
            }
            return null;
        }

        static List<Resource> FillResourcesDictionary(string[] vect, int numberNodes)
        {
            List<Resource> resources = new List<Resource>();
            foreach (string animal in vect)
            {
                resources.Add(new Resource(animal, numberNodes));
            }
            return resources;
        }
    }
}