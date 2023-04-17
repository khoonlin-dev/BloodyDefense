using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Credit to https://www.youtube.com/watch?v=RVJqf4mL1pU

public class SwipePlayer : MonoBehaviour
{
    private Vector2 startPos, endPos, direction;

    private GameObject player;

    private PlayerStatistics playerStats;

    private GameObject durationBar;

    private GameObject sword;

    private GameObject swordDashSpark;

    private float touchTimeStart, touchTimeFinish, timeInterval;
    public float slideTime;

    private GameObject normalAttackButton;
    private GameObject skillOneButton;
    private GameObject vpadButton;
    private GameObject cooldownOneButton;


    private CapsuleCollider2D collider;

    [System.NonSerialized]
    public float dashingCooldownLeft;

    public float dashingCooldown;

    public float dashingDuration;

    private float dashingDurationLeft;

    public int dashDamage;

    [System.NonSerialized]
    public bool isDashableState;

    //[Range(0.05f, 10f)]
    public float throwForce = 15.0f;

    [System.NonSerialized]
    public bool isDashing;

    public DashSparklePoolBehavior dashSparkles;

    [System.NonSerialized]
    public GameObject ultiScreen;

    private Vector3 vector3PlaceHolder;

    private Vector3 swipeDirection;

    // Use this for initialization
    void Start()
    {
        swipeDirection = new Vector3();

        vector3PlaceHolder = new Vector3();

        player = this.transform.gameObject;
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStatistics>();

            collider = this.transform.gameObject.GetComponentInChildren<CapsuleCollider2D>();

            durationBar = playerStats.durationBar;

            normalAttackButton = playerStats.normalAttackButton;

            skillOneButton = playerStats.skillOneButton;

            vpadButton = playerStats.vpadButton;

            cooldownOneButton = playerStats.cooldownOneButton;

            if (StaticDataHandler.characterChosen == 1)    //If neutrophil is chosen
            {
                sword = player.transform.Find("Sword").gameObject;
                swordDashSpark = sword.transform.Find("Dash-Spark").gameObject;
            }
        }
    }

    void onEnable()
    {
        isDashing = false;
        isDashableState = false;

    }

    public void startDashing()
    {
        playerStats.hideErrorText();
        if (durationBar != null)
        {
            durationBar.SetActive(true);
            durationBar.GetComponentInChildren<Text>().text = "Swipe screen to dash!";
        }
        dashingDurationLeft = dashingDuration;
        durationBar.SetActive(true);
        isDashableState = true ;
        normalAttackButton.SetActive(false);
        //skillOneButton.SetActive(false);
        vpadButton.SetActive(false);


        if (ultiScreen != null)
            ultiScreen.SetActive(true);

        Invoke("stopDashing", dashingDuration);

        GameLevelAudioManager.pauseBGMandPlay(GameLevelAudioManager.audio.ulti);
    }

    public void stopDashing()
    {

        if (durationBar != null)
            durationBar.SetActive(false);
        isDashableState = false;
        if (normalAttackButton!=null)
            normalAttackButton.SetActive(true);
        if (vpadButton != null)
            vpadButton.SetActive(true);

        if (ultiScreen != null)
            ultiScreen.SetActive(false);

        GameLevelAudioManager.resumeBGMandStop(GameLevelAudioManager.audio.ulti);
    }


    void FixedUpdate()
    {
        if (isDashing)
        {
            if (playerStats.playerDirection.x >= 0)
            {
                if (player.GetComponent<MovePlayers>().moveRightPermission == 0.0f)
                {
                    stop();
                    return;
                }
            }
            else
            {
                if (player.GetComponent<MovePlayers>().moveLeftPermission == 0.0f)
                {
                    stop();
                    return;
                }
            }

            if (playerStats.playerDirection.y >= 0)
            {
                if (player.GetComponent<MovePlayers>().moveUpPermission == 0.0f)
                {
                    stop();
                    return;
                }
            }
            else
            {
                if (player.GetComponent<MovePlayers>().moveDownPermission == 0.0f)
                {
                    stop();
                    return;
                }
            }

        }
    }

    void Update()
    {
        if(isDashableState && !isDashing)
        {
            calculateDashingInput();
        }

        calculateDashingCooldown();

        if (dashingDurationLeft > 0)
        {
            dashingDurationLeft -= Time.deltaTime;
            if (dashingDurationLeft <= 0)
            {
                dashingDurationLeft = 0;
            }

            durationBar.GetComponent<Image>().fillAmount = dashingDurationLeft / dashingDuration;
        }
    }

    void OnDestroy()
    {
        stopDashing();
    }

    void OnDisable()
    {
        stopDashing();
    }

    void calculateDashingInput()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchTimeStart = Time.time;
            startPos = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            touchTimeFinish = Time.time;
            timeInterval = touchTimeFinish - touchTimeStart;
            endPos = Input.GetTouch(0).position;
            direction = startPos - endPos;

            //Debug.Log("Original Direction: " + (-direction));

            if (direction.x > 1.0f || direction.y > 1.0f || direction.x < -1.0f || direction.y < -1.0f)
            {
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    direction.y /= Mathf.Abs(direction.x);
                    direction.x /= Mathf.Abs(direction.x);
                }
                else
                {
                    direction.x /= Mathf.Abs(direction.y);
                    direction.y /= Mathf.Abs(direction.y);
                }
            }

            playerStats.playerDirection = -direction;
            swipeDirection = playerStats.playerDirection;

            if (playerStats.playerDirection.x < 0)
            {
                if (collider.offset.x > 0)
                {
                    collider.offset = new Vector2(-collider.offset.x, collider.offset.y);

                }
            }

            else if (playerStats.playerDirection.x > 0)
            {
                if (collider.offset.x < 0)
                {

                    collider.offset = new Vector2(-collider.offset.x, collider.offset.y);


                }
            }

            //GetComponent<Rigidbody2D>().AddForce(-direction / timeInterval * throwForce);


            if (swipeDirection.x >= 0)
            {
                swipeDirection.x *= player.GetComponent<MovePlayers>().moveRightPermission;
            }
            else
            {
                swipeDirection.x *= player.GetComponent<MovePlayers>().moveLeftPermission;
            }

            if (swipeDirection.y >= 0)
            {
                swipeDirection.y *= player.GetComponent<MovePlayers>().moveUpPermission;
            }
            else
            {
                swipeDirection.y *= player.GetComponent<MovePlayers>().moveDownPermission;
            }


            GetComponent<Rigidbody2D>().velocity = swipeDirection * throwForce;
            isDashing = true;

            if (StaticDataHandler.characterChosen == 1)    //If neutrophil is chosen
            {
                vector3PlaceHolder = Vector3.zero;

                vector3PlaceHolder.y = -0.2f;

                vector3PlaceHolder.z = -5.0f;

                sword.transform.localPosition = vector3PlaceHolder;

                sword.transform.rotation = Quaternion.Euler(0, 0, (-(Mathf.Atan2(playerStats.playerDirection.x, playerStats.playerDirection.y)) * 180 / Mathf.PI) + 90);

                if (swordDashSpark != null)
                {
                    swordDashSpark.SetActive(true);
                }

                if (dashSparkles != null)
                {
                    dashSparkles.setSparkle(Quaternion.Euler((-(Mathf.Atan2(playerStats.playerDirection.x, playerStats.playerDirection.y)) * 180 / Mathf.PI) - 90, dashSparkles.transform.localRotation.y, dashSparkles.transform.localRotation.z));
                }

                GameLevelAudioManager.playSound(GameLevelAudioManager.audio.dash, 0);
            }
            Invoke("stop", slideTime);
        }
    }

    void calculateDashingCooldown()
    {
        if (dashingCooldownLeft > 0)
        {
            dashingCooldownLeft -= Time.deltaTime;
            if (dashingCooldownLeft <= 0)
            {
                dashingCooldownLeft = 0;
                //Debug.Log("The dashing cooldown has stopped!");

            }

            cooldownOneButton.GetComponent<Image>().fillAmount = dashingCooldownLeft / dashingCooldown;
        }
    }

    void stop()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        isDashing = false;


        if(swordDashSpark!=null)
        {
            swordDashSpark.SetActive(false);
        }


        if (playerStats.playerDirection.x < 0)
        {

               if (StaticDataHandler.characterChosen == 1)
               {

                    vector3PlaceHolder.x = -0.3f;

                    vector3PlaceHolder.y = -1.0f;

                    vector3PlaceHolder.z = -5.0f;

                    sword.transform.localPosition = vector3PlaceHolder;

                    vector3PlaceHolder = Vector3.zero;

                    vector3PlaceHolder.z = 135;

                    sword.transform.eulerAngles = vector3PlaceHolder;
                }

        }

        else if (playerStats.playerDirection.x > 0)
        {


            if (StaticDataHandler.characterChosen == 1)
            {
                vector3PlaceHolder.x = 0.3f;

                vector3PlaceHolder.y = -1.0f;

                vector3PlaceHolder.z = -5.0f;

                sword.transform.localPosition = vector3PlaceHolder;

                vector3PlaceHolder = Vector3.zero;

                vector3PlaceHolder.z = 45;

                sword.transform.eulerAngles = vector3PlaceHolder;
            }
        }

        //Debug.Log(collider.offset.x);

        CancelInvoke("stop");
    }
}