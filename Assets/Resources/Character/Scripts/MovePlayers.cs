using UnityEngine;
using System.Collections;

public class MovePlayers : MonoBehaviour
{

    private string wallTag = "Wall";

    public float moveSpeed = 05f;
    public VJHandler jsMovement;

    public GameObject player;
    public PlayerStatistics playerStats;

    private GameObject sword;

    private GameObject helpersKit;

    private Vector3 direction;

    public PlayerMelee playerMelee;

    private CapsuleCollider2D collider;

    private int isWalkingHash;

    private Vector2 vector2PlaceHolder;

    private Vector3 vector3PlaceHolder;

    private RaycastHit2D hitLeft;
    private RaycastHit2D hitRight;
    private RaycastHit2D hitUp;
    private RaycastHit2D hitDown;

    private RaycastHit2D hitFacedDirection;

    [System.NonSerialized]
    public float moveLeftPermission = 1.0f;
    [System.NonSerialized]
    public float moveRightPermission = 1.0f;
    [System.NonSerialized]
    public float moveUpPermission = 1.0f;
    [System.NonSerialized]
    public float moveDownPermission = 1.0f;

    private int layerMask;

    public GameObject minimapSprite;


    public GameObject mapIdentifier;

    //void OnCollisionEnter2D(Collision2D col)
    //{

    //    previousPosition -= (Vector2)direction * moveSpeed * Time.deltaTime;

    //    if(col.gameObject.CompareTag("Wall"))
    //    {   
    //        Debug.Log("Hi, someone collided me~");
    //        lockMovement = true;

    //    }
    //}

    //void OnCollisionStay2D(Collision2D col)
    //{


    //    if (col.gameObject.CompareTag("Wall"))
    //    {
    //        transform.position = previousPosition;
    //    }
    //}

    //void OnCollisionExit2D(Collision2D col)
    //{
    //    if (col.gameObject.CompareTag("Wall"))
    //    {
    //        lockMovement = false;
    //    }
    //}
    void Start()
    {
        layerMask = LayerMask.GetMask("Wall");


        vector2PlaceHolder = new Vector2();

        vector3PlaceHolder = new Vector3();

        player = this.transform.gameObject;

        direction = new Vector3();

        isWalkingHash = Animator.StringToHash("isWalking");  //I use hash id to access the parameters of the animator (i don't want to use name string becoz it's too tedious to check it every frame)

        if (player != null)
        {

            playerStats = player.GetComponent<PlayerStatistics>();

            //Debug.Log(playerStats);

            collider = this.transform.gameObject.GetComponentInChildren<CapsuleCollider2D>();

            if (StaticDataHandler.characterChosen == 1)    //If neutrophil is chosen
            {
                sword = player.transform.Find("Sword").gameObject;
            }
            else if (StaticDataHandler.characterChosen == 4) //If helper t cell is chosen
            {
                helpersKit = player.transform.Find("Kit").gameObject;
            }
        }



    }

    void FixedUpdate()
    {
        moveRightPermission = 1.0f;
        moveLeftPermission = 1.0f;
        moveUpPermission = 1.0f;
        moveDownPermission = 1.0f;

        hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.4f, layerMask);

        if (hitLeft.collider!=null)
        {
            if (hitLeft.collider.CompareTag(wallTag))
            {
                //Debug.Log("Left hit!");

                moveLeftPermission = 0.0f;
            }
        }


        hitRight = Physics2D.Raycast(transform.position, Vector2.right, 1.0f, layerMask);
        if (hitRight.collider != null)
        {
            if (hitRight.collider.CompareTag(wallTag))
            {

                //Debug.Log("Right hit!");

                moveRightPermission = 0.0f;
            }
        }


        hitUp = Physics2D.Raycast(transform.position, Vector2.up, 1.0f, layerMask);


        if (hitUp.collider != null)
        {

            if (hitUp.collider.CompareTag(wallTag))
            {



                //Debug.Log("Up hit!");
                moveUpPermission = 0.0f;
            }

        }


