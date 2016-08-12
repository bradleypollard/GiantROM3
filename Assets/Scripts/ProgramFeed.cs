using UnityEngine;
using System.Collections;

public class ProgramFeed : MonoBehaviour {

  [Header("Players")]
  [SerializeField]
  GameObject player1;
  [SerializeField]
  GameObject player2;
  [SerializeField]
  GameObject player3;
  [SerializeField]
  GameObject player4;

  [Header("Consoles")]
  [SerializeField]
  GameObject playstation;
  [SerializeField]
  GameObject nintendo;
  [SerializeField]
  GameObject xbox;

  [Header("GamePads")]
  [SerializeField]
  GameObject playstationGamePad;
  [SerializeField]
  GameObject nintendoGamePad;
  [SerializeField]
  GameObject xboxGamePad;
  [SerializeField]
  GameObject VRGamePad;

  [Header("Logic")]
  [SerializeField]
  DemoStation demoStation;
  [SerializeField]
  Microphone microphone;

  [Header("Settings")]
  [SerializeField]
  [Range(0, 1)]
  float VRProbability;
  [SerializeField]
  [Range(2, 4)]
  int numberOfPlayers = 4;

  private string[] consoleNames = {"Playstation", "WiiU", "Xbox"};
  private string[] gamePadNames = {"Playstation GamePad", "WiiU GamePad", "Xbox GamePad", "VR GamePad"};
  private GameObject[] consolePrefabs;
  private GameObject[] gamePadPrefabs;

  private enum ConsoleType
  {
    Playstation,
    Nintendo,
    Xbox,
    None
  }

  private enum GamePadType
  {
    Playstation,
    Nintendo,
    Xbox,
    VR,
    None
  }

  private struct DemoData
  {
    public DemoData(int _playerID, ConsoleType _console, GamePadType _gamePad, int _discColour)
    {
      playerID = _playerID;
      console = _console;
      gamePad = _gamePad;
      discColour = _discColour;
    }

    public int playerID;
    public ConsoleType console;
    public GamePadType gamePad;
    public int discColour;
  }

  private struct SpeechData
  {
    public SpeechData(int _playerID)
    {
      playerID = _playerID;
    }

    public int playerID;
  }

  private struct NPCData
  {
    public NPCData(bool _isMale)
    {
      isMale = _isMale;
    }
    
    public bool isMale;
  }

  private NPCData currentNPC;
  private NPCData upcomingNPC;
  private DemoData currentDemo;
  private DemoData upcomingDemo;
  private SpeechData currentSpeech;
  private SpeechData upcomingSpeech;

  // Use this for initialization
  void Start () {
    GenerateSpeech();
    SetUpcomingSpeechToCurrent();

    GenerateDemo();
    SetUpcomingDemoToCurrent();

    GenerateNPC();
    SetUpcomingNPCToCurrent();
	}

  private void GenerateSpeech()
  {
    // Randomly select a different player
    int currSpeaker = 0;
    if (!Equals(currentSpeech, default(SpeechData)))
    {
      currSpeaker = currentSpeech.playerID;
    }

    int upcomingSpeaker = currSpeaker;
    while (upcomingSpeaker == currSpeaker)
    {
      upcomingSpeaker = Random.Range(1, numberOfPlayers + 1);
    }

    // Assign upcoming speech
    upcomingSpeech = new SpeechData(upcomingSpeaker);
  }

  private void GenerateDemo()
  {
    // Randomly select a different player
    int currPlayer = 0;
    if (!Equals(currentDemo, default(DemoData)))
    {
      currPlayer = currentDemo.playerID;
    }

    int upcomingPlayer = currPlayer;
    while (upcomingPlayer == currPlayer)
    {
      upcomingPlayer = Random.Range(1, numberOfPlayers + 1);
    }

    // Randomly select a console and controller
    ConsoleType upcomingConsole = (ConsoleType)Random.Range(0, 3);
    GamePadType upcomingGamePad = (GamePadType)upcomingConsole;

    // Randomly select disc colour
    int colour = Random.Range(1, 4);

    if (Random.Range(0f,1f) < VRProbability)
    {
      upcomingGamePad = GamePadType.VR;
    }

    // Assign upcoming demo
    upcomingDemo = new DemoData(upcomingPlayer, upcomingConsole, upcomingGamePad, colour);

  }

  private void GenerateNPC()
  {
    upcomingNPC = new NPCData(Random.Range(0,2) == 0);
  }

  public void OnSpeechFinished()
  {
    SetUpcomingSpeechToCurrent();
  }

  private void SetUpcomingSpeechToCurrent()
  {
    currentSpeech = upcomingSpeech;
    microphone.speakerID = currentSpeech.playerID;
    GenerateSpeech();
  }

  public void OnDemoFinished()
  {
    demoStation.EjectAll();
    SetUpcomingDemoToCurrent();
  }

  private void SetUpcomingDemoToCurrent()
  {
    currentDemo = upcomingDemo;
    demoStation.playerID = currentDemo.playerID;
    demoStation.expectedConsoleName = consoleNames[(int)currentDemo.console];
    demoStation.expectedGamePadName = gamePadNames[(int)currentDemo.gamePad];
    demoStation.expectedDiscColour = currentDemo.discColour;
    GenerateDemo();
  }

  public void OnNPCFinished()
  {
    SetUpcomingNPCToCurrent();
  }

  private void SetUpcomingNPCToCurrent()
  {
    currentNPC = upcomingNPC;
    // TODO: Set NPC dropoff variables
    GenerateNPC();
  }

  // Update is called once per frame
  void Update () {
	  if (Input.GetKeyDown(KeyCode.Q))
    {
      // TODO: Remove this debug input
      SetUpcomingDemoToCurrent();
      SetUpcomingNPCToCurrent();
    }
	}

  void OnGUI()
  {
    GUI.Label(new Rect(0, 0, 200, 20), "Current speaker: " + currentSpeech.playerID);
    GUI.Label(new Rect(0, 30, 200, 20), "Upcoming speaker: " + upcomingSpeech.playerID);

    GUI.Label(new Rect(200, 0, 400, 20), "Current demoer: " + currentDemo.playerID 
      + ". Game " + currentDemo.discColour + " on " + consoleNames[(int)currentDemo.console] + " with " + gamePadNames[(int)currentDemo.gamePad]);
    GUI.Label(new Rect(200, 30, 400, 20), "Upcoming demoer: " + upcomingDemo.playerID 
      + ". Game " + currentDemo.discColour + " on " + consoleNames[(int)upcomingDemo.console] + " with " + gamePadNames[(int)upcomingDemo.gamePad]);

    GUI.Label(new Rect(600, 0, 200, 20), "Current NPC: " + (currentNPC.isMale ? "Male" : "Female"));
    GUI.Label(new Rect(600, 30, 200, 20), "Upcoming NPC: " + (upcomingNPC.isMale ? "Male" : "Female"));
  }
}
