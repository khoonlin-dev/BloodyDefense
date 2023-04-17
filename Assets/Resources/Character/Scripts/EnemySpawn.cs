using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    public float spawnInterval;

    public GameObject shootingEnemy;

    public GameObject meleeEnemy;

    private List<GameObject> shootingEnemies;

    private List<GameObject> meleeEnemies;

    public List<float> squareBoxMinX;

    public List<float> squareBoxMaxX;

    public List<float> squareBoxMinY;

    public List<float> squareBoxMaxY;

    private List<float> dummyListFloat;

    private int randomNum;

    private Vector3 position;

    public int noOfAdjustments;

    public int adjustmentInterval;

	// Use this for initialization
	void Start () {

        shootingEnemies = new List<GameObject>();
        meleeEnemies = new List<GameObject>();

        for (int x = 0; x < 15; x++)
        {
            GameObject obj = (GameObject)Instantiate(shootingEnemy);
            obj.SetActive(false);
            obj.transform.SetParent(this.transform, false);
            shootingEnemies.Add(obj);
        }

        for (int x = 0; x < 30; x++)
        {
            GameObject obj = (GameObject)Instantiate(meleeEnemy);
            obj.SetActive(false);
            obj.transform.SetParent(this.transform, false);
            meleeEnemies.Add(obj);
        }

        Invoke("adjustSpawnInterval", adjustmentInterval);

        Invoke("spawnEnemy", spawnInterval);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void spawnEnemy()
    {

        randomNum = Random.Range(0, (squareBoxMinX.Count - 1));

        if (randomNum == 0)
        {
            spawn(randomNum, Random.Range(1, 2));
        }
        else if (randomNum == 1 ||randomNum == 2)
        {
            spawn(randomNum, 0);
        }

        Invoke("spawnEnemy", Mathf.Abs(Random.Range(spawnInterval - 3.0f, spawnInterval + 3.0f)));

    }

    void adjustSpawnInterval()
    {
        if (noOfAdjustments > 0)
        {
            spawnInterval *= 0.75f;

            noOfAdjustments--;

            if (noOfAdjustments>0)
                Invoke("adjustSpawnInterval", adjustmentInterval);
        }

    }

    void spawn(int spawnPoint, int assemblyPoint)
    {
        position.x = Random.Range(squareBoxMinX[spawnPoint], squareBoxMaxX[spawnPoint]);
        position.y = Random.Range(squareBoxMinY[spawnPoint], squareBoxMaxY[spawnPoint]);
        position.z = -2;

        randomNum = Random.Range(1, 4);

        switch(randomNum)
        {
            case 2:
                {
                    for (int x = 0; x < shootingEnemies.Count; x++)
                    {
                        if (!shootingEnemies[x].activeInHierarchy)
                        {
                            shootingEnemies[x].transform.position = position;

                            position.x = Random.Range(squareBoxMinX[assemblyPoint], squareBoxMaxX[assemblyPoint]);
                            position.y = Random.Range(squareBoxMinY[assemblyPoint], squareBoxMaxY[assemblyPoint]);

                            shootingEnemies[x].GetComponentInChildren<EnemyAI>().resetAI(position);
                            shootingEnemies[x].SetActive(true);

                            break;
                        }
                    }
                    break;
                }
            default:
                {
                    for (int x = 0; x < meleeEnemies.Count; x++)
                    {
                        if (!meleeEnemies[x].activeInHierarchy)
                        {
                            meleeEnemies[x].transform.position = position;

                            position.x = Random.Range(squareBoxMinX[assemblyPoint], squareBoxMaxX[assemblyPoint]);
                            position.y = Random.Range(squareBoxMinY[assemblyPoint], squareBoxMaxY[assemblyPoint]);

                            meleeEnemies[x].GetComponentInChildren<EnemyAI>().resetAI(position);
                            meleeEnemies[x].SetActive(true);

                            break;
                        }
                    }
                    break;
                }
        }
        
    }
}
