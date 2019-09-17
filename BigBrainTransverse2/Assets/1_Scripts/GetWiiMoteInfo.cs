﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class GetWiiMoteInfo : MonoBehaviour
{
    public GameObject model;

    public RectTransform[] ir_dots;
    public RectTransform[] ir_bb;
    public RectTransform ir_pointer;

    private Quaternion initial_rotation;

    private Wiimote wiimote;

    private Vector2 scrollPosition;

    private Vector3 wmpOffset = Vector3.zero;

    private void Start()
    {
        //initial_rotation = model.transform.localRotation;

        WiimoteManager.FindWiimotes();
    }

    private static void OnWiimoteConnect(Wiimote wiimote)
    {
        Debug.Log("New Wiimote has connected!  Type: " + wiimote.Type);
    }

    void Update()
    {
        if (!WiimoteManager.HasWiimote()) { return; }

        wiimote = WiimoteManager.Wiimotes[0];

        int ret;
        do
        {
            ret = wiimote.ReadWiimoteData();

            if (ret > 0 && wiimote.current_ext == ExtensionController.MOTIONPLUS)
            {
                Vector3 offset = new Vector3(-wiimote.MotionPlus.PitchSpeed,
                                                wiimote.MotionPlus.YawSpeed,
                                                wiimote.MotionPlus.RollSpeed) / 95f; // Divide by 95Hz (average updates per second from wiimote)
                wmpOffset += offset;
            }
        } while (ret > 0);

        GetButtons();

        if (ir_dots.Length < 4) return;

        float[,] ir = wiimote.Ir.GetProbableSensorBarIR();
        for (int i = 0; i < 2; i++)
        {
            float x = (float)ir[i, 0] / 1023f;
            float y = (float)ir[i, 1] / 767f;
            if (x < 0 || y < 0)
            {
                ir_dots[i].anchorMin = new Vector2(0, 0);
                ir_dots[i].anchorMax = new Vector2(0, 0);
            }

            ir_dots[i].anchorMin = new Vector2(x, y);
            ir_dots[i].anchorMax = new Vector2(x, y);

            if (ir[i, 2] < 0)
            {
                int index = (int)ir[i, 2];
                float xmin = (float)wiimote.Ir.ir[index, 3] / 127f;
                float ymin = (float)wiimote.Ir.ir[index, 4] / 127f;
                float xmax = (float)wiimote.Ir.ir[index, 5] / 127f;
                float ymax = (float)wiimote.Ir.ir[index, 6] / 127f;
                ir_bb[i].anchorMin = new Vector2(xmin, ymin);
                ir_bb[i].anchorMax = new Vector2(xmax, ymax);
            }
        }


        float[] pointer = wiimote.Ir.GetPointingPosition();
        ir_pointer.anchorMin = new Vector2(pointer[0], pointer[1]);
        ir_pointer.anchorMax = new Vector2(pointer[0], pointer[1]);
    }

    void GetButtons()
    {
        if (wiimote.Button.a)
        {
            Debug.Log("A pressed");
        }

        if (wiimote.Button.b)
        {
            Debug.Log("B pressed");
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
}