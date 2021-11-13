using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkBehaviour : StateMachineBehaviour
{
    public const float blinkMinTime = 1;
    public const float blinkMaxTime = 10;

    private float blinkTimer = 0;
    private static readonly int Blink = Animator.StringToHash("blink");

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (blinkTimer <= 0)
        {
            blinkTimer = Random.Range(blinkMinTime, blinkMaxTime);
            animator.SetTrigger(Blink);
        }
        else
        {
            blinkTimer -= Time.deltaTime;
        }
    }

}
