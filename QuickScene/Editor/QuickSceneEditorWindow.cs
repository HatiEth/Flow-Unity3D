using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class QuickSceneEditorWindow : EditorWindow {

    [MenuItem("Window/QuickScene %#o")]
    public static void OpenEditorWindow()
    {
        var w = EditorWindow.GetWindow(typeof(QuickSceneEditorWindow)) as QuickSceneEditorWindow;
        w.titleContent = new GUIContent("QuickScene");
        w.Show();
    }

    List<string> AssetFilePaths = new List<string>();

    public void OnEnable()
    {
        DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
        FileInfo[] goFileInfo = directory.GetFiles("*.unity", SearchOption.AllDirectories);

        var tmp = goFileInfo.Select(fi => fi.FullName).Except(AssetFilePaths);
        AssetFilePaths.AddRange(tmp);

        EditorGUI.FocusTextInControl("QuickSceneSearchField");
    }

    private Vector2 currentSceneScroll = new Vector2();
    private static string searchReg = "";
    int CurrentSelectionIndex = 0;


    public void OnGUI()
    {
        Regex tmpRegEx = new Regex(searchReg.ToLower());
        var tmpList = AssetFilePaths.Where(path => tmpRegEx.Matches(path.ToLower()).Count>0).ToList();
        CurrentSelectionIndex = Mathf.Clamp(CurrentSelectionIndex, 0, tmpList.Count -1);

        if(Event.current.type == EventType.KeyDown)
        {
            if(Event.current.keyCode == KeyCode.Escape)
            {
                this.Close();
                Event.current.Use();
            }
            if(Event.current.keyCode == KeyCode.DownArrow)
            {
                CurrentSelectionIndex++;
                Event.current.Use();
                CurrentSelectionIndex = Mathf.Clamp(CurrentSelectionIndex, 0, tmpList.Count -1);
                currentSceneScroll.y += 23f;
            }
            if(Event.current.keyCode == KeyCode.UpArrow)
            {
                CurrentSelectionIndex--;
                Event.current.Use();
                CurrentSelectionIndex = Mathf.Clamp(CurrentSelectionIndex, 0, tmpList.Count -1);
                currentSceneScroll.y -= 23f;
            }
            if(Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter)
            {
                if(tmpList.Count >0)
                {
                    ChangeScene(tmpList[CurrentSelectionIndex]);
                    Event.current.Use();
                }
            }
        }
        EditorGUILayout.BeginVertical();

        GUI.SetNextControlName("QuickSceneSearchField");
        searchReg = EditorGUILayout.TextField("Search: ", searchReg);
        currentSceneScroll = EditorGUILayout.BeginScrollView(currentSceneScroll, false, false);

        for (int i = 0; i < tmpList.Count; ++i)
        {
            GUI.color = i == CurrentSelectionIndex ? Color.white : Color.grey;
            if (GUILayout.Button(tmpList[i], GUILayout.MinHeight(20f)))
            {
                ChangeScene(tmpList[i]);
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        GUI.FocusControl("QuickSceneSearchField");
    }

    private void ChangeScene(string path)
    {
        bool swapscene = true;
        if (EditorApplication.isSceneDirty)
        {
            swapscene = EditorApplication.SaveCurrentSceneIfUserWantsTo();
        }
        if (swapscene)
        {
            EditorApplication.OpenScene(path);
            this.Close();
        }
    }
}
