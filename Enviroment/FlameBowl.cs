using UnityEngine;

public class FlameBowl : MonoBehaviour
{
   public Animator anim;
   void Awake()
   {
       anim = GetComponent<Animator>();
       UnLight();
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
