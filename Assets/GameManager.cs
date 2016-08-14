using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  [Header("References")]
  [SerializeField]
  GameObject[] playerGbs;
  [SerializeField]
  Text scoreText;
  [SerializeField]
  Text timeText;
  [SerializeField]
  GameObject titleScreen;
  [SerializeField]
  TitleScreen titleScreenLogic;
  [SerializeField]
  ProgramFeed programFeed;

  [Header("Settings")]
  [SerializeField]
  public float gameLength;
  [SerializeField]
  public bool debugMode = false;

  [Header("State")]
  public bool gameInProgress = false;
  public int score = 0;
  public bool gameFinished = false;

  private float timer;
  private float counter;

  void Update()
  {
    if (gameInProgress)
    {
      titleScreen.SetActive(false);
      if (counter < gameLength)
      {
        counter += Time.deltaTime;
      }
      else
      {
        GameFinished();
      }

      scoreText.text = "Score: " + score;
      timeText.text = "Time Left: " + (gameLength - counter);
    }
    else if (!gameFinished)
    {
      // Game hasn't started yet, display title logic
      titleScreen.SetActive(true);
      if (titleScreenLogic.ready)
      {
        // Game is ready to start, record player count
        gameInProgress = true;
        if (debugMode)
        {
          programFeed.numberOfPlayers = 2;
        }
        else
        {
          programFeed.numberOfPlayers = titleScreenLogic.playerCount;
          for (int i = programFeed.numberOfPlayers; i < 4; ++i)
          {
            Destroy(playerGbs[i]);
            playerGbs[i] = null;
          }
        }
      }
    }
    else
    {
      // Game is now over

      // TODO: Show score rating (out of three stars)

      if (Input.GetButtonDown("A_P1"))
      {
        RestartGame();
      }
    }
  }

  public void StartGame()
  {
    SetPlayerControllerState(true);
    gameInProgress = true;
    scoreText.enabled = true;
    timeText.enabled = true;
  }

  public void SetPlayerControllerState(bool state)
  {
    foreach (GameObject gb in playerGbs)
    {
      if (gb != null)
      {
        gb.GetComponent<PlayerMovement>().enabled = state;
      }
    }
  }

  public void GameFinished()
  {
    SetPlayerControllerState(false);
    gameInProgress = false;
    scoreText.enabled = false;
    timeText.enabled = false;
    gameFinished = true;
  }

  public void IncreaseScore(int points)
  {
    score += points;
  }

  public void RestartGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

}
