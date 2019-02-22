using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    Vector2 touchStartPos;
    Vector2 touchEndPos;
    bool directionChosen;
    
    public float zoomSpeed = 0.05f;
    public float panSpeed = 5f;
    public float[] zoomClamp = { 5.0f, 20.0f };
    Vector3 zoomOutCamPos = Vector3.zero;

    Camera cam;
    
    GameController gC;
    
    bool canMove = true;
    

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        gC = GetComponent<GameController>();
        zoomOutCamPos = cam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            if (canMove)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    touchStartPos = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    touchEndPos = touch.position;
                    directionChosen = true;
                }
            }
            else
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    canMove = true;
            }
        }
        else if (Input.touchCount == 2)
        {
            canMove = false;

            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            
            cam.orthographicSize += deltaMagnitudeDiff * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, zoomClamp[0], zoomClamp[1]);                
            
        }

        if (cam.orthographicSize < zoomClamp[1] - ((zoomClamp[1] - zoomClamp[0]) * 0.25))
        {
            Vector3 playerPos = gC.player.transform.position;
            cam.transform.position = Vector3.Slerp(cam.transform.position, new Vector3(playerPos.x, playerPos.y, cam.transform.position.z), 0.2f);
        }
        else
        {
            cam.transform.position = Vector3.Slerp(cam.transform.position, zoomOutCamPos, 0.2f);
        }

        if (directionChosen)
        {
            Vector2 direction = Vector2.zero;

            if (Mathf.Abs(touchStartPos.x - touchEndPos.x) > Screen.width / 6 || Mathf.Abs(touchStartPos.y - touchEndPos.y) > Screen.height / 6)
            {
                if (Mathf.Abs(touchEndPos.y - touchStartPos.y) > Mathf.Abs(touchEndPos.x - touchStartPos.x))
                {
                    if (touchEndPos.y > touchStartPos.y)
                    {
                        direction = Vector2.up;
                    }
                    else
                    {
                        direction = Vector2.down;
                    }
                }
                else if (Mathf.Abs(touchEndPos.y - touchStartPos.y) < Mathf.Abs(touchEndPos.x - touchStartPos.x))
                {
                    if (touchEndPos.x > touchStartPos.x)
                    {
                        direction = Vector2.right;
                    }
                    else
                    {
                        direction = Vector2.left;
                    }
                }
            }

            gC.ChosenDirection(direction);

            directionChosen = false;
        }
    }
}
