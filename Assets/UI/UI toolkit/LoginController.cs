using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    private TextField usernameField;
    private TextField passwordField;
    private Button loginButton;
    private Label errorLabel;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        usernameField = root.Q<TextField>("username-field");
        passwordField = root.Q<TextField>("password-field");
        loginButton = root.Q<Button>("login-button");
        errorLabel = root.Q<Label>("error-label");

        loginButton.clicked += OnLoginButtonClicked;
    }

    private void OnLoginButtonClicked()
    {
        string username = usernameField.value.Trim();
        string password = passwordField.value;

        if ((username == "yuxinbo" || username == "xiezeyuan") && password == "88888888")
        {
            // 登录成功，跳转场景（替换为你的目标场景名或索引）
            SceneManager.LoadScene("DemoScene");  // 或 SceneManager.LoadScene(1);
        }
        else
        {
            // 显示错误提示
            errorLabel.style.display = DisplayStyle.Flex;
            errorLabel.text = "账号或密码错误！";
        }
    }
}