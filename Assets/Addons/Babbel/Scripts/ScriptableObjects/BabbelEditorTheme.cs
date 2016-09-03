using UnityEngine;


namespace Babbel
{
    [CreateAssetMenu(fileName = "BabbelTheme", menuName = "Babbel/New Theme", order = 1)]
    public class BabbelEditorTheme : ScriptableObject
    {
        public GUISettings TagSettings;

        [Space()]

        public Texture2D BabbelLogo;
        public Texture2D WindowBackground;

        [Space()]

        public GUIStyle UpStateToggle;
        public GUIStyle DownStateToggle;
        public GUIStyle Input;
        public GUIStyle IconAligningInput;
        public GUIStyle Title;
        public GUIStyle Text;
        public GUIStyle Scrolls;

        [Space()]

        public GUIContent StoryActs;
        public GUIContent StoryScenes;
        
        [Space()]

        public GUIContent Add;
        public GUIContent Save;
        public GUIContent SaveClose;
        public GUIContent Close;
        public GUIContent Delete;
        public GUIContent Filter;
        public GUIContent Menu;

        [Space()]

        public Texture2D textIcon;

        [Space()]

        public GUIContent TagModeAll;
        public GUIContent TagModeRanked;
        public GUIContent TagModeUsed;
        public GUIContent TagModeUsedActive;
        public GUIContent TagModeNotUsed;   
        
    }

}