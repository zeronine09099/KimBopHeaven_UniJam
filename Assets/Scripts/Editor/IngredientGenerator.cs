using System.IO;
using System.Text;
using Database;
using Machamy.Utils;
using UnityEditor;
using UnityEngine;


public static class IngredientGenerator
{
    [MenuItem("Tools/Generate Ingredient ScriptableObject")]
    public static void GenerateIngredientScriptableObjectCS()
    {
        if (DatabaseManager.Instance)
        {
            var database = DatabaseManager.Instance.Database;
            var dataList = database.IngredientDataList;
            foreach (var data in dataList)
            {
                LogEx.Log($"Generating Ingredient ScriptableObject for {data.name} with tag {data.tag}");
                GenerateCSFile(data.tag, data.name);
            }
            AssetDatabase.Refresh();
            LogEx.Log("Ingredient ScriptableObject generation completed.");
        }
        else
        {
            LogEx.LogError("DatabaseManager 인스턴스를 찾을 수 없습니다. 먼저 데이터베이스를 초기화하세요.");
        }
    }


    private static void GenerateCSFile(string tag, string name)
    {
        string folderPath = Path.Combine(Application.dataPath, "Scripts", "Game", "Ingredients") + "/";
        string camelCaseName = char.ToUpper(name[0]) + name.Substring(1).ToLower();
        string className = camelCaseName + "Ingredient";
        string filePath = folderPath + className + ".cs";

        if (System.IO.File.Exists(filePath))
        {
            UnityEngine.Debug.LogError("File already exists: " + filePath);
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($@"using System.Collections;
using BandoWare.GameplayTags;
using BandoWare.GameplayTags;
using Game.Field;

namespace Game.Ingredients
{{
    public class {className} : IngredientSO
    {{
        public override GameplayTag Tag => AllGameplayTags.{tag}.Get();
        public override IEnumerator OnFall(Tile tile)
        {{
            yield break;
        }}

        public override IEnumerator OnExplode(Tile tile)
        {{
            yield break;
        }}
    }}
}}
");
        
        System.IO.File.WriteAllText(filePath, sb.ToString());
        UnityEngine.Debug.Log("Generated Ingredient ScriptableObject: " + filePath);
        
    }


}
