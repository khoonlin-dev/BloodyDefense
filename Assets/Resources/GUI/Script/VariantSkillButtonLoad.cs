using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariantSkillButtonLoad : MonoBehaviour {

    public GameObject normalAttackButton;
    public GameObject skillOneButton;

    public GameObject skillTwoButton;

    public GameObject rightJoystickButton;

	// Use this for initialization
	void Start () {
		switch(StaticDataHandler.characterChosen)
        {
            case 0:
                {
                    Debug.Log("Error, no character chosen!");
                    break;
                }
            case 1:
                {
                    if (normalAttackButton != null)
                    {
                        normalAttackButton.SetActive(true);
                        normalAttackButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("GUI/Graphic/normal-attack-button");
                        PressNormalAttack normalAttack = normalAttackButton.AddComponent<PressNormalAttack>() as PressNormalAttack;
                    }
                    skillOneButton.SetActive(true);
                    skillOneButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("GUI/Graphic/skill-sword-dash-button");
                    PressEnableDash skillOne = skillOneButton.AddComponent<PressEnableDash>() as PressEnableDash;

                    if (skillTwoButton != null)
                        skillTwoButton.SetActive(false);

                    break;
                }
            case 2:
                {
                    if (normalAttackButton != null)
                    {
                        normalAttackButton.SetActive(true);
                        normalAttackButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("GUI/Graphic/normal-attack-button");
                        PressNormalAttack normalAttack = normalAttackButton.AddComponent<PressNormalAttack>() as PressNormalAttack;
                    }
                    skillOneButton.SetActive(true);
                    skillOneButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("GUI/Graphic/skill-glutton-button");
                    PressMunch skillOne = skillOneButton.AddComponent<PressMunch>() as PressMunch;

                    if (skillTwoButton != null)
                        skillTwoButton.SetActive(false);
                    break;
                }
            case 3:
                {
                    if (normalAttackButton != null)
                    {
                        normalAttackButton.SetActive(false);
                        //normalAttackButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("GUI/Graphic/normal-shooting-button");
                        //PressShoot normalAttack = normalAttackButton.AddComponent<PressShoot>() as PressShoot;
                    }

                    if (skillOneButton != null)
                        skillOneButton.SetActive(false);

                    if (skillTwoButton != null)
                        skillTwoButton.SetActive(false);

                    if (rightJoystickButton != null)
                    {
                        rightJoystickButton.SetActive(true);
                    }

                    break;
                }
            case 4:
                {
                    if (normalAttackButton != null)
                    {
                        normalAttackButton.SetActive(true);
                        normalAttackButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("GUI/Graphic/normal-attack-button");
                        PressNormalAttack normalAttack = normalAttackButton.AddComponent<PressNormalAttack>() as PressNormalAttack;
                    }

                    skillOneButton.SetActive(true);
                    skillOneButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("GUI/Graphic/skill-simple-heal-button");
                    PressNormalHealing skillOne = skillOneButton.AddComponent<PressNormalHealing>() as PressNormalHealing;

                    skillTwoButton.SetActive(true);
                    skillTwoButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("GUI/Graphic/skill-summon-button");
                    PressSummon skillTwo = skillTwoButton.AddComponent<PressSummon>() as PressSummon;


                    break;
                }
        }
	}

	// Update is called once per frame
	void Update () {
		
	}
}
