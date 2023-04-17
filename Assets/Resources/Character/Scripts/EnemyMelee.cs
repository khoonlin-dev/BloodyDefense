using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using UnityEngine.Animations;

using UnityEngine.Playables;



[RequireComponent(typeof(Collider2D))]

public class EnemyMelee : MonoBehaviour {

    [System.NonSerialized]
    public EnemyAI enemyAI;

    [System.NonSerialized]
    public Enemy enemy;

    private Vector3 Vector3Container;

    private Vector3 contactPoint;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (enemyAI.enemyAttackMode != EnemyAI.AttackMode.Shooting)
        {

            if (enemyAI.enemyAIMode != EnemyAI.AIMode.ChaseObjective)
            {
                if ((col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Ally") || col.gameObject.CompareTag("Objective")))
                {

                    enemyAI.objectsInMeleeBox.Add(col.gameObject);

                    if (enemyAI.chaseTargets.Count == 0 && !enemyAI.chaseTargets.Contains(col.gameObject))
                    {
                        enemyAI.startChasing(col.gameObject);
                    }

                    if (enemyAI.chaseTargets.Count > 0 && col.gameObject == enemyAI.chaseTargets[0])
                    {

                        if (enemyAI.getState() == (int)EnemyAI.enemyState.Chasing)
                        {
                            enemyAI.stopChasing();
                        }


                        if (enemyAI.debug)
                            Debug.Log("I've entered melee box!");
                        enemyAI.startAttackState();
                        startAttacking();
                    }
                }
            }
            else
            {
                if (col.gameObject.CompareTag("Objective"))
                {

                    enemyAI.objectsInMeleeBox.Add(col.gameObject);

                    if (enemyAI.chaseTargets.Count == 0 && !enemyAI.chaseTargets.Contains(col.gameObject))
                    {
                        enemyAI.startChasing(col.gameObject);
                    }

                    if (enemyAI.chaseTargets.Count > 0 && col.gameObject == enemyAI.chaseTargets[0])
                    {

                        if (enemyAI.getState() == (int)EnemyAI.enemyState.Chasing)
                        {
                            enemyAI.stopChasing();
                        }

                        if (enemyAI.debug)
                            Debug.Log("I've entered melee box!");
                        enemyAI.startAttackState();
                        startAttacking();
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (enemyAI.enemyAttackMode != EnemyAI.AttackMode.Shooting)
        {
            if (enemyAI.enemyAIMode != EnemyAI.AIMode.ChaseObjective)
            {
                if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Ally"))
                {
                    enemyAI.objectsInMeleeBox.Remove(col.gameObject);

                    if (enemyAI.chaseTargets.Count > 0 && col.gameObject == enemyAI.chaseTargets[0])
                    {

                        stopAttacking();
                        enemyAI.continueChasing();

                    }
                }
            }
            else
            {
                if (col.gameObject.CompareTag("Objective"))
                {
                    enemyAI.objectsInMeleeBox.Remove(col.gameObject);

                    if (enemyAI.chaseTargets.Count > 0 && col.gameObject == enemyAI.chaseTargets[0])
                    {

                        stopAttacking();
                        enemyAI.continueChasing();

                    }
                }
            }
        }
    }


	// Use this for initialization
	void Start () {
        enemyAI = this.GetComponentInParent<EnemyAI>();

        enemy = this.GetComponentInParent<Enemy>();

        Vector3Container = new Vector3();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startAttacking()
    {

        if (enemyAI.playableGraph.IsValid())
        {
            enemyAI.playableGraph.Stop();
        }


        enemyAI.animator.SetBool(enemyAI.isWalkingHash, false);

        System.Random rand = new System.Random(Guid.NewGuid().GetHashCode());

        CancelInvoke("attack");

        Invoke("attack", rand.Next(enemyAI.minAttackInterval, enemyAI.maxAttackInterval));

        if (enemyAI.debug)
            Debug.Log("I've start attacking");
    }


    void stopAttacking()
    {
        if(enemyAI.playableGraph.IsValid())
        {
            enemyAI.playableGraph.Stop();
        }
        CancelInvoke("attack");
    }

    private void pauseAttacking()
    {
        if (enemyAI.playableGraph.IsValid())
        {
            enemyAI.playableGraph.Stop();
        }

        System.Random rand = new System.Random(Guid.NewGuid().GetHashCode());
        Invoke("attack", rand.Next(enemyAI.minAttackInterval, enemyAI.maxAttackInterval));
    }

    void attack()
    {

        if (enemyAI.chaseTargets.Count>0&&enemyAI.chaseTargets[0].gameObject.GetComponent<PlayerStatistics>().isActive)
        {
            if (enemyAI.getState() == (int)EnemyAI.enemyState.Attacking && enemyAI.chaseTargets.Count > 0)
            {
                if (enemyAI.playableGraph.IsValid())
                {
                    enemyAI.playableGraph.Stop();
                }

                AnimationPlayableUtilities.PlayClip(enemyAI.animator, enemyAI.attackAnimation, out enemyAI.playableGraph);


                Vector3Container = this.transform.position;

                Vector3Container.x += 2.5f;

                Vector3Container.z -= 5.0f;


                enemyAI.chaseTargets[0].GetComponent<PlayerStatistics>().sufferDamage(enemy.enemyDamage, Vector3Container);

                GameLevelAudioManager.playSound(GameLevelAudioManager.audio.punched, Vector2.Distance(this.transform.position, StartupConfig.player.transform.position));

                //Debug.Log("I've attacked " + enemyAI.chaseTargets[0] + " and dealt " + enemy.enemyDamage + " damage!");


                if (enemyAI.chaseTargets.Count > 0 && enemyAI.chaseTargets[0].activeInHierarchy)
                {
                    //System.Random rand = new System.Random(Guid.NewGuid().GetHashCode());

                    if (enemyAI.getState() == (int)EnemyAI.enemyState.Attacking)
                    {
                        Invoke("pauseAttacking", 0.2f);
                    }
                }

            }
        }
        else
        {
            Invoke("pauseAttacking",0.2f);
        }
    }
}
