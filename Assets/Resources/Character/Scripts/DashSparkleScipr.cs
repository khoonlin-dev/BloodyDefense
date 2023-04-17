using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSparkleScipr : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    void OnEnable()
    {
        Invoke("disable", 1.5f);
    }

    void disable()
    {
        this.gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
