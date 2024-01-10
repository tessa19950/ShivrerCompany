using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShivrerCompany.util
{
    internal class EnemyUtil
    {
        public static void SpawnEnemyFromVent(EnemyVent vent, string query)
        {
            RoundManager manager = RoundManager.Instance;
            for (int i = 0; i < manager.currentLevel.Enemies.Count; i++)
            {
                SpawnableEnemyWithRarity spawnableEnemy = manager.currentLevel.Enemies[i];
                if (spawnableEnemy.enemyType.enemyName.Contains(query, StringComparison.OrdinalIgnoreCase))
                {
                    Plugin.instance.Log("Spawning enemy [" + spawnableEnemy.enemyType.enemyName + "] from a vent!");
                    EnemyUtil.SpawnEnemyFromVent(vent, i);
                }
            }
        }

        public static void SpawnEnemyFromVent(EnemyVent vent, int enemyType)
        {
            Vector3 position = vent.floorNode.position;
            float y = vent.floorNode.eulerAngles.y;
            RoundManager.Instance.SpawnEnemyOnServer(position, y, enemyType);
            vent.OpenVentClientRpc();
            vent.occupied = false;
        }
    }
}
