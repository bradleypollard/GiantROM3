using UnityEngine;
using System.Collections.Generic;

public class ButtonPrompt : MonoBehaviour
{
  private GamePlayDemo gameplayDemo;
  private Transform playerTransform;
  private string button = "";
  private float ttl = 0f;
  private Vector3 startPos;
  private float life = 0f;
  private bool initialised = false;

  public void Init(string _button, float _ttl, Transform _player, GamePlayDemo _demo)
  {
    button = _button;
    ttl = _ttl;
    life = ttl;
    playerTransform = _player;
    gameplayDemo = _demo;
    startPos = transform.position;
    initialised = ttl > 0f;

  }

  public string GetButton()
  {
    return button;
  }

  // Update is called once per frame
  void Update()
  {
    if (initialised && gameplayDemo.isRunning)
    {
      ttl -= Time.deltaTime;
      transform.position = Vector3.Lerp(startPos, playerTransform.position, Mathf.Pow(1 - (ttl / life), 3)); // Lerp proportional to the amount of life left
      if (ttl < 0f)
      {
        gameplayDemo.OnMiss(button); // Prompt has reached the player, it's now a miss
      }
    }
  }
}
