using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyPool : MonoBehaviour {

    public GameObject pooledObject;
    [System.NonSerialized]
    public List<GameObject> pooledObjects;
    public int pooledAmount = 5;

    [System.NonSerialized]
    public List<GameObject> activeObjects;

    public int maxSummonLimit = 0;

    private int summonNum;

    private int currentReplacementIndex;

	// Use this for initialization
	void Start () {

        activeObjects = new List<GameObject>();

        summonNum = 0;

        currentReplacementIndex = 0;

        pooledObjects = new List<GameObject>();

        for (int x = 0; x < pooledAmount; x++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.SetActive(false);
            //obj.transform.SetParent(this.transform, false);
            pooledObjects.Add(obj);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void summonAlly(Vector3 position)
    {




        //Debug.Log("currentSummonNum(): " + currentSummonNum() + " maxSummonLimit: " + maxSummonLimit);


        if (currentSummonNum() < maxSummonLimit)
        {

            for (int x = 0; x < pooledObjects.Count; x++)
            {
                if (!pooledObjects[x].activeInHierarchy)
                {
                    pooledObjects[x].transform.position = position;
                    pooledObjects[x].SetActive(true);
                    activeObjects.Add(pooledObjects[x]);

                    //Debug.Log("currentSummonNum(): " + currentSummonNum() + " maxSummonLimit: " + maxSummonLimit);
                    Invoke("playSummonSound", 0.75f);

                    return;
                }
            }
        }
        else
        {
            if (currentSummonNum() == maxSummonLimit)
            {


                for (int x = 0; x < pooledObjects.Count; x++)
                {
                    if (!pooledObjects[x].activeInHierarchy)
                    {
                        pooledObjects[x].transform.position = position;
                        pooledObjects[x].SetActive(true);
                        activeObjects.Add(pooledObjects[x]);


                        Invoke("playSummonSound", 0.75f);

                        //Debug.Log("currentSummonNum() 0: " + currentSummonNum() + " maxSummonLimit 0: " + maxSummonLimit);

                        break;
                    }
                }

                if (activeObjects.Count > 0)
                {
                    activeObjects[0].GetComponent<PlayerStatistics>().defeated();
                }

                //Debug.Log("currentSummonNum() 1: " + currentSummonNum() + " maxSummonLimit 1: " + maxSummonLimit);



                return;

            }
        }


        return;
    }

    private int currentSummonNum()
    {

        return activeObjects.Count;
    }

    public void removeThisObject(GameObject ally)
    {
        if(activeObjects.Contains(ally))
        {
            activeObjects.Remove(ally);
        }
    }

    void playSummonSound()
    {
        CancelInvoke("playSummonSound");
        GameLevelAudioManager.playSound(GameLevelAudioManager.audio.spawn, 5.0f);
        Invoke("playReloadSound", 0.8f);
    }

    void playReloadSound()
    {
        GameLevelAudioManager.playSound(GameLevelAudioManager.audio.reload, 5.0f);
    }
}
