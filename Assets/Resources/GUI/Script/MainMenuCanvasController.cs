using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCanvasController : MonoBehaviour {

    public Canvas mainMenuCanvas;

    public Canvas heroSelectionCanvas;

    public Canvas heroDetailCanvas;

    public Canvas settingCanvas;

    static public bool isSelectingHero = false;

    [System.NonSerialized]
    public List<GameObject> activeHeroesOnScreen = new List<GameObject>();


    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void showHeroDetail()
    {
        heroSelectionCanvas.gameObject.SetActive(false);
        heroDetailCanvas.gameObject.SetActive(true);
        isSelectingHero = false;
    }

    public void highlightAllObjects(bool toHighlight)
    {
        for(int x=0;x<activeHeroesOnScreen.Count;x++)
        {
            activeHeroesOnScreen[x].GetComponent<MenuCharacterHighlightScript>().highlightObject(toHighlight);
        }
    }
}
