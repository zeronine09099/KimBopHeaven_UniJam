using System.Collections;
using Common.Singleton;
using Core;

namespace Game
{
    public class StageManager : Singleton<StageManager>
    {
        protected override void AfterAwake()
        {
            
        }
        
        public IEnumerator Init()
        {
            yield return null;
        }

        public IEnumerator StartStage()
        {
            // 스테이지 데이터 초기화
            Root.Field.InitField(6,6);

            yield return null;
        }
        
    }
}