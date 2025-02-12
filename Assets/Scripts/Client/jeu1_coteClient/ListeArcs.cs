using System.Collections.Generic;
using System;
using System.Linq;
public class ListeArcs
{
	public List<Arc> Liste { get; private set; }
	// private readonly List<Arc> Liste;

	public ListeArcs(List<Arc> liste)
	{
		// Ensuring the input list is not null
		if (liste == null)
			throw new ArgumentNullException(nameof(liste));

		// Immediately converting the IEnumerable from OrderBy to List
		this.Liste = liste.OrderBy(l => l.Id).ToList();

		// Assuming VerifAucunDoublon checks for duplicates and returns a boolean
		if (!this.VerifAucunDoublon())
		{
			throw new ArgumentException("Duplicate arcs are not allowed.");
		}
	}


	private int IndiceArc(int id) //possibilite d'optimisation en faisant par dichotomie
	{
		for (int i = 0; i < Liste.Count; i++)
		{
			if (id == Liste[i].Id)
			{
				return i;
			}
		}
		return -1;
	}

	private bool VerifAucunDoublon()
	{
		return Liste.Count == Liste.Distinct().Count();
	}


	public bool Equals(ListeArcs other)
	{
		if (Liste.Count != other.Liste.Count) return false;

		for (int i = 0; i < Liste.Count; i++)
		{
			if (Liste[i] != other.Liste[i])
			{
				return false;
			}
		}
		return true;
	}

	public Arc GetRoute(int idRoute)
	{
		int i = IndiceArc(idRoute);
		if (i == -1)
		{
			throw new Exception("La route n'existe pas");
		}

		return Liste[i];
	}

	public void AssignerJoueurARoute(int idJoueur, int idRoute)
	{
		int i = IndiceArc(idRoute);
		if (i == -1)
		{
			throw new Exception("La route n'existe pas");
		}

		Liste[i].SetPrise(idJoueur);
	}

	public override string ToString()
	{
		string res = "";
		foreach (Arc a in Liste)
		{
			res += $"{a}\n";
		}
		return res;
	}
}
