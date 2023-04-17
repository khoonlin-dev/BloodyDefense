using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseScreenScript : MonoBehaviour {

    public List<GameObject> tabs;

    public List<GameObject> tabsContents; 

    public Sprite unselectedSprite;

    public Sprite selectedSprite;

    private Vector3 vector3Holder;

    public float tabEnabledXPos;

    public float tabDisabledXPos;

    void Awake()
    {
        vector3Holder = new Vector3();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void showTab(GameObject tab)
    {
        for(int x=0;x<tabs.Count;x++)
        {
            if(tabs[x]!=tab)
            {
                vector3Holder = tabs[x].GetComponent<RectTransform>().anchoredPosition;
                vector3Holder.x = tabDisabledXPos;
                tabs[x].GetComponent<RectTransform>().anchoredPosition = vector3Holder;
                tabs[x].GetComponent<Image>().sprite = unselectedSprite;
                tabsContents[x].SetActive(false);
            }
            else
            {
                vector3Holder = tabs[x].GetComponent<RectTransform>().anchoredPosition;
                vector3Holder.x = tabEnabledXPos;
                tabs[x].GetComponent<RectTransform>().anchoredPosition = vector3Holder;
                tabs[x].GetComponent<Image>().sprite = selectedSprite;
                tabsContents[x].SetActive(true);
            }
        }
    }
}
