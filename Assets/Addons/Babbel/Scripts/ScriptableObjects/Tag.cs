using UnityEngine;
using System.Collections;

namespace Babbel {
	    
    public enum FilterMatching {
        Never = 0,
        Case = 1 << 0,
        Name = 1 << 1,
        Description = 1 << 2,
        Both = (1 << 1) | (1 << 2),
        Any = (1 << 1) | (1 << 2) | (1 << 3)
    };

    public static class FilterMatchingTools
    {
        public static bool In(this FilterMatching me, FilterMatching other)
        {
            return (me & other) == me;
        }
    }
    
	public class Tag : ScriptableObject {
        public string description;

        public bool Matches(string filter, FilterMatching criteria = FilterMatching.Any)
        {
            if (criteria == FilterMatching.Never)
            {
                return false;
            }

            string desc = description;
            string name = this.name;

            if (!FilterMatching.Case.In(criteria))
            {
                filter = filter.ToLower();
                if (FilterMatching.Name.In(criteria)) {
                    name = this.name.ToLower();
                } 

                if (FilterMatching.Description.In(criteria))
                {
                    desc = description.ToLower();
                } 
                
            }

            if (FilterMatching.Any.In(criteria))
            {
                return name.Contains(filter) || desc.Contains(filter);
            }
            else if (FilterMatching.Both.In(criteria))
            {
                return name.Contains(filter) && desc.Contains(filter);
            } else if (FilterMatching.Name.In(criteria))
            {
                return name.Contains(filter);
            } else if (FilterMatching.Description.In(criteria))
            {
                return desc.Contains(filter);
            } else
            {
                throw new System.MissingMemberException("Can't filter on requested criteria: " + criteria);
            }
        }
    }
}