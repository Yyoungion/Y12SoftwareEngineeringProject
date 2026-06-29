using UnityEngine; // Imports the Unity engine.
using UnityEngine.UIElements; // Imports the UI Toolkit.
using UnityEngine.SceneManagement; // Imports scene management.

public class LoginController : MonoBehaviour // Defines the LoginController class.
{
    // Reference to the UI Document attached to this GameObject.
    private UIDocument uiDocument;

    // Reference to the root visual element containing all UI elements.
    private VisualElement root;

    // Login screen UI elements.
    private TextField loginPlayerNameField; // Input field for the player's username.
    private TextField loginPasswordField; // Input field for the player's password.
    private Button loginButton; // Button used to log into an existing account.
    private Button createNewButton; // Button that opens the account creation screen.

    // Create account screen UI elements.
    private TextField createPlayerNameField; // Input field for the new username.
    private TextField createPasswordField; // Input field for the new password.
    private TextField confirmPasswordField; // Input field to confirm the password.
    private Button createButton; // Button used to create a new account.
    private Button backButton; // Button used to return to the login screen.

    // General UI elements.
    private Label errorMessage; // Displays validation and login errors.
    private VisualElement loginPanel; // Login screen panel.
    private VisualElement createPanel; // Create account screen panel.

    void Start()
    {
        // Print a message confirming the script has started.
        Debug.Log("LoginController Start() called");

        // Get the UIDocument component.
        uiDocument = GetComponent<UIDocument>();

        // Stop execution if no UIDocument exists.
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component not found on this GameObject!");
            return;
        }

        // Get the root visual element of the UI.
        root = uiDocument.rootVisualElement;

        // Stop execution if the root element cannot be found.
        if (root == null)
        {
            Debug.LogError("Root visual element is null!");
            return;
        }

        // Locate the login screen UI elements.
        loginPlayerNameField = root.Q<TextField>("LoginPlayerNameField");
        loginPasswordField = root.Q<TextField>("LoginPasswordField");
        loginButton = root.Q<Button>("LoginButton");
        createNewButton = root.Q<Button>("CreateNewButton");

        // Locate the create account screen UI elements.
        createPlayerNameField = root.Q<TextField>("CreatePlayerNameField");
        createPasswordField = root.Q<TextField>("CreatePasswordField");
        confirmPasswordField = root.Q<TextField>("ConfirmPasswordField");
        createButton = root.Q<Button>("CreateButton");
        backButton = root.Q<Button>("BackButton");

        // Locate other UI elements.
        errorMessage = root.Q<Label>("ErrorMessage");
        loginPanel = root.Q<VisualElement>("LoginPanel");
        createPanel = root.Q<VisualElement>("CreatePanel");

        // Print whether each UI element was successfully found.
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

        // Register the login button's click event.
        if (loginButton != null)
        {
            loginButton.RegisterCallback<ClickEvent>(evt => OnLoginClick());
            Debug.Log("LoginButton callback registered");
        }

        // Register the create new account button's click event.
        if (createNewButton != null)
        {
            createNewButton.RegisterCallback<ClickEvent>(evt => OnCreateNewClick());
            Debug.Log("CreateNewButton callback registered");
        }

        // Register the create account button's click event.
        if (createButton != null)
        {
            createButton.RegisterCallback<ClickEvent>(evt => OnCreateClick());
            Debug.Log("CreateButton callback registered");
        }

        // Register the back button's click event.
        if (backButton != null)
        {
            backButton.RegisterCallback<ClickEvent>(evt => OnBackClick());
            Debug.Log("BackButton callback registered");
        }

        // Allow the Enter key to submit the login form.
        if (loginPasswordField != null)
        {
            loginPasswordField.RegisterCallback<KeyDownEvent>(evt =>
            {
                // Check if the Enter key was pressed.
                if (evt.keyCode == KeyCode.Return)

                    // Attempt to log in.
                    OnLoginClick();
            });
        }

        // Allow the Enter key to submit the create account form.
        if (confirmPasswordField != null)
        {
            confirmPasswordField.RegisterCallback<KeyDownEvent>(evt =>
            {
                // Check if the Enter key was pressed.
                if (evt.keyCode == KeyCode.Return)

                    // Attempt to create the account.
                    OnCreateClick();
            });
        }

