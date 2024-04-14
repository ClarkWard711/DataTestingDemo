using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TagModule : ScriptableObject
{
    public abstract void Apply(TagInfo tagInfo);
}
