using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolBehavior : MonoBehaviour {

    public BulletPoolBehavior bulletPool;
    public GameObject pooledObject;
    public int pooledAmount = 30;

    private Vector3 vector3Container;

    List<GameObject> pooledObjects;

    void Awake()
    {
        bulletPool = this;

        vector3Container = new Vector3();
    }

	// Use this for initialization
	void Start () {

        pooledObjects = new List<GameObject>();

        for (int x=0; x<pooledAmount;x++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.SetActive(false);
            obj.transform.SetParent(this.transform, false);
            pooledObjects.Add(obj);
        }

	}

    public GameObject getPooledObject(GameObject shooter)
    {
        for (int x=0;x<pooledObjects.Count;x++)
        {
            if (!pooledObjects[x].activeInHierarchy)
            {
                
                pooledObjects[x].GetComponent<BulletBehavior>().setShooterCharacter(shooter);
                pooledObjects[x].SetActive(true);
                return pooledObjects[x];
            }
        }

        Debug.Log("Insufficient object in bullet pool");

        return null;
    }

    public GameObject getPooledObject(GameObject shooter, Vector3 direction)
    {
        for (int x = 0; x < pooledObjects.Count; x++)
        {
            if (!pooledObjects[x].activeInHierarchy)
            {

                pooledObjects[x].GetComponent<BulletBehavior>().setShooterCharacter(shooter);


                pooledObjects[x].GetComponent<BulletBehavior>().setShootingDirection(direction);

                pooledObjects[x].SetActive(true);
                return pooledObjects[x];
            }
        }

        Debug.Log("Insufficient object in bullet pool");

        return null;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
