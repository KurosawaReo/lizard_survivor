using Const;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    public List<DropObj> inventory
    {
        get; set;
    }

    //初期化処理(コンストラクタ)
    public Lizard(Vector2Int _pos, Direction _dir, int _hp, int _tailGage, bool _isTail, bool _isOpeAble, List<DropObj> _inventory)
    {
        pos = _pos;
        dir = _dir;
        hp = _hp;
        tailGage = _tailGage;

        isTail = _isTail;
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
        tmpPos = _tmpPos;
        tmpDir = _tmpDir;
    }
}

public class LizardManager : MonoBehaviour
{
    [Header("- アニメーション用 -")]
    //[Tooltip("タイマー")]
    //float timer;
    //[Tooltip("ブリンク間隔")]
    //float BLINK = 0.25f;

    [Header("- object -")]
    [SerializeField] GameObject objLizardImg; //トカゲの画像obj.

    [Header("- script -")]
    GameManager gm;
    EnemyManager em;
    //[SerializeField] GameObject objBrdMng;    //BoardManager. ※prefabにしたことでこの方法は使えなくなった.
    //BoardManager scptBrdMng;

    //トカゲの情報.
    Lizard lizard = new Lizard(

        new Vector2Int(1, 1), //pos.
        Direction.UP,         //dir.
        Common.LIZARD_HP_MAX, //hp.
        0,                    //tailGage.
        true,                 //isTail.
        true,                 //isOpeAble.

        //inventory.
        new List<DropObj>()
    );
    //トカゲの操作情報.
    LizardOpe ope = new LizardOpe(
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
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

        //トカゲを初期位置に移動.
        Common.BoardPosSet(gameObject, lizard.pos.x, lizard.pos.y, false);
    }

    void Update()
    {
        //プレイヤー操作.
        //移動か巣を作るとターン消費.
        if (gm.IsPlayerInputWait() && gm.isGame)
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
                }
                else if (ope.nestBuf > 0)
                {
                    OpeNestExe();
                    gm.PlayerTurnEnd();
                }

