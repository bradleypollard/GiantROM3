using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ConsoleStand : MonoBehaviour
{

  [Header("References")]
  [SerializeField]
  GamePlayDemo gameplayDemo;
  [SerializeField]
  Transform gamePadPos;
  [SerializeField]
  Transform consolePos;
  [SerializeField]
  DemoStation demoStation;
  [SerializeField]
  Canvas progressSlider;
  [SerializeField]
  GameObject smoke;


  public GameObject currentHeldConsole = null;
  public GameObject currentHeldGamePad = null;
  public GameObject currentHeldDisc = null;
  public float repairTaps = 10f;


  private Dictionary<int, GameObject> IDToPlayerMap = new Dictionary<int, GameObject>();
  private bool repairInProgress = false;
  private float repairCompletion = 0f;

  // Update is called once per frame
  void Update()
  {
    smoke.SetActive(!gameplayDemo.isWorking);

    foreach (int id in IDToPlayerMap.Keys)
    {
      if (gameplayDemo.isWorking)
      {
        if (Input.GetButtonDown("A_P" + id))
        {
          GameObject heldItem = IDToPlayerMap[id].GetComponent<Interaction>().objectInHands;
          if (heldItem == null)
          {
            // Eject all!
            if (currentHeldConsole != null && !(demoStation.hasCorrectConsole && demoStation.hasCorrectGamePad && demoStation.hasCorrectDisc))
            {
              EjectConsole();
            }
            if (currentHeldGamePad != null && !(demoStation.hasCorrectConsole && demoStation.hasCorrectGamePad && demoStation.hasCorrectDisc))
            {
              EjectGamePad();
            }
            if (currentHeldDisc != null && !(demoStation.hasCorrectConsole && demoStation.hasCorrectGamePad && demoStation.hasCorrectDisc))
            {
              EjectDisc(false);
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
              currentHeldConsole.GetComponentInChildren<Renderer>().material.SetFloat("_OutlineTransparency", 0);
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
              currentHeldGamePad.GetComponentInChildren<Renderer>().material.SetFloat("_OutlineTransparency", 0);
            }
          }
          else if (heldItem.tag == "Disc" && currentHeldDisc == null)
          {
            // Insert disc!
            currentHeldDisc = heldItem;
            IDToPlayerMap[id].GetComponent<Interaction>().ReleaseObject();
            currentHeldDisc.transform.parent = consolePos;
            currentHeldDisc.transform.localPosition = Vector3.zero;
            currentHeldDisc.transform.localRotation = Quaternion.identity;
            currentHeldDisc.GetComponent<Rigidbody>().isKinematic = true;

            foreach (BoxCollider b in currentHeldDisc.GetComponentsInChildren<BoxCollider>())
            {
              b.enabled = false;
            }

            if (currentHeldDisc.GetComponentInChildren<Renderer>().material.color.Equals(demoStation.discColours[demoStation.expectedDiscColour - 1]))
            {
              // TODO: Disc highlighting
              //GameObject.Find(demoStation.expectedGamePadName).GetComponentInChildren<Renderer>().material.SetFloat("_OutlineTransparency", 0);
            }
          }
        }
      }
      else if (id != gameplayDemo.playerID)
      {
        if (repairInProgress)
        {
          if (Input.GetButtonDown("A_P" + id))
          {
            if (repairCompletion < repairTaps - 1)
            {
              // Repair in progress
              repairCompletion += 1;
              progressSlider.GetComponentInChildren<Slider>().value = repairCompletion / repairTaps;
            }
            else
            {
              // Repair done
              IDToPlayerMap[id].GetComponent<PlayerMovement>().lockMovement = false;
              gameplayDemo.SetWorking(true);
              repairCompletion = 0;
              progressSlider.GetComponentInChildren<Slider>().value = repairCompletion;
              progressSlider.gameObject.SetActive(false);
              repairInProgress = false;
            }
          }
        }
        else
        {
          if (Input.GetButtonDown("A_P" + id))
          {
            // Begin repair process
            IDToPlayerMap[id].GetComponent<PlayerMovement>().lockMovement = true;
            repairCompletion = 0f;
            progressSlider.gameObject.SetActive(true);
            progressSlider.GetComponentInChildren<Slider>().value = repairCompletion;
            repairInProgress = true;
          }
        }
      }
    }
  }

  private void CancelRepair(GameObject player)
  {
    // Cancel repair
    player.GetComponent<PlayerMovement>().lockMovement = false;
    repairCompletion = 0f;
    progressSlider.GetComponentInChildren<Slider>().value = repairCompletion;
    progressSlider.gameObject.SetActive(false);
    repairInProgress = false;
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

    if (currentHeldConsole.name == demoStation.expectedConsoleName)
    {
      currentHeldConsole.GetComponentInChildren<Renderer>().material.SetFloat("_OutlineTransparency", 1);
    }

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

    if (currentHeldGamePad.name == demoStation.expectedGamePadName)
    {
      currentHeldGamePad.GetComponentInChildren<Renderer>().material.SetFloat("_OutlineTransparency", 1);
    }

    // Fling
    Vector3 fwd = gameObject.transform.parent.TransformDirection(Vector3.right);
    Vector3 up = gameObject.transform.parent.TransformDirection(Vector3.up);
    currentHeldGamePad.transform.position = currentHeldGamePad.transform.position + fwd + up;
    currentHeldGamePad.GetComponent<Rigidbody>().AddForce(fwd * 200 + new Vector3(0, 200, 0));
    Debug.Log("Ejecting " + currentHeldGamePad.name);

    currentHeldGamePad = null;
  }

  public void EjectDisc(bool consumeDisc)
  {
    if (consumeDisc)
    {
      Destroy(currentHeldDisc);
    }
    else
    {
      // Reset
      foreach (BoxCollider b in currentHeldDisc.GetComponentsInChildren<BoxCollider>())
      {
        b.enabled = true;
      }
      currentHeldDisc.GetComponent<Rigidbody>().isKinematic = false;
      currentHeldDisc.transform.parent = null;

      if (currentHeldDisc.GetComponentInChildren<Renderer>().material.color.Equals(demoStation.discColours[demoStation.expectedDiscColour - 1]))
      {
        currentHeldDisc.GetComponentInChildren<Renderer>().material.SetFloat("_OutlineTransparency", 1);
      }

      // Fling
      Vector3 fwd = gameObject.transform.parent.TransformDirection(Vector3.right);
      Vector3 up = gameObject.transform.parent.TransformDirection(Vector3.up);
      currentHeldDisc.transform.position = currentHeldDisc.transform.position + fwd + up;
      currentHeldDisc.GetComponent<Rigidbody>().AddForce(fwd * 200 + new Vector3(0, 200, 0));
      Debug.Log("Ejecting " + currentHeldDisc.GetComponentInChildren<Renderer>().material.color.ToString());
    }

    currentHeldDisc = null;
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
      CancelRepair(collider.gameObject);
      Debug.Log("Player " + playerIndex + " left the console stand");
      IDToPlayerMap.Remove(playerIndex);
    }
  }
}
