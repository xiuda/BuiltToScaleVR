using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
   public Animator anim;

   void LateUpdate()
   {
       if (IsAnimationNotPlaying())
           Destroy(gameObject);
   }
   
   public bool IsAnimationNotPlaying()
   {
       float compare = anim.GetCurrentAnimatorStateInfo(0).speed;
       return anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= compare && !anim.IsInTransition(0);
   }
}
