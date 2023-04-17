using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressPauseTab : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public PauseScreenScript pauseScript;

    public void OnPointerDown(PointerEventData eventData)
    {
        pauseScript.showTab(this.gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
