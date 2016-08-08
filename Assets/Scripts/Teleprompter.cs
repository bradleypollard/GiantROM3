using UnityEngine;
using System.Collections.Generic;

public class Teleprompter : MonoBehaviour
{

  [Header("Settings")]
  [Range(0, 4)]
  [SerializeField]
  int speakerID = 0;
  [Range(50, 200)]
  [SerializeField]
  int maxQueueSize = 100;

  private string[] buttons = { "A", "B", "X", "Y" };
  private Queue<string> visiblePrompts;
  private Queue<string> upcomingPrompts;
  private string telepromter = "";
  private GUIStyle hitStyle = new GUIStyle();
  private string hit = "";
  private GUIStyle missStyle = new GUIStyle();
  private string miss = "";
  private float timeTillNextPrompt = 0f;

  // Use this for initialization
  void Start()
  {
    visiblePrompts = new Queue<string>();
    upcomingPrompts = new Queue<string>();
    hitStyle.normal.textColor = Color.green;
    missStyle.normal.textColor = Color.red;

    SetSpeakerID(1);
  }

  // Called when a new speaker arrives on the stage
  public void SetSpeakerID(int _id)
  {
    speakerID = _id;
    AddMorePrompts();
  }

  // Called once the current speaker has finished
  private void ClearSpeakerID()
  {
    speakerID = 0;
  }

  // Called when the handle is cranked, or a new speaker arrives
  public void AddMorePrompts()
  {
    for (int i = upcomingPrompts.Count - 1; i < maxQueueSize; ++i)
    {
      string button = buttons[Random.Range(0, buttons.Length)];
      upcomingPrompts.Enqueue(button);
    }
  }

  // Update is called once per frame
  void Update()
  {
    // Reveal new button to player
    if (upcomingPrompts.Count > 0 && timeTillNextPrompt <= 0f)
    {
      string button = upcomingPrompts.Dequeue();
      visiblePrompts.Enqueue(button);
      telepromter += button;
      timeTillNextPrompt = 2 * Mathf.Exp(-5 * upcomingPrompts.Count / maxQueueSize);
    }

    timeTillNextPrompt -= Time.deltaTime;

    // Check if player hit a prompt
    foreach (string b in buttons)
    {
      if (visiblePrompts.Count > 0 && Input.GetButtonDown(b + "_P" + speakerID))
      {
        string button = visiblePrompts.Dequeue();
        telepromter = telepromter.Substring(1);
        if (button == b)
        {
          hit += button;
        }
        else
        {
          miss += button;
        }
      }
    }

  }

  // Debug drawing the input string
  void OnGUI()
  {
    GUI.Label(new Rect(0, 0, 500, 20), telepromter);
    GUI.Label(new Rect(0, 30, 500, 20), hit, hitStyle);
    GUI.Label(new Rect(0, 60, 500, 20), miss, missStyle);
  }
}
