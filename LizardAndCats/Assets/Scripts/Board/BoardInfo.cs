/*
   - BoardInfo.cs -
   黒澤ver.
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

/// <summary>
/// boardクラス(boardの情報をまとめた所)
/// </summary>
public class Board
{
    //地形情報.
    BoardTerrain terrain;
    //落ちてる物情報.
    BoardObject boardObj = new BoardObject(BoardType.NONE, 0, 0);

    //初期化処理(コンストラクタ)
    public Board(BoardTerrain _terra)
    {
        terrain = _terra;
    }

    //set.
    public void SetTerrain(BoardTerrain _terrain)
    {
        terrain = _terrain;
    }
    public void SetBoardObject(BoardType _type, int _id, int _hp)
    {
        boardObj = new BoardObject(_type, _id, _hp);
    }

    //get.
    public BoardTerrain GetTerrain()
    {
        return terrain;
    }
    public BoardObject GetBoardObject()
    {
        return boardObj;
    }
}
