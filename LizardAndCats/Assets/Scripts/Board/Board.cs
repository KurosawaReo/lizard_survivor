using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Const;

public class Board
{
    BoardTerrain terrain; //’nŒ`î•ñ.
    BoardObject boardObj = new BoardObject(BoardType.NONE, 0, 0);     //—‚¿‚Ä‚é•¨î•ñ.

    //‰Šú‰»ˆ—.
    public Board(BoardTerrain _terra)
    {
        terrain = _terra;
    }

    //setŠÖ”.
    public void SetTerrain(BoardTerrain _terrain)
    {
        terrain = _terrain;
    }
    public void SetBoardObject(BoardType _type, int _id, int _hp)
    {
        boardObj = new BoardObject(_type, _id, _hp);
    }

    //getŠÖ”.
    public BoardTerrain GetTerrain()
    {
        return terrain;
    }
    public BoardObject GetBoardObject()
    {
        return boardObj;
    }
}
