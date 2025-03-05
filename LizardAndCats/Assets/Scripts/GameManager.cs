using Const;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    const int TMP_LEN = 12;




    //盤面データ.
    Board[,] board = new Board[TMP_LEN, TMP_LEN];

    [SerializeField] GameObject squarePrfb; //四角形prefab.


    void Start()
    {
        BoardInit();
        BoardMake();
    }

    private void Update()
    {
        // プレイヤーの入力待ち
    }

    private void BoardInit()
    {

        //盤面ループ.
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = new Board(BoardTerrain.GROUND);


                //周りのマス.
                if (i == 0 || j == 0 || i == board.GetLength(0) - 1 || j == board.GetLength(1) - 1)
                {
                    board[i, j].SetTerrain(BoardTerrain.WALL);
                }
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
                        obj.GetComponent<SpriteRenderer>().color = Color.white;
                        break;
                    case BoardTerrain.GROUND: //床.
                        obj.GetComponent<SpriteRenderer>().color = Color.black;
                        break;

                    default:
                        break;
                }
            }
        }
    }
    public Board[,] GetBoard()
    {

        return board;
    }
}
