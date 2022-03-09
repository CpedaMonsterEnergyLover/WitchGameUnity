using System.Collections.Generic;
using UnityEngine;

namespace NewGen
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField, Header("Игрок")]
        private Transform playerTransform;
        
        [Header("Игровые настройки")]
        public PlayerSettings playerSettings;
        
        [Header("К чему крепить Interactable")]
        public Transform interactableTransform;
        
        [SerializeField, Header("Генератор")]
        private Generator generator;
        public bool generateOnStart = true;
        
        [Header("Слои грида")]
        public List<WorldLayer> layers;

        
        
        public WorldData WorldData { get; private set; }


        
        private void Awake()
        {
            Application.targetFrameRate = playerSettings.targetFrameRate;
            
            if (!generateOnStart) return;
            
            ClearAllTiles();
            GenerateWorld();
            
            int mapCenterX = generator.generatorSettings.width / 2;
            int mapCenterY = generator.generatorSettings.height / 2;
            playerTransform.position = new Vector3(mapCenterX, mapCenterY, 0f);
        }
        
        public void GenerateWorld()
        {
           WorldData = generator.GenerateWorld(layers);
        }

        public void DrawAllTiles()
        {
            for (int x = 0; x < generator.generatorSettings.width; x++)
            {
                for (int y = 0; y < generator.generatorSettings.height; y++)
                {
                    DrawTile(x, y);
                }
            }
        }

        public void ClearAllTiles()
        {
            layers.ForEach(layer => layer.tilemap.ClearAllTiles());
            while (interactableTransform.childCount > 0)
                DestroyImmediate(interactableTransform.GetChild(0).gameObject);
        }
        
        public void DrawTile(int x, int y)
        {
            Vector3Int position = new Vector3Int(x, y, 0);

            WorldTile tile = WorldData.GetTile(x, y);
            
            for (var i = 0; i < layers.Count; i++)
            {
                WorldLayer worldLayer = layers[i];
                worldLayer.tilemap.SetTile(position, 
                    tile.Layers[i] ? worldLayer.tileBase : null);
            }
        }

        public void EraseTile(int x, int y)
        {
            Vector3Int position = new Vector3Int(x, y, 0);
            layers.ForEach(layer =>
            {
                layer.tilemap.SetTile(position, null); 
            });
        }
    }
}