using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;

public abstract class EnemyBase : MonoBehaviour
{
    protected Vector2Int pos;

    public void Move()
    {
    }

    public void SetPosition()
    {
        var (lb, rt) = Common.GetWorldWindowSize();
        var poss = new Vector3(lb.x + pos.x + 0.5f, rt.y - pos.y - 0.5f, 0);

        transform.position = poss;
    }



    // Start is called before the first frame update
    void Start()
    {
        var board = GameObject.Find("EnemyBoardManger").GetComponent<EnemyBoard>().GetBoard();
        var xlen = board.GetLength(1) - 1;
        var ylen = board.GetLength(0) - 1;
        pos.x = xlen;
        pos.y = ylen;

        //board[ylen - 1, xlen - 1];
        SetPosition();


    }

    // Update is called once per frame
    void Update()
    {
        var board = GameObject.Find("EnemyBoardManger").GetComponent<EnemyBoard>().GetBoard();
        var xlen = board.GetLength(1) - 1;
        var ylen = board.GetLength(0) - 1;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (pos.y != 0)
            {
                pos.y = pos.y - 1;
                SetPosition();
            }
            Debug.Log("à⁄ìÆÇµÇ‹Ç∑");


            //É|ê}ÇÃà íuÇ™îzóÒÇÃÇOî‘ñ⁄ÇÃéûé~Ç‹ÇÈ
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (pos.y != ylen)
            {
                Debug.Log("à⁄ìÆÇµÇ‹Ç∑");
                pos.y = pos.y + 1;
                SetPosition();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (pos.x != 0)
            {
                Debug.Log("à⁄ìÆÇµÇ‹Ç∑");
                pos.x = pos.x - 1;
                SetPosition();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (pos.x != xlen)
            {
                Debug.Log("à⁄ìÆÇµÇ‹Ç∑");
                pos.x = pos.x + 1;
                SetPosition();
            }
        }
    }
}
