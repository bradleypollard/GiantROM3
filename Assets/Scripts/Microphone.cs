using UnityEngine;
using System.Collections;

public class Microphone : MonoBehaviour
{
  [Header("Two Player Reference")]
  [SerializeField]
  DemoStation demoStation;
  [Header("References")]
  [SerializeField]
  public Teleprompter teleprompter;
  [SerializeField]
  ProgramFeed programFeed;
  [SerializeField]
  Renderer[] meshes;
  [Header("Settings")]
  [Range(0, 4)]
  [SerializeField]
  public int speakerID = 0;

  private bool playerCanUseMicrophone = false;
  private Transform playerTransform;

  // Update is called once per frame
  void Update()
  {
    if ((programFeed.numberOfPlayers == 2 && demoStation.playerID == demoStation.gameplayDemo.playerID))
    {
      // In the rare case that we are in 2 player mode and someone is on the other station, this cant be used
      foreach (Renderer r in meshes)
      {
        r.material.SetFloat("_OutlineTransparency", 0);
      }
    }
    else
    {
      // Turn highlighting back on always
      foreach (Renderer r in meshes)
      {
        r.material.SetFloat("_OutlineTransparency", 1);
      }
      // Handle input
      if (playerCanUseMicrophone && Input.GetButton("A_P" + speakerID))
      {
        teleprompter.SetSpeakerID(speakerID, playerTransform);
        playerCanUseMicrophone = false;
      }
    }
  }

  void OnTriggerEnter(Collider collider)
  {
		if (collider.GetComponent<PlayerMovement> ())
		{
			int playerIndex;
			playerIndex = collider.GetComponent<PlayerMovement> ().playerIndex;
			playerTransform = collider.transform;

			//int playerIndex = collider.transform.root.GetComponent<PlayerMovement>().playerIndex;
			//playerTransform = collider.transform.root;

			Debug.Log ("Player " + playerIndex + " entered the microphone");
			if (playerIndex == speakerID)
			{
				Debug.Log ("This player is next on stage!");
				playerCanUseMicrophone = true;
			}
			else
			{
				Debug.Log ("This player should not be on stage :(");
			}
		}
  }

  void OnTriggerExit(Collider collider)
  {
		if (collider.GetComponent<PlayerMovement> ())
		{
			Debug.Log ("Player " + collider.GetComponent<PlayerMovement> ().playerIndex + " left the microphone");
			//Debug.Log("Player " + collider.transform.root.GetComponent<PlayerMovement>().playerIndex + " left the microphone");
			playerCanUseMicrophone = false;
			playerTransform = null;
		}
  }
}
