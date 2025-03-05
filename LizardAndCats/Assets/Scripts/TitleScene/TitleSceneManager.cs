using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;
using System;
using static UnityEngine.ParticleSystem;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;

enum E_PANEL
{ 
    TITLE,
    STAAGE1,
    STAAGE2,
}

enum E_BUTTON
{
    START,

    STAGE,
    ENDLESS,

    FIRST,
    SECOND,
    THIRD,
};

enum E_CURSOR
{
    UP,
    DOWN,
    RIGHT,
    LEFT,
}

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    GameObject parentButtons;
    [SerializeField]
    GameObject[] panel;
    [SerializeField]
    //GameObject[] buttonObj = new GameObject[(int)E_BUTTON.COUNT];
    GameObject[] buttonObj;
    [SerializeField]
    string stageName;
    [SerializeField]
    Animator fadeAnimator;

    E_BUTTON currentBottonMemo;

    E_BUTTON currentBotton;

    E_PANEL onPanel;

    void Init()
    {
        currentBotton = E_BUTTON.START;
        currentBottonMemo = currentBotton;

        for(int i = 0; i < panel.Length; i++)
        {
            switch (i)
            {
                default:
                    panel[i].SetActive(false);
                    break;

                case 0:
                    panel[i].SetActive(true);
                    break;
            }
        }

        for(int i = 0; i < buttonObj.Length; i++)
        {
            switch (i) 
            {
                default:
                    buttonObj[i].GetComponent<Outline>().enabled = false;
                    break;

                case 0:
                    buttonObj[i].GetComponent<Outline>().enabled = true;
                    break;
            }
        }

        onPanel = E_PANEL.TITLE;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        ButtonMove();
        ButtonEnter();
        ButtonOutline();

        Debug.Log(currentBotton);
    }

    void ButtonOutline()
    {
        if(currentBottonMemo != currentBotton)
        {
            buttonObj[(int)currentBottonMemo].GetComponent<Outline>().enabled = false;
            buttonObj[(int)currentBotton].GetComponent<Outline>().enabled = true;
            currentBottonMemo = currentBotton;
        }

#if false
            for(int i = 0; i < buttonObj.Length; i++)
        {
            buttonObj[i].GetComponent<Outline>().enabled = false;
        }
        buttonObj[(int)currentBotton].GetComponent<Outline>().enabled = true;
#endif
    }

    void ButtonEnter()
    {
        if (Input.GetKeyDown(KeyCode.Return) == true)
        {
            switch (currentBotton)
            {
                case E_BUTTON.START:
                    StartButtonOn();
                    break;

                case E_BUTTON.STAGE:
                    StageButtonOn();
                    break;

                default:
                    Debug.Log(currentBotton);
                    fadeAnimator.SetTrigger("FadeIn");
                    PlayerPrefs.SetInt("STAGE", (int)currentBotton);
                    //PlayerPrefs.SetInt("STAGE", (int)currentBotton);
                    //Common.LoadScene(stageName);
                    break;
            };
        }
    }
    public void Change()
    {
        Common.LoadScene(stageName);
    }

    void CheckCursor(E_BUTTON max, E_CURSOR Cursor)
    {
        if((Cursor == E_CURSOR.UP)||(Cursor == E_CURSOR.LEFT))
        {
            if (currentBotton <= max) return;
            currentBotton -= 1;
        }
        else
        {
            if (currentBotton >= max) return;
            currentBotton += 1;
        }
    }

    private void ButtonMove()
    {
        switch(onPanel)
        {
            case E_PANEL.TITLE:

                break;

            //case E_PANEL.STAAGE1:
            //    if ((Input.GetKeyDown(KeyCode.LeftArrow) == true) || (Input.GetKeyDown(KeyCode.A) == true))
            //    {
            //        CheckCursor(E_BUTTON.STAGE, E_CURSOR.LEFT);
            //        //                    if (currentBotton >= E_BUTTON.THIRD) return;
            //        //                    currentBotton += 1;
            //    }
            //    else if((Input.GetKeyDown(KeyCode.RightArrow) == true) || (Input.GetKeyDown(KeyCode.D) == true))
            //    {
            //        CheckCursor(E_BUTTON.ENDLESS, E_CURSOR.RIGHT);
            //        if (currentBotton <= E_BUTTON.FIRST) return;
            //        currentBotton -= 1;
            //    }
            //    break;
            case E_PANEL.STAAGE1:
            case E_PANEL.STAAGE2:
                //上ボタンが押されたなら　-1
                if ((Input.GetKeyDown(KeyCode.UpArrow) == true) || (Input.GetKeyDown(KeyCode.W) == true))
                {
                    if(onPanel == E_PANEL.STAAGE1)
                    {
                        CheckCursor(E_BUTTON.STAGE, E_CURSOR.UP);
                    }
                    else
                    {
                        CheckCursor(E_BUTTON.FIRST, E_CURSOR.UP);
                    }
                }

                //下ボタンが押されたなら　+1
                else if ((Input.GetKeyDown(KeyCode.DownArrow) == true) || (Input.GetKeyDown(KeyCode.S) == true))
                {
                    if (onPanel == E_PANEL.STAAGE1)
                    {
                        CheckCursor(E_BUTTON.ENDLESS, E_CURSOR.DOWN);
                    }
                    else
                    {
                        CheckCursor(E_BUTTON.THIRD, E_CURSOR.DOWN);
                    }
                }
                break;
        }
    }

    public void StageButtonOn2()
    {
        panel[(int)onPanel - 1].SetActive(false);
        panel[(int)onPanel].SetActive(true);
    }

    public void StageButtonOn()
    {
        currentBotton = E_BUTTON.FIRST;
        onPanel = E_PANEL.STAAGE2;
        fadeAnimator.SetTrigger("BlackOut2");

        //parentButtons.GetComponent<Animator>().SetInteger("onButton", (int)onButton);
    }

    public void StartButtonOn2()
    {
        
        panel[(int)onPanel - 1].SetActive(false);
        panel[(int)onPanel].SetActive(true);
    }

    //スタートボタンが押されたら…
    public void StartButtonOn()
    {
        Debug.Log("StartButtonOn");
        //選択状況をファーストに
        currentBotton = E_BUTTON.STAGE;
        onPanel = E_PANEL.STAAGE1;
        fadeAnimator.SetTrigger("StartButton");

        //アニメーションのOnButtonにtrueを入れ込む
        //parentButtons.GetComponent<Animator>().SetInteger("nButton", (int)onButton);
    }

    public void FirstButtonOn()
    {
        PlayerPrefs.SetInt("STAGE", (int)E_BUTTON.FIRST);
        Common.LoadScene(stageName);
    }

    public void SecondButtonOn()
    {
        PlayerPrefs.SetInt("STAGE", (int)E_BUTTON.SECOND);
        Common.LoadScene(stageName);
    }

    public void ThirdButtonOn()
    {
        PlayerPrefs.SetInt("STAGE", (int)E_BUTTON.THIRD);
        Common.LoadScene(stageName);
    }

    public void EndlessButtonOn()
    {
        PlayerPrefs.SetInt("STAGE", (int)E_BUTTON.ENDLESS);
        Common.LoadScene(stageName);
    }
}
