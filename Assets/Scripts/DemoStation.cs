using UnityEngine;
using System.Collections;

public class DemoStation : MonoBehaviour
{
  [Header("References")]
  [SerializeField]
  GamePlayDemo gameplayDemo;
  [SerializeField]
  ConsoleStand consoleStand;
  [SerializeField]
  GameObject noGamePadIcon;
  [SerializeField]
  GameObject noConsoleIcon;
  [Header("Settings")]
  [Range(0, 4)]
  [SerializeField]
  public int playerID = 0;

  public int expectedDiscColour = 0;

  public string expectedConsoleName = "Console Playstation";
  public string expectedGamePadName = "Controller (Playstation)";

  public bool hasCorrectConsole = false;
  public bool hasCorrectGamePad = false;

  private bool playerCanUseConsole = false;
  private Transform playerTransform;

  // Update is called once per frame
  void Update()
  {
    // Update console status
    if (consoleStand.currentHeldConsole != null)
    {
      hasCorrectConsole = consoleStand.currentHeldConsole.name == expectedConsoleName;
    }
    else
    {
      hasCorrectConsole = false;
    }
    // Update controller status
    if (consoleStand.currentHeldGamePad != null)
    {
      hasCorrectGamePad = consoleStand.currentHeldGamePad.name == expectedGamePadName;
    }
    else
    {
      hasCorrectGamePad = false;
    }

    noConsoleIcon.SetActive(!hasCorrectConsole);
    noGamePadIcon.SetActive(!hasCorrectGamePad);

    // Handle input
    if (playerCanUseConsole && hasCorrectConsole && hasCorrectGamePad && Input.GetButton("A_P" + playerID))
    {
      gameplayDemo.SetPlayerID(playerID, playerTransform);
      playerCanUseConsole = false;
    }
  }

  public void EjectAll()
  {
    consoleStand.EjectConsole();
    consoleStand.EjectGamePad();
  }

  void OnTriggerEnter(Collider collider)
  {
    int playerIndex = collider.GetComponent<PlayerMovement>().playerIndex;
    playerTransform = collider.transform;
    Debug.Log("Player " + playerIndex + " entered the gamepad");
    if (playerIndex == playerID)
    {
      Debug.Log("This player is the demoer for this console!");
      playerCanUseConsole = true;
    }
    else
    {
      Debug.Log("This player is not on this demo :(");
    }

  }

  void OnTriggerExit(Collider collider)
  {
    Debug.Log("Player " + collider.transform.root.GetComponent<PlayerMovement>().playerIndex + " left the gamepad");
    playerCanUseConsole = false;
    playerTransform = null;
  }
}
