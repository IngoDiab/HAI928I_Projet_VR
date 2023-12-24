using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GPE : MonoBehaviour
{
    public abstract void UseGPE(Player _playerAttached);

    public abstract void ExitGPE();
}
