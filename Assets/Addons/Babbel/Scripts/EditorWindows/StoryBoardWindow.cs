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
        string editActiveActName = null;
        string editAcitveSceneName = null;
        
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

            if (GUI.Button(menuPosition, theme.Menu, expandedMenu ? theme.TitleAcitveIconAligning : theme.TitleIconAligning)) {
                expandedMenu = !expandedMenu;
            }

            expandedMenuPosition.width = maxSize.x - (4 + expandedMenuPosition.x);

            GUILayout.BeginArea(expandedMenuPosition);


            if (expandedMenu)
            {

                //ACTS

                GUILayout.BeginHorizontal();
                if (GUILayout.Button(theme.StoryActs, theme.Title))
                {
                    activeAct = null;
                    activeScene = null;
                    editActiveActName = null;
                }

                foreach (StoryAct act in Story.acts)
                {
                    bool isTheActiveAct = act == activeAct && activeAct != null;
                    if (isTheActiveAct && editActiveActName != null)
                    {                        
                        editActiveActName = GUILayout.TextField(editActiveActName, theme.IconAligningInput);
                        if (GUILayout.Button(theme.SaveIcon, theme.Title))
                        {
                            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(activeAct), editActiveActName);
                            editActiveActName = null;
                        }
                    } else if (GUILayout.Button(act.name, isTheActiveAct ? theme.TitleAcitveIconAligning : theme.TitleIconAligning))
                    {
                        if (isTheActiveAct)
                        {
                            editActiveActName = act.name;
                        }
                        else
                        {
                            activeAct = act;
                            editActiveActName = null;
                        }
                        activeScene = null;

                    }
                }

                if (GUILayout.Button(theme.AddIcon, theme.Title))
                {
                    string newActPath = AssetDatabase.GetAssetPath(Story);
                    if (!string.IsNullOrEmpty(newActPath)) {
                        newActPath += "_Acts/";
                        AssetTools.EnsureFolder(newActPath);
                        activeAct = AssetTools.CreateAsset<StoryAct>(newActPath);
                        Story.acts.Add(activeAct);
                        editActiveActName = activeAct.name;
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                //SCENES

                GUILayout.BeginHorizontal();
                if (GUILayout.Button(theme.StoryScenes, theme.Title))
                {
                    activeScene = null;
                    editAcitveSceneName = null;
                }

                if (activeAct != null)
                {
                    foreach(Scene scene in activeAct.All<Scene>())
                    {
                        bool isTheActiveScene = scene == activeScene && activeScene != null;

                        if (isTheActiveScene && editAcitveSceneName != null)
                        {
                            editAcitveSceneName = GUILayout.TextField(editAcitveSceneName, theme.Input);
                            if (GUILayout.Button(theme.SaveIcon, theme.Title))
                            {
                                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(activeScene), editAcitveSceneName);
                                editAcitveSceneName = null;
                            }
                        }
                        else if (GUILayout.Button(scene.name, isTheActiveScene ? theme.TitleAcitveIconAligning : theme.TitleIconAligning))
                        {
                            if (isTheActiveScene)
                            {
                                editAcitveSceneName = scene.name;
                            } else
                            {
                                activeScene = scene;
                                editAcitveSceneName = null;
                            }
                        }
                    }
                }

                if (GUILayout.Button(theme.AddIcon, activeAct == null ? theme.DownStateToggle : theme.Title))
                {
                    //Only allow adding scenes when we are in an act!
                    if (activeAct != null)
                    {
                        string newScenePath = AssetDatabase.GetAssetPath(Story);
                        if (!string.IsNullOrEmpty(newScenePath))
                        {
                            newScenePath += "_Scenes/";
                            AssetTools.EnsureFolder(newScenePath);
                            activeScene = AssetTools.CreateAsset<Scene>(newScenePath);
                            activeAct.scenes.Add(activeScene);
                            editAcitveSceneName = activeScene.name;
                        }
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            } else
            {
                GUILayout.BeginHorizontal();
                if (activeAct == null)
                {
                    GUILayout.Label(theme.NoAct, theme.TitleAcitveIconAligning);
                } else
                {
                    GUILayout.Label(activeAct.name, activeScene == null ? theme.TitleAcitveIconAligning : theme.TitleIconAligning);

                    if (activeScene != null)
                    {
                        GUILayout.Label(activeScene.name, theme.TitleAcitveIconAligning);
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
