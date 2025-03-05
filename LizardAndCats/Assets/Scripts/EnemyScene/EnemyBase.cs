using UnityEngine;
using Const;

public abstract class EnemyBase : MonoBehaviour
{
    protected Vector2Int pos;
    protected GameManager gm;


    public enum MoveVec
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
    }


    virtual public void Init()
    {
        gm = GameObject.Find("GameManger").GetComponent<GameManager>();
        pos = new Vector2Int(gm.GetBoard().GetLength(1) - 1, gm.GetBoard().GetLength(0) - 1);

        SetPosition();
    }

    virtual public void Move(MoveVec _vec)
    {
        switch (_vec)
        {
            case MoveVec.UP:
                pos -= Vector2Int.up;
                break;
            case MoveVec.DOWN:
                pos -= Vector2Int.down;
                break;
            case MoveVec.RIGHT:
                pos += Vector2Int.right;
                break;
            case MoveVec.LEFT:
                pos += Vector2Int.left;
                break;
        }

        SetPosition();
    }

    public void SetPosition()
    {
        var (lb, rt) = Common.GetWorldWindowSize();
        var poss = new Vector3(lb.x + pos.x + 0.5f, rt.y - pos.y - 0.5f, 0);

        transform.position = poss;
    }


    // Update is called once per frame
    void Update()
    {
        var xlen = gm.GetBoard().GetLength(1) - 1;
        var ylen = gm.GetBoard().GetLength(0) - 1;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (pos.y != 0)
            {
                pos.y = pos.y - 1;
                
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (pos.y != ylen)
            {
                pos.y = pos.y + 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (pos.x != 0)
            {
                pos.x = pos.x - 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (pos.x != xlen)
            {
                pos.x = pos.x + 1;
            }
        }
    }
}
