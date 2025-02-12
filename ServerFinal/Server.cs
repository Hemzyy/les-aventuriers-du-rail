using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading;
using MySqlX.XDevAPI;
using serverPackage;
using static System.Runtime.InteropServices.JavaScript.JSType;


public class Server
{
    public int port = 10000;
    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    private Thread updateThread;
    //le serveur en lui meme
    private TcpListener server;
    private bool serverStarted;
    public int ClientId = 0;


    //ici pour faire mes test lobby 
    private List<Lobby> lobbysList;
    private int IdLobby = 1;
    private int IndexList = 0;

    private MethodesGeneration Methodes;
    //lance le server

    private DBInteract dbInteract; //ali 
    //ici on déclare

    public void Init()
    {
        //a ajouter lors de la liaison avec unity pour pouvoir changer de scence
        //DontDestroyOnLoad(gameObject);

        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        //section lobby 
        lobbysList = new List<Lobby>();
        Methodes = new MethodesGeneration();
        //fin section lobby
        
        dbInteract = new DBInteract();
        dbInteract.Start();
        
        try
        {
            //voir qui accepter puis start le server
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            startLinstening();
            serverStarted = true;
        }
        catch (Exception e)
        {
            Debug.WriteLine("Socket error: " + e.Message);
        }
    }
    
    public void Update()
    {
        if (!serverStarted)
        {
            Console.WriteLine("Server not Started.");
            return;
        }

        while (serverStarted)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                //verifie si le client est encore connecter ?
                if (clients[i] != null && clients[i].tcp != null)
                {
                    if (!IsConnected(clients[i].tcp))
                    {
                        clients[i].tcp.Close();
                        disconnectList.Add(clients[i]);
                        continue;
                    }

                    NetworkStream s = clients[i].tcp.GetStream();
                    if (s.DataAvailable)
                    {   
                        
                        StreamReader reader = new StreamReader(s,true);
                        string jsonData = reader.ReadLine(); // Lecture de la chaîne JSON directement depuis le flux

                        if (!string.IsNullOrEmpty(jsonData))
                        {
                            // Traiter les paquets venant des clients
                            handleData(jsonData, clients, i);
                        }
                    }
                }
            }

