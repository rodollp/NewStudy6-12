using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class StageData
    {
        [Header("몬스터 종류")]
        public List<GameObject> monsters;
        [Header("최소 몬스터 수")]
        public int minSpawnCount;
        [Header("최대 몬스터 수")]
        public int maxSpawnCount;
        [Header("보스 스테이지인지 체크")]
        public bool isBossStage;
        [Header("보스 몬스터")]
        public GameObject boss;
    }
}
