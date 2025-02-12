
public class ServerMain
{
    public static void Main(string[] args)
    {
        // Créez une instance de votre serveur et lancez-le dans un thread séparé
        Server server = new Server();
        server.Init();
        Console.WriteLine("Le serveur est lancé:");
        server.Update();
    }    
}
