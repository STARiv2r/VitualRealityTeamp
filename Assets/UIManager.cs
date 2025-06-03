using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject tutorialUI;
    public GameObject pauseUI;

    private bool tutorialUIFlag = false;

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            tutorialUIFlag = !tutorialUIFlag;
            tutorialUI.SetActive(tutorialUIFlag);
        }
    }
}
