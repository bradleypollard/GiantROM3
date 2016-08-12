using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	private List<int> playerID = new List<int>();
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
		foreach (int id in playerID)
		{
			if (Input.GetButton ("A_P" + id))
			{
				TurnOn ();
			}
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
		if (collider.GetComponent<PlayerMovement> ())
		{
			playerID.Add(collider.gameObject.GetComponent<PlayerMovement> ().playerIndex);
		}
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.GetComponent<PlayerMovement> ())
		{	
			playerID.Remove (collider.gameObject.GetComponent<PlayerMovement> ().playerIndex);
		}
	}
}

//  void OnTriggerEnter(Collider collider)
//  {
//	if (collider.GetComponent<PlayerMovement> ())
//	{
//		canUseSpotlightSwitch = true;
//		playerID = collider.gameObject.GetComponent<PlayerMovement> ().playerIndex;
//	}
//  }
//
//  void OnTriggerExit(Collider collider)
//  {
//	if (collider.GetComponent<PlayerMovement> ())
//	{
//		canUseSpotlightSwitch = false;
//		playerID = 0;
//	}
//  }
//}