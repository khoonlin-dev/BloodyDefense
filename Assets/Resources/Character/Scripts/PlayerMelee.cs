using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Animations;

using UnityEngine.Playables;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]

//Playables part credit to https://docs.unity3d.com/Manual/Playables-Examples.html

public class PlayerMelee : MonoBehaviour {

    //This script is to be attached to the player's melee attack box. 

    private GameObject player;

    private PlayerStatistics playerStats;

    private SwipePlayer playerSwipe;

    private GameObject cooldownOneButton;
    private GameObject normalAttackButton;

    private GameObject vpadButton;

    private GameObject durationBar;


    [System.NonSerialized]
    public bool isMunching;

    [System.NonSerialized]
    public bool isHealing;

    [System.NonSerialized]
    public List<GameObject> enemiesWithinAttackRange = new List<GameObject>();

    [System.NonSerialized]
    public List<GameObject> alliesWithinHealRange = new List<GameObject>();

    [System.NonSerialized]
    public float munchingCoolDownLeft;

    [System.NonSerialized]
    public float healingCoolDownLeft;

    [System.NonSerialized]
    public GameObject ultiScreen;

    public float munchingCooldownTime;

    public float munchingDuration;

    private float munchingDurationLeft;

    public int munchDamage;

    public float munchInterval;

    public float normalHealInterval;

    public float healingCooldownTime;

    public float normalHealingDuration;

    private float normalHealingDurationLeft;

    public float normalHealingValue;

    private int allyHealed;


    private bool normalAttackLock;

    public AnimationClip rightSwingAnimation;

    public AnimationClip leftSwingAnimation;

    public AnimationClip normalMunchAnimation;

    private PlayableGraph playableGraph;

