using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensing : MonoBehaviour {

    [System.NonSerialized]
    public EnemyAI enemyAI;
    [System.NonSerialized]
    public Enemy enemy;

    void OnTriggerEnter2D(Collider2D col)
    {
        // If someone enters the attack box
        if (enemyAI.enemyAIMode != EnemyAI.AIMode.Chase && enemyAI.enemyAIMode != EnemyAI.AIMode.ChaseObjective && enemyAI.enemyAttackMode != EnemyAI.AttackMode.Shooting)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                if (enemyAI.getState() != (int)EnemyAI.enemyState.Chasing && enemyAI.getState() != (int)EnemyAI.enemyState.Attacking && enemyAI.getState() != (int)EnemyAI.enemyState.Shooting)
                {

                    enemyAI.startChasing(col.gameObject);
                }
                else
                    if (!enemyAI.chaseTargets.Contains(col.gameObject))
                    {
                        if (enemyAI.enemyAttackPriority != EnemyAI.AttackPriority.Player)
                            enemyAI.chaseTargets.Add(col.gameObject);
                        else
                        {
                            enemyAI.chaseTargets.Insert(0, col.gameObject);
                            enemyAI.startChasing(col.gameObject);
                        }

                    }
            }
            else if (col.gameObject.CompareTag("Ally"))
            {
                if (enemyAI.getState() != (int)EnemyAI.enemyState.Chasing && enemyAI.getState() != (int)EnemyAI.enemyState.Attacking && enemyAI.getState() != (int)EnemyAI.enemyState.Shooting)
                {

                    enemyAI.startChasing(col.gameObject);
                }
                else
                    if (!enemyAI.chaseTargets.Contains(col.gameObject))
                    {
                        if (enemyAI.enemyAttackPriority != EnemyAI.AttackPriority.Ally)
                            enemyAI.chaseTargets.Add(col.gameObject);
                        else
                        {
                            enemyAI.chaseTargets.Insert(0, col.gameObject);
                            enemyAI.startChasing(col.gameObject);
                        }
                    }
            }
            else if (col.gameObject.CompareTag("Objective"))
            {
                if (enemyAI.getState() != (int)EnemyAI.enemyState.Chasing && enemyAI.getState() != (int)EnemyAI.enemyState.Attacking && enemyAI.getState() != (int)EnemyAI.enemyState.Shooting)
                {

                    enemyAI.startChasing(col.gameObject);
                }
                else
                    if (!enemyAI.chaseTargets.Contains(col.gameObject))
                    {
                        if (enemyAI.enemyAttackPriority != EnemyAI.AttackPriority.Objective)
                            enemyAI.chaseTargets.Add(col.gameObject);
                        else
                        {
                            enemyAI.chaseTargets.Insert(0, col.gameObject);
                            enemyAI.startChasing(col.gameObject);
                        }
                    }
            }
        }
        else if (enemyAI.enemyAttackMode == EnemyAI.AttackMode.Shooting)
        {
            if (col.gameObject.CompareTag("Player"))
            {

                enemyAI.objectsInShootingBox.Add(col.gameObject);

                if (enemyAI.enemyAIMode == EnemyAI.AIMode.Chase)
                {
                    if (enemyAI.chaseTargets.Count != 0 && col.gameObject == enemyAI.chaseTargets[0])
                    {
                        enemyAI.startShoot();
                    }
                    else if (enemyAI.chaseTargets.Count == 0)
                    {
                        enemyAI.chaseTargets.Add(col.gameObject);
                        enemyAI.startShoot();
                    }
                }
                else if (enemyAI.enemyAIMode != EnemyAI.AIMode.ChaseObjective)
                {
                    if (enemyAI.enemyAttackPriority == EnemyAI.AttackPriority.First || enemyAI.enemyAttackPriority == EnemyAI.AttackPriority.Ally)
                    {
                        
                        enemyAI.chaseTargets.Add(col.gameObject);
                        if (enemyAI.chaseTargets.Count==1)
                        {
                            enemyAI.startShoot();
                        }
                    }
                    else if (enemyAI.enemyAttackPriority == EnemyAI.AttackPriority.Player || enemyAI.enemyAttackPriority == EnemyAI.AttackPriority.Last || enemyAI.enemyAttackPriority == EnemyAI.AttackPriority.Objective)
                    {
                        enemyAI.chaseTargets.Insert(0, col.gameObject);
                        if (enemyAI.chaseTargets.Count == 1)
                        {
                            enemyAI.startShoot();
                        }
                    }

                }
                
            }
            else if (col.gameObject.CompareTag("Ally"))
            {

                enemyAI.objectsInShootingBox.Add(col.gameObject);

                if (enemyAI.enemyAIMode == EnemyAI.AIMode.Chase)
                {
                    if (enemyAI.chaseTargets.Count != 0 && col.gameObject == enemyAI.chaseTargets[0])
                    {
                        enemyAI.startShoot();
                    }
                    else if (enemyAI.chaseTargets.Count == 0)
                    {
                        enemyAI.chaseTargets.Add(col.gameObject);
                        enemyAI.startShoot();
                    }
                }
                else if (enemyAI.enemyAIMode != EnemyAI.AIMode.ChaseObjective)
                {
                    if (enemyAI.enemyAttackPriority == EnemyAI.AttackPriority.First || enemyAI.enemyAttackPriority == EnemyAI.AttackPriority.Player)
                    {

                        enemyAI.chaseTargets.Add(col.gameObject);
                        if (enemyAI.chaseTargets.Count == 1)
                        {
                            enemyAI.startShoot();
                        }
                    }
                    else if (enemyAI.enemyAttackPriority == EnemyAI.AttackPriority.Ally || enemyAI.enemyAttackPriority == EnemyAI.AttackPriority.Last || enemyAI.enemyAttackPriority == EnemyAI.AttackPriority.Objective)
                    {
                        enemyAI.chaseTargets.Insert(0, col.gameObject);
                        if (enemyAI.chaseTargets.Count == 1)
                        {
                            enemyAI.startShoot();
                        }
                    }

                }
            }
            else if (col.gameObject.CompareTag("Objective") )
            {
                enemyAI.objectsInShootingBox.Add(col.gameObject);

                if (enemyAI.enemyAIMode == EnemyAI.AIMode.Chase)
                {
                    if (enemyAI.chaseTargets.Count != 0 && col.gameObject == enemyAI.chaseTargets[0])
                    {
                        enemyAI.startShoot();
                    }
                    else if (enemyAI.chaseTargets.Count == 0)
                    {
                        enemyAI.chaseTargets.Add(col.gameObject);
                        enemyAI.startShoot();
                    }
                }
                else if (enemyAI.enemyAIMode != EnemyAI.AIMode.ChaseObjective)
                {
                    if (enemyAI.enemyAttackPriority == EnemyAI.AttackPriority.First || enemyAI.enemyAttackPriority == EnemyAI.AttackPriority.Objective)
                    {

                        enemyAI.chaseTargets.Add(col.gameObject);
                        if (enemyAI.chaseTargets.Count == 1)
                        {
                            enemyAI.startShoot();
                        }
                    }
                    else if (enemyAI.enemyAttackPriority == EnemyAI.AttackPriority.Ally || enemyAI.enemyAttackPriority == EnemyAI.AttackPriority.Player|| enemyAI.enemyAttackPriority == EnemyAI.AttackPriority.Last)
                    {
                        enemyAI.chaseTargets.Insert(0, col.gameObject);
                        if (enemyAI.chaseTargets.Count == 1)
                        {
                            enemyAI.startShoot();
                        }
                    }

                }
                else
                {
                    if (enemyAI.chaseTargets.Count != 0 && col.gameObject == enemyAI.chaseTargets[0])
                    {
                        enemyAI.startShoot();
                    }
                }
            }

        }
        
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (enemyAI.enemyAIMode != EnemyAI.AIMode.Chase && enemyAI.enemyAIMode != EnemyAI.AIMode.ChaseObjective)
        {

            if (enemyAI.chaseTargets.Count != 0 && enemyAI.chaseTargets.Contains(col.gameObject))
            {
                if (enemyAI.getState() == (int)EnemyAI.enemyState.Chasing || enemyAI.getState() == (int)EnemyAI.enemyState.Attacking)
                {
                    if (col.gameObject == enemyAI.chaseTargets[0])
                    {
                        //If target exits range during pursue
                        enemyAI.chaseTargets.Remove(col.gameObject);    //Remove the target from target list
                        if (enemyAI.chaseTargets.Count > 0)
                        {   //If there is still target in target list
                            enemyAI.continueChasing();
                        }
                        else
                        {
                            enemyAI.stopChasing();

                            if (enemyAI.destinationList.Count != 0)
                            {
                                enemyAI.continuePatrol();
                            }
                            else
                            {


                                if (this.transform.root.position != enemyAI.idlePoint)
                                {
                                    enemyAI.startPatrol(false, enemyAI.idlePoint);
                                }
                                else
                                    enemyAI.startIdle();
                            }
                        }
                    }
                    else
                    {
                        enemyAI.chaseTargets.Remove(col.gameObject);
                    }

                }
                else if (enemyAI.getState() == (int)EnemyAI.enemyState.Shooting)
                {

                    enemyAI.chaseTargets.Remove(col.gameObject);

                    if(enemyAI.chaseTargets.Count==0)
                    {
                        enemyAI.stopShoot();
                    }
                }
            }
        }
        else if ((enemyAI.enemyAIMode == EnemyAI.AIMode.Chase || enemyAI.enemyAIMode == EnemyAI.AIMode.ChaseObjective) && enemyAI.enemyAttackMode != EnemyAI.AttackMode.Shooting)
        {
            if (enemyAI.chaseTargets.Count != 0 && enemyAI.chaseTargets.Contains(col.gameObject))
            {
                if (enemyAI.getState() == (int)EnemyAI.enemyState.Chasing || enemyAI.getState() == (int)EnemyAI.enemyState.Shooting || enemyAI.getState() == (int)EnemyAI.enemyState.Attacking)
                {
                    if (col.gameObject == enemyAI.chaseTargets[0] && !col.gameObject.activeInHierarchy)
                    {
                        enemyAI.chaseTargets.Remove(col.gameObject);
                        enemyAI.startChasing(enemyAI.findNewTarget());
                    }
                    else
                    {
                        enemyAI.continueChasing();
                        //Continue chasing;
                    }
                }
            }
            
        }
        else if (enemyAI.enemyAttackMode == EnemyAI.AttackMode.Shooting)
        {
            if (enemyAI.objectsInShootingBox.Contains(col.gameObject))
                enemyAI.objectsInShootingBox.Remove(col.gameObject);

            if (enemyAI.chaseTargets.Contains(col.gameObject))
            {
                if (enemyAI.enemyAIMode == EnemyAI.AIMode.Chase || enemyAI.enemyAIMode == EnemyAI.AIMode.ChaseObjective)
                {
                    if (col.gameObject == enemyAI.chaseTargets[0])      //If it is your target
                    {
                        if (enemyAI.chaseTargets[0].activeInHierarchy)   //if it exits your collision box not because he died
                        {
                            enemyAI.stopShoot();
                            enemyAI.continueChasing();
                        }
                        else  //If it exits your collision box because he died
                        {
                            enemyAI.chaseTargets.Remove(col.gameObject);
                            enemyAI.startChasing(enemyAI.findNewTarget());
                            //chase this enemyAI.findNewTarget();
                        }
                    }
                }
                else
                {
                    enemyAI.chaseTargets.Remove(col.gameObject);
                    if (enemyAI.chaseTargets.Count==0)
                    {
                        enemyAI.stopShoot();

                    }
                }
            }
        }
    }

	// Use this for initialization
	void Awake () {
        enemyAI = this.GetComponentInParent<EnemyAI>();

        enemy = this.GetComponentInParent<Enemy>();
	}
	
    void OnEnable()
    {

    }

	// Update is called once per frame
	void Update () {
		
	}
}
