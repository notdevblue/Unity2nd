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
    private List<Image> bulletMarkList = new List<Image>();

    public Image reloadGage;
    public RectTransform reloadHitPoint;
    private RectTransform reloadRect;

    public bool reloadSuccess = true; // TODO : ?

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Multiple UIManager is running in same scene");
        }
        instance = this;

        bulletMarkPannel.GetComponentsInChildren<Image>(bulletMarkList);

        bulletMarkList.RemoveAt(0);
        bulletMarkList.ForEach(x => x.gameObject.SetActive(false));

        playerGun.UpdateMaxBullet += value => {

            remainBullet.text = value.ToString("D2");

            for(int i = 0; i < value / 10; ++i)
            {
                bulletMarkList[i].gameObject.SetActive(true);
            }
        };

        playerGun.UpdateBullet += value => {

            remainBullet.text = value.ToString("D2"); // 01, 10, 03...

            int cnt = (int)Mathf.Floor(value / 10);
            for(int i = 0; i < bulletMarkList.Count; ++i)
            {
                if(!bulletMarkList[i].gameObject.activeSelf) break;
                bulletMarkList[i].sprite = i < cnt ? markerImages[0] : markerImages[1];
            }
        };
        
        reloadRect = reloadGage.gameObject.GetComponent<RectTransform>();
        playerGun.ReloadEvent += value => {
            reloadGage.fillAmount = Mathf.Clamp(value, 0, 1);
            if(value >= 1)
            {
                
            }
        };
    }

    private void CompleteReload()
    {
        reloadGage.fillAmount = 0;
        reloadHitPoint.gameObject.SetActive(false);
        reloadSuccess = true;
    }

    public void StartReload()
    {
        reloadHitPoint.gameObject.SetActive(true);
        Vector2 aPos = reloadHitPoint.anchoredPosition; // position based on anchor

        aPos.x = Random.Range(120, 150);
        reloadHitPoint.anchoredPosition = aPos;
    }

    public bool CheckFastReload()
    {
        if(!reloadSuccess) return false;

        float currentPoint = reloadRect.rect.width * reloadGage.fillAmount;
        Vector2 aPos = reloadHitPoint.anchoredPosition;

        reloadSuccess = currentPoint >= aPos.x
                     && currentPoint <= aPos.x + reloadHitPoint.rect.width;


        if(!reloadSuccess)
        {
            reloadHitPoint.gameObject.SetActive(false);
        }

        return reloadSuccess;

    }
    

}
