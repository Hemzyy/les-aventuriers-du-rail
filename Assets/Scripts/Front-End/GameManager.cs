using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Color = UnityEngine.Color;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int NumberOfColors;
    public int NumberOfPlayers;
    public int NumberOfMissions;
    public List<Player> PlayerList = new List<Player>();
    public List<CardTrain> CardTrainList = new List<CardTrain>();
    public List<CardMission> CardMissionList = new List<CardMission>();
    public List<CardTrain> PiocheWagonList = new List<CardTrain>();
    public List<CardMission> PiocheMissionList = new List<CardMission>();
    public int CurrentPlayerIndex;
    public GameObject PiocheWagon;
    public GameObject PlayerNamesDisplay;
    public GameObject PlayHandDisplay;
    public GameObject showMissionBtn;
    public GameObject PlayMissionContainer;
    public GameObject Timer;
    public GameObject FeedBackMessageDisplay;
    public GameObject FenetrePiocheMission;
    public GameObject FenetreMissionJoueur;
    public GameObject Routes;
    public int CartesRestantentAPiocherPendantCeTour;
    public Color[] playercolors = {Color.red, Color.green, Color.blue, Color.yellow};
    public float max_time = 30.0f;

    private Client client;

    void Awake()
    {
        Instance = this;
	}

    void Start()
    {
        client = Client.Instance;
        if (client == null)
        {
            Debug.LogError("Client instance not found.");
        }

        
        //string[] playernames = {"jose","raoul","jeanlouis","micro"};
        string[] hexColors = {"#FF0414", "#286DE7", "#1FAB2B", "#ECF338","#FE42AD","#717171"};
        string[] symboles = {"R","B","V","J","P","X"};
        NumberOfColors = hexColors.Length;
        NumberOfPlayers = client.nomJoueursDansLeLobby.Count;
        CurrentPlayerIndex = Client.Instance.idPlayerLobby;

        InitCards(hexColors, symboles);
        InitPlayers(NumberOfPlayers, playercolors, client.nomJoueursDansLeLobby);
        InitTrainDraw();

		/*CardMissionList.Add(new CardMission("Bassin du commerce", "Montagne Verte", 10));
        CardMissionList.Add(new CardMission("Etoile-Bourse", "Carpe Haute", 5));
        CardMissionList.Add(new CardMission("Lycee Kleber", "Place de Hagueunau", 1));
        CardMissionList.Add(new CardMission("Petite France", "Etoile-Bourse", 2));
        CardMissionList.Add(new CardMission("Kehl", "Rotonde", 3));
        CardMissionList.Add(new CardMission("Arcelor Mittal", "Montagne Verte", 4));
        CardMissionList.Add(new CardMission("Citadelle", "Gare Central", 5));
        CardMissionList.Add(new CardMission("Esplanade", "Kehl", 6));
        CardMissionList.Add(new CardMission("Republique", "Landsberg", 7));*/
		CardMissionList.Add(new CardMission(1, "Place de Haguenau", "Lycée Kleber", 2));
		CardMissionList.Add(new CardMission(2, "Gare Centrale", "Montagne Verte", 2));
		CardMissionList.Add(new CardMission(3, "Rotonde", "Montagne Verte", 3));
		CardMissionList.Add(new CardMission(4, "Place de Haguenau", "Petite France", 3));
		CardMissionList.Add(new CardMission(5, "Esplanade", "Bassin du Commerce", 3));
		CardMissionList.Add(new CardMission(6, "Esplanade", "Petite France", 3));
		CardMissionList.Add(new CardMission(7, "République", "Wagner", 3));
		CardMissionList.Add(new CardMission(8, "République", "Landsberg", 3));
		CardMissionList.Add(new CardMission(9, "Etoile-Bourse", "Wagner", 3));
		CardMissionList.Add(new CardMission(10, "Arcelor Mittal", "Esplanade", 4));
		CardMissionList.Add(new CardMission(11, "République", "Citadelle", 4));
		CardMissionList.Add(new CardMission(12, "Arcelor Mittal", "Citadelle", 5));
		CardMissionList.Add(new CardMission(13, "Landsberg", "Bassin du Commerce", 5));
		CardMissionList.Add(new CardMission(14, "Lycée Kleber", "Kehl", 6));
		CardMissionList.Add(new CardMission(15, "Rotonde", "Esplanade", 6));
		CardMissionList.Add(new CardMission(16, "Kehl", "Etoile-Bourse", 6));
		CardMissionList.Add(new CardMission(17, "Gare Centrale", "Carpe Haute", 6));


		NumberOfMissions = CardMissionList.Count;
        //InitMissionDraw();
        PlayerNamesDisplay.GetComponent<DisplayPlayerNames>().DisplayPlayers(PlayerList);
        //FenetrePiocheMission.SetActive(true);
        CartesRestantentAPiocherPendantCeTour = 2;
        Timer.GetComponent<UnityEngine.UI.Image>().color = GetCurrentPlayer().playerColor;
        FenetreMissionJoueur.SetActive(false);

		foreach (var mission in Client.Instance.case20060)
		{
			Debug.Log(mission[0]);
			Debug.Log(mission[1]);
			Debug.Log(mission[2]);
			Debug.Log(mission[3]);
            Debug.Log(mission[4]);
            Debug.Log(mission[5]);
			CardMission card = new CardMission(mission[0], mission[2], int.Parse(mission[4]));
			GameManager.Instance.GetCurrentPlayer().missions.Add(card);
		}

        foreach (var wagon in Client.Instance.case20080)
        {
			Player cplayer = GameManager.Instance.GetCurrentPlayer();
            int idWagon = int.Parse(wagon[0]);
			cplayer.hand[idWagon - 1].count += 1;
		}
		PlayHandDisplay.GetComponent<InstanciateCards>().DisplayWagons(GetCurrentPlayer().hand, 0);
	}

	public Player GetCurrentPlayer()
    {
        return PlayerList[CurrentPlayerIndex];
    }

	public void NextTurn()
	{
		GameManager.Instance.GetCurrentPlayer().action = PlayerAction.Nothing;
		//CurrentPlayerIndex = (CurrentPlayerIndex + 1) % PlayerList.Count;
		//affiche la main du prochain joueur
		// PlayHandDisplay.GetComponent<InstanciateCards>().DisplayWagons(GetCurrentPlayer().hand, 0);
		// showMissionBtn.GetComponent<InstanciateCards>().DisplayPlayerMissions(GetCurrentPlayer().missions, PlayMissionContainer.transform);
		// PlayerNamesDisplay.GetComponent<DisplayPlayerNames>().DisplayPlayers(PlayerList);
		// FeedBackMessageDisplay.GetComponent<FeedBackMessage>().DisplayMessage("Au tour de "+PlayerList[CurrentPlayerIndex].playerName, Color.yellow);
		// FenetrePiocheMission.SetActive(false);
		// FenetreMissionJoueur.SetActive(false);
		FenetrePiocheMission.SetActive(false);
		CartesRestantentAPiocherPendantCeTour = 2;
		client.myTurn = false; //ce n'est plus son tour
		string senddata;
		if (GameManager.Instance.GetCurrentPlayer().wagonCount <= 2)
		{
			senddata = "99993|1";
		}
		else
		{
			senddata = "99993|0";
		}
		StartCoroutine(WaitSecondes());
		client.Send(senddata, client.socket);
		//TimerController tc = Timer.GetComponent<TimerController>();
		//tc.time_remaining = tc.max_time;
	}

	private IEnumerator WaitSecondes()
    {
        float timeout = 1f; // Timeout after 1 seconds
        float timer = 0f;

        // Wait until the client is connected or the timeout is reached
        while (timer < timeout)
        {
            yield return new WaitForSeconds(0.1f); // Wait for 100 milliseconds before checking again
            timer += 0.1f;
        }
    }


    public void InitCards(string[] hexColors, string[] symboles)
    {
        UnityEngine.Color color;
        for(int i=0; i < hexColors.Length; i++)
        {
            ColorUtility.TryParseHtmlString(hexColors[i], out color);
            CardTrainList.Add(new CardTrain(color, symboles[i]));
        }
    }
    public void InitPlayers(int n, Color[] colors, List<string> names)
    {
        for(int i=0; i < n; i++)
        {
            PlayerList.Add(new Player(names[i], colors[i]));
            for(int j=0; j < NumberOfColors; j++){
                CardTrain card = new CardTrain(CardTrainList[j].color, CardTrainList[j].symbole);
                card.count = 0;
                PlayerList[i].hand.Add(card);
            }
        }
    }



	public void InitTrainDraw()
    {
        //Appelle réseau ajout carte
        foreach (var wagon in Client.Instance.case20050)
        {
            int idWagon = int.Parse(wagon[0]);
            CardTrain card = new CardTrain(CardTrainList[idWagon-1].color, CardTrainList[idWagon-1].symbole);
            card.count = 1;
            PiocheWagonList.Add(card);
        }

        //Appelle réseau ajout carte
/*        for (int i = 0; i < 5; i++)
		{
			PiocheWagonList.Add(GetRandomCardTrain());
		}*/

		//Initiailse la pioche sur l'interface
		InstanciateCards piocheDisplayer = PiocheWagon.GetComponent<InstanciateCards>();
        if(piocheDisplayer != null){
            piocheDisplayer.DisplayWagons(PiocheWagonList, 0);
        }
        else
            Debug.Log("Pioche wagon not found.");
    }

    public CardTrain GetRandomCardTrain(){
        int rd = UnityEngine.Random.Range(0, NumberOfColors);
        CardTrain card = new CardTrain(CardTrainList[rd].color, CardTrainList[rd].symbole);
        card.count = 1;
        return card;
    }

    public void InitMissionDraw()//List<List<string>> packet)
    {
        
        PiocheMissionList.Clear();
        List<int> rd_numbers = new List<int>();
        for(int i = 0; i < NumberOfMissions && i < 3; i++){
            int rd;
            do{
                rd = UnityEngine.Random.Range(0, NumberOfMissions);
            } while(rd_numbers.Contains(rd));
            rd_numbers.Add(rd);
            PiocheMissionList.Add(CardMissionList[rd]);
        }
        
        /*
        PiocheMissionList.Clear();
        for(int i=0; i < packet.Count; i++){
            int id , points = 0;
            Int32.TryParse(packet[i][0], out id);
            Int32.TryParse(packet[i][3], out points);
            PiocheMissionList.Add(new CardMission(id, packet[i][1], packet[i][2],points));
        }
        */
    }

    //trouve une route parmis toutes les routes, de id id
    public Route FindRoute(int id, GameObject routeContainer)
    {
        foreach(Transform route in routeContainer.transform){
            Route routecompo = route.GetComponent<Route>();
            if(routecompo.idRoute == id){
                return routecompo;
            }
        }
        return null;
    }

    public enum PlayerAction {
        Options,
        PiocheMission,
        PiocheWagon,
        ConstruireRoute,
        Nothing
    }
}

