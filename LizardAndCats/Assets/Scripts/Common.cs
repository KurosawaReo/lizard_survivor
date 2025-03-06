using UnityEngine;
using UnityEngine.SceneManagement;

namespace Const
{
    /*
       [ボード情報]
       ・床、壁の列挙
       ・なし、巣、食べ物、素材の列挙
    */

    //地形情報.
    public enum BoardTerrain
    {
        GROUND,   //床.
        WALL,    //壁.
    };
    //落ちてる物情報.
    public enum DropObj
    {
        NONE,     //なし.
        NEST,     //巣.
        FOOD,     //食べ物.
        MATERIAL, //素材.
        TAIL      //尻尾.
    };

    /// <summary>
    /// 移動方向
    /// </summary>
    public enum MoveVec
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
    }


    public enum StageId
    {
        STAGE_01,
        STAGE_02,
        STAGE_03,

        END_LESS,
    }
    public class Common
    {
        // 定数
        public const string SCENE_NAME_TITLE = "Title";

        public const int   BOARD_WID = 6;            //board配列の横.
        public const int   BOARD_HEI = 6;            //board配列の縦.
        public const float BOARD_BASE_X = 2f;        //board左上の座標x.
        public const float BOARD_BASE_Y = 1f;        //board左上の座標y.
        public const float BOARD_GRID_SIZE = 1.2f;   //マスを描画するサイズ.

        public const int   LIZARD_LIFE_MAX = 1;      //トカゲの最大体力.
        public const int   LIZARD_HEALGAGE_MAX = 10; //トカゲの回復ゲージ最大.

        public const int   INVENTORY_CNT = 4;        //インベントリ個数.

        // 汎用関数
        public static void LoadScene(string _sceneName)
        {
            SceneManager.LoadScene(_sceneName);
        }

        /// <summary>
        /// 画面の左下と右上の世界座標を返す
        /// </summary>
        /// <returns></returns>
        public static (Vector3 leftBottom, Vector3 rightTop) GetWorldWindowSize()
        {
            Vector3 leftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);
            Vector3 rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

            return (leftBottom, rightTop);
        }


        public static MoveVec GetMoveVec(Vector2Int _vec)
        {
            if (_vec == Vector2Int.down)
            {
                return MoveVec.UP;
            }
            else if (_vec == Vector2Int.left)
            {
                return MoveVec.LEFT;
            }
            else if (_vec == Vector2Int.right)
            {
                return MoveVec.RIGHT;
            }
            else
            {
                return MoveVec.DOWN;
            }
        }
    }
}




