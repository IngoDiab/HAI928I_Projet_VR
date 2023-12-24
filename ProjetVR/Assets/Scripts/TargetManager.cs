using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : Singleton<TargetManager>
{
    [SerializeField] List<Target> mAllTargets = new List<Target>();
    [SerializeField] int mNbTargetDestroyed = 0;

    public int GetNbTargets() { return mAllTargets.Count; }
    public int GetNbTargetsDestroyed() { return mNbTargetDestroyed; }
    public void AddDestroyed() { ++mNbTargetDestroyed; }
}
