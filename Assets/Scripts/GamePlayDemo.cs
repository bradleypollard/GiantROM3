using UnityEngine;
using System.Collections.Generic;

public class GamePlayDemo : MonoBehaviour
{

  [Header("References")]
  [SerializeField]
  Transform playerTransform;
  [SerializeField]
  Transform buttonPromptSpawnPoint;
  [SerializeField]
  GameObject aButtonPrefab;
  [SerializeField]
  GameObject bButtonPrefab;
  [SerializeField]
  GameObject xButtonPrefab;
  [SerializeField]
  GameObject yButtonPrefab;
  [SerializeField]
  Shader defaultButtonShader;
  [SerializeField]
  Shader greyscaleButtonShader;
  [SerializeField]
  ProgramFeed programFeed;
  [SerializeField]
  GameObject litIcon;
  [SerializeField]
  Renderer lightMesh;
  [SerializeField]
  AudioClip[] arcadeGood;
  [SerializeField]
  AudioClip[] arcadeBad;
  [SerializeField]
  AudioSource playerAudioSource;
  [SerializeField]
  AudioSource applauseAudioSource;

  [Header("Settings")]
  [Range(0, 4)]
  [SerializeField]
  public int playerID = 0;
  [Range(10, 100)]
  [SerializeField]
  int queueSize = 50;
  [Range(0, 5)]
  [SerializeField]
  float promptGenerationTime = 1;
  [Range(1, 10)]
  [SerializeField]
  float promptTTL = 5;
  [SerializeField]
  public bool isLit = true;
  [SerializeField]
  public bool isWorking = true;
  [SerializeField]
  public bool isRunning = true;

  private string[] buttons = { "A", "B", "X", "Y" };
  private Dictionary<string, GameObject> buttonPrefabs;
  private Queue<ButtonPrompt> visiblePrompts;
  private Queue<string> upcomingPrompts;
  private string gameInputs = "";
  private GUIStyle hitStyle = new GUIStyle();
  private string hit = "";
  private GUIStyle missStyle = new GUIStyle();
  private string miss = "";
  private float timeTillNextPrompt = 0f;
  private bool prevRunning = true;

  // Use this for initialization
  void Start()
  {
    visiblePrompts = new Queue<ButtonPrompt>();
    upcomingPrompts = new Queue<string>();
    hitStyle.normal.textColor = Color.green;
    missStyle.normal.textColor = Color.red;

    buttonPrefabs = new Dictionary<string, GameObject>();
    buttonPrefabs["A"] = aButtonPrefab;
    buttonPrefabs["B"] = bButtonPrefab;
    buttonPrefabs["X"] = xButtonPrefab;
    buttonPrefabs["Y"] = yButtonPrefab;
  }

  // Called when a new speaker arrives on the stage
  public void SetPlayerID(int _id, Transform _player)
  {
    playerID = _id;
    playerTransform = _player;
    playerTransform.gameObject.GetComponent<PlayerMovement>().lockMovement = true;
    AddMorePrompts();
  }

  // Called once the current player has finished
  private void ClearPlayerID()
  {
    playerTransform.gameObject.GetComponent<PlayerMovement>().lockMovement = false;
    programFeed.OnDemoFinished();

    playerID = 0;
    playerTransform = null;

    while (visiblePrompts.Count > 0)
    {
      ButtonPrompt bp = visiblePrompts.Dequeue();
      Destroy(bp);
    }
    upcomingPrompts.Clear();
    timeTillNextPrompt = 0f;
    hit = "";
    miss = "";
    gameInputs = "";

    applauseAudioSource.Play();
  }

  // Called when the player arrives
  private void AddMorePrompts()
  {
    if (playerID != 0)
    {
      for (int i = upcomingPrompts.Count - 1; i < queueSize; ++i)
      {
        string button = buttons[Random.Range(0, buttons.Length)];
        upcomingPrompts.Enqueue(button);
      }
    }
  }

  // Update is called once per frame
  void Update()
  {
    isRunning = isLit && isWorking;

    // Display icon
    if (playerID == 0)
    {
      litIcon.SetActive(false);
      // Hide outline if no-one is on stage
      lightMesh.material.SetFloat("_OutlineTransparency", 0);
    }
    else
    {
      litIcon.SetActive(!isLit);
      // show outline when not lit
      lightMesh.material.SetFloat("_OutlineTransparency", isLit ? 0 : 1);
    }

    if (isRunning != prevRunning)
    {
      // If running state has changed, shader must switch
      if (isRunning)
      {
        foreach (ButtonPrompt bp in visiblePrompts)
        {
          bp.gameObject.GetComponent<Renderer>().material.shader = defaultButtonShader;
        }
      }
      else
      {
        foreach (ButtonPrompt bp in visiblePrompts)
        {
          bp.gameObject.GetComponent<Renderer>().material.shader = greyscaleButtonShader;
        }
      }

      prevRunning = isRunning;
    }

    if (isRunning)
    {
      if (playerTransform != null && upcomingPrompts.Count == 0 && visiblePrompts.Count == 0)
      {
        ClearPlayerID();
      }

      // Reveal new button to player
      if (upcomingPrompts.Count > 0 && timeTillNextPrompt <= 0f)
      {
        string button = upcomingPrompts.Dequeue();
        ButtonPrompt bp = Instantiate(buttonPrefabs[button]).GetComponent<ButtonPrompt>();
        bp.gameObject.transform.position = buttonPromptSpawnPoint.position;
        bp.Init(button, promptTTL, playerTransform, this);
        visiblePrompts.Enqueue(bp);
        gameInputs += button;
        timeTillNextPrompt = promptGenerationTime;
      }

      timeTillNextPrompt -= Time.deltaTime;

      // Check if player hit a prompt
      foreach (string b in buttons)
      {
        if (visiblePrompts.Count > 0 && Input.GetButtonDown(b + "_P" + playerID))
        {
          string button = visiblePrompts.Peek().GetButton();
          if (button == b)
          {
            OnHit(button);
          }
          else
          {
            OnMiss(button);
          }
        }
      }
    }
  }

  // Called when a player hits the right button
  public void OnHit(string _button)
  {
    hit += _button;
    ButtonPrompt bp = visiblePrompts.Dequeue();
    Destroy(bp.gameObject);
    gameInputs = gameInputs.Substring(1);

    playerAudioSource.clip = arcadeGood[Random.Range(0, arcadeGood.Length)];
    playerAudioSource.Play();
  }

  // Called when a player hits the wrong button, or doesn't hit any button in time
  public void OnMiss(string _button)
  {
    miss += _button;
    ButtonPrompt bp = visiblePrompts.Dequeue();
    Destroy(bp.gameObject);
    gameInputs = gameInputs.Substring(1);
    SetWorking(false);

    playerAudioSource.clip = arcadeBad[Random.Range(0, arcadeBad.Length)];
    playerAudioSource.Play();
  }

  public void SetWorking(bool _isWorking)
  {
    isWorking = _isWorking;
  }
}
