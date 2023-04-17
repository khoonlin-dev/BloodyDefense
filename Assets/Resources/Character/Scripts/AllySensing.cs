using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.Animations;

using UnityEngine.Playables;




public class AllySensing : MonoBehaviour
{

    private string allyTag = "Ally";

    private string objectiveTag = "Objective";

    private GameObject bulletSpawned;

    public bool debug;

    private PlayerStatistics playerStats;

    private List<GameObject> attackTargets = new List<GameObject>();

    public enum allyState
    {
        Idle, Shooting
    }

    private allyState state;

    public GameObject gun;

    private int isShootingHash;

    //private PlayableGraph playableGraph;

    //public AnimationClip shootAnimation;

    //public Animation shootAnim;

    private Animator animator;

    private Vector3 vector3Container;

    public GameObject gunTransform;

    public AnimationClip shootingSpawn;

    private PlayableGraph playableGraph;

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if(col.CompareTag("Enemy"))
        {
            if(!attackTargets.Contains(col.gameObject))
            {
                attackTargets.Add(col.gameObject);
                if (state != allyState.Shooting)
                {
                    startAttack();
                }
            }
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            if (attackTargets.Contains(col.gameObject))
            {
                attackTargets.Remove(col.gameObject);

                if (attackTargets.Count == 0)
                {
                    stopAttack();
                }
            }
        }
    }

    private bool isShootLocked = true;

	// Use this for initialization
	void Start ()
    {
        playerStats = GetComponentInParent<PlayerStatistics>();

        animator = gun.GetComponent<Animator>();

        isShootingHash = Animator.StringToHash("isShooting");

        vector3Container = new Vector3();
	}
	
    void OnDisable()
    {
        isShootLocked = true;
        stopAttack();
    }

    void Awake()
    {
        playerStats = GetComponentInParent<PlayerStatistics>();

        animator = gun.GetComponent<Animator>();

        isShootingHash = Animator.StringToHash("isShooting");

        vector3Container = new Vector3();
    }

    void OnEnable()
    {

        animator.applyRootMotion = false;

        if (playableGraph.IsValid())
        {
            playableGraph.Stop();
        }

        if (shootingSpawn!=null)
            AnimationPlayableUtilities.PlayClip(gun.GetComponent<Animator>(), shootingSpawn, out playableGraph);

        Invoke("unlockShoot", 2.5f);
    }

    void OnDestroy()
    {
        if(playableGraph.IsValid())
            playableGraph.Destroy();
    }

	// Update is called once per frame
	void Update ()
    {
	}

    void startAttack()
    {

            CancelInvoke("shoot");

            state = allyState.Shooting;

            if (playerStats == null)
            {
                playerStats = GetComponentInParent<PlayerStatistics>();
            }

            InvokeRepeating("shoot", 1.0f, playerStats.bulletFiringInterval);
    }

    void shoot()
    {
        //Debug.Log("Started attack");

        if (!isShootLocked && playerStats.isActive)
        {
            CancelInvoke("stopShootAnimation");

            if (debug)
                Debug.Log("I am shooting");

            if (StartupConfig.bulletPool == null)
            {
                StartupConfig.bulletPool = GameObject.Find("BulletPool").GetComponent<BulletPoolBehavior>();
            }

            playerStats.playerDirection = (attackTargets[0].transform.position - this.gameObject.transform.position).normalized;

            if ((playerStats.playerDirection.x <= 0 && playerStats.gameObject.transform.localScale.x < 0) || (playerStats.playerDirection.x > 0 && playerStats.gameObject.transform.localScale.x > 0))
            {
                playerStats.flipPlayer();

            }

            if (transform.root.gameObject.CompareTag(allyTag))
            {
                vector3Container.x = 0.0f;
                vector3Container.y = -0.3f;
                vector3Container.z = -1.0f;

                gun.transform.localPosition = vector3Container;

            }

            if (playerStats.playerDirection.x <= 0)
            {
                gun.transform.localRotation = Quaternion.Euler(0, 0, ((-(Mathf.Atan2(playerStats.playerDirection.x, playerStats.playerDirection.y)) * 180 / Mathf.PI) + 90) - 180);
            }
            else
            {
                gun.transform.localRotation = Quaternion.Euler(0, 0, -((-(Mathf.Atan2(playerStats.playerDirection.x, playerStats.playerDirection.y)) * 180 / Mathf.PI) + 90));
            }

            if (debug)
                Debug.Log("I am shooting " + attackTargets[0] + " at " + playerStats.playerDirection);

            bulletSpawned = StartupConfig.bulletPool.getPooledObject(this.transform.root.gameObject, (attackTargets[0].transform.position - this.gameObject.transform.position).normalized);

            animator.SetBool(isShootingHash, true);

            Invoke("stopShootAnimation", 0.01f);
        }
    }

    void stopShootAnimation()
    {
        animator.SetBool(isShootingHash, false);

    }

    void stopAttack()
    {
        CancelInvoke("shoot");

        stopShootAnimation();

        if (transform.root.gameObject.CompareTag(allyTag))
        {
            vector3Container.x = -0.6f;
            vector3Container.y = -0.6f;
            vector3Container.z = -1.0f;

            gun.transform.localPosition = vector3Container;

            gun.transform.localRotation = Quaternion.Euler(0, 0, -45);
        }
        state = allyState.Idle;
    }

    void unlockShoot()
    {

        isShootLocked = false;

        if (playableGraph.IsValid())
        {
            playableGraph.Stop();
            animator.applyRootMotion = true;
        }
        playerStats.isActive = true;
    }
}
