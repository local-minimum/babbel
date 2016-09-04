using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Babbel
{
    public class StoryAct : AbstractBoard
    {

        List<Scene> scenes = new List<Scene>();

        public override bool Contains(SpeechAct speechAct)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(Tag tag)
        {
            return scenes.Any(e => e.Contains(tag));
        }

        public override IEnumerable<T> All<T>()
        {
            return scenes.SelectMany(e => e.All<T>());
        }
    }

}