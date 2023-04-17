using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    private GameObject player;       //Public variable to store a reference to the player game object


    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    [System.NonSerialized]
    public bool cameraFollow = false;

    // Use this for initialization
    void Start()
    {

        player = StartupConfig.player;
    }


    public void SetUpCamFollow()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
        cameraFollow = true;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if (cameraFollow)
        {// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
            transform.position = player.transform.position + offset;
        }
    }
}
