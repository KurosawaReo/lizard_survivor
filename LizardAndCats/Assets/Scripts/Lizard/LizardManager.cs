using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Const;

//プレイヤー(トカゲ)の情報.
public class Lizard
{
    //現在位置.
    public Vector2Int pos
    { 
        get; set; //csはこれだけでゲッターセッターができる.
    }
    public Direction dir
    { 
        get; set; 
    }
    //体力.
    public int hp
    {
        get; set;
    }
    //尻尾回復ゲージ.
    public int tailGage
    {
        get; set;
    }

    //尻尾があるかどうか.
    public bool isTail
    { 
        get; set; 
    }
    //操作可能か.
    public bool isOpeAble
    {
        get; set;
    }

    //持ち物.
    public DropObj[] inventory
    {
        get; set;
    }

    //初期化処理(コンストラクタ)
    public Lizard(Vector2Int _pos, Direction _dir, int _hp, int _tailGage, bool _isTail, bool _isOpeAble, DropObj[] _inventory)
    {
        pos       = _pos;
        dir       = _dir;
        hp        = _hp;
        tailGage  = _tailGage;

        isTail    = _isTail;
        isOpeAble = _isOpeAble;

        inventory = _inventory;
    }
}

//プレイヤー(トカゲ)のアニメ情報.
public class LizardAnim
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
    public LizardAnim(float _timer, bool _isMidPass)
    {
        timer     = _timer;
        isMidPass = _isMidPass;
    }
}

//プレイヤー操作情報.
public class LizardOpe
{
    //移動操作.
    public bool isMoved
    {
        get; set;
    }
    //巣を作る操作.
    public bool isMadeNest
    {
        get; set;
    }

    //初期化処理(コンストラクタ)
    public LizardOpe(bool _isMoved, bool _isMadeNest)
    {
        isMoved    = _isMoved;
        isMadeNest = _isMadeNest;
    }
}

public class LizardManager : MonoBehaviour
{
    [Header("- object -")]
    [SerializeField] GameObject objLizardImg; //トカゲの画像obj.

    [Header("- script -")]
    [SerializeField] GameObject objBrdMng;    //BoardManager.
    BoardManager scptBrdMng;

    //トカゲの情報.
    Lizard lizard = new Lizard(

        new Vector2Int(1, 1), //pos.
        Direction.UP,         //dir.
        Common.LIZARD_HP_MAX, //hp.
        0,                    //tailGage.
        true,                 //isTail.
        true,                 //isOpeAble.

        //inventory.
        new DropObj[Common.INVENTORY_CNT] {
            DropObj.NONE,
            DropObj.NONE,
            DropObj.NONE,
            DropObj.NONE
        }
    );
    //トカゲのアニメ情報.
    LizardAnim anim = new LizardAnim(0, false);
    //トカゲの操作情報.
    LizardOpe  ope  = new LizardOpe(false, false);

    void Start()
    {
        //別script取得.
        scptBrdMng = objBrdMng.GetComponent<BoardManager>();

        //Unityでの位置を設定.
        GameMoveExe(gameObject, lizard.pos);
    }

    void Update()
    {
        if(anim.timer > 0 && !anim.isMidPass)
        {
            //操作を行わない.
        }
        else
        {
            CommandMove(); //移動操作.
            CommandNest(); //巣を作る操作.
        }
        
        //操作可能になったら.
        if (lizard.isOpeAble)
        {
            //操作があれば.
            if (ope.isMoved)
            {
                MoveAnimStart(); //アニメーション開始.
            }
            if (ope.isMadeNest)
            {
                MakeNestExe();   //巣作成.
            }

            //リセット.
            ope.isMoved = false;
            ope.isMadeNest = false;
        }
        else
        {
            MoveAnim();    //移動アニメ.
        }
    }

