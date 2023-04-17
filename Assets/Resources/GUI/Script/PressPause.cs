using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressPause : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public GameObject pauseScreenCanvas;

	// Use this for initialization
	void Start () {
		
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        Invoke("pause", 0.2f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
    

	// Update is called once per frame
	void Update () {
		
	}

    void pause()
    {
        Time.timeScale = 0;
        this.transform.root.gameObject.SetActive(false);
        pauseScreenCanvas.SetActive(true);
    }
}
