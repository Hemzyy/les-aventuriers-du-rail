
using System;
using System.Net.Sockets;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

using serverPackage;

public class Lobby
{
    public int IdLobby;
    public List<ServerClient> ListPlayer; 
    private int MaxPlayer = 4;
    public int nbrPlayerInLobby = 0;
    public string codeLobby;
    private ServerClient ClientCreateur;
    public JeuServeur jeu;

    public Lobby(int idLobby,string codeLobby,ServerClient createur)
    {
        this.IdLobby = idLobby;
        this.codeLobby = codeLobby;
        this.ClientCreateur = createur;
        ListPlayer = new List<ServerClient>();
        jeu = new JeuServeur(ListeArcs.createMap(ListeArcs.GetRails()),Foret.getVilles());
    }
    
    public void createLobby()
    {
        ListPlayer.Add(ClientCreateur);
        nbrPlayerInLobby++;
        Console.WriteLine("le joueur Id ("+ClientCreateur.ClientiD+": ) ("+ClientCreateur.pseudo+" ) a cree le lobby id ("+IdLobby+") codeLobby: "+codeLobby);
    }

    public void joinLobby(PaquetMessage code,ServerClient sc)
    {   
        ListPlayer.Add(sc);
        nbrPlayerInLobby++;
        Console.WriteLine("nombre de joueur dans le lobby id (" + IdLobby + ") : " + nbrPlayerInLobby);
        Console.WriteLine("le joueur id ("+sc.ClientiD+") : ("+sc.pseudo+" ) a rejoint le lobby Id ("+IdLobby+")");        
    }

    public void QuitLobby(ServerClient sc)
    {
        //on verra apres quoi faire ici
        ListPlayer.Remove(sc);
        Console.WriteLine(sc.pseudo + "deconnecte ");
        nbrPlayerInLobby--;
        Console.WriteLine("player restant dans lobby: "+ListPlayer.Count);
    }
}