    private int isMunchingHash;


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.gameObject.CompareTag("Ally"))
        {
            if(StaticDataHandler.characterChosen==4)
                if (col.transform.gameObject.GetComponent<PlayerStatistics>().playersCurrentHealthPoint != col.transform.gameObject.GetComponent<PlayerStatistics>().maxHealthPoint)
                {
                    col.transform.GetChild(2).gameObject.SetActive(true);
                }   
            //Debug.Log("An ally has entered my healing range");
            alliesWithinHealRange.Add(col.transform.gameObject);
            return;
        }

        if (col.transform.gameObject.CompareTag("Enemy"))
        {
            enemiesWithinAttackRange.Add(col.transform.gameObject);
            return;
        }
        //Debug.Log("New enemy within range! Current list is now " + enemiesWithinAttackRange.Count);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.gameObject.CompareTag("Ally"))
        {
            if (StaticDataHandler.characterChosen == 4)
                col.transform.GetChild(2).gameObject.SetActive(false);
            alliesWithinHealRange.Remove(col.transform.gameObject);
            return;
        }

        if (col.transform.gameObject.CompareTag("Enemy"))
        {
            enemiesWithinAttackRange.Remove(col.transform.gameObject);
            return;
        }
        
        //Debug.Log("An enemy leaves range! Current list is now " + enemiesWithinAttackRange.Count);
    }

	// Use this for initialization
	void Start () 
    {
        normalAttackLock = false;
        isMunching = false;
        isHealing = false;
        munchingCoolDownLeft = 0;
        healingCoolDownLeft = 0;
        player = StartupConfig.player;

        if(player!=null)
        {

            playerStats = player.GetComponent<PlayerStatistics>();

            playerSwipe = player.GetComponent<SwipePlayer>();

            durationBar = playerStats.durationBar;

            vpadButton = playerStats.vpadButton;

            cooldownOneButton = playerStats.cooldownOneButton;

            normalAttackButton = playerStats.normalAttackButton;

            if(StaticDataHandler.characterChosen == 2)  //If macrophage chosen
            {
                isMunchingHash = Animator.StringToHash("isMunching");               
            }
        }
	}
	
	// Update is called once per frame
	void Update () 
    {

        if (munchingDurationLeft > 0)
        {
            munchingDurationLeft -= Time.deltaTime;
            if (munchingDurationLeft <= 0)
            {
                munchingDurationLeft = 0;
            }

            durationBar.GetComponent<Image>().fillAmount = munchingDurationLeft / munchingDuration;
        }

		if(munchingCoolDownLeft>0)
        {

            munchingCoolDownLeft -= Time.deltaTime;
            if (munchingCoolDownLeft <= 0)
            {
                munchingCoolDownLeft = 0;
                //Debug.Log("The munching cooldown has stopped!");
                            
            }

            cooldownOneButton.GetComponent<Image>().fillAmount = munchingCoolDownLeft / munchingCooldownTime;
        }

        if (normalHealingDurationLeft > 0)
        {
            normalHealingDurationLeft -= Time.deltaTime;
            if (normalHealingDurationLeft <= 0)
            {
                normalHealingDurationLeft = 0;
            }

            durationBar.GetComponent<Image>().fillAmount = normalHealingDurationLeft / normalHealingDuration;
        }

        if (healingCoolDownLeft > 0)
        {


            healingCoolDownLeft -= Time.deltaTime;
            if (healingCoolDownLeft <= 0)
            {
                healingCoolDownLeft = 0;
                //Debug.Log("The healing cooldown has stopped!");

            }

            cooldownOneButton.GetComponent<Image>().fillAmount = healingCoolDownLeft / healingCooldownTime;
        }
	}

    public void startMunching()
    {

        playerStats.hideErrorText();

        if (durationBar != null)
        {
            durationBar.SetActive(true);
            durationBar.GetComponentInChildren<Text>().text = "Move Around to Swallow The Enemies";
        }


        if (ultiScreen != null)
            ultiScreen.SetActive(true);

        playerStats.animator.SetBool(isMunchingHash, true);

        //Debug.Log("I have started to munch!");
        CancelInvoke("dealNormalDamageToEnemy");
        munchingDurationLeft = munchingDuration;
        InvokeRepeating("dealMunchDamageToEnemy", 0, munchInterval);
        normalAttackButton.SetActive(false);
        Invoke("stopMunching", munchingDuration);

        GameLevelAudioManager.pauseBGMandPlay(GameLevelAudioManager.audio.ulti);


    }

    public void stopMunching()
    {
        if (durationBar != null)
            durationBar.SetActive(false);
        if (normalAttackButton!=null)
            normalAttackButton.SetActive(true);

        playerStats.animator.SetBool(isMunchingHash, false);
        CancelInvoke("dealMunchDamageToEnemy");
        isMunching = false;
        //Debug.Log("I have stopped munching!");
        CancelInvoke("stopMunching");

        if (ultiScreen != null)
            ultiScreen.SetActive(false);

        GameLevelAudioManager.resumeBGMandStop(GameLevelAudioManager.audio.ulti);
        
    }

    public void startNormalAttack()
    {
        if (!normalAttackLock)
        {
            if (StaticDataHandler.characterChosen != 2)    //If macrophage not chosen
            {
                if (playerStats.playerDirection.x > 0)
                {
                    if (StaticDataHandler.characterChosen == 1)
                        AnimationPlayableUtilities.PlayClip(player.transform.Find("Sword").GetComponent<Animator>(), rightSwingAnimation, out playableGraph);
                    else if (StaticDataHandler.characterChosen == 4)
                    {
                        AnimationPlayableUtilities.PlayClip(player.transform.Find("Kit").GetComponent<Animator>(), rightSwingAnimation, out playableGraph);
                    }
                    //Right Swing
                }
                else
                {
                    if (StaticDataHandler.characterChosen == 1)
                    {
                        AnimationPlayableUtilities.PlayClip(player.transform.Find("Sword").GetComponent<Animator>(), leftSwingAnimation, out playableGraph);
                        GameLevelAudioManager.playSound(GameLevelAudioManager.audio.swoosh, 0);
                    }
                    else if (StaticDataHandler.characterChosen == 4)
                    {
                        AnimationPlayableUtilities.PlayClip(player.transform.Find("Kit").GetComponent<Animator>(), leftSwingAnimation, out playableGraph);
                    }
                    //Left Swing
                }
            }   //End of if macrophage is not selected
            else
            {
                AnimationPlayableUtilities.PlayClip(playerStats.animator, normalMunchAnimation, out playableGraph);
            }

            Invoke("dealNormalDamageToEnemy", 0.1f); //this start time should be sync with player's attack animation
          
            normalAttackLock = true;
        }
    }

    public void stopNormalAttack()
    {

        if (playableGraph.IsValid())
        {
            playableGraph.Stop();
            playableGraph.Destroy();
        }
        normalAttackLock = false;
        CancelInvoke("dealNormalDamageToEnemy");
    }

    public void startNormalHealing()
    {
        playerStats.hideErrorText();

        if (durationBar != null)
        {
            durationBar.SetActive(true);
            durationBar.GetComponentInChildren<Text>().text = "Healing Now. Don't Move or Attack";
        }
        lockVpad(2.0f);
        CancelInvoke("dealNormalDamageToEnemy");
        normalHealingDurationLeft = normalHealingDuration;
        InvokeRepeating("healAllyPerSecond", 0, normalHealInterval);
        Invoke("stopNormalHealing", normalHealingDuration);
        isHealing = true;

        GameLevelAudioManager.playSound(GameLevelAudioManager.audio.healing, 0);
        //Debug.Log("I've Started Healing for every " + normalHealInterval+ "s");
    }

    public void stopNormalHealing()
    {

        if (durationBar != null)
            durationBar.SetActive(false);
        CancelInvoke("healAllyPerSecond");
        isHealing = false;
        for (int x = 0; x < alliesWithinHealRange.Count; x++)
        {
            if (alliesWithinHealRange[x] == null || alliesWithinHealRange[x].activeSelf == false)
            {
                alliesWithinHealRange.Remove(alliesWithinHealRange[x]);
                x--;
                continue;
            }

            alliesWithinHealRange[x].GetComponent<PlayerStatistics>().stopHealingEffect();
        }

        releaseVpadLock();

        //Debug.Log("I've Stop Healing");
        CancelInvoke("stopNormalHealing");
    }

    void healAllyPerSecond()
    {
        allyHealed = 0;

        for (int x = 0; x < alliesWithinHealRange.Count; x++)
        {

            if (alliesWithinHealRange[x] == null || alliesWithinHealRange[x].activeSelf==false)
            {
                alliesWithinHealRange.Remove(alliesWithinHealRange[x]);
                x--;
                continue;
            }

            if(alliesWithinHealRange[x].GetComponent<PlayerStatistics>().playersCurrentHealthPoint < alliesWithinHealRange[x].GetComponent<PlayerStatistics>().maxHealthPoint)
            {

                alliesWithinHealRange[x].GetComponent<PlayerStatistics>().playHealingEffect(); //The function will check so we don't have to

                //Debug.Log("I've found one ally need healing. I will heal him " + player.normalHealingValue);
                if(alliesWithinHealRange[x].GetComponent<PlayerStatistics>().healDamage((int)normalHealingValue))
                {
                    alliesWithinHealRange[x].transform.GetChild(2).gameObject.SetActive(false);

                    //allyHealed++;
                }

                
            }
            else
            {
                alliesWithinHealRange[x].GetComponent<PlayerStatistics>().stopHealingEffect();

                allyHealed++;
            }
            
            //alliesWithinHealRange[x].GetComponent<Enemy>().sufferDamage(PlayerStatistics.playersMunchDamage);
        }

        if(allyHealed == alliesWithinHealRange.Count)   //if all allies are already fully healed during this loop, end the healing prematurely.
        {
            stopNormalHealing();
        }

    }

    void dealMunchDamageToEnemy()
    {
        for (int x = 0; x < enemiesWithinAttackRange.Count; x++)
        {
            enemiesWithinAttackRange[x].GetComponent<Enemy>().sufferDamage(munchDamage, false);
        }
    }

    void dealNormalDamageToEnemy()
    {
        for(int x=0;x<enemiesWithinAttackRange.Count;x++)
        {
            enemiesWithinAttackRange[x].GetComponent<Enemy>().sufferDamage(player.GetComponent<PlayerStatistics>().normalDamage, false);
        }
        Invoke("stopNormalAttack", 0.25f);
    }

    public bool areEveryAllyOk()
    {

        if (alliesWithinHealRange.Count==0)
        {
            playerStats.flashErrorText("There is no allies within range", 1.0f, 0.5f);
            return true;
        }

        for (int x = 0; x < alliesWithinHealRange.Count; x++)
        {
            if (alliesWithinHealRange[x].GetComponent<PlayerStatistics>().playersCurrentHealthPoint < alliesWithinHealRange[x].GetComponent<PlayerStatistics>().maxHealthPoint)
            {
                return false;
            }
        }

        playerStats.flashErrorText("Your allies needs no help from you now", 1.0f, 1.0f);
        return true;
    }

    void lockVpad(float duration)
    {
        vpadButton.GetComponent<VJHandler>().enabled = false;
        Invoke("releaseVpadLock", duration);
    }

    void releaseVpadLock()
    {
        if (vpadButton!=null)
            vpadButton.GetComponent<VJHandler>().enabled = true;
    }

    void OnDestroy()
    {
        isMunching = false;
        isHealing = false;
        munchingCoolDownLeft = 0;
        healingCoolDownLeft = 0;
        enemiesWithinAttackRange.Clear();
        alliesWithinHealRange.Clear();

        releaseVpadLock();

        if(playableGraph.IsValid())
            playableGraph.Destroy();
    }

    public bool isInSkillState()
    {
        if(isMunching || isHealing || playerSwipe.isDashableState)
        {
            return true;
        }

        return false;
    }
}
