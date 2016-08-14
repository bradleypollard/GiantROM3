using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NPCZone : MonoBehaviour
{

  [Header("References")]
  [SerializeField]
  public GameObject npcAccepted;
  [SerializeField]
  public Slider progressBar;
  [SerializeField]
  public GameObject requireWaterIcon;
  [SerializeField]
  GameObject litIcon;
  [SerializeField]
  Renderer lightMesh;
  [SerializeField]
  ProgramFeed programFeed;
  [SerializeField]
  GameObject ring;
  [SerializeField]
  Renderer[] waterCooler;

  [Header("Settings")]
  [SerializeField]
  public bool npcSpeaking;
  [SerializeField]
  [Range(1, 8)]
  public float npcSpeakingTime;
  public Color notInUseColour;
  public Color inUseColour;
  public Color finishedColour;

  [Header("State")]
  public bool requireWater;
  public float timeTilWater;
  public bool waitingWater;
  [SerializeField]
  public bool isLit = false;
  public string gender = "Undefined";

  private float counter;

  void Update()
  {
    if (npcAccepted && npcAccepted.transform.parent == null)
    {
      ring.SetActive(false);
      if (npcSpeaking)
      {
        progressBar.gameObject.SetActive(true);
        // Display lit icon
        litIcon.SetActive(!isLit);
        // Outline light station
        lightMesh.material.SetFloat("_OutlineTransparency", isLit ? 0 : 1);

        if (requireWater && !waitingWater && counter > timeTilWater)
        {
          waitingWater = true;
          requireWaterIcon.SetActive(true);
          foreach (Renderer r in waterCooler)
          {
            r.material.SetFloat("_OutlineTransparency", 1);
          }
        }
        else if (counter < npcSpeakingTime && !waitingWater && isLit)
        {
          counter += Time.deltaTime;
          progressBar.value = counter / npcSpeakingTime;
        }
        else if (counter > npcSpeakingTime)
        {
          GetComponent<Renderer>().material.SetColor("_Color", finishedColour);
          Destroy(npcAccepted);
          programFeed.OnNPCFinished();
          ResetZone();
        }
      }
      else
      {
        npcSpeaking = true;
        GetComponent<Renderer>().material.SetColor("_Color", inUseColour);
      }
    }
    else
    {
      ring.SetActive(true);
      // Display lit icon
      litIcon.SetActive(false);
      // Outline light station
      lightMesh.material.SetFloat("_OutlineTransparency", 0);
    }
  }

  void OnTriggerEnter(Collider collider)
  {
    Debug.Log(collider.name + " Entered");

    if (collider.transform.name.Contains("Dressed") && collider.transform.name.Contains(gender) && !npcAccepted)
    {
      npcAccepted = collider.gameObject;
      if (Random.Range(0, 10) > 5)
      {
        requireWater = true;
        timeTilWater = Random.Range(1, npcSpeakingTime);
      }
      else
      {
        requireWater = false;
        timeTilWater = 0;
      }
    }

    if (waitingWater && collider.name.Contains("Water"))
    {
      waitingWater = false;
      requireWater = false;
      requireWaterIcon.SetActive(false);
      foreach (Renderer r in waterCooler)
      {
        r.material.SetFloat("_OutlineTransparency", 0);
      }
    }
  }

  void OnTriggerExit(Collider collider)
  {
    if (collider.transform.name.Contains("Dressed"))
    {
      ResetZone();
    }
  }

  private void ResetZone()
  {
    npcAccepted = null;
    GetComponent<Renderer>().material.SetColor("_Color", notInUseColour);
    counter = 0;
    progressBar.value = counter;
    progressBar.gameObject.SetActive(false);
  }

}
