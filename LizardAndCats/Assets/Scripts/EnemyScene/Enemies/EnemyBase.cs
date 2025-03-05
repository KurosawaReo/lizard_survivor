using UnityEngine;
using Const;
using System.Collections.Generic;

public abstract class EnemyBase : MonoBehaviour
{
    public enum EnemyState
    {
        ACTIVE,     // 元気
        FATIGUE,    // 疲労
    }

    protected readonly Vector2Int ERROR_VEC = new Vector2Int(-1, -1);


    protected int fatigueCount = 0;

    protected Vector2Int pos;
    protected GameManager gm;

    protected Vector2Int oldPos;

    [Tooltip("通過可能なマスの種類"), SerializeField]
    protected List<BoardType> passableBoards;
    [Tooltip("敵管理クラス")]
    EnemyManager em;


    virtual public void Init(Vector2Int _pos, EnemyManager _em)
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        em = _em;
        pos = _pos;
        oldPos = _pos;

        SetPosition();
    }

    virtual public void Move(MoveVec _vec)
    {
        oldPos = pos;

        switch (_vec)
        {
            case MoveVec.UP:
                pos -= Vector2Int.up;
                break;
            case MoveVec.DOWN:
                pos -= Vector2Int.down;
                break;
            case MoveVec.RIGHT:
                pos += Vector2Int.right;
                break;
            case MoveVec.LEFT:
                pos += Vector2Int.left;
                break;
        }

        SetPosition();
    }

    protected void SetPosition()
    {
        var (lb, rt) = Common.GetWorldWindowSize();
        var tmpPos = new Vector3(lb.x + pos.x + 0.5f, rt.y - pos.y - 0.5f, 0);

        transform.position = tmpPos;
    }

    // todo:盤面を参照して行動を決定
    abstract public void CheckMove();

    /// <summary>
    /// 通れるマスを判定
    /// </summary>
    /// <returns></returns>
    public bool IsPassable(Vector2Int _pos)
    {
        if (
            gm.GetBoardSquare(_pos).GetTerrain() != BoardTerrain.WALL &&
            _pos != gm.GetPlayerPos() &&
            em.IsNoEnemies(_pos)
            )
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 探索範囲を取得
    /// </summary>
    /// <returns></returns>
    protected List<Vector2Int> GetCheckList()
    {
        var board = gm.GetBoard();
        var checkList = new List<Vector2Int>();

        if (pos.y != 0 && em.IsNoEnemies(new Vector2Int(pos.y - 1, pos.x)))
        {
            checkList.Add(new Vector2Int(pos.y - 1, pos.x));
        }
        if (pos.y != board.GetLength(0) - 1 && em.IsNoEnemies(new Vector2Int(pos.y + 1, pos.x)))
        {
            checkList.Add(new Vector2Int(pos.y + 1, pos.x));
        }
        if (pos.x != 0 && em.IsNoEnemies(new Vector2Int(pos.y, pos.x - 1)))
        {
            checkList.Add(new Vector2Int(pos.y, pos.x - 1));
        }
        if (pos.x != board.GetLength(1) - 1 && em.IsNoEnemies(new Vector2Int(pos.y, pos.x + 1)))
        {
            checkList.Add(new Vector2Int(pos.y, pos.x + 1));
        }
        return checkList;
    }


    public Vector2Int GetPos()
    {
        return pos;
    }

    protected void Attack()
    {
        // todo:プレイヤーに攻撃
    }

    protected void EatFood()
    {
        // todo:食べ物消失
    }

    protected void BreakNest()
    {
        // todo:巣を壊す
    }

    protected void BreakMaterial()
    {
        // todo:素材消滅
    }

}
