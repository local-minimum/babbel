using UnityEngine;
using System.Collections;

namespace Babbel
{
    public abstract class LayoutableItem : ScriptableObject
    {

        public Vector2 anchor;
        public Vector2 size;
        public int zPosition;

    }

}