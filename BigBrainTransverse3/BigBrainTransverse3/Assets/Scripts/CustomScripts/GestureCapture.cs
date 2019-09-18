using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestureCapture : MonoBehaviour
{
    public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
    public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint1 = KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
    public GameObject objectToMove;
    public GameObject objectToMove1;

    public float smoothFactor = 5f;

    private float distanceToCamera = 10f;

    public RawImage bG;

    void Start()
    {
        if (objectToMove)
        {
            distanceToCamera = (objectToMove.transform.position - Camera.main.transform.position).magnitude;
        }
    }

    private void Update()
    {
        KinectManager manager = KinectManager.Instance;

        if (manager && manager.IsInitialized())
        {
            if (bG && bG.texture == null)
            {
                bG.texture = manager.GetUsersClrTex();
            }

            int iJointIndex = (int)TrackedJoint;
            int iJointIndex1 = (int)TrackedJoint1;            

            if (manager.IsUserDetected())
            {
                uint userId = manager.GetPlayer1ID();

                if (manager.IsJointTracked(userId, iJointIndex) && manager.IsJointTracked(userId, iJointIndex1))
                {
                    Vector3 posJoint = manager.GetRawSkeletonJointPos(userId, iJointIndex);
                    Vector3 posJoint1 = manager.GetRawSkeletonJointPos(userId, iJointIndex1);

                    if (posJoint != Vector3.zero && posJoint1 != Vector3.zero)
                    {
                        // 3d position to depth
                        Vector2 posDepth = manager.GetDepthMapPosForJointPos(posJoint);
                        Vector2 posDepth1 = manager.GetDepthMapPosForJointPos(posJoint1);

                        // depth pos to color pos
                        Vector2 posColor = manager.GetColorMapPosForDepthPos(posDepth);
                        Vector2 posColor1 = manager.GetColorMapPosForDepthPos(posDepth1);

                        float scaleX = posColor.x / KinectWrapper.Constants.ColorImageWidth;
                        float scaleY = 1.0f - posColor.y / KinectWrapper.Constants.ColorImageHeight;

                        float scaleX1 = posColor1.x / KinectWrapper.Constants.ColorImageWidth;
                        float scaleY1 = 1.0f - posColor1.y / KinectWrapper.Constants.ColorImageHeight;

                        if (objectToMove)
                        {
                            Vector3 vPosOverlay = Camera.main.ViewportToWorldPoint(new Vector3(scaleX, scaleY, distanceToCamera));
                            objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, vPosOverlay, smoothFactor * Time.deltaTime);
                        }

                        if (objectToMove1)
                        {
                            Vector3 vPosOverlay1 = Camera.main.ViewportToWorldPoint(new Vector3(scaleX1, scaleY1, distanceToCamera));
                            objectToMove1.transform.position = Vector3.Lerp(objectToMove1.transform.position, vPosOverlay1, smoothFactor * Time.deltaTime);
                        }
                    }
                }
            }
        }        
    }
}