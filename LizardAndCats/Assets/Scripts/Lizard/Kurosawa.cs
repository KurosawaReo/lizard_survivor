using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Const; //ヘッダのような使い方.

public class Kurosawa : MonoBehaviour
{
    //盤面データ.
    Board[,] board = new Board[6, 6]
    {
        { Board.CAT,  Board.NONE, Board.NONE, Board.NONE, Board.NONE, Board.NONE   },
        { Board.NONE, Board.NONE, Board.NONE, Board.NONE, Board.NONE, Board.NONE   },
        { Board.NONE, Board.NONE, Board.NONE, Board.NONE, Board.NONE, Board.NONE   },
        { Board.NONE, Board.NONE, Board.NONE, Board.NONE, Board.NONE, Board.NONE   },
        { Board.NONE, Board.NONE, Board.NONE, Board.NONE, Board.NONE, Board.NONE   },
        { Board.NONE, Board.NONE, Board.NONE, Board.NONE, Board.NONE, Board.LIZARD },
    };

    public GameObject squarePrfb; //四角形prefab.

    void Start()
    {
        //ウィンドウの端の座標取得.
        var (lb, rt) = Common.GetWorldWindowSize();

        //盤面ループ.
        for (int i = 0; i < board.GetLength(0); i++){
            for (int j = 0; j < board.GetLength(1); j++){

                //四角形の生成.
                var pos = new Vector3(lb.x+j+0.5f, rt.y-i-0.5f, 0);
                var obj = Instantiate(squarePrfb, pos, Quaternion.identity); //Quaternion.identityは回転の設定(おそらく回転を固定する)

                //マスデータ別.
                switch (board[i, j]) {

                    case Board.CAT:    //猫(敵)
                        obj.GetComponent<SpriteRenderer>().color = Color.red;
                        break;
                    
                    case Board.LIZARD: //トカゲ.
                        obj.GetComponent<SpriteRenderer>().color = Color.green;
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
