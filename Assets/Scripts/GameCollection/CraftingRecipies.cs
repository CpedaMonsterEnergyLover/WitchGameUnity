using System.Collections.Generic;
using UnityEngine;

namespace GameCollection
{
    public class CraftingRecipies : MonoBehaviour
    {
        public List<CraftingRecipe> objects;
        
        public static List<CraftingRecipe> Collection = new();

        public void Init()
        {
            Collection.Clear();
            Collection = objects;
        }
    }
}