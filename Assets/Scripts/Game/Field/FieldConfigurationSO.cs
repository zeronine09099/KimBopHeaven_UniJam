using UnityEngine;

namespace Game.Field
{
    [CreateAssetMenu(fileName = "FieldConfigurationSO", menuName = "Game/Field/Field Configuration")]
    public class FieldConfigurationSO : ScriptableObject
    {
        public float TileScale = 1.0f;
    }
}