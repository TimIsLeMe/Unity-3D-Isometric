using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Entity // for anything that has effects (usually animations/sounds) when dying or taking damage
{
    public void Die();
    public void TakeDamage();
    
}
