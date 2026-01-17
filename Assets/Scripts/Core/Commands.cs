using System;
using System.Collections.Generic;
using BandoWare.GameplayTags;
using Game;
using Machamy.DeveloperConsole.Attributes;
using Machamy.DeveloperConsole.Commands;
using Player;
using UnityEngine.Scripting;

namespace Core
{
    [ConsoleCommandClass,Preserve]
    public class InventoryAddCommand : IConsoleCommand
    {
        public string Command => "inventory.add";
        public string Description => "인벤토리에 재료를 추가합니다. Usage: inventory.add <ingredient_tag> [count]";
        public string Signature => "<ingredient_tag> [count]";
        
        public void Execute(string[] args)
        {
            if (args.Length < 1)
            {
                Machamy.DeveloperConsole.McConsole.MessageError("Usage: " + Signature);
                return;
            }

            string ingredientTagString = args[0];
            
            // 게임플레이 태그 파싱 (문자열에서 암시적 변환)
            GameplayTag ingredientTag = ingredientTagString;

            // IngredientLibrary에서 재료 찾기
            if (!IngredientLibrary.Instance.IsInitialized)
            {
                Machamy.DeveloperConsole.McConsole.MessageError("IngredientLibrary is not initialized yet.");
                return;
            }

            if (!IngredientLibrary.Instance.AllIngredients.TryGetValue(ingredientTag, out var ingredient))
            {
                Machamy.DeveloperConsole.McConsole.MessageError($"Ingredient not found for tag: {ingredientTagString}");
                return;
            }

            // 개수 파싱 (기본값 1)
            int count = 1;
            if (args.Length >= 2)
            {
                if (!int.TryParse(args[1], out count) || count <= 0)
                {
                    Machamy.DeveloperConsole.McConsole.MessageError($"Invalid count: {args[1]}. Must be a positive integer.");
                    return;
                }
            }

            // PlayerState의 GameDeck에 추가
            if (PlayerState.Current != null && PlayerState.Current.GameDeck != null)
            {
                PlayerState.Current.GameDeck.AddIngredient(ingredient, count);
                Machamy.DeveloperConsole.McConsole.MessageSuccess($"Added {count}x {ingredient.name} ({ingredientTagString}) to inventory");
            }
            else
            {
                Machamy.DeveloperConsole.McConsole.MessageError("PlayerState or GameDeck is not available.");
            }
        }

        public void AutoComplete(Span<string> args, ref List<string> suggestions)
        {
            if (args.Length == 1)
            {
                // 재료 태그 자동완성
                if (!IngredientLibrary.Instance.IsInitialized)
                    return;

                string input = args[0];
                foreach (var tag in IngredientLibrary.Instance.AllIngredientTagList)
                {
                    string tagString = tag.ToString();
                    if (tagString.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                    {
                        suggestions.Add(tagString);
                    }
                }
            }
            else if (args.Length == 2)
            {
                // 개수 예시 자동완성
                suggestions.Add("1");
                suggestions.Add("5");
                suggestions.Add("10");
                suggestions.Add("20");
                suggestions.Add("50");
                suggestions.Add("100");
            }
        }
    }

    [ConsoleCommandClass,Preserve]
    public class PuzzleAddCommand : IConsoleCommand
    {
        public string Command => "puzzle.add";
        public string Description => "퍼즐의 가능한 재료 목록(possibleIngredients)에 재료를 추가합니다.";
        public string Signature => "<ingredient_tag> [count]";
        
        public void Execute(string[] args)
        {
            if (args.Length < 1)
            {
                Machamy.DeveloperConsole.McConsole.MessageError("Usage: " + Signature);
                return;
            }

            string ingredientTagString = args[0];
            
            // 게임플레이 태그 파싱 (문자열에서 암시적 변환)
            GameplayTag ingredientTag = ingredientTagString;

            // IngredientLibrary에서 재료 찾기
            if (!IngredientLibrary.Instance.IsInitialized)
            {
                Machamy.DeveloperConsole.McConsole.MessageError("IngredientLibrary is not initialized yet.");
                return;
            }

            if (!IngredientLibrary.Instance.AllIngredients.TryGetValue(ingredientTag, out var ingredient))
            {
                Machamy.DeveloperConsole.McConsole.MessageError($"Ingredient not found for tag: {ingredientTagString}");
                return;
            }

            // 개수 파싱 (기본값 1)
            int count = 1;
            if (args.Length >= 2)
            {
                if (!int.TryParse(args[1], out count) || count <= 0)
                {
                    Machamy.DeveloperConsole.McConsole.MessageError($"Invalid count: {args[1]}. Must be a positive integer.");
                    return;
                }
            }

            // PuzzleManager의 possibleIngredients에 추가
            if (PuzzleManager.Instance != null)
            {
                PuzzleManager.Instance.AddToPossibleIngredients(ingredient, count);
                Machamy.DeveloperConsole.McConsole.MessageSuccess($"Added {count}x {ingredient.name} ({ingredientTagString}) to puzzle possible ingredients (immediately available)");
            }
            else
            {
                Machamy.DeveloperConsole.McConsole.MessageError("PuzzleManager is not available.");
            }
        }

        public void AutoComplete(Span<string> args, ref List<string> suggestions)
        {
            if (args.Length == 1)
            {
                // 재료 태그 자동완성
                if (!IngredientLibrary.Instance.IsInitialized)
                    return;

                string input = args[0];
                foreach (var tag in IngredientLibrary.Instance.AllIngredientTagList)
                {
                    string tagString = tag.ToString();
                    if (tagString.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                    {
                        suggestions.Add(tagString);
                    }
                }
            }
            else if (args.Length == 2)
            {
                // 개수 예시 자동완성
                suggestions.Add("1");
                suggestions.Add("5");
                suggestions.Add("10");
                suggestions.Add("20");
                suggestions.Add("50");
                suggestions.Add("100");
            }
        }
    }
}