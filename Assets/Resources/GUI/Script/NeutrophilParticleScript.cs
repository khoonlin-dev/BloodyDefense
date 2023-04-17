using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutrophilParticleScript : MonoBehaviour {


    public MainMenuCanvasController mainMenuCanvas;

    public Vector3 spawnPoint;

    public Vector3 endPoint;

    public float speed;

    public float delay;

    private bool startWalk = false;

	// Use this for initialization
	void Start () {


        if (!mainMenuCanvas.activeHeroesOnScreen.Contains(this.gameObject))
        {
            mainMenuCanvas.activeHeroesOnScreen.Add(this.gameObject);
        }

        this.GetComponent<Animator>().SetBool("isWalking", true);
        Invoke("startWalking", delay);

	}
	
	// Update is called once per frame
	void Update () 
    {
        if (startWalk)
        {
            if (this.transform.position != endPoint)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, endPoint, speed * Time.deltaTime);

            }
            else
            {
                this.transform.position = spawnPoint;
            }
        }
	}

    void startWalking()
    {
        startWalk = true;
    }

    void OnMouseDown()
    {
        if (MainMenuCanvasController.isSelectingHero)
        {

            //Hide everything
            //is selectinghero = false;
            //Show killer info canvas;
            StaticDataHandler.characterChosen = (int)StaticDataHandler.characterCode.Neutrophil;

            mainMenuCanvas.showHeroDetail();
        }
    }
}
