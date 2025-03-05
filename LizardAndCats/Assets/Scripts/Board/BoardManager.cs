/*
   - BoardGenerator.cs -
   黒澤ver.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Const;
using UnityEngine.SceneManagement; //ヘッダのような使い方.

/// <summary>
/// 盤面生成のプログラム.
/// board配列はここに入ってる.
/// </summary>
public class BoardManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] GameObject prfbSquare;   //四角形prefab.
    [SerializeField] GameObject prfbSquareIn; //prefabを入れる場所.

    //  [SerializeField] Sprite lizardImg;
    //  [SerializeField] Sprite catImg;

    //盤面データ.
    Board[,] board = new Board[Common.BOARD_HEI, Common.BOARD_WID] //Board[y, x]
    {
        { new Board(BoardTerrain.WALL)  , new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.WALL)   },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.WALL)   },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.WALL),   new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.WALL),   new Board(BoardTerrain.GROUND) },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.WALL),   new Board(BoardTerrain.WALL),   new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
    };

    void Start()
    {
        BoardMake();
    }

    void Update()
    {

    }

#if false
    //board配列の初期化.
    private void BoardInit()
    {
        //盤面ループ.
        for (int i = 0; i < Common.BOARD_HEI; i++){
            for (int j = 0; j < Common.BOARD_WID; j++){

                //周りのマス.
                if (i == 0 || j == 0 || i == Common.BOARD_HEI - 1 || j == Common.BOARD_WID - 1)
                {
                    board[j, i].SetTerrain(BoardTerrain.WALL);
                }
            }
        }
    }
#endif

    //board配列を元にステージ生成.
    private void BoardMake()
    {
        //ウィンドウの端の座標取得.
        var (lb, rt) = Common.GetWorldWindowSize();

        //盤面ループ.
        for (int i = 0; i < board.GetLength(0); i++){
            for (int j = 0; j < board.GetLength(1); j++){

                //座標の計算.
                float x = lb.x+Common.BOARD_BASE_X + (j+0.5f) * Common.BOARD_GRID_SIZE;
                float y = rt.y-Common.BOARD_BASE_Y - (i+0.5f) * Common.BOARD_GRID_SIZE;
                var pos = new Vector3(x, y, 0);

                //背景用prefabの生成.
                var obj = Instantiate(prfbSquare, prfbSquareIn.transform);
                //位置設定.
                obj.transform.position = pos;
                obj.transform.localScale = new Vector3(Common.BOARD_GRID_SIZE, Common.BOARD_GRID_SIZE, 0);

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

    public void SetBoard(Vector2Int _pos, Board _board)
    {
        board[_pos.y, _pos.x] = _board;
    }
    public Board GetBoard(Vector2Int _pos)
    {
        return board[_pos.y, _pos.x];
    }
}
