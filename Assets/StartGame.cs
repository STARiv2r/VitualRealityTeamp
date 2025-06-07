using System.Collections;
using UnityEngine;


public class StartGame : MonoBehaviour
{
    public GameObject Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SetPositionAfterDelay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SetPositionAfterDelay()
    {
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
}
