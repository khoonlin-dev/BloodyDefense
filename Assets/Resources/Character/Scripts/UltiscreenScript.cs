using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltiscreenScript : MonoBehaviour {

    private Color color;

    private Color colorContainer;

    private bool isIncreasing;

	// Use this for initialization
	void Start () {
        isIncreasing = true;
        colorContainer = new Color();
        color = this.GetComponent<SpriteRenderer>().color;

        //Debug.Log(color);

	}
	
	// Update is called once per frame
	void Update () 
    {

        //Debug.Log("color : " + color);

        //Debug.Log("this.GetComponent<SpriteRenderer>().color : " + this.GetComponent<SpriteRenderer>().color);

        if (isIncreasing)
		{
            if(color.a <0.7f)
            {
                colorContainer = color;

                colorContainer.a += 0.75f * Time.deltaTime;

                color = colorContainer;

                if(color.a >= 0.7f)
                {
                    colorContainer = color;

                    colorContainer.a = 0.7f;

                    color = colorContainer;

                    isIncreasing = false;
                }

                this.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, colorContainer.a);
            }

        }
        else
        {
            if(color.a > 0)
            {
                colorContainer = color;

                colorContainer.a -= 0.75f * Time.deltaTime;
                color = colorContainer;
                if (color.a <= 0)
                {
                    colorContainer = color;

                    colorContainer.a = 0.0f;

                    color = colorContainer;
                    isIncreasing = true;
                }

                this.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, colorContainer.a);
            }
        }
	}
}
