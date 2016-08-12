using UnityEngine;
using System.Collections;

public class Microphone : MonoBehaviour
{
  [Header("References")]
  [SerializeField]
  Teleprompter teleprompter;
  [SerializeField]
  ProgramFeed programFeed;
  [Header("Settings")]
  [Range(0, 4)]
  [SerializeField]
  public int speakerID = 0;

  private bool playerCanUseMicrophone = false;
  private Transform playerTransform;

  // Update is called once per frame
  void Update()
  {
    if (playerCanUseMicrophone && Input.GetButton("A_P" + speakerID))
    {
      teleprompter.SetSpeakerID(speakerID, playerTransform);
      playerCanUseMicrophone = false;
    }
  }

  void OnTriggerEnter(Collider collider)
  {
    int playerIndex;
    playerIndex = collider.GetComponent<PlayerMovement>().playerIndex;
    playerTransform = collider.transform;

    //int playerIndex = collider.transform.root.GetComponent<PlayerMovement>().playerIndex;
    //playerTransform = collider.transform.root;

    print("Player " + playerIndex + " entered the microphone");
    if (playerIndex == speakerID)
    {
      print("This player is next on stage!");
      playerCanUseMicrophone = true;
    }
    else
    {
      print("This player should not be on stage :(");
    }

  }

  void OnTriggerExit(Collider collider)
  {
    print("Player " + collider.GetComponent<PlayerMovement>().playerIndex + " left the microphone");
    //print("Player " + collider.transform.root.GetComponent<PlayerMovement>().playerIndex + " left the microphone");
    playerCanUseMicrophone = false;
    playerTransform = null;
  }
}
