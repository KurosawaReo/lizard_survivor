using Const;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
public Vector2Int pos
{
    //プロパティと呼ばれる機能.
    //Unityでのセッタ,ゲッタはこれだけでok. 
    get; set;
}
*/

//プレイヤー(トカゲ)の情報.
public class Lizard
{
    public Vector2Int pos //現在位置.
    { 
        get; set; 
    }
    public int life //残機.
    {
        get; set;
    }

    //初期化処理(コンストラクタ)
    public Lizard(Vector2Int _pos, int _life)
    {
        pos  = _pos;
        life = _life;
    }
}

public class LizardManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] GameObject prfbNest;   //巣prefab.
    [SerializeField] GameObject prfbNestIn; //prefabの入る所.

    [Header("- script -")]
    [SerializeField] GameObject objBrdMng; //BoardManager.
    BoardManager scptBrdMng;

    //トカゲの情報を補間する変数.
    Lizard lizard = new Lizard(
        new Vector2Int(1, 1),   //pos.
        Common.LIZARD_LIFE_MAX  //life.
    );

    void Start()
    {
        scptBrdMng = objBrdMng.GetComponent<BoardManager>();
    }

    void Update()
    {
        //操作.
        CommandMove();
        CommandNest();
        //処理.
        MoveExe(gameObject, lizard.pos);
    }

    //操作:トカゲ移動.
    private void CommandMove()
    {
        Vector2Int tmpPos = lizard.pos; //仮変更用にコピー.

        //上.
        if (Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.UpArrow))
        {
            tmpPos -= Vector2Int.up;
        }
        //下.
        if (Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.DownArrow))
        {
            tmpPos -= Vector2Int.down;
        }
        //左.
        if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tmpPos += Vector2Int.left;
        }
        //右.
        if (Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.RightArrow))
        {
            tmpPos += Vector2Int.right;
        }

        //マスの範囲内である.
        if(tmpPos.x >= 0 && tmpPos.x < Common.BOARD_WID &&
           tmpPos.y >= 0 && tmpPos.y < Common.BOARD_HEI)
        {
            //移動できる空間がある.
            if (
                scptBrdMng.GetBoard(tmpPos).GetTerrain() != BoardTerrain.WALL
                /* TODO: 敵たちの座標 */
            ){
                lizard.pos = tmpPos; //移動実行.
            }
        }
    }
    //操作:巣を作る.
    private void CommandNest()
    {
        //スペースを押したら.
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    //移動実行.
    private void MoveExe(GameObject _obj, Vector2Int _pos)
    {
        //ウィンドウの端の座標取得.
        var (lb, rt) = Common.GetWorldWindowSize();

        //座標計算.
        float x = lb.x+Common.BOARD_BASE_X + ( _pos.x+0.5f) * Common.BOARD_GRID_SIZE;
        float y = rt.y-Common.BOARD_BASE_Y + (-_pos.y-0.5f) * Common.BOARD_GRID_SIZE;
        //移動.
        _obj.transform.position = new Vector2(x, y);
    }
}
