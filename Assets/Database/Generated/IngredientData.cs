using System.Text;
using System;
using System.Collections.Generic;

namespace Database.Generated
{

    [UnityEngine.Scripting.Preserve]
    [Serializable]
    public partial class IngredientData: IDBData {

        /// <summary> 태그 </summary>
        public string tag;
        /// <summary> 영어이름 </summary>
        public string name;
        /// <summary> 한국어이름 </summary>
        public string koreanName;
        /// <summary> 기본 점수 </summary>
        public float baseScore;
        /// <summary> 인게임 설명 </summary>
        public string description;
        /// <summary> 희귀도 </summary>
        public Common.Rarity rarity;
        /// <summary> 기타 변수 1 </summary>
        public float variable01;
        /// <summary> 기타 변수 2 </summary>
        public float variable02;
        /// <summary> 기타변수 </summary>
        public List<string> additionalVariables;
    }
}
