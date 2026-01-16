using UnityEngine;

namespace Common.Attributes
{
    /// <summary>
    /// 인스펙터에서 필드의 레이블을 커스텀 텍스트로 변경하는 속성입니다.
    /// </summary>
    public class LabelAttribute : PropertyAttribute
    {
        public string Text { get; }
        
        public LabelAttribute(string text)
        {
            Text = text;
        }
    }
}