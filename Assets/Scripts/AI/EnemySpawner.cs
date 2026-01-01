using UnityEngine;
using System.Collections;
using Xianxiao;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private AbstractUnitSO enemyUnitPrefab; // 敌方单位预制体
    [SerializeField] private Transform spawnPoint; // 生成点位置
    [SerializeField] private Transform targetBase; // 我方大本营位置
    [SerializeField] private float spawnInterval = 300f; // 生成间隔时间(300秒)

    private void Start()
    {
        StartCoroutine(SpawnEnemyUnits());
    }

    private IEnumerator SpawnEnemyUnits()
    {
        while (true)
        {
            SpawnEnemyUnit();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemyUnit()
    {
        GameObject enemyInstance = Instantiate(enemyUnitPrefab.Prefab, spawnPoint.position, spawnPoint.rotation);
        
        if (enemyInstance.TryGetComponent(out AbstractUnit enemyUnit))
        {
            // 设置敌方单位所有者为敌方
            enemyUnit.Owner = Owner.AI1;
            
            // 让敌方单位攻击我方大本营
            if (targetBase.TryGetComponent(out IDamageable damageable))
            {
                enemyUnit.Attack(damageable);
            }
        }
    }
}
