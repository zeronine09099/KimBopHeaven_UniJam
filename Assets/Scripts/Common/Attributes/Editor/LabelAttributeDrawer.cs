using UnityEditor;
using UnityEngine;

namespace Common.Attributes.Editor
{
    /// <summary>
    /// <see cref="LabelAttribute"/>를 사용한 필드의 인스펙터 표시를 담당하는 커스텀 프로퍼티 드로워입니다.
    /// 필드의 기본 레이블을 LabelAttribute에 지정된 텍스트로 변경합니다.
    /// </summary>
    [CustomPropertyDrawer(typeof(LabelAttribute))] 
    public class LabelAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            LabelAttribute labelAttribute = attribute as LabelAttribute;
            string tooltip = label.tooltip;
            if (labelAttribute != null)
            {
                label.text = labelAttribute.Text;
                
            }
            
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.LabelField(position, new GUIContent("", tooltip));
            EditorGUI.EndProperty();
        }
    }
}