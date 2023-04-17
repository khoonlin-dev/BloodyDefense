using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScreenScript : MonoBehaviour {

    public Image fill;

    public List<Sprite> heroes;

    public List<String> tips;

    private int randomNum;

    public Image heroImage;

    public Text heroTips;

    private bool readyToLoad = false;

    private bool ready;

    static public StaticDataHandler.scene toScene;

    public AsyncOperation async;

    public GameObject loadingBar;

    public GameObject proceedButton;

	// Use this for initialization
	void Start () {

        ready = false;

        System.Random rand = new System.Random(Guid.NewGuid().GetHashCode());

        randomNum = rand.Next(0, 4);

        if(randomNum>3)
        {
            randomNum = 3;
        }

        heroImage.sprite = heroes[randomNum];

        heroTips.text = tips[randomNum];

        readyToLoad = true;

        fill.fillAmount = 0;

        StartCoroutine(wait());

        StartCoroutine(load());
	}
	
    IEnumerator load()
    {
        async = Application.LoadLevelAsync((int)toScene);

        async.allowSceneActivation = false;

        ready = true;

        yield return async;
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2.0f);
    }

    public void proceed()
    {
        StartCoroutine(wait());
        async.allowSceneActivation = true;
    }

	// Update is called once per frame
	void Update () {


        if (async != null && ready)
        {
            loadingBar.SetActive(false);

            proceedButton.SetActive(true);
        }

        else
        {
            if (readyToLoad)
            {
                fill.fillAmount += 0.8f * Time.deltaTime;

                if (fill.fillAmount >= 1.0f)
                {
                    readyToLoad = false;
                    //SceneManager.LoadScene((int)toScene);
                }
            }
        }
	}
}
