using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterScreenScript : MonoBehaviour {

    public Image characterImage;

    public Text nameText;

    public Text detailText;

    public StartupConfig startupConf;

	// Use this for initialization
	void Start () {


        characterImage.sprite = startupConf.heroesSprite[(int)StaticDataHandler.characterChosen - 1];

        nameText.text = startupConf.heroesName[(int)StaticDataHandler.characterChosen - 1];

        detailText.text = startupConf.heroesDetail[(int)StaticDataHandler.characterChosen - 1];

        //switch(StaticDataHandler.characterChosen)
        //{
        //    case 1:
        //        {
                    
        //            break;
        //        }
        //    case 2:
        //        {
        //            break;
        //        }
        //    case 3:
        //        {
        //            break;
        //        }
        //    case 4:
        //        {
        //            break;
        //        }
        //}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
