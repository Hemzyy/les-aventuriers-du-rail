using System.Linq;

public class Joueur
{
    // class qui represente le joueur

    // chaine de caractere qui represente le nom / pseudo du joueur
    public string Nom { get; private set; }
    // entier identifiant de maniere unique le joueur (a voir dans la base de donnees)
    public int Identifiant { get; private set; }
    // entier representant la couleur qu'on va associer au joueur
    //public int Couleur { get; private set; }
    // les cartes missions du joueur (la main mission)
    public List<CarteMission> Missions { get; private set; }
    // les cartes wagons du joueur (la main wagon)
    public List<CarteWagon> Wagons { get; private set; }
    // le gestionnaire de pile, celui a partir duquel on va avoir access 
    public GestionnairePioche pileManager { get; private set; }
    // le nombre de wagon restant chez un joueur
    public int WagonsRestants { get; private set; }
    // le score actuel du joueur
    public int Score { get; private set; }

    // tableau permettant d'attribuer un nombre de point a un nombre de wagons
    private List<(int, int)> Correspondance = new() { (1, 1), (2, 2), (3, 4), (4, 7), (6, 15), (8, 21) };



    // le constructeur prend en arguments le nom, l'identifiant, la couleur pour la version alpha
    // mais il doit toujours prendre un Gestionnaire de pioche, quelque soit la version
    public Joueur(string Nom, int Identifiant, GestionnairePioche pileManager)
    {
        this.Nom = Nom;
        this.Identifiant = Identifiant;
        //this.Couleur = Couleur;
        Missions = new List<CarteMission>();
        Wagons = new List<CarteWagon>();
        this.pileManager = pileManager;
        WagonsRestants = 45;
        Score = 0;
    }


    // fonction qui ajouter une carte mission a la main du joueur
    public void ajouterMission(CarteMission cartePioche)
    {
        Missions.Add(cartePioche);
    }


    // fonction qui pioche des cartes missions depuis la pioche mission
    // elle va prendre un argument indexCartes et elle va ajouter les cartes (dont l'index a ete specifie) dans la main du joeur
    // si indexCartes est null, elle va ajouter le dernier elements du tirage dans la main du joueur
    // le reste des cartes non choisi vont etre defausse
    public CarteMission[] piocherMission()
    {
        return pileManager.piocherCarteMission();
    }

    // fonction permettant d'ajouter une carte wagon a la main du joueur
    public void ajouterWagon(CarteWagon cartePioche)
    {
        Wagons.Add(cartePioche);
    }


    // fonction permettant de tirer une carte wagon depuis la pile wagon
    public CarteWagon piocherPileWagon()
    {
        CarteWagon cw = new CarteWagon();
        ajouterWagon(cw);
        return cw;
    }

    // fonction qui permet de piocher a partir des cartes disponibles sur table 
    public CarteWagon piocherTableWagon(int indexCarte)
    {
        CarteWagon cw = pileManager.piocherSurTable(indexCarte);
        ajouterWagon(cw);
        return cw;
    }

    private int GetNombreDePoints(int nombre)
    {
        foreach ((int a, int b) in Correspondance)
        {
            if (a == nombre)
            {
                return b;
            }
        }
        throw new Exception("Impossible de trouver le nombre de points correspondants !");
    }

    // fonction responsable de la pose de wagons
    public void poserWagons(int nombre)
    {
        if (nombre > WagonsRestants)
        {
            throw new Exception("On ne peut pas poser plus de wagons que ceux qu'on possède !");
        }
        WagonsRestants -= nombre;
        Score += GetNombreDePoints(nombre);
    }

    public CarteMission getMission(int idMission)
    {
        CarteMission card = Missions.FirstOrDefault(c => c.Identifiant == idMission);
        Missions.Remove(card);
        return card;
    }
    
}
