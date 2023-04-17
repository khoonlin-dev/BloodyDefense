using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using UnityEngine.Animations;

using UnityEngine.Playables;



public class PlayerStatistics : MonoBehaviour {

    public AllyPool allyPool;

    public GameObject SummonBoxes;

    public float allySummonCooldown;

    [System.NonSerialized]
    public float allySummonCooldownLeft;

    [System.NonSerialized]
    public ExplosionPoolObject explosionPool;

    [System.NonSerialized]
    public Vector2 playerDirection; //Independant of character's local scale. 

    [System.NonSerialized]
    public Vector2 playerPosition;

    private Vector3 vector3Holder;

    public float playersCurrentHealthPoint;

    private string untaggedString = "Untagged";
    
    public float bulletFiringInterval;  

    public GameObject healthBarObject;

    public GameObject healthBarPivot;

    [System.NonSerialized]
    public GameObject errorText;

    [System.NonSerialized]
    public GameObject durationBar;

    [System.NonSerialized]
    public GameObject normalAttackButton;
    [System.NonSerialized]
    public GameObject skillOneButton;
    [System.NonSerialized]
    public GameObject vpadButton;
    [System.NonSerialized]
    public GameObject rightVpadButton;
    [System.NonSerialized]
    public GameObject cooldownOneButton;
    [System.NonSerialized]
    public GameObject skillTwoButton;


    [System.NonSerialized]
    public bool isImmune;

    public int allySummonNum;

    [System.NonSerialized]
    public bool isShooting;

    public GameObject healingParticleObject;

    [System.NonSerialized]
    private ParticleSystem healingParticleSystem;

    [System.NonSerialized]
    public Animator animator;

    [System.NonSerialized]
    public bool isMunching;
    //public List <GameObject> enemiesWithinAttackRange = new List <GameObject>();

    public int bulletDamage;

    public int normalDamage;

    public int maxHealthPoint;

    private Color colorPlaceHolder;

    public GameObject gun;

    [System.NonSerialized]
    public bool isActive;

    private int isDefeatedHash;

    public AnimationClip killerGunDefeated;

    private PlayableGraph playableGraph;

    [System.NonSerialized]
    public WinningScript endGame;

	// Use this for initialization
	void Start () {


        this.explosionPool = StartupConfig.explosion;

        this.allyPool = StartupConfig.allyPool;

        if (playersCurrentHealthPoint <1.0f)
        {
            playersCurrentHealthPoint = maxHealthPoint;
        }

        isActive = false;

        healingParticleSystem = healingParticleObject.GetComponent<ParticleSystem>();

        stopHealingEffect(true);

        animator = this.GetComponent<Animator>();

        if(animator==null||this.gameObject.CompareTag("Ally"))
        {
            animator = this.gameObject.transform.GetChild(5).GetComponent<Animator>();
        }

        colorPlaceHolder = new Color();

        isDefeatedHash = Animator.StringToHash("isDefeated");

        vector3Holder = new Vector3();

        if(this.gameObject.CompareTag("Player"))
        {
            isActive = true;
        }
	}
	
	// Update is called once per frame
	void Update () {

        
        playerPosition = transform.position;


	}

