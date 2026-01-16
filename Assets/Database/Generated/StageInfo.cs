using System.Text;
using System;
using System.Collections.Generic;

namespace Database.Generated
{

    [UnityEngine.Scripting.Preserve]
    [Serializable]
    public partial class StageInfo: IDBData {

        /// <summary> 스테이지 번호 </summary>
        public int Stage;
        /// <summary> 목표 점수 </summary>
        public int goalScore;
        /// <summary> 이동횟수 </summary>
        public int moveCount;
        /// <summary> 일반등급 확률 </summary>
        public float normalPercentage;
        /// <summary> 희귀등급 확률 </summary>
        public float rarePercentage;
        /// <summary> 특급등급 확률 </summary>
        public float epicPercentage;
    }
}
