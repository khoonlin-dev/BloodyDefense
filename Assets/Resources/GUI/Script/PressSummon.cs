using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressSummon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{


    private GameObject player;

    private SummonBoxContainer summonBox;

    private AllyPool allyPool;

    private float summonAllyCooldown;

    private float summonAllyCooldownLeft;

    private GameObject cooldownButton;

	// Use this for initialization
	void Start () {
        player = StartupConfig.player;

        if (player != null)
        {
            summonBox = player.GetComponentInChildren<SummonBoxContainer>();
            summonAllyCooldown = player.GetComponent<PlayerStatistics>().allySummonCooldown;
            allyPool = player.GetComponent<PlayerStatistics>().allyPool;
            summonAllyCooldownLeft = 0;
            cooldownButton = this.transform.GetChild(0).gameObject;
        }
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (summonAllyCooldownLeft == 0)
        {

            summonAllyCooldownLeft = summonAllyCooldown;

            allyPool.summonAlly(summonBox.getAvailableSummonPosition());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

	// Update is called once per frame
	void Update () {
		if(summonAllyCooldownLeft>0)
        {
            summonAllyCooldownLeft -= Time.deltaTime;
            if (summonAllyCooldownLeft <= 0)
            {
                summonAllyCooldownLeft = 0;
            }

            cooldownButton.GetComponent<Image>().fillAmount = summonAllyCooldownLeft / summonAllyCooldown;
        }
	}
}
