
class Klientas
{
    public int KlientoID { get; set; }
    public string Vardas { get; set; }
    public string Pavarde { get; set; }
    public string TelefonoNumeris { get; set; }
    public string ElPastas { get; set; }

    public Klientas(int klientoID, string vardas, string pavarde, string telefonoNumeris, string elPastas)
    {
        KlientoID = klientoID;
        Vardas = vardas;
        Pavarde = pavarde;
        TelefonoNumeris = telefonoNumeris;
        ElPastas = elPastas;
    }
}

class Uzsakymas
{
    public int UzsakymoID { get; set; }
    public int KlientoID { get; set; }
    public string Preke { get; set; }
    public int Kiekis { get; set; }
    public DateTime UzsakymoData { get; set; }
    public double KainaUzVnt { get; set; }

    public Uzsakymas(int uzsakymoID, int klientoID, string preke, int kiekis, DateTime uzsakymoData, double kainaUzVnt)
    {
        UzsakymoID = uzsakymoID;
        KlientoID = klientoID;
        Preke = preke;
        Kiekis = kiekis;
        UzsakymoData = uzsakymoData;
        KainaUzVnt = kainaUzVnt;
    }

    public double BendraKaina => Kiekis * KainaUzVnt;
}

class Program
{
    static List<Klientas> klientai = new List<Klientas>();
    static List<Uzsakymas> uzsakymai = new List<Uzsakymas>();

    static void Main()
    {
        klientai = NuskaitytiKlientusIsFailo("klientai.txt");
        uzsakymai = NuskaitytiUzsakymusIsFailo("uzsakymai.txt");

        bool running = true;
        while (running)
        {
            Console.WriteLine("\nMeniu pasirinkimai:");
            Console.WriteLine("1. Peržiūrėti visus klientus ir jų užsakymus");
            Console.WriteLine("2. Peržiūrėti užsakymą pagal ID");
            Console.WriteLine("3. Pridėti naują klientą");
            Console.WriteLine("4. Pridėti naują užsakymą klientui");
            Console.WriteLine("5. Ištrinti klientą pagal ID");
            Console.WriteLine("6. Ištrinti užsakymą pagal ID");
            Console.WriteLine("7. Išsaugoti visus pakeitimus į failą");
            Console.WriteLine("8. Išeiti");

            Console.Write("\nPasirinkite veiksmą: ");
            int pasirinkimas = int.Parse(Console.ReadLine());

            switch (pasirinkimas)
            {
                case 1:
                    PerziuretiVisusKlientusIrUzsakymus();
                    break;
                case 2:
                    PerziuretiUzsakymaPagalID();
                    break;
                case 3:
                    PridetiNaujaKlienta();
                    break;
                case 4:
                    PridetiNaujaUzsakyma();
                    break;
                case 5:
                    IstrintiKlientaPagalID();
                    break;
                case 6:
                    IstrintiUzsakymaPagalID();
                    break;
                case 7:
                    IssaugotiViskaIFaila("atnaujinti_duomenys.txt");
                    break;
                case 8:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Neteisingas pasirinkimas. Bandykite dar kartą.");
                    break;
            }
        }

        // Filtravimas ir papildomi skaičiavimai
        FiltravimasIrSkaičiavimai();
    }

