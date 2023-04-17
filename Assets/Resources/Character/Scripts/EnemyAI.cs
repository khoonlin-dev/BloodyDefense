using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    using UnityEngine.Animations;

    using UnityEngine.Playables;

[RequireComponent(typeof(Collider2D))]

public class EnemyAI : MonoBehaviour {

    public enum enemyState
    {
        Idle,
        Patrolling,
        Chasing,
        Shooting,
        Attacking
    };

    public enum AttackMode
    {
        None, Melee, Shooting
    }

    public AttackMode enemyAttackMode;

    public enum AttackPriority
    {
        First, Last, Ally, Player, Objective
    }

    public AttackPriority enemyAttackPriority;

    public enum AIMode
    {
        Patrol, IdleOnTheSpot, IdleAtPoint, Chase, ChaseObjective, ChasePoint
    }

    public AIMode enemyAIMode;

    public Vector3 patrolStartPoint;

    public Vector3 patrolEndPoint;

    public float patrolWaitTime;

    public Vector3 chasePoint;

    public Vector3 idlePoint;

    //public GameObject chaseObject;

    [Range(1, 5)]
    public int aggressiveness = 1;


    [System.NonSerialized]
    public int minAttackInterval;

    [System.NonSerialized]
    public int maxAttackInterval;

    private int state;

    private bool isWaitingAtPatrolPoint = false;

    //private Collider2D sensingBox;

    //private Collider2D attackingBox;

    [System.NonSerialized]
    public List<GameObject> chaseTargets = new List<GameObject>();

    public List<Vector2> chasePointList = new List<Vector2>();

    [System.NonSerialized]
    public List<Vector2> destinationList = new List<Vector2>();

    [System.NonSerialized]
    public List<GameObject> objectsInMeleeBox = new List<GameObject>();

    [System.NonSerialized]
    public List<GameObject> objectsInShootingBox = new List<GameObject>();

    private Vector2 currentDestination;

    private Vector2 targetLastPosition;

    private Enemy enemyScript;

    public bool debug = false;

    public Animator animator;

    public AnimationClip attackAnimation;

    [System.NonSerialized]
    public PlayableGraph playableGraph;

    [System.NonSerialized]
    public int isWalkingHash;

    private static Vector3 vector3Container = new Vector3();

    private RaycastHit2D hitFacedDirection;

    private bool moveLock;

    private int layerMask;

    private string wallTag = "Wall";

    public GameObject healthBarPivot;

	// Use this for initialization

    void Awake()
    {

        isWalkingHash = Animator.StringToHash("isWalking");

        if (enemyAIMode == AIMode.IdleOnTheSpot)
        {
            idlePoint = this.transform.position;
        }

        if(enemyAIMode==AIMode.ChaseObjective)
        {
            enemyAttackPriority = AttackPriority.Objective;
        }

        if(enemyAIMode==AIMode.ChasePoint && chasePointList.Count!=0)
        {
            this.destinationList = chasePointList;
        }

   
    }

	void Start () 
    {

        layerMask = LayerMask.GetMask("Wall");


        enemyScript = this.gameObject.GetComponent<Enemy>();
        
        switch (aggressiveness)
        {
            case 1:
                {
                    minAttackInterval = 10;
                    maxAttackInterval = 15;
                    break;
                }
            case 2:
                {
                    minAttackInterval = 8;
                    maxAttackInterval = 12;
                    break;
                }
            case 3:
                {
                    minAttackInterval = 6;
                    maxAttackInterval = 10;
                    break;
                }
            case 4:
                {
                    minAttackInterval = 4;
                    maxAttackInterval = 7;
                    break;
                }
            case 5:
                {
                    minAttackInterval = 1;
                    maxAttackInterval = 3;
                    break;
                }
        }
        //sensingBox = this.gameObject.GetComponent<Collider2D>();

        //attackingBox = this.transform.Find("Melee Box").GetComponent<Collider2D>();
	}

