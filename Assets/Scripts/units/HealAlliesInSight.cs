using UnityEngine;
using Xianxiao;

public class HealAlliesInSight : MonoBehaviour
{
    [SerializeField] private int healAmount = 10;  // 每次回血量
    [SerializeField] private float healInterval = 2f;  // 回血间隔时间
    [SerializeField] private float sightRadius = 10f;  // 检测友军的视野范围

    private float nextHealTime;
    private Collider[] allyColliders = new Collider[50];  // 存储检测到的友军碰撞体
    private int foundAlliesCount;  // 检测到的友军数量
    private AbstractCommandable selfCommandable;  // 自身的 AbstractCommandable 组件

    private void Awake()
    {
        // 获取自身的 AbstractCommandable 组件
        selfCommandable = GetComponent<AbstractCommandable>();
    }

    private void Update()
    {
        if (Time.time >= nextHealTime)
        {
            HealNearbyAllies();
            nextHealTime = Time.time + healInterval;
        }
    }

    private void HealNearbyAllies()
    {
        // 检测范围内的友军
        foundAlliesCount = Physics.OverlapSphereNonAlloc(
            transform.position, 
            sightRadius, 
            allyColliders,
            LayerMask.GetMask("Units")
        );

        for (int i = 0; i < foundAlliesCount; i++)
        {
            AbstractCommandable ally = allyColliders[i].GetComponent<AbstractCommandable>();
            
            // 确保是友军且不是自己
            if (ally != null && ally.Owner == selfCommandable.Owner && ally != selfCommandable)
            {
                ally.Heal(healAmount);
            }
        }
    }

    // 在Scene视图中显示视野范围（可选）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
}

