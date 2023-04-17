using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressNormalAttack : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private GameObject player;

    private PlayerStatistics playerStats;

    private PlayerMelee playerMelee;

    private SwipePlayer playerSwipe;

    public void OnPointerDown(PointerEventData eventData)
    {
       if (!playerSwipe.isDashableState && !playerMelee.isMunching)
       {

           if(playerMelee.isHealing)
           {
               playerMelee.stopNormalHealing();
           }

           playerMelee.startNormalAttack();

       }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

	// Use this for initialization
	void Start () {
        player = StartupConfig.player;

        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStatistics>();
            playerSwipe = player.GetComponent<SwipePlayer>();
            playerMelee = player.GetComponentInChildren<PlayerMelee>();
        }
        else
        {
            Debug.Log("Player is NULL!!");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
