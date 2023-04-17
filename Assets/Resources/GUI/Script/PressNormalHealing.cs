using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PressNormalHealing : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private GameObject player;

    private PlayerMelee playerMelee;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerMelee.healingCoolDownLeft == 0 && !playerMelee.areEveryAllyOk())
        {
            playerMelee.healingCoolDownLeft = playerMelee.healingCooldownTime;
            playerMelee.isHealing = true;
            playerMelee.startNormalHealing();
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
            playerMelee = player.GetComponentInChildren<PlayerMelee>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