    void OnEnable()
    {

        if (StaticDataHandler.characterChosen == 3 || this.gameObject.CompareTag("Ally"))
        {
            if(animator!=null)
                animator.applyRootMotion = false;
        }

        StartupConfig.allGoodGuysOnMap.Add(this.gameObject);

        if(this.gameObject.CompareTag("Ally"))
        {
            StartupConfig.activeAllyOnMap.Add(this.gameObject);

            Debug.Log("StartupConfig.enemyChasingCharacter.Count");

            for (int x=0;x<StartupConfig.enemyChasingCharacter.Count;x++)
            {
                StartupConfig.enemyChasingCharacter[x].GetComponent<EnemyAI>().newAllyInstantiated(this.gameObject);
            }
        }
        else if (this.gameObject.CompareTag("Player"))
        {
            for (int x = 0; x < StartupConfig.enemyChasingCharacter.Count; x++)
            {
                StartupConfig.enemyChasingCharacter[x].GetComponent<EnemyAI>().newPlayerInstantiated(this.gameObject);
            }
        }
        else if (this.gameObject.CompareTag("Objective"))
        {

            StartupConfig.allObjectiveObject.Add(this.gameObject);

            for (int x = 0; x < StartupConfig.enemyChasingObjective.Count; x++)
            {
                StartupConfig.enemyChasingObjective[x].GetComponent<EnemyAI>().newObjectiveInstantiated(this.gameObject);
            }
        }

        playerDirection = new Vector2(-1, 0);

        isShooting = false;


        if(healingParticleSystem!=null)
        {
            stopHealingEffect(true);
        }

        if ((playerDirection.x <= 0 && transform.localScale.x < 0) || (playerDirection.x > 0 && gameObject.transform.localScale.x > 0))
        {
            flipPlayer();

        }

        this.playersCurrentHealthPoint = this.maxHealthPoint;

        healthBarObject.GetComponent<HealthBar>().updateBar(playersCurrentHealthPoint, maxHealthPoint);
    }

