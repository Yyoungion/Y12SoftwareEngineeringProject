using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    
    
    private TextField loginPlayerNameField;
    private TextField loginPasswordField;
    private Button loginButton;
    private Button createNewButton;
    
    
    private TextField createPlayerNameField;
    private TextField createPasswordField;
    private TextField confirmPasswordField;
    private Button createButton;
    private Button backButton;
    
    
    private Label errorMessage;
    private VisualElement loginPanel;
    private VisualElement createPanel;
    
    void Start()
    {
        Debug.Log("LoginController Start() called");
        
        uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component not found on this GameObject!");
            return;
        }
        
        root = uiDocument.rootVisualElement;
        if (root == null)
        {
            Debug.LogError("Root visual element is null!");
            return;
        }
        
        Debug.Log("Root element found. Querying child elements...");
        
        
        loginPlayerNameField = root.Q<TextField>("LoginPlayerNameField");
        loginPasswordField = root.Q<TextField>("LoginPasswordField");
        loginButton = root.Q<Button>("LoginButton");
        createNewButton = root.Q<Button>("CreateNewButton");
        
        
        createPlayerNameField = root.Q<TextField>("CreatePlayerNameField");
        createPasswordField = root.Q<TextField>("CreatePasswordField");
        confirmPasswordField = root.Q<TextField>("ConfirmPasswordField");
        createButton = root.Q<Button>("CreateButton");
        backButton = root.Q<Button>("BackButton");
        
        
        errorMessage = root.Q<Label>("ErrorMessage");
        loginPanel = root.Q<VisualElement>("LoginPanel");
        createPanel = root.Q<VisualElement>("CreatePanel");
        
        
        Debug.Log($"LoginPlayerNameField: {(loginPlayerNameField != null ? "FOUND" : "NOT FOUND")}");
        Debug.Log($"LoginPasswordField: {(loginPasswordField != null ? "FOUND" : "NOT FOUND")}");
        Debug.Log($"LoginButton: {(loginButton != null ? "FOUND" : "NOT FOUND")}");
        Debug.Log($"CreateNewButton: {(createNewButton != null ? "FOUND" : "NOT FOUND")}");
        Debug.Log($"CreatePlayerNameField: {(createPlayerNameField != null ? "FOUND" : "NOT FOUND")}");
        Debug.Log($"CreatePasswordField: {(createPasswordField != null ? "FOUND" : "NOT FOUND")}");
        Debug.Log($"ConfirmPasswordField: {(confirmPasswordField != null ? "FOUND" : "NOT FOUND")}");
        Debug.Log($"CreateButton: {(createButton != null ? "FOUND" : "NOT FOUND")}");
        Debug.Log($"BackButton: {(backButton != null ? "FOUND" : "NOT FOUND")}");
        Debug.Log($"ErrorMessage: {(errorMessage != null ? "FOUND" : "NOT FOUND")}");
        Debug.Log($"LoginPanel: {(loginPanel != null ? "FOUND" : "NOT FOUND")}");
        Debug.Log($"CreatePanel: {(createPanel != null ? "FOUND" : "NOT FOUND")}");
        
        
        if (loginButton != null)
        {
            loginButton.RegisterCallback<ClickEvent>(evt => OnLoginClick());
            Debug.Log("LoginButton callback registered");
        }
        
        if (createNewButton != null)
        {
            createNewButton.RegisterCallback<ClickEvent>(evt => OnCreateNewClick());
            Debug.Log("CreateNewButton callback registered");
        }
        
        if (createButton != null)
        {
            createButton.RegisterCallback<ClickEvent>(evt => OnCreateClick());
            Debug.Log("CreateButton callback registered");
        }
        
        if (backButton != null)
        {
            backButton.RegisterCallback<ClickEvent>(evt => OnBackClick());
            Debug.Log("BackButton callback registered");
        }
        
        
        if (loginPasswordField != null)
        {
            loginPasswordField.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.keyCode == KeyCode.Return)
                    OnLoginClick();
            });
        }
        
        if (confirmPasswordField != null)
        {
            confirmPasswordField.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.keyCode == KeyCode.Return)
                    OnCreateClick();
            });
        }
        
        Debug.Log("LoginController initialization complete!");
    }
    
    void OnLoginClick()
    {
        Debug.Log("OnLoginClick called");
        
        string playerName = loginPlayerNameField != null ? loginPlayerNameField.value.Trim() : "";
        string password = loginPasswordField != null ? loginPasswordField.value : "";
        
        Debug.Log($"Login attempt with name: '{playerName}'");
        
        if (string.IsNullOrEmpty(playerName))
        {
            ShowError("Player name cannot be empty");
            return;
        }
        
        if (string.IsNullOrEmpty(password))
        {
            ShowError("Password cannot be empty");
            return;
        }
        
        
        if (PlayerManager.Instance.Login(playerName, password))
        {
            Debug.Log($"Login successful for {playerName}");
            LoadGameScene();
        }
        else
        {
            ShowError("Incorrect player name or password");
            loginPasswordField.value = ""; 
        }
    }
    
    void OnCreateNewClick()
    {
        Debug.Log("OnCreateNewClick called");
        
        if (loginPanel != null)
            loginPanel.style.display = DisplayStyle.None;
        if (createPanel != null)
            createPanel.style.display = DisplayStyle.Flex;
        if (createPlayerNameField != null)
            createPlayerNameField.Focus();
        ClearError();
    }
    
    void OnCreateClick()
    {
        Debug.Log("OnCreateClick called");
        
        string playerName = createPlayerNameField != null ? createPlayerNameField.value.Trim() : "";
        string password = createPasswordField != null ? createPasswordField.value : "";
        string confirmPassword = confirmPasswordField != null ? confirmPasswordField.value : "";
        
        Debug.Log($"Create attempt with name: '{playerName}'");
        
        
        if (string.IsNullOrEmpty(playerName))
        {
            ShowError("Player name cannot be empty");
            return;
        }
        
        if (playerName.Length < 3)
        {
            ShowError("Player name must be at least 3 characters");
            return;
        }
        
        if (playerName.Length > 20)
        {
            ShowError("Player name must be 20 characters or less");
            return;
        }
        
        if (string.IsNullOrEmpty(password))
        {
            ShowError("Password cannot be empty");
            return;
        }
        
        if (password.Length < 6)
        {
            ShowError("Password must be at least 6 characters");
            return;
        }
        
        if (password != confirmPassword)
        {
            ShowError("Passwords do not match");
            confirmPasswordField.value = "";
            createPasswordField.value = "";
            return;
        }
        
        if (PlayerManager.Instance.PlayerExists(playerName))
        {
            ShowError($"Player '{playerName}' already exists");
            return;
        }
        
        
        if (PlayerManager.Instance.CreateNewPlayer(playerName, password))
        {
            Debug.Log($"Player created: {playerName}");
            
            
            if (PlayerManager.Instance.Login(playerName, password))
            {
                LoadGameScene();
            }
        }
        else
        {
            ShowError("Failed to create account. Please try again.");
        }
    }
    
    void OnBackClick()
    {
        Debug.Log("OnBackClick called");
        
        if (loginPanel != null)
            loginPanel.style.display = DisplayStyle.Flex;
        if (createPanel != null)
            createPanel.style.display = DisplayStyle.None;
        if (loginPlayerNameField != null)
            loginPlayerNameField.Focus();
        
        
        if (loginPasswordField != null)
            loginPasswordField.value = "";
        if (createPasswordField != null)
            createPasswordField.value = "";
        if (confirmPasswordField != null)
            confirmPasswordField.value = "";
        
        ClearError();
    }
    
    void ShowError(string message)
    {
        Debug.LogWarning($"Error shown: {message}");
        if (errorMessage != null)
        {
            errorMessage.text = message;
            errorMessage.style.display = DisplayStyle.Flex;
        }
    }
    
    void ClearError()
    {
        if (errorMessage != null)
        {
            errorMessage.text = "";
            errorMessage.style.display = DisplayStyle.None;
        }
    }
    
    void LoadGameScene()
    {
        Debug.Log("Loading GameScene...");
        SceneManager.LoadScene("MainMenuScene");
    }
}
