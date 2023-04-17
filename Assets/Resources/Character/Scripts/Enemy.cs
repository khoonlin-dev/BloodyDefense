using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Animations;

using UnityEngine.Playables;

public class Enemy : MonoBehaviour {

    public int maxHP;
    public int currentHP;

    private bool immuneToDashes;

    public GameObject healthBarObject;

    private GameObject player;

    private PlayerStatistics playerStats;

    private SwipePlayer playerSwipe;

    public int enemyDamage;

    public int enemySpeed;

    public int enemyShootingInterval;

    public AnimationClip dieAnimation;

    private PlayableGraph playableGraph;

    private Vector3 vector3Holder;

    public AudioSource audioSource;

    private Color spriteColor;

	// Use this for initialization
	void Start () {
        vector3Holder = new Vector3();

        currentHP = maxHP;
        immuneToDashes = false;

        //Debug.Log("Player is " + StartupConfig.player);

        player = StartupConfig.player;

        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStatistics>();
            playerSwipe = player.GetComponent<SwipePlayer>();
        }


	}

    void OnEnable()
    {
        currentHP = maxHP;
        immuneToDashes = false;


        spriteColor = this.GetComponent<SpriteRenderer>().color;
        spriteColor.a = 0;
        this.GetComponent<SpriteRenderer>().color = spriteColor;

    }
	
	// Update is called once per frame
	void Update () 
    {

		if(this.GetComponent<SpriteRenderer>().color.a < 1)
        {
            spriteColor = this.GetComponent<SpriteRenderer>().color;
            spriteColor.a+=1.0f*Time.deltaTime;
            if (spriteColor.a > 1)
            {
                spriteColor.a = 1;
            }
            this.GetComponent<SpriteRenderer>().color = spriteColor;
        }
	}

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.CompareTag("Bullet"))
        {
            //Debug.Log("Oops...Collision entered!");
            sufferDamage(playerStats.bulletDamage, true);

            return;
        }

        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Neutrophil's Sword"))
        {

            

            if (col.gameObject.GetComponent<SwipePlayer>().isDashing && !immuneToDashes)
            {
                //Debug.Log("I have collided with a player! IsDashing is " + col.gameObject.GetComponent<SwipePlayer>().isDashing + " and immuneToDashes is " + immuneToDashes);
                sufferDamage(playerSwipe.dashDamage, false);
                immuneToDashes = true;
                Invoke("cancelDashImmunity", 0.5f);
                return;
            }


        }

        //or when player uses skill or when player is in dashing state
    }

    private void cancelDashImmunity()
    {
        immuneToDashes = false;
        CancelInvoke("cancelDashImmunity");
        return;
    }

    public void sufferDamage(int damage, bool isBullet)
    {


        if (damage <= 0 || currentHP <= 0)
        {
            return;
        }

        vector3Holder = this.transform.position;

        vector3Holder.z -= 10.0f;

        StartupConfig.explosion.setExplosion(vector3Holder);

        currentHP-=damage;

        if (currentHP <= 0)
        {
            currentHP = 0;

            if (playableGraph.IsValid())
            {
                playableGraph.Stop();
            }

            AnimationPlayableUtilities.PlayClip(this.gameObject.GetComponent<Animator>(), dieAnimation, out playableGraph);

            Invoke("destroy", 0.45f);
        }

        healthBarObject.GetComponent<HealthBar>().updateBar(currentHP, maxHP);

        if (!isBullet)
        {
            switch (StaticDataHandler.characterChosen)
            {
                case 1: //Case neutrophil
                    {

                        GameLevelAudioManager.playSound(GameLevelAudioManager.audio.slashed, Vector2.Distance(this.transform.position, StartupConfig.player.transform.position));
                        break;
                    }
                case 2: //Case macrophage
                    {
                        GameLevelAudioManager.playSound(GameLevelAudioManager.audio.munching, Vector2.Distance(this.transform.position, StartupConfig.player.transform.position));
                        break;
                    }
                case 4: //Case helper
                    {
                        GameLevelAudioManager.playSound(GameLevelAudioManager.audio.punched, Vector2.Distance(this.transform.position, StartupConfig.player.transform.position));
                        break;
                    }
            }
        }
        //Debug.Log("I have taken " + damage + " damage. My HP is now " + currentHP);
    }

    void destroy()
    {
        this.GetComponent<Transform>().gameObject.SetActive(false);
        CancelInvoke("destroy");
        //To be disabled;
    }

  
}
