using System.Collections;
using Common.Singleton;
using Title;
using UnityEngine;

namespace Core
{
    public class UIManager : Singleton<UIManager>
    {
        protected override void AfterAwake()
        {
            
        }
        
        public IEnumerator Init()
        {
            yield return null;
        }

        [field:SerializeField] public TitleUI TitleUI { get; set; }
        
    }
}