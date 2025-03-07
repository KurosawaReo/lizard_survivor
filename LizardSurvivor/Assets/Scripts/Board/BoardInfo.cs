/*
   - BoardInfo.cs -
   board配列データ用のクラス.
   
   ・Terrain: 地形情報.
   ・DropObj: 落下物情報.
*/

using Gloval;

/// <summary>
/// 配列で、落下物情報を管理するための型.
/// </summary>
public class BoardDropObj
{
    public DropObj type { get; set; }
    public string  id { get; set; }
    public int     hp { get; set; }
    public int     heal { get; set; }
    public int     damage { get; set; }

    //初期化処理(コンストラクタ)
    public BoardDropObj(DropObj _type, string _id, int _hp, int _heal, int _damage)
    {
        type   = _type;
        id     = _id;
        hp     = _hp;
        heal   = _heal;
        damage = _damage;
    }
}

/// <summary>
/// boardクラス(boardの情報をまとめた所)
/// </summary>
public class Board
{
    //地形情報.
    BoardTerrain terrain;
    //落下物情報.
    BoardDropObj dropObj = new BoardDropObj(DropObj.NONE, "", 0, 0, 0);

    //初期化処理(コンストラクタ)
    public Board(BoardTerrain _terrain)
    {
        terrain = _terrain;
    }

    //set.
    public void SetTerrain(BoardTerrain _terrain)
    {
        terrain = _terrain;
    }
    public void SetObject(DropObj _type, string _id, int _hp, int _heal, int _damage)
    {
        dropObj = new BoardDropObj(_type, _id, _hp, _heal, _damage);
    }

    //get.
    public BoardTerrain GetTerrain()
    {
        return terrain;
    }
    public BoardDropObj GetDropObj()
    {
        return dropObj;
    }
}
