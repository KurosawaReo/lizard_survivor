using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動アニメーション.
/// </summary>
public class MoveAnim
{
    //アニメ用タイマー.
    public float timer
    {
        get; set;
    }
    //折り返し地点を経過したか.
    public bool isMidPass
    {
        get; set;
    }

    //初期化処理(コンストラクタ)
    public MoveAnim(float _timer, bool _isMidPass)
    {
        timer = _timer;
        isMidPass = _isMidPass;
    }
}
