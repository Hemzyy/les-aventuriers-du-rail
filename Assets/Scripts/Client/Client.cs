using System;
using System.Net.Sockets;
using System.IO;
using clientPackage;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class Client : MonoBehaviour
    {
        public static Client Instance { get; private set; }
        private bool socketReady;
        public TcpClient socket;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;
        private Thread updateThread; 

        public string MdpDuLobby;
        public bool LobbyExists;
        public bool connectedABDD;
        public bool chefDuLobby;
        public List<string> nomJoueursDansLeLobby;
        public bool myTurn;


        public List<List<string>> case20005;
        public List<List<string>> case20010;
        public List<List<string>> case20020;
        public List<List<string>> case20040;
        public List<List<string>> case20050;
	    public List<List<string>> case20060;
        public List<List<string>> case20080;
        public List<List<string>> case20090;
        public List<List<string>> case20100;
        public List<List<string>> case99000;
        public List<String> scoreJoueur;


	public int idPlayerLobby;

        //nous avons besoin d'un point d'entree ou on decide de se connecter
        private void Awake()
        {
            Application.runInBackground = true;
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void Start()
        {
            string serverAddress = "185.155.93.105"; // Server IP address
           // string serverAddress = "localhost"; // Server IP address
            // int serverPort = 11000; // Server port
            int serverPort = 11001; // Server port
            //int serverPort = 10001; // Server port
            ConnectToServer(serverAddress, serverPort);
            StartUpdateThread();
        }
        public bool ConnectToServer(string host, int port)
        {
            if (socketReady) // si on est déja connecter pas la peine de se reconnecter
                return false;

            try
            {
                socket = new TcpClient(host, port);
                stream = socket.GetStream();
                writer = new StreamWriter(stream);
                reader = new StreamReader(stream);
                socketReady = true;
            }
            catch(Exception e)
            {
                Debug.Log("socket error: "+e);
            }

            return socketReady;
        }
        
        
        //je vais crée un thread pour que la fonction update soit tout le temps lancer en permanance au cas ou il ya des messages qui arrivent du server
        //lors de l'utilisation de unity on pourra enlever ce thread car la fonction update est prise en compte automatiquement
        public void StartUpdateThread()
        {
            updateThread = new Thread(Update);
            updateThread.IsBackground = true; // defini en arriere plan de tel sort qu'il se termine lorsque le programme principale s'arrete
            updateThread.Start();
        }
        //fonction pour arreter le thread
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
            // Le while boucle infinie n'est peut-être pas nécessaire, selon votre implémentation dans Unity.
            // Si vous utilisez Unity, vous pouvez gérer cet aspect différemment selon le modèle de mise à jour de votre jeu.
           
            if (socketReady && stream.DataAvailable)
            {
                try
                {
                    string jsonData = reader.ReadLine(); // Read the JSON string directly from the stream
                    if (jsonData != null)
                    {
                        OnIncomingData(jsonData);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Read error: " + e);
                }
            }
        }
        
        private void OnIncomingData(string data)
        {   
            try
            {
                // Try to deserialize the data as a PaquetMessage object.
                PaquetMessage message = JsonConvert.DeserializeObject<PaquetMessage>(data);

                // If deserialization was successful, handle the message.
                HandlePacket(message);
            }
            catch (JsonException e)
            {
                // If deserialization failed, log an error message or take appropriate action.
                //Console.WriteLine("Failed to deserialize data as PaquetMessage.");
                
                //print the deserialized data
                //string dataString = Encoding.UTF8.GetString(data);
                Console.WriteLine(e);

            }
        }


        private void HandlePacket(PaquetMessage message)
        {
            // Handle the message based on its action ID.
            switch (message.IdAction)
            {
                case 0: //ici on reçoit le mdp du lobby et on l'affiche sur l'ihm
                    verif(socket);
                    Debug.Log("debut du case 0");                    
                    string MdpDuLobby2 = message.Parametres[0][0]; //ici on stock le mdp pour l'afficher dans l'IHM
                    //affiche sur l'ihm le mdp du lobby
                    //transform.parent.parent.Find("Affiche_MdpLobby").GetComponent<TextMeshProUGUI>().text = MdpDuLobby2;
                    MdpDuLobby = MdpDuLobby2;
                    Debug.Log("FIN du case 0 et le mdpduLobby:" + MdpDuLobby );
                    break;

                case 2: //playr got a paquetMessage with the IDs of three cards that he will have to pick from
                    verif(socket);
                    Console.WriteLine("vous avez quitter le lobby");
                    break;

                case 20005:
                    case20005 = new(message.Parametres);
                    verif(socket);
                    Console.WriteLine("Vous (joueur: " + message.IdJoueur + ") avez reçu la carte:  " + message.Parametres[0][0]);
                    break;

                case 20010:
				    case20010 = new(message.Parametres);
				    verif(socket);
                    Console.WriteLine("Vous (joueur: " + message.IdJoueur + ") avez dmandé l'affichage de la carte wagons:  ");

                    // Récupérer la première liste de Parametres qui contient les missions
                    List<string> wagonsListe = message.Parametres[0];

                    // Récupérer la première (et unique) mission en tant que chaîne de caractères
                    string WagonsEnString = wagonsListe[0];

                    // Afficher les missions
                    Console.WriteLine(WagonsEnString);

                    break;


                case 20020: // AFFICHAGE DES CARTES MISSION
                    case20020 = new(message.Parametres);

				    verif(socket);
                    // on fait boucle pour afficher les cartes missions une à une
                    Console.WriteLine("Voici les cartes missions qu'on vous propose:");
                    for (int i = 0; i < message.Parametres.Count; i++)
                    {
                        Console.WriteLine($"Card {i + 1}:");
                        foreach (string detail in message.Parametres[i])
                        {
                            Console.WriteLine(detail);
                        }
                        Console.WriteLine(); // Add an empty line for better readability
                    }

                    break;
                
                case 20040: // ICI TOUS LES CLIENTS RECOIVENT LES INFORMATIONS D UN JOUEUR QUI A CHOISI UNE ROUTE
							// LE 20040 VIENT FORCEMENT D UN BROADCAST (REGARDER server.cs le case 10040 du switch, on fait un broadcast)
				    case20040 = new(message.Parametres);
				    verif(socket);
                    Debug.Log("Le joueur:" + message.IdJoueur + " a choisi la route d'iD " + message.Parametres[0][0]);
                    Route route = GameManager.Instance.FindRoute(int.Parse(message.Parametres[0][0]), GameManager.Instance.Routes);
                    route.CaptureRoute(route.transform, GameManager.Instance.playercolors[message.IdJoueur]);
                    GameManager.Instance.PlayerList[message.IdJoueur].wagonCount -= route.nbrTiles;
                    GameManager.Instance.PlayerNamesDisplay.GetComponent<DisplayPlayerNames>().DisplayPlayers(GameManager.Instance.PlayerList);
                    break;
                
                case 20050:
                    case20050 = new(message.Parametres);

				    verif(socket);
                    Console.WriteLine("vous avez recu les carte wagons sur table suivante: "+message.ToString());
                    break;
                case 20060:
                    case20060 = new(message.Parametres);
				    verif(socket);
                    Console.WriteLine("vos cartes missions :"+message.ToString());
                    break;
                
                case 20070:
                    verif(socket);
                    Console.WriteLine(message.ToString());
                    break;
                case 20080:
                    case20080 = new(message.Parametres);

				    verif(socket);
                    Console.WriteLine("vous avez recu vos cartes wagons suivante :"+ message.ToString());
                    break;
                

                case 20100: // reçoit la full pioche + maj client
                    case20100 = new(message.Parametres);
                    verif(socket);
                    GameManager.Instance.PiocheWagonList.Clear();
                    foreach (var wagon in message.Parametres)
			        {
				        int idWagon = int.Parse(wagon[0]);
				        CardTrain card = new CardTrain(GameManager.Instance.CardTrainList[idWagon-1].color, GameManager.Instance.CardTrainList[idWagon-1].symbole);
				        card.count = 1;
				        GameManager.Instance.PiocheWagonList.Add(card);
			        }
                    GameManager.Instance.PiocheWagon.GetComponent<InstanciateCards>().DisplayWagons(GameManager.Instance.PiocheWagonList, 0);
                    break;

                /*case 20090:
                    case20090 = new(message.Parametres);
                    GameManager.Instance.PiocheWagonList.Clear();
                    Debug.Log(GameManager.Instance.CardTrainList[0]);
			        foreach (var wagon in Client.Instance.case20090)
			        {
                        
				        int idWagon = int.Parse(wagon[0]);
				        CardTrain card = new CardTrain(GameManager.Instance.CardTrainList[idWagon].color, GameManager.Instance.CardTrainList[idWagon].symbole);
				        card.count = 1;
				        GameManager.Instance.PiocheWagonList.Add(card);
			        }
				verif(socket);
                    break;*/


                /*
                case 99980:
                    verif(socket);
                    if (message.Parametres[0][0] == "")
                    {
                        connectedABDD = true;
                    }
                    break;
                */
                
                case 99993:
                    verif(socket);
                    Debug.Log("Changement de tour!! myid = "+idPlayerLobby);
                    Debug.Log("///////////////   "+message.Parametres[0][0]+"   /////////");
                    if(int.Parse(message.Parametres[0][0]) == idPlayerLobby){
                        GameManager.Instance.FeedBackMessageDisplay.GetComponent<FeedBackMessage>().DisplayMessage("C'est à votre tour de jouer.",Color.green);
                        myTurn = true;
                        GameManager.Instance.CartesRestantentAPiocherPendantCeTour = 2;
                    }
                    else{
                        GameManager.Instance.FeedBackMessageDisplay.GetComponent<FeedBackMessage>().DisplayMessage("C'est au tour du joueur "+message.Parametres[0][0],Color.yellow);
                        myTurn = false;
                    }
                    GameManager.Instance.Timer.GetComponent<TimerController>().timerFilling.color = GameManager.Instance.GetCurrentPlayer().playerColor;
                    GameManager.Instance.Timer.GetComponent<TimerController>().time_remaining = GameManager.Instance.max_time;
                    break;
                
                
                
                case 99999:
                    verif(socket);
                    if (message.Parametres[0][0] == "0")
                    {
                        connectedABDD = true;
                    }
                    else
                    {
                        connectedABDD = false;
                    }
                    Console.WriteLine(message.ToString());
                    break;
                
                case 99998:
                    verif(socket);
                    if (message.Parametres[0][0] == "0")
                    {
                        connectedABDD = true;
                    }
                    else
                    {
                        connectedABDD = false;
                    }
                    Debug.Log(message.ToString());
                    break;
            

                
                case 99997:
                    verif(socket);
                    nomJoueursDansLeLobby.Clear();
                    string[] msg_split = message.Parametres[0][0].Split("|");
                    for(int i=1; i< msg_split.Length; i++){
                        Debug.Log("pseudo = "+msg_split[i]);
                        nomJoueursDansLeLobby.Add(msg_split[i]);
                    }
                    Debug.Log("list = "+nomJoueursDansLeLobby);
                    break;
                
                case 99996:
                    verif(socket);
                    if (message.Parametres[0][0] == "0")
                    {
                        chefDuLobby = true;
                    }
                    else if (message.Parametres[0][0] == "1")
                    {
                        chefDuLobby = false;
                    }
                    break;
                    
                case 99994: //Commence la partie
                    verif(socket);
                    Debug.Log("il est digne");
                    Debug.Log("id joueur = "+message.Parametres[0][0]);
                    idPlayerLobby = int.Parse(message.Parametres[0][0]);

                    if (idPlayerLobby == 0)
                    {
                        myTurn = true;
                    }

                    SceneManager.LoadScene("Game");
                    break;
                
                
                case 9999:
                    verif(socket);
                    //Console.WriteLine(message.Parametres.ToString());
                    //if the first parameter is 0, the lobby does not exist
                    if (message.Parametres[0][0] == "0")
                    {
                        LobbyExists = true;
                        //Console.WriteLine("Le lobby n'existe pas");
                    }
                    else if (message.Parametres[0][0] == "1")
                    {
                        LobbyExists = false;
                        Debug.Log(LobbyExists.ToString());
                    }
                    Debug.Log(message.ToString());
                    
                    // Handle the message as an unknown action message.
                    break;
                /*case jene sais pas:
                Load scene fin 
                fini*/
                case 99000:
                    verif(socket);
                    case99000 = message.Parametres;
				    case99000.Sort((x, y) => string.Compare(x[0], y[0]));
                    foreach (var item in case99000)
                    {
                        Debug.Log(item[1]);
                        scoreJoueur.Add(item[1]);
                    }

                    SceneManager.LoadScene("ScoreScreen");
                    break;

                default:
                    // Handle the message as an unknown action message.
                    break;
            }

            
        }
        public void verif(TcpClient cl)
        {
            PaquetMessage check = new PaquetMessage(0, 36, new List<List<string>>());
            string jsonData = JsonConvert.SerializeObject(check);
            StreamWriter writer = new StreamWriter(cl.GetStream());
                
            writer.WriteLine(jsonData);
            writer.Flush();
        }
        
        //envoyer les messages vers le serveur
        public void Send(string data, TcpClient cl)
        {   
            if (!socketReady)
            {
                return;
            }
            try
            {
                // Split the input data by '|' to extract the actionId and parameters
                string[] input = data.Split('|');

                int actionId = int.Parse(input[0]);
                List<List<string>> parametres = new List<List<string>>();

                // Iterate over the input array starting from index 1 to get the parameters
                for (int i = 1; i < input.Length; i++)
                {
                    // Split each parameter by ',' to create a list of strings
                    string[] paramParts = input[i].Split(',');
                    List<string> paramList = new List<string>(paramParts);
                    parametres.Add(paramList);
                }

                // Create a new PaquetMessage object with the actionId and parameters
                PaquetMessage message = new PaquetMessage(0, actionId, parametres);

                // Serialize the message object to JSON
                string jsonData = JsonConvert.SerializeObject(message); // Sérialisation directe en chaîne JSON

                // Send the serialized data to the server
                StreamWriter writer = new StreamWriter(cl.GetStream());
                writer.WriteLine(jsonData);
                writer.Flush();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        
        
        /* ces deux fonction permettent de fermer le socket si on quitte lablication ou desactiver sur unity
        private void OnApplicationQuit()
        {
            CloseSocket();
        }

        private void OnDisable()
        {
            CloseSocket();
        }
        */
        //si ca crache ou autre on doit pas laisser le socket tourner sur le port
        public void CloseSocket()
        {
            if (!socketReady)
                return;
            writer.Close();
            reader.Close();
            socket.Close();
            socketReady = false;
        }
        
    }