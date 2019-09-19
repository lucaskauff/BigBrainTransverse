using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WiimoteApi;

public class GetWiiMoteInfo : MonoBehaviour
{
    [Header("Serializable variables")]
    [SerializeField, Range(0, 1)] int player;
    [SerializeField, Range(0, 4)] int[] lumi = new int[4];

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
        initial_rotation = model.transform.localRotation;

        WiimoteManager.FindWiimotes();
    }

    private static void OnWiimoteConnect(Wiimote wiimote)
    {
        Debug.Log("New Wiimote has connected!  Type: " + wiimote.Type);
    }

    void Update()
    {
        if (!WiimoteManager.HasWiimote()) { return; }

        wiimote = WiimoteManager.Wiimotes[player];

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

                model.transform.Rotate(offset, Space.Self);
            }
        } while (ret > 0);

        GetButtons();

        if (Input.GetKey(KeyCode.Space))
        {
            FeedbackLED(lumi[0], lumi[1], lumi[2], lumi[3]);
        }
        else
        {
            FeedbackLED(0, 0, 0, 0);
        }

        /*
        if (wiimote.current_ext != ExtensionController.MOTIONPLUS)
            model.transform.localRotation = initial_rotation;
        */
        
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

        GetAxe();

        MotionPlus();
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

    void FeedbackLED(int intens0, int intens1, int intens2, int intens3)
    {
        for (int x = 0; x < 4; x++)
            wiimote.SendPlayerLED(x == intens0, x == intens1, x == intens2, x == intens3);
    }

    void GetAxe()
    {
        float[] pointer = wiimote.Ir.GetPointingPosition();
        
        ir_pointer.anchorMin = new Vector2(pointer[0], pointer[1]);
        ir_pointer.anchorMax = new Vector2(pointer[0], pointer[1]);

        Debug.Log(pointer[0].ToString());
        Debug.Log(pointer[1].ToString());        
    }

    void MotionPlus()
    {
        model.transform.rotation = Quaternion.FromToRotation(model.transform.rotation * GetAccelVector(), Vector3.up) * model.transform.rotation;
        model.transform.rotation = Quaternion.FromToRotation(model.transform.forward, Vector3.forward) * model.transform.rotation;
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