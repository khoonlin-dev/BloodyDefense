using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PressSelectCharacter : MonoBehaviour {

    public StaticDataHandler.characterCode character;
    public GameObject startButton;

    public List<GameObject> highlights = new List<GameObject>();

	// Use this for initialization
	void Start () {
        this.GetComponent<Button>().onClick.AddListener(() => setCharacter());
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void setCharacter()
    {
        StaticDataHandler.characterChosen = (int)character;
        startButton.SetActive(true);
        Debug.Log("Is Active liao");
        for(int x=0;x<highlights.Count;x++)
        {
            if(x==0)
            {
                highlights[x].SetActive(true);
                continue;
            }
            highlights[x].SetActive(false);
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2.0f);
    }

    public void returnToCampaign()
    {
        StartCoroutine(wait());
        SceneManager.LoadScene((int)StaticDataHandler.scene.campaign);
    }
}
