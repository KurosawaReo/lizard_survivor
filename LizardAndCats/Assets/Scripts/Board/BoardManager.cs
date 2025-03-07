/*
   - BoardGenerator.cs -
   仮で作ったもの.
   おそらくGameManagerとして生まれ変わったため
   ここはもう使われていない.
*/
using Gloval;
using UnityEngine;

/// <summary>
/// 盤面生成のプログラム.
/// board配列はここに入ってる.
/// </summary>
public class BoardManager : MonoBehaviour
{
    [Header("- prefab -")]
    [SerializeField] GameObject prfbSquare;    //四角形prefab.
    [SerializeField] GameObject prfbSquareIn;  //prefabを入れる場所.
    [Space]
    [SerializeField] GameObject prfbNest;      //巣prefab.
    [SerializeField] GameObject prfbFood;      //食べ物prefab.
    [SerializeField] GameObject prfbMaterial;  //素材prefab.
    //[SerializeField] GameObject prfbTail;      //尻尾prefab.
    [SerializeField] GameObject prfbDropObjIn; //prefabを入れる場所.

    //  [SerializeField] Sprite lizardImg;
    //  [SerializeField] Sprite catImg;

    //盤面データ.
    Board[,] board = new Board[Gl_Const.STAGE_LV3_BOARD_SIZE, Gl_Const.STAGE_LV3_BOARD_SIZE] //Board[y, x]
    {
        { new Board(BoardTerrain.WALL)  , new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.WALL)   },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.WALL)   },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.WALL),   new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.WALL),   new Board(BoardTerrain.GROUND) },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
        { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.WALL),   new Board(BoardTerrain.WALL),   new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND) },
    };
    public void SetBoard(Vector2Int _pos, Board _board)
    {
        board[_pos.y, _pos.x] = _board;
    }
    public Board GetBoard(Vector2Int _pos)
    {
        return board[_pos.y, _pos.x];
    }

    void Start()
    {
        BoardInit();
        BoardMakeTer();
    }

    void Update()
    {
        BoardMakeDrop();
    }

    //board配列の初期化.
    private void BoardInit()
    {
        //食べ物の設置(仮)
        //board[1, 1].SetObject(DropObj.FOOD, "foodA", 0, 3, 0);
        //board[1, 2].SetObject(DropObj.FOOD, "foodB", 0, 3, 0);
        //board[1, 3].SetObject(DropObj.FOOD, "foodC", 0, 3, 0);
        //board[1, 4].SetObject(DropObj.NEST, "foodC", 0, 3, 0);
        //board[1, 5].SetObject(DropObj.MATERIAL, "foodC", 0, 3, 0);
        //board[2, 3].SetObject(DropObj.TAIL, "foodC", 0, 3, 0);

        /*
        //盤面ループ.
        for (int i = 0; i < Common.BOARD_HEI; i++){
            for (int j = 0; j < Common.BOARD_WID; j++){

                
            }
        }
        */
    }

    //board配列を元にステージ生成.
    private void BoardMakeTer()
    {
        //盤面ループ.
        for (int i = 0; i < board.GetLength(0); i++) {
            for (int j = 0; j < board.GetLength(1); j++) {

                //prefabの生成.
                var ter = Instantiate(prfbSquare, prfbSquareIn.transform);
                Gl_Func.BoardPosSet(ter, j, i, board.GetLength(0), true);

                //boardの地形別.
                switch (board[i, j].GetTerrain())
                {
                    case BoardTerrain.WALL: //壁.
                        ter.GetComponent<SpriteRenderer>().color = Color.white;
                        break;
                    case BoardTerrain.GROUND: //床.
                        ter.GetComponent<SpriteRenderer>().color = Color.black;
                        break;
                }
            }
        }
    }
    //board配列を元にステージ生成.
    private void BoardMakeDrop()
    {
        //盤面ループ.
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                //boardの落下物別.
                switch (board[i, j].GetDropObj().type)
                {
                    case DropObj.NONE: //無し.
                        break;
                    case DropObj.NEST: //巣.
                        var nest = Instantiate(prfbNest, prfbDropObjIn.transform);
                        Gl_Func.BoardPosSet(nest, j, i, board.GetLength(0), false);
                        break;
                    case DropObj.FOOD: //食べ物.
                        var food = Instantiate(prfbFood, prfbDropObjIn.transform);
                        Gl_Func.BoardPosSet(food, j, i, board.GetLength(0), false);
                        break;
                    case DropObj.MATERIAL: //素材.
                        var material = Instantiate(prfbMaterial, prfbDropObjIn.transform);
                        Gl_Func.BoardPosSet(material, j, i, board.GetLength(0), false);
                        break;
                    //case DropObj.TAIL: //尻尾.
                    //    var tail = Instantiate(prfbTail, prfbDropObjIn.transform);
                    //    Gl_Func.BoardPosSet(tail, j, i, board.GetLength(0), false);
                    //    break;
                }
            }
        }
    }
}
