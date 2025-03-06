using Const;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    const int TMP_LEN = 12;


    //盤面データ.
    [Header("- 盤面情報系 -")]
    [Tooltip("四角形prefab.マスオブジェクト"), SerializeField]
    GameObject squarePrfb;
    [Tooltip("盤面情報")]
    Board[,] board;
    [Tooltip("盤面のインスタンス")]
    Square[,] boardInst = new Square[TMP_LEN, TMP_LEN];
    [Tooltip("セルサイズ")]
    public float cellSize { get; private set; }
    [Tooltip("道の画像"), SerializeField]
    Sprite sprGround;
    [Tooltip("道の画像"), SerializeField]
    Sprite sprWall;


    [Header("- 敵管理系 -")]
    [Tooltip("敵の管理クラスのインスタンス"), SerializeField]
    EnemyManager em;


    [Header("- プレイヤー系 -")]
    [Tooltip("プレイヤーprefab"), SerializeField]
    GameObject prefabPlayer;
    [Tooltip("プレイヤーインスタンス")]
    LizardManager lm;
    [Tooltip("プレイヤーの入力受付待機中フラグ")]
    bool isPlayerWait = true;

    [Header("- ゲーム管理系 -")]
    [Tooltip("受け取るステージのID")]
    StageId stageId;
    [Tooltip("ゲーム中かどうか")]
    public bool isGame = true;
    [Tooltip("現在の経過ターン数")]
    int currentElapsedTurns = 0;
    [Tooltip("昼の長さ")]
    const int LENGTH_DAYTIME = 2;
    [Tooltip("夜の長さ")]
    const int LENGTH_NIGHT = 2;
    [Tooltip("終了に必要な日数")]
    int completeDays = 12;
    [Tooltip("昼かどうか")]
    public bool isDayTime = true;
    [Tooltip("昼の画像"), SerializeField]
    Sprite sprDayTime;
    [Tooltip("夜の画像"), SerializeField]
    Sprite sprNight;
    [Tooltip("回転する画像"), SerializeField]
    Image imgRotate;

    [Tooltip("現在の昼or夜を加算する変数")]
    int currentTimeZoneCount = 0;





    [Header("- UI系 -")]
    [Tooltip("太陽の画像"), SerializeField]
    Image imgSun;
    [Tooltip("経過ターンのテキスト"), SerializeField]
    Text textElapsedTurns;
    [Tooltip("残りのターン数"), SerializeField]
    Text textRemainingTurns;
    [Tooltip("リザルトパネル"), SerializeField]
    Image imgResultPanel;
    [Tooltip("勝敗の文字"), SerializeField]
    Text textResult;
    [Tooltip("しっぽゲージ"), SerializeField]
    Image imgTailGauge;
    [Tooltip("しっぽアイコン"), SerializeField]
    Image imgTailIcon;
    [Tooltip("インベントリリスト"), SerializeField]
    List<GameObject> listInventory;
    [Tooltip("勝利画像"), SerializeField]
    Image imgWin;
    [Tooltip("敗北画像"), SerializeField]
    Image imgLose;



    const int LV1 = 4;
    const int LV2 = 5;
    const int LV3 = 6;





    void Start()
    {
        //todo:エンドレスモード
        stageId = StageId.END_LESS;



        lm = Instantiate(prefabPlayer).GetComponent<LizardManager>();


        BoardInit();
        BoardMake();


        // UIの初期化
        UIDisplay();

        imgResultPanel.gameObject.SetActive(false);

        var ls = lm.transform.localScale;
        ls *= cellSize;
        lm.transform.localScale = ls;
        lm.transform.position = GetCellWorldPosition(lm.GetPos());


        em.Init(stageId, this, lm);


    }



    private void BoardInit()
    {


        switch (stageId)
        {
            case StageId.STAGE_01:
                //盤面データ.
                lm.SetPos(new Vector2Int());
                board = new Board[LV1, LV1]
                {
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND)*/},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND)*/},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND)*/},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND)*/},
                    //{ new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)},
                };
                boardInst = new Square[LV1, LV1];
                completeDays = 72;
                break;
            case StageId.STAGE_02:
                lm.SetPos(new Vector2Int());

                board = new Board[LV2, LV2]
                {
                    //{ new Board(BoardTerrain.GROUND)  , new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)   },
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND)   */},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND) */},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*,   new Board(BoardTerrain.GROUND) */},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND) */},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND) */},
                };
                boardInst = new Square[LV2, LV2];
                completeDays = 108;
                break;
            case StageId.STAGE_03:
                lm.SetPos(new Vector2Int());
                board = new Board[LV3, LV3]
                {
                    //{ new Board(BoardTerrain.GROUND)  , new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)    },
                    //{ new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)    },
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)*/},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)    */},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)    */},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)    */},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)    */},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)    */},
                };
                boardInst = new Square[LV3, LV3];
                completeDays = 144;
                break;
            case StageId.END_LESS:
                lm.SetPos(new Vector2Int());
                board = new Board[LV3, LV3]
                {
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)*/},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)    */},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)    */},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)    */},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)    */},
                    { new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND),   new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)/*, new Board(BoardTerrain.GROUND), new Board(BoardTerrain.GROUND)    */},
                };
                boardInst = new Square[LV3, LV3];
                completeDays = int.MaxValue;
                break;
        }
    }

    private void BoardMake()
    {
        //ウィンドウの端の座標取得.
        var (lb, rt) = Common.GetWorldWindowSize();
        cellSize = (rt.y - lb.y) / board.GetLength(0);
        var center = new Vector3((lb.x + rt.x) / 2, (lb.y + rt.y) / 2, 0);
        var boardTopLeft = center + new Vector3(-cellSize * (board.GetLength(0) - 1) / 2, cellSize * (board.GetLength(0) - 1) / 2, 0);


        //盤面ループ.
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {

                //背景の生成.
                //var pos = new Vector3(lb.x + j + 0.5f, rt.y - i - 0.5f, 0);
                var pos = boardTopLeft + new Vector3(j * cellSize, -i * cellSize, 0);
                var obj = Instantiate(squarePrfb, pos, Quaternion.identity);

                var ls = obj.transform.localScale;
                ls *= cellSize;
                obj.transform.localScale = ls;

                //マスデータ別.
                switch (board[i, j].GetTerrain())
                {
                    case BoardTerrain.WALL:   //壁.
                        obj.GetComponent<SpriteRenderer>().sprite = sprWall;
                        break;
                    case BoardTerrain.GROUND: //床.
                        obj.GetComponent<SpriteRenderer>().sprite = sprGround;
                        break;

                    default:
                        break;
                }

                boardInst[i, j] = obj.GetComponent<Square>();
                boardInst[i, j].Init();

            }
        }
    }

    private void Update()
    {
        if (!isGame && imgResultPanel.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {

                print("タイトルに戻る");
                BackTitle();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {

                print("同じ難易度でリプレイ");
                Replay();
            }
        }
    }

    public Board[,] GetBoard()
    {
        return board;
    }

    public void SetBoardSquare(Vector2Int _pos, Board _val)
    {
        board[_pos.y, _pos.x] = _val;
        boardInst[_pos.y, _pos.x].SetObj(_val.GetDropObj().type);
    }

    public void DeleteDrops(Vector2Int _pos)
    {
        board[_pos.y, _pos.x].SetObject(DropObj.NONE, "", 0, 0, 0);
        boardInst[_pos.y, _pos.x].DeleteObj();
    }

    public Board GetBoardSquare(Vector2Int _pos)
    {
        return board[_pos.y, _pos.x];
    }


    public Vector3 GetCellWorldPosition(Vector2Int _pos)
    {
        if (_pos.x < 0 || _pos.x >= board.GetLength(1) || _pos.y < 0 || _pos.y >= board.GetLength(0))
        {
            Debug.LogWarning($"GetCellWorldPosition: ({_pos.x}, {_pos.y}) は範囲外です");
            return Vector3.zero;
        }

        // 画面中央を基準に盤面の左上座標を計算
        var (lb, rt) = Common.GetWorldWindowSize();
        var center = new Vector3((lb.x + rt.x) / 2, (lb.y + rt.y) / 2, 0);
        var boardTopLeft = center + new Vector3(-cellSize * (board.GetLength(0) - 1) / 2, cellSize * (board.GetLength(0) - 1) / 2, 0);

        // 指定のセルの座標を計算
        return boardTopLeft + new Vector3(_pos.x * cellSize, -_pos.y * cellSize, 0);
    }

    public bool IsPlayerInputWait()
    {
        return isPlayerWait;
    }


    public void PlayerTurnEnd()
    {
        isPlayerWait = false;
        em.EnemiesMove();
        lm.AddTailGauge();

        // 材料と食べ物を生成する
        var rand = Random.Range(0, 100);
        if (rand < 3)
        {
            // 食べ物を生成する
            var list = GetNoneSquares();
            rand = Random.Range(0, list.Count);
            var pos = list[rand];
            board[pos.y, pos.x].SetObject(DropObj.FOOD, "", 0, 0, 0);
            boardInst[pos.y, pos.x].SetObj(DropObj.FOOD);
        }
        else if (rand < 6)
        {
            // 材料を生成する

            var list = GetNoneSquares();
            rand = Random.Range(0, list.Count);
            var pos = list[rand];
            board[pos.y, pos.x].SetObject(DropObj.MATERIAL, "", 0, 0, 0);
            boardInst[pos.y, pos.x].SetObj(DropObj.MATERIAL);
        }




        currentElapsedTurns++;
        currentTimeZoneCount++;
        if (isDayTime)
        {
            if (currentTimeZoneCount >= LENGTH_DAYTIME)
            {
                isDayTime = !isDayTime;
                currentTimeZoneCount = 0;
            }
        }
        else
        {
            if (currentTimeZoneCount >= LENGTH_NIGHT)
            {
                isDayTime = !isDayTime;
                currentTimeZoneCount = 0;
            }
        }

        UIDisplay();



        if (currentElapsedTurns >= completeDays)
        {
            ShowResult(true);
            return;
        }


        isPlayerWait = true;
    }


    public List<Vector2Int> GetNoneSquares()
    {
        var ret = new List<Vector2Int>();
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                var pos = new Vector2Int(j, i);
                if (
                    pos != GetPlayerPos() &&
                    em.IsNoEnemies(pos) &&
                    board[i, j].GetDropObj().type == DropObj.NONE
                    )
                {
                    ret.Add(pos);
                }
            }
        }

        return ret;
    }


    public Vector2Int GetPlayerPos()
    {
        return lm.GetPos();
    }


    public void ShowResult(bool _isWin)
    {
        isGame = false;
        // リザルト表示
        imgResultPanel.gameObject.SetActive(true);
        if (_isWin)
        {
            textResult.text = "勝利";
            imgWin.gameObject.SetActive(true);
            //imgLose.gameObject.SetActive(false);
        }
        else
        {
            textResult.text = "敗北";
            imgWin.gameObject.SetActive(false);
            //imgLose.gameObject.SetActive(true);
        }
    }


    public void Replay()
    {
        print("リプレイ");
        Common.LoadScene("EnemyMove");
    }

    public void BackTitle()
    {
        Common.LoadScene("TitleScene");
        print("止める");
    }

    public void ConsumeMaterial()
    {
        int cnt = 2;

        for(int i = listInventory.Count - 1; i >= 0; i--)
        {
            if (listInventory[i].transform.GetChild(0).gameObject.activeSelf)
            {
                if(cnt > 0)
                {
                    cnt--;
                    listInventory[i].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }

    public void GetMaterial()
    {
        for(int i = 0; i < listInventory.Count - 1; i++)
        {
            if (!listInventory[i].transform.GetChild(0).gameObject.activeSelf)
            {
                listInventory[i].transform.GetChild(0).gameObject.SetActive(true);
                return;
            }
        }
    }


    void UIDisplay()
    {
        imgTailGauge.fillAmount = lm.GetNormalizedTailGaugeAMount();
        if (lm.IsTail())
        {
            imgTailIcon.color = Color.white;
        }
        else
        {
            imgTailIcon.color = Color.gray;
        }


        var tmp = imgRotate.transform.rotation;
        tmp.eulerAngles = new Vector3(0, 0, currentElapsedTurns * -(360f / (LENGTH_DAYTIME + LENGTH_NIGHT))) ;
        imgRotate.transform.rotation = tmp;



        textElapsedTurns.text = "経過ターン:" + currentElapsedTurns;
        textRemainingTurns.text = "終了まで後:" + (completeDays - currentElapsedTurns);

        if (isDayTime)
        {
            imgSun.sprite = sprDayTime;
        }
        else
        {
            imgSun.sprite = sprNight;
        }
    }
}
