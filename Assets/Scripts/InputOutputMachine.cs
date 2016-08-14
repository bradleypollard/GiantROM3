using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InputOutputMachine : MonoBehaviour
{

  public GameObject acceptedInputItem;
  public GameObject playerUsingMachine;
  public int playerIndexUsingMachine;
  public bool machineInUse;

  public List<GameObject> playersInRange;

  [Range(1, 4)]
  public float machineHoldTime;

  public enum InputType { Hold, Tap };
  public InputType selectedInputType;

  [Range(5, 20)]
  public int machineTapCount;

  public float counter;

  public Slider progressSlider;

  void Start()
  {
    playersInRange = new List<GameObject>();
  }

  void Update()
  {

    if (machineInUse)
    {
      if (selectedInputType == InputType.Hold)
      {
        if (Input.GetButtonUp("A_P" + playerUsingMachine.GetComponent<PlayerMovement>().playerIndex))
        {
          playerUsingMachine.GetComponent<PlayerMovement>().enabled = true;
          acceptedInputItem.SetActive(true);
          acceptedInputItem = null;
          counter = 0;
          machineInUse = false;
          playerUsingMachine = null;
          progressSlider.gameObject.SetActive(false);
        }

        if (counter < machineHoldTime)
        {
          counter += Time.deltaTime;
          progressSlider.value = counter / machineHoldTime;
        }
        else
        {
          Debug.Log("Machine finished, reseting and giving item to player");
          SwapObject(acceptedInputItem);
          playerUsingMachine.GetComponent<PlayerMovement>().enabled = true;
          MachineFinished(playerUsingMachine);
        }
      }
      else if (selectedInputType == InputType.Tap)
      {
        if (Input.GetButtonDown("A_P" + playerUsingMachine.GetComponent<PlayerMovement>().playerIndex))
        {
          if (counter < machineTapCount - 1)
          {
            counter += 1;
            progressSlider.value = counter / machineTapCount;
          }
          else
          {
            Debug.Log("Machine finished, reseting and giving item to player");
            SwapObject(acceptedInputItem);
            playerUsingMachine.GetComponent<PlayerMovement>().enabled = true;
            MachineFinished(playerUsingMachine);
          }
        }
      }
    }
    else
    {
      foreach (GameObject gb in playersInRange)
      {
        if (Input.GetButtonDown("A_P" + gb.GetComponent<PlayerMovement>().playerIndex) && gb.GetComponent<Interaction>().objectInHands != null)
        {
          playerUsingMachine = gb;
          acceptedInputItem = gb.GetComponent<Interaction>().objectInHands;
          machineInUse = true;
          acceptedInputItem.SetActive(false);
          progressSlider.value = 0;
          progressSlider.gameObject.SetActive(true);
          playerUsingMachine.GetComponent<PlayerMovement>().enabled = false;
        }
      }
    }

  }

  void OnTriggerEnter(Collider collider)
  {
    if (collider.gameObject.tag == "Player")
    {
      ItemRecipe itemRecipeCheck = collider.GetComponentInChildren<ItemRecipe>();

      if (itemRecipeCheck != null && itemRecipeCheck.IWorkWithThisMachine == this.name)
      {
        playersInRange.Add(collider.gameObject);
      }
    }

  }

  void OnTriggerExit(Collider collider)
  {
    if (collider.gameObject.tag == "Player" && playersInRange.Contains(collider.gameObject))
    {
      playersInRange.Remove(collider.gameObject);
    }
  }


  private void SwapObject(GameObject oldGB)
  {
    ItemRecipe gbItemRecipe = oldGB.GetComponent<ItemRecipe>();

    GameObject newGb = Instantiate(gbItemRecipe.ITurnInto, Vector3.zero, Quaternion.identity) as GameObject;
    playerUsingMachine.GetComponent<Interaction>().GrabObject(newGb.transform.GetChild(0).gameObject);

    if (gbItemRecipe.CopyColourFromHere)
    {
      newGb.GetComponentInChildren<Renderer>().material.color = gbItemRecipe.CopyColourFromHere.material.color;
    }

    Destroy(oldGB);
  }

  private void MachineFinished(GameObject player)
  {
    playersInRange.Remove(player);
    acceptedInputItem = null;
    counter = 0;
    machineInUse = false;
    playerUsingMachine = null;
    progressSlider.gameObject.SetActive(false);
  }
}
