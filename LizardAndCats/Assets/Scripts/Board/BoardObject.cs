using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Const;

public class BoardObject
{
    public BoardType type;
    public int id;
    public int hp;

    public BoardObject(BoardType _type, int _id, int _hp)
    {
        type = _type;
        id = _id;
        hp = _hp;
    }
}
