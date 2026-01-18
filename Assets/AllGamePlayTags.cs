

namespace BandoWare.GameplayTags
{



}

namespace BandoWare.GameplayTags
{
    public static class AllGameplayTags
    {
        public static class Ingredient
        {
            public static class Test
            {
                public static class TestIngredient
                {
                    private static readonly GameplayTag _tag = new GameplayTag("Ingredient.Test.TestIngredient");
                    public static GameplayTag Get() => _tag;
                }
            }

            public static class Essential
            {
                public static readonly GameplayTag _tag = new GameplayTag("Ingredient.Essential");
                public static GameplayTag Get() => _tag;
                public static class Gim
                {
                    private static readonly GameplayTag _tag = new GameplayTag("Ingredient.Essential.Gim");
                    /// <summary>김 - 필수 재료</summary>
                    public static GameplayTag Get() => _tag;
                }
                public static class Rice
                {
                    private static readonly GameplayTag _tag = new GameplayTag("Ingredient.Essential.Rice");
                    /// <summary>밥 - 필수 재료</summary>
                    public static GameplayTag Get() => _tag;
                }
                public static class PickledRadish
                {
                    private static readonly GameplayTag _tag = new GameplayTag("Ingredient.Essential.PickledRadish");
                    /// <summary>단무지 - 필수 재료</summary>
                    public static GameplayTag Get() => _tag;
                }
                public static class Ham
                {
                    private static readonly GameplayTag _tag = new GameplayTag("Ingredient.Essential.Ham");
                    /// <summary>햄 - 필수 재료</summary>
                    public static GameplayTag Get() => _tag;
                }
            }

            public static class Meat
            {
                public static readonly GameplayTag _tag = new("Ingredient.Meat");
                public static GameplayTag Get() => _tag;
                
                public static class CrabStick { private static readonly GameplayTag _tag = new("Ingredient.Meat.CrabStick"); public static GameplayTag Get() => _tag; }
                public static class Egg { private static readonly GameplayTag _tag = new("Ingredient.Meat.Egg"); public static GameplayTag Get() => _tag; }
                public static class PorkBelly { private static readonly GameplayTag _tag = new("Ingredient.Meat.PorkBelly"); public static GameplayTag Get() => _tag; }
                public static class Chicken { private static readonly GameplayTag _tag = new("Ingredient.Meat.Chicken"); public static GameplayTag Get() => _tag; }
                public static class PorkCutlet { private static readonly GameplayTag _tag = new("Ingredient.Meat.PorkCutlet"); public static GameplayTag Get() => _tag; }
                public static class Bacon { private static readonly GameplayTag _tag = new("Ingredient.Meat.Bacon"); public static GameplayTag Get() => _tag; }
                public static class Duck { private static readonly GameplayTag _tag = new("Ingredient.Meat.Duck"); public static GameplayTag Get() => _tag; }
            }

            public static class Seafood
            {
                public static readonly GameplayTag _tag = new("Ingredient.Seafood");
                public static GameplayTag Get() => _tag;
                public static class Salmon { private static readonly GameplayTag _tag = new("Ingredient.Seafood.Salmon"); public static GameplayTag Get() => _tag; }
                public static class Octopus { private static readonly GameplayTag _tag = new("Ingredient.Seafood.Octopus"); public static GameplayTag Get() => _tag; }
                public static class Shrimp { private static readonly GameplayTag _tag = new("Ingredient.Seafood.Shrimp"); public static GameplayTag Get() => _tag; }
                public static class FishCake { private static readonly GameplayTag _tag = new("Ingredient.Seafood.FishCake"); public static GameplayTag Get() => _tag; }
                public static class CrabStick { private static readonly GameplayTag _tag = new("Ingredient.Seafood.CrabStick"); public static GameplayTag Get() => _tag; }
                public static class Tuna { private static readonly GameplayTag _tag = new("Ingredient.Seafood.Tuna"); public static GameplayTag Get() => _tag; }
            }

            public static class Vegetable
            {
                private static readonly GameplayTag _tag = new("Ingredient.Vegetable");
                public static GameplayTag Get() => _tag; 
                
