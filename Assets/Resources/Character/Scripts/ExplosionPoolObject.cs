using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPoolObject : MonoBehaviour {

    public GameObject pooledObject;
    public List<GameObject> pooledObjects;
    public int pooledAmount = 30;

	// Use this for initialization
	void Awake () {
        pooledObjects = new List<GameObject>();

        for (int x = 0; x < pooledAmount; x++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.SetActive(false);
            obj.transform.SetParent(this.transform, false);
            pooledObjects.Add(obj);
        }
	}

    public void setExplosion(Vector3 position)
    {
        for (int x = 0; x < pooledObjects.Count; x++)
        {
            if (!pooledObjects[x].activeInHierarchy)
            {
                pooledObjects[x].transform.position = position;
                pooledObjects[x].SetActive(true);

                return;
            }
        }


        return;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
