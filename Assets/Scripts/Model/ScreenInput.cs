using UnityEngine;

public class ScreenInput
{
    public enum Swipe
    {
        Up,
        Left,
        Down,
        Right,
        None
    };

    private static float sqrtTwoDivTwo = (Mathf.Sqrt(2f) / 2f);

    public static Swipe GetSwipe()
    {
        Touch[] touches = Input.touches;

        //Detect screen input.
        if (touches.Length > 0)
        {
            Vector2 deltaPos = touches[0].deltaPosition.normalized;

            if (IsUpSwipe(deltaPos))
            {
                return Swipe.Up;
            }
            else if (IsLeftSwipe(deltaPos))
            {
                return Swipe.Left;
            }
            else if (IsDownSwipe(deltaPos))
            {
                return Swipe.Down;
            }
            else if (IsRightSwipe(deltaPos))
            {
                return Swipe.Right;
            }
        } 
        //Detect keyboard input.
        else if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                return Swipe.Up;
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                return Swipe.Left;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                return Swipe.Down;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                return Swipe.Right;
            }
        }
          
        return Swipe.None;
    }

    private static bool IsUpSwipe(Vector2 deltaPos)
    {
        return (deltaPos.x > -sqrtTwoDivTwo) &&
            (deltaPos.x < sqrtTwoDivTwo) &&
            (deltaPos.y > 0) &&
            (deltaPos.y < 1f);
    }

    private static bool IsLeftSwipe(Vector2 deltaPos)
    {
        return (deltaPos.x > -1f) &&
            (deltaPos.x < 0) &&
            (deltaPos.y > -sqrtTwoDivTwo) &&
            (deltaPos.y < sqrtTwoDivTwo);
    }

    private static bool IsDownSwipe(Vector2 deltaPos)
    {
        return (deltaPos.x > -sqrtTwoDivTwo) &&
            (deltaPos.x < sqrtTwoDivTwo) &&
            (deltaPos.y > -1) &&
            (deltaPos.y < 0);
    }

    private static bool IsRightSwipe(Vector2 deltaPos)
    {
        return (deltaPos.x > 0) &&
            (deltaPos.x < 1) &&
            (deltaPos.y > -sqrtTwoDivTwo) &&
            (deltaPos.y < sqrtTwoDivTwo);
    }
}
