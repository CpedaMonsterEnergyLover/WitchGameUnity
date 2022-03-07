using System;
using System.Collections;
using UnityEngine;

public class WoodTree : Interactable
{
    #region Vars

    // Public fields
    public new TreeData Data => (TreeData) data;
    public new TreeSaveData SaveData => (TreeSaveData) saveData;

    // Private fields
    [SerializeField]
    public bool isFalling;
    
    #endregion
    
    
    
    #region ClassMethods

    protected override void InitSaveData(InteractableData origin)
    {
        saveData = new TreeSaveData(origin);
        SaveData.health = Data.health;
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
             StartCoroutine(Fall(2.5f, 
                 transform.position.x - GameObject.FindWithTag("Player").transform.position.x));
         else StartCoroutine(Shake(1f, (float) Math.PI * 6f));
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
     
     #endregion



     #region Coroutines

     private IEnumerator Fall(float duration, float direction)
     {
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

         isFalling = false;
         RemoveLeaves();
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
     }

     #endregion
}