/*
   - BoardObject.cs -
   製作:黒澤
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Const;

/// <summary>
/// 配列で、落ちてる物情報を管理するための型.
/// </summary>
public class BoardObject
{
    BoardType type;
    int id;
    int hp;

    //初期化処理(コンストラクタ)
    public BoardObject(BoardType _type, int _id, int _hp)
    {
        type = _type;
        id = _id;
        hp = _hp;
    }
}