                public static class Burdock { private static readonly GameplayTag _tag = new("Ingredient.Vegetable.Burdock"); public static GameplayTag Get() => _tag; }
                public static class Carrot { private static readonly GameplayTag _tag = new("Ingredient.Vegetable.Carrot"); public static GameplayTag Get() => _tag; }
                public static class Spinach { private static readonly GameplayTag _tag = new("Ingredient.Vegetable.Spinach"); public static GameplayTag Get() => _tag; }
                public static class Kimchi { private static readonly GameplayTag _tag = new("Ingredient.Vegetable.Kimchi"); public static GameplayTag Get() => _tag; }
                public static class Mushroom { private static readonly GameplayTag _tag = new("Ingredient.Vegetable.Mushroom"); public static GameplayTag Get() => _tag; }
                public static class GrilledMushroom { private static readonly GameplayTag _tag = new("Ingredient.Vegetable.GrilledMushroom"); public static GameplayTag Get() => _tag; }
                public static class ChiliPepper { private static readonly GameplayTag _tag = new("Ingredient.Vegetable.ChiliPepper"); public static GameplayTag Get() => _tag; }
                public static class Fire { private static readonly GameplayTag _tag = new("Ingredient.Vegetable.Fire"); public static GameplayTag Get() => _tag; }
                public static class Garlic { private static readonly GameplayTag _tag = new("Ingredient.Vegetable.Garlic"); public static GameplayTag Get() => _tag; }
                public static class GrilledGarlic { private static readonly GameplayTag _tag = new("Ingredient.Vegetable.GrilledGarlic"); public static GameplayTag Get() => _tag; }
                public static class PerillaLeaf { private static readonly GameplayTag _tag = new("Ingredient.Vegetable.PerillaLeaf"); public static GameplayTag Get() => _tag; }
                public static class Cucumber { private static readonly GameplayTag _tag = new("Ingredient.Vegetable.Cucumber"); public static GameplayTag Get() => _tag; }
                public static class Tofu { private static readonly GameplayTag _tag = new("Ingredient.Vegetable.Tofu"); public static GameplayTag Get() => _tag; }
            }

            public static class Sauce
            {
                public static readonly GameplayTag _tag = new("Ingredient.Sauce");
                public static GameplayTag Get() => _tag;
                
                public static class Mayonnaise { private static readonly GameplayTag _tag = new("Ingredient.Sauce.Mayonnaise"); public static GameplayTag Get() => _tag; }
                public static class SoySauce { private static readonly GameplayTag _tag = new("Ingredient.Sauce.SoySauce"); public static GameplayTag Get() => _tag; }
                public static class SesameOil { private static readonly GameplayTag _tag = new("Ingredient.Sauce.SesameOil"); public static GameplayTag Get() => _tag; }
                public static class Cheese { private static readonly GameplayTag _tag = new("Ingredient.Sauce.Cheese"); public static GameplayTag Get() => _tag; }
                public static class HotSauce { private static readonly GameplayTag _tag = new("Ingredient.Sauce.HotSauce"); public static GameplayTag Get() => _tag; }
            }

            public static class Etc
            {
                public static readonly GameplayTag _tag = new("Ingredient.Etc");
                public static GameplayTag Get() => _tag;
                public static class Cheese { private static readonly GameplayTag _tag = new("Ingredient.Etc.Cheese"); public static GameplayTag Get() => _tag; }
                public static class Fire { private static readonly GameplayTag _tag = new("Ingredient.Etc.Fire"); public static GameplayTag Get() => _tag; }
                public static class Tofu { private static readonly GameplayTag _tag = new("Ingredient.Etc.Tofu"); public static GameplayTag Get() => _tag; }
                public static class Egg { private static readonly GameplayTag _tag = new("Ingredient.Etc.Egg"); public static GameplayTag Get() => _tag; }
                public static class Air { private static readonly GameplayTag _tag = new("Ingredient.Etc.Air"); public static GameplayTag Get() => _tag; }
            }
        }

        public static class Category
        {
            public static class Meat { private static readonly GameplayTag _tag = new("Category.Meat"); public static GameplayTag Get() => _tag; }
            public static class Seafood { private static readonly GameplayTag _tag = new("Category.Seafood"); public static GameplayTag Get() => _tag; }
            public static class Vegetable { private static readonly GameplayTag _tag = new("Category.Vegetable"); public static GameplayTag Get() => _tag; }
            public static class Sauce { private static readonly GameplayTag _tag = new("Category.Sauce"); public static GameplayTag Get() => _tag; }
        }

        public static class Encounter
        {
            public static class MeatSelect { private static readonly GameplayTag _tag = new("Encounter.MeatSelect"); public static GameplayTag Get() => _tag; }
            public static class VegetableSelect { private static readonly GameplayTag _tag = new("Encounter.VegetableSelect"); public static GameplayTag Get() => _tag; }
            public static class SeafoodSelect { private static readonly GameplayTag _tag = new("Encounter.SeafoodSelect"); public static GameplayTag Get() => _tag; }
        }
    }
}