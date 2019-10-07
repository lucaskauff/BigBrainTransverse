using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionBoxes : MonoBehaviour
{
    public bool isSelected;
    public int selectedBy;
    public bool connecting;
    public int whichLobby;
    [SerializeField] PlayerMenuController[] playerControllers;
    [SerializeField] Image linkedGauge;
    [SerializeField] Image linkedBlason;
    [SerializeField] Color cachedColor;
    [SerializeField] float connectingT = 3;
    float startFillT = 0;
    float connectionFill = 0;
    bool alreadyConnecting;

	void Update ()
    {
		if (connecting && !isSelected)
        {
            if (!alreadyConnecting)
            {
                startFillT = Time.time;
                StartCoroutine(Connecting());
                alreadyConnecting = true;
            }
            else
            {
                connectionFill = Time.time - startFillT;
            }
        }
        else
        {
            StopAllCoroutines();
            connectionFill = 0;
            alreadyConnecting = false;
        }

        linkedGauge.fillAmount = connectionFill / connectingT;
	}

    IEnumerator Connecting()
    {
        yield return new WaitForSeconds(connectingT);
        linkedBlason.color = cachedColor;
        isSelected = true;

        if (selectedBy == 1)
            playerControllers[0].hasSelected = true;
        else if (selectedBy == 2)
            playerControllers[1].hasSelected = true;
    }
}