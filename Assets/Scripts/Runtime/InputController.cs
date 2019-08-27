using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hexic.Runtime
{
    public class InputController : MonoBehaviour
    {

        public enum SwipeType
        {
            None,
            Up,
            Down,
            Right,
                Left
        }

        public static InputController _instance //Singleton here
        {
            get
            {
                return FindObjectOfType<InputController>();
            }
            set
            {
                _instance = FindObjectOfType<InputController>();
            }
        }
        public bool GetScreenTouch(out Vector3 position)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount > 0)
                {
                    for (int i = 0; i < Input.touchCount; ++i)
                    {
                        Touch touch = Input.GetTouch(i);
                        if (touch.phase == TouchPhase.Began)
                        {
                            position = Camera.main.WorldToScreenPoint(touch.position);
                            return true;
                        }
                    }
                }

            }

            if (Input.GetButtonDown("Fire1"))
            {

                position = Input.mousePosition;
                return true;

            }



            position = Vector3.zero;
            return false;

        }

        Vector2 firstPressPos;
        Vector2 secondPressPos;
        Vector2 currentSwipe;

        public SwipeType GetScreenSwipe()
        {
            if (Input.touches.Length > 0)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)
                {
                    //save began touch 2d point
                    firstPressPos = new Vector2(t.position.x, t.position.y);
                }
                if (t.phase == TouchPhase.Ended)
                {
                    //save ended touch 2d point
                    secondPressPos = new Vector2(t.position.x, t.position.y);

                    //create vector from the two points
                    currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                    //normalize the 2d vector
                    currentSwipe.Normalize();

                    //swipe upwards
                    if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                        Debug.Log("up swipe");
                        return SwipeType.Up;
                    }
                    //swipe down
                    if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                   {
                        Debug.Log("down swipe");
                        return SwipeType.Down;

                    }
                    //swipe left
                    if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
             {
                        Debug.Log("left swipe");
                        return SwipeType.Left;

                    }
                    //swipe right
                    if (currentSwipe.x > 0 &&  currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
             {
                        Debug.Log("right swipe");
                        return SwipeType.Right;

                    }
                }
                return SwipeType.None;

            }

            if (Input.GetMouseButtonDown(0))
            {
                //save began touch 2d point
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
            if (Input.GetMouseButtonUp(0))
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                //create vector from the two points
                currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                //normalize the 2d vector
                currentSwipe.Normalize();

                //swipe upwards
                //swipe upwards
                if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    Debug.Log("up swipe");
                    return SwipeType.Up;
                }
                //swipe down
                if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    Debug.Log("down swipe");
                    return SwipeType.Down;

                }
                //swipe left
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    Debug.Log("left swipe");
                    return SwipeType.Left;

                }
                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    Debug.Log("right swipe");
                    return SwipeType.Right;

                }
            }
            return SwipeType.None;

        }
    }


}