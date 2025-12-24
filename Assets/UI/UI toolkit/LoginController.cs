using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    private TextField usernameField;
    private TextField passwordField;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // 获取输入框
        usernameField = root.Q<TextField>("username-field");
        passwordField = root.Q<TextField>("password-field");

        // 登录按钮
        root.Q<Button>("login-btn").clicked += OnLoginClicked;

        // 注册按钮（临时提示）
        root.Q<Button>("register-btn").clicked += () =>
        {
            Debug.Log("注册功能正在开发中...");
            // 以后可以跳转注册场景
        };
    }

    private void OnLoginClicked()
    {
        string username = usernameField.value.Trim();
        string password = passwordField.value;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.Log("请输入完整的账号和密码！");
            return;
        }

        // 简单验证（你以后可以换成服务器验证或PlayerPrefs）
        if (username == "admin" && password == "123456")
        {
            Debug.Log("登录成功，进入游戏！");
            SceneManager.LoadScene("MainMenu"); // 跳转到你的主菜单场景
        }
        else
        {
            Debug.Log("账号或密码错误！");
            // 后续可以加一个红色提示Label
        }
    }
}