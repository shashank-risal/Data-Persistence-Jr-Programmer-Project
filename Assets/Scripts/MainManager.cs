using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public PlayerName player;

    public int bestScore;
     
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text ScoreText2;
    public TextMeshProUGUI HighScoreText;

    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;
     
    public static string playerNameFull;

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
    
    // Start is called before the first frame update
    void Start()
    {             
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }        
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
                
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        
        CheckScore();
        BestScoreAndName();
    }

    void CheckScore()
    {
        if (bestScore < m_Points)
        {
            bestScore = m_Points;
            SaveHighScore();                
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        
    }

    public void BestScoreAndName()
    {
        ScoreText2.text = $"Best Score: {bestScore} Name:{playerNameFull}";
    }

    private void Awake()
    {
        
        if (playerNameFull != null)
        {
            Destroy(Instance);
            return;
        }
        else
        {            
            Instance = this;
            LoadHighScore();
            return;
        }
        
    }

    public void GetPlayerName()
    {
        playerNameFull = player.GetName();        
    }

    [System.Serializable]
    class SaveData
    {
        public int allTimeHighScore;
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.allTimeHighScore = bestScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/HighScore.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/HighScore.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            bestScore = data.allTimeHighScore;
            HighScoreText.text = $"Highest Score: {bestScore}";
           
        }
        else
        {
            print("no save data");
        }
    }

}

