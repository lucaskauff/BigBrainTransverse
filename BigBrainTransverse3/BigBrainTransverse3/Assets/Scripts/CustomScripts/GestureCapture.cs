using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestureCapture : MonoBehaviour
{
    public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
    public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint1 = KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
    
    public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint2 = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
    public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint3 = KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
    

    public GameObject objectToMove;
    public GameObject objectToMove1;
    public GameObject objectToMove2;
    public GameObject objectToMove3;

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
            
            int iJointIndex2 = (int)TrackedJoint2;
            int iJointIndex3 = (int)TrackedJoint3;
            

            if (manager.IsUserDetected())
            {
                uint userId = manager.GetPlayer1ID();
                uint userId2 = manager.GetPlayer2ID();

                if (manager.IsJointTracked(userId, iJointIndex) && manager.IsJointTracked(userId, iJointIndex1) 
                    /*&& manager.IsJointTracked(userId2, iJointIndex) && manager.IsJointTracked(userId2, iJointIndex1)*/)
                {
                    Vector3 posJoint = manager.GetRawSkeletonJointPos(userId, iJointIndex);
                    Vector3 posJoint1 = manager.GetRawSkeletonJointPos(userId, iJointIndex1);

                    
                    Vector3 posJoint2 = manager.GetRawSkeletonJointPos(userId2, iJointIndex2);
                    Vector3 posJoint3 = manager.GetRawSkeletonJointPos(userId2, iJointIndex3);
                    Debug.Log(posJoint2);

                    if (posJoint != Vector3.zero && posJoint1 != Vector3.zero && posJoint2 != Vector3.zero && posJoint3 != Vector3.zero)
                    {
                        // 3d position to depth
                        Vector2 posDepth = manager.GetDepthMapPosForJointPos(posJoint);
                        Vector2 posDepth1 = manager.GetDepthMapPosForJointPos(posJoint1);

                        
                        Vector2 posDepth2 = manager.GetDepthMapPosForJointPos(posJoint2);
                        Vector2 posDepth3 = manager.GetDepthMapPosForJointPos(posJoint3);
                        

                        // depth pos to color pos
                        Vector2 posColor = manager.GetColorMapPosForDepthPos(posDepth);
                        Vector2 posColor1 = manager.GetColorMapPosForDepthPos(posDepth1);

                        
                        Vector2 posColor2 = manager.GetColorMapPosForDepthPos(posDepth2);
                        Vector2 posColor3 = manager.GetColorMapPosForDepthPos(posDepth3);
                        

                        float scaleX = posColor.x / KinectWrapper.Constants.ColorImageWidth;
                        float scaleY = 1.0f - posColor.y / KinectWrapper.Constants.ColorImageHeight;
                        float scaleX1 = posColor1.x / KinectWrapper.Constants.ColorImageWidth;
                        float scaleY1 = 1.0f - posColor1.y / KinectWrapper.Constants.ColorImageHeight;

                        
                        float scaleX2 = posColor2.x / KinectWrapper.Constants.ColorImageWidth;
                        float scaleY2 = 1.0f - posColor2.y / KinectWrapper.Constants.ColorImageHeight;
                        float scaleX3 = posColor3.x / KinectWrapper.Constants.ColorImageWidth;
                        float scaleY3 = 1.0f - posColor3.y / KinectWrapper.Constants.ColorImageHeight;
                        

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

                        if (objectToMove2)
                        {
                            Vector3 vPosOverlay2 = Camera.main.ViewportToWorldPoint(new Vector3(scaleX2, scaleY2, distanceToCamera));
                            objectToMove2.transform.position = Vector3.Lerp(objectToMove2.transform.position, vPosOverlay2, smoothFactor * Time.deltaTime);
                        }

                        if (objectToMove3)
                        {
                            Vector3 vPosOverlay3 = Camera.main.ViewportToWorldPoint(new Vector3(scaleX3, scaleY3, distanceToCamera));
                            objectToMove3.transform.position = Vector3.Lerp(objectToMove3.transform.position, vPosOverlay3, smoothFactor * Time.deltaTime);
                        }
                    }
                }
            }
        }        
    }
}