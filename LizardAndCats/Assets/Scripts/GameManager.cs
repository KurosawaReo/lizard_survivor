using Const;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    const int TMP_LEN = 12;


    //盤面データ.
    [Header("盤面情報系")]
    [Tooltip("盤面情報")]
    Board[,] board = new Board[TMP_LEN, TMP_LEN];
    [Tooltip("四角形prefab.マスオブジェクト"), SerializeField]
    GameObject squarePrfb;


    [Header("プレイヤー系")]
    [Tooltip("仮のプレイヤーオブジェクト"), SerializeField]
    GameObject player;
    [Tooltip("プレイヤーの座標")]
    Vector2Int playerPos;


    [Header("敵管理系")]
    [Tooltip("敵の管理クラスのインスタンス"), SerializeField]
    EnemyManager em;



    void Start()
    {
        BoardInit();
        BoardMake();

        TmpPlayerInit();

        em.Init(StageId.STAGE_01);
    }

    private void Update()
    {
        GameLoop();
    }

    private void BoardInit()
    {

        //盤面ループ.
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = new Board(BoardTerrain.GROUND);


                ////周りのマス.
                //if (i == 0 || j == 0 || i == board.GetLength(0) - 1 || j == board.GetLength(1) - 1)
                //{
                //    board[i, j].SetTerrain(BoardTerrain.WALL);
                //}
            }
        }
    }

    private void BoardMake()
    {
        //ウィンドウの端の座標取得.
        var (lb, rt) = Common.GetWorldWindowSize();

        //盤面ループ.
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {

                //背景の生成.
                var pos = new Vector3(lb.x + j + 0.5f, rt.y - i - 0.5f, 0);
                var obj = Instantiate(squarePrfb, pos, Quaternion.identity);

                //マスデータ別.
                switch (board[i, j].GetTerrain())
                {
                    case BoardTerrain.WALL:   //壁.
                        //obj.GetComponent<SpriteRenderer>().color = Color.white;
                        break;
                    case BoardTerrain.GROUND: //床.
                        obj.GetComponent<SpriteRenderer>().color = Color.white;
                        break;

                    default:
                        break;
                }
            }
        }
    }

    void TmpPlayerInit()
    {
        playerPos = new Vector2Int(0, 0);
        TmpPlayerDisp();
    }

    /// <summary>
    /// 仮のループ
    /// </summary>
    /// <returns></returns>
    void GameLoop()
    {
        // todo:プレイヤーの入力待ち
        if(PlayerInput())
        {
            // todo:敵の行動まち
            em.EnemiesMove();
        }

    }



    bool PlayerInput()
    {
        var xlen = board.GetLength(1) - 1;
        var ylen = board.GetLength(0) - 1;


        if (Input.GetKeyDown(KeyCode.W))
        {
            if (playerPos.y != 0)
            {
                PlayerMove(MoveVec.UP);
                return true;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (playerPos.y != ylen)
            {
                PlayerMove(MoveVec.DOWN);
                return true;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (playerPos.x != 0)
            {
                PlayerMove(MoveVec.LEFT);
                return true;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (playerPos.x != xlen)
            {
                PlayerMove(MoveVec.RIGHT);
                return true;
            }
        }

        return false;
    }

    void PlayerMove(MoveVec _vec)
    {
        switch (_vec)
        {
            case MoveVec.UP:
                playerPos -= Vector2Int.up;
                break;
            case MoveVec.DOWN:
                playerPos -= Vector2Int.down;
                break;
            case MoveVec.RIGHT:
                playerPos += Vector2Int.right;
                break;
            case MoveVec.LEFT:
                playerPos += Vector2Int.left;
                break;
        }

        TmpPlayerDisp();
    }

    void TmpPlayerDisp()
    {
        var (lb, rt) = Common.GetWorldWindowSize();
        var pos = new Vector3(lb.x + playerPos.x + 0.5f, rt.y - playerPos.y - 0.5f, 0);

        player.transform.position = pos;
    }

    public Board[,] GetBoard()
    {
        return board;
    }

    public Board GetBoardSquare(Vector2Int _pos)
    {
        return board[_pos.y, _pos.x];
    }


    public Vector2Int GetPlayerPos()
    {
        return playerPos;
    }

}
