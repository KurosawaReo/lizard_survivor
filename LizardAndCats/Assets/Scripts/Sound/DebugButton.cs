using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugButton : MonoBehaviour
{
    // 🎵 ボタンの参照
    public Button startBGMButton;
    public Button stopBGMButton;
    public Button switchBGMButton;

    public Button playSEClickButton;
    public Button playSEEnemyAttackButton;
    public Button playSEEatButton;



    private void Start()
    {
        // BGM デバッグ用ボタンの登録
        startBGMButton.onClick.AddListener(PlayBGM);

        // SE デバッグ用ボタンの登録
        playSEClickButton.onClick.AddListener(() => PlaySE("click"));
        playSEEnemyAttackButton.onClick.AddListener(() => PlaySE("eAtk"));
        playSEEatButton.onClick.AddListener(() => PlaySE("eat"));
    }

    // 🎶 BGM 再生
    public void PlayBGM()
    {
        Debug.Log("【デバッグ】BGM を再生");
        SoundManager.Instance.PlayBGM(SoundManager.Instance.startBGM, 1.0f);
    }

    // 🎵 SE 再生
    public void PlaySE(string seName)
    {
        Debug.Log("【デバッグ】SE: " + seName + " を再生");
        SoundManager.Instance.PlaySE(seName, 1.0f);
    }
}
