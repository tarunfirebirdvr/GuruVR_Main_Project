using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using TMPro;
using System.Text.RegularExpressions;

public class UserAuthConfig : MonoBehaviour
{
   
    [Header("UI References")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
   
    public TextMeshProUGUI statusText;
    public Button registerButton;
    public Button loginButton;
   

    async void Start()
    {
        await InitializeUnityServices();

        registerButton.onClick.AddListener(() =>
        {
             string username = usernameInput.text.Trim();
              string password = passwordInput.text; 
           /* string username = "tarun1";
            string password = "Pass@123";*/
            _ = RegisterAndLoginAsync(username, password);
        });

        loginButton.onClick.AddListener(() =>
        {
            string username = usernameInput.text.Trim();
            string password = passwordInput.text;
           /* string username = "tarun";
            string password = "Pass@123";*/
            _ = SignInAsync(username, password);
        });
    }

    async Task InitializeUnityServices()
    {
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
                Debug.Log(" Unity Services Initialized");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Unity Services Initialization Failed: " + e);
            statusText.text = $" Unity Init Error: {e.Message}";
        }
    }

    bool IsValidUsername(string username)
    {
        if (string.IsNullOrEmpty(username))
            return false;

        // Regex for valid characters and length (3–20)
        Regex regex = new Regex(@"^[a-zA-Z0-9._\-@]{3,20}$");
        return regex.IsMatch(username);
    }

    async Task RegisterAndLoginAsync(string username, string password)
    {
       /* if (!IsValidUsername(username))
        {
            Debug.LogWarning($" Invalid Username: '{username}' (Length: {username.Length})");
            statusText.text = " Username must be 3–20 chars: a-z, 0-9, ., -, _, @";
            return;
        }*/

        try
        {
            Debug.Log($"Attempting to register user: {username}");
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log(" Registration successful. Logging in...");
            statusText.text = " Registered. Logging in...";
            await SignInAsync(username, password);
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError(" Registration failed: " + ex.Message);
            statusText.text = $" Registration error: {ex.Message}";
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError(" Request failed: " + ex.Message);
            statusText.text = $"Request error: {ex.Message}";
        }
    }

    async Task SignInAsync(string username, string password)
    {
        try
        {
            Debug.Log($"Attempting to sign in user: {username}");
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log(" Sign In Successful");
            statusText.text = "Signed in!";
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError(" Sign In Failed: " + ex.Message);
            statusText.text = $"Sign In Error: {ex.Message}";
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError(" Request Failed: " + ex.Message);
            statusText.text = $"Request Error: {ex.Message}";
        }
    }
}