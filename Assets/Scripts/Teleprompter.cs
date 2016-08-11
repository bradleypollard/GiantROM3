﻿using UnityEngine;
using System.Collections.Generic;

public class Teleprompter : MonoBehaviour
{

  [Header("References")]
  [SerializeField]
  Transform playerTransform;
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

  [Header("Settings")]
  [Range(0, 4)]
  [SerializeField]
  int speakerID = 0;
  [Range(10, 100)]
  [SerializeField]
  int maxQueueSize = 100;
  [Range(0, 5)]
  [SerializeField]
  float promptGenerationDelay = 1;
  [SerializeField]
  public bool isLit = true;

  private string[] buttons = { "A", "B", "X", "Y" };
  private Dictionary<string, GameObject> buttonPrefabs;
  private ButtonPrompt visiblePrompt;
  private Queue<string> upcomingPrompts;
  private float timeTillNextPrompt = 0f;
  private bool prevIsLit = true;

  // Use this for initialization
  void Start()
  {
    upcomingPrompts = new Queue<string>();

    buttonPrefabs = new Dictionary<string, GameObject>();
    buttonPrefabs["A"] = aButtonPrefab;
    buttonPrefabs["B"] = bButtonPrefab;
    buttonPrefabs["X"] = xButtonPrefab;
    buttonPrefabs["Y"] = yButtonPrefab;
  }

  // Called when a new speaker arrives on the stage
  public void SetSpeakerID(int _id, Transform _player)
  {
    speakerID = _id;
    playerTransform = _player;
    playerTransform.gameObject.GetComponent<PlayerMovement>().lockMovement = true;
    AddMorePrompts();
  }

  // Called once the current speaker has finished
  private void ClearSpeakerID()
  {
    playerTransform.gameObject.GetComponent<PlayerMovement>().lockMovement = false;

    speakerID = 0;
    playerTransform = null;

    if (visiblePrompt)
    {
      Destroy(visiblePrompt.gameObject);
      visiblePrompt = null;
    }

    upcomingPrompts.Clear();
    timeTillNextPrompt = 0f;
  }

  // Called when the handle is cranked, or a new speaker arrives
  public void AddMorePrompts()
  {
    if (speakerID != 0)
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
      bp.gameObject.transform.position = playerTransform.position + new Vector3(0, 4f, 0);
      bp.Init(button, -1f, playerTransform, null); // Negative TTL means prompt won't move at all
      visiblePrompt = bp;
      timeTillNextPrompt = promptGenerationDelay * Mathf.Exp(-upcomingPrompts.Count / (float)maxQueueSize);
    }
    
    // If there is no prompt on screen we need to count down to the next one!
    if (visiblePrompt == null)
    {
      timeTillNextPrompt -= Time.deltaTime;
    }

    // If the light has turned on or off this frame we need to change the shader now
    if (isLit != prevIsLit && visiblePrompt != null)
    {
      if (isLit)
      {
        visiblePrompt.gameObject.GetComponent<Renderer>().material.shader = defaultButtonShader;
      }
      else
      {
        visiblePrompt.gameObject.GetComponent<Renderer>().material.shader = greyscaleButtonShader;
      }

      // Update prevLit state for next frame
      prevIsLit = isLit;
    }


    // Finally, if the light is on we should take input from the player
    if (isLit)
    {
      // Check if player hit a prompt
      if (visiblePrompt != null && Input.GetButtonDown(visiblePrompt.GetButton() + "_P" + speakerID))
      {
        Debug.Log("Speaker " + speakerID + " hit prompt " + visiblePrompt.GetButton());
        Destroy(visiblePrompt.gameObject);
        visiblePrompt = null;
      }
    }
  }
}
