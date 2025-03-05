using Const;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : EnemyBase
{

    public override void CheckMove()
    {
        var vec = ERROR_VEC;

        var posList = GetCheckList();

        // 食べ物を検知
        foreach (var pos in posList)
        {
            if(
                gm.
                GetBoardSquare(pos).
                GetBoardObject().
                type == BoardType.FOOD)
            {
                vec = pos;
                break;
            }
        }

        if(vec != ERROR_VEC)
        {
            Move(Common.GetMoveVec(vec - pos));
            EatFood();
            return;
        }

        // 巣を検知
        foreach (var pos in posList)
        {
            if (gm.GetBoardSquare(pos).GetBoardObject().type == BoardType.NEST)
            {
                vec = pos;
                break;
            }
        }

        if (vec != ERROR_VEC)
        {
            Move(Common.GetMoveVec(vec - pos));
            BreakNest();
            return;
        }


        // 素材を検知
        foreach (var pos in posList)
        {
            if (gm.GetBoardSquare(pos).GetBoardObject().type == BoardType.MATERIAL)
            {
                vec = pos;
                break;
            }
        }

        if (vec != ERROR_VEC)
        {
            Move(Common.GetMoveVec(vec - pos));
            BreakMaterial();
            return;
        }

        // リストから前フレームにいなかった位置をランダム
        posList.Remove(oldPos);
        // 袋小路を考慮してエラー処理
        if (posList.Count > 0)
        {
            var index = Random.Range(0, posList.Count);
            vec = posList[index];
            Move(Common.GetMoveVec(vec - pos));
            return;
        }
        else
        {
            vec = oldPos;
            Move(Common.GetMoveVec(vec - pos));
            return;
        }


    }
}
