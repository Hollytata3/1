using UnityEngine;
using Xianxiao;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private float dayDuration = 300f; // 5分钟（300秒）完成一个昼夜循环
    [SerializeField] private Light directionalLight; // 主方向光
    
    private float timeOfDay; // 当前时间（0-1之间，0为日出，0.5为日落）
    private float daySpeed; // 每秒变化的时间
    private AbstractCommandable[] commandables; // 场景中的所有可命令单位

    private void Awake()
    {
        daySpeed = 1f / dayDuration;
        commandables = FindObjectsByType<AbstractCommandable>(FindObjectsSortMode.None);
        timeOfDay = 0.5f;
        if (directionalLight != null)
        {
            directionalLight.transform.rotation = Quaternion.Euler(90f, 170f, 0);
        }
    }

    private void Update()
    {
        // 更新时间
        timeOfDay += Time.deltaTime * daySpeed;
        if (timeOfDay >= 1f)
        {
            timeOfDay -= 1f;
        }

        // 更新光照
        UpdateLighting();    
    }

    private void UpdateLighting()
    {
        // 计算太阳角度（0-360度）
        float sunAngle = timeOfDay * 360f;
           
        // 更新方向光旋转
        if (directionalLight != null)
        {
            directionalLight.transform.rotation = Quaternion.Euler(sunAngle - 90f, 170f, 0);
        }
    }
}



