using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class WoodTree : Interactable
{
    // Public fields
    public new TreeData Data => (TreeData) data;
    public new TreeSaveData SaveData => (TreeSaveData) saveData;

    // Private fields
    [SerializeField]
    public bool isFalling;

    private Coroutine _fallRoutine;
    private Coroutine _shakeRoutine;
    private float _fallDirection;

    private void OnDisable()
    {
        if (_fallRoutine is not null)
        {
            StopCoroutine(_fallRoutine);
            OnFallDown();
        }
        if(_shakeRoutine is not null) StopCoroutine(_shakeRoutine);
    }
    
    
    protected override void InitSaveData(InteractableData origin)
    {
        saveData = ScriptableObject.CreateInstance<TreeSaveData>();
        saveData.id = origin.id;
        if (origin is TreeData treeData)
        {
            SaveData.health = treeData.health;
        }
    }


    public override void OnTileLoad(WorldTile loadedTile)
    {
        base.OnTileLoad(loadedTile);
        if(SaveData.health <= Data.fallOnHealth) RemoveLeaves();
    }

    public void Chop(int dmg)
    {
        if (SaveData.health - dmg <= 0 && !SaveData.isChopped) SaveData.health = 1;
        else SaveData.health -= dmg;

        Debug.Log($"{SaveData.health} hp left");

        if (SaveData.health > 0)
        {
         if (SaveData.health <= Data.fallOnHealth && !SaveData.isChopped)
             _fallRoutine = StartCoroutine(Fall(2.5f, 
                 transform.position.x - GameObject.FindWithTag("Player").transform.position.x));
         else _shakeRoutine = StartCoroutine(Shake(1f, (float) Math.PI * 6f));
        }
        else 
        {
         Destroy();
        }
    }

     private void RemoveLeaves()
     {
         if(Fader is not null) Destroy(Fader.gameObject);
     }

     private void OnFallDown()
     {
         isFalling = false;
         RemoveLeaves();

         
         Vector2 logPosition = transform.position;
         
         for (int i = 1; i <= Data.logAmount; i++)
         {
             ItemEntity itemEntity = (ItemEntity)Entity.Create(
                 new ItemEntitySaveData(Item.Create(Data.logItem.identifier), 1, 
                     logPosition + Vector2.right / 2f * _fallDirection * i));
             
             itemEntity.rigidbody.AddForce(Random.insideUnitCircle.normalized * 7.5f);
             
         }
     }


     #region Coroutines

     private IEnumerator Fall(float duration, float direction)
     {
         _fallDirection = direction;
         isFalling = true;
         SaveData.isChopped = true;
         FadeIn();
         Fader.IsBlocked = true;
         float t = 0.0f;
         
         while ( t  < duration )
         {
             t += Time.deltaTime;
             Fader.transform.rotation = Quaternion.AngleAxis
             (t / duration * 85f, 
                 direction < 0 ? Vector3.forward : Vector3.back);
             yield return null;
         }

         _fallRoutine = null;
         OnFallDown();
     }

     private IEnumerator Shake(float duration, float speed)
     {
         float rootsX = transform.position.x;
         
         float t = 0.0f;
         while ( t  < duration )
         {
             t += Time.deltaTime;
             if (!SaveData.isChopped)
                Fader.transform.rotation  = Quaternion.AngleAxis(Mathf.Sin(t * speed), Vector3.forward);
             else
             {
                 Vector3 position = transform.position;
                 position.x = rootsX + Mathf.Sin(t * speed) / 30;
                 transform.position = position;
             }
             yield return null;
         }

         _shakeRoutine = null;
     }

     #endregion
}