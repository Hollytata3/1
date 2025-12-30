using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;

#if UNITY_EDITOR
using UnityEditor;  // 只在 Editor 中用到
#endif

public class MainMenuController : MonoBehaviour
{
    public RenderTexture videoTexture; // 在 Inspector 中拖入你的 VideoRT

    private VisualElement background;
    private VisualElement pressAnyKey;
    private VisualElement menuButtons;
    private VisualElement settingsPanel;
    private Slider volumeSlider;

    private bool hasEnteredMenu = false; // 标记是否已经进入菜单，避免重复触发

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // 找到各个元素
        background = root.Q<VisualElement>("Background");
        pressAnyKey = root.Q<Label>("PressAnyKeyText");
        menuButtons = root.Q<VisualElement>("MenuButtons");
        settingsPanel = root.Q<VisualElement>("SettingsPanel");
        volumeSlider = root.Q<Slider>();

        // 设置视频背景
        if (videoTexture != null)
        {
            background.style.backgroundImage = Background.FromRenderTexture(videoTexture);
        }
        else
        {
            Debug.LogError("VideoTexture 未拖入！请在 Inspector 中拖入 RenderTexture");
        }

        // 默认音量 80%
        volumeSlider.value = 0.8f;
        AudioListener.volume = volumeSlider.value;

        // 音量滑动条实时控制
        volumeSlider.RegisterValueChangedCallback(evt =>
        {
            AudioListener.volume = evt.newValue;
        });

        // 开始游戏按钮
        root.Q<Button>("start-button").clicked += () =>
        {
            SceneManager.LoadScene("LoginScene");
        };

        // 设置按钮
        root.Q<Button>("settings-button").clicked += () =>
        {
            settingsPanel.style.display = DisplayStyle.Flex;
            root.Q("start-button").style.display = DisplayStyle.None;
            root.Q("settings-button").style.display = DisplayStyle.None;
            root.Q("quit-button").style.display = DisplayStyle.None;
        };

        // === 退出按钮：新增安全退出逻辑 ===
        var quitButton = root.Q<Button>("quit-button");
        if (quitButton == null)
        {
            Debug.LogError("找不到 name 为 'quit-button' 的 Button！请检查 UXML 中的 name 是否正确。");
        }
        else
        {
            quitButton.clicked += QuitGame;
            // 可选：加个测试日志，确认按钮能点到
            // quitButton.clicked += () => Debug.Log("退出按钮被点击！");
        }

        // 返回按钮
        root.Q<Button>("back-button").clicked += () =>
        {
            settingsPanel.style.display = DisplayStyle.None;
            root.Q("start-button").style.display = DisplayStyle.Flex;
            root.Q("settings-button").style.display = DisplayStyle.Flex;
            root.Q("quit-button").style.display = DisplayStyle.Flex;
        };
    }

    // 新增：统一的退出方法
    private void QuitGame()
    {
        Debug.Log("执行退出游戏...");

#if UNITY_EDITOR
        // 在 Unity Editor 中：停止 Play 模式
        EditorApplication.isPlaying = false;
#else
        // 在打包后的游戏中：真正退出程序
        Application.Quit();
#endif
    }

    void Update()
    {
        // 如果已经进入菜单，就不再检测
        if (hasEnteredMenu) return;

        // 检测任意键盘按键 或 鼠标任意按钮按下
        if (Input.anyKeyDown)
        {
            pressAnyKey.style.display = DisplayStyle.None;
            menuButtons.style.display = DisplayStyle.Flex;
            hasEnteredMenu = true; // 只触发一次
        }
    }
}