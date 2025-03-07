using Gloval;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardData", menuName = "GameData/BoardData")]
public class BoardData : ScriptableObject
{
    public int rows;
    public int cols;
    public BoardTerrain[,] board;

    public void InitializeBoard()
    {
        board = new BoardTerrain[rows, cols];
    }

    public void SetTerrain(int x, int y, BoardTerrain terrain)
    {
        if (board == null) InitializeBoard();
        board[x, y] = terrain;
    }

    public BoardTerrain GetTerrain(int x, int y)
    {
        if (board == null) InitializeBoard();
        return board[x, y];
    }
}
