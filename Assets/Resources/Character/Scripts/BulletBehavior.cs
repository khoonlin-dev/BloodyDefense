using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {



    private string objectiveTag = "Objective";

    private ExplosionPoolObject explosion;

    private GameObject player;

    private PlayerStatistics playerStats;

    public Enemy enemyStats;

    private Vector2 playerDirection;

    public float bulletLifeTime = 3f;

    public float bulletSpeed = 5f;

    public GameObject bulletPool;

    private Vector3 vector3Holder;

    private string bulletString = "Bullet";

    void OnEnable()
    {

        if (this.gameObject.CompareTag(bulletString))
        {
            //transform.position = playerStats.playerPosition;


            //Debug.Log(player);

            playerStats = player.GetComponent<PlayerStatistics>();

            vector3Holder.x = playerStats.playerPosition.x;
            vector3Holder.y = playerStats.playerPosition.y - 0.2f;
            vector3Holder.z = -10.0f;

            transform.position = vector3Holder;

            GameLevelAudioManager.playSound(GameLevelAudioManager.audio.shoot,Vector2.Distance(this.transform.position, StartupConfig.player.transform.position));

            //transform.rotation = Quaternion.Euler(0, 0, (-(Mathf.Atan2(playerStats.playerDirection.x, playerStats.playerDirection.y)) * 180 / Mathf.PI) + 90);
            //Debug.Log("Rotation: " + ( (-(Mathf.Atan2(playerStats.playerDirection.x, playerStats.playerDirection.y)) * 180 / Mathf.PI) + 90) + " degree.");

            vector3Holder = Vector3.zero;

            if (player.CompareTag(objectiveTag))
            {
                vector3Holder.x = 6.0f;
            }
            else
                vector3Holder.x = 2.0f;

            transform.Translate(vector3Holder);

            Invoke("Destroy", bulletLifeTime);
            playerDirection = playerStats.playerDirection;

        }
        else
        {

            enemyStats = player.GetComponent<Enemy>();


            transform.position = player.transform.position;

            Invoke("Destroy", bulletLifeTime);
        }
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

	// Use this for initialization
	void Start () 
    {
        explosion = StartupConfig.explosion;

        vector3Holder = new Vector3();

        player = StartupConfig.player;

        if(player!=null)
        {
            playerStats = player.GetComponent<PlayerStatistics>();
        }
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        explosion.setExplosion(this.transform.position);
        CancelInvoke();

        if (this.gameObject.CompareTag(bulletString))
        {
            GameLevelAudioManager.playSound(GameLevelAudioManager.audio.shotByAntibody, Vector2.Distance(this.transform.position, StartupConfig.player.transform.position));
        }
        else
        {
            GameLevelAudioManager.playSound(GameLevelAudioManager.audio.shotByAcid, Vector2.Distance(this.transform.position, StartupConfig.player.transform.position));

        }

        Destroy();



    }

	
	// Update is called once per frame
	void Update () {

        //this.GetComponent<Rigidbody2D>().AddForce(player.playerDirection * bulletSpeed);

        //Debug.Log(-playerDirection * bulletSpeed);

        //Debug.Log("Player Direction = " + playerDirection.x + ", " + playerDirection.y + " Bullet Translation: " + (-playerDirection) * bulletSpeed);

        transform.Translate(bulletSpeed * Time.deltaTime, 0, 0);

	}

    public void setShootingDirection(Vector3 direction)
    {
        this.transform.rotation = Quaternion.Euler(0, 0, (-(Mathf.Atan2(direction.x, direction.y)) * 180 / Mathf.PI) + 90);
    }

    public void setShooterCharacter(GameObject shootingCharacter)
    {
        //set the character who shoots the bullet (might not be the player)
        player = shootingCharacter;

        if (this.gameObject.CompareTag(bulletString))
            playerStats = shootingCharacter.GetComponent<PlayerStatistics>();
        else
            enemyStats = shootingCharacter.GetComponent<Enemy>();

    }
}
