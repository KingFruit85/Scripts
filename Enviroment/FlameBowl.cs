using UnityEngine;

public class FlameBowl : MonoBehaviour
{
   private Animator anim;
   void Awake()
   {
       anim = GetComponent<Animator>();
       Light();
   }

   public void Light()
   {
       anim.Play("lit");
   }
   
   public void UnLight()
   {
       anim.Play("unlit");
   }
}
