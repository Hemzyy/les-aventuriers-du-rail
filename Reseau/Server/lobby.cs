
using System;
using System.Net.Sockets;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
public class Lobby
{
    public int IdLobby;
    public List<ServerClient> ListPlayer; 
    private int MaxPlayer = 4;
    public int nbrPlayerInLobby = 0;
    public string codeLobby;
    private ServerClient ClientCreateur;

    public Lobby(int idLobby,string codeLobby,ServerClient createur)
    {
        this.IdLobby = idLobby;
        this.codeLobby = codeLobby;
        this.ClientCreateur = createur;
        ListPlayer = new List<ServerClient>();
    }
    
    public void createLobby()
    {
        ListPlayer.Add(ClientCreateur);
        nbrPlayerInLobby++;
        Console.WriteLine("le joueur Id ("+ClientCreateur.ClientiD+") a cree le lobby id ("+IdLobby+") codeLobby: "+codeLobby);
    }

    public void joinLobby(string code,ServerClient sc)
    {   
        ListPlayer.Add(sc);
        nbrPlayerInLobby++;
        Console.WriteLine("nombre de joueur dans le lobby id (" + IdLobby + ") : " + nbrPlayerInLobby);
        Console.WriteLine("le joueur id ("+sc.ClientiD+") a rejoint le lobby Id ("+IdLobby+")");        
    }

    public void QuitLobby()
    {
        //on verra apres quoi faire ici
    }
}
