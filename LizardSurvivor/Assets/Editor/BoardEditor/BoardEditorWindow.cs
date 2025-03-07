using Gloval;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class BoardEditorWindow : EditorWindow
{
    private string boardName = "NewBoard";
    private BoardData currentBoardData;
    private SerializedObject serializedBoardData;
    private Vector2 scrollPosition;
    private BoardTerrain[,] tempBoard;
    private List<BoardData> savedBoards;

    [MenuItem("Tools/Board Editor")]
    public static void ShowWindow()
    {
        GetWindow<BoardEditorWindow>("Board Editor");
    }

    private void OnEnable()
    {
        LoadSavedBoards();
    }

    private void LoadSavedBoards()
    {
        savedBoards = Resources.LoadAll<BoardData>("").ToList();
    }

    private void LoadBoard(BoardData board)
    {
        currentBoardData = board;
        serializedBoardData = new SerializedObject(currentBoardData);
        ResizeTempBoard(currentBoardData.rows, currentBoardData.cols);
    }

    private void ResizeTempBoard(int newRows, int newCols)
    {
        tempBoard = new BoardTerrain[newCols, newRows];
        for (int y = 0; y < newRows; y++)
            for (int x = 0; x < newCols; x++)
                tempBoard[x, y] = currentBoardData.GetTerrain(x, y);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Board Data Manager", EditorStyles.boldLabel);

        // 保存データ一覧のプレビューと削除ボタン
        EditorGUILayout.LabelField("Saved Boards", EditorStyles.boldLabel);
        if (savedBoards.Count > 0)
        {
            foreach (var board in savedBoards.ToList())
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(board.name))
                {
                    LoadBoard(board);
                }
                if (GUILayout.Button("削除", GUILayout.Width(60)))
                {
                    DeleteBoardData(board);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            EditorGUILayout.LabelField("No saved board data found.");
        }

        EditorGUILayout.Space();
        boardName = EditorGUILayout.TextField("Board Name", boardName);

        if (GUILayout.Button("Create New Board"))
        {
            CreateNewBoard();
        }

        if (currentBoardData == null)
            return;

        serializedBoardData.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Board Settings", EditorStyles.boldLabel);
        currentBoardData.rows = EditorGUILayout.IntField("Rows", currentBoardData.rows);
        currentBoardData.cols = EditorGUILayout.IntField("Columns", currentBoardData.cols);

        if (GUILayout.Button("Apply Size Change"))
        {
            currentBoardData.InitializeBoard();
            ResizeTempBoard(currentBoardData.rows, currentBoardData.cols);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Board Layout", EditorStyles.boldLabel);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(400));

        for (int y = 0; y < currentBoardData.rows; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < currentBoardData.cols; x++)
            {
                if (GUILayout.Button(tempBoard[x, y].ToString(), GUILayout.Width(80), GUILayout.Height(30)))
                {
                    tempBoard[x, y] = (tempBoard[x, y] == BoardTerrain.GROUND) ? BoardTerrain.WALL : BoardTerrain.GROUND;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Save Board Data"))
        {
            SaveBoardData();
        }

        serializedBoardData.ApplyModifiedProperties();
    }

    private void CreateNewBoard()
    {
        string path = $"Assets/Resources/{boardName}.asset";

        if (File.Exists(path))
        {
            Debug.LogWarning("A board with this name already exists!");
            return;
        }

        currentBoardData = CreateInstance<BoardData>();
        currentBoardData.name = boardName;
        AssetDatabase.CreateAsset(currentBoardData, path);
        AssetDatabase.SaveAssets();
        LoadSavedBoards();
        LoadBoard(currentBoardData);
    }

    private void SaveBoardData()
    {
        for (int y = 0; y < currentBoardData.rows; y++)
            for (int x = 0; x < currentBoardData.cols; x++)
                currentBoardData.SetTerrain(x, y, tempBoard[x, y]);

        EditorUtility.SetDirty(currentBoardData);
        AssetDatabase.SaveAssets();
    }

    private void DeleteBoardData(BoardData board)
    {
        string path = AssetDatabase.GetAssetPath(board);
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            LoadSavedBoards();
            if (currentBoardData == board)
            {
                currentBoardData = null;
            }
        }
    }
}
