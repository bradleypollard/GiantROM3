using UnityEngine;
using System.Collections.Generic;

public class ConsoleStand : MonoBehaviour
{

  [Header("References")]
  [SerializeField]
  Transform gamePadPos;
  [SerializeField]
  Transform consolePos;
  [SerializeField]
  DemoStation demoStation;

  public GameObject currentHeldConsole = null;
  public GameObject currentHeldGamePad = null;

  private Dictionary<int, GameObject> IDToPlayerMap = new Dictionary<int, GameObject>();

  // Update is called once per frame
  void Update()
  {
    foreach (int id in IDToPlayerMap.Keys)
    {
      if (Input.GetButtonDown("A_P" + id))
      {
        GameObject heldItem = IDToPlayerMap[id].GetComponent<Interaction>().objectInHands;
        if (heldItem == null)
        {
          // Eject all!
          if (currentHeldConsole != null && !(demoStation.hasCorrectConsole && demoStation.hasCorrectGamePad))
          {
            EjectConsole();
          }
          if (currentHeldGamePad != null && !(demoStation.hasCorrectConsole && demoStation.hasCorrectGamePad))
          {
            EjectGamePad();
          }
        }
        else if (heldItem.tag == "Console" && currentHeldConsole == null)
        {
          // Insert console!
          currentHeldConsole = heldItem;
          IDToPlayerMap[id].GetComponent<Interaction>().ReleaseObject();
          currentHeldConsole.transform.parent = consolePos;
          currentHeldConsole.transform.localPosition = Vector3.zero;
          currentHeldConsole.transform.localRotation = Quaternion.identity;
          currentHeldConsole.GetComponent<Rigidbody>().isKinematic = true;

          foreach (BoxCollider b in currentHeldConsole.GetComponentsInChildren<BoxCollider>())
          {
            b.enabled = false;
          }

          if (currentHeldConsole.name == demoStation.expectedConsoleName)
          {
            GameObject.Find(demoStation.expectedConsoleName).GetComponentInChildren<Renderer>().material.SetFloat("_OutlineTransparency", 0);
          }
        }
        else if (heldItem.tag == "GamePad" && currentHeldGamePad == null)
        {
          // Insert gamepad!
          currentHeldGamePad = heldItem;
          IDToPlayerMap[id].GetComponent<Interaction>().ReleaseObject();
          currentHeldGamePad.transform.parent = gamePadPos;
          currentHeldGamePad.transform.localPosition = Vector3.zero;
          currentHeldGamePad.transform.localRotation = Quaternion.identity;
          currentHeldGamePad.GetComponent<Rigidbody>().isKinematic = true;

          foreach (BoxCollider b in currentHeldGamePad.GetComponentsInChildren<BoxCollider>())
          {
            b.enabled = false;
          }

          if (currentHeldGamePad.name == demoStation.expectedGamePadName)
          {
            GameObject.Find(demoStation.expectedGamePadName).GetComponentInChildren<Renderer>().material.SetFloat("_OutlineTransparency", 0);
          }
        }
      }
    }
  }

  public void EjectConsole()
  {
    // Reset
    foreach (BoxCollider b in currentHeldConsole.GetComponentsInChildren<BoxCollider>())
    {
      b.enabled = true;
    }
    currentHeldConsole.GetComponent<Rigidbody>().isKinematic = false;
    currentHeldConsole.transform.parent = null;

    // Fling
    Vector3 fwd = gameObject.transform.parent.TransformDirection(Vector3.right);
    Vector3 up = gameObject.transform.parent.TransformDirection(Vector3.up);
    currentHeldConsole.transform.position = currentHeldConsole.transform.position + fwd + up;
    currentHeldConsole.GetComponent<Rigidbody>().AddForce(fwd * 200 + new Vector3(0, 200, 0));
    Debug.Log("Ejecting " + currentHeldConsole.name);

    currentHeldConsole = null;
  }

  public void EjectGamePad()
  {
    // Reset
    foreach (BoxCollider b in currentHeldGamePad.GetComponentsInChildren<BoxCollider>())
    {
      b.enabled = true;
    }
    currentHeldGamePad.GetComponent<Rigidbody>().isKinematic = false;
    currentHeldGamePad.transform.parent = null;

    // Fling
    Vector3 fwd = gameObject.transform.parent.TransformDirection(Vector3.right);
    Vector3 up = gameObject.transform.parent.TransformDirection(Vector3.up);
    currentHeldGamePad.transform.position = currentHeldGamePad.transform.position + fwd + up;
    currentHeldGamePad.GetComponent<Rigidbody>().AddForce(fwd * 200 + new Vector3(0, 200, 0));
    Debug.Log("Ejecting " + currentHeldGamePad.name);

    currentHeldGamePad = null;
  }

  void OnTriggerEnter(Collider collider)
  {
    if (collider.GetComponent<PlayerMovement>())
    {
      int playerIndex = collider.GetComponent<PlayerMovement>().playerIndex;
      Debug.Log("Player " + playerIndex + " entered the console stand");
      IDToPlayerMap.Add(playerIndex, collider.gameObject);
    }
  }

  void OnTriggerExit(Collider collider)
  {
    if (collider.GetComponent<PlayerMovement>())
    {
      int playerIndex = collider.GetComponent<PlayerMovement>().playerIndex;
      Debug.Log("Player " + playerIndex + " left the console stand");
      IDToPlayerMap.Remove(playerIndex);
    }
  }
}
