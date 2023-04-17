using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonBoxScript : MonoBehaviour {

    private List<GameObject> gameObjectsCollided;

    private string wallString = "Wall";

	// Use this for initialization
	void Awake () {
        gameObjectsCollided = new List<GameObject>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {

        if(col.gameObject.CompareTag(wallString))
        {
            gameObjectsCollided.Add(col.gameObject);

            Debug.Log(isAnyoneCollided());

        }
        
    }

    void OnTriggerExit2D(Collider2D col)
    {

        if (col.gameObject.CompareTag(wallString))
        {
            if(gameObjectsCollided.Contains(col.gameObject))
            {
                gameObjectsCollided.Remove(col.gameObject);
            }

            Debug.Log(isAnyoneCollided());
        }

    }

    public bool isAnyoneCollided()
    {
        if (gameObjectsCollided.Count > 0)
            return true;
        else
            return false;
    }
}
