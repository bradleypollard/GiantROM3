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
  GameObject noDiscIcon;
  [SerializeField]
  Renderer[] meshes;
  [Header("Settings")]
  [Range(0, 4)]
  [SerializeField]
  public int playerID = 0;

  public int expectedDiscColour = 0;
  public Color[] discColours = { Color.cyan, Color.magenta, Color.yellow };

  public string expectedConsoleName = "";
  public string expectedGamePadName = "";

  public bool hasCorrectConsole = false;
  public bool hasCorrectGamePad = false;
  public bool hasCorrectDisc = false;
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
    // Update disc status
    if (consoleStand.currentHeldDisc != null)
    {
      hasCorrectDisc = consoleStand.currentHeldDisc.GetComponentInChildren<Renderer>().material.color.Equals(discColours[expectedDiscColour-1]);
    }
    else
    {
      hasCorrectDisc = false;
    }

    noConsoleIcon.SetActive(!hasCorrectConsole);
    noGamePadIcon.SetActive(!hasCorrectGamePad);
    noDiscIcon.SetActive(!hasCorrectDisc);

    if (gameplayDemo.playerID != 0)
    {
      // Console has broken, highlight for repair
      foreach (Renderer r in meshes)
      {
        r.material.SetFloat("_OutlineTransparency", gameplayDemo.isWorking ? 0 : 1);
      }
    }

    if (isReady == false && hasCorrectGamePad && hasCorrectConsole && hasCorrectDisc)
    {
      // Only activate outline on the frame if all items are ready!
      foreach (Renderer r in meshes)
      {
        r.material.SetFloat("_OutlineTransparency", 1);
      }
    }
    isReady = hasCorrectConsole && hasCorrectGamePad && hasCorrectDisc;

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

  public void EjectAll(bool consumeDisc)
  {
    consoleStand.EjectConsole();
    consoleStand.EjectGamePad();
    consoleStand.EjectDisc(consumeDisc);
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
