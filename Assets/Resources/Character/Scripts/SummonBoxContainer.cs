using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonBoxContainer : MonoBehaviour {

    public List<SummonBoxScript> summonBoxes;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 getAvailableSummonPosition()
    {
        if (summonBoxes.Count != 0)
        {
            for (int i = 0; i < summonBoxes.Count; i++)
            {
                if (!summonBoxes[i].isAnyoneCollided())
                {
                    return summonBoxes[i].gameObject.transform.position;
                }
            }

            return summonBoxes[summonBoxes.Count - 1].gameObject.transform.position;
        }

        return Vector3.zero;
    }
}
