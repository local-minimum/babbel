using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Babbel
{
    [CreateAssetMenu(fileName = "BabbelStory", menuName = "Babbel/New Story", order = 0)]
    public class BabbelStory : AbstractBoard
    {
        public static string defaultLocation = "Assets/Data/Babbel";

        public static BabbelStory Main
        {
            get
            {
                BabbelStory story = AssetTools.LoadByQuery<BabbelStory>("t:BabbelStory");
                if (story == null)
                {
                    return AssetTools.CreateAsset<BabbelStory>(defaultLocation);
                } else
                {
                    return story;
                }
            }
        }

        public List<StoryAct> acts = new List<StoryAct>();

        public override bool Contains(Tag tag)
        {
            return acts.Any(e => e.Contains(tag));
        }

        public override bool Contains(SpeechAct speechAct)
        {
            return acts.Any(e => e.Contains(speechAct));
        }

        public override IEnumerable<T> All<T>()
        {
            return acts.SelectMany(e => e.All<T>());
        }
    }

}