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
            GetNodesReferences(nodes);

            Menu(nodes, numberNodes);

            foreach (Node node in nodes)
            {
                Console.WriteLine("ID: " + node.Id + " Status: " + node.Status +
                " Next Node ID: " + node.NextNode.Id + " Previous Node ID: " + node.PreviousNode.Id);

                if (node.NextActiveNode != null)
                {
                    Console.WriteLine("Próximo Nodo Ativo ID:" + node.NextActiveNode.Id);
                }

                if (node.PreviousActiveNode != null)
                {
                    Console.WriteLine("Anterior Nodo Ativo ID:" + node.PreviousActiveNode.Id);
                }
                Console.WriteLine("\n\n");
            }
        }
       /// <summary>
       /// exibir interface
       /// </summary>
       /// <param name="nodes">lista de nodos do anel</param>
       /// <param name="numberNodes">numero de nodos do anel</param>
        static void Menu(List<Node> nodes, int numberNodes)
        {
            int op = -1;
            while (op != 0)
            {
                Console.WriteLine("1 - Encontrar Recurso");
                Console.WriteLine("2 - Desativar Nodo");
                Console.WriteLine("3 - Ativar Nodo");
                Console.WriteLine("4 - Adicionar Recurso");
                Console.WriteLine("0 - Sair");
                op = int.Parse(Console.ReadLine());

                switch (op)
                {
                    case 1:
                        Console.WriteLine("Digite o recurso: ");
                        string value = Console.ReadLine();
                        if (value!=null)
                        {
                            (int id, value) = FindResource(value, nodes, numberNodes);

                            if (id != 0)
                            {
                                Console.WriteLine("O recurso: " + value + " foi encontrado no nodo de ID: " + id);
                            }
                            else
                            {
                                Console.WriteLine("Recurso não existe.");
                            }
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

                    case 4:
                        Console.WriteLine("Digite o recurso a ser inserido.");
                        string addedValue = Console.ReadLine();
                        if (addedValue!=null)
                        {
                            Resource resource = new Resource(addedValue, numberNodes);
                            AddResource(GetNodeById(resource.Hash, nodes), resource.Hash, addedValue);
                        }

                        break;

                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// desativar nodo ativo do anel
        /// </summary>
        /// <param name="id">id do nodo a ser desativado</param>
        /// <param name="nodes">lista de nodos do anel</param>
        static void DisableNode(int id, List<Node> nodes)
        {
            Node node = nodes.Find(node => node.Id == id);

            if (node != null)
            {
                node.Status = false;
                node.NextActiveNode = null;
                node.PreviousActiveNode = null;
                DefineActiveNodes(nodes);
            }
        }
        /// <summary>
        /// ativar nodo desativado do anel
        /// </summary>
        /// <param name="id">id do nodo a ser ativado</param>
        /// <param name="nodes">lista de nodos do anel</param>
        static void EnableNode(int id, List<Node> nodes)
        {
            Node node = nodes.Find(node => node.Id == id);
            if (node != null)
            {
                node.Status = true;
                DefineActiveNodes(nodes);
            }
            
        }

        /// <summary>
        /// inicializa a lista de nodos do anel, define os nodos sucessores e predecessores de cada um, preenche os nodos com
        /// recursos, e define quais serão os nodos iniciais, além dos ponteiros dos próximos nodos ativos
        /// </summary>
        /// <param name="nodes">lista de nodos do anel</param>
        /// <param name="numberNodes">numero de nodos do anel</param>
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
            DefineActiveNodes(nodes);
        }

        /// <summary>
        /// Definir nodos os ponteiros dos nodos ativos para os próximos nodos ativos
        /// </summary>
        /// <param name="nodes">lista de nodos do anel</param>
        static void DefineActiveNodes(List<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                if (node.Status)
                {
                    node.NextActiveNode = node.DefineNextActiveNode();
                    node.PreviousActiveNode = node.DefinePreviousActiveNode();
                }
            }
        }
        /// <summary>
        /// encontra em qual nodo um recurso está localizado
        /// </summary>
        /// <param name="value">recurso a ser buscado</param>
        /// <param name="nodes">lista de nodos do anel</param>
        /// <param name="numberNodes">numero de nodos do anel</param>
        /// <returns>id do nodo onde o recurso está localizado</returns>
        static int FindResourceLocation(string value, List<Node> nodes, int numberNodes)
        {
            int id = 0;
            Resource resource = new Resource(value, numberNodes);
            foreach (Node node in nodes)
            {
                if (node.Status)
                {
                    id = node.GetNodeReference(resource);
                    if (id != 0)
                    {
                        return id;
                    }
                }
            }
            return id;
        }
        /// <summary>
        /// encontra o recurso no nodo em que ele está localizado
        /// </summary>
        /// <param name="value">recurso a ser buscado</param>
        /// <param name="nodes">lista de nodos do anel</param>
        /// <param name="numberNodes"> numero de nodos do anel</param>
        /// <returns>retorna o valor hash do recurso e e seu valor</returns>
        static (int, string) FindResource(string value, List<Node> nodes, int numberNodes)
        {
            int nodeId = FindResourceLocation(value, nodes, numberNodes);

            if (nodeId != 0)
            {
                Node node = nodes.Find(node => node.Id == nodeId);
                if (node!=null)
                {
                    foreach (var resource in node.Resource)
                    {
                        if (resource.Key == value)
                        {
                            return (resource.Value, resource.Key);
                        }
                    }
                }
            }
            return (0, value);
        }
        /// <summary>
        /// adiciona os recuros para os seus respectivos nodos, de acordo com o hash do recurso e id do nodo
        /// </summary>
        /// <param name="nodes">lista de nodos do anel</param>
        /// <param name="resources">lista de recursos a serem distribuidos entre os nodos</param>
        static void DistributeResources(List<Node> nodes, List<Resource> resources)
        {
            foreach (Resource resource in resources)
            {
                Node node = GetNodeById(resource.Hash, nodes);
                AddResource(node, resource.Hash, resource.Value);
                //Console.WriteLine("ID: " + node.Id + 
                //" Recurso: " + resource.Value + " Hash: " + resource.Hash);
            }
        }
        /// <summary>
        /// adiciona um recurso para um nodo
        /// </summary>
        /// <param name="node">nodo ao qual o recurso será adicionado</param>
        /// <param name="hash">hash do recurso que também é o id do nodo ao qual o recurso será adicionado</param>
        /// <param name="value">valor do recurso</param>
        static void AddResource(Node node, int hash, string value)
        {
            node.AddResource(hash, value);
        }

        /// <summary>
        /// Adiciona a lista de nodos que um nodo ativo é responsável por indexar a pesquisa
        /// </summary>
        /// <param name="nodes">lista de nodos do anel</param>
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
        /// <summary>
        /// retorna um nodo caso exista um com o id informado.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        static Node GetNodeById(int id, List<Node> nodes)
        {
            return nodes.Find(node => node.Id == id);
        }
        /// <summary>
        /// preenche uma lista com os recursos, incluindo seu valor e hash
        /// </summary>
        /// <param name="vect">vetor de valores de recursos</param>
        /// <param name="numberNodes">numero de nodos do anel</param>
        /// <returns>retorna a lista de recursos, com o valor e hash do recurso</returns>
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