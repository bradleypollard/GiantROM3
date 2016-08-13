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
  [SerializeField]
  Renderer[] meshes;
  [Header("Settings")]
  [Range(0, 4)]
  [SerializeField]
  public int playerID = 0;

  public int expectedDiscColour = 0;

  public string expectedConsoleName = "";
  public string expectedGamePadName = "";

  public bool hasCorrectConsole = false;
  public bool hasCorrectGamePad = false;
  private bool isReady = false;

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

    if (isReady == false && hasCorrectGamePad && hasCorrectConsole)
    {
      // Only set transparency of outline to 1 on the frame both items are ready!
      foreach (Renderer r in meshes)
      {
        r.material.SetFloat("_OutlineTransparency", 1);
      }
    }
    isReady = hasCorrectConsole && hasCorrectGamePad;

    // Handle input
    if (playerCanUseConsole && isReady && Input.GetButton("A_P" + playerID))
    {
      gameplayDemo.SetPlayerID(playerID, playerTransform);
      foreach (Renderer r in meshes)
      {
        r.material.SetFloat("_OutlineTransparency", 0);
      }
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
    if (collider.GetComponent<PlayerMovement>())
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
  }

  void OnTriggerExit(Collider collider)
  {
    if (collider.GetComponent<PlayerMovement>())
    {
      Debug.Log("Player " + collider.GetComponent<PlayerMovement>().playerIndex + " left the gamepad");
      playerCanUseConsole = false;
      playerTransform = null;
    }
  }
}
