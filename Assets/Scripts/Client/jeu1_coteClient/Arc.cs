using System.Collections.Generic;
using System;
public class Arc
	{

		public int Id { get; private set; } //identifiant de la route
		public int PointA { get; private set; } //identifiant du premier point
		public int PointB { get; private set; } //identifiant du deuxieme point
		public int Couleur { get; private set; } //identifiant de la couleur
		public int Longueur { get; private set; } //longueur de la route
		public int Prise { get; private set; } //-1 si la route n'est pas prise, identifiant du joueur sinon
		private static List<int> Existants = new List<int>(); //permet de verifier que chaque identifiant est unique 
		
		
		//Fonctions vérifiant les erreurs => generation d'une exception
		
		private static void CheckPoint(int pointA, int pointB) //verifie que les deux villes sont distinctes
		{
			if (pointA == pointB)
			{
				throw new Exception("Une route ne peut pas relier une route a elle-meme !");
			}
		}
		
		private static void CheckLongueur(int longueur) //verifie que la longueur n'est pas negative ou nulle
		{
			if (longueur <= 0)
			{
				throw new Exception("Une route ne peut pas avoir de longueur negative ou nulle !");
			}
		}

		private static void CheckPrise(int prise) //verifie que le parametre prise est valide (-1 ou identifiant d'un joueur)
		{
			if (prise < -1)
			{
				throw new Exception("Une route appartient à un joueur (son identifiant) ou a personne (-1) !");
			}
		}

		private static void CheckId(int id) //verifie que l'id est valide (chaque id doit etre unique)
		{
			bool test = true;
			for(int i = 0; i<Existants.Count; i++)
			{
				if(id == Existants[i])
				{
					test = false;
				}
			}

			if(!test)
			{
				throw new Exception("L'identifiant existe deja !");
			}
			else
			{
				Existants.Add(id);
			}
		}

		private void CheckPriseChangeable() //verifie que la route n'appartient pas deja a un joueur
		{
			if(Prise != -1)
			{
				throw new Exception("La route appartient deja a un joueur !");
			}
		}
		
		
		//Constructeurs

		public Arc(int id, int pointA, int pointB, int couleur, int longueur, int prise)
		{
			CheckId(id);
			CheckPoint(pointA, pointB);
			CheckLongueur(longueur);
			CheckPrise(prise);
			
			if(pointA < pointB)
			{
				this.PointA = pointA;
				this.PointB = pointB;
			}
			else
			{
				this.PointA = pointB;
				this.PointB = pointA;
			}
			
			this.Couleur = couleur;
			this.Longueur = longueur;
			this.Prise = prise;
			this.Id = id;
		}
		
		
		//Ensemble de fonctions set
		
		public void SetPrise(int prise)
		{
			CheckPriseChangeable();
			CheckPrise(prise);
			this.Prise = prise;
		}
		
		
		//Ensemble de fonctions override
		/*
		public override bool Equals(Object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			Arc other = (Arc)obj;
			return Id == other.GetId();
		}
		*/

		public override string ToString()
		{
			return $"({Id}, {PointA}, {PointB}, {Couleur}, {Longueur}, {Prise})";
		}
	}