                ope.moveBuf = 0; //リセット.
                ope.nestBuf = 0; //リセット.
            }
            else
            {
                MoveAnim(); //移動アニメ.
            }
        }

        //処理.
        //MoveExe(gameObject, lizard.pos);
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
        var board = gm.GetBoard(lizard.pos);            //[取得]現在マスの情報.
        board.SetObject(DropObj.NEST, "none", 1, 0, 0); //[編集]巣にする.
        gm.SetBoard(lizard.pos, board);                 //[更新]board置き換え.
    }

    /// <summary>
    /// 移動操作の入力.
    /// </summary>
    private void CommandMove()
    {
        //入力があったか.
        var isInput = false;

        //上.
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ope.tmpPos = lizard.pos - Vector2Int.up;
            ope.tmpDir = Direction.UP;
            isInput = true;
        }
        //下.
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ope.tmpPos = lizard.pos - Vector2Int.down;
            ope.tmpDir = Direction.DOWN;
            isInput = true;
        }
        //左.
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ope.tmpPos = lizard.pos + Vector2Int.left;
            ope.tmpDir = Direction.LEFT;
            isInput = true;
        }
        //右.
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ope.tmpPos = lizard.pos + Vector2Int.right;
            ope.tmpDir = Direction.RIGHT;
            isInput = true;
        }

        //操作があった場合のみ.
        if (isInput)
        {
            //範囲外に出ていなければ.
            if (ope.tmpPos.x >= 0 && ope.tmpPos.x < gm.GetBoardAry().GetLength(1) &&
                ope.tmpPos.y >= 0 && ope.tmpPos.y < gm.GetBoardAry().GetLength(0))
            {
                //移動先のマスの物取得.
                var brdTer = gm.GetBoard(ope.tmpPos).GetTerrain();

                //障害物がなければ.
                if (
                    brdTer != BoardTerrain.WALL
                )
                {
                    //敵がいなければ.
                    if (em.IsNoEnemies(ope.tmpPos))
                    {
                        //操作の猶予を作る.
                        ope.moveBuf = Common.OPE_MOVE_BUF_TM;
                    }
                    //敵がいれば移動せずダメージ.
                    else
                    {
                        // プレイヤーが敵に突進したときにダメージを受けるようにする
                        Damage();
                    }
                }
            }
        }
    }
    /// <summary>
    /// 巣作成操作の入力.
    /// </summary>
    private bool CommandNest()
    {
        //スペースを押したら.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 材料を消費したら処理続行
            if (lizard.inventory.Count >= 2)
            {
                lizard.inventory.RemoveAt(0);
                lizard.inventory.RemoveAt(0);

                gm.ConsumeMaterial();

                //var board = scptBrdMng.GetBoard(lizard.pos);    //[取得]現在マスの情報.
                var board = gm.GetBoardSquare(lizard.pos);    //[取得]現在マスの情報.
                board.SetObject(DropObj.NEST, "none", 1, 0, 0); //[編集]巣にする.
                                                                //scptBrdMng.SetBoard(lizard.pos, board);         //[更新]board置き換え.
                gm.SetBoardSquare(lizard.pos, board);         //[更新]board置き換え.
                return true;
            }
        }
        return false;
    }

    //食べ物処理.
    private void EatFood()
    {
        var board = gm.GetBoardSquare(lizard.pos); //[取得]現在マスの情報.

        //尻尾回復ゲージ増加.
        //lizard.tailGage += board.GetDropObj().heal;
        if (!lizard.isTail)
        {
            lizard.tailGage += 5;
            //ゲージが最大になったら
            if (lizard.tailGage >= Common.LIZARD_HEALGAGE_MAX)
            {
                lizard.tailGage = 0;
                lizard.isTail = true; //尻尾復活.
            }
        }

        //食べ物を消す.
        board.SetObject(DropObj.NONE, "none", 0, 0, 0); //[編集]無にする.
        gm.SetBoardSquare(lizard.pos, board);         //[更新]board置き換え.
    }

    //ダメージ処理.
    //public void HpDamage()
    public void Damage()
    {
        //lizard.hp -= 1;

        ////残機0になったら.
        //if (lizard.hp <= 0)
        //{
        //    LizardDeath();
        //}
        //print("ダメージ倉田");

        // 被ダメージ時にプレイヤーを赤くする
        // しっぽ切れてないとき→しっぽが切れる
        if (lizard.isTail)
        {
            lizard.isTail = false;
        }
        // しっぽ切れてるとき→死ぬ
        else
        {
            LizardDeath();
        }
    }
    //死亡処理.
    private void LizardDeath()
    {
        // プレイヤー死亡
        print("負け");
        gm.ShowResult(false);
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
                objLizardImg.transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case Direction.DOWN:
                objLizardImg.transform.localPosition += new Vector3(0, -move, 0);
                objLizardImg.transform.eulerAngles = new Vector3(0, 0, 180);
                break;
            case Direction.RIGHT:
                objLizardImg.transform.localPosition += new Vector3(+move, 0, 0);
                objLizardImg.transform.eulerAngles = new Vector3(0, 0, 270);
                break;
            case Direction.LEFT:
                objLizardImg.transform.localPosition += new Vector3(-move, 0, 0);
                objLizardImg.transform.eulerAngles = new Vector3(0, 0, 90);
                break;
        }

        //前半.
        if (!anim.isMidPass)
        {
            //前半終了.
            if (anim.timer >= Common.LIZARD_MOVE_ANIM_SEC / 2)
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

        //トカゲ画像の相対座標を反転(→アニメーションを繋ぐのに必要)
        objLizardImg.transform.localPosition = -objLizardImg.transform.localPosition;
    }
    /// <summary>
    /// 移動アニメ終了.
    /// </summary>
    private void MoveAnimEnd()
    {
        //移動先のマスの物取得.
        var brdDrop = gm.GetBoard(lizard.pos).GetDropObj();
        
        //食べ物があれば.
        if (brdDrop.type == DropObj.FOOD)
        {
            EatFood(); //食べる処理.
        }
        //素材があれば取得.
        if (brdDrop.type == DropObj.MATERIAL)
        {
            GetMaterial();
        }

        //操作可能に.
        lizard.isOpeAble = true;
        //プレイヤーのターン終了.
        gm.PlayerTurnEnd();
        //位置リセット.
        objLizardImg.transform.localPosition = Vector3.zero;
    }

    public void SetPos(Vector2Int _pos) { lizard.pos = _pos; }
    public Vector2Int GetPos() { return lizard.pos; }

    void GetMaterial()
    {
        if (lizard.inventory.Count < Common.INVENTORY_CNT)
        {
            lizard.inventory.Add(DropObj.MATERIAL);
            gm.GetMaterial();
        }
        var b = gm.GetBoardSquare(lizard.pos);
        b.SetObject(DropObj.NONE, "", 0, 0, 0);
        gm.SetBoardSquare(lizard.pos, b);

    }

    public float GetNormalizedTailGaugeAMount()
    {
        var val = lizard.tailGage;
        var max = Common.LIZARD_HEALGAGE_MAX;

        var norm = (float)val / (float)max;

        return norm;
    }

    public void AddTailGauge()
    {
        if (!lizard.isTail)
        {
            lizard.tailGage += 1;
            if (lizard.tailGage >= Common.LIZARD_HEALGAGE_MAX)
            {
                lizard.tailGage = 0;
                lizard.isTail = true; //尻尾復活.
            }
        }
    }

    public bool IsTail() { return lizard.isTail; }
}
