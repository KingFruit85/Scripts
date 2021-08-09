using UnityEngine;

public class FlameBowl : MonoBehaviour
{
   public Animator anim;
   public bool startLit;
   void Awake()
   {
    anim = GetComponent<Animator>();
    if (!startLit)
    {
        UnLight();   
    }
   }

   public void Light()
   {
       anim.Play("lit");
   }
   
   public void UnLight()
   {
       anim.Play("unlit");
   }

   void Update()
   {
    if (startLit)
        {
            Light();
        }
   }
}
