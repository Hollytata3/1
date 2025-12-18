using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;  // 新增这行：新输入系统

public class MainMenuController : MonoBehaviour
{
    private VisualElement root;
    private Label pressLabel;
    private VisualElement mainButtons;
    private bool hasPressed = false;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        pressLabel = root.Q<Label>("PressAnyKeyLabel");
        mainButtons = root.Q<VisualElement>("MainButtons");

        root.Q<Button>("start-btn").clicked += StartGame;
        root.Q<Button>("settings-btn").clicked += OpenSettings;
        root.Q<Button>("quit-btn").clicked += QuitGame;
    }

    void Update()
    {
        // 新输入系统检测：任意键或鼠标左键
        if (!hasPressed &&
            (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame ||
             Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame))
        {
            hasPressed = true;
            ShowMainMenu();
        }
    }

    private void ShowMainMenu()
    {
        pressLabel.style.display = DisplayStyle.None;
        mainButtons.style.display = DisplayStyle.Flex;
    }

    private void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // 改成你的游戏场景名
    }

    private void OpenSettings()
    {
        Debug.Log("打开设置");
    }

    private void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}