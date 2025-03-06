using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOut : MonoBehaviour
{
    public void FadeOut()
    {
        GameObject.Find("TitleManager").GetComponent<TitleSceneManager>().StageButtonOn2();
    }
    public void SceneChange()
    {
        GameObject.Find("TitleManager").GetComponent<TitleSceneManager>().Change();
    }
}
