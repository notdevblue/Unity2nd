using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Menu<T> : Menu where T : Menu<T>
{
    static private T _instance = null;
    static public T Instance { get { return _instance; } }

    protected virtual void Awake()
    {
        if(_instance != null)
        {
            Debug.LogWarning("Duplicate, Destroying.");
            Destroy(gameObject);
        }
        else
        {
            _instance = this as T;
        }
    }

    protected virtual void OnDestory()
    {
        _instance = null;
    }

    static public void Open()
    {
        if (MenuManager.Instance != null && Instance != null)
        {
            MenuManager.Instance.OpenMenu(Instance);
        }
    }
}


abstract public class Menu : MonoBehaviour
{
    public virtual void OnBackPressed()
    {
        MenuManager.Instance?.CloseMenu();
    }

}