    //操作:トカゲ移動.
    private void CommandMove()
    {
        //仮変更用.
        Vector2Int tmpPos = lizard.pos;
        Direction  tmpDir = Direction.UP;
        //操作があったか.
        bool isInput = false;

        //上.
        if (Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.UpArrow))
        {
            tmpPos -= Vector2Int.up;
            tmpDir  = Direction.UP;
            isInput = true;
        }
        //下.
        if (Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.DownArrow))
        {
            tmpPos -= Vector2Int.down;
            tmpDir  = Direction.DOWN;
            isInput = true;
        }
        //左.
        if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tmpPos += Vector2Int.left;
            tmpDir  = Direction.LEFT;
            isInput = true;
        }
        //右.
        if (Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.RightArrow))
        {
            tmpPos += Vector2Int.right;
            tmpDir  = Direction.RIGHT;
            isInput = true;
        }

        //操作があった場合のみ.
        if (isInput)
        {
            //範囲外に出ていなければ.
            if (tmpPos.x >= 0 && tmpPos.x < Common.BOARD_WID &&
                tmpPos.y >= 0 && tmpPos.y < Common.BOARD_HEI)
            {
                //移動先のマスの物取得.
                var brdTer = scptBrdMng.GetBoard(tmpPos).GetTerrain();

                //障害物がなければ.
                if (
                    brdTer != BoardTerrain.WALL
                    /* TODO:敵たちの座標 */
                )
                {
                    //データだけ更新.
                    lizard.pos = tmpPos;
                    lizard.dir = tmpDir;

                    ope.isMoved = true;
                }
            }
        }
    }
    //操作:巣を作る.
    private void CommandNest()
    {
        //スペースを押したら.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ope.isMadeNest = true;
        }
    }

    //食べ物処理.
    private void EatFood()
    {
        var board = scptBrdMng.GetBoard(lizard.pos); //[取得]現在マスの情報.

        //尻尾回復ゲージ増加.
        lizard.tailGage += board.GetDropObj().heal;
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
    private void GameMoveExe(GameObject _obj, Vector2Int _pos)
    {
        //ウィンドウの端の座標取得.
        var (lb, rt) = Common.GetWorldWindowSize();

        //座標計算.
        float x = lb.x+Common.BOARD_BASE_X + ( _pos.x+0.5f) * Common.BOARD_GRID_SIZE;
        float y = rt.y-Common.BOARD_BASE_Y + (-_pos.y-0.5f) * Common.BOARD_GRID_SIZE;
        //移動.
        _obj.transform.position = new Vector2(x, y);
    }
    //巣作成実行.
    private void MakeNestExe()
    {
        var board = scptBrdMng.GetBoard(lizard.pos);    //[取得]現在マスの情報.
        board.SetObject(DropObj.NEST, "none", 1, 0, 0); //[編集]巣にする.
        scptBrdMng.SetBoard(lizard.pos, board);         //[更新]board置き換え.
    }

    //移動アニメ開始.
    private void MoveAnimStart()
    {
        lizard.isOpeAble = false; //アニメ中は操作不可.
    }
    //移動アニメ処理.
    private void MoveAnim()
    {
        //1秒で+1.
        anim.timer += Time.deltaTime;
        //移動量.
        float move = Common.LIZARD_MOVE_ANIM_VEL * Time.deltaTime;
        
        //トカゲの向き別.
        switch (lizard.dir) 
        {
            case Direction.UP:
                objLizardImg.transform.localPosition += new Vector3(0, +move, 0);
                break;
            case Direction.DOWN:
                objLizardImg.transform.localPosition += new Vector3(0, -move, 0);
                break;
            case Direction.RIGHT:
                objLizardImg.transform.localPosition += new Vector3(+move, 0, 0);
                break;
            case Direction.LEFT:
                objLizardImg.transform.localPosition += new Vector3(-move, 0, 0);
                break;
        }

        //前半.
        if (!anim.isMidPass)
        {
            //前半終了.
            if (anim.timer >= (float)Common.LIZARD_MOVE_ANIM_SEC/2)
            {
                anim.isMidPass = true;
                MoveAnimMid();
            }
        }
        //後半.
        else
        {
            //後半終了.
            if (anim.timer >= Common.LIZARD_MOVE_ANIM_SEC)
            {
                anim.timer = 0;
                anim.isMidPass = false;
                MoveAnimEnd();
            }
        }
    }
    //移動アニメ中盤.
    private void MoveAnimMid()
    {
        //Unityの位置を更新.
        GameMoveExe(gameObject, lizard.pos);
        //画像の相対座標を反転.
        objLizardImg.transform.localPosition = -objLizardImg.transform.localPosition;
    }
    //移動アニメ終了.
    private void MoveAnimEnd()
    {
        //操作可能に.
        lizard.isOpeAble = true;
        //位置リセット.
        objLizardImg.transform.localPosition = Vector3.zero;

        //尻尾がないなら.
        if (!lizard.isTail)
        {
            //移動先のマスの物取得.
            var brdDrop = scptBrdMng.GetBoard(lizard.pos).GetDropObj();
            //食べ物があれば.
            if (brdDrop.type == DropObj.FOOD)
            {
                EatFood(); //食べる処理.
            }
        }

        //TODO:自分から敵にぶつかった時どうなる?<<<<<<<<<<<<<<<<<<<<<<<
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
