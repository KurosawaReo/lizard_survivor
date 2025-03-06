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
        WALL,    //壁.
        GROUND   //床.
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
    //4方向.
    public enum Direction
    {
        UP,    //上
        RIGHT, //右.
        DOWN,  //下.
        LEFT,  //左.
    }

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
        public const string KEY_GAME_LEVEL = "Level";

        public const int    BOARD_WID = 6;             //board配列の横.
        public const int    BOARD_HEI = 6;             //board配列の縦.
        public const float BOARD_BASE_X = 2f;         //board左上の座標x.
        public const float BOARD_BASE_Y = 1f;         //board左上の座標y.
        public const float BOARD_GRID_SIZE = 1.2f;    //マスを描画するサイズ.

        public const int    LIZARD_HP_MAX = 1;         //トカゲの最大体力.
        public const int    LIZARD_HEALGAGE_MAX = 10;  //トカゲの回復ゲージ最大.

        public const float LIZARD_MOVE_ANIM_VEL = 6.0f;  //トカゲの移動アニメの速度.
        public const float LIZARD_MOVE_ANIM_SEC = 0.15f; //トカゲの移動アニメの秒数.

        public const float OPE_MOVE_BUF_TM = 0.15f;     //移動操作バッファ時間.
        public const float OPE_NEST_BUF_TM = 0.15f;     //巣作り操作バッファ時間.

        public const int   INVENTORY_CNT = 4;         //インベントリ個数.

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

        /// <summary>
        /// board上の座標を元に、オブジェクトを配置する.
        /// </summary>
        /// <param name="_obj">配置するオブジェクト.</param>
        /// <param name="_x">配置するx座標.</param>
        /// <param name="_y">配置するy座標.</param>
        /// <param name="_isAlignSize">グリッドにサイズを合わせるか.</param>
        public static void BoardPosSet(GameObject _obj, int _x, int _y, bool _isAlignSize)
        {
            //ウィンドウの端の座標取得.
            var (lb, rt) = GetWorldWindowSize();

            //座標計算.
            float x = lb.x + BOARD_BASE_X + (_x + 0.5f) * BOARD_GRID_SIZE;
            float y = rt.y - BOARD_BASE_Y - (_y + 0.5f) * BOARD_GRID_SIZE;
            //移動.
            _obj.transform.position = new Vector2(x, y);

            //サイズを合わせるなら.
            if (_isAlignSize)
            {
                _obj.transform.localScale = new Vector2(BOARD_GRID_SIZE, BOARD_GRID_SIZE);
            }
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




