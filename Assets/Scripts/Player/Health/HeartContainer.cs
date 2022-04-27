using System.Collections.Generic;
using System.Text;
using UnityEngine;

public partial class HeartContainer : MonoBehaviour
{
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform unitsTransform;
    [SerializeField] private int maxHearts;

    private readonly List<Heart> _hearts = new List<Heart>();
    private readonly List<HeartUnit> _units = new List<HeartUnit>();
    private int _addedCount;
    
    [Header("EditorField")] 
    public int indexToPop;
    public HeartOrigin origin;
    public HeartType type;
    public DamageType damageToApply;
    
    protected virtual HeartUnit GetUnit(GameObject from) => from.GetComponent<HeartUnit>();

    
    
    #region Units

    private void MoveUnit(int indexFrom, int newIndex)
    {
        HeartUnit unit = _units[indexFrom];
        _units.Insert(newIndex, unit);
        _units.RemoveAt(indexFrom);
    }
    
    private void Start()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < maxHearts; i++)
        {
            _units.Add(CreateHeartUnit());
        }
    }

    private HeartUnit CreateHeartUnit()
    {
        GameObject unitGameObject = Instantiate(unitPrefab, unitsTransform);
        unitGameObject.SetActive(false);
        return GetUnit(unitGameObject);;
    }

    private bool GetFreeUnit(out HeartUnit unit)
    {
        unit = _addedCount < maxHearts ? _units[_addedCount] : null;
        return unit is not null;
    }

    #endregion
    
    
    
    
    public bool Add(Heart toAdd)
    {
        if (GetFreeUnit(out HeartUnit unit))
        {
            toAdd.SetUnit(unit);
            toAdd.Index = _addedCount;
            toAdd.OnCreated();
            _hearts.Insert(_addedCount, toAdd);
            unit.gameObject.SetActive(true);
            unit.PLayCreate();
            _addedCount++;
            return true;
        }

        return false;
    }

    public void Pop(int index)
    {
        Heart heart = _hearts[index];
        _addedCount--;
        heart.Pop();
        heart.Unit.PlayPop(maxHearts);
        MoveUnit(heart.Index, maxHearts);
        _hearts.RemoveAt(index);
        RecalculateHeartIndexes();
    }

    public void ChangeOrigin(int index, HeartOrigin newOrigin)
    {
        Heart heart = _hearts[index];
        Heart newHeart = Heart.Create(newOrigin, heart.Type);
        newHeart.Index = index;
        newHeart.SetUnit(heart.Unit);
        newHeart.Unit.PlayFlip();
        _hearts[index] = newHeart;
    }

    public void ApplyDamage(DamageType damageType)
    {
        if(_addedCount == 0) return;
        int index = _addedCount - 1;
        Heart heart = _hearts[_addedCount - 1];
        if (heart.ApplyDamage(damageToApply, BulletSize.Normal)) Pop(index);
    }

    


    private void RecalculateHeartIndexes()
    {
        for (int i = 0; i < _addedCount; i++)
        {
            _hearts[i].Index = i;
        }
    }

    public void PrintIndexes()
    {
        StringBuilder sb = new StringBuilder().Append("[");
        for (var i = 0; i < _hearts.Count; i++)
        {
            sb.Append(i).Append(i == _hearts.Count - 1 ? "" : " ");
        }

        sb.Append("]");
        Debug.Log(sb.ToString());
    }
    
}

