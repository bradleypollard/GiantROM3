using UnityEngine;
using System.Collections.Generic;

public class Teleprompter : MonoBehaviour {

  public int speakerID = -1;

  private string[] buttons = { "A", "B", "X", "Y" };
  private Queue<string> prompts;
  private string label = "";

	// Use this for initialization
	void Start () {
    prompts = new Queue<string>();
    for (int i = 0; i < 100; ++i)
    {
      string button = buttons[Random.Range(0, buttons.Length)];
      prompts.Enqueue(button);
      label += button;
    }
	}
	
	// Update is called once per frame
	void Update () {
	  if (prompts.Count > 0 && Input.GetButtonDown(prompts.Peek() + "_P" + speakerID))
    {
      prompts.Dequeue();
      label = label.Substring(1);
    }
	}

  void OnGUI()
  {
    GUI.Label(new Rect(0, 0, 500, 20), label);
  }
}
