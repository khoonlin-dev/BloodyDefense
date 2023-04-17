using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Transform))]

public class HealthBar : MonoBehaviour
{

    private Gradient barGradient = new Gradient(); 
    private GradientColorKey[] colorKey;
    private GradientAlphaKey[] alphaKey;

    private SpriteRenderer spriteRenderer;

    public Color lowHPColour;
    public Color fullHPColour;

    public GameObject parentObject;

    private Vector3 vector3Container;

    //To obtain colour at a time, use: gradient.Evaluate(0.0f~1.0f);

    private float currentFraction = 1.0f;


    private Color spriteColor;


    void Awake()
    {

        vector3Container = new Vector3();

        currentFraction = 1.0f;

        colorKey = new GradientColorKey[2];
        colorKey[0].color = lowHPColour;
        colorKey[0].time = 0.0f;
        colorKey[1].color = fullHPColour;
        colorKey[1].time = 1.0f;

        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;

        barGradient.SetKeys(colorKey, alphaKey);

        spriteRenderer = this.GetComponent<SpriteRenderer>();

        spriteRenderer.color = fullHPColour;

    }

	// Use this for initialization
	void OnEnable () {

        //-------Initialize health bar with values different than FULL BAR-----------

        if (parentObject.CompareTag("Ally") || parentObject.CompareTag("Player") || parentObject.CompareTag("Objective"))
        {

                 //Debug.Log("HP current is : " + parentObject.GetComponent<PlayerStatistics>().playersCurrentHealthPoint + ", max health point : " + parentObject.GetComponent<PlayerStatistics>().maxHealthPoint);
            updateBar(parentObject.GetComponent<PlayerStatistics>().playersCurrentHealthPoint, parentObject.GetComponent<PlayerStatistics>().maxHealthPoint);
        
        }

        else
        {
            if (parentObject.GetComponent<Enemy>()!=null)
                updateBar(parentObject.GetComponent<Enemy>().currentHP, parentObject.GetComponent<Enemy>().maxHP);

            spriteColor = this.GetComponent<SpriteRenderer>().color;
            spriteColor.a = 0;
            this.GetComponent<SpriteRenderer>().color = spriteColor;
        }

	}
	
	// Update is called once per frame
	void Update () {
        if (this.GetComponent<SpriteRenderer>().color.a < 1)
        {
            spriteColor = this.GetComponent<SpriteRenderer>().color;
            spriteColor.a += 1.0f * Time.deltaTime;
            if (spriteColor.a > 1)
            {
                spriteColor.a = 1;
            }
            this.GetComponent<SpriteRenderer>().color = spriteColor;
        }
	}

    public void updateBar(float currentValue, float maxValue)
    {

        if(currentValue > maxValue)
        {
            currentValue = maxValue;
        }

        if (currentValue < 0.0f)
        {
            currentValue = 0.0f;
        }

        currentFraction = currentValue / maxValue;


        //Debug.Log("Bar Gradient . Evaluate (0.34) = " + barGradient.Evaluate(0.34f));

        //Debug.Log("I have updated HP with current fraction of " + currentFraction + " and color of " + barGradient.Evaluate(currentFraction));

        this.GetComponent<SpriteRenderer>().color = barGradient.Evaluate(currentFraction);


        vector3Container = transform.parent.gameObject.GetComponent<Transform>().transform.localScale;

        if (vector3Container.x < 0)
        {
            vector3Container.x = -(currentFraction);
        }
        else
        {
            vector3Container.x = currentFraction;
        }

        transform.parent.gameObject.GetComponent<Transform>().transform.localScale = vector3Container;
            

    }
}
