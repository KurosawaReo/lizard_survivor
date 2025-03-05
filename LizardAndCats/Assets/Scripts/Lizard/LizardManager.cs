using Const;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//プレイヤー(トカゲ)の情報.
public class Lizard
{
    //現在位置.
    public Vector2Int pos
    { 
        get; set; //csはこれだけでゲッターセッターができる.
    }
    //体力.
    public int hp
    {
        get; set;
    }
    //尻尾があるかどうか.
    public bool isTail
    { 
        get; set; 
    }
    //尻尾回復ゲージ.
    public int tailGage
    {
        get; set;
    }
    //持ち物.
    public DropObj[] inventory 
    {
        get; set;
    }

    //初期化処理(コンストラクタ)
    public Lizard(Vector2Int _pos, int _hp, bool _isTail, int _tailGage, DropObj[] _inventory)
    {
        pos       = _pos;
        hp        = _hp;
        isTail    = _isTail;
        tailGage  = _tailGage;
        inventory = _inventory;
    }
}

public class LizardManager : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] GameObject objBrdMng; //BoardManager.
    BoardManager scptBrdMng;

    //トカゲの情報.
    Lizard lizard = new Lizard(
 
        new Vector2Int(1, 1),   //pos.
        Common.LIZARD_LIFE_MAX, //life.
        true,                   //isTail.
        0,                      //tailGage.

        //inventory.
        new DropObj[Common.INVENTORY_CNT] {
            DropObj.NONE,
            DropObj.NONE,
            DropObj.NONE,
            DropObj.NONE
        }
    );

    void Start()
    {
        scptBrdMng = objBrdMng.GetComponent<BoardManager>();
    }

    void Update()
    {
        //操作.
        CommandMove();
        CommandNest();
        //処理.
        MoveExe(gameObject, lizard.pos);
    }

    //操作:トカゲ移動.
    private void CommandMove()
    {
        Vector2Int tmpPos = lizard.pos; //仮変更用にコピー.

        //上.
        if (Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.UpArrow))
        {
            tmpPos -= Vector2Int.up;
        }
        //下.
        if (Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.DownArrow))
        {
            tmpPos -= Vector2Int.down;
        }
        //左.
        if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tmpPos += Vector2Int.left;
        }
        //右.
        if (Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.RightArrow))
        {
            tmpPos += Vector2Int.right;
        }

        //範囲外に出ていなければ.
        if(tmpPos.x >= 0 && tmpPos.x < Common.BOARD_WID &&
           tmpPos.y >= 0 && tmpPos.y < Common.BOARD_HEI)
        {
            //移動先のマスの物取得.
            var brdTer = scptBrdMng.GetBoard(tmpPos).GetTerrain();
            var brdObj = scptBrdMng.GetBoard(tmpPos).GetObject();

            //障害物がなければ.
            if (
                brdTer != BoardTerrain.WALL
                /* TODO:敵たちの座標 */
            ){
                //移動実行.
                lizard.pos = tmpPos;

                //尻尾がないなら.
                if (!lizard.isTail){
                    //食べ物があれば.
                    if (brdObj.type == DropObj.FOOD)
                    {
                        EatFood(); //食べる処理.
                    }
                }
#if false
                //敵がいれば.
                if (/* TODO:敵たちの座標 */)
                {
                    //ダメージ処理.
                    HpDamage();
                }
#endif
            }
        }
    }
    //操作:巣を作る.
    private void CommandNest()
    {
        //スペースを押したら.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var board = scptBrdMng.GetBoard(lizard.pos);    //[取得]現在マスの情報.
            board.SetObject(DropObj.NEST, "none", 1, 0, 0); //[編集]巣にする.
            scptBrdMng.SetBoard(lizard.pos, board);         //[更新]board置き換え.
        }
    }

    //食べ物処理.
    private void EatFood()
    {
        var board = scptBrdMng.GetBoard(lizard.pos); //[取得]現在マスの情報.

        //尻尾回復ゲージ増加.
        lizard.tailGage += board.GetObject().heal;
        //ゲージが最大になったら
        if(lizard.tailGage >= Common.LIZARD_HEALGAGE_MAX)
        {
            lizard.tailGage = 0;
            lizard.isTail = true; //尻尾復活.
        }
        
        //食べ物を消す.
        board.SetObject(DropObj.NONE, "none", 0, 0, 0); //[編集]無にする.
        scptBrdMng.SetBoard(lizard.pos, board);         //[更新]board置き換え.
    }

    //ダメージ処理.
    private void HpDamage()
    {
        lizard.hp -= 1;

        //残機0になったら.
        if(lizard.hp <= 0)
        {
            LizardDeath();
        }
    }
    //死亡処理.
    private void LizardDeath()
    {
        /* TODO:プレイヤー死亡 */
    }

    //移動実行.
    private void MoveExe(GameObject _obj, Vector2Int _pos)
    {
        //ウィンドウの端の座標取得.
        var (lb, rt) = Common.GetWorldWindowSize();

        //座標計算.
        float x = lb.x+Common.BOARD_BASE_X + ( _pos.x+0.5f) * Common.BOARD_GRID_SIZE;
        float y = rt.y-Common.BOARD_BASE_Y + (-_pos.y-0.5f) * Common.BOARD_GRID_SIZE;
        //移動.
        _obj.transform.position = new Vector2(x, y);
    }
}