    void OnEnable()
    {
        //state = (int)enemyState.Idle;

        animator.SetBool(isWalkingHash, false);

        if(enemyAIMode==AIMode.Patrol)
        {
            startPatrol(true, patrolStartPoint, patrolEndPoint);
        }

        else if (enemyAIMode==AIMode.IdleAtPoint || enemyAIMode==AIMode.IdleOnTheSpot)
        {
            if(this.transform.position!=idlePoint)
            {
                if (debug)
                    Debug.Log("Idle point is " + idlePoint);
                startPatrol(false, idlePoint);

            }
        }

        else if (enemyAIMode == AIMode.Chase || enemyAIMode == AIMode.ChaseObjective)
        {

            if(enemyAttackPriority!=AttackPriority.Objective)
            {
                StartupConfig.enemyChasingCharacter.Add(this.gameObject);
            }
            else
            {
                StartupConfig.enemyChasingObjective.Add(this.gameObject);
            }

              if(chaseTargets.Count==0)
              {
                  startChasing(findNewTarget());
              }
            
        }
    }
	
    public void resetAI(Vector3 idlePoint)
    {
        enemyAIMode = AIMode.IdleAtPoint;

        if (this.transform.position != idlePoint)
        {
            if (debug)
                Debug.Log("Idle point is " + idlePoint);
            startPatrol(false, idlePoint);

        }

    }

    void OnDisable()
    {

        destinationList.Clear();

        chaseTargets.Clear();

        startIdle();

        if (enemyAttackPriority != AttackPriority.Objective)
        {
            StartupConfig.enemyChasingCharacter.Remove(this.gameObject);
        }
        else
        {
            StartupConfig.enemyChasingObjective.Remove(this.gameObject);
        }

    }

    void OnDestroy()
    {
        if(playableGraph.IsValid())
            playableGraph.Destroy();
    }

    void FixedUpdate()
    {

        if (state == (int)enemyState.Chasing || state == (int)enemyState.Patrolling)
        {
            moveLock = false;


            

                if (state == (int)enemyState.Chasing && chaseTargets.Count > 0)
                    vector3Container = chaseTargets[0].transform.position - this.transform.position;
                else if (state == (int)enemyState.Patrolling && destinationList.Count>0)
                    vector3Container = (Vector3)destinationList[0] - this.transform.position;

                hitFacedDirection = Physics2D.Raycast(transform.position, vector3Container, 1.0f, layerMask);

                if (hitFacedDirection.collider != null && hitFacedDirection.collider.CompareTag(wallTag))
                {

                    moveLock = true;

                }


        }
    }


	// Update is called once per frame
	void Update () 
    {
        switch (state)
        {
            case (int)enemyState.Patrolling:
                {
                    if (this.transform.position != (Vector3)currentDestination) //If enemy haven't reach point
                    {

                        if ((((Vector3)currentDestination - this.transform.position).normalized.x > 0 && this.transform.localScale.x < 0) || (((Vector3)currentDestination - this.transform.position).normalized.x < 0 && this.transform.localScale.x >= 0))
                        {
                            flipEnemy();
                        }



                        if(!moveLock)
                            this.transform.position = Vector3.MoveTowards(this.transform.position, (Vector3)currentDestination, this.gameObject.GetComponent<Enemy>().enemySpeed * Time.deltaTime);

                        
                    }
                    else //If enemy reaches point
                    {
                        if (destinationList.Count > 1)
                        {

                            if (!isWaitingAtPatrolPoint)
                            {
                                isWaitingAtPatrolPoint = true;
                                Invoke("continuePatrolAfterWaiting", patrolWaitTime);
                            }
                        }
                        else
                        {
                            destinationList.Clear();
                            startIdle();
                        }

                      
                    }
                    break;
                }
            case (int)enemyState.Chasing:
                {
                    if(hasTargetMoved())
                    {
                        calculateAttackPosition();
                    }


                    if (this.transform.position != (Vector3)currentDestination) //If enemy haven't reach point
                    {
                        if ((((Vector3)currentDestination - this.transform.position).normalized.x > 0 && this.transform.localScale.x < 0) || (((Vector3)currentDestination - this.transform.position).normalized.x < 0 && this.transform.localScale.x >= 0))
                        {
                            flipEnemy();
                        }

                        if (!moveLock)
                
                            this.transform.position = Vector3.MoveTowards(this.transform.position, (Vector3)currentDestination, this.gameObject.GetComponent<Enemy>().enemySpeed * Time.deltaTime);
                    }

                    //No need to calculate when to stop this state here. If the attack box is triggered by player, this state will stop.

                    break;
                }
            case (int)enemyState.Attacking:
                {
                    if (hasTargetMoved())
                    {
                        calculateAttackPosition();
                    }

                    break;
                }
        }


	}

