using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressSelectHero : MonoBehaviour {

    public MainMenuCanvasController mainMenu;

	// Use this for initialization
	void Start () 
    {

        this.GetComponent<Button>().onClick.AddListener(() => showHero());
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void showHero()
    {
        mainMenu.mainMenuCanvas.gameObject.SetActive(false);
        mainMenu.heroSelectionCanvas.gameObject.SetActive(true);
        MainMenuCanvasController.isSelectingHero = true;
        mainMenu.highlightAllObjects(true);
    }

    public void hideHero()
    {
        mainMenu.mainMenuCanvas.gameObject.SetActive(true);
        mainMenu.heroSelectionCanvas.gameObject.SetActive(false);
        MainMenuCanvasController.isSelectingHero = false;
        mainMenu.highlightAllObjects(false);
    }

    public void HideHeroInfo()
    {
        
        mainMenu.heroDetailCanvas.gameObject.SetActive(false);
        mainMenu.heroSelectionCanvas.gameObject.SetActive(true);
        MainMenuCanvasController.isSelectingHero = true;

    }

    public void ShowSettings()
    {
        mainMenu.settingCanvas.gameObject.SetActive(true);
        mainMenu.mainMenuCanvas.gameObject.SetActive(false);
    }

    public void HideSettings()
    {
        mainMenu.settingCanvas.gameObject.SetActive(false);
        mainMenu.mainMenuCanvas.gameObject.SetActive(true);
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2.0f);
    }


    public void GoToCampaign()
    {
        StartCoroutine(wait());
        SceneManager.LoadScene((int)StaticDataHandler.scene.campaign);
    }
}
