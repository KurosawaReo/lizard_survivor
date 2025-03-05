/*
   - Board.cs -
   製作:黒澤
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Const;

/// <summary>
/// boardに使う型の定義と、配列データのある場所.
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

    //set関数.
    public void SetTerrain(BoardTerrain _terrain)
    {
        terrain = _terrain;
    }
    public void SetBoardObject(BoardType _type, int _id, int _hp)
    {
        boardObj = new BoardObject(_type, _id, _hp);
    }

    //get関数.
    public BoardTerrain GetTerrain()
    {
        return terrain;
    }
    public BoardObject GetBoardObject()
    {
        return boardObj;
    }
}
