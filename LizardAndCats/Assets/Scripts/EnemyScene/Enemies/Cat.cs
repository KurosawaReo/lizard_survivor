
using UnityEngine;

using Const;

public class Cat : EnemyBase
{
    public override void DayMode()
    {
        if(dayActiveTimer < 0)
        {
            NightMode();
            dayActiveTimer = frequency;
        }
        else
        {
            dayActiveTimer--;
        }
    }



    public override void NightMode()
    {
        //疲労してたら疲労回復してスキップ
        if (fatigueCount > 0)
        {
            fatigueCount--;

            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.blue;
            return;
        }
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;


        var newPos = ERROR_VEC;

        var posList = GetSearchList();

        if (posList.Count == 0)
        {
            return;
        }

        // 巣を検知
        foreach (var tmpPos in posList)
        {
            if (gm.GetBoardSquare(tmpPos).GetDropObj().type == DropObj.NEST)
            {
                newPos = tmpPos;
                break;
            }
        }

        if (newPos != ERROR_VEC)
        {
            BreakNest(newPos);
            return;
        }

        // プレイヤーを検知
        foreach (var tmpPos in posList)
        {
            if (gm.GetPlayerPos() == tmpPos)
            {
                newPos = tmpPos;
                break;
            }
        }

        if (newPos != ERROR_VEC)
        {
            Attack(Common.GetMoveVec(newPos - pos));
            return;
        }



        // リストから前フレームにいなかった位置をランダム
        posList.Remove(oldPos);

        // 袋小路を考慮してエラー処理
        if (posList.Count > 0)
        {
            var index = Random.Range(0, posList.Count);
            newPos = posList[index];
            // 食べ物があったら食べちゃう
            if(gm.GetBoardSquare(newPos).GetDropObj().type == DropObj.FOOD)
            {
                EatFood(newPos);
            }
            Move(Common.GetMoveVec(newPos - pos));
            return;
        }
        else
        {
            newPos = oldPos;
            Move(Common.GetMoveVec(newPos - pos));
            return;
        }


    }
}
