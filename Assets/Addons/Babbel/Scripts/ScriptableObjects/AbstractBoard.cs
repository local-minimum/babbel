using UnityEngine;
using System.Collections.Generic;

namespace Babbel
{

    public abstract class AbstractBoard : ScriptableObject
    {
        public Vector2 viewAnchor = Vector2.zero;
        public float viewScale = 0;

        abstract public bool Contains(Tag tag);
        abstract public bool Contains(SpeechAct speechAct);
        abstract public IEnumerable<T> All<T>() where T : ScriptableObject;
    }

}