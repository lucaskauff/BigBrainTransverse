using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorMovementTest : MonoBehaviour
{
    //GameManager
    InputManager inputManager;

    [Header("Components to serialize")]
    RectTransform myTrans;

    [Header("Objects to serialize")]
    //[SerializeField] RectTransform centerUI;

    //[Header("Variables to serialize")]
    [SerializeField] float inputImpact = 1;
    [SerializeField] float minRangeX;
    [SerializeField] float minRangeY;
    [SerializeField] float maxRangeX;
    [SerializeField] float maxRangeY;

    //Private
    Vector2 moveInput;

    private void Start()
    {
        inputManager = GameManager.Instance.inputManager;
    }

    private void Update()
    {
        CursorMovement();
    }

    void CursorMovement()
    {
        int[] boolsToInts = new int[4];
        boolsToInts[0] = inputManager.buttonUpPressed1 ? 1 : 0;
        boolsToInts[1] = inputManager.buttonRightPressed1 ? 1 : 0;
        boolsToInts[2] = inputManager.buttonDownPressed1 ? 1 : 0;
        boolsToInts[3] = inputManager.buttonLeftPressed1 ? 1 : 0;

        float[] intsToFloats = new float[4];
        for (int i = 0; i < boolsToInts.Length; i++)
        {
            intsToFloats[i] = boolsToInts[i];
        }

        moveInput = new Vector2(intsToFloats[1] - intsToFloats[3], intsToFloats[0] - intsToFloats[2]);
        if (moveInput.x != 0 && moveInput.y != 0)
        {
            float multiplier = Mathf.Cos(45 * Mathf.Deg2Rad);

            if (moveInput.x < 0) moveInput.x = - multiplier;
            else moveInput.x = multiplier;

            if (moveInput.y < 0) moveInput.y = - multiplier;
            else moveInput.y = multiplier;
        }

        if (transform.localPosition.x < minRangeX || transform.localPosition.y < minRangeY || transform.localPosition.x > maxRangeX || transform.localPosition.y > maxRangeY)
        {
            if (transform.localPosition.x < minRangeX)
                transform.localPosition = new Vector3(minRangeX, transform.localPosition.y, transform.position.z);
            if (transform.localPosition.y < minRangeY)
                transform.localPosition = new Vector3(transform.localPosition.x, minRangeY, transform.position.z);
            if (transform.localPosition.x > maxRangeX)
                transform.localPosition = new Vector3(maxRangeX, transform.localPosition.y, transform.position.z);
            if (transform.localPosition.y > maxRangeY)
                transform.localPosition = new Vector3(transform.localPosition.x, maxRangeY, transform.position.z);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x + moveInput.x * inputImpact * Time.deltaTime,
            transform.localPosition.y + moveInput.y * inputImpact * Time.deltaTime, 0);
        }
    }
}