    void calculateAttackPosition()
    {
        targetLastPosition = (Vector2)chaseTargets[0].transform.position;

        currentDestination = targetLastPosition;

        if (debug)
            Debug.Log("Current destination: " + currentDestination);

    }

    public void startIdle()
    {

        if (debug)
            Debug.Log("I've started to idle");


        animator.SetBool(isWalkingHash, false);

        state = (int)enemyState.Idle;
    }

    public void stopIdle()
    {
        if (debug)
            Debug.Log("I've stop iddling");
    }

    public void pausePatrol()
    {

        if (debug)
            Debug.Log("I've stopped patrol");
    }

    public void startPatrol(bool loop, Vector2 position)
    {

        animator.SetBool(isWalkingHash, true);

        destinationList.Clear();
        if (loop)
            destinationList.Add(this.transform.position);
        destinationList.Add(position);

        state = (int)enemyState.Patrolling;

        currentDestination = position;

        if (debug)
        {
            Debug.Log("I've started to patrol to " + currentDestination);
            Debug.Log("I've started to patrol to " + position);
        }
    }

    public void startPatrol(bool loop, Vector2 startPosition, Vector2 endPosition)
    {
        state = (int)enemyState.Patrolling;
        destinationList.Add(startPosition);
        destinationList.Add(endPosition);
        currentDestination = destinationList[0];
    }

    public void continuePatrol()
    {

        animator.SetBool(isWalkingHash, true);

        state = (int)enemyState.Patrolling;
        currentDestination = destinationList[0];

        if (debug)
            Debug.Log("I've continued patrol");
    }

    void continuePatrolAfterWaiting()
    {
        if (state == (int)enemyState.Patrolling)
        {
            if (destinationList.Count > 1) //If looping
            {

                animator.SetBool(isWalkingHash, true);

                destinationList.Add(destinationList[0]);
                destinationList.RemoveAt(0);


                currentDestination = destinationList[0];


                isWaitingAtPatrolPoint = false;
            }
            else //If NOT looping
            {
                pausePatrol();
                startIdle();
            }
        }
    }

    public void startChasing(GameObject target)
    {


        if (target != null)
        {

            if(!chaseTargets.Contains(target))
                chaseTargets.Add(target);

            switch (state)
            {
                case (int)enemyState.Idle:
                    {
                        stopIdle();
                        break;
                    }
                case (int)enemyState.Patrolling:
                    {
                        pausePatrol();
                        break;
                    }
            }

            calculateAttackPosition();

            if (enemyAttackMode != AttackMode.Shooting)
            {

                if (!objectsInMeleeBox.Contains(target))
                {

                    animator.SetBool(isWalkingHash, true);


                    state = (int)enemyState.Chasing;

                    if (debug)
                        Debug.Log("I have located " + target + " i will now pursue him!");

                }
                else
                {

                    animator.SetBool(isWalkingHash, false);

                    startAttackState();
                    this.gameObject.GetComponentInChildren<EnemyMelee>().startAttacking();

                    if (debug)
                        Debug.Log("I have located " + target + " which is in my box! i will now attack him!");
                }
            }
            else
            {
                if(!objectsInShootingBox.Contains(target))
                {

                    animator.SetBool(isWalkingHash, true);


                    state = (int)enemyState.Chasing;

                    if (debug)
                        Debug.Log("I have located " + target + " i will now pursue him!");
                }
                else
                {
                    startShoot();

                    if (debug)
                        Debug.Log("I have located " + target + " which is in my box! i will now shoot him!");
                }
            }
        }

        else
        {
            startIdle();
        }
    }

