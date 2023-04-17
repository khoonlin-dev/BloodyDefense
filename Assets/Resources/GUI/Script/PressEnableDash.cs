using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressEnableDash : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private GameObject player;

    private PlayerStatistics playerStats;

    private SwipePlayer playerSwipe;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerSwipe.dashingCooldownLeft == 0)
        {

            Invoke("enableDash", 0.5f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

	// Use this for initialization
	void Start () 
    {

        player = StartupConfig.player;
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStatistics>();
            playerSwipe = player.GetComponent<SwipePlayer>();
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void enableDash()
    {
        playerSwipe.dashingCooldownLeft = playerSwipe.dashingCooldown;
        playerSwipe.startDashing();
    }
}
