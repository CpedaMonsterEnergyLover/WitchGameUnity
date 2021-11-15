using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBehaviour : StateMachineBehaviour
{
    private const float MinTime = 1;
    private const float MaxTime = 9;

    private float _timer = 0;
    private static readonly int Blink = Animator.StringToHash("Blink");
    private static readonly int Tongue = Animator.StringToHash("Tongue");
    private int[] blinkTongue = {Blink, Tongue};

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_timer <= 0)
        {
            _timer = Random.Range(MinTime, MaxTime);
            animator.SetTrigger(blinkTongue[Random.Range(0,2)]);
        }
        else  _timer -= Time.deltaTime;
    
    }

}
