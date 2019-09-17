using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class GetInfoFromWiiMote : MonoBehaviour
{
    private Wiimote wiimote;

    void Update()
    {
        if (!WiimoteManager.HasWiimote()) { return; }

        wiimote = WiimoteManager.Wiimotes[0];

        int ret;
        do
        {
            ret = wiimote.ReadWiimoteData();
        } while (ret > 0);

        if (wiimote.Button.a)
        {
            Debug.Log("Button A pressed");
        }
    }
}