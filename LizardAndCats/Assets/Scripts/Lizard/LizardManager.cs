/*
   - LizardManager.cs -
   トカゲのメインプログラム.

   (Lizard = トカゲ)
*/
using Gloval;
using System.Collections.Generic;
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
    public MoveDir dir
    {
        get; set;
    }
    //体力.
    public int hp
    {
        get; set;
    }
    //尻尾回復ゲージ.
    public int tailGauge
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
    public Lizard(Vector2Int _pos, MoveDir _dir, int _hp, int _tailGauge, bool _isTail, bool _isOpeAble, List<DropObj> _inventory)
    {
        pos = _pos;
        dir = _dir;
        hp = _hp;
        tailGauge = _tailGauge;

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
    public Vector2Int nextPos
    {
        get; set;
    }
    //仮保存:方向.
    public MoveDir nextDir
    {
        get; set;
    }

    //初期化処理(コンストラクタ)
    public LizardOpe(float _moveBuf, float _nestBuf, Vector2Int _tmpPos, MoveDir _tmpDir)
    {
        moveBuf = _moveBuf;
        nestBuf = _nestBuf;
        nextPos = _tmpPos;
        nextDir = _tmpDir;
    }
}

/// <summary>
/// トカゲのメインプログラム.
/// </summary>
public class LizardManager : MonoBehaviour
{
    //[Header("- アニメーション用 -")]
    //[Tooltip("タイマー")]
    //float timer;
    //[Tooltip("ブリンク間隔")]
    //float BLINK = 0.25f;

    [Header("- object -")]
    [SerializeField] GameObject objLizardImg; //トカゲの画像obj.

    [Header("- script -")]
    GameManager scptGameMng;
    EnemyManager scptEnmMng;

    //トカゲの情報.
    Lizard lizard = new Lizard(
        new Vector2Int(1, 1),   //pos.
        MoveDir.UP,             //dir.
        Gl_Const.LIZARD_HP_MAX, //hp.
        0,                      //tailGauge.
        true,                   //isTail.
        true,                   //isOpeAble.
        new List<DropObj>()     //inventory.
    );
    //トカゲの操作情報.
    LizardOpe ope = new LizardOpe(
        0,                      //moveBuf. 
        0,                      //nestBuf.
        new Vector2Int(0, 0),   //tmpPos.
        MoveDir.UP              //tmpDir.
    );
    //アニメーション情報.
    MoveAnim anim = new MoveAnim(
        0,                      //timer.
        false                   //isMidPass.
    );

    int boardLen; //盤面の列数;

    void Start()
    {
        //別script取得.
        scptGameMng = GameObject.Find("GameManager").GetComponent<GameManager>();
        scptEnmMng  = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

        //盤面の列数取得.
        boardLen = scptGameMng.GetBoardAry().GetLength(0);
        //トカゲを初期位置に移動.
        Gl_Func.BoardPosSet(gameObject, lizard.pos.x, lizard.pos.y, boardLen, false);
    }

    void Update()
    {
        //プレイヤー操作.
        //移動 or 巣作成でターン消費.
        if (scptGameMng.IsPlayerInputWait() && scptGameMng.isGame)
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
                    scptGameMng.PlayerTurnEnd();
                }

