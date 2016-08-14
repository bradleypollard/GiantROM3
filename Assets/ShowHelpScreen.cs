using UnityEngine;
using System.Collections;

public class ShowHelpScreen : MonoBehaviour {

  public GameObject helpScreen;
  public GameManager gameManager;

	void Update () 
  {

    if(Input.GetButtonDown("Start") && gameManager.gameInProgress)
    {
      if(helpScreen.activeSelf) {
        helpScreen.SetActive(false);
        gameManager.SetPauseState(false);
      } else {
        helpScreen.SetActive(true);
        gameManager.SetPauseState(true);
      }
    }
	}

}
