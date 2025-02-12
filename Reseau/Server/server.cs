using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;


    public class Server
    {
        public int port = 10000;
        private List<ServerClient> clients;
        private List<ServerClient> disconnectList;

        private Thread updateThread;
        //le serveur en lui meme
        private TcpListener server;
        private bool serverStarted;
        public int ClientId=0;
        
        
        //ici pour faire mes test lobby 
        private List<Lobby> lobbysList;
        private int IdLobby = 1;
        private int IndexList = 0;
        private List<String> codePourTester = new List<string> { "30","40","50","60","70","80"};
        
        //lance le server
        public void Init()
        {
            //a ajouter lors de la liaison avec unity pour pouvoir changer de scence
            //DontDestroyOnLoad(gameObject);

            clients = new List<ServerClient>();
            disconnectList = new List<ServerClient>();

            //section lobby 
            lobbysList = new List<Lobby>();
            //fin section lobby
            
            
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
        
        public void StartUpdateThread()
        {
            updateThread = new Thread(Update);
            updateThread.IsBackground = true; // defini en arriere plan de tel sort qu'il se termine lorsque le programme principale s'arrete
            updateThread.Start();
        }
        
        public void StopUpdateThread()
        {
            //on verifie deja si le thread est bel est bien existant avant dd'essayer de l'arreter
            if (updateThread != null && updateThread.IsAlive) 
            {
                updateThread.Abort();
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
                for(int i=0 ; i < clients.Count ; i++)
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
                            byte[] data = new byte[clients[i].tcp.Available];
                            s.Read(data, 0, data.Length);
                            
                            if (data != null)
                            {
                                //handleData(data,clients, i,reader);
                                handleData(data,clients, i);
                            }

                        }
                    }
                }

                //une boucle pour deconnecter les clients
                for (int i = disconnectList.Count - 1 ; i >= 0; i--)
                {
                    Console.WriteLine("client id: ("+ disconnectList[i].ClientiD + ") deconnecter.");
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

            ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar),ClientId);
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


        //pour envoyer les data a tout les clients
        public void Broadcast(string data, List<ServerClient> lsc)
        {
            foreach (ServerClient sc in lsc)
            {
                try
                {
                    if (sc.tcp != null && IsConnected(sc.tcp))
                    {   
                        byte[] jsonString = JsonSerializer.SerializeToUtf8Bytes(data);
                        NetworkStream stream = sc.tcp.GetStream();
                        stream.Write(jsonString, 0, jsonString.Length);
                        stream.Flush();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
        //pour envoyer a un seul client
        public void Send(string data,ServerClient sc)
        {
            try
            {
                if (sc.tcp != null && IsConnected(sc.tcp))
                {
                    byte[] jsonString = JsonSerializer.SerializeToUtf8Bytes(data);
                    NetworkStream stream = sc.tcp.GetStream();
                    stream.Write(jsonString, 0, jsonString.Length);
                    stream.Flush();
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
            Console.WriteLine("message recu de IdClient " +"("+ c.ClientiD+")"+" : "+ data );
        }

        //pour traiter les data recu
        public void handleData(byte[] jsonData, List<ServerClient> clients, int i)
        {   
            
            
            
            //je vais chercher le lobby du joueur dans le quel il est pour pouvoir faire un broadcast que au joueurs se trouvant dans le lobby
            int lbIndex = lobbysList.FindIndex(lobby => lobby.ListPlayer.Any(player => player.ClientiD == clients[i].ClientiD));
            string data = JsonSerializer.Deserialize<string>(jsonData);
            string[] aData = data.Split('|');
            switch (aData[0])
            {
                case "clickOnRail":
                    Broadcast("le joueur id: (" + clients[i].ClientiD + ") a cliqué sur le rail id: " + aData[1],lobbysList[lbIndex].ListPlayer);
                    break;

                case "blindDraw":
                    Broadcast("le joueur id: (" + clients[i].ClientiD + ") a pioché une carte",lobbysList[lbIndex].ListPlayer);
                    break;

                case "pickTwo":
                    Broadcast("le joueur id: (" + clients[i].ClientiD + ") a choisi les cartes: " + aData[1] + " et " + aData[2],lobbysList[lbIndex].ListPlayer);
                    break;
                
                case "host":
                    if (IndexList >= 0 && IndexList < 7)
                    {
                        //je dois verifier si le client n'est pas deja dans un lobby et ne pas creer un autre encore 
                        //je dois mettre des codes differents pour tester plus
                        
                        bool ExistingPlayer = (lobbysList != null) && lobbysList.Any(lobby => lobby.ListPlayer.Any(player => player.ClientiD == clients[i].ClientiD));
                        if(!ExistingPlayer)
                        {   
                            //pour l'instant juste pour piocher un code dans la liste 
                            Lobby lb = new Lobby(IdLobby, codePourTester[IndexList], clients[i]);
                            lobbysList.Add(lb);
                            lobbysList[IndexList].createLobby();
                            Send("votre code lobby: " + codePourTester[IndexList],clients[i]);
                            IndexList++;
                            IdLobby++;
                        }
                        else
                        {
                            Send("vous etes deja dans un lobby existant",clients[i]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("nombre de lobby max atteint");
                    }
                    break;

                case "join":
                   // string codeTmp = aData[1]; // Assuming this is meant to read from the console; adjust accordingly if it's not.
                   //je dois pas pouvoir rejoindre si le joueur est deja dans un lobby a condition qu'il quitte 
                   bool ExistingPlayerJoin = (lobbysList != null) && lobbysList.Any(lobby => lobby.ListPlayer.Any(player => player.ClientiD == clients[i].ClientiD));
                   if (!ExistingPlayerJoin)
                   {
                       if (lobbysList != null && lobbysList.Any(lobby => lobby.codeLobby == aData[1]))
                       {
                           int lobbyIndex = lobbysList.FindIndex(lobby => lobby.codeLobby == aData[1]);
                           if (lobbysList[lobbyIndex].nbrPlayerInLobby < 4)
                           {
                               lobbysList[lobbyIndex].joinLobby(data, clients[i]);
                           }
                           else
                           {
                               Send("nombre de joueur max atteint", clients[i]);
                           }
                       }
                       else
                       {
                           Console.WriteLine("le lobby n'existe pas");
                       }
                   }
                   else
                   {
                       Send("vous etes deja dans un lobby existant",clients[i]);
                   }

                   break;

                default:
                    OnIncomingData(clients[i], data);
                    break;
            }
        }
    }

    //la definition du client qu'il recoit
    public class ServerClient
    {
        public int ClientiD=0;
        //socket TCP
        public TcpClient tcp;

        public ServerClient(TcpClient tcp, int id)
        {   
            this.tcp = tcp;
            this.ClientiD = id;
        }
    }
