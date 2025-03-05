using Const;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    const int MAX_ENEMIES = 32;

    [Tooltip("全エネミー配列")]
    List<EnemyBase> enemies = new List<EnemyBase>();
    [Tooltip("猫prefab"), SerializeField]
    GameObject prefabCat;
    [Tooltip("蛇prefab"), SerializeField]
    GameObject prefabSnake;
    [Tooltip("狐prefab"), SerializeField]
    GameObject prefabFox;


    public void Init(StageId _stageId)
    {
        //todo:ステージごとに初期化処理を変更する
        switch(_stageId)
        {
            case StageId.STAGE_01:

                var obj = Instantiate(prefabSnake).GetComponent<EnemyBase>();
                obj.Init(new Vector2Int(5, 5), this);
                enemies.Add(obj);

                obj = Instantiate(prefabCat).GetComponent<EnemyBase>();
                obj.Init(new Vector2Int(8, 8), this);
                enemies.Add(obj);


                break;
            case StageId.STAGE_02:
                break;
            case StageId.STAGE_03:
                break;
            case StageId.END_LESS:
                break;
        }
    }




    public void EnemiesMove()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].CheckMove();
        }
    }


    public bool IsNoEnemies(Vector2Int _pos)
    {
        for(int i = 0;i < enemies.Count;i++)
        {
            if (enemies[i].GetPos() == _pos)
            {
                return false;
            }
        }
        return true;
    }
}
