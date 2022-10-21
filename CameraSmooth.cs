using UnityEngine;
using System.Collections;

public class CameraSmooth : MonoBehaviour
{

    public GameObject _target; //플레이어
    public Vector3 _iniPos; // 카메라의 현재 포지션

    // Use this for initialization
    void Start()
    {
        _iniPos = transform.position; //현재 카메라의 포지션을 저장함

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _iniPos + _target.transform.position; //카메라의 포지션은 현재카메라 위치에서 타겟의포지션을 더한값만큼에 위치
    }
}
