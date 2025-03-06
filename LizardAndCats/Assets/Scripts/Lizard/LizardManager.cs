using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Const;

/// <summary>
/// トカゲの情報.
/// </summary>
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

/// <summary>
/// トカゲ操作情報.
/// </summary>
public class LizardOpe
{
    //移動操作.
    public float moveBuf
    {
        get; set;
    }
    //巣作成操作.
    public float nestBuf
    {
        get; set;
    }

    //仮保存:座標.
    public Vector2Int tmpPos
    { 
        get; set;
    }
    //仮保存:方向.
    public Direction tmpDir
    {
        get; set;
    }

    //初期化処理(コンストラクタ)
    public LizardOpe(float _moveBuf, float _nestBuf, Vector2Int _tmpPos, Direction _tmpDir)
    {
        moveBuf = _moveBuf;
        nestBuf = _nestBuf;
        tmpPos  = _tmpPos;
        tmpDir  = _tmpDir;
    }
}

/// <summary>
/// トカゲのメインプログラム.
/// </summary>
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
    //トカゲの操作情報.
    LizardOpe  ope  = new LizardOpe(
        0,                    //moveBuf. 
        0,                    //nestBuf.
        new Vector2Int(0, 0), //tmpPos.
        Direction.UP          //tmpDir.
    );
    //アニメーション情報.
    MoveAnim anim = new MoveAnim(0, false);

    void Start()
    {
        //別script取得.
        scptBrdMng = objBrdMng.GetComponent<BoardManager>();

        //トカゲを初期位置に移動.
        Common.BoardPosSet(gameObject, lizard.pos.x, lizard.pos.y, false);
    }

    void Update()
    {
        CommandMove(); //移動操作.
        CommandNest(); //巣作成操作.

        //バッファタイマー.
        ope.moveBuf = (ope.moveBuf <= 0) ? 0 : ope.moveBuf - Time.deltaTime;
        ope.nestBuf = (ope.nestBuf <= 0) ? 0 : ope.nestBuf - Time.deltaTime;
        
        //操作可能になったら.
        if (lizard.isOpeAble)
        {
            //操作猶予が残ってるなら.
            if (ope.moveBuf > 0)
            {
                OpeMoveExe();
                ope.moveBuf = 0; //リセット.
            }
            if (ope.nestBuf > 0)
            {
                OpeNestExe();
                ope.nestBuf = 0; //リセット.
            }
        }
        else
        {
            MoveAnim(); //移動アニメ.
        }
    }

    /// <summary>
    /// 移動操作の実行.
    /// </summary>
    private void OpeMoveExe()
    {
        //データ更新.
        lizard.pos = ope.tmpPos;
        lizard.dir = ope.tmpDir;
        //アニメーション開始.
        MoveAnimStart();
    }
    /// <summary>
    /// 巣作成操作の実行.
    /// </summary>
    private void OpeNestExe()
    {
        var board = scptBrdMng.GetBoard(lizard.pos);    //[取得]現在マスの情報.
        board.SetObject(DropObj.NEST, "none", 1, 0, 0); //[編集]巣にする.
        scptBrdMng.SetBoard(lizard.pos, board);         //[更新]board置き換え.
    }

    /// <summary>
    /// 移動操作の入力.
    /// </summary>
    private void CommandMove()
    {
        //操作があったか.
        bool isInput = false;

        //上.
        if (Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.UpArrow))
        {
            ope.tmpPos = lizard.pos - Vector2Int.up;
            ope.tmpDir = Direction.UP;
            isInput = true;
        }
        //下.
        if (Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.DownArrow))
        {
            ope.tmpPos = lizard.pos - Vector2Int.down;
            ope.tmpDir = Direction.DOWN;
            isInput = true;
        }
        //左.
        if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ope.tmpPos = lizard.pos + Vector2Int.left;
            ope.tmpDir = Direction.LEFT;
            isInput = true;
        }
        //右.
        if (Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.RightArrow))
        {
            ope.tmpPos = lizard.pos + Vector2Int.right;
            ope.tmpDir = Direction.RIGHT;
            isInput = true;
        }

        //操作があった場合のみ.
        if (isInput)
        {
            //範囲外に出ていなければ.
            if (ope.tmpPos.x >= 0 && ope.tmpPos.x < Common.BOARD_WID &&
                ope.tmpPos.y >= 0 && ope.tmpPos.y < Common.BOARD_HEI)
            {
                //移動先のマスの物取得.
                var brdTer = scptBrdMng.GetBoard(ope.tmpPos).GetTerrain();

                //障害物がなければ.
                if (
                    brdTer != BoardTerrain.WALL
                    /* TODO:敵たちの座標 */
                ){
                    //操作の猶予を作る.
                    ope.moveBuf = Common.OPE_MOVE_BUF_TM;
                }
            }
        }
    }
    /// <summary>
    /// 巣作成操作の入力.
    /// </summary>
    private void CommandNest()
    {
        //スペースを押したら.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //操作の猶予を作る.
            ope.nestBuf = Common.OPE_NEST_BUF_TM;
        }
    }

    /// <summary>
    /// 食べ物を取った時の処理.
    /// </summary>
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
    /// <summary>
    /// ダメージ処理.
    /// </summary>
    private void HpDamage()
    {
        lizard.hp -= 1;

        //残機0になったら.
        if(lizard.hp <= 0)
        {
            LizardDeath();
        }
    }
    /// <summary>
    /// 死亡処理.
    /// </summary>
    private void LizardDeath()
    {
        /* TODO:プレイヤー死亡 */
    }

    /// <summary>
    /// 移動アニメ開始.
    /// </summary>
    private void MoveAnimStart()
    {
        lizard.isOpeAble = false; //アニメ中は操作不可.
    }
    /// <summary>
    /// 移動アニメ処理.
    /// </summary>
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
    /// <summary>
    /// 移動アニメ中盤.
    /// </summary>
    private void MoveAnimMid()
    {
        //トカゲの移動実行.
        Common.BoardPosSet(gameObject, lizard.pos.x, lizard.pos.y, false);
        //画像の相対座標を反転(→アニメーションを繋ぐのに必要)
        objLizardImg.transform.localPosition = -objLizardImg.transform.localPosition;
    }
    /// <summary>
    /// 移動アニメ終了.
    /// </summary>
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
