using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Const
{
    public class Common
    {
        // ’è”
        public const string SCENE_NAME_TITLE = "Title";



        // ”Ä—pŠÖ”
        public static void LoadScene(string _sceneName)
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}




