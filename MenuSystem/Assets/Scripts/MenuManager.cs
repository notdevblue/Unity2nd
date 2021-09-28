using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection; // 오.. 있어보이는. 잘쓰면 어렵게 짤 수 있음. = 멋지다 = 유용하다.

public class MenuManager : MonoBehaviour
{
    // 필요한 메뉴 프리팹들을 변수롤 선언
    public MainMenu mainMenuPrefab = null;
    public OptionMenu optionMenuPrefab = null;
    public CreditMenu creditMenuPrefab = null;

    // 부모로 쓸 Transform 변수 선언
    [SerializeField] private Transform _menuParent = null;

    // Stack 으로 캔버스 메뉴들을 관리
    private Stack<Menu> _menuStack = new Stack<Menu>();

    #region Singleton

    static private MenuManager _instance;
    static public MenuManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Duplicate MenuManger. Destroying");
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            InitMenu();
            DontDestroyOnLoad(this.gameObject);
        }

    }

    private void OnDestroy()
    {
        if(_instance == this)
        {
            _instance = null;
        }
    }

#endregion // Singleton

    private void InitMenu()
    {
        if(_menuParent == null)
        {
            GameObject menuParentObj = new GameObject("Menus");
            _menuParent = menuParentObj.transform;
        }

        DontDestroyOnLoad(_menuParent.gameObject);

        // 리플랙션을 통한 함수 타입을 얻어와서 통합시켜주는 코드
        System.Type myType = this.GetType();

        BindingFlags myFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly; // Instance 있고. public 이고 ,선언된 것들을 긁어옴
        FieldInfo[] fields = myType.GetFields(myFlag); // 긁어온 함수들

        for (int i = 0; i < fields.Length; ++i)
        {
            Debug.Log($"필드: {fields[i]}");
        }

        foreach(FieldInfo field in fields)
        {
            Menu prefab = field.GetValue(this) as Menu;
        // // 매뉴가 추가될 때마다 수동으로 메뉴프리팹을 추가해주는 코드
        // Menu[] menuPrefabs = { mainMenuPrefab, optionMenuPrefab, creditMenuPrefab };
        // foreach(Menu prefab in menuPrefabs)
        //{
            if(prefab != null)
            {
                Menu menuInst = Instantiate(prefab, _menuParent);

                // 첫 오픈하는 매뉴는 메인 매뉴
                if(prefab != mainMenuPrefab)
                {
                    menuInst.gameObject.SetActive(false);
                }
                else
                {
                    OpenMenu(menuInst);
                }
            }
        }
    }

    public void OpenMenu(Menu menuInst)
    {
        if(menuInst == null)
        {
            Debug.LogError("menuinst is null");
            return;
        }
        
        if(_menuStack.Count > 0)
        {
            foreach(Menu menu in _menuStack)
            {
                menu.gameObject.SetActive(false);
            }
        }

        menuInst.gameObject.SetActive(true);
        _menuStack.Push(menuInst);
    }

    public void CloseMenu()
    {
        if(_menuStack.Count == 0)
        {
            Debug.LogError("there is no opened menu left");
            Debug.Log($"menuStack.Count => ${_menuStack.Count}");
            return;
        }

        // 마지막에 열린 메뉴를 꺼내고 제거
        Menu topMenu = _menuStack.Pop();
        topMenu.gameObject.SetActive(false);

        if(_menuStack.Count > 0)
        {
            //바로 그 다음 매뉴 꺼내 활성화 ( 제거 x )
            Menu nextMenu = _menuStack.Peek();
            nextMenu.gameObject.SetActive(true);
        }
    }
}
