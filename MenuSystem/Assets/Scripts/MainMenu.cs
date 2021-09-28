using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu<MainMenu>
{
    public void OnPlayPressed()
    {
        Debug.Log("Echo Game startted");
    }

    public void OnOptionPressed()
    {
        OptionMenu.Open();
    }

    public void OnCreditPressed()
    {
        CreditMenu.Open();
    }

    public override void OnBackPressed()
    {
        // 메인매뉴에서의 뒤로가기는 나가기
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
