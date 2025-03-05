using Const;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : EnemyBase
{

    public override void NightMode()
    {
        //疲労してたら疲労回復してスキップ
        if (fatigueCount > 0)
        {
            fatigueCount--;
            return;
        }


        var newPos = ERROR_VEC;

        var posList = GetSearchList();


        // 食べ物を検知
        foreach (var tmpPos in posList)
        {
            if (gm.GetBoardSquare(tmpPos).GetDropObj().type == DropObj.FOOD)
            {
                newPos = tmpPos;
                break;
            }
        }

        if (newPos != ERROR_VEC)
        {
            EatFood(newPos);
            if (gm.GetPlayerPos() != newPos)
            {
                Move(Common.GetMoveVec(newPos - pos));
            }
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
            // プレイヤーと重ならない
            if (gm.GetPlayerPos() != newPos)
            {
                Move(Common.GetMoveVec(newPos - pos));
            }
            return;
        }


        // 素材を検知
        foreach (var tmpPos in posList)
        {
            if (gm.GetBoardSquare(tmpPos).GetDropObj().type == DropObj.MATERIAL)
            {
                newPos = tmpPos;
                break;
            }
        }

        if (newPos != ERROR_VEC)
        {
            BreakMaterial(newPos);
            // プレイヤーと重ならない
            if (gm.GetPlayerPos() != newPos)
            {
                Move(Common.GetMoveVec(newPos - pos));
            }
            return;
        }

        // プレイヤーには攻撃してこない
        if (posList.Contains(gm.GetPlayerPos()))
        {
            posList.Remove(gm.GetPlayerPos());
        }

        // リストから前フレームにいなかった位置をランダム
        posList.Remove(oldPos);

        // 袋小路を考慮してエラー処理
        if (posList.Count > 0)
        {
            var index = Random.Range(0, posList.Count);
            newPos = posList[index];
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