    void OnDisable()
    {
        StartupConfig.allGoodGuysOnMap.Remove(this.gameObject);

        if (this.gameObject.CompareTag("Ally"))
        {
            StartupConfig.activeAllyOnMap.Remove(this.gameObject);
            allyPool.removeThisObject(this.gameObject);

        }
        else if (this.gameObject.CompareTag("Objective"))
        {
            StartupConfig.allObjectiveObject.Remove(this.gameObject);
        }

        for (int x = 0; x < StartupConfig.enemyChasingCharacter.Count; x++)
        {
            StartupConfig.enemyChasingCharacter[x].GetComponent<EnemyAI>().anCharacterIsDestroyed(this.gameObject);
        }
        for (int x = 0; x < StartupConfig.enemyChasingObjective.Count; x++)
        {
            StartupConfig.enemyChasingObjective[x].GetComponent<EnemyAI>().anCharacterIsDestroyed(this.gameObject);
        }


            stopHealingEffect(true);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!isImmune)
        {
            if (col.gameObject.CompareTag("EnemyBullet"))
            {
               
                sufferDamage(col.transform.gameObject.GetComponent<BulletBehavior>().enemyStats.enemyDamage);
                return;
            }
        }

    }

    private void invokeImmunity(float time)
    {
        isImmune = true;
        Invoke("cancelImmunity", time);
    }

    private void cancelImmunity()
    {
        isImmune = false;
        CancelInvoke("cancelImmunity");
        return;
    }

    public bool healDamage(int healingValue)
    {
        //Return false if not eligible to heal or not fully healed. Return true if fully healed.



        if (healingValue <= 0 || playersCurrentHealthPoint == maxHealthPoint)
        {
            return false;
        }

        playersCurrentHealthPoint += healingValue;

        if (playersCurrentHealthPoint >= maxHealthPoint)
        {
            playersCurrentHealthPoint = maxHealthPoint;
        }

        healthBarObject.GetComponent<HealthBar>().updateBar(playersCurrentHealthPoint, maxHealthPoint);

        if (playersCurrentHealthPoint == maxHealthPoint)
        {
            return true;
        }
        else
            return false;

        //Debug.Log("I have healed" + healingValue + " damage. My HP is now " + playersCurrentHealthPoint);
    }

    void destroy()
    {
        this.GetComponent<Transform>().gameObject.SetActive(false);
        CancelInvoke("destroy");
        //To be disabled;
    }

    public void flashErrorText(string text, float whenToStartFade, float fadeDuration)
    {
        if(errorText != null)
        {
            CancelInvoke("fadeErrorText");
            errorText.SetActive(true);
            errorText.GetComponent<Text>().text = text;
            colorPlaceHolder = errorText.GetComponent<Text>().color;
            colorPlaceHolder.a = 1;
            errorText.GetComponent<Text>().color = colorPlaceHolder;
            InvokeRepeating("fadeErrorText", whenToStartFade, fadeDuration / (255 / 6));

        }
    }

    private void fadeErrorText()
    {
        if(errorText.GetComponent<Text>().color.a>0)
        {
            colorPlaceHolder= errorText.GetComponent<Text>().color;

            colorPlaceHolder.a -= (6.0f/255.0f);

            if (colorPlaceHolder.a < 0)
            {
                colorPlaceHolder.a  = 0;
                
                CancelInvoke("fadeErrorText");

                hideErrorText();

                return;
            }

            errorText.GetComponent<Text>().color = colorPlaceHolder;

        }
    }

    public void hideErrorText()
    {
        if(errorText!=null)
        {
            errorText.SetActive(false);
        }
    }

    public void playHealingEffect()
    {
        if (!healingParticleSystem.isPlaying)
        {
            healingParticleSystem.Play();
        }
    }

    public void stopHealingEffect(bool clearParticles)
    {

        if (healingParticleSystem.isPlaying)
        {
            healingParticleSystem.Stop();

            if (clearParticles)
                healingParticleSystem.Clear();

        }
    }

    public void stopHealingEffect()
    {
        if (healingParticleSystem.isPlaying)
        {
            healingParticleSystem.Stop();
        }
    }

    public void sufferDamage(int damage, Vector3 contactPoint)
    {

        if (!this.gameObject.CompareTag(untaggedString))
        {
            if (damage <= 0 || playersCurrentHealthPoint <= 0)
            {
                return;
            }



            explosionPool.setExplosion(contactPoint);

            playersCurrentHealthPoint -= damage;

            if (playersCurrentHealthPoint <= 0)
            {
                playersCurrentHealthPoint = 0;

                isActive = false;


                //if (this.gameObject.CompareTag("Ally"))
                //{
                animator.SetBool(isDefeatedHash, true);

                if (StaticDataHandler.characterChosen == 3 || this.gameObject.CompareTag("Ally"))
                {

                    animator.applyRootMotion = false;

                    if(gun!=null)
                        gun.GetComponent<Animator>().applyRootMotion = false;

                    Debug.Log("Apply root motion? " + gun.GetComponent<Animator>().applyRootMotion);

                    if (playableGraph.IsValid())
                    {
                        playableGraph.Stop();
                    }

                    AnimationPlayableUtilities.PlayClip(gun.GetComponent<Animator>(), killerGunDefeated, out playableGraph);
                }

                Invoke("defeated", 0.8f);
                //}

            }

            healthBarObject.GetComponent<HealthBar>().updateBar(playersCurrentHealthPoint, maxHealthPoint);
        }
    }

    public void sufferDamage(int damage)
    {

        if (!this.gameObject.CompareTag(untaggedString))
        {
            if (damage <= 0 || playersCurrentHealthPoint <= 0)
            {
                return;
            }

            playersCurrentHealthPoint -= damage;

            if (playersCurrentHealthPoint <= 0)
            {
                playersCurrentHealthPoint = 0;

                isActive = false;


                //if (this.gameObject.CompareTag("Ally"))
                //{
                animator.SetBool(isDefeatedHash, true);

                if (StaticDataHandler.characterChosen == 2 || this.gameObject.CompareTag("Ally"))
                {

                    animator.applyRootMotion = false;

                    if (gun != null)
                        gun.GetComponent<Animator>().applyRootMotion = false;

                    Debug.Log("Apply root motion? " + gun.GetComponent<Animator>().applyRootMotion);

                    if (playableGraph.IsValid())
                    {
                        playableGraph.Stop();
                    }

                    AnimationPlayableUtilities.PlayClip(gun.GetComponent<Animator>(), killerGunDefeated, out playableGraph);
                }

                Invoke("defeated", 0.8f);
                //}

            }

            healthBarObject.GetComponent<HealthBar>().updateBar(playersCurrentHealthPoint, maxHealthPoint);
        }
    }

    public void defeated()
    {

        if (StaticDataHandler.characterChosen == 3 || StaticDataHandler.characterChosen == 4)
        {

            animator.applyRootMotion = true;

            if(playableGraph.IsValid())
                playableGraph.Stop();
        }

        if (this.gameObject.CompareTag("Ally") || this.gameObject.CompareTag("Objective"))
        {
            this.gameObject.transform.localPosition = Vector3.zero;

            this.gameObject.SetActive(false);

            this.transform.GetChild(2).gameObject.SetActive(false);

            this.stopHealingEffect();
        }
        else if (this.gameObject.CompareTag("Player") )
        {
            Debug.Log("Lost~");
            endGame.loseLevel();
        }

        if (animator != null && animator.isActiveAndEnabled)
            animator.SetBool(isDefeatedHash, false);
    }

    public void flipPlayer()
    {
        vector3Holder = this.transform.localScale;

        vector3Holder.x = -(vector3Holder.x);

        this.transform.localScale = vector3Holder;

        vector3Holder = healthBarPivot.transform.localScale;

        vector3Holder.x = -(vector3Holder.x);


        healthBarPivot.transform.localScale = vector3Holder;


        vector3Holder = healthBarPivot.transform.localPosition;

        vector3Holder.x = -(vector3Holder.x);

        healthBarPivot.transform.localPosition = vector3Holder;

        vector3Holder = healingParticleSystem.transform.localScale;

        vector3Holder.x = -(vector3Holder.x);

        healingParticleSystem.transform.localScale = vector3Holder;

        if (StaticDataHandler.characterChosen == 3)
        {
            if (playerDirection.x <= 0)
            {
                gun.transform.localRotation = Quaternion.Euler(0, 0, ((-(Mathf.Atan2(rightVpadButton.GetComponent<PressShoot>().InputDirection.x, rightVpadButton.GetComponent<PressShoot>().InputDirection.y)) * 180 / Mathf.PI) + 90) - 180);
            }
            else
            {
                gun.transform.localRotation = Quaternion.Euler(0, 0, -((-(Mathf.Atan2(rightVpadButton.GetComponent<PressShoot>().InputDirection.x, rightVpadButton.GetComponent<PressShoot>().InputDirection.y)) * 180 / Mathf.PI) + 90));
            }
        }

    }

    public bool isPlayerUsingProgressBar()
    {
        if (!durationBar.activeInHierarchy)
        {
            return false;
        }

        else
            return true;
    }

    public bool isInSkillState()
    {
        return this.gameObject.GetComponentInChildren<PlayerMelee>().isInSkillState();
    }

    public void enableAllCharacterInput(bool enable)
    {
        if (!enable)
        {

            if (StaticDataHandler.characterChosen == 3)
            {
                rightVpadButton.SetActive(false);
            }
            else
            {
                normalAttackButton.SetActive(false);
            }

            if (StaticDataHandler.characterChosen == 4)
            {
                skillTwoButton.SetActive(false);
            }

            skillOneButton.SetActive(false);
            vpadButton.SetActive(false);
            cooldownOneButton.SetActive(false);

            this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        }

        else
        {
            if(StaticDataHandler.characterChosen!=3)
            {
                skillOneButton.SetActive(true);
                cooldownOneButton.SetActive(true);
            }

            if (StaticDataHandler.characterChosen == 3)
            {
                rightVpadButton.SetActive(true);
            }
            else
            {
                normalAttackButton.SetActive(true);
            }

            if (StaticDataHandler.characterChosen == 4)
            {
                skillTwoButton.SetActive(true);
            }

            vpadButton.SetActive(true);
        }
    }

    public void enableAllCharacterInputExceptMovement(bool enable)
    {
        if (!enable)
        {

            if (StaticDataHandler.characterChosen == 3)
            {
                rightVpadButton.SetActive(false);
            }
            else
            {
                normalAttackButton.SetActive(false);
            }

            if (StaticDataHandler.characterChosen == 4)
            {
                skillTwoButton.SetActive(false);
            }

            skillOneButton.SetActive(false);
            cooldownOneButton.SetActive(false);

            this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        }

        else
        {
            if (StaticDataHandler.characterChosen != 3)
            {
                skillOneButton.SetActive(true);
                cooldownOneButton.SetActive(true);
            }

            if (StaticDataHandler.characterChosen == 3)
            {
                rightVpadButton.SetActive(true);
            }
            else
            {
                normalAttackButton.SetActive(true);
            }

            if (StaticDataHandler.characterChosen == 4)
            {
                skillTwoButton.SetActive(true);
            }

        }
    }

    static public int getLevelFromExp(int heroChosen, int exp)
    {
        switch(heroChosen)
        {
            case 1:
                {
                    return Mathf.FloorToInt(1.0f + 0.11f * Mathf.Sqrt(exp));
                }
            case 2:
                {
                    return Mathf.FloorToInt(1.0f + 0.09f * Mathf.Sqrt(exp));
                }
            case 3:
                {
                    return Mathf.FloorToInt(1.0f + 0.12f * Mathf.Sqrt(exp));
                }
            case 4:
                {
                    return Mathf.FloorToInt(1.0f + 0.15f * Mathf.Sqrt(exp));
                }

        }

        return Mathf.FloorToInt(1.0f + 0.3f * Mathf.Sqrt(exp));
    }

    static public float getCurrentExpPercentage(int heroChosen, int exp)
    {
        switch (heroChosen)
        {
            case 1:
                {

                    return Mathf.Repeat(0.11f * Mathf.Sqrt(exp), 1.0f);
                }
            case 2:
                {
                    return Mathf.Repeat(0.09f * Mathf.Sqrt(exp), 1.0f);
                }
            case 3:
                {
                    return Mathf.Repeat(0.12f * Mathf.Sqrt(exp), 1.0f);
                }
            case 4:
                {
                    return Mathf.Repeat(0.15f * Mathf.Sqrt(exp), 1.0f);
                }

        }

        return Mathf.Repeat(0.3f * Mathf.Sqrt(exp), 1.0f);

    }

    static public int getAttackFromLevel(int heroChosen, int level)
    {
        switch (heroChosen)
        {
            case 1:
                {

                    return 12 + (int)((level - 1)*(10.5));
                }
            case 2:
                {
                    return 7 + (int)((level - 1)*(6.5));
                }
            case 3:
                {
                    return 10 + (int)((level - 1)*(9.5));
                }
            case 4:
                {
                    return 6 + (int)((level - 1)*(4.5));
                }

        }

        return 8 + (int)((level - 1) * (7));
    }

    static public int getDefenseFromLevel(int heroChosen, int level)
    {
        switch (heroChosen)
        {
            case 1:
                {

                    return 6 + (int)((level - 1) * (6.5));
                }
            case 2:
                {
                    return 6 + (int)((level - 1) * (5.5));
                }
            case 3:
                {
                    return 5 + (int)((level - 1) * (3.5));
                }
            case 4:
                {
                    return 10 + (int)((level - 1) * (10.5));
                }

        }

        return 8 + (int)((level - 1) * (7));
    }

    static public int getHPFromLevel(int heroChosen, int level)
    {
        switch (heroChosen)
        {
            case 1:
                {

                    return 100 + (int)((level - 1) * (51.8));
                }
            case 2:
                {
                    return 165 + (int)((level - 1) * (76.9));
                }
            case 3:
                {
                    return 80 + (int)((level - 1) * (42.4));
                }
            case 4:
                {
                    return 120 + (int)((level - 1) * (62.5));
                }

        }

        return 100 + (int)((level - 1) * (50));
    }
}
