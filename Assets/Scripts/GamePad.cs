using UnityEngine;
using System.Collections;

public class GamePad : MonoBehaviour
{
  [Header("References")]
  [SerializeField]
  GamePlayDemo gameplayDemo;
  [Header("Settings")]
  [Range(0, 4)]
  [SerializeField]
  int speakerID = 0;

  private bool playerCanUseConsole = false;
  private Transform playerTransform;

  // Update is called once per frame
  void Update()
  {
    if (playerCanUseConsole && Input.GetButton("A_P" + speakerID))
    {
      gameplayDemo.SetPlayerID(speakerID, playerTransform);
      playerCanUseConsole = false;
    }
  }

  void OnTriggerEnter(Collider collider)
  {
    int playerIndex = collider.transform.root.GetComponent<PlayerMovement>().playerIndex;
    playerTransform = collider.transform.root;
    print("Player " + playerIndex + " entered");
    if (playerIndex == speakerID)
    {
      print("This player is the demoer for this console!");
      playerCanUseConsole = true;
    }
    else
    {
      print("This player is not on this demo :(");
    }

  }

  void OnTriggerExit(Collider collider)
  {
    print("Player " + collider.transform.root.GetComponent<PlayerMovement>().playerIndex + " left");
    playerCanUseConsole = false;
    playerTransform = null;
  }
}
