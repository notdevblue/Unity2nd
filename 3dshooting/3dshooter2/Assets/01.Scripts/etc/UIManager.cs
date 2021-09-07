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

    private void Awake()
    {
        bulletMarkPannel.GetComponentsInChildren<Image>(bulletMarkList);

        bulletMarkList.RemoveAt(0);
        bulletMarkList.ForEach(x => x.gameObject.SetActive(false));

        playerGun.UpdateMaxBullet += value => {
            for(int i = 0; i < value / 10; ++i)
            {
                bulletMarkList[i].gameObject.SetActive(true);
            }
        };

        playerGun.UpdateBullet += value => {
            int cnt = (int)Mathf.Floor(value / 10);
            for(int i = 0; i < bulletMarkList.Count; ++i)
            {
                if(!bulletMarkList[i].gameObject.activeSelf) break;
                bulletMarkList[i].sprite = i < cnt ? markerImages[0] : markerImages[1];
            }
        };
        
    }
    

}
