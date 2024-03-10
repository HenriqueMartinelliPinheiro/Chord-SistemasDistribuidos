using System.ComponentModel.DataAnnotations;
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

            Menu(nodes, numberNodes);

        //    foreach (Node node in nodes)
        //    {
        //        Console.WriteLine("ID: " + node.Id + " Status: " + node.Status +
        //        " Next Node ID: " + node.NextNode.Id + " Previous Node ID: " + node.PreviousNode.Id);

        //        if (node.NextActiveNode!=null)
        //        {
        //            Console.WriteLine("Próximo Nodo Ativo ID:" + node.NextActiveNode.Id);
        //        }

        //        if (node.PreviousActiveNode!=null)
        //        {
        //            Console.WriteLine("Anterior Nodo Ativo ID:" + node.PreviousActiveNode.Id);
        //        }
        //        Console.WriteLine("\n\n");
        //    }
        }

        static void Menu(List<Node> nodes, int numberNodes)
        {
            int op = -1;
            while (op != 0)
            {
                Console.WriteLine("1 - Encontrar Recurso");
                Console.WriteLine("2 - Ativar Nodo");
                Console.WriteLine("3 - Desativar Nodo");
                Console.WriteLine("0 - Sair");
                op = int.Parse(Console.ReadLine());

                switch (op)
                {
                    case 1:
                        Console.WriteLine("Digite o recurso: ");
                        string value = Console.ReadLine();

                        (int id, value) = FindResource(value, nodes, numberNodes);

                        if (id != 0)
                        {
                            Console.WriteLine("O recurso: " + value + " foi encontrado no nodo de ID: " + id);
                        }
                        else
                        {
                            Console.WriteLine("Recurso não existe.");
                        }

                        break;

                    case 2:
                        Console.WriteLine("Digite o ID do Nodo que você deseja desativar: ");
                        int idDisable = int.Parse(Console.ReadLine());

                        DisableNode(idDisable, nodes);

                        break;

                    case 3:
                        Console.WriteLine("Digite o ID do Nodo que você deseja ativar: ");
                        int idEnable = int.Parse(Console.ReadLine());

                        EnableNode(idEnable, nodes);

                        break;
                    default:
                        break;
                }
            }
        }

        static void DisableNode(int id, List<Node> nodes)
        {
            Node node = nodes.Find(node => node.Id == id);

            if(node != null) {
                node.Status = false;
            }
            node.NextActiveNode = null;
            node.PreviousActiveNode = null; 
            DefineActiveNodes(nodes);
        }

        static void EnableNode(int id, List<Node> nodes)
        {
            Node node = nodes.Find(node => node.Id == id);

            if (node != null)
            {
                node.Status = true;
            }
            DefineActiveNodes(nodes);
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
            DistributeResources(nodes, resources);
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

        static int FindResourceLocation(string value, List<Node> nodes, int numberNodes)
        {
            int id = 0;
            Resource resource = new Resource(value, numberNodes);
            foreach (Node node in nodes)
            {
                if (node.Status)
                {
                    id = node.GetNodeReference(resource);
                    if (id!=0)
                    {
                        return id;
                    }
                }
            }
            return id;
        }

        static (int, string) FindResource(string value, List<Node> nodes, int numberNodes)
        {
            int nodeId = FindResourceLocation(value, nodes, numberNodes);

            if (nodeId != 0)
            {
                Node node = nodes.Find(node => node.Id == nodeId);
                foreach (var resource in node.Resource)
                {
                    if (resource.Key == value)
                    {
                        return (resource.Value, resource.Key);
                    }
                }
            }
            return (0, value);
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

        static void DistributeResources(List<Node> nodes, List<Resource> resources)
        {
            foreach (Resource resource in resources)
            {
                Node node = GetNodeById(resource.Hash, nodes);
                node.AddResource(resource.Hash, resource.Value);
                //Console.WriteLine("ID: " + node.Id + 
                //" Recurso: " + resource.Value + " Hash: " + resource.Hash);
            }
        }

        static void GetNodesReferences(List<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                if (node.Status && node.PreviousNode != null)
                {
                    node.AddNodeReferencence(node);
                    node.NodeReference = node.PreviousNode.GetPreviousNodeReference(node.NodeReference);
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