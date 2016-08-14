using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterMachine : MonoBehaviour {

    public GameObject waterCupPrefab;
    public List<GameObject> playersInRange = new List<GameObject>();

    void Update()
    {
        foreach (GameObject player in playersInRange)
        {
            if (Input.GetButtonDown("A_P" + player.GetComponent<PlayerMovement>().playerIndex) && player.GetComponent<Interaction>().tempObjectinHands == null)
            {
                GameObject newGB = Instantiate(waterCupPrefab, transform) as GameObject;
                newGB.transform.parent = null;
                player.GetComponent<Interaction>().GrabObject(newGB.transform.FindChild("InteractObject").gameObject);
            }
        }
      
    }

    void OnTriggerEnter(Collider collider)
    {
        print(collider.name);
        if (collider.name.Contains("Player") && !playersInRange.Contains(collider.gameObject))
        {
            playersInRange.Add(collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.name.Contains("Player") && playersInRange.Contains(collider.gameObject))
        {
            playersInRange.Remove(collider.gameObject);
        }
    }
}
