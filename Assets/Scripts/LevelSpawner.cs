using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSpawner : MonoBehaviour
{
    public GameObject[] obstacleModel;
    [HideInInspector]
    public GameObject[] obstaclePrefab = new GameObject[4];

    GameObject tempObstacle, temp2Obstacle;

    public GameObject winPrefab;
    int level = 1, addNumber = 7;

    float obstacleNumber;

    public Material plateMat, baseMat;
    public MeshRenderer playerMeshRenderer;

    private void Start()
    {
        level = PlayerPrefs.GetInt("Level", 1);
        RandomObstacleGenerator();
        GenerateObstacles();
        SetupPlayerAppearance();
    }

    private void RandomObstacleGenerator()
    {
        int random = Random.Range(0, 5);
        switch (random)
        {
            case 0:
                for (int i = 0; i < 4; i++)
                {
                    obstaclePrefab[i] = obstacleModel[i];
                }
                break;
            case 1:
                for (int i = 0; i < 4; i++)
                {
                    obstaclePrefab[i] = obstacleModel[i + 4];
                }
                break;
            case 2:
                for (int i = 0; i < 4; i++)
                {
                    obstaclePrefab[i] = obstacleModel[i + 8];
                }
                break;
            case 3:
                for (int i = 0; i < 4; i++)
                {
                    obstaclePrefab[i] = obstacleModel[i + 12];
                }
                break;
            case 4:
                for (int i = 0; i < 4; i++)
                {
                    obstaclePrefab[i] = obstacleModel[i + 16];
                }
                break;
            default:
                break;
        }
    }

    private void GenerateObstacles()
    {
        float randomNumber = Random.value;
        for (obstacleNumber = 0; obstacleNumber > -level - addNumber; obstacleNumber -= 0.5f)
        {
            int obstacleIndex = GetObstacleIndex();
            tempObstacle = Instantiate(obstaclePrefab[obstacleIndex]);
            tempObstacle.transform.position = new Vector3(0, obstacleNumber - 0.01f, 0);
            tempObstacle.transform.eulerAngles = new Vector3(0, obstacleNumber * 8, 0);

            if (IsObstacleRotated(obstacleNumber))
            {
                tempObstacle.transform.eulerAngles += Vector3.up * 180;
            }

            tempObstacle.transform.parent = FindObjectOfType<RotateManager>().transform;
        }

        CreateWinObstacle();
    }

    private int GetObstacleIndex()
    {
        if (level <= 20)
            return Random.Range(0, 2);
        else if (level > 20 && level < 50)
            return Random.Range(1, 3);
        else if (level >= 50 && level <= 100)
            return Random.Range(2, 4);
        else
            return Random.Range(3, 4);
    }

    private bool IsObstacleRotated(float obstacleNumber)
    {
        if (Mathf.Abs(obstacleNumber) >= level * 0.3f && Mathf.Abs(obstacleNumber) <= level * 0.6f)
        {
            return true;
        }
        else if (Mathf.Abs(obstacleNumber) > level * 0.8f)
        {
            return (Random.value > 0.75f);
        }
        return false;
    }

    private void CreateWinObstacle()
    {
        temp2Obstacle = Instantiate(winPrefab);
        temp2Obstacle.transform.position = new Vector3(0, obstacleNumber - 0.01f, 0);
    }

    private void SetupPlayerAppearance()
    {
        plateMat.color = Random.ColorHSV(0, 1, 0.5f, 1, 1, 1);
        baseMat.color = plateMat.color + Color.gray;
        playerMeshRenderer.material.color = baseMat.color;
    }
}