using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressShoot : MonoBehaviour,IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private GameObject player;

    private PlayerStatistics playerStats;

    private bool shootingLock; //To prevent player from spamming the shoot button, we set a lock to prevent player from shooting before the firing rate interval for the previous shot is finished 

    private int isShootingHash;

    private float playerAngleDegree;

    private Vector3 vector3container;

    private GameObject bulletPool;

    private RawImage jsContainer;
    private RawImage joystick;

    [System.NonSerialized]
    public Vector3 InputDirection;


	// Use this for initialization
	void Start () {

        jsContainer = GetComponent<RawImage>();
        joystick = transform.GetChild(0).GetComponent<RawImage>(); //this command is used because there is only one child in hierarchy
        InputDirection = Vector3.zero;

        player = StartupConfig.player;

        shootingLock = false;

        vector3container = new Vector3();


        if(player!=null)
        {
            playerStats = player.GetComponent<PlayerStatistics>();

            isShootingHash = Animator.StringToHash("isShooting");
        }

	}


    public void OnPointerDown(PointerEventData eventData)
    {
        if (!shootingLock)
        {
            playerStats.isShooting = true;

            playerStats.gun.GetComponent<Animator>().SetBool(isShootingHash, true);
            InvokeRepeating("Fire", 0, playerStats.bulletFiringInterval);
            lockShooting(playerStats.bulletFiringInterval - 0.05f);
        }

        OnDrag(eventData);

       
    }


    public void OnDrag(PointerEventData ped)
    {

        //Debug.Log(ped.position);

        Vector2 position = Vector2.zero;

        //Vector2 position = new Vector2(jsContainer.rectTransform.rect.height/2, jsContainer.rectTransform.rect.width/2);

        //To get InputDirection
        RectTransformUtility.ScreenPointToLocalPointInRectangle
                (jsContainer.rectTransform,
                ped.position,
                ped.pressEventCamera,
                out position);

        position.x = (position.x / jsContainer.rectTransform.sizeDelta.x);
        position.y = (position.y / jsContainer.rectTransform.sizeDelta.y);

        float x = (jsContainer.rectTransform.pivot.x == 1f) ? position.x * 2 + 1 : position.x * 2 - 1;
        float y = (jsContainer.rectTransform.pivot.y == 1f) ? position.y * 2 + 1 : position.y * 2 - 1;

        InputDirection = new Vector3(x, y, 0);
        InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;

        //Debug.Log(InputDirection);

        //to define the area in which joystick can move around
        joystick.rectTransform.anchoredPosition = new Vector3(InputDirection.x * (jsContainer.rectTransform.sizeDelta.x / 3)
                                                               , InputDirection.y * (jsContainer.rectTransform.sizeDelta.y) / 3);

        vector3container.x = 0.0f;
        vector3container.y = -0.3f;
        vector3container.z = -1.0f;

        playerStats.gun.transform.localPosition = vector3container;

        if (playerStats.playerDirection.x <= 0)
        {
            playerStats.gun.transform.localRotation = Quaternion.Euler(0, 0, ((-(Mathf.Atan2(InputDirection.x, InputDirection.y)) * 180 / Mathf.PI) + 90) - 180);
        }
        else
        {
            playerStats.gun.transform.localRotation = Quaternion.Euler(0, 0, -((-(Mathf.Atan2(InputDirection.x, InputDirection.y)) * 180 / Mathf.PI) + 90));
        }

    }

    public void OnPointerUp(PointerEventData ped)
    {
        //Debug.Log("LOL");
        InputDirection = Vector3.zero;
        joystick.rectTransform.anchoredPosition = Vector2.zero;

        playerStats.isShooting = false;

        vector3container.x = -0.6f;
        vector3container.y = -0.6f;
        vector3container.z = -1.0f;

        playerStats.gun.transform.localPosition = vector3container;

        playerStats.gun.transform.localRotation = Quaternion.Euler(0, 0, -45);

        playerStats.gun.GetComponent<Animator>().SetBool(isShootingHash, false);
        CancelInvoke("Fire");
    }

    void Fire()
    {

        if (StartupConfig.bulletPool == null)
        {
            StartupConfig.bulletPool = GameObject.Find("BulletPool").GetComponent<BulletPoolBehavior>();
        }

        else
        {
            StartupConfig.bulletPool.getPooledObject(player, InputDirection);

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void lockShooting(float duration)
    {
        shootingLock = true;
        Invoke("unlockShooting", duration);
    }

    void unlockShooting()
    {
        shootingLock = false;
    }
}
