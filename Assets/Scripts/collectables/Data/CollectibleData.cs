using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CollectibleData : ScriptableObject
{
    [SerializeField]
    private int _count;

    public int Count
    {
        get { return _count; }
        set { _count = value; }
    }

}
