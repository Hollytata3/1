using UnityEngine;
using Xianxiao;

public class HealAlliesInSight : MonoBehaviour
{
    [SerializeField] private int healAmount = 10;  // 每次回血量
    [SerializeField] private float healInterval = 2f;  // 回血间隔时间
    [SerializeField] private float sightRadius = 10f;  // 检测范围
    [SerializeField] private int damageAmount = 10;  // 对敌军伤害量

    private float nextActionTime;  // 下次行动时间
    private Collider[] detectedUnits = new Collider[50];  // 存储检测到的单位
    private int detectedUnitsCount;  // 检测到的单位数量
    private AbstractCommandable selfCommandable;  // 自身的 AbstractCommandable 组件

    private void Awake()
    {
        selfCommandable = GetComponent<AbstractCommandable>();
    }

    private void Update()
    {
        if (Time.time >= nextActionTime)
        {
            ProcessNearbyUnits();
            nextActionTime = Time.time + healInterval;
        }
    }

    private void ProcessNearbyUnits()
    {
        // 检测范围内的所有单位
        detectedUnitsCount = Physics.OverlapSphereNonAlloc(
            transform.position, 
            sightRadius, 
            detectedUnits,
            LayerMask.GetMask("Units")
        );

        for (int i = 0; i < detectedUnitsCount; i++)
        {
            AbstractCommandable unit = detectedUnits[i].GetComponent<AbstractCommandable>();
            
            if (unit == null || unit == selfCommandable) continue;

            if (unit.Owner == selfCommandable.Owner)
            {
                // 友军回血
                unit.Heal(healAmount);
            }
            else
            {
                // 敌军造成伤害
                unit.TakeDamage(damageAmount);
            }
        }
    }
}


