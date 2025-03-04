using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Const
{
    //public enum Board
    //{
    //    NONE,     //なし.
    //    CAT,      //猫.
    //    LIZARD,   //トカゲ.
    //};

    /*
    ボード情報
    ・床、壁の列挙
    ・なし、巣、食べ物、素材の列挙
    */
    public enum BoardTerrain
    {
        WALL,   //壁.
        GROUND  //床.
    };
    public enum BoardType
    {
        NONE,
        NEST,
        FOOD,
        MATERIAL
    };


    public class Common
    {
        // 定数
        public const string SCENE_NAME_TITLE = "Title";

        public const int BOARD_HEI = 6;
        public const int BOARD_WID = 6;



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
    }
}




