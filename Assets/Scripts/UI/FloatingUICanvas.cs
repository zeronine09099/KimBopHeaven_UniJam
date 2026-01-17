using Common.Singleton;

namespace UI
{
    public class FloatingUICanvas : Singleton<FloatingUICanvas>
    {
        protected override bool DontDestroyOnLoad => false;

        protected override void AfterAwake()
        {
            
        }
    }
}