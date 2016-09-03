using UnityEngine;
using UnityEditor;

namespace Babbel
{


    public abstract class AbstractBabbelWindow : EditorWindow
    {
        [SerializeField]
        BabbelEditorTheme _theme;

        private static string defaultThemeLocation = "Assets/Addons/Babbel/Style";

        public BabbelEditorTheme theme
        {
            get
            {
                return _theme;
            }
        }

        public void SetTheme(BabbelEditorTheme theme = null)
        {
            if (theme == null)
            {
                theme = AssetTools.LoadByQuery<BabbelEditorTheme>("t:BabbelEditorTheme");
            }

            if (theme == null)
            {

                AssetTools.EnsureFolder(defaultThemeLocation);
                theme = AssetTools.CreateAsset<BabbelEditorTheme>(defaultThemeLocation); ;

            }

            titleContent = new GUIContent(GetBabbelName(), theme.BabbelLogo);

            _theme = theme;
        }

        private string GetBabbelName()
        {
            if (typeof(TagsWindow) == this.GetType())
            {
                return "Babbel:Tags";
            }
            else if (typeof(StoryBoardWindow) == this.GetType())
            {
                return "Babbel:Story";
            }
            else
            {
                return "Babbel:???";
            }
        }

        protected virtual void OnGUI()
        {

            if (theme == null)
            {
                SetTheme();
            }

            GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), theme.WindowBackground, ScaleMode.StretchToFill);

        }

    }
}