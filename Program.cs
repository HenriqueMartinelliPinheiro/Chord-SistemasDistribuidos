using System.Globalization;

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

            foreach (Node node in nodes)
            {
                Console.WriteLine(node.ToString());
            }
            FillResourcesDictionary(animals, numberNodes);
        }
        public static Dictionary<string, int> FillResourcesDictionary(string[] vect, int numberNodes)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            foreach (string animal in vect)
            {
                if (!dictionary.ContainsKey(animal))
                {
                    dictionary.Add(animal, GetHash(animal, numberNodes));
                    Console.WriteLine("Hash: " + GetHash(animal, numberNodes) + " Animal: " + animal);
                }
            }
            return dictionary;
        }
        private static int GetHash(string value, int numberNodes)
        {
            int h = 0;
            for (int i = 0; i < value.Length; i++)
            {
                h += value[i];
            }
            return (h*31)%numberNodes;
        }
    }

}
