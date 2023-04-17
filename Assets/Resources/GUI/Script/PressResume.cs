using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressResume : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public GameObject inGameCanvas;

    public void OnPointerDown(PointerEventData eventData)
    {
        Time.timeScale = 1;
        Invoke("resume", 0.2f);
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


    void resume()
    {
        this.transform.root.gameObject.SetActive(false);
        inGameCanvas.SetActive(true);
    }
}
