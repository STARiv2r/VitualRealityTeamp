using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    public List<GameObject> MapImageList;
    public GameObject MapUI;
    private int mapIndex = 0;
    private bool mapFlag = false;

    public GameObject GameClearTitle;

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            mapFlag = !mapFlag;
            MapUI.SetActive(mapFlag);
            MapImageList[mapIndex].SetActive(true);
        }
        if (mapFlag == true)
        {
            if (OVRInput.GetDown(OVRInput.RawButton.Y))
            {
                MapImageList[mapIndex].SetActive(false);
                mapIndex++;
                if (mapIndex >= MapImageList.Count)
                    mapIndex = 0;
                MapImageList[mapIndex].SetActive(true);
            }
        }
    }
}
