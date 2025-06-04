using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 자막용 UI 사용

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
        public bool isHot; // 🔥 이 문이 뜨거운 문인가?
    }

    public List<DoorGet> UseDoors = new List<DoorGet>();

    public bool door_in_use;
    public Coroutine DoorStartUsing;

    // 🔤 자막 출력용 텍스트 (필요 시 UI에 연결)
    public Text subtitleText;
    private float subtitleDisplayTime = 2f;

    // 🔈 아파하는 소리용
    public AudioClip ouchSound;
    private AudioSource audioSource;

    void Start()
    {
        // AudioSource 컴포넌트 받아오기
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // 없으면 추가
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void MoveMyDoor()
    {
        Debug.Log("MoveDoorCalled");

        foreach (var door in UseDoors)
        {
            if (door.Door == gameObject)
            {
                if (door.isHot) // 🔥 뜨거운 문이면
                {
                    ShowSubtitle("앗 뜨거워!");
                    PlayOuchSound();
                    return;
                }

                if (!door_in_use)
                {
                    door_in_use = true;

                    if (!door.isDoorOpen)
                    {
                        door.isDoorOpen = true;
                        Debug.Log("Before Start Open Coroutine");
                        DoorStartUsing = StartCoroutine(OpenDoor(door.OpenValue, door.Door, door.RotationOrigin));
                    }
                    else
                    {
                        door.isDoorOpen = false;
                        Debug.Log("Before Start Close Coroutine");
                        DoorStartUsing = StartCoroutine(CloseDoor(door.CloseValue, door.Door, door.OpenValue, door.RotationOrigin));
                    }
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

    private void ShowSubtitle(string message)
    {
        if (subtitleText == null)
        {
            Debug.LogWarning("Subtitle Text가 연결되지 않았습니다.");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(DisplaySubtitle(message));
    }

    private IEnumerator DisplaySubtitle(string message)
    {
        subtitleText.text = message;
        subtitleText.enabled = true;
        yield return new WaitForSeconds(subtitleDisplayTime);
        subtitleText.enabled = false;
    }

    private void PlayOuchSound()
    {
        if (audioSource != null && ouchSound != null)
        {
            audioSource.PlayOneShot(ouchSound);
        }
        else
        {
            Debug.LogWarning("AudioSource 또는 OuchSound가 설정되지 않았습니다.");
        }
    }

    public IEnumerator OpenDoor(int Angle, GameObject currentDoor, GameObject RotationOri)
    {
        Debug.Log("OpenDoor 시작됨");

        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            if (Angle > 0)
            {
                RotationOri.transform.Rotate(new Vector3(0, 0, 95 * Time.deltaTime));

                if (Angle < RotationOri.transform.localEulerAngles.z)
                {
                    door_in_use = false;
                    yield break;
                }
            }
            else if (Angle < 0)
            {
                RotationOri.transform.Rotate(new Vector3(0, 0, -95 * Time.deltaTime));

                if ((360 + Angle) > RotationOri.transform.localEulerAngles.z)
                {
                    door_in_use = false;
                    yield break;
                }
            }
        }
    }

    public IEnumerator CloseDoor(int Angle, GameObject currentDoor, int OpenValue, GameObject RotationOri)
    {
        Debug.Log("CloseDoor 시작됨");

        while (true)
        {
            yield return new WaitForSeconds(0.008f);

            if (OpenValue == 88)
            {
                RotationOri.transform.Rotate(new Vector3(0, 0, -95 * Time.deltaTime));

                if ((Angle + 2) > RotationOri.transform.localEulerAngles.z)
                {
                    door_in_use = false;
                    RotationOri.transform.localEulerAngles = new Vector3(RotationOri.transform.localEulerAngles.x, RotationOri.transform.localEulerAngles.y, Angle);
                    yield break;
                }
            }
            else if (OpenValue == -88)
            {
                RotationOri.transform.Rotate(new Vector3(0, 0, 95 * Time.deltaTime));

                if (RotationOri.transform.localEulerAngles.z > 358)
                {
                    door_in_use = false;
                    RotationOri.transform.localEulerAngles = new Vector3(RotationOri.transform.localEulerAngles.x, RotationOri.transform.localEulerAngles.y, Angle);
                    yield break;
                }
            }
        }
    }
}
