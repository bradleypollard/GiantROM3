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
  GameObject endScreen;
  [SerializeField]
  Text scoreEndText;
  [SerializeField]
  TitleScreen titleScreenLogic;
  [SerializeField]
  public ProgramFeed programFeed;
  [SerializeField]
  AudioSource musicPlayer;
  [SerializeField]
  AudioClip inGameMusic;
  [SerializeField]
  AudioClip endGameMusic;

  [Header("Settings")]
  [SerializeField]
  public float gameLength;
  [SerializeField]
  public bool debugMode = false;

  [Header("State")]
  public bool gameInProgress = false;
  public int score = 0;
  public bool gameFinished = false;
  public bool gamePaused = false;

  private float timer;
  private float counter;

  void Update()
  {
    if (gameInProgress)
    {
      // Game is running, update score and timer
      titleScreen.SetActive(false);
      if (counter < gameLength)
      {
        if (!gamePaused)
        {
          counter += Time.deltaTime;
        }
      }
      else
      {
        GameFinished(); // Time up
      }

      scoreText.text = "Score: " + score;
      System.TimeSpan t = System.TimeSpan.FromSeconds(Mathf.RoundToInt((gameLength - counter)));
      string timeRemaining = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
      timeText.text = "Time Left: " + timeRemaining;
    }
    else if (!gameFinished)
    {
      // Game hasn't started yet, display title logic
      titleScreen.SetActive(true);
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

  public void StartGame(int playerCount)
  {
    if (playerCount < 1 || playerCount > 4)
    {
      Debug.LogError("PLAYER COUNT INCORRECT: " + playerCount);
    }

    foreach (KeyValuePair<int,int> pair in programFeed.PIDToCIDMap)
    {
      playerGbs[pair.Key - 1].GetComponent<PlayerMovement>().playerIndex = pair.Value;
    }
    
    // Game is ready to start, record player count
    gameInProgress = true;
    if (debugMode)
    {
      programFeed.numberOfPlayers = 2;
    }
    else
    {
      programFeed.numberOfPlayers = playerCount;
      for (int i = programFeed.numberOfPlayers; i < 4; ++i)
      {
        Destroy(playerGbs[i]);
        playerGbs[i] = null;
      }
    }
    SetPlayerControllerState(true);
    gameInProgress = true;
    scoreText.enabled = true;
    timeText.enabled = true;

    // Play end game music
    musicPlayer.Stop();
    musicPlayer.clip = inGameMusic;
    musicPlayer.Play();

    // Run first time initialisation of program feed
    programFeed.Init();
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
    endScreen.SetActive(true);

    scoreEndText.text = "You scored:\n " + score;

    // Play end game music
    musicPlayer.Stop();
    musicPlayer.clip = endGameMusic;
    musicPlayer.Play();
  }

  public void IncreaseScore(int points)
  {
    score += points;
  }

  public void RestartGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void SetPauseState(bool state)
  {
    if(state) {
      SetPlayerControllerState(false);
      gamePaused = true;
    } else {
      SetPlayerControllerState(true);
      gamePaused = false;
    }
  }

}
