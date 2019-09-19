using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class GetMotionPlusTest : MonoBehaviour
{
    //GameManager
    InputManager inputManager;

    public GameObject model;

    private Quaternion initial_rotation;

    private Wiimote wiimote;

    private Vector2 scrollPosition;

    private Vector3 wmpOffset = Vector3.zero;

    void Start()
    {
        inputManager = GameManager.Instance.inputManager;

        WiimoteManager.FindWiimotes();

        initial_rotation = model.transform.localRotation;
    }

    private static void OnWiimoteConnect(Wiimote wiimote)
    {
        Debug.Log("New Wiimote has connected!  Type: " + wiimote.Type);
    }

    private void Update()
    {
        if (!WiimoteManager.HasWiimote()) { return; }

        wiimote = WiimoteManager.Wiimotes[0];

        Debug.Log(wiimote.current_ext);

        int ret;
        do
        {
            ret = wiimote.ReadWiimoteData();

            if (ret > 0 /*&& wiimote.current_ext == ExtensionController.MOTIONPLUS*/)
            {
                Vector3 offset = new Vector3(-wiimote.MotionPlus.PitchSpeed,
                                                wiimote.MotionPlus.YawSpeed,
                                                wiimote.MotionPlus.RollSpeed) / 95f; // Divide by 95Hz (average updates per second from wiimote)
                wmpOffset += offset;

                model.transform.Rotate(offset, Space.Self);
            }
        } while (ret > 0);

        GetButtons();

        MotionPlusData data = wiimote.MotionPlus;
        model.transform.rotation = Quaternion.FromToRotation(model.transform.rotation * GetAccelVector(), Vector3.up) * model.transform.rotation;
        model.transform.rotation = Quaternion.FromToRotation(model.transform.forward, Vector3.forward) * model.transform.rotation;

        /*
        if (wiimote.current_ext != ExtensionController.MOTIONPLUS)
        {
            model.transform.localRotation = initial_rotation;
        }
        else
        {
            MotionPlusData data = wiimote.MotionPlus;
            model.transform.rotation = Quaternion.FromToRotation(model.transform.rotation * GetAccelVector(), Vector3.up) * model.transform.rotation;
            model.transform.rotation = Quaternion.FromToRotation(model.transform.forward, Vector3.forward) * model.transform.rotation;
        }
        */
    }

    void GetButtons()
    {
        if(inputManager.buttonAPressed1)
        {
            Debug.Log("A pressed");
        }
    }

    private Vector3 GetAccelVector()
    {
        float accel_x;
        float accel_y;
        float accel_z;

        float[] accel = wiimote.Accel.GetCalibratedAccelData();
        accel_x = accel[0];
        accel_y = -accel[2];
        accel_z = -accel[1];

        return new Vector3(accel_x, accel_y, accel_z).normalized;
    }

    void OnApplicationQuit()
    {
        if (wiimote != null)
        {
            WiimoteManager.Cleanup(wiimote);
            wiimote = null;
        }
    }
}