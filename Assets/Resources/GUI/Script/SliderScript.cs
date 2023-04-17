using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour, IEndDragHandler
{


    public enum sliderFunction
    {
        musicVolume, soundVolume
    };

    public sliderFunction slidersFunction;

    public Text valueText;

    private Slider slider;

    private string valueStringKey;

    private int sliderValueInt;

    public AudioManagerMainMenu audioManager; 

	// Use this for initialization
	void Start () {

        slider = this.GetComponent<Slider>();

        slider.onValueChanged.AddListener(delegate { onValueChanged(); });

        switch (slidersFunction)
        {
            case sliderFunction.musicVolume:
                {
                    valueStringKey = "Music Volume";
                    break;
                }
            case sliderFunction.soundVolume:
                {
                    valueStringKey = "Sound Volume";
                    break;
                }
        }

        if(valueStringKey!=null && PlayerPrefs.HasKey(valueStringKey))
        {
            sliderValueInt = PlayerPrefs.GetInt(valueStringKey);
            valueText.text = sliderValueInt.ToString();
            slider.value = (float)sliderValueInt / 100.0f;
        }

        else if (!PlayerPrefs.HasKey(valueStringKey))
        {
            PlayerPrefs.SetInt(valueStringKey, 50);
            valueText.text = sliderValueInt.ToString();
            slider.value = (float)sliderValueInt / 100.0f;
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void onValueChanged()
    {
        valueText.text = ((int)(slider.value * 100)).ToString() ;

        if (slidersFunction == sliderFunction.musicVolume)
        {
            audioManager.setMusicVolume(((int)(slider.value * 100)));
            GameLevelAudioManager.adjustMusicVolume(((int)(slider.value * 100)));
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        Debug.Log(PlayerPrefs.HasKey(valueStringKey));

        if(valueStringKey!=null && PlayerPrefs.HasKey(valueStringKey))
        {


            PlayerPrefs.SetInt(valueStringKey, ((int)(slider.value * 100)));

            audioManager.playNormalButtonSound();

        }

    }

}