                ope.moveBuf = 0; //リセット.
                ope.nestBuf = 0; //リセット.
            }
            else
            {
                MoveAnim(); //移動アニメ.
            }
        }
    }

    /// <summary>
    /// 移動操作の実行.
    /// </summary>
    private void OpeMoveExe()
    {
        //データ更新.
        lizard.pos = ope.nextPos;
        lizard.dir = ope.nextDir;
        //アニメーション開始.
        MoveAnimStart();
    }
    /// <summary>
    /// 巣作成操作の実行.
    /// </summary>
    private void OpeNestExe()
    {
        var board = scptGameMng.GetBoard(lizard.pos);   //[取得]現在マスの情報.
        board.SetObject(DropObj.NEST, "none", 1, 0, 0); //[編集]巣にする.
        scptGameMng.SetBoard(lizard.pos, board);        //[更新]board置き換え.
    }

    /// <summary>
    /// 移動操作の入力.
    /// </summary>
    private void CommandMove()
    {
        //仮設定用の変数.
        Vector2Int tmpPos = new Vector2Int();
        MoveDir tmpDir = new MoveDir();
        //入力があったかどうか.
        var isInput = false;

        //上.
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            tmpPos = lizard.pos - Vector2Int.up;
            tmpDir = MoveDir.UP;
            isInput = true;
        }
        //下.
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            tmpPos = lizard.pos - Vector2Int.down;
            tmpDir = MoveDir.DOWN;
            isInput = true;
        }
        //左.
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tmpPos = lizard.pos + Vector2Int.left;
            tmpDir = MoveDir.LEFT;
            isInput = true;
        }
        //右.
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            tmpPos = lizard.pos + Vector2Int.right;
            tmpDir = MoveDir.RIGHT;
            isInput = true;
        }

        //操作があった場合のみ.
        if (isInput)
        {
            //範囲外に出ていなければ.
            if (tmpPos.x >= 0 && tmpPos.x < scptGameMng.GetBoardAry().GetLength(1) &&
                tmpPos.y >= 0 && tmpPos.y < scptGameMng.GetBoardAry().GetLength(0))
            {
                //次の移動方向を確定.
                ope.nextPos = tmpPos;
                ope.nextDir = tmpDir;

                //移動先のマスの物取得.
                var brdTer = scptGameMng.GetBoard(ope.nextPos).GetTerrain();
                //障害物がなければ.
                if (brdTer != BoardTerrain.WALL)
                {
                    //敵がいなければ移動.
                    if (scptEnmMng.IsNoEnemies(ope.nextPos))
                    {
                        //操作の猶予を作る.
                        ope.moveBuf = Gl_Const.OPE_MOVE_BUF_TM;
                    }
                    //敵がいて移動できなかった場合、そのままターン終了.
                    else
                    {
                        //プレイヤーのターン終了.
                        scptGameMng.PlayerTurnEnd();

                        //Damage();
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

                scptGameMng.ConsumeMaterial();

                var board = scptGameMng.GetBoardSquare(lizard.pos);  //[取得]現在マスの情報.
                board.SetObject(DropObj.NEST, "none", 1, 0, 0);      //[編集]巣にする.
                scptGameMng.SetBoardSquare(lizard.pos, board);       //[更新]board置き換え.
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 食べ物処理.
    /// </summary>
    private void EatFood()
    {
        var board = scptGameMng.GetBoardSquare(lizard.pos); //[取得]現在マスの情報.

        //尻尾回復ゲージ増加.
        if (!lizard.isTail)
        {
            lizard.tailGauge += 5;
            //ゲージが最大になったら
            if (lizard.tailGauge >= Gl_Const.LIZARD_HEALGAUGE_MAX)
            {
                lizard.tailGauge = 0;
                lizard.isTail = true; //尻尾復活.
            }
        }

        //食べ物を消す.
        board.SetObject(DropObj.NONE, "none", 0, 0, 0); //[編集]無にする.
        scptGameMng.SetBoardSquare(lizard.pos, board);  //[更新]board置き換え.
    }

    /// <summary>
    /// ダメージ処理.
    /// </summary>
    public void Damage()
    {
        /*
        lizard.hp -= 1;

        //残機0になったら.
        if (lizard.hp <= 0)
        {
            LizardDeath();
        }
        */

        // 被ダメージ時にプレイヤーを赤くする
        // しっぽ切れてないとき→しっぽが切れる
        if (lizard.isTail)
        {
            lizard.isTail = false;
        }
        // しっぽ切れてるとき→死ぬ
        else
        {
            Death();
        }
    }
    /// <summary>
    /// 死亡処理.
    /// </summary>
    private void Death()
    {
        // プレイヤー死亡
        scptGameMng.ShowResult(false);
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
        float move = Gl_Const.LIZARD_MOVE_ANIM_VEL * transform.localScale.x * Time.deltaTime;

        //トカゲの向き別.
        switch (lizard.dir)
        {
            case MoveDir.UP:
                objLizardImg.transform.localPosition += new Vector3(0, +move, 0);
                objLizardImg.transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case MoveDir.DOWN:
                objLizardImg.transform.localPosition += new Vector3(0, -move, 0);
                objLizardImg.transform.eulerAngles = new Vector3(0, 0, 180);
                break;
            case MoveDir.RIGHT:
                objLizardImg.transform.localPosition += new Vector3(+move, 0, 0);
                objLizardImg.transform.eulerAngles = new Vector3(0, 0, 270);
                break;
            case MoveDir.LEFT:
                objLizardImg.transform.localPosition += new Vector3(-move, 0, 0);
                objLizardImg.transform.eulerAngles = new Vector3(0, 0, 90);
                break;
        }

        //前半.
        if (!anim.isMidPass)
        {
            //前半終了.
            if (anim.timer >= Gl_Const.LIZARD_MOVE_ANIM_SEC / 2)
            {
                anim.isMidPass = true;
                MoveAnimMid();
            }
        }
        //後半.
        else
        {
            //後半終了.
            if (anim.timer >= Gl_Const.LIZARD_MOVE_ANIM_SEC)
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
        Gl_Func.BoardPosSet(gameObject, lizard.pos.x, lizard.pos.y, boardLen, false);

        //トカゲ画像の相対座標を反転(→アニメーションを繋ぐのに必要)
        objLizardImg.transform.localPosition = -objLizardImg.transform.localPosition;
    }
    /// <summary>
    /// 移動アニメ終了.
    /// </summary>
    private void MoveAnimEnd()
    {
        //移動先のマスの物取得.
        var brdDrop = scptGameMng.GetBoard(lizard.pos).GetDropObj();
        //落下物別.
        switch (brdDrop.type)
        {
            case DropObj.FOOD:     EatFood();     break; //食べ物があれば食べる.
            case DropObj.MATERIAL: GetMaterial(); break; //素材があれば取得.
        }

        //操作可能に.
        lizard.isOpeAble = true;
        //位置リセット.
        objLizardImg.transform.localPosition = Vector3.zero;

        //プレイヤーのターン終了.
        scptGameMng.PlayerTurnEnd();
    }

    ///////////////////////////////////////////////////////////////
    // ～詳細の分からない関数たち～
    // 作:たぶん西田先輩.

    public void SetPos(Vector2Int _pos) { lizard.pos = _pos; }
    public Vector2Int GetPos() { return lizard.pos; }

    void GetMaterial()
    {
        if (lizard.inventory.Count < Gl_Const.INVENTORY_CNT)
        {
            lizard.inventory.Add(DropObj.MATERIAL);
            scptGameMng.GetMaterial();
        }
        var b = scptGameMng.GetBoardSquare(lizard.pos);
        b.SetObject(DropObj.NONE, "", 0, 0, 0);
        scptGameMng.SetBoardSquare(lizard.pos, b);

    }

    public float GetNormalizedTailGaugeAMount()
    {
        var val = lizard.tailGauge;
        var max = Gl_Const.LIZARD_HEALGAUGE_MAX;

        var norm = (float)val / (float)max;

        return norm;
    }

    public void AddTailGauge()
    {
        if (!lizard.isTail)
        {
            lizard.tailGauge += 1;
            if (lizard.tailGauge >= Gl_Const.LIZARD_HEALGAUGE_MAX)
            {
                lizard.tailGauge = 0;
                lizard.isTail = true; //尻尾復活.
            }
        }
    }

    public bool IsTail() { return lizard.isTail; }
}
