using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewGen
{
    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance;

        
        [Header("Игровые настройки")]
        public TileLoader playerSettings;
        [Header("К чему крепить Interactable")]
        public Transform interactableTransform;
        [Header("Генератор")]
        private Generator _generator;
        [Header("Слои грида")]
        [SerializeField]
        private List<WorldLayer> layers;
        [Header("Загрузчик мира")]
        [SerializeField]
        private TileLoader tileLoader;
        

        public WorldData WorldData { get; private set; }

        public void Init()
        {
            Instance = this;
           
        }
        
        public void GenerateWorld()
        {
           WorldData = _generator.GenerateWorld(layers);
        }

        public void DrawAllTiles()
        {
            Debug.Log("Draw");
        }

        public void ClearAllTiles()
        {
            layers.ForEach(layer => layer.Tilemap.ClearAllTiles());
            while (interactableTransform.childCount > 0)
                DestroyImmediate(interactableTransform.GetChild(0).gameObject);
        }
    }
}