using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public int width = 10;
    public int height = 10;

    private Dictionary<Vector2Int, bool> grid = new Dictionary<Vector2Int, bool>();

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyCount = 3;

    private GameObject playerInstance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        GenerateGrid();
        SpawnPlayer();
        Invoke("SpawnEnemies", 0.2f); // 🔥 Player가 먼저 생성된 후 적 배치
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[new Vector2Int(x, y)] = true;
            }
        }

        // 테스트용 장애물 추가
        SetObstacle(new Vector2Int(3, 3));
        SetObstacle(new Vector2Int(4, 4));
        SetObstacle(new Vector2Int(5, 5));
    }

    public void SetObstacle(Vector2Int position)
    {
        if (grid.ContainsKey(position))
        {
            grid[position] = false;
        }
    }

    public bool IsWalkable(Vector2Int position)
    {
        return grid.ContainsKey(position) && grid[position];
    }

    Vector2Int GetRandomWalkablePosition()
    {
        List<Vector2Int> walkableTiles = new List<Vector2Int>();

        foreach (var tile in grid)
        {
            if (tile.Value) walkableTiles.Add(tile.Key);
        }

        return walkableTiles.Count == 0 ? Vector2Int.zero : walkableTiles[Random.Range(0, walkableTiles.Count)];
    }

    void SpawnPlayer()
    {
        Vector2Int playerPos = GetRandomWalkablePosition();
        playerInstance = Instantiate(playerPrefab, (Vector2)playerPos, Quaternion.identity);
        grid[playerPos] = false;
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2Int enemyPos;
            do { enemyPos = GetRandomWalkablePosition(); }
            while (enemyPos == Vector2Int.RoundToInt(playerInstance.transform.position));

            Instantiate(enemyPrefab, (Vector2)enemyPos, Quaternion.identity);
            grid[enemyPos] = false;
        }
    }
}
