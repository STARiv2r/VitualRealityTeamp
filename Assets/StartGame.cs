using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;


public class StartGame : MonoBehaviour
{
    public GameObject Player;
    public Transform startPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WaitForCameraStableAndSetPosition());
        startPoint = gameObject.transform;

    }

    // Update is called once per frame
    void Update()
    {
        Player.transform.position = gameObject.transform.position;
        Player.transform.rotation = gameObject.transform.rotation;
    }

    IEnumerator SetPositionAfterDelay()
    {
        yield return null; // 한 프레임 대기
        yield return null; // 한 프레임 대기
        yield return null; // 한 프레임 대기
        yield return null; // 한 프레임 대기
        yield return null; // 한 프레임 대기
        yield return null; // 한 프레임 대기
        yield return null; // 한 프레임 대기

        while (!OVRManager.isHmdPresent)
        {
            yield return null; // HMD가 없으면 계속 대기
        }


        Player.transform.position = gameObject.transform.position;
        Player.transform.rotation = gameObject.transform.rotation;
    }

    IEnumerator WaitForCameraStableAndSetPosition()
    {
        OVRManager manager = FindObjectOfType<OVRManager>();
        manager.trackingOriginType = OVRManager.TrackingOrigin.EyeLevel;

        for (int i = 0; i < 10; i++)
        {
            yield return null;
        }

        OVRCameraRig rig = FindObjectOfType<OVRCameraRig>();
        rig.transform.position = startPoint.position;
        rig.transform.rotation = startPoint.rotation;

        // CharacterController 튜닝
        CharacterController cc = Player.GetComponent<CharacterController>();
        cc.height = 1.7f;
        cc.center = new Vector3(0, 0.85f, 0);
        cc.radius = 0.3f; // 옵션

        // (Optional) LateUpdate 위치 고정 필요하면 추가 가능
    }

}
