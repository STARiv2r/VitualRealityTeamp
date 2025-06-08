using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;



public class Door : MonoBehaviour
{


    [System.Serializable]
    public class DoorGet
    {

        public GameObject Door;
        public int CloseValue;
        public int OpenValue;
        public bool isDoorOpen;
        public GameObject RotationOrigin;


    }

    public List<DoorGet> UseDoors = new List<DoorGet>();


    public bool door_in_use;



    public void MoveMyDoor()
    {


        foreach (var door in UseDoors)
        {
            if (door.Door == gameObject)
            {
                var navLink = GetComponent<NavMeshLink>();


                if (door.isDoorOpen == false && !door_in_use)
                {

                    door_in_use = true;

                    door.isDoorOpen = true;

                    if (navLink != null)
                        navLink.enabled = true; // 문 열릴 때 NavMesh 링크 활성화

                    DoorStartUsing = StartCoroutine(OpenDoor(door.OpenValue, door.Door, door.RotationOrigin));




                }

                if (door.isDoorOpen == true && !door_in_use)
                {


                    door_in_use = true;

                    door.isDoorOpen = false;

                    if (navLink != null)
                        navLink.enabled = false; // 문 닫힐 때 NavMesh 링크 비활성화


                    DoorStartUsing = StartCoroutine(CloseDoor(door.CloseValue, door.Door, door.OpenValue, door.RotationOrigin));

                }


            }
        }
    }




    public void ActionDoor()
    {



        foreach (var door in UseDoors)
        {

            door.Door.GetComponent<Door>().MoveMyDoor();

        }


    }




    public Coroutine DoorStartUsing;


    public IEnumerator OpenDoor(int Angle, GameObject currentDoor, GameObject RotationOri)
    {


    repeatLoop:
        yield return new WaitForSeconds(0.01f);



        if (Angle > 0)
        {
            RotationOri.transform.Rotate(new Vector3(0, 0, 95 * Time.deltaTime));

            if (Angle < RotationOri.transform.localEulerAngles.z)
            {

                door_in_use = false;
                StopCoroutine(DoorStartUsing);
            }
            if (Angle != RotationOri.transform.localEulerAngles.y)
            {
                goto repeatLoop;
            }
        }
        if (Angle < 0)
        {

            RotationOri.transform.Rotate(new Vector3(0, 0, -95 * Time.deltaTime));

            if ((360 + Angle) > RotationOri.transform.localEulerAngles.z)
            {

                door_in_use = false;
                StopCoroutine(DoorStartUsing);
            }
            if (Angle != RotationOri.transform.localEulerAngles.y)
            {

                goto repeatLoop;
            }
        }



    }



    public IEnumerator CloseDoor(int Angle, GameObject currentDoor, int OpenValue, GameObject RotationOri)
    {
    repeatLoop:
        yield return new WaitForSeconds(0.008f);




        if (OpenValue == 88)
        {

            RotationOri.transform.Rotate(new Vector3(0, 0, -95 * Time.deltaTime));


            if ((Angle + 2) > RotationOri.transform.localEulerAngles.z)
            {

                door_in_use = false;
                RotationOri.transform.localEulerAngles = new Vector3(RotationOri.transform.localEulerAngles.x, RotationOri.transform.localEulerAngles.y, Angle);
                StopCoroutine(DoorStartUsing);
            }
            if (Angle != RotationOri.transform.localEulerAngles.z)
            {
                goto repeatLoop;
            }
        }
        if (OpenValue == -88)
        {

            RotationOri.transform.Rotate(new Vector3(0, 0, 95 * Time.deltaTime));

            if (RotationOri.transform.localEulerAngles.z > 358)
            {

                door_in_use = false;
                RotationOri.transform.localEulerAngles = new Vector3(RotationOri.transform.localEulerAngles.x, RotationOri.transform.localEulerAngles.y, Angle);
                StopCoroutine(DoorStartUsing);
            }
            if (Angle != RotationOri.transform.localEulerAngles.z)
            {

                goto repeatLoop;
            }
        }




        if (Angle != RotationOri.transform.localEulerAngles.z)
        {
            goto repeatLoop;
        }

    }

}
