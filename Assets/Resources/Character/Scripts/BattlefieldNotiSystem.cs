using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlefieldNotiSystem : MonoBehaviour {

    public GameObject notiBar;

    public GameObject text;

    public Image notiBarImage;

    private Color notiBarColor;

    private bool isFading;

	// Use this for initialization
	void Start () {



        isFading = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (isFading)
        {

            notiBarColor = notiBarImage.color;

            notiBarColor.a -= 0.5f* Time.deltaTime;

            notiBarImage.color = notiBarColor;

            notiBarColor = this.text.GetComponent<Text>().color;

            notiBarColor.a -= 0.5f * Time.deltaTime;

            this.text.GetComponent<Text>().color = notiBarColor;

            //Debug.Log(notiBarImage.color);

            if (notiBarImage.color.a <= 0)
            {
                isFading = false;
            }
        }
	}

    public void flashNotification(float duration, string text)
    {
        Debug.Log("FlashedNoti~");

        GameLevelAudioManager.playSound(GameLevelAudioManager.audio.notification, 0);

        this.text.GetComponent<Text>().text = text;

        notiBarColor = notiBarImage.color;

        notiBarColor.a = 0.88f;

        notiBarImage.color = notiBarColor;

        notiBarColor = this.text.GetComponent<Text>().color;

        notiBarColor.a = 0.88f;

        this.text.GetComponent<Text>().color = notiBarColor;

        Invoke("startFade", duration);
    }

    void startFade()
    {
        isFading = true;
    }
}