        hitDown = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, layerMask);
        if (hitDown.collider!=null)
        {
            if (hitDown.collider.CompareTag(wallTag))
            {


                //Debug.Log("Down hit!");

                moveDownPermission = 0.0f;
            }

        }



        if (moveRightPermission == 1.0f && moveLeftPermission == 1.0f && moveUpPermission == 1.0f && moveDownPermission == 1.0f)
        {
            hitFacedDirection = Physics2D.Raycast(transform.position, jsMovement.InputDirection, 1.0f, layerMask);

            if (hitFacedDirection.collider != null && hitFacedDirection.collider.CompareTag(wallTag))
            {

                    if (jsMovement.InputDirection.x > 0)
                    {

                        moveRightPermission = 0.0f;
                    }
                    else
                    {
                        moveLeftPermission = 0.0f;
                    }

                    if (jsMovement.InputDirection.y > 0)
                    {
                        moveUpPermission = 0.0f;
                    }
                    else
                    {
                        moveDownPermission = 0.0f;
                    }

            }
        }


    }

    void Update()
    {

        direction = jsMovement.InputDirection; //InputDirection can be used as per the need of your project



        if ( direction.magnitude != 0 && !player.GetComponent<SwipePlayer>().isDashableState && playerStats.vpadButton.GetComponent<VJHandler>().enabled)
        {

   

            if (playerMelee.isHealing)
            {
                playerMelee.stopNormalHealing();
            }

            //Debug.Log(direction * moveSpeed * Time.deltaTime);


            //this.GetComponent<Rigidbody2D>().AddForce(direction * moveSpeed);


                if (direction.x >= 0)
                {
                    direction.x *= moveRightPermission;
                }
                else
                {
                    direction.x *= moveLeftPermission;
                }

                if (direction.y >= 0)
                {
                    direction.y *= moveUpPermission;
                }
                else
                {
                    direction.y *= moveDownPermission;
                }

                this.GetComponent<Rigidbody2D>().velocity = direction * moveSpeed;

            //this.GetComponent<Rigidbody2D>().MovePosition(transform.position += direction * moveSpeed * Time.deltaTime);

            //transform.position += direction * moveSpeed * Time.deltaTime;
            //transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), Mathf.Clamp(transform.position.y, yMin, yMax), 0f);//to restric movement of player

            playerStats.playerDirection = direction;

            //Set state to walking if current state is not:

            if (!playerStats.animator.GetBool(isWalkingHash))    //isWalkingHash is id for isWalking. I could've checked with name for precision but it will be tedious as this line will be runned per frame update
            {
                playerStats.animator.SetBool(isWalkingHash, true);
            }

            //Debug.Log(direction);

            if (direction.x <= 0)
            {

                minimapSprite.transform.localRotation = Quaternion.Euler(0, 0, ((-(Mathf.Atan2(playerStats.playerDirection.x, playerStats.playerDirection.y)) * 180 / Mathf.PI) + 90) - 180);


                if (StaticDataHandler.characterChosen != 2 && StaticDataHandler.characterChosen != 3) //If macrophage and killer is not selected
                {


                    if (collider.offset.x > 0)
                    {

                        vector2PlaceHolder.x = -collider.offset.x;

                        vector2PlaceHolder.y = collider.offset.y;

                        collider.offset = vector2PlaceHolder;

                        if (StaticDataHandler.characterChosen == 1)
                        {
                  

                            vector3PlaceHolder.x = -0.3f;

                            vector3PlaceHolder.y = -1.0f;

                            vector3PlaceHolder.z = -5.0f;

                            sword.transform.localPosition = vector3PlaceHolder;

                            vector3PlaceHolder = Vector3.zero;

                            vector3PlaceHolder.z = 135;

                            sword.transform.eulerAngles = vector3PlaceHolder;
                        }

                        else if (StaticDataHandler.characterChosen == 4)
                        {
                            vector3PlaceHolder.x = -0.8f;

                            vector3PlaceHolder.y = -0.7f;

                            vector3PlaceHolder.z = -5.0f;

                            helpersKit.transform.localPosition = vector3PlaceHolder;


                            if (playerStats.SummonBoxes != null)
                                {
                                    vector3PlaceHolder = playerStats.SummonBoxes.transform.localScale;
                                    vector3PlaceHolder.x = -vector3PlaceHolder.x;

                                    playerStats.SummonBoxes.transform.localScale = vector3PlaceHolder;
                                }

                        }
                    }
                }       //End of if macrophage and killer is not selected
                else
                {



                    if (transform.localScale.x < 0)
                    {
                        playerStats.flipPlayer();

                        if (mapIdentifier.transform.localScale.x < 0)
                        {
                            vector3PlaceHolder = mapIdentifier.transform.localScale;

                            vector3PlaceHolder.x = -vector3PlaceHolder.x;

                            mapIdentifier.transform.localScale = vector3PlaceHolder;

                            vector3PlaceHolder = mapIdentifier.transform.localPosition;

                            vector3PlaceHolder.x = -vector3PlaceHolder.x;

                            mapIdentifier.transform.localPosition = vector3PlaceHolder;
                        }
                    }


                }
            }

            else if (direction.x > 0)
            {

                if (StaticDataHandler.characterChosen == 1 || StaticDataHandler.characterChosen == 4)
                {
                    minimapSprite.transform.localRotation = Quaternion.Euler(0, 0, ((-(Mathf.Atan2(playerStats.playerDirection.x, playerStats.playerDirection.y)) * 180 / Mathf.PI) + 90) - 180);
                }
                else
                {
                    minimapSprite.transform.localRotation = Quaternion.Euler(0, 0, -((-(Mathf.Atan2(playerStats.playerDirection.x, playerStats.playerDirection.y)) * 180 / Mathf.PI) + 90));

                    if (mapIdentifier.transform.localScale.x > 0)
                    {


                        vector3PlaceHolder = mapIdentifier.transform.localScale;

                        vector3PlaceHolder.x = -vector3PlaceHolder.x;

                        mapIdentifier.transform.localScale = vector3PlaceHolder;

                        vector3PlaceHolder = mapIdentifier.transform.localPosition;

                        vector3PlaceHolder.x = -vector3PlaceHolder.x;

                        mapIdentifier.transform.localPosition = vector3PlaceHolder;

                    }
                }

                if (StaticDataHandler.characterChosen != 2 && StaticDataHandler.characterChosen != 3) //If macrophage and killer is not selected
                {
                    

                    if (collider.offset.x < 0)
                    {

                        vector2PlaceHolder.x = -collider.offset.x;

                        vector2PlaceHolder.y = collider.offset.y;

                        collider.offset = vector2PlaceHolder;

                        if (StaticDataHandler.characterChosen == 1)
                        {
                            vector3PlaceHolder.x = 0.3f;

                            vector3PlaceHolder.y = -1.0f;

                            vector3PlaceHolder.z = -5.0f;

                            sword.transform.localPosition = vector3PlaceHolder;

                            vector3PlaceHolder = Vector3.zero;

                            vector3PlaceHolder.z = 45;

                            sword.transform.eulerAngles = vector3PlaceHolder;

                        }

                        else if (StaticDataHandler.characterChosen == 4)
                        {
                            vector3PlaceHolder.x = 0.8f;

                            vector3PlaceHolder.y = -0.7f;

                            vector3PlaceHolder.z = -5.0f;

                            helpersKit.transform.localPosition = vector3PlaceHolder;



                            if (playerStats.SummonBoxes != null)
                            {
                                vector3PlaceHolder = playerStats.SummonBoxes.transform.localScale;
                                vector3PlaceHolder.x = -vector3PlaceHolder.x;

                                playerStats.SummonBoxes.transform.localScale = vector3PlaceHolder;
                            }
                        }
                    }
                }   //End of if macrophage and killer is not selected
                else
                {
                    if (transform.localScale.x > 0)
                    {
                        playerStats.flipPlayer();
                    }

                }
            }
        }

        else
        {
            if (playerStats != null)
            {
                if (playerStats.animator != null)
                {
                    if (playerStats.animator.GetBool(isWalkingHash))   //isWalkingHash is id for isWalking. I could've checked with name for precision but it will be tedious as this line will be runned per frame update
                    {

                        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                        playerStats.animator.SetBool(isWalkingHash, false);
                    }
                }
            }
        }
    }

}