    static List<Klientas> NuskaitytiKlientusIsFailo(string failoPavadinimas)
    {
        List<Klientas> klientai = new List<Klientas>();
        try
        {
            foreach (var eilute in File.ReadAllLines(failoPavadinimas))
            {
                var dalys = eilute.Split(',');
                if (dalys.Length == 5)
                {
                    int klientoID = int.Parse(dalys[0]);
                    string vardas = dalys[1];
                    string pavarde = dalys[2];
                    string telefonoNumeris = dalys[3];
                    string elPastas = dalys[4];
                    klientai.Add(new Klientas(klientoID, vardas, pavarde, telefonoNumeris, elPastas));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Klaida skaitant failą: {ex.Message}");
        }
        return klientai;
    }

    static List<Uzsakymas> NuskaitytiUzsakymusIsFailo(string failoPavadinimas)
    {
        List<Uzsakymas> uzsakymai = new List<Uzsakymas>();
        try
        {
            foreach (var eilute in File.ReadAllLines(failoPavadinimas))
            {
                var dalys = eilute.Split(',');
                if (dalys.Length == 6)
                {
                    int uzsakymoID = int.Parse(dalys[0]);
                    int klientoID = int.Parse(dalys[1]);
                    string preke = dalys[2];
                    int kiekis = int.Parse(dalys[3]);
                    DateTime uzsakymoData = DateTime.Parse(dalys[4]);
                    double kainaUzVnt = double.Parse(dalys[5]);
                    uzsakymai.Add(new Uzsakymas(uzsakymoID, klientoID, preke, kiekis, uzsakymoData, kainaUzVnt));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Klaida skaitant failą: {ex.Message}");
        }
        return uzsakymai;
    }

    static void PerziuretiVisusKlientusIrUzsakymus()
    {
        foreach (var klientas in klientai)
        {
            Console.WriteLine($"\nKlientas: {klientas.Vardas} {klientas.Pavarde} ({klientas.KlientoID})");
            var klientoUzsakymai = uzsakymai.Where(u => u.KlientoID == klientas.KlientoID).ToList();

            if (klientoUzsakymai.Any())
            {
                foreach (var uzsakymas in klientoUzsakymai)
                {
                    Console.WriteLine($"  Užsakymas {uzsakymas.UzsakymoID}: {uzsakymas.Preke} x{uzsakymas.Kiekis} - {uzsakymas.BendraKaina} EUR, {uzsakymas.UzsakymoData:yyyy-MM-dd}");
                }
            }
            else
            {
                Console.WriteLine("  Šiuo metu klientas neturi aktyvių užsakymų.");
            }
        }
    }

    static void PerziuretiUzsakymaPagalID()
    {
        Console.Write("Įveskite užsakymo ID: ");
        int id = int.Parse(Console.ReadLine());
        var uzsakymas = uzsakymai.FirstOrDefault(u => u.UzsakymoID == id);

        if (uzsakymas != null)
        {
            Console.WriteLine($"Užsakymas {uzsakymas.UzsakymoID}: {uzsakymas.Preke} - {uzsakymas.Kiekis} vnt., {uzsakymas.KainaUzVnt} EUR/vnt., {uzsakymas.UzsakymoData:yyyy-MM-dd}");
        }
        else
        {
            Console.WriteLine("Užsakymas nerastas.");
        }
    }

    static void PridetiNaujaKlienta()
    {
        Console.Write("Įveskite kliento ID: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Įveskite kliento vardą: ");
        string vardas = Console.ReadLine();
        Console.Write("Įveskite kliento pavardę: ");
        string pavarde = Console.ReadLine();
        Console.Write("Įveskite telefono numerį: ");
        string telefonas = Console.ReadLine();
        Console.Write("Įveskite el. pašto adresą: ");
        string elPastas = Console.ReadLine();

        klientai.Add(new Klientas(id, vardas, pavarde, telefonas, elPastas));
        Console.WriteLine("Klientas pridėtas sėkmingai.");
    }

    static void PridetiNaujaUzsakyma()
    {
        Console.Write("Įveskite užsakymo ID: ");
        int uzsakymoID = int.Parse(Console.ReadLine());
        Console.Write("Įveskite kliento ID: ");
        int klientoID = int.Parse(Console.ReadLine());
        Console.Write("Įveskite prekės pavadinimą: ");
        string preke = Console.ReadLine();
        Console.Write("Įveskite prekės kiekį: ");
        int kiekis = int.Parse(Console.ReadLine());
        Console.Write("Įveskite kainą už vienetą: ");
        double kaina = double.Parse(Console.ReadLine());
        Console.Write("Įveskite užsakymo datą (yyyy-MM-dd): ");
        DateTime uzsakymoData = DateTime.Parse(Console.ReadLine());

        uzsakymai.Add(new Uzsakymas(uzsakymoID, klientoID, preke, kiekis, uzsakymoData, kaina));
        Console.WriteLine("Užsakymas pridėtas sėkmingai.");
    }

    static void IstrintiKlientaPagalID()
    {
        Console.Write("Įveskite kliento ID: ");
        int id = int.Parse(Console.ReadLine());
        klientai.RemoveAll(k => k.KlientoID == id);
        uzsakymai.RemoveAll(u => u.KlientoID == id);
        Console.WriteLine("Klientas ir jo užsakymai pašalinti sėkmingai.");
    }

    static void IstrintiUzsakymaPagalID()
    {
        Console.Write("Įveskite užsakymo ID: ");
        int id = int.Parse(Console.ReadLine());
        uzsakymai.RemoveAll(u => u.UzsakymoID == id);
        Console.WriteLine("Užsakymas pašalintas sėkmingai.");
    }

    static void IssaugotiViskaIFaila(string failoPavadinimas)
    {
        using (StreamWriter writer = new StreamWriter(failoPavadinimas))
        {
            foreach (var klientas in klientai)
            {
                writer.WriteLine($"{klientas.KlientoID},{klientas.Vardas},{klientas.Pavarde},{klientas.TelefonoNumeris},{klientas.ElPastas}");
            }

            foreach (var uzsakymas in uzsakymai)
            {
                writer.WriteLine($"{uzsakymas.UzsakymoID},{uzsakymas.KlientoID},{uzsakymas.Preke},{uzsakymas.Kiekis},{uzsakymas.UzsakymoData:yyyy-MM-dd},{uzsakymas.KainaUzVnt}");
            }
        }
        Console.WriteLine("Pakeitimai išsaugoti sėkmingai.");
    }

    static void FiltravimasIrSkaičiavimai()
    {
        double sumaSenuUzsakymu = 0;
        int klientuSuDideliuUzsakymuSkaicius = 0;

        foreach (var klientas in klientai)
        {
            var klientoUzsakymai = uzsakymai.Where(u => u.KlientoID == klientas.KlientoID).ToList();
            if (!klientoUzsakymai.Any())
            {
                Console.WriteLine($"Klientas {klientas.Vardas} {klientas.Pavarde} šiuo metu neturi aktyvių užsakymų.");
            }
            else
            {
                foreach (var uzsakymas in klientoUzsakymai)
                {
                    if (uzsakymas.BendraKaina > 1000)
                    {
                        Console.WriteLine($"Klientas {klientas.Vardas} {klientas.Pavarde} turi užsakymą su bendra verte virš 1000 eurų.");
                    }
                    if ((DateTime.Now - uzsakymas.UzsakymoData).TotalDays > 365)
                    {
                        Console.WriteLine($"Užsakymas {uzsakymas.Preke} klientui {klientas.Vardas} {klientas.Pavarde} pateiktas daugiau nei prieš metus.");
                        sumaSenuUzsakymu += uzsakymas.BendraKaina;
                    }
                }
                if (klientoUzsakymai.Sum(u => u.BendraKaina) > 5000)
                {
                    klientuSuDideliuUzsakymuSkaicius++;
                }
            }
        }

        Console.WriteLine($"\nBendra suma užsakymų, senesnių nei metai: {sumaSenuUzsakymu} EUR");
        Console.WriteLine($"Klientų, kurių bendra užsakymų suma viršija 5000 EUR, kiekis: {klientuSuDideliuUzsakymuSkaicius}");
    }
}
