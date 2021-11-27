using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WoodTree : Interactable
{
    #region Vars

    // Public fields
    public new TreeData GetData() => (TreeData) Data;

    public new TreeSaveData GetInstanceData() => (TreeSaveData) InstanceData;

    // Private fields
    private bool _isShaking;
    
    #endregion
    
    
    
    #region ClassMethods

    protected override void Interact()
    {
        base.Interact();
        ChopTree();
    }

    protected override void InitInstanceData(InteractableSaveData data)
    {
        InstanceData = new TreeSaveData(data.identifier.id);
        InstanceData.instanceID = GenerateID();
    }

    public override void OnTileLoad()
    {
        base.OnTileLoad();
        if(GetInstanceData().isChopped) RemoveLeaves();
    }
    
     private void ChopTree()
     {
         if (_isShaking || GetInstanceData().isChopped) return;
         GetInstanceData().health--;
         Debug.Log("Hp left " + GetInstanceData().health);
         if (GetInstanceData().health > 0)
         {
             StartCoroutine(Shake(0.75f, 15f));
         }
         else 
         {
             _fader.FadeIn();
             StartCoroutine(Fall(2.5f, 
                 transform.position.x - GameObject.FindWithTag("Player").transform.position.x));
         }
     }

     private void RemoveLeaves()
     {
         if(_fader is not null) Destroy(_fader.gameObject);
     }
     
     #endregion



     #region Coroutines

     private IEnumerator Fall(float duration, float direction)
     {
         GetInstanceData().isChopped = true;
         _fader.IsBlocked = true;
         float t = 0.0f;
         while ( t  < duration )
         {
             t += Time.deltaTime;
             _fader.transform.rotation = Quaternion.AngleAxis
             (t / duration * 85f, 
                 direction < 0 ? Vector3.forward : Vector3.back);
             yield return null;
         }

         RemoveLeaves();
     }

     private IEnumerator Shake(float duration, float speed)
     {
         _isShaking = true;
         float t = 0.0f;
         while ( t  < duration )
         {
             t += Time.deltaTime;
             _fader.transform.rotation  = Quaternion.AngleAxis(Mathf.Sin(t * speed), Vector3.forward);
             yield return null;
         }
         _isShaking = false;
     }

     #endregion
}