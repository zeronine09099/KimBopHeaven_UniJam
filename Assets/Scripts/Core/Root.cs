using Game;
using Game.Field;

namespace Core
{
    public static class Root
    {
        public static StageManager StageManager => StageManager.Instance;
        public static UIManager UIManager => UIManager.Instance;
        
        
        public static Field Field => GameManager.Instance.Field;
    }
}