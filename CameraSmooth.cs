using UnityEngine;
using System.Collections;

public class CameraSmooth : MonoBehaviour
{

    public GameObject _target; //�÷��̾�
    public Vector3 _iniPos; // ī�޶��� ���� ������

    // Use this for initialization
    void Start()
    {
        _iniPos = transform.position; //���� ī�޶��� �������� ������

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _iniPos + _target.transform.position; //ī�޶��� �������� ����ī�޶� ��ġ���� Ÿ������������ ���Ѱ���ŭ�� ��ġ
    }
}
