using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

namespace WiimoteApi
{
    public class GetInfoFromWiiMote : MonoBehaviour
    {
        private Wiimote wiimote;

        void Update()
        {
            if (!WiimoteManager.HasWiimote()) { return; }

            wiimote = WiimoteManager.Wiimotes[0];

            if (wiimote.Button.a)
            {
                Debug.Log("Button A pressed");
            }
        }
    }
}