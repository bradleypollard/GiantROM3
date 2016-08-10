using UnityEngine;
using System.Collections.Generic;

public class ButtonPrompt : MonoBehaviour {
  
  private Teleprompter teleprompter;
  private Transform playerTransform;
  private string button = "";
  private float ttl = 0f;
  private Vector3 startPos;
  private float life = 0f;
  private bool initialised = false;

  public void Init(string _button, float _ttl, Transform _player, Teleprompter _tele)
  {
    button = _button;
    ttl = _ttl;
    life = ttl;
    playerTransform = _player;
    teleprompter = _tele;
    startPos = transform.position;
    initialised = true;
  }

  public string GetButton()
  {
    return button;
  }

	// Use this for initialization
	void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
    if (initialised)
    {
      ttl -= Time.deltaTime;
      transform.position = Vector3.Lerp(startPos, playerTransform.position, Mathf.Pow(1 - (ttl / life), 3)); // Lerp proportional to the amount of life left
      if (ttl < 0f)
      {
        teleprompter.OnMiss(button); // Prompt has reached the player, it's now a miss
      }
    }
	}
}
