using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int score;

    public bool gameInProgress;
    private float timer;
    public float gameLength;
    private float counter;

    public GameObject[] playerGbs;

    public Text scoreText;
    public Text timeText;

    void Update()
    {
        if (gameInProgress)
        {
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
            gb.GetComponent<PlayerMovement>().enabled = state;
        }
    }

    public void GameFinished()
    {
        SetPlayerControllerState(false);
        gameInProgress = false;
        scoreText.enabled = false;
        timeText.enabled = false;
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
