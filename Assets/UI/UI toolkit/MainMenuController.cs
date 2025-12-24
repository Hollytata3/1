using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class MainMenuController : MonoBehaviour
{
    public RenderTexture videoTexture; // 在 Inspector 中拖入你的 VideoRT

    private VisualElement background;
    private VisualElement pressAnyKey;
    private VisualElement menuButtons;
    private VisualElement settingsPanel;
    private Slider volumeSlider;

    private bool hasEnteredMenu = false; // 新增：标记是否已经进入菜单，避免重复触发

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

        // 按钮点击事件
        root.Q<Button>("start-button").clicked += () =>
        {
            SceneManager.LoadScene("LoginScene");
        };

        root.Q<Button>("settings-button").clicked += () =>
        {
            settingsPanel.style.display = DisplayStyle.Flex;
            root.Q("start-button").style.display = DisplayStyle.None;
            root.Q("settings-button").style.display = DisplayStyle.None;
            root.Q("quit-button").style.display = DisplayStyle.None;
        };

        root.Q<Button>("quit-button").clicked += Application.Quit;

        root.Q<Button>("back-button").clicked += () =>
        {
            settingsPanel.style.display = DisplayStyle.None;
            root.Q("start-button").style.display = DisplayStyle.Flex;
            root.Q("settings-button").style.display = DisplayStyle.Flex;
            root.Q("quit-button").style.display = DisplayStyle.Flex;
        };

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