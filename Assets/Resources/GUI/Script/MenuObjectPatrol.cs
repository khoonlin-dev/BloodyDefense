using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuObjectPatrol : MonoBehaviour {

    public bool isHelper;

    private bool isPatrolling;

    public List<Vector2> route;

    private int routeCounter;

    public int waitingTime;

    private Vector3 vector3Container;

    private int isWalkingHash;

    public Animator animator;

    public MainMenuCanvasController mainMenuCanvas;

	// Use this for initialization
	void Start () {

        if (!mainMenuCanvas.activeHeroesOnScreen.Contains(this.gameObject))
        {
            mainMenuCanvas.activeHeroesOnScreen.Add(this.gameObject);
        }

        vector3Container = new Vector3();

        isWalkingHash = Animator.StringToHash("isWalking");

        isPatrolling = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (isPatrolling)
        {
            if((Vector2)this.transform.position != route[routeCounter])
            {

                if (route[routeCounter].x > this.transform.position.x )
                {
                    if(this.transform.localScale.x > 0)
                    {
                        flipPlayer();
                    }
                }
                else if (route[routeCounter].x <= this.transform.position.x)
                {
                    if (this.transform.localScale.x < 0)
                    {
                        flipPlayer();
                    }
                }

                vector3Container = route[routeCounter];
                vector3Container.z = this.transform.position.z;

                this.transform.position = Vector3.MoveTowards(this.transform.position, vector3Container, 8 * Time.deltaTime);

                if(!animator.GetBool(isWalkingHash))
                    animator.SetBool(isWalkingHash, true);

            }
            else
            {
                if(routeCounter == route.Count-1)
                {
                    routeCounter = 0;
                }
                else
                {
                    routeCounter++;
                }

                isPatrolling = false;


                animator.SetBool(isWalkingHash, false);


                Invoke("startPatrol", waitingTime);
            }
        }

	}

    void OnMouseDown()
    {

        if(MainMenuCanvasController.isSelectingHero)
        {

            //Hide everything
            //is selectinghero = false

            if (isHelper)
            {
                StaticDataHandler.characterChosen = (int)StaticDataHandler.characterCode.Helper;
  
                //Show helper info canvas
            }
            else
            {
                StaticDataHandler.characterChosen = (int)StaticDataHandler.characterCode.Macrophage;

                //Show mac info canvas
            }
            mainMenuCanvas.showHeroDetail();
        }
    }

    void startPatrol()
    {
        isPatrolling = true;
    }

    public void flipPlayer()
    {

        vector3Container = this.transform.localScale;
        vector3Container.x = -(vector3Container.x);
        this.transform.localScale = vector3Container;


    }
}
