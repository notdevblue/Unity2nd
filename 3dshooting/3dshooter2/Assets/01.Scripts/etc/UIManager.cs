using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static public UIManager instance;
    public Text remainBullet;
    public RectTransform bulletMarkPannel;
    
    public Sprite[] markerImages;
    
    public Gun playerGun;
    public List<Image> bulletMarkList = new List<Image>();

    private void Start()
    {
        
    }


}
