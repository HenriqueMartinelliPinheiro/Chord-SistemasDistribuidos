using System.Globalization;
using System.Xml.Linq;

namespace Chord
{
    public class Program
    {
        static void Main()
        {
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
            int numberNodes = 10;
            List<Node> nodes = new List<Node>();

            for (int i = 0; i < numberNodes; i++)
            {
                int status = 0;

                if (i % 5 == 0)
                {
                    status = 1;
                }
                nodes.Add(new Node(i + 1, status));
            }

            List<Resource> resources = FillResourcesDictionary(animals, numberNodes);

            foreach (Node node in nodes)
            {
                foreach (Resource resource in resources)
                {
                    if (node.Id == resource.Hash)
                    {
                        node.AddResource(resource.Hash, resource.Value);
                        Console.WriteLine("ID: " + node.Id + " Status: " + node.Status +
                       " Recurso: " + resource.Value + " Hash: " + resource.Hash);
                    }
                }
            }
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
