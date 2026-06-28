using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public string passwordHash;
    public int highScore;
    public int totalGamesPlayed;
    public int totalGoldEarned;
}

[System.Serializable]
public class PlayersDatabase
{
    public List<PlayerData> players = new List<PlayerData>();
}

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    
    public string CurrentPlayerName { get; private set; }
    public bool IsLoggedIn { get; private set; }
    
    private PlayersDatabase database;
    private string databasePath;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        
        databasePath = Path.Combine(Application.dataPath, "LoginDetails", "players.json");
        
        
        string directory = Path.GetDirectoryName(databasePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            Debug.Log($"Created directory: {directory}");
        }
        
        
        LoadDatabase();
        
        Debug.Log($"PlayerManager initialized. Database path: {databasePath}");
    }
    
    
    
    
    private void LoadDatabase()
    {
        if (File.Exists(databasePath))
        {
            string json = File.ReadAllText(databasePath);
            database = JsonUtility.FromJson<PlayersDatabase>(json);
            Debug.Log($"Loaded {database.players.Count} player accounts from {databasePath}");
        }
        else
        {
            database = new PlayersDatabase();
            Debug.Log($"No database file found. Created new database at {databasePath}");
        }
    }
    
    
    
    
    private void SaveDatabase()
    {
        string json = JsonUtility.ToJson(database, true); 
        File.WriteAllText(databasePath, json);
        Debug.Log($"Database saved to {databasePath}");
    }
    
    
    
    
    public bool CreateNewPlayer(string playerName, string password)
    {
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogError("Player name cannot be empty");
            return false;
        }
        
        if (string.IsNullOrEmpty(password))
        {
            Debug.LogError("Password cannot be empty");
            return false;
        }
        
        
        if (PlayerExists(playerName))
        {
            Debug.LogError($"Player '{playerName}' already exists");
            return false;
        }
        
        
        string passwordHash = HashPassword(password);
        
        
        PlayerData newPlayer = new PlayerData
        {
            playerName = playerName,
            passwordHash = passwordHash,
            highScore = 0,
            totalGamesPlayed = 0,
            totalGoldEarned = 0
        };
        
        database.players.Add(newPlayer);
        SaveDatabase();
        
        Debug.Log($"New player created: {playerName}");
        return true;
    }
    
    
    
    
    public bool Login(string playerName, string password)
    {
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogError("Player name cannot be empty");
            return false;
        }
        
        if (string.IsNullOrEmpty(password))
        {
            Debug.LogError("Password cannot be empty");
            return false;
        }
        
        PlayerData player = GetPlayerData(playerName);
        if (player == null)
        {
            Debug.LogError($"Player '{playerName}' does not exist");
            return false;
        }
        
        
        if (!VerifyPassword(password, player.passwordHash))
        {
            Debug.LogError($"Incorrect password for player '{playerName}'");
            return false;
        }
        
        CurrentPlayerName = playerName;
        IsLoggedIn = true;
        
        Debug.Log($"Successfully logged in as: {playerName}");
        return true;
    }
    
    public void Logout()
    {
        CurrentPlayerName = "";
        IsLoggedIn = false;
        Debug.Log("Logged out");
    }
    
    
    
    
    private PlayerData GetPlayerData(string playerName)
    {
        return database.players.Find(p => p.playerName == playerName);
    }
    
    
    
    
    public bool PlayerExists(string playerName)
    {
        return GetPlayerData(playerName) != null;
    }
    
    
    
    
    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return System.Convert.ToBase64String(hashedBytes);
        }
    }
    
    
    
    
    private bool VerifyPassword(string password, string storedHash)
    {
        string inputHash = HashPassword(password);
        return inputHash == storedHash;
    }
    
    public int GetHighScore(string playerName)
    {
        PlayerData player = GetPlayerData(playerName);
        return player != null ? player.highScore : 0;
    }
    
    public int GetCurrentPlayerHighScore()
    {
        if (!IsLoggedIn) return 0;
        return GetHighScore(CurrentPlayerName);
    }
    
    public void UpdateHighScore(int wave)
    {
        if (!IsLoggedIn) return;
        
        PlayerData player = GetPlayerData(CurrentPlayerName);
        if (player != null && wave > player.highScore)
        {
            player.highScore = wave;
            SaveDatabase();
            Debug.Log($"New high score for {CurrentPlayerName}: Wave {wave}");
        }
    }
    
    public void UpdateGameStats(int waveReached, int goldEarned)
    {
        if (!IsLoggedIn) return;
        
        PlayerData player = GetPlayerData(CurrentPlayerName);
        if (player != null)
        {
            
            UpdateHighScore(waveReached);
            
            
            player.totalGamesPlayed++;
            
            
            player.totalGoldEarned += goldEarned;
            
            SaveDatabase();
        }
    }
    
    public int GetTotalGamesPlayed()
    {
        if (!IsLoggedIn) return 0;
        PlayerData player = GetPlayerData(CurrentPlayerName);
        return player != null ? player.totalGamesPlayed : 0;
    }
    
    public int GetTotalGoldEarned()
    {
        if (!IsLoggedIn) return 0;
        PlayerData player = GetPlayerData(CurrentPlayerName);
        return player != null ? player.totalGoldEarned : 0;
    }
    
    
    
    
    public static void OpenLoginDetailsFolder()
    {
        string folderPath = Path.Combine(Application.dataPath, "LoginDetails");
        
        #if UNITY_STANDALONE_WIN
            System.Diagnostics.Process.Start("explorer.exe", folderPath);
        #elif UNITY_STANDALONE_OSX
            System.Diagnostics.Process.Start("open", folderPath);
        #elif UNITY_STANDALONE_LINUX
            System.Diagnostics.Process.Start("xdg-open", folderPath);
        #endif
    }
}