using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseScreenTabThree : MonoBehaviour {

    public List<Sprite> characterSprites;

    public List<string> characterDesc;

    public List<string> characterNames;

    private int characterExp;

    private int characterChosen;

    private int characterLvl;

    public Text characterNameText;

    public Text characterDescText;

    public Text characterLevelText;

    public Text characterHpText;

    public Text characterAtkText;

    public Text characterDefText;

    public Image characterExpFill;

    public Image characterSprite;

	// Use this for initialization
	void Start () {

 
	}
	
    void OnEnable()
    {
        characterChosen = (int)StaticDataHandler.characterChosen;

        switch (characterChosen)
        {
            case 1:
                {
                    characterExp = PlayerPrefs.GetInt("NeutrophilExperience");
                    break;
                }
            case 2:
                {
                    characterExp = PlayerPrefs.GetInt("MacrophageExperience");
                    break;
                }
            case 3:
                {
                    characterExp = PlayerPrefs.GetInt("KillerExperience");
                    break;
                }
            case 4:
                {
                    characterExp = PlayerPrefs.GetInt("HelperExperience");
                    break;
                }
        }


        characterNameText.text = characterNames[characterChosen - 1];


        characterLvl = PlayerStatistics.getLevelFromExp(characterChosen, characterExp);

        characterLevelText.text = characterLvl.ToString();

        characterSprite.sprite = characterSprites[characterChosen - 1];

        characterHpText.text = PlayerStatistics.getHPFromLevel(characterChosen, characterLvl).ToString();

        characterAtkText.text = PlayerStatistics.getAttackFromLevel(characterChosen, characterLvl).ToString();

        characterDefText.text = PlayerStatistics.getDefenseFromLevel(characterChosen, characterLvl).ToString();

        characterDescText.text = characterDesc[characterChosen - 1];

        characterExpFill.fillAmount = PlayerStatistics.getCurrentExpPercentage(characterChosen, characterExp);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
