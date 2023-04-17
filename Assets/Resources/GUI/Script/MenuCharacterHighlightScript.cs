using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCharacterHighlightScript : MonoBehaviour {

    public GameObject highlight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void highlightObject(bool toHighlight)
    {
        if (toHighlight)
        {
            highlight.SetActive(true);
        }
        else
            highlight.SetActive(false);
    }
}
