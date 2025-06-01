using UnityEngine;

public class DoorHandleTrigger : MonoBehaviour
{
    private bool isHandNear = false;
    [SerializeField] private Door door;

    private void Update()
    {
        if (isHandNear && OVRInput.GetDown(OVRInput.RawButton.A))
        {
            Debug.Log("Door!!");
            door.MoveMyDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Door In 11!!");
        if (other.CompareTag("PlayerHand"))
        {
            Debug.Log("Door In!!");
            isHandNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Door Out 11!!");
        if (other.CompareTag("PlayerHand"))
        {
            Debug.Log("Door Out!!");
            isHandNear = false;
        }
    }

}
