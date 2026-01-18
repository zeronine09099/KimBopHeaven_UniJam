namespace Android
{
    using System.Collections;
    using System.IO;
    using UnityEngine;
    using UnityEngine.Networking;

// 게임 시작 시 가장 먼저 실행되어야 합니다.


    
    [DefaultExecutionOrder(-100000)]
    public class GameplayTagCopier : MonoBehaviour
    {
        // 복사가 완료되었는지 확인하는 플래그
        public static bool IsReady = false; 

        IEnumerator Start()
        {
            string fileName = "GameplayTags";
            string destPath = Path.Combine(Application.persistentDataPath, fileName);
            Debug.Log($"현재 저장소 경로: {Application.persistentDataPath}");
            // 이미 파일이 복사되어 있다면 스킵 (버전 관리가 필요하다면 로직 추가 필요)
            if (File.Exists(destPath))
            {
                IsReady = true;
                yield break;
            }

            string srcPath = Path.Combine(Application.streamingAssetsPath, fileName);
        
            // 안드로이드일 경우 UnityWebRequest 사용
            if (Application.platform == RuntimePlatform.Android)
            {
                using (UnityWebRequest www = UnityWebRequest.Get(srcPath))
                {
                    yield return www.SendWebRequest();

                    if (www.result == UnityWebRequest.Result.Success)
                    {
                        File.WriteAllBytes(destPath, www.downloadHandler.data);
                        Debug.Log("GameplayTags copied to persistentDataPath (Android)");
                    }
                    else
                    {
                        Debug.LogError($"Error copying tags: {www.error}");
                    }
                }
            }
            else
            {
                // PC/Editor 등은 그냥 파일 복사
                File.Copy(srcPath, destPath, true);
            }

            IsReady = true;
        }
    }
}