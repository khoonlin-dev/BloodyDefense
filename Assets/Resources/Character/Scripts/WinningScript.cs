using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningScript : MonoBehaviour {

    public GameObject gameplayCanvas;

    public GameObject pauseScreenCanvas;

    public GameObject winScreenCanvas;

    public GameObject loseScreenCanvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void loadMainMenu()
    {

        SceneManager.LoadScene((int)StaticDataHandler.scene.loadingScene);

        LoadScreenScript.toScene = StaticDataHandler.scene.mainMenu;

    }

    public void winLevel()
    {

        gameplayCanvas.SetActive(false);

        pauseScreenCanvas.SetActive(false);

        winScreenCanvas.SetActive(true);

        loseScreenCanvas.SetActive(false);

        GameLevelAudioManager.playSound(GameLevelAudioManager.audio.victory, 0);

        Invoke("loadMainMenu", 4.0f);

    }

    public void loseLevel()
    {
        gameplayCanvas.SetActive(false);

        pauseScreenCanvas.SetActive(false);

        winScreenCanvas.SetActive(false);

        loseScreenCanvas.SetActive(true);


        Invoke("loadMainMenu", 4.0f);

    }
}
