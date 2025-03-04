using Const;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoard : MonoBehaviour
{
    //盤面データ.
    Board[,] board = new Board[Common.BOARD_WID, Common.BOARD_HEI]
    {
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
    };

    [SerializeField] GameObject squarePrfb; //四角形prefab.

    //    [SerializeField] Sprite lizardImg;
    //    [SerializeField] Sprite catImg;

    void Start()
    {
        BoardInit();
        BoardMake();
    }

    private void BoardInit()
    {
        //盤面ループ.
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {

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

    void Update()
    {

    }
}
