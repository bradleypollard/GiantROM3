﻿using UnityEngine;
using System.Collections;

public class Microphone : MonoBehaviour
{
  [Header("References")]
  [SerializeField]
  Teleprompter teleprompter;
  [Header("Settings")]
  [Range(0, 4)]
  [SerializeField]
  int speakerID = 0;

  private bool playerCanUseMicrophone = false;
  private Transform playerTransform;

  // Update is called once per frame
  void Update()
  {
    if (playerCanUseMicrophone && Input.GetButton("A_P" + speakerID))
    {
      teleprompter.SetSpeakerID(speakerID, playerTransform);
      playerTransform.gameObject.GetComponent<PlayerMovement>().lockMovement = true;
    }
  }

  void OnTriggerEnter(Collider collider)
  {
    int playerIndex = collider.transform.root.GetComponent<PlayerMovement>().playerIndex;
    playerTransform = collider.transform.root;
    print("Player " + playerIndex + " entered");
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
    print("Player " + collider.transform.root.GetComponent<PlayerMovement>().playerIndex + " left");
    playerCanUseMicrophone = false;
    playerTransform = null;
  }
}