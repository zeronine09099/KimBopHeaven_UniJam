using System.Text;
using System;
using System.Collections.Generic;

namespace Database.Generated
{

    [UnityEngine.Scripting.Preserve]
    [Serializable]
    public partial class EncounterReward: IDBData {

        /// <summary> 인카운터 태그 </summary>
        public string tag;
        /// <summary> 보상 적용 대상 </summary>
        public string target;
        /// <summary> 보상 수치 </summary>
        public int value;
        /// <summary> 한국어이름 </summary>
        public string koreanName;
    }
}
