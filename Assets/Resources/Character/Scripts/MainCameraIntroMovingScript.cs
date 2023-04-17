using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraIntroMovingScript : MonoBehaviour {

    public List<Vector3> movePoints;

    public Canvas gameplayCanvas;

    public BattlefieldNotiSystem notiBar;

    private int currentPoint = 0;

    public CameraFollow camera;

    private bool allowMove = true;

    public float waitTime;

    private float moveSpeed = 3.5f;

    private float countDownClock;

    private bool isStillIntro = true;

	// Use this for initialization

    void Awake()
    {
        Time.timeScale = 0;
    }

	void Start () {
        countDownClock = 0;
        allowMove = true;
        gameplayCanvas.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (isStillIntro)
        {
            if (countDownClock == 0)
            {
                if (this.transform.position != movePoints[currentPoint])
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, movePoints[currentPoint], moveSpeed);
                }
                else
                {
                    if (currentPoint != movePoints.Count - 1)
                    {
                        currentPoint++;

                        countDownClock = waitTime;

                        //allowMove = false;

                        //CancelInvoke("allowCamToMove");
                        //Invoke("allowCamToMove", waitTime);

                        if (currentPoint == movePoints.Count - 1)
                        {
                            moveSpeed = 0.75f;
                        }
                    }
                    else
                    {
                        camera.SetUpCamFollow();
                        Time.timeScale = 1;
                        gameplayCanvas.gameObject.SetActive(true);
                        Invoke("showNoti", 0.5f);
                        isStillIntro = false;
                    }
                }
            }
            else
            {
                countDownClock -= 0.1f;
                if (countDownClock <= 0)
                {
                    countDownClock = 0;
                }
            }
        }
        
	}

    public void showNoti()
    {
        notiBar.flashNotification(2.5f, "Activate the killer b-cells cannon turret!");
    }

    void allowCamToMove()
    {
        allowMove = true;
    }


}
