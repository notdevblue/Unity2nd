using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript2 : MonoBehaviour
{
    TestScript ts;

    private void Awake()
    {
        ts = GetComponent<TestScript>();
    }

    private void Start()
    {
        ts.tc += () =>
        {
            Debug.Log("���� ��� ���մϴ�.");
        };
    }
}
