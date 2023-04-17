using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void OnEnable()
    {
        Invoke("disable", 2.0f);
    }

    void disable()
    {
        this.transform.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
