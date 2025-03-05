using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Const;


public class Fox : EnemyBase
{
    //todo:・床、食べ物、素材、巣を移動可能
    //todo:・巣と隣接している場合、巣を攻撃し終わったら疲労する
    //todo:・食べ物を通過したら食べ物が消滅する
    //todo:・素材を通過した場合素材を破壊され消滅する
    //todo:・縦横２マス（二回行動）

    public override void CheckMove()
    {
        throw new System.NotImplementedException();
    }
}
