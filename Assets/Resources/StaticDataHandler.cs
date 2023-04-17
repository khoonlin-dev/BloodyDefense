using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDataHandler : MonoBehaviour {

    //This script handles data for cross-scene

    public enum characterCode
    {
        None,
        Neutrophil,
        Macrophage,
        Killer,
        Helper
    };

    public static int characterChosen;  //Can be 0, 1 (Neutrophil), 2 (Macrophage), 3 (Killer) and 4 (Helper)

    public enum scene
    {
        mainMenu, campaign, characterSelection, level5, loadingScene
    }

	// Use this for initialization
	void Start () {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        //characterChosen = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
