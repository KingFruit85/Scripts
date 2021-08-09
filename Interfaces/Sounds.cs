using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaySound
{
    void TakeMeleeDamage(string receiverOfDamage);
    void TakeRangedDamage(string receiverOfDamage);

}
