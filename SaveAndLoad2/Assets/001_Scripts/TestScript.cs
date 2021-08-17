using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    #region delegate 1

    //// 1급 시민
    //// 변수에 들어갈 수 있는 것들
    //// 클래스도 인스턴스 들감
    //// 정수도 가능
    //// 메서드? => delegate (함수를 담는 자료형)

    //public delegate int Add(int a, int b);

    //// delegate += 하면
    //// 호출할 때 가지고 있는 모든 함수가 실행된다.
    //// 뺄 수도 있음
    //// ㄷㄷㄷㄷ

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
    public event Action tc; // 이 스크립트만 호출이 가능
    //public event Action<int> tc; // <= Param 을 넣을 수 있게 됨
    //public event Action<int, long, ...>
    // Func<..., 리턴타입> <= 맨 마지막 친구가 리턴 타입임

    private void Awake()
    {
        tc += () =>
        {
            Debug.Log("와 센즈 아시는구나!");
        };
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            tc();
        }
    }

    // Action : 반환값 없는 Delegate 정의
    // Func : 반환값 있는 Delegate 정의
}
