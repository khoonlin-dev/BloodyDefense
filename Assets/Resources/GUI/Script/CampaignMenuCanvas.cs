using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CampaignMenuCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2.0f);
    }


    public void returnToMainMenu()
    {
        StartCoroutine(wait());
        SceneManager.LoadScene((int)StaticDataHandler.scene.mainMenu);
    }

    public void selectCharacter()
    {
        StartCoroutine(wait());

        SceneManager.LoadScene((int)StaticDataHandler.scene.characterSelection);
    }
}
