using System;
using System.Collections.Generic;
using UnityEngine;
using Database.Generated;

namespace Database
{
    [UnityEngine.Scripting.Preserve]
    [Serializable]
    public class McDatabase
    {
        public List<EncounterReward> EncounterRewardList = new List<EncounterReward>();
        public List<StageInfo> StageInfoList = new List<StageInfo>();
        public List<IngredientData> IngredientDataList = new List<IngredientData>();
        public readonly List<string> ClassNames = new List<string> {
            "EncounterReward",
            "StageInfo",
            "IngredientData"
        };


        public T FindByName<T>(string name) where T : class
        {
            if (typeof(T) == null) return null;
            switch (typeof(T).Name)
            {
                case "IngredientData":
                    foreach (var instance in IngredientDataList)
                    {
                        if (instance.name == name)
                            return instance as T;
                    }
                    break;
                default:
                    Debug.LogWarning($"[MDatabase] 정의되지 않은 클래스 타입: {typeof(T).Name}");
                    return null;
            }
            return null;
        }

        public T FindByTag<T>(string tag) where T : class
        {
            if (typeof(T) == null) return null;
            switch (typeof(T).Name)
            {
                case "IngredientData":
                    foreach (var instance in IngredientDataList)
                    {
                        if (instance.tag == tag)
                            return instance as T;
                    }
                    break;
                default:
                    Debug.LogWarning($"[MDatabase] 정의되지 않은 클래스 타입: {typeof(T).Name}");
                    return null;
            }
            return null;
        }

        public void ClearAll()
        {
            EncounterRewardList.Clear();
            StageInfoList.Clear();
            IngredientDataList.Clear();
        }



        private List<T> CreateInstance<T>(DataFrame df) where T : new()
        {
            object[] instances = ClassInstanceFactory.CreateInstance(df);
            List<T> list = new List<T>();
            foreach (var instance in instances)
            {
                if (instance is T typedInstance)
                {
                    list.Add(typedInstance);
                }
            }
            return list;
        }

        public void InitializeAll(List<DataFrame> dataFrames)
        {
            foreach (var df in dataFrames)
            {
                switch (df.name)
                {
                    case "EncounterReward":
                        EncounterRewardList = CreateInstance<EncounterReward>(df);
                        break;
                    case "StageInfo":
                        StageInfoList = CreateInstance<StageInfo>(df);
                        break;
                    case "IngredientData":
                        IngredientDataList = CreateInstance<IngredientData>(df);
                        break;
                    default:
                        Debug.LogWarning($"[MDatabase] 정의되지 않은 클래스 이름: {df.name}");
                        break;
                }
            }
        }


        public void AddInstancesFromJsonList(string className, string json)
        {
            switch (className)
            {
                case "EncounterReward":
                    var newEncounterRewardItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EncounterReward>>(json);
                    EncounterRewardList.AddRange(newEncounterRewardItems);
                    break;
                case "StageInfo":
                    var newStageInfoItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StageInfo>>(json);
                    StageInfoList.AddRange(newStageInfoItems);
                    break;
                case "IngredientData":
                    var newIngredientDataItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IngredientData>>(json);
                    IngredientDataList.AddRange(newIngredientDataItems);
                    break;
                default:
                    Debug.LogWarning($"[MDatabase] 정의되지 않은 클래스 이름: {className}");
                    break;
            }
        }


        public Type GetTypeByName(string className)
        {
            switch (className)
            {
                case "EncounterReward":
                    return typeof(EncounterReward);
                case "StageInfo":
                    return typeof(StageInfo);
                case "IngredientData":
                    return typeof(IngredientData);
                default:
                    Debug.LogWarning($"[MDatabase] 정의되지 않은 클래스 이름: {className}");
                    return null;
            }
        }
    }
}
