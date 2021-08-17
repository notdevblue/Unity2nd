using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    #region delegate 1

    //// 1�� �ù�
    //// ������ �� �� �ִ� �͵�
    //// Ŭ������ �ν��Ͻ� �鰨
    //// ������ ����
    //// �޼���? => delegate (�Լ��� ��� �ڷ���)

    //public delegate int Add(int a, int b);

    //// delegate += �ϸ�
    //// ȣ���� �� ������ �ִ� ��� �Լ��� ����ȴ�.
    //// �� ���� ����
    //// ��������

    //void Start()
    //{
    //    Add a = MyAdd;
    //    a += (a, b) => b - a;

    //    Debug.Log("");
    //}

    //public int MyAdd(int a, int b)
    //{
    //    return a + b;
    //}

    //void Update()
    //{

    //}
    #endregion

    #region delegate 2

    //private List<int> list = new List<int>();


    //private void Start()
    //{
    //    list.Add(5);
    //    list.Add(7);
    //    list.Add(4);
    //    list.Add(2);
    //    list.Add(10);

    //    list.Sort((x, y) => y.CompareTo(x));

    //    PrintList(list);
    //}

    //private void PrintList<T>(List<T> list)
    //{
    //    string a = "";
    //    foreach (T item in list)
    //    {
    //        a += item + ", ";
    //    }

    //    Debug.Log(a);
    //}
    #endregion

    //public delegate void TestCall();
    public event Action tc; // �� ��ũ��Ʈ�� ȣ���� ����
    //public event Action<int> tc; // <= Param �� ���� �� �ְ� ��
    //public event Action<int, long, ...>
    // Func<..., ����Ÿ��> <= �� ������ ģ���� ���� Ÿ����

    private void Awake()
    {
        tc += () =>
        {
            Debug.Log("�� ���� �ƽô±���!");
        };
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            tc();
        }
    }

    // Action : ��ȯ�� ���� Delegate ����
    // Func : ��ȯ�� �ִ� Delegate ����
}
