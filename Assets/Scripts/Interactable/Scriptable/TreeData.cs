using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Tree")]
[Serializable]
public class TreeData : InteractableData
{
    [Header("Tree data")] 
    
    [Tooltip("Имеет ли плоды. Если нет, то данные о цветении и плодоносии не будут использоваться")]
    public bool hasFruits = false;
    
    /*[Tooltip("Сезон, в котором дерево начинает цвести")]
    [SerializeField]
    public Season bloomSeason = Season.Spring;*/
    
    [Range(0.1f, 6f)]
    [Tooltip("Длительность цветения в сезонах")]
    public float bloomDuration = 1;
    
    /*[Tooltip("Сезон, в котором дерево начинает плодоносить")]
    [SerializeField]
    public Season fertileSeason = Season.EarlyAutumn;*/
    
    [Range(0.1f, 6f)]
    [Tooltip("Длительность созревания плодов в сезонах")]
    public float fertileDuration = 1;

    [SerializeField] private string barkItem;
    [SerializeField] private string resinItem;
    [SerializeField] private string logItem;

}