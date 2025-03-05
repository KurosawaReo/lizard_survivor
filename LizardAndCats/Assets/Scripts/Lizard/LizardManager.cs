using Const;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤー(リザード)の情報.
public class Lizard
{
    //宣言.
    public Vector2Int pos
    {
        //プロパティと呼ばれる機能.
        //Unityでのセッタ,ゲッタはこれだけでok.
        get; set;
    }

    //初期化処理(コンストラクタ)
    public Lizard(Vector2Int _pos)
    {
        pos = _pos;
    }
}

public class LizardManager : MonoBehaviour
{
    [Header("- prefab -")]

    Lizard lizard = new Lizard(new Vector2Int(0, 0)); //初期値.

    void Start()
    {
        
    }

    void Update()
    {
        MoveInput();
        MoveExe();
    }

    //移動操作.
    private void MoveInput()
    {
        //上.
        if (Input.GetKeyDown(KeyCode.W) || 
            Input.GetKeyDown(KeyCode.UpArrow)
        ){
            if (lizard.pos.y > 0)
            {
                lizard.pos -= Vector2Int.up;
            }
        }
        //下.
        if (Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.DownArrow)
        ){
            if (lizard.pos.y < Common.BOARD_HEI-1)
            {
                lizard.pos -= Vector2Int.down;
            }
        }
        //左.
        if (Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.LeftArrow)
        ){
            if (lizard.pos.x > 0)
            {
                lizard.pos += Vector2Int.left;
            }
        }
        //右.
        if (Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.RightArrow)
        ){
            if (lizard.pos.x < Common.BOARD_WID-1)
            {
                lizard.pos += Vector2Int.right;
            }
        }
    }
    //移動実行.
    private void MoveExe()
    {
        //ウィンドウの端の座標取得.
        var (lb, rt) = Common.GetWorldWindowSize();

        //座標計算.
        float x = lb.x+Common.BOARD_BASE_X + ( lizard.pos.x+0.5f) * Common.BOARD_GRID_SIZE;
        float y = rt.y+Common.BOARD_BASE_Y + (-lizard.pos.y-0.5f) * Common.BOARD_GRID_SIZE;
        //移動.
        transform.position = new Vector2(x, y);
    }
}
