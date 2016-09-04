using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Babbel
{
    public class StoryBoardWindow : AbstractBabbelWindow
    {

        bool expandedMenu = false;
        Rect menuPosition = new Rect(4, 4, 19, 19);
        Rect expandedMenuPosition = new Rect(25, 4, 0, 120);

        BabbelStory activeStory = null;
        StoryAct activeAct = null;
        Scene activeScene = null;
        
        static StoryBoardWindow window;

        public static BabbelStory Story
        {
            get
            {
                if (window == null)
                {
                    return BabbelStory.Main;
                } else if (window.activeStory == null)
                {
                    window.activeStory = BabbelStory.Main;
                }
                return window.activeStory;
            }
        }

        public static AbstractBoard ActiveBoard
        {
            get
            {
                if (window == null)
                {
                    return null;
                } else if (window.activeScene == null)
                {
                    return window.activeAct;
                } else
                {
                    return window.activeScene;
                }
            }
        }
        
        [MenuItem("Window/Babbel/Storyboard", priority = 0)]
        public static void ShowWindow()
        {
            window = GetWindow<StoryBoardWindow>(typeof(StoryBoardWindow));           
            window.Focus();
        }


        protected override void OnGUI()
        {
            base.OnGUI();
            AddMenu();
        }

        void AddMenu()
        {

            if (GUI.Button(menuPosition, theme.Menu, expandedMenu ? theme.TitleActive : theme.Title)) {
                expandedMenu = !expandedMenu;
            }

            expandedMenuPosition.width = maxSize.x - (4 + expandedMenuPosition.x);

            GUILayout.BeginArea(expandedMenuPosition);


            if (expandedMenu)
            {

                //ACTS

                GUILayout.BeginHorizontal();
                GUILayout.Label(theme.StoryActs, theme.Title);

                foreach (StoryAct act in Story.acts)
                {
                    bool isTheActiveAct = act == activeAct;
                    if (GUILayout.Button(act.name, isTheActiveAct ? theme.DownStateToggle : theme.UpStateToggle))
                    {
                        if (isTheActiveAct)
                        {
                            activeAct = null;
                        }
                        else
                        {
                            activeAct = act;
                        }
                        activeScene = null;

                    }
                }

                if (GUILayout.Button(theme.Add, theme.UpStateToggle))
                {

                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                //SCENES

                GUILayout.BeginHorizontal();
                GUILayout.Label(theme.StoryScenes, theme.Title);

                if (activeAct != null)
                {
                    foreach(Scene scene in activeAct.All<Scene>())
                    {
                        bool isTheActiveScene = scene == activeScene;
                        if (GUILayout.Button(scene.name, isTheActiveScene ? theme.DownStateToggle : theme.UpStateToggle))
                        {
                            if (isTheActiveScene)
                            {
                                activeScene = null;
                            } else
                            {
                                activeScene = scene;
                            }
                        }
                    }
                }

                if (GUILayout.Button(theme.Add, activeAct == null ? theme.DownStateToggle : theme.UpStateToggle))
                {
                    if (activeAct == null)
                    {

                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            } else
            {
                GUILayout.BeginHorizontal();
                if (activeAct == null)
                {
                    GUILayout.Label(theme.NoAct, theme.Title);
                } else
                {
                    GUILayout.Label(activeAct.name, theme.Title);

                    if (activeScene == null)
                    {
                        GUILayout.Label(theme.NoScene, theme.Text);
                    } else
                    {
                        GUILayout.Label(activeScene.name, theme.Text);
                    }
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndArea();

        }

    }

}
