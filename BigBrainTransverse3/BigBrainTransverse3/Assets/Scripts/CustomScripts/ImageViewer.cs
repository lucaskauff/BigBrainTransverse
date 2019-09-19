using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageViewer : MonoBehaviour
{
    public MultiSourceManager multiSourceManager;

    public RawImage rawImage;

    private void Update()
    {
        rawImage.texture = multiSourceManager.GetColorTexture();
    }
}