using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int score;

    private bool gameInProgress;
    private float timer;
    public float gameLength;
    private float counter;

    public GameObject[] playerGbs;

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
        }
    }

    public void StartGame()
    {
        foreach(GameObject gb in playerGbs)
        {
            gb.GetComponent<PlayerMovement>().enabled = true;
        }

        gameInProgress = true;

    }

    public void GameFinished()
    {

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
