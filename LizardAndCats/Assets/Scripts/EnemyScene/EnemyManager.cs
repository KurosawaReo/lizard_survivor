using Const;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    GameManager gm;
    LizardManager lm;


    public void Init(StageId _stageId, GameManager _gm, LizardManager _lm)
    {
        gm = _gm;
        lm = _lm;


        //ステージごとに初期化処理を変更する
        switch(_stageId)
        {
            case StageId.STAGE_01:
                EnemyBase obj = null;

                //obj = Instantiate(prefabSnake).GetComponent<EnemyBase>();
                //obj.Init(new Vector2Int(5, 5), this, gm.cellSize);
                //enemies.Add(obj);

                obj = Instantiate(prefabCat).GetComponent<EnemyBase>();
                obj.Init(new Vector2Int(1, 4), this, gm.cellSize);
                enemies.Add(obj);

                //obj = Instantiate(prefabFox).GetComponent<EnemyBase>();
                //obj.Init(new Vector2Int(4, 1), this, gm.cellSize);
                //enemies.Add(obj);
                

                break;
            case StageId.STAGE_02:
                obj = Instantiate(prefabCat).GetComponent<EnemyBase>();
                obj.Init(new Vector2Int(1, 4), this, gm.cellSize);
                enemies.Add(obj);

                obj = Instantiate(prefabFox).GetComponent<EnemyBase>();
                obj.Init(new Vector2Int(4, 1), this, gm.cellSize);
                enemies.Add(obj);
                obj = Instantiate(prefabCat).GetComponent<EnemyBase>();
                obj.Init(new Vector2Int(5, 3), this, gm.cellSize);
                enemies.Add(obj);
                break;
            case StageId.STAGE_03:
                obj = Instantiate(prefabCat).GetComponent<EnemyBase>();
                obj.Init(new Vector2Int(1, 4), this, gm.cellSize);
                enemies.Add(obj);

                obj = Instantiate(prefabFox).GetComponent<EnemyBase>();
                obj.Init(new Vector2Int(4, 1), this, gm.cellSize);
                enemies.Add(obj);
                obj = Instantiate(prefabCat).GetComponent<EnemyBase>();
                obj.Init(new Vector2Int(5, 3), this, gm.cellSize);
                enemies.Add(obj);

                obj = Instantiate(prefabFox).GetComponent<EnemyBase>();
                obj.Init(new Vector2Int(3, 3), this, gm.cellSize);
                enemies.Add(obj);

                obj = Instantiate(prefabFox).GetComponent<EnemyBase>();
                obj.Init(new Vector2Int(7, 7), this, gm.cellSize);
                enemies.Add(obj);

                obj = Instantiate(prefabFox).GetComponent<EnemyBase>();
                obj.Init(new Vector2Int(6, 5), this, gm.cellSize);
                enemies.Add(obj);

                obj = Instantiate(prefabFox).GetComponent<EnemyBase>();
                obj.Init(new Vector2Int(1, 7), this, gm.cellSize);
                enemies.Add(obj);
                break;
            case StageId.END_LESS:
                break;
        }
    }




    public void EnemiesMove()
    {
        // 昼と夜で違う動きする

        for (int i = 0; i < enemies.Count; i++)
        {
            if(gm.isDayTime)
            {
                //enemies[i].NightMode();
            }
            else
            {
                enemies[i].NightMode();
            }
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


    /// <summary>
    /// プレイヤーに攻撃する処理
    /// </summary>
    public void EnemyAttack()
    {
        lm.Damage();
    }

    /// <summary>
    /// そのマスに落ちてるもの消滅させる
    /// </summary>
    /// <param name="_pos"></param>
    public void EnemyEat(Vector2Int _pos)
    {
        gm.DeleteDrops(_pos);
    }
}
