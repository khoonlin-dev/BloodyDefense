using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PressExitLevel : MonoBehaviour {

	// Use this for initialization
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() => toMainMenu());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2.0f);
    }

    void toMainMenu()
    {
        StartCoroutine(wait());
        Time.timeScale = 1.0f;
        SceneManager.LoadScene((int)StaticDataHandler.scene.loadingScene);

        LoadScreenScript.toScene = StaticDataHandler.scene.mainMenu;
    }
}
