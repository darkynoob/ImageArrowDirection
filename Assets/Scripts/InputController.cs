using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    #region Properties
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    [SerializeField]
    private bool detectSwipeOnlyAfterRelease = false;

    [SerializeField]
    private float minDistanceForSwipe = 20f;

    public static event Action<SwipeData> OnSwipe = delegate { };

    #endregion

    #region Unity Callbacks
    private void Update()
    {
#if UNITY_IOS || UNITY_ANDROID

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPosition = touch.position;
                fingerDownPosition = touch.position;
            }

            if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();

                if (fingerUpPosition == fingerDownPosition)
                {
                    var direction = SwipeDirection.Touch;
                    SendSwipe(direction);
                }
            }
        }

#endif

#if UNITY_EDITOR

        if (Input.GetKeyUp("space"))
        {
            SendSwipe(SwipeDirection.Touch);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            SendSwipe(SwipeDirection.Right);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            SendSwipe(SwipeDirection.Left);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            SendSwipe(SwipeDirection.Down);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            SendSwipe(SwipeDirection.Up);
        }
        //if (Input.GetMouseButtonDown(0))
        if (Input.GetKey(KeyCode.Mouse0))
        {
            fingerDownPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            fingerUpPosition = fingerDownPosition;
            SendSwipe(SwipeDirection.Touch);
        }
#endif

    }
    #endregion

    #region Private Methods

    private void DetectSwipe()
    {
        if (SwipeDistanceCheckMet())
        {
            if (IsVerticalSwipe())
            {
                var direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
            }
            else
            {
                var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
            }
            fingerUpPosition = fingerDownPosition;
        }
    }

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = fingerDownPosition,
            EndPosition = fingerUpPosition
        };
        OnSwipe(swipeData);
    }

    #endregion
}

#region Structures and enums
public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right,
    Touch
}

#endregion