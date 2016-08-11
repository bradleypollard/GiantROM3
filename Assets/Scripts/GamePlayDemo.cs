﻿using UnityEngine;
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

  [Header("Settings")]
  [Range(0, 4)]
  [SerializeField]
  int playerID = 0;
  [Range(50, 200)]
  [SerializeField]
  int maxQueueSize = 100;
  [Range(0, 5)]
  [SerializeField]
  float promptGenerationTime = 1;
  [Range(1, 10)]
  [SerializeField]
  float promptTTL = 5;

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

    //SetSpeakerID(1, playerTransform);
  }

  // Called when a new speaker arrives on the stage
  public void SetPlayerID(int _id, Transform _player)
  {
    playerID = _id;
    playerTransform = _player;
    AddMorePrompts();
  }

  // Called once the current speaker has finished
  private void ClearPlayerID()
  {
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
  }

  // Called when the handle is cranked, or a new speaker arrives
  private void AddMorePrompts()
  {
    if (playerID != 0)
    {
      for (int i = upcomingPrompts.Count - 1; i < maxQueueSize; ++i)
      {
        string button = buttons[Random.Range(0, buttons.Length)];
        upcomingPrompts.Enqueue(button);
      }
    }
  }

  // Update is called once per frame
  void Update()
  {
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

  // Called when a player hits the right button
  public void OnHit(string _button)
  {
    hit += _button;
    ButtonPrompt bp = visiblePrompts.Dequeue();
    Destroy(bp.gameObject);
    gameInputs = gameInputs.Substring(1);
  }

  // Called when a player hits the wrong button, or doesn't hit any button in time
  public void OnMiss(string _button)
  {
    miss += _button;
    ButtonPrompt bp = visiblePrompts.Dequeue();
    Destroy(bp.gameObject);
    gameInputs = gameInputs.Substring(1);
  }

  // Debug drawing the input string
  void OnGUI()
  {
    GUI.Label(new Rect(0, 0, 500, 20), gameInputs);
    GUI.Label(new Rect(0, 30, 500, 20), hit, hitStyle);
    GUI.Label(new Rect(0, 60, 500, 20), miss, missStyle);
  }
}