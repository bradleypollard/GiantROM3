using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ProgramFeed : MonoBehaviour
{
  [Header("NPCs")]
  [SerializeField]
  GameObject malePrefab;
  [SerializeField]
  GameObject femalePrefab;
  [SerializeField]
  GameObject maleInstance;
  [SerializeField]
  GameObject femaleInstance;
  [SerializeField]
  Transform maleSpawn;
  [SerializeField]
  Transform femaleSpawn;

  [Header("Logic")]
  [SerializeField]
  DemoStation demoStation;
  [SerializeField]
  Microphone microphone;
  [SerializeField]
  NPCZone npcZone;

  [Header("Settings")]
  [SerializeField]
  [Range(0, 1)]
  float VRProbability;
  [SerializeField]
  [Range(2, 4)]
  public int numberOfPlayers = 4;
  public Dictionary<int, int> PIDToCIDMap;

  private string[] consoleNames = { "Playstation", "WiiU", "Xbox" };
  private string[] gamePadNames = { "Playstation GamePad", "WiiU GamePad", "Xbox GamePad", "VR GamePad" };
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
    public DemoData(int _playerID, ConsoleType _console, GamePadType _gamePad, int _discColour, string _gameTitle)
    {
      playerID = _playerID;
      console = _console;
      gamePad = _gamePad;
      discColour = _discColour;
      gameTitle = _gameTitle;
    }

    public int playerID;
    public ConsoleType console;
    public GamePadType gamePad;
    public int discColour;
    public string gameTitle;
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
    public NPCData(bool _isMale, string _npcName)
    {
      isMale = _isMale;
      npcName = _npcName;
    }

    public bool isMale;
    public string npcName;
  }

  private NPCData currentNPC;
  private NPCData upcomingNPC;
  private DemoData currentDemo;
  private DemoData upcomingDemo;
  private SpeechData currentSpeech;
  private SpeechData upcomingSpeech;

  void Start()
  {
    PIDToCIDMap = new Dictionary<int, int>();
  }

  // Use this for initialization
  public void Init()
  {
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
      currSpeaker = PIDToCIDMap[currentSpeech.playerID];
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

    if (Random.Range(0f, 1f) < VRProbability)
    {
      upcomingGamePad = GamePadType.VR;
    }

    // Assign upcoming demo
    upcomingDemo = new DemoData(upcomingPlayer, upcomingConsole, upcomingGamePad, colour, CreateNewGameTitle());

  }

  private void GenerateNPC()
  {
    int randomNumber = Random.Range(0, 2);
    upcomingNPC = new NPCData(randomNumber == 0, CreateNewNPCName(randomNumber));
  }

  public void OnSpeechFinished()
  {
    SetUpcomingSpeechToCurrent();
  }

  private void SetUpcomingSpeechToCurrent()
  {
    currentSpeech = upcomingSpeech;
    microphone.speakerID = PIDToCIDMap[currentSpeech.playerID];
    GenerateSpeech();

    RenderCurrentSpeechCard();
    RenderUpcomingSpeech();
  }

  public void OnDemoFinished()
  {
    SetUpcomingDemoToCurrent();
    demoStation.EjectAll();
  }

  private void SetUpcomingDemoToCurrent()
  {
    currentDemo = upcomingDemo;
    demoStation.playerID = PIDToCIDMap[currentDemo.playerID];
    demoStation.expectedConsoleName = consoleNames[(int)currentDemo.console];
    demoStation.expectedGamePadName = gamePadNames[(int)currentDemo.gamePad];
    demoStation.expectedDiscColour = currentDemo.discColour;
    GameObject.Find(demoStation.expectedConsoleName).GetComponentInChildren<Renderer>().material.SetFloat("_OutlineTransparency", 1);
    GameObject.Find(demoStation.expectedGamePadName).GetComponentInChildren<Renderer>().material.SetFloat("_OutlineTransparency", 1);
    GenerateDemo();

    RenderCurrentDemoCard();
    RenderUpcomingDemoCard();
  }

  public void OnNPCFinished()
  {
    // Spawn new NPC
    if (currentNPC.isMale)
    {
      maleInstance = (GameObject)Instantiate(malePrefab, maleSpawn.position, Quaternion.identity);
    }
    else
    {
      femaleInstance = (GameObject)Instantiate(femalePrefab, femaleSpawn.position, Quaternion.identity);
    }

    SetUpcomingNPCToCurrent();
  }

  private void SetUpcomingNPCToCurrent()
  {
    currentNPC = upcomingNPC;
    npcZone.gender = currentNPC.isMale ? "Male" : "Female";
    
    // Highlight chosen NPC
    if (currentNPC.isMale)
    {
      foreach (Renderer r in maleInstance.GetComponentInChildren<MeshSaver>().meshes)
      {
        foreach (Material m in r.materials)
        {
          m.SetFloat("_OutlineTransparency", 1);
        }
      }
    }
    else
    {
      foreach (Renderer r in femaleInstance.GetComponentInChildren<MeshSaver>().meshes)
      {
        foreach (Material m in r.materials)
        {
          m.SetFloat("_OutlineTransparency", 1);
        }
      }
    }

    GenerateNPC();

    RenderUpcomingNPCCard();
    RenderCurrentNPCCard();
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Q))
    {
      // TODO: Remove this debug input
      SetUpcomingSpeechToCurrent();
      SetUpcomingDemoToCurrent();
      SetUpcomingNPCToCurrent();
      TestAll();
    }
  }

  //  void OnGUI()
  //  {
  //    GUI.Label(new Rect(0, 0, 200, 20), "Current speaker: " + currentSpeech.playerID);
  //    GUI.Label(new Rect(0, 30, 200, 20), "Upcoming speaker: " + upcomingSpeech.playerID);
  //
  //    GUI.Label(new Rect(200, 0, 400, 20), "Current demoer: " + currentDemo.playerID 
  //      + ". Game " + currentDemo.discColour + " on " + consoleNames[(int)currentDemo.console] + " with " + gamePadNames[(int)currentDemo.gamePad]);
  //    GUI.Label(new Rect(200, 30, 400, 20), "Upcoming demoer: " + upcomingDemo.playerID 
  //      + ". Game " + currentDemo.discColour + " on " + consoleNames[(int)upcomingDemo.console] + " with " + gamePadNames[(int)upcomingDemo.gamePad]);
  //
  //    GUI.Label(new Rect(600, 0, 200, 20), "Current NPC: " + (currentNPC.isMale ? "Male" : "Female"));
  //    GUI.Label(new Rect(600, 30, 200, 20), "Upcoming NPC: " + (upcomingNPC.isMale ? "Male" : "Female"));
  //  }



  [Header("GUI STUFF")]
  [SerializeField]
  List<TaskCard> taskCards;
  List<string> maleDevNames = new List<string>();
  List<string> femaleDevNames = new List<string>();
  List<string> gameNames = new List<string>();

  void
  TestAll()
  {
    RenderCurrentDemoCard();
    RenderCurrentSpeechCard();
    RenderUpcomingSpeech();
    RenderUpcomingNPCCard();
    RenderCurrentNPCCard();
    RenderUpcomingDemoCard();
  }

  void
  RenderCurrentDemoCard()
  {
    TaskCardInfo info = new TaskCardInfo();

    info.playerID = currentDemo.playerID;
    info.gameID = currentDemo.discColour;
    info.consoleType = consoleNames[(int)currentDemo.console];
    info.gameName = currentDemo.gameTitle;
    if (currentDemo.gamePad == GamePadType.VR)
      info.vr = true;
    else
      info.vr = false;

    if (gamePadNames[(int)currentDemo.gamePad] == "VR Gamepad")
    {
      info.vr = true;
    }

    taskCards[0].UpdateGraphics(info);
  }

  void
  RenderCurrentSpeechCard()
  {
    TaskCardInfo info = new TaskCardInfo();

    info.playerID = currentSpeech.playerID;

    taskCards[1].UpdateGraphics(info);
  }

  void
  RenderCurrentNPCCard()
  {
    TaskCardInfo info = new TaskCardInfo();

    info.type = CardType.NPC;
    info.isMale = currentNPC.isMale;
    info.npcName = currentNPC.npcName;

    taskCards[2].UpdateGraphics(info);
  }

  void
  RenderUpcomingDemoCard()
  {
    TaskCardInfo info = new TaskCardInfo();

    info.playerID = upcomingDemo.playerID;
    info.gameID = upcomingDemo.discColour;
    info.consoleType = consoleNames[(int)upcomingDemo.console];
    info.gameName = upcomingDemo.gameTitle;

    if (currentDemo.gamePad == GamePadType.VR)
      info.vr = true;
    else
      info.vr = false;

    taskCards[3].UpdateGraphics(info);
  }

  void
  RenderUpcomingSpeech()
  {
    TaskCardInfo info = new TaskCardInfo();

    info.playerID = upcomingSpeech.playerID;

    taskCards[4].UpdateGraphics(info);
  }

  void
  RenderUpcomingNPCCard()
  {
    TaskCardInfo info = new TaskCardInfo();

    info.type = CardType.NPC;
    info.isMale = upcomingNPC.isMale;
    info.npcName = upcomingNPC.npcName;

    taskCards[5].UpdateGraphics(info);
  }



  string CreateNewGameTitle()
  {
    if (gameNames.Count == 0)
      PushGameNames();
    string newName = gameNames[0];

    gameNames.Remove(gameNames[0]);

    return newName;
  }

  string CreateNewNPCName(int randomNumber)
  {
    string newName = "";

    if (randomNumber == 1)
    {
      if (femaleDevNames.Count == 0)
        PushFemaleNames();
      newName = femaleDevNames[0];
      femaleDevNames.Remove(femaleDevNames[0]);
    }
    else
    {
      if (maleDevNames.Count == 0)
        PushMaleNames();
      newName = maleDevNames[0];
      maleDevNames.Remove(maleDevNames[0]);
    }

    return newName;
  }

  void
  OnValidate()
  {
    PushGameNames();
    PushFemaleNames();
    PushMaleNames();
  }

  void
  PushGameNames()
  {
    gameNames.Add("dark_souls");
    gameNames.Add("doom");
    gameNames.Add("fallout");
    gameNames.Add("fifa");
    gameNames.Add("halflife");
    gameNames.Add("inside");
    gameNames.Add("xcom");
    gameNames.Add("zelda");
  }

  void
  PushFemaleNames()
  {
    femaleDevNames.Add("Amy Hennig");
    femaleDevNames.Add("Jennifer Hale");
    femaleDevNames.Add("Kellee Santiago");
  }

  void
  PushMaleNames()
  {
    maleDevNames.Add("John Carmack");
    maleDevNames.Add("Sean Murray");
    maleDevNames.Add("Hideo Kojima");
    maleDevNames.Add("Josh Sawyer");

  }
}