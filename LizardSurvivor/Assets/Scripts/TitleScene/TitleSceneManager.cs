using Gloval;
using UnityEngine;
using UnityEngine.UI;

enum E_PANEL
{ 
    TITLE,
    STAGE1,
    STAGE2,
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
    E_PANEL  onPanel;

    //タイトルのアニメーション中かどうか.
    bool isTitleAnim = false;
    //他のscriptで呼び出す用(セッター)
    public void SetIsTitleAnim(bool _bool)
    {
        isTitleAnim = _bool;
    }

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
        //タイトルのアニメーション中は動かさない.
        if(!isTitleAnim)
        {
            ButtonMove();
            ButtonSpace();
            ButtonOutline();
        }

        Debug.Log(currentBotton);
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
            case E_PANEL.STAGE1:
            case E_PANEL.STAGE2:
                //上ボタンが押されたなら　-1
                if ((Input.GetKeyDown(KeyCode.UpArrow) == true) || (Input.GetKeyDown(KeyCode.W) == true))
                {
                    if(onPanel == E_PANEL.STAGE1)
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
                    if (onPanel == E_PANEL.STAGE1)
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
    private void ButtonSpace()
    {
        if (Input.GetKeyDown(KeyCode.Space)/* == true*/)
        {
            switch (currentBotton)
            {
                case E_BUTTON.START:
                    StartButtonOn();
                    break;

                case E_BUTTON.STAGE:
                    StageButtonOn();
                    break;
                case E_BUTTON.FIRST:

                default:
                    Debug.Log(currentBotton);
                    fadeAnimator.SetTrigger("FadeIn");
                    switch (currentBotton)
                    {
                        case E_BUTTON.FIRST:
                            PlayerPrefs.SetInt(Gl_Const.KEY_GAME_LEVEL, (int)StageId.STAGE_01);
                            break;
                        case E_BUTTON.SECOND:
                            PlayerPrefs.SetInt(Gl_Const.KEY_GAME_LEVEL, (int)StageId.STAGE_02);
                            break;
                        case E_BUTTON.THIRD:
                            PlayerPrefs.SetInt(Gl_Const.KEY_GAME_LEVEL, (int)StageId.STAGE_03);
                            break;
                        case E_BUTTON.ENDLESS:
                            PlayerPrefs.SetInt(Gl_Const.KEY_GAME_LEVEL, (int)StageId.END_LESS);
                            break;

                    }
                    //PlayerPrefs.SetInt("STAGE", (int)currentBotton);
                    //PlayerPrefs.SetInt("STAGE", (int)currentBotton);
                    Change();
                    break;
            };
        }
    }
    private void ButtonOutline()
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
    public void Change()
    {
        Gl_Func.LoadScene(stageName);
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

    //スタートボタンが押されたら…
    public void StartButtonOn()
    {
        //Debug.Log("StartButtonOn");
        
        //選択状況をファーストに
        currentBotton = E_BUTTON.STAGE;
        onPanel = E_PANEL.STAGE1;
        fadeAnimator.SetTrigger("StartButton");

        isTitleAnim = true; //アニメーション中である指示.

        //アニメーションのOnButtonにtrueを入れ込む
        //parentButtons.GetComponent<Animator>().SetInteger("nButton", (int)onButton);
    }
    //ステージのボタンが押されたら…
    public void StageButtonOn()
    {
        currentBotton = E_BUTTON.FIRST;
        onPanel = E_PANEL.STAGE2;
        fadeAnimator.SetTrigger("BlackOut2");

        isTitleAnim = true; //アニメーション中である指示.

        //parentButtons.GetComponent<Animator>().SetInteger("onButton", (int)onButton);
    }
    public void StageButtonOn2()
    {
        panel[(int)onPanel - 1].SetActive(false);
        panel[(int)onPanel].SetActive(true);
    }

#if false
    public void FirstButtonOn()
    {
        PlayerPrefs.SetInt("STAGE", (int)E_BUTTON.FIRST);
        Gl_Func.LoadScene(stageName);
    }
    public void SecondButtonOn()
    {
        PlayerPrefs.SetInt("STAGE", (int)E_BUTTON.SECOND);
        Gl_Func.LoadScene(stageName);
    }
    public void ThirdButtonOn()
    {
        PlayerPrefs.SetInt("STAGE", (int)E_BUTTON.THIRD);
        Gl_Func.LoadScene(stageName);
    }
    public void EndlessButtonOn()
    {
        PlayerPrefs.SetInt("STAGE", (int)E_BUTTON.ENDLESS);
        Gl_Func.LoadScene(stageName);
    }
#endif
}
