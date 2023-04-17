using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSleepScript : MonoBehaviour {

    public Animator animator;

    private int isSnoringHash;

    public int snoringInterval;

    public MainMenuCanvasController mainMenuCanvas;

	// Use this for initialization
	void Start () {

        if(!mainMenuCanvas.activeHeroesOnScreen.Contains(this.gameObject))
        {
            mainMenuCanvas.activeHeroesOnScreen.Add(this.gameObject);
        }

        isSnoringHash = Animator.StringToHash("isSnoring");

        InvokeRepeating("Snore", 0.0f, snoringInterval);
	}
	
    void Snore()
    {
        animator.SetBool(isSnoringHash, true);

        Invoke("StopSnore", 0.2f);

    }

    void OnMouseDown()
    {
        if (MainMenuCanvasController.isSelectingHero)
        {

            //Hide everything
            //is selectinghero = false;
            //Show killer info canvas;
            StaticDataHandler.characterChosen = (int)StaticDataHandler.characterCode.Killer;

            mainMenuCanvas.showHeroDetail();
        }
    }

    void StopSnore()
    {
        animator.SetBool(isSnoringHash, false);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
