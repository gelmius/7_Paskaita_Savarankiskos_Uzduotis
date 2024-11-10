
class Knyga
{
    public int KnygosID { get; set; }
    public string Pavadinimas { get; set; }
    public string Autorius { get; set; }
    public int IsleidimoMetai { get; set; }
    public string Zanras { get; set; }

    public Knyga(int knygosID, string pavadinimas, string autorius, int isleidimoMetai, string zanras)
    {
        KnygosID = knygosID;
        Pavadinimas = pavadinimas;
        Autorius = autorius;
        IsleidimoMetai = isleidimoMetai;
        Zanras = zanras;
    }
}

class Program
{
    static void Main()
    {
        List<Knyga> knygos = NuskaitytiKnygasIsFailo("knygos.txt");

        bool running = true;
        while (running)
        {
            Console.WriteLine("\nPasirinkite veiksmą:");
            Console.WriteLine("1. Rodyti knygas pagal autorių");
            Console.WriteLine("2. Rodyti knygas pagal metus");
            Console.WriteLine("3. Rodyti klasikos žanro knygas");
            Console.WriteLine("4. Baigti programą");

            Console.Write("Įveskite pasirinkimo numerį: ");
            int pasirinkimas = int.Parse(Console.ReadLine());

            switch (pasirinkimas)
            {
                case 1:
                    RodytiKnygasPagalAutoriu(knygos);
                    break;
                case 2:
                    RodytiKnygasPagalMetus(knygos);
                    break;
                case 3:
                    RodytiKlasikosZanroKnygas(knygos);
                    break;
                case 4:
                    running = false;
                    Console.WriteLine("Programa baigiama.");
                    break;
                default:
                    Console.WriteLine("Neteisingas pasirinkimas. Bandykite dar kartą.");
                    break;
            }
        }
    }

    static List<Knyga> NuskaitytiKnygasIsFailo(string failoPavadinimas)
    {
        List<Knyga> knygos = new List<Knyga>();
        try
        {
            foreach (var eilute in File.ReadAllLines(failoPavadinimas))
            {
                var dalys = eilute.Split(',');
                if (dalys.Length == 5)
                {
                    int knygosID = int.Parse(dalys[0]);
                    string pavadinimas = dalys[1];
                    string autorius = dalys[2];
                    int isleidimoMetai = int.Parse(dalys[3]);
                    string zanras = dalys[4];
                    knygos.Add(new Knyga(knygosID, pavadinimas, autorius, isleidimoMetai, zanras));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Klaida skaitant failą: {ex.Message}");
        }
        return knygos;
    }

    static void RodytiKnygasPagalAutoriu(List<Knyga> knygos)
    {
        Console.Write("Įveskite autoriaus vardą ar dalį jo vardo: ");
        string autorius = Console.ReadLine().ToLower();

        var rezultatai = knygos.Where(k => k.Autorius.ToLower().Contains(autorius)).ToList();

        if (rezultatai.Any())
        {
            Console.WriteLine("\nRastos knygos:");
            foreach (var knyga in rezultatai)
            {
                Console.WriteLine($"{knyga.Pavadinimas} ({knyga.Autorius}, {knyga.IsleidimoMetai})");
            }
        }
        else
        {
            Console.WriteLine("Knygų pagal nurodytą autorių nerasta.");
        }
    }

    static void RodytiKnygasPagalMetus(List<Knyga> knygos)
    {
        Console.Write("Įveskite metus: ");
        int metai = int.Parse(Console.ReadLine());

        var rezultatai = knygos.Where(k => k.IsleidimoMetai >= metai).ToList();

        if (rezultatai.Any())
        {
            Console.WriteLine($"\nKnygos, išleistos {metai} metais ar vėliau:");
            foreach (var knyga in rezultatai)
            {
                Console.WriteLine($"{knyga.Pavadinimas} ({knyga.Autorius}, {knyga.IsleidimoMetai})");
            }
        }
        else
        {
            Console.WriteLine("Knygų, išleistų nurodytais metais ar vėliau, nerasta.");
        }
    }

    static void RodytiKlasikosZanroKnygas(List<Knyga> knygos)
    {
        var klasikosKnygos = knygos.Where(k => k.Zanras.Equals("Classic", StringComparison.OrdinalIgnoreCase)).ToList();

        if (klasikosKnygos.Any())
        {
            Console.WriteLine("\nKlasikos žanro knygos:");
            foreach (var knyga in klasikosKnygos)
            {
                Console.WriteLine($"{knyga.Pavadinimas} ({knyga.Autorius}, {knyga.IsleidimoMetai})");
            }
        }
        else
        {
            Console.WriteLine("Klasikos žanro knygų nerasta.");
        }
    }
}
