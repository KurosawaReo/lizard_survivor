using Const;
using System.Collections.Generic;
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

    public int tailMax = 30;

    //持ち物.
    //public DropObj[] inventory
    public List<DropObj> inventory
    {
        get; set;
    }

    //初期化処理(コンストラクタ)
    public Lizard(Vector2Int _pos, int _hp, bool _isTail, int _tailGage, List<DropObj> _inventory)
    {
        pos = _pos;
        hp = _hp;
        isTail = _isTail;
        tailGage = _tailGage;
        inventory = _inventory;
    }
}

public class LizardManager : MonoBehaviour
{
    const int MAX_INVENTORY = 4;


    [Header("- アニメーション用 -")]
    [Tooltip("タイマー")]
    float timer;
    [Tooltip("ブリンク間隔")]
    float BLINK = 0.25f;


    [Header("- script -")]
    //[SerializeField] GameObject objBrdMng; //BoardManager.
    //BoardManager scptBrdMng;
    [SerializeField]
    GameManager gm;
    [SerializeField]
    EnemyManager em;

    //トカゲの情報.
    Lizard lizard = new Lizard(

        new Vector2Int(1, 1),   //pos.
        Common.LIZARD_LIFE_MAX, //life.
        true,                   //isTail.
        0,                      //tailGage.

        //inventory.
        //new DropObj[Common.INVENTORY_CNT] {
        //    DropObj.NONE,
        //    DropObj.NONE,
        //    DropObj.NONE,
        //    DropObj.NONE
        //}
        new List<DropObj>()

    );

    void Start()
    {
        //scptBrdMng = objBrdMng.GetComponent<BoardManager>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > BLINK)
        {
            timer = 0;
            var tmp = transform.localScale;
            tmp.x *= -1;
            transform.localScale = tmp;
        }

        //操作.
        //CommandMove();
        //CommandNest();

        // 移動か巣を作るとターン消費
        if (gm.IsPlayerInputWait() && gm.isGame)
        {
            if (CommandMove() || CommandNest())
            {
                gm.PlayerTurnEnd();
            }
        }



        //処理.
        //MoveExe(gameObject, lizard.pos);
    }

    //操作:トカゲ移動.
    private bool CommandMove()
    {
        var ret = false;
        Vector2Int tmpPos = lizard.pos; //仮変更用にコピー.
        var isInput = false;
        var tmp = transform.GetChild(0).transform.rotation;

        //上.
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            tmpPos -= Vector2Int.up;

            tmp.eulerAngles = new Vector3(0, 0, 0);
            isInput = true;
        }
        //下.
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            tmpPos -= Vector2Int.down;
            tmp.eulerAngles = new Vector3(0, 0, 180);

            isInput = true;
        }
        //左.
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tmpPos += Vector2Int.left;
            tmp.eulerAngles = new Vector3(0, 0, 90);

            isInput = true;
        }
        //右.
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            tmpPos += Vector2Int.right;
            tmp.eulerAngles = new Vector3(0, 0, 270);
            isInput = true;
        }

        //範囲外に出ていなければ.
        if (tmpPos.x >= 0 && tmpPos.x < gm.GetBoard().GetLength(1) &&
           tmpPos.y >= 0 && tmpPos.y < gm.GetBoard().GetLength(0))
        {
            //移動先のマスの物取得.
            //var brdTer = scptBrdMng.GetBoard(tmpPos).GetTerrain();
            var brdTer = gm.GetBoardSquare(tmpPos).GetTerrain();
            //var brdObj = scptBrdMng.GetBoard(tmpPos).GetDropObj();
            var brdObj = gm.GetBoardSquare(tmpPos).GetDropObj();

            //障害物がなければ.
            if (
                brdTer != BoardTerrain.WALL &&
                em.IsNoEnemies(tmpPos)
            )
            {
                //移動実行.
                lizard.pos = tmpPos;
                transform.rotation = tmp;
                transform.position = gm.GetCellWorldPosition(lizard.pos);
                if (isInput)
                {
                    ret = true;
                }


                //食べ物があれば.
                if (brdObj.type == DropObj.FOOD)
                {
                    EatFood(); //食べる処理.
                }



                //素材があれば取得
                if (brdObj.type == DropObj.MATERIAL)
                {
                    GetMaterial();
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

        return ret;
    }


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


    //操作:巣を作る.
    private bool CommandNest()
    {
        //スペースを押したら.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // todo:材料を消費したら処理続行
            if(lizard.inventory.Count >= 2)
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
        print("食べた");
        //var board = scptBrdMng.GetBoard(lizard.pos); //[取得]現在マスの情報.
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
        print("ダメージ倉田");

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

    //移動実行.
    private void MoveExe(GameObject _obj, Vector2Int _pos)
    {
        _obj.transform.position = gm.GetCellWorldPosition(_pos);

        ////座標計算.
        //float x = lb.x + Common.BOARD_BASE_X + (_pos.x + 0.5f) * Common.BOARD_GRID_SIZE;
        //float y = rt.y - Common.BOARD_BASE_Y + (-_pos.y - 0.5f) * Common.BOARD_GRID_SIZE;
        ////移動.
        //_obj.transform.position = new Vector2(x, y);
    }


    public Vector2Int GetPos() { return lizard.pos; }

    public void SetPos(Vector2Int _pos) { lizard.pos = _pos; }

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
