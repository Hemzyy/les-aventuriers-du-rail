
public class Voisin
{
    private readonly int Route;
    private readonly Noeud Ville;
    private readonly int Distance;

    public Voisin(int route, Noeud ville, int distance)
    {
        this.Route = route;
        this.Ville = ville;
        this.Distance = distance;
    }

    public int GetRoute()
    {
        return Route;
    }

    public Noeud GetVille()
    {
        return Ville;
    }

    public int GetDistance()
    {
        return Distance;
    }
}