        // Print a message indicating setup is complete.
        Debug.Log("LoginController initialization complete!");
    }

        // Called when the login button is pressed.
    void OnLoginClick()
    {
        // Debug message for testing.
        Debug.Log("OnLoginClick called");

        // Get and clean the player name input.
        string playerName = loginPlayerNameField != null ? loginPlayerNameField.value.Trim() : "";

        // Get the password input.
        string password = loginPasswordField != null ? loginPasswordField.value : "";

        // Debug log showing attempted login name.
        Debug.Log($"Login attempt with name: '{playerName}'");

        // Check if the player name is empty.
        if (string.IsNullOrEmpty(playerName))
        {
            ShowError("Player name cannot be empty");
            return;
        }

        // Check if the password is empty.
        if (string.IsNullOrEmpty(password))
        {
            ShowError("Password cannot be empty");
            return;
        }

        // Attempt login through PlayerManager.
        if (PlayerManager.Instance.Login(playerName, password))
        {
            // Login successful.
            Debug.Log($"Login successful for {playerName}");

            // Load the main game scene.
            LoadGameScene();
        }
        else
        {
            // Login failed.
            ShowError("Incorrect player name or password");

            // Clear password field for security.
            loginPasswordField.value = "";
        }
    }

    // Switches from login panel to create account panel.
    void OnCreateNewClick()
    {
        // Debug message.
        Debug.Log("OnCreateNewClick called");

        // Hide login panel.
        if (loginPanel != null)
            loginPanel.style.display = DisplayStyle.None;

        // Show create account panel.
        if (createPanel != null)
            createPanel.style.display = DisplayStyle.Flex;

        // Focus username input for better UX.
        if (createPlayerNameField != null)
            createPlayerNameField.Focus();

        // Clear any error messages.
        ClearError();
    }

    // Handles account creation.
    void OnCreateClick()
    {
        // Debug message.
        Debug.Log("OnCreateClick called");

        // Get input values.
        string playerName = createPlayerNameField != null ? createPlayerNameField.value.Trim() : "";
        string password = createPasswordField != null ? createPasswordField.value : "";
        string confirmPassword = confirmPasswordField != null ? confirmPasswordField.value : "";

        // Debug log.
        Debug.Log($"Create attempt with name: '{playerName}'");

        // Validate player name is not empty.
        if (string.IsNullOrEmpty(playerName))
        {
            ShowError("Player name cannot be empty");
            return;
        }

        // Validate minimum username length.
        if (playerName.Length < 3)
        {
            ShowError("Player name must be at least 3 characters");
            return;
        }

        // Validate maximum username length.
        if (playerName.Length > 20)
        {
            ShowError("Player name must be 20 characters or less");
            return;
        }

        // Validate password is not empty.
        if (string.IsNullOrEmpty(password))
        {
            ShowError("Password cannot be empty");
            return;
        }

        // Validate password strength (minimum length).
        if (password.Length < 6)
        {
            ShowError("Password must be at least 6 characters");
            return;
        }

        // Check password confirmation matches.
        if (password != confirmPassword)
        {
            ShowError("Passwords do not match");

            // Clear password fields for retry.
            confirmPasswordField.value = "";
            createPasswordField.value = "";
            return;
        }

        // Check if player already exists.
        if (PlayerManager.Instance.PlayerExists(playerName))
        {
            ShowError($"Player '{playerName}' already exists");
            return;
        }

        // Create new player account.
        if (PlayerManager.Instance.CreateNewPlayer(playerName, password))
        {
            Debug.Log($"Player created: {playerName}");

            // Auto-login after successful creation.
            if (PlayerManager.Instance.Login(playerName, password))
            {
                // Load game scene.
                LoadGameScene();
            }
        }
        else
        {
            // Creation failed.
            ShowError("Failed to create account. Please try again.");
        }
    }

    // Returns from create account screen to login screen.
    void OnBackClick()
    {
        // Debug message.
        Debug.Log("OnBackClick called");

        // Show login panel.
        if (loginPanel != null)
            loginPanel.style.display = DisplayStyle.Flex;

        // Hide create panel.
        if (createPanel != null)
            createPanel.style.display = DisplayStyle.None;

        // Focus login username field.
        if (loginPlayerNameField != null)
            loginPlayerNameField.Focus();

        // Clear all password fields.
        if (loginPasswordField != null)
            loginPasswordField.value = "";
        if (createPasswordField != null)
            createPasswordField.value = "";
        if (confirmPasswordField != null)
            confirmPasswordField.value = "";

        // Clear any error messages.
        ClearError();
    }

    // Displays an error message on screen.
    void ShowError(string message)
    {
        // Debug warning log.
        Debug.LogWarning($"Error shown: {message}");

        // If error label exists...
        if (errorMessage != null)
        {
            // Set error text.
            errorMessage.text = message;

            // Make it visible.
            errorMessage.style.display = DisplayStyle.Flex;
        }
    }

    // Clears any displayed error message.
    void ClearError()
    {
        // If error label exists...
        if (errorMessage != null)
        {
            // Clear text.
            errorMessage.text = "";

            // Hide label.
            errorMessage.style.display = DisplayStyle.None;
        }
    }

    // Loads the main game scene after login or account creation.
    void LoadGameScene()
    {
        // Debug message.
        Debug.Log("Loading GameScene...");

        // Load the scene (currently set to MainMenuScene).
        SceneManager.LoadScene("MainMenuScene");
    }
}