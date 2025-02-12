using System;
using System.Timers;
using Timer = System.Timers.Timer;


// Classe TimerTour gère un compte à rebours pour chaque tour de jeu
public class TimerTour
{
	private Timer timer; // Le timer interne
	public bool TempsEcoule { get; private set; } // Indique si le temps est écoulé

	// Événement déclenché lorsque le temps est écoulé
	public event EventHandler TempsEcouleEvent;

	// initialise le timer
	public TimerTour(double dureeSeconde)
	{
		timer = new Timer(dureeSeconde * 1000); // Convertit to seconds
		timer.Elapsed += OnTimedEvent; // Associe l'événement Elapsed du timer à notre gestionnaire d'événements
		timer.AutoReset = false; // Empêche le timer de se réinitialiser automatiquement
		TempsEcoule = false;
	}

	// Méthode pour démarrer le timer
	public void Demarrer()
	{
		TempsEcoule = false;
		timer.Start(); // Démarre le timer
	}

	// Méthode pour arrêter le timer
	public void Arreter()
	{
		timer.Stop(); // Arrête le timer
		TempsEcoule = true; // Met à jour le statut du timer
	}

	// Gestionnaire d'événements appelé lorsque le timer atteint la durée spécifiée
	private void OnTimedEvent(Object source, ElapsedEventArgs e)
	{
		TempsEcoule = true; // Met à jour le statut pour indiquer que le temps est écoulé
		TempsEcouleEvent?.Invoke(this, EventArgs.Empty); // Déclenche l'événement TempsEcouleEvent
	}
}