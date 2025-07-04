using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class OTPInput : MonoBehaviour
{
    public TMP_InputField inputField;     // Assign in inspector
    public int otpLength = 7;             // Number of OTP characters
    private string rawOTP = "";           // Raw user input

    void Start()
    {
        inputField.characterLimit = otpLength;
        inputField.contentType = TMP_InputField.ContentType.Standard;
        inputField.onValueChanged.AddListener(OnValueChanged);
        inputField.text = "";
        inputField.ActivateInputField(); // Optional: auto-focus on start
    }

    void OnValueChanged(string text)
    {
        // Remove any formatting (spaces)
        string unformatted = text.Replace(" ", "");

        // Limit length
        if (unformatted.Length > otpLength)
            unformatted = unformatted.Substring(0, otpLength);

        rawOTP = unformatted;

        // Format with spaces
        string formatted = string.Join(" ", rawOTP.ToCharArray());

        // Set the formatted text without triggering value change again
        inputField.SetTextWithoutNotify(formatted);
        inputField.caretPosition = formatted.Length;

        // Submit when filled
        if (rawOTP.Length == otpLength)
        {
            EventSystem.current.SetSelectedGameObject(null); // closes keyboard
            OnOTPSubmitted();
        }
    }

    public void OnOTPSubmitted()
    {
        Debug.Log("OTP Submitted: " + rawOTP);
        // Your login logic here
    }

    public void ClearOTP()
    {
        rawOTP = "";
        inputField.SetTextWithoutNotify("");
        inputField.ActivateInputField();
    }
}