    public void stopChasing()
    {
        if (debug)
            Debug.Log("I've stop chasing");
    }

    public void startAttackState()
    {
        if (debug)
            Debug.Log("Starting attack state");
        state = (int)enemyState.Attacking;
    }

    public void continueChasing()
    {

        if (debug)
            Debug.Log("I will continue to chase");


        animator.SetBool(isWalkingHash, true);


        state = (int)enemyState.Chasing;

        calculateAttackPosition();
    }

    public void stopAttackState()
    {
        if (debug)
            Debug.Log("Stopping attack state");
    }

    public int getState()
    {
        return state;
    }

    bool hasTargetMoved()
    {
        if ((Vector2)chaseTargets[0].transform.position != targetLastPosition)
        {
            return true;
        }
        else
            return false;
    }

    public static void shuffleGameObjectList(List<GameObject> list)
    {

        //This is called Fisher-Yates shuffle - found on stack overflow :p 
        //Currently not implemented

        System.Random rng = new System.Random();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject value = list[k];
            list[k] = list[n];
            list[n] = value;
        }  
    }

    public GameObject findNewTarget()
    {
        if (enemyAttackPriority == AttackPriority.Ally)
        {
            //Return closest ALLY in map if there is any ally. Else, return player.
            if (StartupConfig.activeAllyOnMap.Count > 0)
            {
                GameObject closest = null;
                float distance = Mathf.Infinity;
                Vector3 position = this.transform.position;
                foreach (GameObject go in StartupConfig.activeAllyOnMap)
                {
                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        closest = go;
                        distance = curDistance;
                    }
                }
                return closest;
            }
            else if (StartupConfig.player!=null && StartupConfig.player.activeInHierarchy)
            {
                return StartupConfig.player;
            }
            else
                return null;
        }
        else if (enemyAttackPriority == AttackPriority.Player)
        {   //Return player
            if (StartupConfig.player!=null)
                return StartupConfig.player;
            else
                return null;
        }

        else if (enemyAttackPriority == AttackPriority.First)
        {   //Return closest character 
            if (StartupConfig.allGoodGuysOnMap.Count > 0)
            {
                GameObject closest = null;
                float distance = Mathf.Infinity;
                Vector3 position = this.transform.position;
                foreach (GameObject go in StartupConfig.allGoodGuysOnMap)
                {
                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        closest = go;
                        distance = curDistance;
                    }
                }
                return closest;
            }
            else
                return null;
        }
        else if (enemyAttackPriority == AttackPriority.Objective)
        {   //Return closest objective
            if (StartupConfig.allObjectiveObject.Count > 0)
            {

                if (debug)
                    Debug.Log("We are looking for the closest objective from " + StartupConfig.allObjectiveObject.Count + " objectives");

                GameObject closest = null;
                float distance = Mathf.Infinity;
                Vector3 position = this.transform.position;
                foreach (GameObject go in StartupConfig.allObjectiveObject)
                {
                    Vector3 diff = go.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        closest = go;
                        distance = curDistance;
                    }
                }

                if (debug)
                    Debug.Log("The closest objective is " + closest);

                return closest;
            }
            else if (enemyAIMode==AIMode.ChaseObjective)
            {
                return null;
            }
            else if (StartupConfig.player.activeInHierarchy)
            {
                return StartupConfig.player;
            }
            else
                return null;
        }
        else
            return null;
    }

    public void newAllyInstantiated(GameObject newObject)      //To be called by each ally's onenable function
    {

        if (debug)
            Debug.Log("new ally instantiated!!");

        if (enemyAttackPriority == AttackPriority.Ally)
        {

            if (chaseTargets.Count == 0 || chaseTargets[0].CompareTag("Player") || chaseTargets[0].CompareTag("Objective"))   //If currently not pursuing a target (highly unlikely)
                {
                    if (debug)
                        Debug.Log("I have located an ally i will chase him!");

                    chaseTargets.Clear();

                    startChasing(newObject);
                }
                else
                {
                    chaseTargets.Clear();

                    startChasing(findNewTarget());  //Find closer ally to attack
                }
            
            
        }

        else if (enemyAttackPriority == AttackPriority.Player||enemyAttackPriority==AttackPriority.Objective)
        {

                if (chaseTargets.Count == 0)   //If currently not pursuing a target (highly unlikely)
                {
                    chaseTargets.Clear();

                    startChasing(newObject);
                }
            
        }
        else if (enemyAttackPriority == AttackPriority.First)
        {
            chaseTargets.Clear();

            startChasing(findNewTarget());    //Recalculate to find closest character regardless of type
        }

    }
    
    public void newPlayerInstantiated(GameObject newObject)     //To be called by each player's onenable function
    {
        if (enemyAttackPriority == AttackPriority.Ally || enemyAttackPriority == AttackPriority.Objective)
        {

            if (chaseTargets.Count == 0)   //If currently not pursuing a target (highly unlikely) 
            {
                chaseTargets.Clear();

                startChasing(newObject);
            }

        }

        else if (enemyAttackPriority == AttackPriority.Player)
        {

            if (chaseTargets.Count == 0 || chaseTargets[0].CompareTag("Ally") || chaseTargets[0].CompareTag("Objective"))   //If currently not pursuing a target (highly unlikely) or currently pursuing target that is not prioritized
            {
                chaseTargets.Clear();

                startChasing(newObject);
            }
            else
            {
                chaseTargets.Clear();

                startChasing(findNewTarget());  //Find closer player to attack
            }

        }
        else if (enemyAttackPriority == AttackPriority.First)
        {
            chaseTargets.Clear();

            startChasing(findNewTarget());    //Recalculate to find closest character regardless of type
        }
    }

    public void newObjectiveInstantiated(GameObject newObject)
    {
        if (enemyAttackPriority == AttackPriority.Ally || enemyAttackPriority == AttackPriority.Player)
        {

            if (chaseTargets.Count == 0)   //If currently not pursuing a target (highly unlikely) 
            {
                chaseTargets.Clear();

                startChasing(newObject);
            }

        }

        else if (enemyAttackPriority == AttackPriority.Objective)
        {

            if (chaseTargets.Count == 0 || chaseTargets[0].CompareTag("Ally") || chaseTargets[0].CompareTag("Player"))   //If currently not pursuing a target (highly unlikely) or currently pursuing target that is not prioritized
            {
                chaseTargets.Clear();

                startChasing(newObject);
            }
            else
            {
                chaseTargets.Clear();

                startChasing(findNewTarget());  //Find closer player to attack
            }

        }
        else if (enemyAttackPriority == AttackPriority.First)
        {
            chaseTargets.Clear();

            startChasing(findNewTarget());    //Recalculate to find closest character regardless of type
        }
    }

    public void startShoot()
    {
        if (chaseTargets.Count > 0)
        {
            if (((chaseTargets[0].transform.position - this.transform.position).normalized.x > 0 && this.transform.localScale.x < 0) || ((chaseTargets[0].transform.position - this.transform.position).normalized.x < 0 && this.transform.localScale.x >= 0))
            {
                vector3Container = this.transform.localScale;
                vector3Container.x = -(vector3Container.x);
                this.transform.localScale = vector3Container;
            }
        }

        state = (int)enemyState.Shooting;

        animator.SetBool(isWalkingHash, false);

        CancelInvoke("shoot");

        InvokeRepeating("shoot", 1.0f, enemyScript.enemyShootingInterval);
    }

    public void startShoot(GameObject gameObject)
    {
        state = (int)enemyState.Shooting;

        chaseTargets.Insert(0, gameObject);


        CancelInvoke("shoot");


        InvokeRepeating("shoot", 1.0f, enemyScript.enemyShootingInterval);
    }

    public void stopShoot()
    {
        CancelInvoke("shoot");


        if (debug)
            Debug.Log("I will stop shooting now");

        switch (enemyAIMode)
        {
            case AIMode.IdleAtPoint:
            case AIMode.IdleOnTheSpot:
                {
                    if (this.gameObject.transform.position != idlePoint)
                    {
                        continueChasePoint();

                        if (debug)
                            Debug.Log("I will continue to my idle point at " + idlePoint + " now");

                    }
                    else
                    {
                        if (debug)
                            Debug.Log("I will start idle now");
                        startIdle();
                    }
                    break;
                }
            case AIMode.Chase:
            case AIMode.ChaseObjective:
                {
                    if (debug)
                        Debug.Log("I will start idle now");
                    startIdle();
                    break;
                }
            case AIMode.Patrol:
                {
                    if (debug)
                        Debug.Log("I will continue patrol now");
                    continuePatrol();
                    break;
                }
            case AIMode.ChasePoint:
                {
                    if (debug)
                        Debug.Log("I will continue chase point now");
                    continueChasePoint();
                    break;
                }

        }
    }

    void shoot()
    {
        if(chaseTargets.Count==0)
        {
            stopShoot();
            return;
        }

        if(!chaseTargets[0].activeInHierarchy)
        {
            chaseTargets.Remove(chaseTargets[0]);

            if (chaseTargets.Count==0)
            {
                stopShoot();
                return;
            }
        }

        if (chaseTargets.Count > 0)
        {
            if (((chaseTargets[0].transform.position - this.transform.position).normalized.x > 0 && this.transform.localScale.x < 0) || ((chaseTargets[0].transform.position - this.transform.position).normalized.x < 0 && this.transform.localScale.x >= 0))
            {
                flipEnemy();
            }
        }

        if (StartupConfig.enemyBulletPool==null)
        {

            StartupConfig.enemyBulletPool = GameObject.Find("EnemyBulletPool").GetComponent<BulletPoolBehavior>();
        }

        StartupConfig.enemyBulletPool.getPooledObject(this.gameObject, (chaseTargets[0].transform.position - this.gameObject.transform.position).normalized);


        //Debug.Log("I opened fire at " + chaseTargets[0] + " at direction " + (chaseTargets[0].transform.position - this.gameObject.transform.position).normalized);
    }

    public void continueChasePoint()
    {
        if (destinationList.Count > 0)
        {
            state = (int)enemyState.Patrolling;
            currentDestination = destinationList[0];
            if (debug)
                Debug.Log("I've continued patrol");
        }
        else
        {
            startIdle();
        }
    }

    public void anCharacterIsDestroyed(GameObject gameObject)
    {
        if (state == (int)enemyState.Chasing || state == (int)enemyState.Attacking || state == (int)enemyState.Shooting)
        {
            if (chaseTargets.Count > 0 && chaseTargets[0] == gameObject)
            {

                chaseTargets.Remove(gameObject);

                //If kena your gameobject

                switch (enemyAIMode)
                {
                    case AIMode.IdleAtPoint:
                    case AIMode.IdleOnTheSpot:
                        {

                            if (chaseTargets.Count > 0)
                            {
                                if (state != (int)enemyState.Shooting)
                                {
                                    if (objectsInMeleeBox.Contains(chaseTargets[0]))
                                    {
                                        this.gameObject.GetComponentInChildren<EnemyMelee>().startAttacking();
                                    }
                                    else
                                    {
                                        continueChasing();
                                    }
                                }
                                else
                                {
                                    startShoot();
                                }
                            }

                            else
                            {
                                if (this.gameObject.transform.position != idlePoint)
                                {
                                    continueChasePoint();

                                }
                                else
                                    startIdle();
                            }
                            break;
                        }
                    case AIMode.Chase:
                    case AIMode.ChaseObjective:
                        {
                            startChasing(findNewTarget());
                            break;
                        }
                    case AIMode.Patrol:
                        {
                            if (chaseTargets.Count > 0)
                            {
                                if (state != (int)enemyState.Shooting)
                                {
                                    if (objectsInMeleeBox.Contains(chaseTargets[0]))
                                    {
                                        this.gameObject.GetComponentInChildren<EnemyMelee>().startAttacking();
                                    }
                                    else
                                    {
                                        continueChasing();
                                    }
                                }
                                else
                                {
                                    startShoot();
                                }
                            }
                            else
                            {
                                continuePatrol();
                            }
                            break;
                        }
                    case AIMode.ChasePoint:
                        {
                            if (chaseTargets.Count > 0)
                            {
                                if (state != (int)enemyState.Shooting)
                                {
                                    if (objectsInMeleeBox.Contains(chaseTargets[0]))
                                    {
                                        this.gameObject.GetComponentInChildren<EnemyMelee>().startAttacking();
                                    }
                                    else
                                    {
                                        continueChasing();
                                    }
                                }
                                else
                                {
                                    startShoot();
                                }
                            }
                            else
                                continueChasePoint();
                            break;
                        }
                    //Recalculate or Stop activity
                }
            }
            else if (chaseTargets.Contains(gameObject))
            {
                chaseTargets.Remove(gameObject);
            }
        }
    }

    public int findNearestChasePoint()
    {
        Vector3 closest = chasePointList[0];
        Vector3 position = this.transform.position;
        float distance = Mathf.Infinity;
        foreach (Vector3 chasePoint in chasePointList)
        {
            Vector3 diff = chasePoint - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = chasePoint;
                distance = curDistance;
            }
        }
        return chasePointList.IndexOf(closest);
    }

    public void flipEnemy()
    {

        vector3Container = this.transform.localScale;
        vector3Container.x = -(vector3Container.x);
        this.transform.localScale = vector3Container;


        if (this.transform.localScale.x > 0)
        {

            vector3Container = healthBarPivot.transform.localScale;

            if(vector3Container.x < 0)
            {
                vector3Container.x = -(vector3Container.x);

                healthBarPivot.transform.localScale = vector3Container;

                vector3Container = healthBarPivot.transform.localPosition;

                if (vector3Container.x > 0)
                {
                    vector3Container.x = -(vector3Container.x);

                }

                healthBarPivot.transform.localPosition = vector3Container;

                
            }
        }
        else if (this.transform.localScale.x <= 0 )
        {

            vector3Container = healthBarPivot.transform.localScale;

            if (vector3Container.x > 0)
            {
                vector3Container.x = -(vector3Container.x);

                healthBarPivot.transform.localScale = vector3Container;

                vector3Container = healthBarPivot.transform.localPosition;

                if (vector3Container.x < 0)
                {
                    vector3Container.x = -(vector3Container.x);

                }

                healthBarPivot.transform.localPosition = vector3Container;
            }

            //Debug.Log("Transform Scale X: " + this.transform.localScale.x + " Health bar pos x: " + healthBarPivot.transform.localPosition.x + " Health Bar scale x: " + healthBarPivot.transform.localScale.x);
        }


    }
}
