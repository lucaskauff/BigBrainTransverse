using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleGestureCapture : MonoBehaviour
{
    [Header("Serializable variables")]
    [SerializeField] bool isThereTwoPlayers = false;
    [SerializeField] int playerIndex = 0;
    [SerializeField] float cursorRangeMultiplierX = 250;
    [SerializeField] float cursorRangeMultiplierY = 250;
    public KinectWrapper.NuiSkeletonPositionIndex TrackedJoint = KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
    public float smoothFactor = 5f;

    [Header("Objects to serialize")]
    public RectTransform cursorToMove;

    private void Update()
    {
        KinectManager manager = KinectManager.Instance;

        if (manager && manager.IsInitialized())
        {
            int iJointIndex = (int)TrackedJoint;

            if (manager.IsUserDetected())
            {
                uint userId;

                if (playerIndex == 0)
                {
                    userId = manager.GetPlayer1ID();
                }
                else
                {
                    userId = manager.GetPlayer2ID();
                }

                if (manager.IsJointTracked(userId, iJointIndex))
                {
                    Vector3 posJoint = manager.GetRawSkeletonJointPos(userId, iJointIndex);

                    if (posJoint != Vector3.zero)
                    {
                        Vector2 posDepth = manager.GetDepthMapPosForJointPos(posJoint);
                        Vector2 posColor = manager.GetColorMapPosForDepthPos(posDepth);
                        float scaleX = posColor.x / KinectWrapper.Constants.ColorImageWidth;
                        float scaleY = 1.0f - posColor.y / KinectWrapper.Constants.ColorImageHeight;

                        if (cursorToMove)
                        {
                            Vector2 vPosOverlay = new Vector2((scaleX - 0.5f) * cursorRangeMultiplierX, (scaleY - 0.5f) * cursorRangeMultiplierY);
                            Debug.Log(vPosOverlay);
                            cursorToMove.localPosition = Vector2.Lerp(cursorToMove.localPosition, vPosOverlay, smoothFactor * Time.deltaTime);
                        }
                    }
                }
            }
        }
    }
}