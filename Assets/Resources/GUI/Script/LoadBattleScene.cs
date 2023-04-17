using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadBattleScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //this.gameObject.SetActive(false);
        this.GetComponent<Button>().onClick.AddListener(() => switchScene());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2.0f);
    }


    void switchScene()
    {
        StartCoroutine(wait());
        SceneManager.LoadScene((int)StaticDataHandler.scene.loadingScene);
        LoadScreenScript.toScene = StaticDataHandler.scene.level5;
    }
}
