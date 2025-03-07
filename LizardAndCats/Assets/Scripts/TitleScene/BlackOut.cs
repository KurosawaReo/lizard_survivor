using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackOut : MonoBehaviour
{
    TitleSceneManager scptTitleMng; //TitleSceneManager.

    void Start()
    {
        var obj = GameObject.Find("TitleManager");
        scptTitleMng = obj.GetComponent<TitleSceneManager>();
    }

    public void SceneChange()
    {
        scptTitleMng.Change();
    }
    public void FadeOut()
    {
        scptTitleMng.StageButtonOn2();
    }
    public void FadeOutEnd()
    {
        scptTitleMng.SetIsTitleAnim(false); //アニメーションは終わったという指示.
    }
}
