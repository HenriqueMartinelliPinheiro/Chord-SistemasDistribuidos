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

            //FindNextNode(ref nodes);


        }

        //static void FindNextNode(ref List<Node> nodes)
        //{
        //    Node? firstActiveNode = nodes.Find(node => node.Status == 1);
        //    Node? previousNode = firstActiveNode;
        //    int i = 0;
        //    do
        //    {
        //        Node node = nodes[i];
        //        if (node.Status == 1 && node != firstActiveNode) //node ativo
        //        {
        //            if (previousNode != null)
        //            {
        //                previousNode.NextNode = node;
        //                previousNode = node;
        //            }
        //        }

        //        i++;
        //    } while (nodes.Count > i);

        //    if (previousNode != null)
        //    {
        //        previousNode.NextNode = firstActiveNode;
        //    }

        //    foreach (Node node in nodes)
        //    {
        //        if (node.NextNode != null)
        //        {
        //            Console.WriteLine("ID: " + node.Id + "Next Node: " + node.NextNode.Id);
        //        }
        //    }
        //}
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
                "Joaninha", "Libélula", "Lagarta"
            };
            Node previousNode =null;
            Node firstNode = null;
            for (int i = 0; i < numberNodes; i++)
            {
              
                int status = 0;

                if ((i + 1) % 3 == 0)
                {
                    status = 1;
                }
                Node node = new Node(i + 1, status);
                nodes.Add(node);
                node.PreviousNode = previousNode;

                if (previousNode!=null)
                {
                    previousNode.NextNode = node;
                }
                previousNode = node;

                if (i == 0)
                {
                    firstNode = node;
                }

                if (i==numberNodes-1 && firstNode!=null)
                {
                    node.NextNode = firstNode;
                    firstNode.PreviousNode = node;
                }
            }

            List<Resource> resources = FillResourcesDictionary(animals, numberNodes);
            distributeResources(ref nodes, resources);
        }

        static void distributeResources(ref List<Node> nodes, List<Resource> resources)
        {
            foreach (Resource resource in resources)
            {
                Node node = GetNodeById(resource.Hash, nodes);
                node.AddResource(resource.Hash, resource.Value);
                Console.WriteLine("ID: " + node.Id + " Status: " + node.Status +
               " Recurso: " + resource.Value + " Hash: " + resource.Hash + " Next Node ID: " + node.NextNode.Id + " Previous Node ID: " + node.PreviousNode);
            }
        }

        static void GetNodeReference(List<Node> nodes)
        {
            foreach(Node node in nodes)
            {
                if (node.Status==1)
                {
                    node.
                }
            }
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
