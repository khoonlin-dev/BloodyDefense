using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Credit to https://docs.unity3d.com/ScriptReference/Screen-orientation.html

public class StartupConfig : MonoBehaviour {

    //This script handle startup of a level

    public static GameObject player;

    public VJHandler jsMovement;

    public GameObject normalAttackButton;
    public GameObject skillOneButton;
    public GameObject vpadButton;
    public GameObject rightVpadButton;
    public GameObject cooldownOneButton;
    public GameObject errorText;
    public GameObject skillTwoButton;

    public GameObject durationBar;

    public GameObject Neutrophil;
    public GameObject Macrophage;
    public GameObject Killer;
    public GameObject Helper;

    public GameObject ultiScreen;


    public ExplosionPoolObject explosionPool;

    public static ExplosionPoolObject explosion;

    public List<Sprite> heroesSprite;

    public List<string> heroesName;

    public List<string> heroesDetail;

    public WinningScript endGame;

    [System.NonSerialized]
    public static List<GameObject> activeAllyOnMap = new List<GameObject>();

    [System.NonSerialized]
    public static List<GameObject> allGoodGuysOnMap = new List<GameObject>();

    [System.NonSerialized]
    public static List<GameObject> allObjectiveObject = new List<GameObject>();

    [System.NonSerialized]
    public static List<GameObject> enemyChasingCharacter = new List<GameObject>();  //The recalculate function of each member in this list will be invoked
                                                                                    //If a new player or ally is instantiated in the scene

    [System.NonSerialized]
    public static List<GameObject> enemyChasingObjective = new List<GameObject>();  //The recalculate function of each member in this list will be invoked
                                                                                    //If a new objective item is instantiated in the scene


	// Use this for initialization

    [System.NonSerialized]
    public static BulletPoolBehavior bulletPool;

    [System.NonSerialized]
    public static BulletPoolBehavior enemyBulletPool;

    [System.NonSerialized]
    public static AllyPool allyPool;

    [System.NonSerialized]
    public static int objectiveCount = 3;

    [System.NonSerialized]
    public static bool isWon = false;

    [System.NonSerialized]
    public static bool isLose = false;

	void Awake () {

        explosion = explosionPool;

        objectiveCount = 3;

        bulletPool = GameObject.Find("BulletPool").GetComponent<BulletPoolBehavior>();

        enemyBulletPool = GameObject.Find("EnemyBulletPool").GetComponent<BulletPoolBehavior>();

        allyPool = GameObject.Find("AllyPool").GetComponent<AllyPool>();

        switch (StaticDataHandler.characterChosen)
        {
            case 0:
                {
                    Debug.Log("Error, player is not set");
                    break;
                }
            case 1:
                {
                    //player = Neutrophil;
                    //Destroy(Macrophage);
                    //Destroy(Killer);
                    //Destroy(Helper);
                    
                    player = (GameObject)Instantiate(Neutrophil);
                    //Debug.Log("I am instantiated!");
                    player.name = "Neutrophil";
                    player.GetComponent<PlayerStatistics>().errorText = errorText;
                    player.GetComponent<PlayerStatistics>().durationBar = durationBar;
                    player.GetComponent<Transform>().SetSiblingIndex(0);
                    player.GetComponent<MovePlayers>().jsMovement = jsMovement;
                    player.GetComponent<PlayerStatistics>().normalAttackButton = normalAttackButton;
                    player.GetComponent<PlayerStatistics>().skillOneButton = skillOneButton;
                    player.GetComponent<PlayerStatistics>().vpadButton = vpadButton;
                    player.GetComponent<PlayerStatistics>().cooldownOneButton = cooldownOneButton;
                    player.GetComponent<PlayerStatistics>().endGame = endGame;

                    player.GetComponent<SwipePlayer>().ultiScreen = ultiScreen;

                    break;
                }
            case 2:
                {

                    player = (GameObject)Instantiate(Macrophage);
                    //Debug.Log("I am instantiated!");
                    player.name = "Macrophage";
                    player.GetComponent<PlayerStatistics>().errorText = errorText;
                    player.GetComponent<PlayerStatistics>().durationBar = durationBar;
                    player.GetComponent<Transform>().SetSiblingIndex(0);
                    player.GetComponent<MovePlayers>().jsMovement = jsMovement;
                    player.GetComponent<PlayerStatistics>().normalAttackButton = normalAttackButton;
                    player.GetComponent<PlayerStatistics>().skillOneButton = skillOneButton;
                    player.GetComponent<PlayerStatistics>().vpadButton = vpadButton;
                    player.GetComponent<PlayerStatistics>().cooldownOneButton = cooldownOneButton;
                    player.GetComponent<PlayerStatistics>().endGame = endGame;


                    break;
                }
            case 3:
                {
                    //player = Killer;
                    //Destroy(Macrophage);
                    //Destroy(Neutrophil);
                    //Destroy(Helper);
                    player = (GameObject)Instantiate(Killer);
                    //Debug.Log("I am instantiated!");
                    player.name = "Killer";
                    player.GetComponent<PlayerStatistics>().errorText = errorText;
                    player.GetComponent<PlayerStatistics>().durationBar = durationBar;
                    player.GetComponent<Transform>().SetSiblingIndex(0);
                    player.GetComponent<MovePlayers>().jsMovement = jsMovement;
                    player.GetComponent<PlayerStatistics>().normalAttackButton = normalAttackButton;
                    player.GetComponent<PlayerStatistics>().skillOneButton = skillOneButton;
                    player.GetComponent<PlayerStatistics>().vpadButton = vpadButton;
                    player.GetComponent<PlayerStatistics>().cooldownOneButton = cooldownOneButton;
                    player.GetComponent<PlayerStatistics>().endGame = endGame;
                    player.GetComponent<PlayerStatistics>().rightVpadButton = rightVpadButton;
                    break;
                }
            case 4:
                {

                    Debug.Log(jsMovement);
                    //player = Helper;
                    //Destroy(Macrophage);
                    //Destroy(Killer);
                    //Destroy(Neutrophil);
                    player = (GameObject)Instantiate(Helper);
                    //Debug.Log("I am instantiated!");
                    player.name = "Helper";
                    player.GetComponent<PlayerStatistics>().errorText = errorText;
                    player.GetComponent<PlayerStatistics>().durationBar = durationBar;
                    player.GetComponent<Transform>().SetSiblingIndex(0);
                    player.GetComponent<MovePlayers>().jsMovement = jsMovement;
                    player.GetComponent<PlayerStatistics>().normalAttackButton = normalAttackButton;
                    player.GetComponent<PlayerStatistics>().skillOneButton = skillOneButton;
                    player.GetComponent<PlayerStatistics>().vpadButton = vpadButton;
                    player.GetComponent<PlayerStatistics>().cooldownOneButton = cooldownOneButton;
                    player.GetComponent<PlayerStatistics>().endGame = endGame;
                    player.GetComponent<PlayerStatistics>().skillTwoButton = skillTwoButton;

                    break;
                }
        }

	}
	

    void Start()
    {
        if (StaticDataHandler.characterChosen == 2)
            player.GetComponentInChildren<PlayerMelee>().ultiScreen = ultiScreen;
        //player.GetComponent<PlayerStatistics>().explosionPool = explosionPool;

    }

	// Update is called once per frame
	void Update () {
		
	}
}
