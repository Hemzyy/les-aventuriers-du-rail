

public class ClientMain
{
    public static void Main(string[] args)
    {
        Client client = new Client();
        string serverAddress = "127.0.0.1"; // Adresse IP du serveur (localhost pour un test sur la même machine)
        int serverPort = 10000; // Port sur lequel le serveur écoute
        client.ConnectToServer(serverAddress, serverPort);
        
        client.StartUpdateThread();
        
        
        while (true)
        {
            if (Console.KeyAvailable)
            {
                String input = Console.ReadLine();
                
                if (input == "close")
                {
                    client.CloseSocket();
                    return;
                }
                else
                {
                    client.Send(input,client.socket); 
                }
                
            }
        }
        
        
    }
}
