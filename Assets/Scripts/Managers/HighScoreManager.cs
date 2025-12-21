using UnityEngine;
using System;

/// <summary>
/// Singleton manager untuk menyimpan dan mengelola high scores
/// Menggunakan PlayerPrefs untuk persistent storage
/// </summary>
public class HighScoreManager : MonoBehaviour
{
    private static HighScoreManager instance;
    public static HighScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("HighScoreManager");
                instance = go.AddComponent<HighScoreManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    // PlayerPrefs Keys
    private const string CHAPTER1_LEVEL1_KEY = "Chapter1_Level1_HighScore";
    private const string CHAPTER1_LEVEL2_KEY = "Chapter1_Level2_HighScore";
    private const string CHAPTER1_TOTAL_KEY = "Chapter1_Total_HighScore";
    
    private const string CHAPTER1_LEVEL1_DATE_KEY = "Chapter1_Level1_Date";
    private const string CHAPTER1_LEVEL2_DATE_KEY = "Chapter1_Level2_Date";
    private const string CHAPTER1_TOTAL_DATE_KEY = "Chapter1_Total_Date";

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #region Save Score Methods

    /// <summary>
    /// Save score untuk Level 1 (Soal 1-10)
    /// </summary>
    public void SaveLevel1Score(int score)
    {
        int currentHighScore = GetLevel1HighScore();
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(CHAPTER1_LEVEL1_KEY, score);
            PlayerPrefs.SetString(CHAPTER1_LEVEL1_DATE_KEY, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            PlayerPrefs.Save();
            
            Debug.Log($"[HighScore] New Level 1 High Score: {score}");
        }
    }

    /// <summary>
    /// Save score untuk Level 2 (Soal 11-20)
    /// </summary>
    public void SaveLevel2Score(int score)
    {
        int currentHighScore = GetLevel2HighScore();
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(CHAPTER1_LEVEL2_KEY, score);
            PlayerPrefs.SetString(CHAPTER1_LEVEL2_DATE_KEY, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            PlayerPrefs.Save();
            
            Debug.Log($"[HighScore] New Level 2 High Score: {score}");
        }
    }

    /// <summary>
    /// Save total score Chapter 1 (gabungan Level 1 + Level 2)
    /// </summary>
    public void SaveTotalScore(int score)
    {
        int currentHighScore = GetTotalHighScore();
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(CHAPTER1_TOTAL_KEY, score);
            PlayerPrefs.SetString(CHAPTER1_TOTAL_DATE_KEY, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            PlayerPrefs.Save();
            
            Debug.Log($"[HighScore] New Total High Score: {score}");
        }
    }

    #endregion

    #region Get Score Methods

    /// <summary>
    /// Dapatkan high score Level 1
    /// </summary>
    public int GetLevel1HighScore()
    {
        return PlayerPrefs.GetInt(CHAPTER1_LEVEL1_KEY, 0);
    }

    /// <summary>
    /// Dapatkan high score Level 2
    /// </summary>
    public int GetLevel2HighScore()
    {
        return PlayerPrefs.GetInt(CHAPTER1_LEVEL2_KEY, 0);
    }

    /// <summary>
    /// Dapatkan total high score Chapter 1
    /// </summary>
    public int GetTotalHighScore()
    {
        return PlayerPrefs.GetInt(CHAPTER1_TOTAL_KEY, 0);
    }

    /// <summary>
    /// Dapatkan tanggal high score Level 1
    /// </summary>
    public string GetLevel1Date()
    {
        return PlayerPrefs.GetString(CHAPTER1_LEVEL1_DATE_KEY, "-");
    }

    /// <summary>
    /// Dapatkan tanggal high score Level 2
    /// </summary>
    public string GetLevel2Date()
    {
        return PlayerPrefs.GetString(CHAPTER1_LEVEL2_DATE_KEY, "-");
    }

    /// <summary>
    /// Dapatkan tanggal total high score
    /// </summary>
    public string GetTotalDate()
    {
        return PlayerPrefs.GetString(CHAPTER1_TOTAL_DATE_KEY, "-");
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Reset semua high scores (untuk testing atau reset game)
    /// </summary>
    public void ResetAllScores()
    {
        PlayerPrefs.DeleteKey(CHAPTER1_LEVEL1_KEY);
        PlayerPrefs.DeleteKey(CHAPTER1_LEVEL2_KEY);
        PlayerPrefs.DeleteKey(CHAPTER1_TOTAL_KEY);
        PlayerPrefs.DeleteKey(CHAPTER1_LEVEL1_DATE_KEY);
        PlayerPrefs.DeleteKey(CHAPTER1_LEVEL2_DATE_KEY);
        PlayerPrefs.DeleteKey(CHAPTER1_TOTAL_DATE_KEY);
        PlayerPrefs.Save();
        
        Debug.Log("[HighScore] All scores reset!");
    }

    /// <summary>
    /// Check apakah pemain pernah main (ada score tersimpan)
    /// </summary>
    public bool HasPlayedBefore()
    {
        return PlayerPrefs.HasKey(CHAPTER1_LEVEL1_KEY) || 
               PlayerPrefs.HasKey(CHAPTER1_LEVEL2_KEY) ||
               PlayerPrefs.HasKey(CHAPTER1_TOTAL_KEY);
    }

    /// <summary>
    /// Get score summary untuk display
    /// </summary>
    public ScoreSummary GetScoreSummary()
    {
        return new ScoreSummary
        {
            level1HighScore = GetLevel1HighScore(),
            level2HighScore = GetLevel2HighScore(),
            totalHighScore = GetTotalHighScore(),
            level1Date = GetLevel1Date(),
            level2Date = GetLevel2Date(),
            totalDate = GetTotalDate()
        };
    }

    #endregion
}

/// <summary>
/// Data class untuk score summary
/// </summary>
[System.Serializable]
public class ScoreSummary
{
    public int level1HighScore;
    public int level2HighScore;
    public int totalHighScore;
    public string level1Date;
    public string level2Date;
    public string totalDate;

    public override string ToString()
    {
        return $"Level 1: {level1HighScore} | Level 2: {level2HighScore} | Total: {totalHighScore}";
    }
}
