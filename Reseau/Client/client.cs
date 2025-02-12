using System;
using System.Net.Sockets;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
 

public class Client
    {
        private bool socketReady;
        public TcpClient socket;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;
        private Thread updateThread; 

        //nous avons besoin d'un point d'entree ou on decide de se connecter
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
                Debug.WriteLine("socket error: "+e);
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
            //le while est a degager lors de l'utilisation de unity
            while (true)
            {
                if (socketReady)
                {
                    if (stream.DataAvailable)
                    {
                        //string data = reader.ReadLine();
                        byte[] data = new byte[socket.Available];
                        socket.GetStream().Read(data, 0, data.Length);
                        if (data != null)
                        {   
                            OnIncomingData(data);
                        }
                    }
                }
                Thread.Sleep(100); // pour éviter de trop consommer le processeur
            }
        }
        
        //lire les messages depuis le serveur
        private void OnIncomingData(byte[] jsonData)
        {   
            try
            {
                // Désérialiser les données JSON en une chaîne de caractères
                string data = JsonSerializer.Deserialize<string>(jsonData);
                Console.WriteLine(data);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        //envoyer les messages vers le serveur
        public void Send(string data,TcpClient cl)
        {   
            
            if (!socketReady)
            {
                return;
            }
            try
            {
                byte[] jsonString = JsonSerializer.SerializeToUtf8Bytes(data);
                stream.Write(jsonString, 0, jsonString.Length);
                stream.Flush();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
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


    
