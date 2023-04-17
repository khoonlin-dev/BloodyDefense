using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressMunch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private GameObject player;

    private PlayerMelee playerMelee;

	// Use this for initialization
	void Start () {
        player = StartupConfig.player;

        if(player!=null)
        {
            playerMelee = player.GetComponentInChildren<PlayerMelee>();
        }
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerMelee.munchingCoolDownLeft ==0)
        {
            playerMelee.munchingCoolDownLeft = playerMelee.munchingCooldownTime;
            playerMelee.isMunching = true;
            playerMelee.startMunching();
        }
    }
	
    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
    
	// Update is called once per frame
	void Update () {
		
	}
}
