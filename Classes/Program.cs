// See https://aka.ms/new-console-template for more information
public class Program
{
	public static void Main(string[] args)
	{
		Console.WriteLine("hello everyone\n");

		CarteMission carteMission1 = new CarteMission();
		CarteMission carteMission2 = new CarteMission();

		Console.WriteLine(carteMission1.ToString() + "\n" + carteMission2.ToString());

		CarteWagon carteWagon1 = new CarteWagon();
		CarteWagon carteWagon2 = new CarteWagon();


		Console.WriteLine(carteWagon1.ToString() + "\n" + carteWagon2.ToString());
		Console.WriteLine(carteWagon1.Couleur + "\n" + carteWagon2.Couleur);


		Carte carte1W = Carte.createCarteWagon();
		Carte carte1M = Carte.createCarteMission();


		Console.WriteLine(carte1M.GetType());
		Console.WriteLine(carte1M.ToString());
		Console.WriteLine(carte1M.Identifiant);

		Console.WriteLine(carte1W.GetType());
		Console.WriteLine(carte1W.ToString());


		Console.WriteLine("----------------------------------------------------------------------------------------------------------------");

		PiocheCartesMission pileMission = new PiocheCartesMission();
		DefausseCartesMissions defausseMission = new DefausseCartesMissions();


		while (!pileMission.estVide())
		{
			Carte cartePioche = pileMission.piocheCarte();
			if (cartePioche.Identifiant % 2 == 0)
			{
				defausseMission.defausserCarte((CarteMission)cartePioche);
				continue;
			}
			Console.WriteLine(cartePioche.ToString());

		}
		Console.WriteLine(defausseMission.estVide());


		PiocheCartesWagon pileWagon = new PiocheCartesWagon();

		while (!pileWagon.estVide())
		{
			Console.WriteLine(pileWagon.piocherCarte().ToString());
		}


		EnsembleDeCartes alpha = EnsembleDeCartes.createPiocheMission();

		// ici on ne peut donc avoir que une seul et unique pioche de cartes mission. il faut donc faire attention a cela
		Console.WriteLine(alpha.estVide());
		Console.WriteLine(alpha.GetType());

		EnsembleDeCartes beta = EnsembleDeCartes.createPiocheWagon();

		Console.WriteLine(beta.estVide());
		Console.WriteLine(beta.GetType());

		EnsembleDeCartes gama = EnsembleDeCartes.createDefausseMisison();

		Console.WriteLine(gama.estVide());
		Console.WriteLine(gama.GetType());

		Console.WriteLine("----------------------------------------------------------------------------------------------------------------");
	
		GestionnairePioche pileManager = new GestionnairePioche();
		Console.WriteLine(pileManager.SurTable);
		for (int i = 0; i < pileManager.SurTable.Length; i++)
		{
			Console.WriteLine(pileManager.SurTable[i].ToString());
		}

		Console.WriteLine("j'ai pioche a partir de la table la numero 3" + pileManager.piocherSurTable(3));
		for (int i = 0; i < pileManager.SurTable.Length; i++)
		{
			Console.WriteLine(pileManager.SurTable[i].ToString());
		}



	}
}