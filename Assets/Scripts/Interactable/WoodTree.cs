using System;
using System.Collections;
using UnityEngine;

public class WoodTree : Interactable
{
    #region Vars

    // Public fields
    public new TreeData Data => (TreeData) data;
    public new TreeSaveData InstanceData => (TreeSaveData) instanceData;

    // Private fields
    [SerializeField]
    public bool isFalling;
    
    #endregion
    
    
    
    #region ClassMethods

    protected override void InitInstanceData(InteractableSaveData saveData)
    {
        instanceData = new TreeSaveData(saveData);
        InstanceData.health = Data.health;
        base.InitInstanceData(saveData);
    }

    public override void OnTileLoad(WorldTile loadedTile)
    {
        base.OnTileLoad(loadedTile);
        if(InstanceData.health <= Data.fallOnHealth) RemoveLeaves();
    }

    public void Chop(int dmg)
    {
        if (InstanceData.health - dmg <= 0 && !InstanceData.isChopped) InstanceData.health = 1;
        else InstanceData.health -= dmg;

        Debug.Log($"{InstanceData.health} hp left");

        if (InstanceData.health > 0)
        {
         if (InstanceData.health <= Data.fallOnHealth && !InstanceData.isChopped)
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
         InstanceData.isChopped = true;
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
             if (!InstanceData.isChopped)
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