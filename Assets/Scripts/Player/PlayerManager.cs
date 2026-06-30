using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;

// Stores information for a single player account
[System.Serializable]
public class PlayerData
{
    // The player's username
    public string playerName;

    // The player's encrypted password
    public string passwordHash;

    // The player's highest wave reached
    public int highScore;

    // Total number of games played
    public int totalGamesPlayed;

    // Total amount of gold earned
    public int totalGoldEarned;
}

// Stores a list of all player accounts
[System.Serializable]
public class PlayersDatabase
{
    // Collection of all saved players
    public List<PlayerData> players = new List<PlayerData>();
}

// Manages player accounts, login, saving, and statistics
public class PlayerManager : MonoBehaviour
{
    // Singleton instance for global access
    public static PlayerManager Instance { get; private set; }

    // Stores the currently logged in player's name
    public string CurrentPlayerName { get; private set; }

    // Indicates whether a player is logged in
    public bool IsLoggedIn { get; private set; }

    // Stores all player accounts
    private PlayersDatabase database;

    // File path to the player store
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

    // Called before the first frame
    void Start()
    {
        // Create the database file path
        databasePath = Path.Combine(Application.dataPath, "LoginDetails", "players.json");

        // Makes sure path exists
        string directory = Path.GetDirectoryName(databasePath);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            Debug.Log($"Created directory: {directory}");
        }

        // Load the player database
        LoadDatabase();

        Debug.Log($"PlayerManager initialized. Database path: {databasePath}");
    }

    // Loads all player accounts from the JSON file
    private void LoadDatabase()
    {
        if (File.Exists(databasePath))
        {
            // Read the JSON file
            string json = File.ReadAllText(databasePath);

            // Convert JSON into a PlayersDatabase object
            database = JsonUtility.FromJson<PlayersDatabase>(json);

            Debug.Log($"Loaded {database.players.Count} player accounts from {databasePath}");
        }
        else
        {
            // Create a new empty database if none exists
            database = new PlayersDatabase();

            Debug.Log($"No database file found. Created new database at {databasePath}");
        }
    }

    // Saves all player data to the JSON file
    private void SaveDatabase()
    {
        // Convert the database into formatted JSON
        string json = JsonUtility.ToJson(database, true);

        // Write the JSON to the file
        File.WriteAllText(databasePath, json);

        Debug.Log($"Database saved to {databasePath}");
    }

    // Creates a new player account
    public bool CreateNewPlayer(string playerName, string password)
    {
        // Validate username
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogError("Player name cannot be empty");
            return false;
        }

        // Validate password
        if (string.IsNullOrEmpty(password))
        {
            Debug.LogError("Password cannot be empty");
            return false;
        }

        // Prevent duplicate usernames
        if (PlayerExists(playerName))
        {
            Debug.LogError($"Player '{playerName}' already exists");
            return false;
        }

        // Encrypt the password
        string passwordHash = HashPassword(password);

        // Create the new player record
        PlayerData newPlayer = new PlayerData
        {
            playerName = playerName,
            passwordHash = passwordHash,
            highScore = 0,
            totalGamesPlayed = 0,
            totalGoldEarned = 0
        };

        // Add the player to the database
        database.players.Add(newPlayer);

        // Save the updated database
        SaveDatabase();

        Debug.Log($"New player created: {playerName}");

        return true;
    }

    // Logs a player into their account
    public bool Login(string playerName, string password)
    {
        // Validate username
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogError("Player name cannot be empty");
            return false;
        }

        // Validate password
        if (string.IsNullOrEmpty(password))
        {
            Debug.LogError("Password cannot be empty");
            return false;
        }

        // Find the player's account
        PlayerData player = GetPlayerData(playerName);

        if (player == null)
        {
            Debug.LogError($"Player '{playerName}' does not exist");
            return false;
        }

        // Check the password
        if (!VerifyPassword(password, player.passwordHash))
        {
            Debug.LogError($"Incorrect password for player '{playerName}'");
            return false;
        }

        // Store the logged in player's details
        CurrentPlayerName = playerName;
        IsLoggedIn = true;

        Debug.Log($"Successfully logged in as: {playerName}");

        return true;
    }

    // Logs the current player out
    public void Logout()
    {
        CurrentPlayerName = "";
        IsLoggedIn = false;

        Debug.Log("Logged out");
    }

    // Finds a player's data using their username
    private PlayerData GetPlayerData(string playerName)
    {
        return database.players.Find(p => p.playerName == playerName);
    }

    // Checks whether a player already exists
    public bool PlayerExists(string playerName)
    {
        return GetPlayerData(playerName) != null;
    }

    // Encrypts a password using SHA256
    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            return System.Convert.ToBase64String(hashedBytes);
        }
    }

    // Verifies that the entered password matches the stored password
    private bool VerifyPassword(string password, string storedHash)
    {
        string inputHash = HashPassword(password);

        return inputHash == storedHash;
    }

    // Returns a player's highest wave reached
    public int GetHighScore(string playerName)
    {
        PlayerData player = GetPlayerData(playerName);

        return player != null ? player.highScore : 0;
    }

    // Returns the logged in player's high score
    public int GetCurrentPlayerHighScore()
    {
        if (!IsLoggedIn)
            return 0;

        return GetHighScore(CurrentPlayerName);
    }

    // Updates the player's high score if they beat it
    public void UpdateHighScore(int wave)
    {
        if (!IsLoggedIn)
            return;

        PlayerData player = GetPlayerData(CurrentPlayerName);

        if (player != null && wave > player.highScore)
        {
            player.highScore = wave;

            SaveDatabase();

            Debug.Log($"New high score for {CurrentPlayerName}: Wave {wave}");
        }
    }

    // Updates player statistics after each game
    public void UpdateGameStats(int waveReached, int goldEarned)
    {
        if (!IsLoggedIn)
            return;

        PlayerData player = GetPlayerData(CurrentPlayerName);

        if (player != null)
        {
            // Update the player's high score
            UpdateHighScore(waveReached);

            // Increase total games played
            player.totalGamesPlayed++;

            // Add the gold earned this game
            player.totalGoldEarned += goldEarned;

            // Save the updated statistics
            SaveDatabase();
        }
    }

    // Returns the total number of games played
    public int GetTotalGamesPlayed()
    {
        if (!IsLoggedIn)
            return 0;

        PlayerData player = GetPlayerData(CurrentPlayerName);

        return player != null ? player.totalGamesPlayed : 0;
    }

    // Returns the total amount of gold earned
    public int GetTotalGoldEarned()
    {
        if (!IsLoggedIn)
            return 0;

        PlayerData player = GetPlayerData(CurrentPlayerName);

        return player != null ? player.totalGoldEarned : 0;
    }
}