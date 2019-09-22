using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInputManager : MonoBehaviour
{
    //Mouse clicks
    public bool leftClick;
    public bool mouseWheelClick;
    public bool rightClick;

	void Update ()
    {
        //Mouse clicks
        leftClick = Input.GetMouseButton(0);
        rightClick = Input.GetMouseButton(1); ;
        mouseWheelClick = Input.GetMouseButton(2);
    }
}