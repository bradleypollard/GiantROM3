using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitleScreen : MonoBehaviour
{
  [Header("References")]
  [SerializeField]
  GameObject startGame;
  [SerializeField]
  GameObject p1Prompt;
  [SerializeField]
  GameObject p1Joined;
  [SerializeField]
  GameObject p2Prompt;
  [SerializeField]
  GameObject p2Joined;
  [SerializeField]
  GameObject p3Prompt;
  [SerializeField]
  GameObject p3Joined;
  [SerializeField]
  GameObject p4Prompt;
  [SerializeField]
  GameObject p4Joined;
  [SerializeField]
  GameManager gameManager;
  
  private int playerCount = 0;

  private bool p1Ready = false;
  private bool p2Ready = false;
  private bool p3Ready = false;
  private bool p4Ready = false;

  void Update()
  {
    if (Input.GetButtonDown("A_P1") && !p1Ready)
    {
      gameManager.programFeed.PIDToCIDMap[++playerCount] = 1;
      p1Ready = true;
    }
    if (Input.GetButtonDown("A_P2") && !p2Ready)
    {
      gameManager.programFeed.PIDToCIDMap[++playerCount] = 2;
      p2Ready = true;
    }
    if (Input.GetButtonDown("A_P3") && !p3Ready)
    {
      gameManager.programFeed.PIDToCIDMap[++playerCount] = 3;
      p3Ready = true;
    }
    if (Input.GetButtonDown("A_P4") && !p4Ready)
    {
      gameManager.programFeed.PIDToCIDMap[++playerCount] = 4;
      p4Ready = true;
    }

    UpdatePlayerGUI();

    if (playerCount > 1 && Input.GetButtonDown("A_P" + gameManager.programFeed.PIDToCIDMap[1]))
    {
      gameManager.StartGame(playerCount);
    }

  }


  void UpdatePlayerGUI()
  {
    if (playerCount > 0)
    {
      p1Prompt.SetActive(false);
      p1Joined.SetActive(true);
    }
    else
    {
      p1Prompt.SetActive(true);
      p1Joined.SetActive(false);
    }
    if (playerCount > 1)
    {
      p2Prompt.SetActive(false);
      p2Joined.SetActive(true);
      startGame.SetActive(true);
    }
    else
    {
      p2Prompt.SetActive(true);
      p2Joined.SetActive(false);
      startGame.SetActive(false);
    }
    if (playerCount > 2)
    {
      p3Prompt.SetActive(false);
      p3Joined.SetActive(true);
    }
    else
    {
      p3Prompt.SetActive(true);
      p3Joined.SetActive(false);
    }
    if (playerCount > 3)
    {
      p4Prompt.SetActive(false);
      p4Joined.SetActive(true);
    }
    else
    {
      p4Prompt.SetActive(true);
      p4Joined.SetActive(false);
    }
  }
}