using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Babbel
{
    public class Scene : AbstractBoard
    {

        public List<SpeechAct> speechActs = new List<SpeechAct>();

        public override bool Contains(SpeechAct speechAct)
        {
            throw new System.NotImplementedException();
        }

        public override bool Contains(Tag tag)
        {
            return speechActs.Any(e => e.Contains(tag));
        }

        public override IEnumerable<T> All<T>()
        {
            if (typeof(T) == typeof(SpeechAct))
            {
                return speechActs.Cast<T>();
            } else if (typeof(T) == typeof(Tag))
            {
                return speechActs.SelectMany(e => e.tags).Distinct().Cast<T>();
            } else
            {
                throw new NotSupportedException(string.Format("Not supporting getting all {0}", typeof(T)));
            }
        }
    }
}
