using UnityEngine;
using System.Collections.Generic;

public class ButtonPrompt : MonoBehaviour {
  
  private Teleprompter teleprompter;
  private Transform playerTransform;
  private string button = "";
  private float ttl = 0f;
  private Dictionary<string, Color> map;
  private Vector3 startPos;
  private float life = 0f;

  public void Init(string _button, float _ttl, Transform _player, Teleprompter _tele)
  {
    map = new Dictionary<string, Color>();
    map["A"] = Color.green;
    map["B"] = Color.red;
    map["X"] = Color.blue;
    map["Y"] = Color.yellow;

    button = _button;
    ttl = _ttl;
    life = ttl;
    playerTransform = _player;
    GetComponent<MeshRenderer>().material.color = map[button];
    teleprompter = _tele;
    startPos = transform.position;
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
    ttl -= Time.deltaTime;
    transform.position = Vector3.Lerp(startPos, playerTransform.position, Mathf.Pow(1 - (ttl / life), 3)); // Lerp proportional to the amount of life left
    if (ttl < 0f)
    {
      teleprompter.OnMiss(button); // Prompt has reached the player, it's now a miss
    }
	}
}
