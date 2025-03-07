/*
   - EnemyBase.cs -
   敵のベースプログラム(継承用)
*/
using Gloval;
using UnityEngine;
using System.Collections.Generic;

public abstract class EnemyBase : MonoBehaviour
{
    [Tooltip("移動用初期値")]
    protected readonly Vector2Int ERROR_VEC = new Vector2Int(-1, -1);
    [Tooltip("疲労ゲージ.fatigueCount>0なら疲労状態")]
    protected int fatigueCount = 0;
    [Tooltip("攻撃による疲労量.攻撃or巣の破壊で加算され、ターン経過で減少.0で通常に戻る"), SerializeField]
    protected int fatigueAmount = 0;
    [Tooltip("配列座標")]
    protected Vector2Int pos;
    [Tooltip("前ターンの配列座標")]
    protected Vector2Int oldPos;
    [Tooltip("日中の動きの頻度.デフォルトでnターンに一回夜と同じ行動する"), SerializeField]
    protected int frequency;
    [Tooltip("日中の活動タイマー")]
    protected int dayActiveTimer = 0;

    EnemyManager em;
    protected GameManager gm;

    virtual public void Init(Vector2Int _pos, EnemyManager _em, float _lScale)
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        em = _em;
        pos = _pos;
        oldPos = _pos;
        var tmp = transform.localScale;
        tmp *= _lScale;
        transform.localScale = tmp;

        SetPosition();
    }

    virtual public void Move(MoveDir _dir)
    {
        //print($"移動前の配列座標:[{pos.y},{pos.x}]");

        var tmp = transform.rotation;
        switch (_dir)
        {
            case MoveDir.UP:
                pos -= Vector2Int.up;
                tmp.eulerAngles = new Vector3(0, 0, 0);
                break;
            case MoveDir.DOWN:
                pos -= Vector2Int.down;
                tmp.eulerAngles = new Vector3(0, 0, 180);
                break;
            case MoveDir.RIGHT:
                pos += Vector2Int.right;
                tmp.eulerAngles = new Vector3(0, 0, 270);
                break;
            case MoveDir.LEFT:
                pos += Vector2Int.left;
                tmp.eulerAngles = new Vector3(0, 0, 90);
                break;
        }
        transform.rotation = tmp;

        oldPos = pos;

        SetPosition();
        //print($"移動後の配列座標:[{pos.y},{pos.x}]");
        if (gm.GetPlayerPos() == pos)
        {
            //print("プレイヤーに重なった");
        }
    }

    protected void SetPosition()
    {
        var (lb, rt) = Gl_Func.GetWorldWindowSize();
        //var tmpPos = new Vector3(lb.x + pos.x + 0.5f, rt.y - pos.y - 0.5f, 0);
        var tmpPos = gm.GetCellWorldPosition(pos);

        transform.position = tmpPos;
    }

    // 盤面を参照して行動を決定
    abstract public void NightMode();
    virtual public void DayMode()
    {
        if (dayActiveTimer < 0)
        {
            NightMode();
            dayActiveTimer = frequency;
        }
        else
        {
            dayActiveTimer--;
        }
    }

    /// <summary>
    /// 探索範囲を取得
    /// </summary>
    /// <returns></returns>
    protected List<Vector2Int> GetSearchList()
    {
        
        var board = gm.GetBoardAry();
        var checkList = new List<Vector2Int>();

        if (
            pos.y != 0 &&
            em.IsNoEnemies(new Vector2Int(pos.x, pos.y - 1)) &&
            gm.GetBoardSquare(new Vector2Int(pos.x, pos.y - 1)).GetTerrain() != BoardTerrain.WALL
            )
        {
            checkList.Add(new Vector2Int(pos.x, pos.y - 1));
        }
        if (
            pos.y != board.GetLength(0) - 1 &&
            em.IsNoEnemies(new Vector2Int(pos.x, pos.y + 1)) && 
            gm.GetBoardSquare(new Vector2Int(pos.x, pos.y + 1)).GetTerrain() != BoardTerrain.WALL
            )
        {
            checkList.Add(new Vector2Int(pos.x, pos.y + 1));
        }
        if (pos.x != 0 &&
            em.IsNoEnemies(new Vector2Int(pos.x - 1, pos.y)) && 
            gm.GetBoardSquare(new Vector2Int(pos.x - 1, pos.y)).GetTerrain() != BoardTerrain.WALL
            )
        {
            checkList.Add(new Vector2Int(pos.x - 1, pos.y));
        }
        if (pos.x != board.GetLength(1) - 1 &&
            em.IsNoEnemies(new Vector2Int(pos.x + 1, pos.y)) &&
            gm.GetBoardSquare(new Vector2Int(pos.x + 1, pos.y)).GetTerrain() != BoardTerrain.WALL
            )
        {
            checkList.Add(new Vector2Int(pos.x + 1, pos.y));
        }
        return checkList;
    }

    public Vector2Int GetPos()
    {
        return pos;
    }

    virtual protected void Attack(MoveDir _dir)
    {
        var tmp = transform.rotation;

        switch (_dir)
        {
            case MoveDir.UP:
                tmp.eulerAngles = new Vector3(0, 0, 0);
                break;
            case MoveDir.DOWN:
                tmp.eulerAngles = new Vector3(0, 0, 180);
                break;
            case MoveDir.RIGHT:
                tmp.eulerAngles = new Vector3(0, 0, 270);
                break;
            case MoveDir.LEFT:
                tmp.eulerAngles = new Vector3(0, 0, 90);
                break;
        }
        transform.rotation = tmp;

        fatigueCount += fatigueAmount;

        // プレイヤーに攻撃
        em.EnemyAttack();
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ColorReset", 2f);
    }

    void ColorReset()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
    }

    protected void EatFood(Vector2Int _pos)
    {
        // 食べ物消失
        em.EnemyEat(_pos);
    }

    protected void BreakNest(Vector2Int _pos)
    {
        fatigueCount += fatigueAmount;

        // 巣が何回かで壊れるようにする
        em.EnemyEat(_pos);
    }

    protected void BreakMaterial(Vector2Int _pos)
    {
        // 素材消滅
        em.EnemyEat(_pos);
    }
}
