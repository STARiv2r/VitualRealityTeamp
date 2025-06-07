using UnityEngine;

public class DoorAction : MonoBehaviour
{
    public float rayDistance = 1f;  // 레이 쏘는 거리 조절 가능
    public FeedbackManager feedbackManager;

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);

        if (Input.GetKeyDown(KeyCode.E)||OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance))
            {
                Debug.Log("Hit object: " + hit.transform.name);

                if (hit.transform.CompareTag("door"))
                {
                    Door door = hit.transform.GetComponent<Door>();
                    feedbackManager.TriggerHapticFeedback(0.7f,0.5f);
                    if (door != null)
                    {
                        door.ActionDoor();
                    }
                }
                else
                {
                    string objName = hit.collider.gameObject.name;
                    switch (objName)
                    {
                        case "Button floor 1":
                        case "Button floor 2":
                        case "Button floor 3":
                        case "Button floor 4":
                        case "Button floor 5":
                        case "Button floor 6":
                            var passOnParent = hit.transform.gameObject.GetComponent<pass_on_parent>();
                            if (passOnParent != null)
                            {
                                var elevatorControl = passOnParent.MyParent.GetComponent<evelator_controll>();
                                if (elevatorControl != null)
                                {
                                    elevatorControl.AddTaskEve(objName);
                                }
                            }
                            break;
                    }
                }
            }
        }
    }
}
