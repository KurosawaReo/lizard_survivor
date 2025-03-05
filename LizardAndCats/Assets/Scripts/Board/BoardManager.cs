/*
   - BoardGenerator.cs -
   製作:黒澤
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Const; //ヘッダのような使い方.

/// <summary>
/// 盤面生成のプログラム.
/// board配列はここに入ってる.
/// </summary>
public class BoardManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] GameObject prfbSquare; //四角形prefab.

    [Header("- object -")]
    [SerializeField] GameObject objBoardObjs; //生成するobjを入れる場所.

    //  [SerializeField] Sprite lizardImg;
    //  [SerializeField] Sprite catImg;

    //盤面データ.
    Board[,] board = new Board[Common.BOARD_HEI, Common.BOARD_WID] //[y, x]
    {
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
    };

    void Start()
    {
        BoardInit();
        BoardMake();
    }

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
    //board配列を元にステージ生成.
    private void BoardMake()
    {
        //ウィンドウの端の座標取得.
        var (lb, rt) = Common.GetWorldWindowSize();

        //盤面ループ.
        for (int i = 0; i < board.GetLength(0); i++){
            for (int j = 0; j < board.GetLength(1); j++){

                //背景の生成.
                var pos = new Vector3(lb.x + j + 0.5f, rt.y - i - 0.5f, 0);

#if false //TODO:どうするか?
                var obj = Instantiate(prfbSquare, pos, Quaternion.identity);
#else
                //prefab生成.
                var obj = Instantiate(prfbSquare, objBoardObjs.transform);
                //位置設定.
                obj.transform.position = pos;
#endif
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
}
