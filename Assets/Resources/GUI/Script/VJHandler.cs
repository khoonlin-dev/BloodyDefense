using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Credit to http://www.theappguruz.com/blog/beginners-guide-learn-to-make-simple-virtual-joystick-in-unity

public class VJHandler : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RawImage jsContainer;
    private RawImage joystick;

    public Vector3 InputDirection;


    void Start()
    {

        jsContainer = GetComponent<RawImage>();
        joystick = transform.GetChild(0).GetComponent<RawImage>(); //this command is used because there is only one child in hierarchy
        InputDirection = Vector3.zero;
    }

    public void OnDrag(PointerEventData ped)
    {

        //Debug.Log(ped.position);

        Vector2 position = Vector2.zero;

        //Vector2 position = new Vector2(jsContainer.rectTransform.rect.height/2, jsContainer.rectTransform.rect.width/2);

        //To get InputDirection
        RectTransformUtility.ScreenPointToLocalPointInRectangle
                (jsContainer.rectTransform,
                ped.position,
                ped.pressEventCamera,
                out position);

        position.x = (position.x / jsContainer.rectTransform.sizeDelta.x);
        position.y = (position.y / jsContainer.rectTransform.sizeDelta.y);

        float x = (jsContainer.rectTransform.pivot.x == 1f) ? position.x * 2 + 1 : position.x * 2 - 1;
        float y = (jsContainer.rectTransform.pivot.y == 1f) ? position.y * 2 + 1 : position.y * 2 - 1;

        InputDirection = new Vector3(x, y, 0);
        InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;

        //Debug.Log(InputDirection);

        //to define the area in which joystick can move around
        joystick.rectTransform.anchoredPosition = new Vector3(InputDirection.x * (jsContainer.rectTransform.sizeDelta.x / 3)
                                                               , InputDirection.y * (jsContainer.rectTransform.sizeDelta.y) / 3);

        //Debug.Log(joystick.rectTransform.anchoredPosition);

    }

    public void OnPointerDown(PointerEventData ped)
    {

        OnDrag(ped);
    }

    public void OnPointerUp(PointerEventData ped)
    {
        //Debug.Log("LOL");
        InputDirection = Vector3.zero;
        joystick.rectTransform.anchoredPosition = Vector2.zero;
    }

    void OnEnable()
    {
        InputDirection = Vector3.zero;
        if(joystick!=null)
            joystick.rectTransform.anchoredPosition = Vector3.zero;
    }
}