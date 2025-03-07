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
    //csはこれだけでゲッターセッターができる.

    //現在位置.
    public Vector2Int pos { get; set; }
    //現在方向.
    public MoveDir dir { get; set; }
    //体力.
    public int hp { get; set; }
    //尻尾回復ゲージ.
    public int tailGauge { get; set; }

    //尻尾があるかどうか.
    public bool isTail { get; set; }
    //操作可能か.
    public bool isOpeAble { get; set; }

    //持ち物.
    public List<DropObj> inventory { get; set; }

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
    public float moveBuf { get; set; }
    //巣作成操作.
    public float nestBuf { get; set; }
    //入力バッファ.
    public InputBuf inputBuf { get; set; }

    //初期化処理(コンストラクタ)
    public LizardOpe(float _moveBuf, float _nestBuf, InputBuf _inputBuf)
    {
        moveBuf  = _moveBuf;
        nestBuf  = _nestBuf;
        inputBuf = _inputBuf;
    }
}

/// <summary>
/// 入力されたキーを保存する用.
/// </summary>
public class InputBuf
{
    public bool isUp { get; set; }
    public bool isDown { get; set; }
    public bool isLeft { get; set; }
    public bool isRight { get; set; }

    //初期化処理(コンストラクタ)
    public InputBuf(bool _isUp, bool _isDown, bool _isLeft, bool _isRight)
    {
        isUp    = _isUp;
        isDown  = _isDown;
        isLeft  = _isLeft;
        isRight = _isRight;
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
        new InputBuf(
            false,              //isUp.
            false,              //isRight.
            false,              //isDown.
            false               //isLeft.
        )
    );
    //アニメーション情報.
    MoveAnim anim = new MoveAnim(
        0,                      //timer.
        false                   //isMidPass.
    );

    int  boardLen;           //盤面の列数.
    bool isMoveAnim = false; //MoveAnim関数を動かす用.

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
        LizardOperation();

        //移動アニメーションの再生.
        if (isMoveAnim)
        {
            MoveAnim();
        }
    }

    /// <summary>
    /// プレイヤー操作.
    /// 移動 or 巣作成でターン消費.
    /// </summary>
    private void LizardOperation()
    {
        //ゲーム中かつ操作待機中なら.
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
                }

                //操作バッファのリセット.
                ope.moveBuf  = 0;
                ope.nestBuf  = 0;
                ope.inputBuf = new InputBuf(false, false, false, false);
            }
        }
    }

    /// <summary>
    /// 移動操作の入力.
    /// </summary>
    private void CommandMove()
    {
        //上.
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ope.inputBuf.isUp = true;
        }
        //下.
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ope.inputBuf.isDown = true;
        }
        //左.
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ope.inputBuf.isLeft = true;
        }
        //右.
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ope.inputBuf.isRight = true;
        }

        //何かしらの入力があったら.
        if (ope.inputBuf.isUp    ||
            ope.inputBuf.isDown  ||
            ope.inputBuf.isLeft  ||
            ope.inputBuf.isRight 
        ){
            //操作の猶予を作る.
            ope.moveBuf = Gl_Const.OPE_MOVE_BUF_TM;
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
            ope.nestBuf = Gl_Const.OPE_NEST_BUF_TM;
        }
    }

    /// <summary>
    /// 移動操作の実行.
    /// </summary>
    private void OpeMoveExe()
    {
        //仮保存用の変数.
        Vector2Int tmpPos = new Vector2Int();
        MoveDir tmpDir = new MoveDir();

        //入力があれば.
        if (ope.inputBuf.isUp)
        {
            tmpPos = lizard.pos - Vector2Int.up;
            tmpDir = MoveDir.UP;
        }
        else if (ope.inputBuf.isDown)
        {
            tmpPos = lizard.pos - Vector2Int.down;
            tmpDir = MoveDir.DOWN;
        }
        else if(ope.inputBuf.isLeft)
        {
            tmpPos = lizard.pos + Vector2Int.left;
            tmpDir = MoveDir.LEFT;
        }
        else if(ope.inputBuf.isRight)
        {
            tmpPos = lizard.pos + Vector2Int.right;
            tmpDir = MoveDir.RIGHT;
        }

        //範囲外に出ていなければ.
        if (tmpPos.x >= 0 && tmpPos.x < scptGameMng.GetBoardAry().GetLength(1) &&
            tmpPos.y >= 0 && tmpPos.y < scptGameMng.GetBoardAry().GetLength(0))
        {
            //移動先のマスの物取得.
            var brdTer = scptGameMng.GetBoard(tmpPos).GetTerrain();
            //障害物がなければ.
            if (brdTer != BoardTerrain.WALL)
            {
                //敵がいなければ移動.
                if (scptEnmMng.IsNoEnemies(tmpPos))
                {
                    //次の移動方向を確定.
                    lizard.pos = tmpPos;
                    lizard.dir = tmpDir;
                    //アニメーション開始.
                    MoveAnimStart();
                }
                //敵がいて移動できなかった場合、そのままターン終了.
                else
                {
                    scptGameMng.PlayerTurnEnd(); //プレイヤーターン終了.

                    //Damage();
                }
            }
        }
    }
    /// <summary>
    /// 巣作成操作の実行.
    /// </summary>
    private void OpeNestExe()
    {
        //材料が揃っていれば.
        if (lizard.inventory.Count >= Gl_Const.GENERATE_NEST_NEED_CNT)
        {
            //素材の消費.
            for(int i = 0; i < Gl_Const.GENERATE_NEST_NEED_CNT; i++)
            {
                lizard.inventory.RemoveAt(0); //リストから1つ消す.
            }

            scptGameMng.ConsumeMaterial();

            var board = scptGameMng.GetBoardSquare(lizard.pos);  //[取得]現在マスの情報.
            board.SetObject(DropObj.NEST, "", 1, 0, 0);          //[編集]巣にする.
            scptGameMng.SetBoardSquare(lizard.pos, board);       //[更新]board置き換え.

            scptGameMng.PlayerTurnEnd(); //プレイヤーターン終了.
        }
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
        isMoveAnim = true;        //MoveAnim関数を動かす.
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

        lizard.isOpeAble = true; //操作可能に.
        isMoveAnim = false;      //関数の動作を止める.

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
