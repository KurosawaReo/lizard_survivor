/*
   - Common.cs -
   グローバルで使う定数や関数をまとめた所.
*/
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gloval
{
    /*
       [ボード情報]
       ・床、壁の列挙
       ・なし、巣、食べ物、素材の列挙
    */

    /// <summary>
    /// board配列用:地形情報.
    /// </summary>
    public enum BoardTerrain
    {
        WALL,    //壁.
        GROUND   //床.
    };
    /// <summary>
    /// board配列用:落下物情報.
    /// </summary>
    public enum DropObj
    {
        NONE,     //なし.
        NEST,     //巣.
        FOOD,     //食べ物.
        MATERIAL, //素材.
        TAIL      //尻尾.
    };

    /// <summary>
    /// ステージID.
    /// </summary>
    public enum StageId
    {
        STAGE_01,
        STAGE_02,
        STAGE_03,

        END_LESS,
    }
    /// <summary>
    /// 移動方向.
    /// </summary>
    public enum MoveDir
    {
        UP,    //上
        RIGHT, //右.
        DOWN,  //下.
        LEFT,  //左.

        INVALID, //無効.
    }

    /// <summary>
    /// グローバル定数.
    /// </summary>
    public class Gl_Const
    {
        public const string SCENE_NAME_TITLE = "Title";
        public const string KEY_GAME_LEVEL = "Level";

        public const int   STAGE_LV1_BOARD_SIZE = 4;     //Lv1での盤面サイズ.
        public const int   STAGE_LV2_BOARD_SIZE = 5;     //Lv2での盤面サイズ.
        public const int   STAGE_LV3_BOARD_SIZE = 6;     //Lv3での盤面サイズ.

        public const int   LIZARD_HP_MAX = 1;            //トカゲの最大体力.
        public const int   LIZARD_HEALGAUGE_MAX = 10;    //トカゲの回復ゲージ最大.
        public const int   INVENTORY_CNT = 4;            //インベントリ個数.

        public const float LIZARD_MOVE_ANIM_VEL = 900;   //トカゲの移動アニメの速度.
        public const float LIZARD_MOVE_ANIM_SEC = 0.15f; //トカゲの移動アニメの秒数.

        public const float OPE_MOVE_BUF_TM = 0.15f;      //移動操作バッファ時間.
        public const float OPE_NEST_BUF_TM = 0.15f;      //巣作り操作バッファ時間.
    }

    /// <summary>
    /// グローバル関数.
    /// </summary>
    public class Gl_Func
    {
        /// <summary>
        /// 汎用関数?
        /// </summary>
        /// <param name="_sceneName"></param>
        public static void LoadScene(string _sceneName)
        {
            SceneManager.LoadScene(_sceneName);
        }

        /// <summary>
        /// 画面の左下と右上の世界座標を返す.
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
        public static void BoardPosSet(GameObject _obj, int _x, int _y, int _boardLen, bool _isAlignSize)
        {
            var boardTopLeft = GetBoardTopLeft(_boardLen);  //盤面表示座標(左上)
            var cellSize     = GetBoardCellSize(_boardLen); //セルサイズ.

            //座標計算.
            float x = boardTopLeft.x + _x * cellSize;
            float y = boardTopLeft.y - _y * cellSize;
            //移動.
            _obj.transform.position = new Vector2(x, y);

            //サイズを合わせるなら.
            if (_isAlignSize)
            {
                _obj.transform.localScale = new Vector2(cellSize, cellSize);
            }
        }

        /// <summary>
        /// boardのセル表示サイズを取得.
        /// </summary>
        /// <param name="_boardLen">boardの列数.</param>
        /// <returns>セルのサイズ.</returns>
        public static float GetBoardCellSize(int _boardLen)
        {
            //ウィンドウの端の座標取得.
            var (lb, rt) = GetWorldWindowSize();

            //セルサイズを計算して返す.
            return (rt.y - lb.y) / _boardLen;
        }

        /// <summary>
        /// boardを表示する左上座標の取得.
        /// </summary>
        /// <param name="_wid">画面の横幅.</param>
        /// <param name="_hei">画面の縦幅.</param>
        /// <param name="_boardLen">boardの列数.</param>
        /// <returns>左上座標(x, y)</returns>
        public static Vector2 GetBoardTopLeft(int _boardLen)
        {
            //ウィンドウの端の座標取得.
            var (lb, rt) = GetWorldWindowSize();
            //画面の中心点.
            var center = new Vector2((lb.x+rt.x)/2, (lb.y+rt.y)/2);

            //座標を計算して返す.
            var x = -GetBoardCellSize(_boardLen) * (_boardLen-1) / 2;
            var y = +GetBoardCellSize(_boardLen) * (_boardLen-1) / 2;
            return  center + new Vector2(x, y);
        }

        /// <summary>
        /// 座標値を元に方向取得.
        /// </summary>
        /// <param name="_vec"></param>
        /// <returns></returns>
        public static MoveDir GetMoveDir(Vector2Int _vec)
        {
            //方向別.
            if (_vec == Vector2Int.down)
            {
                return MoveDir.UP;
            }
            else if (_vec == Vector2Int.left)
            {
                return MoveDir.LEFT;
            }
            else if (_vec == Vector2Int.right)
            {
                return MoveDir.RIGHT;
            }
            else if (_vec == Vector2Int.up)
            {
                return MoveDir.DOWN;
            }

            //無効な値.
            return MoveDir.INVALID;
        }
    }
}




