using System.Collections.Generic;
using UnityEngine;

namespace GameCollection
{
    public class CraftingRecipies : MonoBehaviour
    {
        public List<CraftingRecipe> objects;
        
        public static List<CraftingRecipe> Collection;

        public void Init()
        {
            Collection = objects;
        }
    }
}