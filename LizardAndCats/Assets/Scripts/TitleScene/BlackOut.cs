using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FadeOut()
    {
        GameObject.Find("Manager").GetComponent<TitleSceneManager>().StageButtonOn2();
    }
    public void SceneChange()
    {
        GameObject.Find("Manager").GetComponent<TitleSceneManager>().Change();
    }
}