            //une boucle pour deconnecter les clients
            for (int i = disconnectList.Count - 1; i >= 0; i--)
            {
                Console.WriteLine("client id: (" + disconnectList[i].ClientiD + ") deconnecter.");
                
                Console.WriteLine("client : (" + disconnectList[i].pseudo + ") deconnecter.");
                int lobbyIndex = FindLobbyIndexByPlayerId(clients[i].ClientiD);
                if (lobbyIndex != -1) {
                    Lobby lobby = lobbysList[lobbyIndex];
                    Joueur joueurToRemove = lobby.jeu.GetJoueur(clients[i].ClientiD);
                    
                    if (joueurToRemove != null) {
                        lobby.QuitLobby(clients[i]);

                        // Console.WriteLine("Vous avez quitté le lobby.", clients[i]);

                        // If the lobby becomes empty, remove the lobby
                        if (lobby.nbrPlayerInLobby == 0) {
                            lobbysList.RemoveAt(lobbyIndex);
                            IndexList--;
                            Console.WriteLine("Lobby ID " + lobby.IdLobby + " supprimé car vide.");
                        } else {
                            // Notify remaining players in the lobby about the player leaving
                            PaquetMessage playerLeftMessage = new PaquetMessage(clients[i].ClientiD, 20070, new List<List<string>> { new List<string> { clients[i].pseudo + " a quitté le lobby." } });
                            Broadcast(playerLeftMessage, lobby.ListPlayer);
                        }
                    } else {
                        PaquetMessage lobbyContientPasClient = new PaquetMessage(0,9999,new List<List<string>> { new List<string> { "lobbyContientPasClient" }});
                        Unicast(lobbyContientPasClient, clients[i]);
                    }
                } else {
                    PaquetMessage ClientPasDansUnLobby = new PaquetMessage(0,9999,new List<List<string>> { new List<string> { "ClientPasDansUnLobby" }});
                    Unicast(ClientPasDansUnLobby, clients[i]);
                }
                clients.Remove(disconnectList[i]);
                disconnectList.RemoveAt(i);
            }
        }
    }


    //fonction qui commence a recevoir les demandes de connections et les accetper
    private void startLinstening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    //accepte le client et l'ajoute a la liste des clients  
    private void AcceptTcpClient(IAsyncResult ar)
    {
        //pour nous rappeler de qui sais
        TcpListener listener = (TcpListener)ar.AsyncState;

        ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar), ClientId,"anonyme");
        Console.WriteLine("Un nouveau client s'est connecté.");
        clients.Add(sc);
        this.ClientId++;
        // apres que le server accepte un nouveau client il oublie de se remettre a ecouter don on rappel startlistening
        startLinstening();
    }

    //fonction pour savoir si le client est connecter 
    private bool IsConnected(TcpClient c)
    {
        if (c?.Client == null || !c.Client.Connected)
        {
            return false;
        }

        try
        {
            if (c.Client.Poll(0, SelectMode.SelectRead))
            {
                return c.Client.Receive(new byte[1], SocketFlags.Peek) != 0;
            }
        }
        catch
        {
            return false;
        }

        return true;
    }


    //new broadcast takes a PaquetMessage object as parameter
    public void Broadcast(PaquetMessage data, List<ServerClient> lsc)
    {
        try
        {
            foreach (ServerClient sc in lsc)
            {
                try
                {
                    if (sc.tcp != null && IsConnected(sc.tcp))
                    {
                        string jsonString = JsonSerializer.Serialize(data);
                        NetworkStream stream = sc.tcp.GetStream();
                        StreamWriter writer = new StreamWriter(stream);
                        writer.WriteLine(jsonString); // Écriture de la chaîne JSON directement dans le flux
                        writer.Flush();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    public void Unicast(PaquetMessage data, ServerClient client)
    {   
        
        try
        {
            if (client.tcp != null && IsConnected(client.tcp))
            {
                try
                {
                    string jsonString = JsonSerializer.Serialize(data);
                    NetworkStream stream = client.tcp.GetStream();
                    StreamWriter writer = new StreamWriter(stream);
                    writer.WriteLine(jsonString); // Écriture de la chaîne JSON directement dans le flux
                    writer.Flush();

                    StreamReader reader = new StreamReader(stream);
                    while (true)
                    {
                        string msg = reader.ReadLine();
                        PaquetMessage verification = JsonSerializer.Deserialize<PaquetMessage>(msg);

                        if (verification.IdAction == 36) break;
                    }
                    
                }
                catch (JsonException ex)
                {
                    Debug.WriteLine("Failed to serialize PaquetMessage. Error: " + ex.Message);
                }
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
    

    //pour recevoir les data du clients
    private void OnIncomingData(ServerClient c, string data)
    {
        Console.WriteLine("message recu de IdClient " + "(" + c.ClientiD + ") (pseudo: "+c.pseudo + " ) : " + data);
    }

    //pour traiter les data recu
    public void handleData(string jsonData, List<ServerClient> clients, int i)
    {

        PaquetMessage data = JsonSerializer.Deserialize<PaquetMessage>(jsonData); //data recu du client
        
        Joueur joueurSelectionne;
		CarteMission[] lesCartesMissions = [];
        int lobbyIndex;
	switch (data.IdAction)
        {
            case 0: //player wants to host a game
                
                    //check if the player is already in a lobby
                    bool ExistingPlayer = (lobbysList != null) && lobbysList.Any(lobby => lobby.ListPlayer.Any(player => player.ClientiD == clients[i].ClientiD));
                    if (!ExistingPlayer)
                    {
                        string codePourTester = Methodes.GenerateRandomCode();
                        
                        //pour l'instant juste pour piocher un code dans la liste 
                        Lobby lb = new Lobby(IdLobby, codePourTester, clients[i]);
                        lobbysList.Add(lb);
                        lobbysList[IndexList].createLobby();
                        PaquetMessage code = new PaquetMessage(clients[i].ClientiD, 20070, new List<List<string>>{new List<string>{codePourTester}});
                        Unicast(code,clients[i]);
                                                
                        //creation du joueur
                        Joueur joueur = new Joueur(clients[i].pseudo,clients[i].ClientiD,lobbysList[IndexList].jeu.pileManager); //couleur a modifier
                        lobbysList[IndexList].jeu.AjouterJoueur(joueur);
                        
                        //creation et envoie des cartes wagons du joueur 
                        //les cartes wagons doivent etre envoyer pour tout le monde les memes a voir 
                        List<List<string>> wagons = new List<List<string>>();
                        for (int j = 0; j < 5; j++)
                        {
                            wagons.Add(new List<string> {lobbysList[IndexList].jeu.pileManager.SurTable[j].Identifiant.ToString()});
                        }
                        PaquetMessage wagonsEnvoyerPaq = new PaquetMessage(clients[i].ClientiD,20050,wagons);
                        Unicast(wagonsEnvoyerPaq,clients[i]);
                        
                        //creation et envoie carte mission
                        List<List<string>> cartesmissions = new List<List<string>>();
                        CarteMission[] cartes = lobbysList[IndexList].jeu.DemandeMissions(joueur);
                        for (int z = 0; z < cartes.Length; z++)
                        {
                            cartesmissions.Add(transformationTabString(cartes[z]));
                        }

                        PaquetMessage cartesmissionsenvoyer = new PaquetMessage(clients[i].ClientiD, 20060, cartesmissions);
                        Unicast(cartesmissionsenvoyer,clients[i]);
                        
                        //les cartes wagons pour le joueur en lui meme 
                        List<List<string>> wagonsSolo = new List<List<string>>();
                        for (int j = 0; j < 4; j++)
                        {
                            wagonsSolo.Add(new List<string> {lobbysList[IndexList].jeu.DemandePileWagon(joueur).Identifiant.ToString()});
                        }

                        PaquetMessage wagonsSoloEnvoyer = new PaquetMessage(clients[i].ClientiD,20080,wagonsSolo);
                        Unicast(wagonsSoloEnvoyer,clients[i]);

                        // Console.WriteLine("Joueurs dans la liste:\n");
                        // foreach (ServerClient joueurloco in lobbysList[IndexList].ListPlayer)
                        // {
                        //     Console.WriteLine("id Joueur: " + joueurloco.ClientiD +  "__" + "pseudo Joueur: " + joueurloco.pseudo);
                        // }

                        /// communiquer les noms des clients dans lobby
                        string listeDesJoueursdansLobby = "";
                        Console.WriteLine("Joueurs dans la liste:\n");
                        foreach (ServerClient joueurloco in lobbysList[IndexList].ListPlayer)
                        {
                            Console.WriteLine("id Joueur: " + joueurloco.ClientiD + "__" + "pseudo Joueur: " + joueurloco.pseudo);
                            Console.WriteLine("id Joueur dan lobby: " + lobbysList[IndexList].ListPlayer.IndexOf(joueurloco) + " __ " + "pseudo Joueur: " + joueurloco.pseudo);
                            listeDesJoueursdansLobby += "|" + joueurloco.pseudo;
                        }

                        PaquetMessage paquetListeDesjoueurs = new PaquetMessage(clients[i].ClientiD, 99997,new List<List<string>> { new List<string> { listeDesJoueursdansLobby }});
                        Broadcast(paquetListeDesjoueurs, lobbysList[IndexList].ListPlayer);


                        // dire ok a createuer qu'il peut lancer la partie
                        PaquetMessage paquetChefLobby = new PaquetMessage(clients[i].ClientiD, 99996,new List<List<string>> { new List<string> { "0" }});
                        Unicast(paquetChefLobby,clients[i]);
                        
                        IndexList++;
                        IdLobby++;
                        PaquetMessage MdpDuLobby = new PaquetMessage(clients[i].ClientiD, 0, new List<List<string>> {new List<string> { codePourTester }});
                        Unicast(MdpDuLobby, clients[i]); // On envoie le mdp du lobby au client qui a host la game.
                    }
                    else
                    {
                        PaquetMessage clientDansUnLobby = new PaquetMessage(0,9999,new List<List<string>> { new List<string> { "vous etes deja dans un lobby existant" }});
                        Unicast(clientDansUnLobby, clients[i]);
                    }
                break;

            case 1: //player wants to join a game 
                //check if the player is already in a lobby
                bool ExistingPlayerJoin = (lobbysList != null) && lobbysList.Any(lobby => lobby.ListPlayer.Any(player => player.ClientiD == clients[i].ClientiD));
                if (!ExistingPlayerJoin)
                {
                    if (lobbysList != null && lobbysList.Any(lobby => lobby.codeLobby == data.Parametres[0][0]))
                    {
                        lobbyIndex = lobbysList.FindIndex(lobby => lobby.codeLobby == data.Parametres[0][0]);
                        if (lobbysList[lobbyIndex].nbrPlayerInLobby < 4)
                        {
                            //send 1 to tell player the code is good and can change scenes
                            PaquetMessage maxJoueurDansLobby = new PaquetMessage(0,9999,new List<List<string>> { new List<string> { "0" }});
                            Unicast(maxJoueurDansLobby,clients[i]);
                            lobbysList[lobbyIndex].joinLobby(data, clients[i]);
                            //creation du joueur
                            Joueur joueur = new Joueur(clients[i].pseudo,clients[i].ClientiD,lobbysList[lobbyIndex].jeu.pileManager); //couleur a modifier
                            lobbysList[lobbyIndex].jeu.AjouterJoueur(joueur);
                            
                            //creation et envoie des cartes wagons du joueur 
                            //les cartes wagons doivent etre envoyer pour tout le monde les memes a voir 
                            List<List<string>> wagons = new List<List<string>>();
                            for (int j = 0; j < 5; j++)
                            {
                                wagons.Add(new List<string> {lobbysList[lobbyIndex].jeu.pileManager.SurTable[j].Identifiant.ToString()});
                            }
                            PaquetMessage wagonsEnvoyerP = new PaquetMessage(clients[i].ClientiD,20050,wagons);
                            Unicast(wagonsEnvoyerP,clients[i]);
                            
                            //creation et envoie carte mission
                            List<List<string>> cartesmissions = new List<List<string>>();
                            CarteMission[] cartes = lobbysList[lobbyIndex].jeu.DemandeMissions(joueur);
                            for (int z = 0; z < cartes.Length; z++)
                            {
                                cartesmissions.Add(transformationTabString(cartes[z]));
                            }

                            PaquetMessage cartesmissionsenvoyer = new PaquetMessage(clients[i].ClientiD, 20060, cartesmissions);
                            Unicast(cartesmissionsenvoyer,clients[i]);
                            
                            //les cartes wagons pour le joueur en lui meme 
                            List<List<string>> wagonsSolo = new List<List<string>>();
                            for (int j = 0; j < 4; j++)
                            {
                                wagonsSolo.Add(new List<string> {lobbysList[lobbyIndex].jeu.DemandePileWagon(joueur).Identifiant.ToString()});
                            }

                            PaquetMessage wagonsSoloEnvoyer = new PaquetMessage(clients[i].ClientiD,20080,wagonsSolo);
                            Unicast(wagonsSoloEnvoyer,clients[i]);

                            string listeDesJoueursdansLobby = "";

                            

                            Console.WriteLine("Joueurs dans la liste:\n");
                            foreach (ServerClient joueurloco in lobbysList[lobbyIndex].ListPlayer)
                            {
                                Console.WriteLine("id Joueur: " + joueurloco.ClientiD + "__" + "pseudo Joueur: " + joueurloco.pseudo);
                                 Console.WriteLine("id Joueur dan lobby: " + lobbysList[lobbyIndex].ListPlayer.IndexOf(joueurloco) + " __ " + "pseudo Joueur: " + joueurloco.pseudo);
                                listeDesJoueursdansLobby += "|" + joueurloco.pseudo;
                            }

                            PaquetMessage paquetListeDesjoueurs = new PaquetMessage(clients[i].ClientiD, 99997,new List<List<string>> { new List<string> { listeDesJoueursdansLobby }});
                            Broadcast(paquetListeDesjoueurs, lobbysList[lobbyIndex].ListPlayer);

    
                            
                        }
                        else
                        {
                            PaquetMessage maxJoueurDansLobby = new PaquetMessage(0,9999,new List<List<string>> { new List<string> { "1" }});
                            Unicast(maxJoueurDansLobby, clients[i]);
                        }
                    }
                    else //wrong code
                    {
                        Console.WriteLine("le lobby n'existe pas ou le code est incorrect");
                        PaquetMessage maxJoueurDansLobby = new PaquetMessage(0,9999,new List<List<string>> { new List<string> { "1" }});
                        Unicast(maxJoueurDansLobby, clients[i]);                        
                    }
                }
                else
                {
                    PaquetMessage clientDansLobby = new PaquetMessage(0,9999,new List<List<string>> { new List<string> { "1" }});
                    Unicast(clientDansLobby, clients[i]);
                }
                break;

            case 2: // Player quits lobby
                lobbyIndex = FindLobbyIndexByPlayerId(clients[i].ClientiD);
                if (lobbyIndex != -1) {
                    Lobby lobby = lobbysList[lobbyIndex];
                    Joueur joueurToRemove = lobby.jeu.GetJoueur(clients[i].ClientiD);

                    if (joueurToRemove != null) {

                        // Console.WriteLine("Vous avez quitté le lobby.", clients[i]);
                            lobby.QuitLobby(clients[i]);
                            
                            PaquetMessage paquetChefLobbyQuit = new PaquetMessage(lobbysList[lobbyIndex].ListPlayer[0].ClientiD, 99996,new List<List<string>> { new List<string> { "0" }});
                            Unicast(paquetChefLobbyQuit,lobbysList[lobbyIndex].ListPlayer[0]);
                                
                            string listeDesJoueursdansLobby = "";                  
                            Console.WriteLine("Joueurs dans la liste:\n");
                            foreach (ServerClient joueurloco in lobbysList[lobbyIndex].ListPlayer)
                            {
                                // Console.WriteLine("id Joueur: " + joueurloco.ClientiD + "__" + "pseudo Joueur: " + joueurloco.pseudo);
                                Console.WriteLine("id Joueur dan lobby: " + lobbysList[lobbyIndex].ListPlayer.IndexOf(joueurloco) + " __ " + "pseudo Joueur: " + joueurloco.pseudo);
                                // if(lobbysList[lobbyIndex].ListPlayer.IndexOf(joueurloco)==0)
                                // {
                                //     PaquetMessage paquetChefLobbyQuit = new PaquetMessage(clients[i].ClientiD, 99996,new List<List<string>> { new List<string> { "0" }});
                                //     Unicast(paquetChefLobbyQuit,clients[i]);
                                // }
                                listeDesJoueursdansLobby += "|" + joueurloco.pseudo;
                            }
                            

                            PaquetMessage paquetListeDesjoueurs = new PaquetMessage(clients[i].ClientiD, 99997,new List<List<string>> { new List<string> { listeDesJoueursdansLobby }});
                            Broadcast(paquetListeDesjoueurs, lobbysList[lobbyIndex].ListPlayer);

                        // If the lobby becomes empty, remove the lobby
                        if (lobby.nbrPlayerInLobby == 0) {
                            lobbysList.RemoveAt(lobbyIndex);
                            Console.WriteLine("index liste",IndexList);
                            IndexList--;
                            Console.WriteLine("index liste",IndexList);

                            Console.WriteLine("Lobby ID " + lobby.IdLobby + " supprimé car vide.");
                            
                            

                        } else {
                            // Notify remaining players in the lobby about the player leaving
                            PaquetMessage playerLeftMessage = new PaquetMessage(clients[i].ClientiD, 9999, new List<List<string>> { new List<string> { clients[i].pseudo + " a quitté le lobby." } });
                            Unicast(playerLeftMessage, clients[i]);
                        }
                    } else {
                        PaquetMessage errJoueurNonTrouve = new PaquetMessage(0,9999,new List<List<string>> { new List<string> { "Erreur : joueur introuvable dans le lobby." }});
                        Unicast(errJoueurNonTrouve, clients[i]);
                    }
                } else {
                    PaquetMessage joueurPasDansLobby = new PaquetMessage(0,9999,new List<List<string>> { new List<string> { "Vous n'êtes pas dans un lobby." }});
                    Unicast(joueurPasDansLobby, clients[i]);
                }
                break;
            
            // PIOCHER CARTE WAGON SUR LA PILE
            case 10005:
                lobbyIndex = FindLobbyIndexByPlayerId(clients[i].ClientiD);
                joueurSelectionne = lobbysList[lobbyIndex].jeu.GetJoueur(clients[i].ClientiD);
                Console.WriteLine("j'ai recu une demande de carte wagon de la pioche du client:" + clients[i].ClientiD);
                CarteWagon laCarteWagonDemandee;
                laCarteWagonDemandee = lobbysList[lobbyIndex].jeu.DemandePileWagon(joueurSelectionne);
                //Console.WriteLine();
                PaquetMessage dataWagonRecu = new PaquetMessage(clients[i].ClientiD, 20005, new List<List<string>> { new List<string> {laCarteWagonDemandee.Identifiant.ToString()} });

                //foreach (CarteWagon carte in joueurSelectionne.Wagons)
                //{
                  //  Console.WriteLine(carte.ToString());
                //}
                Unicast(dataWagonRecu, clients[i]);

                break;
                
            // CHOISIR CARTE WAGON SUR LE COTE
            case 10010:
                lobbyIndex = FindLobbyIndexByPlayerId(clients[i].ClientiD);
                joueurSelectionne = lobbysList[lobbyIndex].jeu.GetJoueur(clients[i].ClientiD);
                Console.WriteLine("j'ai recu une demande de carte wagon de la table, du client:" + clients[i].ClientiD);
                string idCarteWagonChoisieEnString=data.Parametres[0][0];
                int idCarteWagonChoisie = int.Parse(idCarteWagonChoisieEnString);
                PaquetMessage dataWagonPlateau = new PaquetMessage(clients[i].ClientiD, 20010 , new List<List<string>> { new List<string> { lobbysList[lobbyIndex].jeu.pileManager.SurTable[idCarteWagonChoisie].Identifiant.ToString() } });
                Unicast(dataWagonPlateau, clients[i]);
                break;

            case 10011: //reçoit l'id de la carte à changer, renvoi le nouvelle list à tous les joueurs
                lobbyIndex = FindLobbyIndexByPlayerId(clients[i].ClientiD);
                joueurSelectionne = lobbysList[lobbyIndex].jeu.GetJoueur(clients[i].ClientiD);
                Console.WriteLine("j'ai recu une demande de carte wagon de la table, du client:" + clients[i].ClientiD);
                string carteWagonChoisieEnString=data.Parametres[0][0];
                int carteWagonChoisie = int.Parse(carteWagonChoisieEnString);

                lobbysList[lobbyIndex].jeu.DemandeTableWagon(joueurSelectionne, carteWagonChoisie);
                //lobbysList[lobbyIndex].jeu.pileManager.SurTable
                List<List<string>> wagonsEnvoyer = new List<List<string>>();
                for (int j = 0; j < 5; j++)
                {
                    wagonsEnvoyer.Add(new List<string> {lobbysList[lobbyIndex].jeu.pileManager.SurTable[j].Identifiant.ToString()});
                }
                Console.WriteLine(wagonsEnvoyer[1][0]);

                PaquetMessage wagonsEnvoyerPaquet = new PaquetMessage(clients[i].ClientiD,20100,wagonsEnvoyer);
                Broadcast(wagonsEnvoyerPaquet,lobbysList[lobbyIndex].ListPlayer);

                break;
            
                
            

            // DEMANDE DE CARTES MISSIONS: le joueur demande à piocher les 3 cartes missions
            case 10020:
                lobbyIndex = FindLobbyIndexByPlayerId(clients[i].ClientiD);

                joueurSelectionne = lobbysList[lobbyIndex].jeu.GetJoueur(clients[i].ClientiD);
                Console.WriteLine("demande de pioche et d'affichage de cartes missions du joueur:" + clients[i].ClientiD);

                //creation et envoie carte mission
                List<List<string>> listcm = new List<List<string>>();
                lesCartesMissions = lobbysList[lobbyIndex].jeu.DemandeMissions(joueurSelectionne);
                for (int z = 0; z < lesCartesMissions.Length; z++)
                {
                    listcm.Add(transformationTabString(lesCartesMissions[z]));
                }

                PaquetMessage cm2Send = new PaquetMessage(clients[i].ClientiD, 20020, listcm);
                Unicast(cm2Send,clients[i]);
                break;


            

            //ICI LE JOUEUR CHOISIES SES CARTES PARMIS LES 3 CARTES PROPOSEES
            case 10025:
                lobbyIndex = FindLobbyIndexByPlayerId(clients[i].ClientiD);
                // on doit en premier recevoir l'id de la carte a defausse
                Console.WriteLine("le client: " + clients[i].ClientiD + "est entrain de choisir ses cartes...");
                //List<CarteMission> listeDesChoix;
                int idCarte2Defausse = int.Parse(data.Parametres[0][0]);
                CarteMission cart2Defausse = lobbysList[lobbyIndex].jeu.GetJoueur(clients[i].ClientiD).getMission(idCarte2Defausse);
                lobbysList[lobbyIndex].jeu.pileManager.defausserMission(cart2Defausse);
                //listeDesChoix = EnsembleChoix(lesCartesMissions, lesChoixDuJoueur);
                
                //(List<CarteMission>, List<CarteMission>) res = lobbysList[lobbyIndex].jeu.EnsembleChoix(lesCartesMissions, lesChoixDuJoueur);
                Console.WriteLine("carte defausse est" + cart2Defausse.ToString());

				break;

            // AJOUTER ROUTE
            case 10040:
                lobbyIndex = FindLobbyIndexByPlayerId(clients[i].ClientiD);

                joueurSelectionne = lobbysList[lobbyIndex].jeu.GetJoueur(clients[i].ClientiD);
                string idDelaRouteEnString;
                idDelaRouteEnString = data.Parametres[0][0];
                int idDelaRouteEnId;
                idDelaRouteEnId = int.Parse(idDelaRouteEnString);

                lobbysList[lobbyIndex].jeu.DemandeAjoutDeRoute(joueurSelectionne, idDelaRouteEnId);


                //ici on annonce à tout le monde la route que le joueur a choisi. data contient déjà l'id de la route et du l'id du joueur qui a fait l'action
                data.IdAction = 20040;
                Broadcast(data, lobbysList[lobbyIndex].ListPlayer);

                break;

            
            case 99993: // passe au joueur suivant 
                lobbyIndex = FindLobbyIndexByPlayerId(clients[i].ClientiD);
                lobbysList[lobbyIndex].jeu.PasserAuJoueurSuivant();
                PaquetMessage joueurSuivantPaquet = new PaquetMessage(clients[i].ClientiD, 99993,new List<List<string>> { new List<string> {""}});
                Unicast(joueurSuivantPaquet,lobbysList[lobbyIndex].ListPlayer[lobbysList[lobbyIndex].jeu.IdentifiantJoueurActuel]);
                // //les cartes wagons pour le joueur en lui meme 
                // List<List<string>> WS = new List<List<string>>();    
                // for (int j = 0; j < 5; j++)
                // {
                //     WS.Add(new List<string> {lobbysList[lobbyIndex].jeu.pileManager.SurTable[j].Identifiant.ToString()});
                // }

                // PaquetMessage WSP = new PaquetMessage(clients[i].ClientiD,20090,WS);
                // Broadcast(WSP,lobbysList[lobbyIndex].ListPlayer);

            break;
            
                
            case 99994: //lancer la partie + donner l'id de joueur dans lobby
                lobbyIndex = FindLobbyIndexByPlayerId(clients[i].ClientiD);
                // Broadcast(PaquetlancerPartie, lobbysList[lobbyIndex].ListPlayer);
            // Console.WriteLine("id Joueur dan lobby: " + lobbysList[lobbyIndex].ListPlayer.IndexOf(joueurloco) + " __ " + "pseudo Joueur: " + joueurloco.pseudo);
                foreach(ServerClient player in lobbysList[lobbyIndex].ListPlayer )
                {

                    PaquetMessage PaquetlancerPartie = new PaquetMessage(clients[i].ClientiD, 99994,new List<List<string>> { new List<string> {lobbysList[lobbyIndex].ListPlayer.IndexOf(player).ToString()}});
                    // PaquetMessage pseudo = new PaquetMessage(clients[i].ClientiD, 99993,new List<List<string>> { new List<string> {lobbysList[lobbyIndex].ListPlayer.IndexOf(player)}});
                    Unicast(PaquetlancerPartie,player);
                }

                

            break;    
            case 99999://case sign up
                string pseudo = data.Parametres[0][0].ToString();
                string mdp = data.Parametres[1][0].ToString();
                bool signUpResult = dbInteract.InsertPlayer(pseudo,mdp);
                if (signUpResult) {
                    clients[i].pseudo = pseudo;
                    PaquetMessage signUpResultPaquet = new PaquetMessage(0,99999,new List<List<string>> { new List<string> { "0" }});
                    Unicast(signUpResultPaquet, clients[i]);
                } else {
                    PaquetMessage signUpResultPaquet = new PaquetMessage(0,99999,new List<List<string>> { new List<string> { "1" }});
                    Unicast(signUpResultPaquet, clients[i]);
                }
                break;

            case 99998: //case Login
                string pseudoLogin = data.Parametres[0][0].ToString();
                string mdpLogin = data.Parametres[1][0].ToString();
                bool loginResult = dbInteract.Login(pseudoLogin,mdpLogin);
                
                if (loginResult) {
                    clients[i].pseudo = pseudoLogin;
                    PaquetMessage signInResultPaquet = new PaquetMessage(0,99998,new List<List<string>> { new List<string> { "0" }});
                    Unicast(signInResultPaquet, clients[i]);
                } else {
                    PaquetMessage signInResultPaquet = new PaquetMessage(0,99998,new List<List<string>> { new List<string> { "1" }});
                    Unicast(signInResultPaquet, clients[i]);
                }
                break;
            

        }


    }

    //fonction d'envoi d'un message d'erreur a un client (apres les verifs des actions)
    public void SendError(string message, ServerClient sc)
    {
        PaquetMessage data = new PaquetMessage(0, 0, new List<List<string>> { new List<string> { message } });
        Unicast(data, sc);
    }
    
    public static List<string> transformationTabString(object input)
    {
        List<string> result = new List<string>();

        // Récupérer les propriétés de l'objet
        PropertyInfo[] properties = input.GetType().GetProperties();

        // Pour chaque propriété, ajouter son nom et sa valeur à la liste de chaînes
        foreach (PropertyInfo property in properties)
        {
            string propertyName = property.Name;
            object propertyValue = property.GetValue(input, null); // Obtient la valeur de la propriété

            // Convertir la valeur en chaîne de caractères
            string valueAsString = (propertyValue != null) ? propertyValue.ToString() : "null";

            // Ajouter le nom de la propriété et sa valeur à la liste de résultats
            result.Add($"{valueAsString}");
        }

        return result;
    }

    
    
    public int FindLobbyIndexByPlayerId(int playerId)
    {
        for (int i = 0; i < lobbysList.Count; i++)
        {
            if (lobbysList[i].ListPlayer.Any(player => player.ClientiD == playerId))
            {
                return i;
            }
        }
        return -1; // Return -1 if the player is not found in any lobby
    }
    
    
}

//la definition du client qu'il recoit
public class ServerClient
{
    public int ClientiD = 0;
    //socket TCP
    public TcpClient tcp;
    public string pseudo ="Anonyme";
    public ServerClient(TcpClient tcp, int id, string pseudo)
    {
        this.pseudo = pseudo;
        this.tcp = tcp;
        this.ClientiD = id;
    }
}

