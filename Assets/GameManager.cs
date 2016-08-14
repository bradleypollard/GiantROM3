using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int score;

    private bool gameInProgress;
    private float timer;
    public float gameLength;

    public GameObject[] playerGBs;

    void Update()
    {
        if (gameInProgress)
        {
            if (gameLength > 0)
            {
                gameLength -= Time.deltaTime;
            }
            else
            {
                GameFinished();
            }
        }
    }

    public void StartGame()
    {
        foreach(GameObject gb in playerGBs)
        {
            gb.GetComponent<PlayerMovement>().enabled = true;
        }
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
