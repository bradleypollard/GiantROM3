using UnityEngine;
using System.Collections;

public class Spotlight : MonoBehaviour
{

  [Header("References")]
  [SerializeField]
  Light spot;
  [SerializeField]
  GamePlayDemo gameplayDemo;
  [SerializeField]
  Teleprompter teleprompter;

  [Header("Settings")]
  [Range(1, 30)]
  [SerializeField]
  float spotlightDuration = 5;

  private int playerID = 0;
  private float ttl = 0f;

  private bool canUseSpotlightSwitch = false;

  private void TurnOn()
  {
    ttl = spotlightDuration;
    spot.enabled = true;
    GetComponent<AudioSource>().Play();

    if (teleprompter != null)
    {
      teleprompter.isLit = true;
    }
    if (gameplayDemo != null)
    {
      gameplayDemo.isLit = true;
    }
  }

  private void TurnOff()
  {
    spot.enabled = false;
    if (teleprompter != null)
    {
      teleprompter.isLit = false;
    }
    if (gameplayDemo != null)
    {
      gameplayDemo.isLit = false;
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (canUseSpotlightSwitch && playerID != 0 && Input.GetButton("A_P" + playerID))
    {
      TurnOn();
    }

    if (ttl <= 0f)
    {
      TurnOff();
    }
    else
    {
      ttl -= Time.deltaTime;
    }

  }

  void OnTriggerEnter(Collider collider)
  {
    canUseSpotlightSwitch = true;
    playerID = collider.gameObject.GetComponent<PlayerMovement>().playerIndex;

  }

  void OnTriggerExit(Collider collider)
  {
    canUseSpotlightSwitch = false;
    playerID = 0;
  }
}
