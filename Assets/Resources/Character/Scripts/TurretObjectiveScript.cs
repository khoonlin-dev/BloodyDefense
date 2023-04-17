using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TurretObjectiveScript : MonoBehaviour {

    public float activateDuration;


    private string objectiveTag = "Objective";

    [System.NonSerialized]
    public bool isActivated = false;

    public GameObject HPBar; 

    private bool isActivating;

    public GameObject turretMaster;

    public GameObject sensingBox;

    public GameObject outlineGlowSprite;

    private string playerMeleeBoxTag;

    private float activationTimeLeft;

    private PlayerStatistics playerStats;

    private PlayerStatistics ownStats;

    private GameObject errorText;

    private GameObject durationBar;

    private int isActivatedHash;

    public Animator minimapSpriteAnimator;

    public GameObject tappingTips;
    
    public BattlefieldNotiSystem notiBar;

    public WinningScript endGame;

    public Text missionText;

    private string missionText1 = "Activate antibody turrets (";
    private string missionText2 = "/3)";

	// Use this for initialization

    void Awake()
    {
        isActivatedHash = Animator.StringToHash("isActivated");
    }

	void Start () {

       
        Physics2D.queriesHitTriggers = true;
        playerMeleeBoxTag = "Player Melee Box";

        playerStats = StartupConfig.player.GetComponent<PlayerStatistics>();

        ownStats = this.GetComponentInParent<PlayerStatistics>();

        errorText = playerStats.errorText;

        durationBar = playerStats.durationBar;

	}
	
	// Update is called once per frame
	void Update () {
        if(isActivating && activationTimeLeft > 0)
        {
            activationTimeLeft -= Time.deltaTime;
            if (activationTimeLeft <= 0)
            {
                activationTimeLeft = 0;

                activateTurret();
            }

            durationBar.GetComponent<Image>().fillAmount = activationTimeLeft / activateDuration;
        }
	}

    void OnEnable()
    {
        minimapSpriteAnimator.SetBool(isActivatedHash, false);
        isActivating = false;
        isActivated = false;
        outlineGlowSprite.SetActive(false);
        HPBar.SetActive(false);
        activationTimeLeft = activateDuration;
        turretMaster.SetActive(false);
        sensingBox.GetComponent<AllySensing>().enabled = false;
    }

    void OnDisable()
    {
        if(isActivating)
        {
            
            outlineGlowSprite.SetActive(false);
            stopActivating();
        }

        if(isActivated && this.CompareTag(objectiveTag))
        {
            StartupConfig.allObjectiveObject.Remove(this.gameObject);

            for (int x = 0; x < StartupConfig.enemyChasingObjective.Count; x++)
            {
                StartupConfig.enemyChasingObjective[x].GetComponent<EnemyAI>().anCharacterIsDestroyed(this.gameObject);
            }

            minimapSpriteAnimator.SetBool(isActivatedHash, false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.CompareTag(playerMeleeBoxTag) && !isActivated)
        {
            outlineGlowSprite.SetActive(true);
            startActivating();
        }

    }


    void OnTriggerExit2D(Collider2D col)
    {
        if (!isActivated &&  col.CompareTag(playerMeleeBoxTag))
        {
            stopActivating();
            outlineGlowSprite.SetActive(false);
        }
    }

    public void activateTurret()
    {

        stopActivating();

        isActivated = true;

        this.gameObject.tag = objectiveTag;

        minimapSpriteAnimator.SetBool(isActivatedHash, true);

        StartupConfig.allObjectiveObject.Add(this.gameObject);

        for (int x = 0; x < StartupConfig.enemyChasingObjective.Count; x++)
        {
            StartupConfig.enemyChasingObjective[x].GetComponent<EnemyAI>().newObjectiveInstantiated(this.gameObject);
        }

        turretMaster.SetActive(true);
        sensingBox.GetComponent<AllySensing>().enabled = true;
        outlineGlowSprite.SetActive(false);

        ownStats.playersCurrentHealthPoint = ownStats.maxHealthPoint;


        //Debug.Log("HP current is : " + playerStats.playersCurrentHealthPoint + ", max health point : " + playerStats.GetComponent<PlayerStatistics>().maxHealthPoint);
        //HPBar.GetComponentInChildren<HealthBar>().updateBar(playerStats.playersCurrentHealthPoint, playerStats.maxHealthPoint);

        HPBar.SetActive(true);

        tappingTips.SetActive(false);


        StartupConfig.objectiveCount--;

        missionText.text = missionText1 + (3 - StartupConfig.objectiveCount) + missionText2;

        GameLevelAudioManager.playSound(GameLevelAudioManager.audio.activation, 0);

        if (StartupConfig.objectiveCount == 0)
        {
            notiBar.flashNotification(2.5f, "All turrets are activated. Killer B-Cells are in defense position. Good work.");

            if (!StartupConfig.isLose)
            {
                StartupConfig.isWon = true;

                Invoke("winGame", 4.0f);
            }
        }
        else
        {
            notiBar.flashNotification(2.5f, "Turret activated. " + StartupConfig.objectiveCount + "/3 turrets are left");
        }


    }

    void winGame()
    {
        endGame.winLevel();
    }

    void startActivating()
    {
        if (!isActivated && outlineGlowSprite.activeInHierarchy && !isActivating && !playerStats.isPlayerUsingProgressBar() && !playerStats.isInSkillState())
        {
            isActivating = true;

            playerStats.enableAllCharacterInputExceptMovement(false);

            errorText.SetActive(false);

            durationBar.GetComponentInChildren<Text>().text = "Activating Now. Go away from the turret";

            durationBar.GetComponent<Image>().fillAmount = 1.0f;

            durationBar.SetActive(true);
        }
    }

    void stopActivating()
    {
        if (!isActivated && outlineGlowSprite.activeInHierarchy && isActivating)
        {
            isActivating = false;

            playerStats.enableAllCharacterInputExceptMovement(true);

            durationBar.GetComponent<Image>().fillAmount = 1.0f;

            durationBar.SetActive(false);

            activationTimeLeft = activateDuration;
        }
    }
}
