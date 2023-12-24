using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GPE : MonoBehaviour
{
    public abstract void UseGPE(Player _playerAttached, Hand _handAttached);

    public abstract void ExitGPE(Player _playerAttached, Hand _handAttached);
}
