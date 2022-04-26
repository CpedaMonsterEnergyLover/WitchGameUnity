using System;
using System.Collections.Generic;
using UnityEngine;

public class HeartBar : MonoBehaviour
{
    public int maxPossibleHeartsAmount;

    public GameObject heartPrefab;

    public HeartOriginsData[] heartOriginsData =
    {
        new(HeartOrigin.Human, null),
        new(HeartOrigin.Shadow, null),
        new(HeartOrigin.Demonic, null),
        new(HeartOrigin.Beast, null),
        new(HeartOrigin.Spectral, null),
        new(HeartOrigin.Archdemonic, null),
        new(HeartOrigin.Wild, null),
    };

    [Header("Field for editor")] 
    public HeartData heartDataToAdd;
    public HeartType heartTypeToAdd;
    public int indexToConvert;
    

    private List<Heart> _hearts = new();
    private List<Heart> _activeHearts = new();
    private bool HasActiveHearts => _activeHearts.Count > 0;

    private void Start() => CreatePool();

    
    
    public void ApplyDamage(DamageType damageType)
    {
        if (!HasActiveHearts)
        {
            Debug.Log("Player's dead");
            return;
        }
        Heart heart = _activeHearts[^1];
        if (heart.ApplyDamage(damageType))
        {
            _activeHearts.RemoveAt(_activeHearts.IndexOf(heart));
        }
    }
    
    public void AddHeart(HeartData heartData, HeartType heartType)
    {
        int activeCount = _activeHearts.Count;
        if(activeCount >= maxPossibleHeartsAmount) return;
        SetHeart(activeCount, heartData, heartType);
        
    }
    
    public void ConvertHeart(int index, HeartOrigin origin)
    {
        Heart target = _hearts[index];
        if(target.Origin == origin) return;
        
        Heart newHeart = CreateHeartWithOrigin(origin, target.gameObject);
        newHeart.Init(GetHeartDataByOrigin(origin), target.Type, target.isPopping);
        newHeart.UpdateFlip();
        _hearts[index] = newHeart;
        Destroy(target);
    }

    
    
    
    

    private void SetHeart(int index, HeartData heartData, HeartType heartType)
    {
        Heart heart = _hearts[index];

        if (heart.Origin != heartData.origin)
        {
            Heart newHeart = CreateHeartWithOrigin(heartData.origin, heart.gameObject);
            Destroy(heart);
            heart = newHeart;
            _hearts[index] = newHeart;
        }
        
        heart.Init(heartData, heartType, heart.isPopping);
        heart.UpdateInstant();
        _activeHearts.Add(heart);
    }

    private void CreatePool()
    {
        RemoveAll();
        for (int i = 0; i < maxPossibleHeartsAmount; i++)
        {
            Heart heart = Instantiate(heartPrefab, transform).GetComponent<Heart>();
            heart.gameObject.SetActive(false);
            _hearts.Add(heart);
        }
    }
    
    private void RemoveAll()
    {
        foreach (Heart unit in _hearts) DestroyImmediate(unit.gameObject);
        _hearts = new List<Heart>();
        _activeHearts = new List<Heart>();
    }
    
    private static Heart CreateHeartWithOrigin(HeartOrigin origin, GameObject parent)
    {
        return origin switch
        {
            HeartOrigin.Beast => parent.AddComponent<HeartOfTheBeast>(),
            HeartOrigin.Shadow => parent.AddComponent<ShadowHeart>(),
            HeartOrigin.Demonic => parent.AddComponent<DemonicHeart>(),
            HeartOrigin.Human => parent.AddComponent<HumanHeart>(),
            HeartOrigin.Archdemonic => parent.AddComponent<ArchDemonicHeart>(),
            HeartOrigin.Wild => parent.AddComponent<HeartOfTheNature>(),
            HeartOrigin.Spectral => parent.AddComponent<SpectralHeart>(),
            _ => null
        };
    }

    private HeartData GetHeartDataByOrigin(HeartOrigin origin) => heartOriginsData[(int) origin].data;
}

[Serializable]
public struct HeartOriginsData
{
    public HeartOrigin origin;
    public HeartData data;

    public HeartOriginsData(HeartOrigin origin, HeartData data)
    {
        this.data = data;
        this.origin = origin;
    }
}
