using System;
using System.IO;
using System.Text;
using BandoWare.GameplayTags;
using Database;
using Game;
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

    [MenuItem("Tools/Generate Ingredient ScriptableObject(Overwrite)")]
    public static void GenerateIngredientScriptableObjectCS_Overwrite()
    {
        if (DatabaseManager.Instance)
        {
            var database = DatabaseManager.Instance.Database;
            var dataList = database.IngredientDataList;
            foreach (var data in dataList)
            {
                LogEx.Log($"Generating Ingredient ScriptableObject for {data.name} with tag {data.tag} (overwrite)");
                GenerateCSFile(data.tag, data.name, overwrite: true);
            }
            AssetDatabase.Refresh();
            LogEx.Log("Ingredient ScriptableObject generation (overwrite) completed.");
        }
        else
        {
            LogEx.LogError("DatabaseManager 인스턴스를 찾을 수 없습니다. 먼저 데이터베이스를 초기화하세요.");
        }
    }

    [MenuItem("Tools/Generate Ingredient ScriptableObject Asset(Overwrite)")]
    public static void GenerateIngredientScriptableObjectAssets_Overwrite()
    {
        var ingredientTypes = ReflectionUtil.FindDerivedTypes<IngredientSO>();
        foreach (var type in ingredientTypes)
        {
            GenerateSOFile(type, overwrite: true);
        }
        AssetDatabase.Refresh();
        LogEx.Log("Ingredient ScriptableObject asset generation (overwrite) completed.");
    }
    
    [MenuItem("Tools/Generate Ingredient ScriptableObject Assets")]
    public static void GenerateIngredientScriptableObjectAssets()
    {
        var ingredientTypes = ReflectionUtil.FindDerivedTypes<IngredientSO>();
        foreach (var type in ingredientTypes)
        {
            GenerateSOFile(type);
        }
        AssetDatabase.Refresh();
        LogEx.Log("Ingredient ScriptableObject asset generation completed.");
    }

    private static void GenerateSOFile(Type ingredientType, bool overwrite = false)
    {
        string folderPath = Path.Combine(Application.dataPath, "Resources", "ScriptableObjects", "Ingredients") + "/";
        string className = ingredientType.Name;
        string filePath = folderPath + className + ".asset";
        if (System.IO.File.Exists(filePath) && !overwrite)
        {
            UnityEngine.Debug.LogError("File already exists: " + filePath);
            return;
        }
        IngredientSO ingredientSO = ScriptableObject.CreateInstance(ingredientType) as IngredientSO;
        if (ingredientSO == null)
        {
            UnityEngine.Debug.LogError("Failed to create instance of: " + ingredientType.Name);
            return;
        }

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        AssetDatabase.CreateAsset(ingredientSO, "Assets/Resources/ScriptableObjects/Ingredients/" + className + ".asset");
        AssetDatabase.SaveAssets();
    }


    private static void GenerateCSFile(string tag, string name, bool overwrite = false)
    {
        string folderPath = Path.Combine(Application.dataPath, "Scripts", "Game", "Ingredients") + "/";
        string camelCaseName = char.ToUpper(name[0]) + name.Substring(1).ToLower();
        string className = camelCaseName + "Ingredient";
        string filePath = folderPath + className + ".cs";

        if (System.IO.File.Exists(filePath) && !overwrite)
        {
            UnityEngine.Debug.LogError("File already exists: " + filePath);
            return;
        }
        

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($@"using System.Collections;
using BandoWare.GameplayTags;
using Game.Field;
using Cysharp.Threading.Tasks;

namespace Game.Ingredients
{{
    public class {className}SO : IngredientSO
    {{
        public override GameplayTag Tag => AllGameplayTags.{tag}.Get();

        public override async UniTask OnFall(Tile tile)
        {{ 
            await base.OnFall(tile);
          
        }}

        public override async UniTask OnTrigger(Tile tile)
        {{
            await base.OnTrigger(tile);
         
        }}
    }}
}}
");
        
        System.IO.File.WriteAllText(filePath, sb.ToString());
        UnityEngine.Debug.Log("Generated Ingredient ScriptableObject: " + filePath);
        
    }


}
