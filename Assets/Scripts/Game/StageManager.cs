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
            /*
             * 초기화 단계
             */
            
            // 스테이지 데이터 초기화
            Root.Field.InitField(6,6);
            
            
            /*
             * 연출 단계
             */

            yield return null;
        }
        
        
        
        public IEnumerator StageSuccess()
        {
            // 스테이지 성공 처리, 리워드로
            yield return null;
        }
        
        public IEnumerator StageFail()
        {
            // 스테이지 실패 처리, 타이틀로
            yield return null;
        }
    }
}