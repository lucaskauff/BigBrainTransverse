using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class InputManager : MonoBehaviour
{
    Wiimote wiimote1;
    //Wiimote wiimote2;

    [Header("Player 1 Buttons")]
    public bool buttonAPressed1;
    public bool buttonBPressed1;
    public bool buttonUpPressed1;
    public bool buttonRightPressed1;
    public bool buttonDownPressed1;
    public bool buttonLeftPressed1;
    public bool buttonMinusPressed1;
    public bool buttonHomePressed1;
    public bool buttonPlusPressed1;
    public bool buttonOnePressed1;
    public bool buttonTwoPressed1;

    void Start()
    {
        WiimoteManager.FindWiimotes();
    }

    void Update ()
    {
        if (!WiimoteManager.HasWiimote()) { return; }

        wiimote1 = WiimoteManager.Wiimotes[0];

        int ret;
        do
        {
            ret = wiimote1.ReadWiimoteData();

        } while (ret > 0);

        //Player 1 Buttons
        buttonAPressed1 = wiimote1.Button.a;
        buttonBPressed1 = wiimote1.Button.b;
        buttonUpPressed1 = wiimote1.Button.d_up;
        buttonRightPressed1 = wiimote1.Button.d_right;
        buttonDownPressed1 = wiimote1.Button.d_down;
        buttonLeftPressed1 = wiimote1.Button.d_left;
        buttonMinusPressed1 = wiimote1.Button.minus;
        buttonHomePressed1 = wiimote1.Button.home;
        buttonPlusPressed1 = wiimote1.Button.plus;
        buttonOnePressed1 = wiimote1.Button.one;
        buttonTwoPressed1 = wiimote1.Button.two;

        //Player 2 Buttons

    }
}