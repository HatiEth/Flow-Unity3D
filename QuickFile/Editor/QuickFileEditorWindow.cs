using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Object = UnityEngine.Object;

public class QuickFileEditorWindow : EditorWindow
{
    [MenuItem("Window/QuickFile %#e")]
    public static void OpenEditorWindow()
    {
        var w = EditorWindow.GetWindow<QuickFileEditorWindow>();
        w.titleContent = new GUIContent("QuickScene");
        w.Show();
    }

    List<string> FilePaths = new List<string>();
    // static List<string> Recent = new List<string>(); // NOT YET IMPLEMENTED

    private Vector2 currentSceneScroll = new Vector2();
    private static string searchReg = "";
    int CurrentSelectionIndex = 0;

    public void OnEnable()
    {
        DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath);

        FileInfo[] files = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);
        Debug.Log(files.Length);
        Regex metaRegex = new Regex(".meta");
        Regex gitRegex = new Regex(".git");

        var allAssetsInDB = AssetDatabase.GetAllAssetPaths();
        //var tmp = files.Select(fi => fi.FullName).Where(name => metaRegex.Matches(name).Count == 0).Where(name => gitRegex.Matches(name).Count ==0 ).Except(Recent);
        var lscenes = files.Where(fi => fi.FullName.Contains("Default.unity")).ToList();

        Debug.Log(lscenes.Count());
        for(int i=0;i<lscenes.Count();++i)
        {
            Debug.Log(lscenes[i].FullName);
        }

        var tmp = allAssetsInDB.Where(s => {
            var t = s.Replace('/', Path.DirectorySeparatorChar);
            return files.Any(fi => fi.FullName.EndsWith(t));
        });
        FilePaths.AddRange(tmp);
    }

    public void OnGUI()
    {
        Regex tmpRegEx;
        try {
            tmpRegEx = new Regex(searchReg.ToLower());

        }
        catch(ArgumentException)
        {
            tmpRegEx = new Regex("");
        }
        var tmpList = FilePaths.Where(path => tmpRegEx.Matches(path.ToLower()).Count > 0).ToList();
        CurrentSelectionIndex = Mathf.Clamp(CurrentSelectionIndex, 0, tmpList.Count - 1);

        if (Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode == KeyCode.Escape)
            {
                this.Close();
                Event.current.Use();
            }
            if (Event.current.keyCode == KeyCode.DownArrow)
            {
                CurrentSelectionIndex++;
                Event.current.Use();
                CurrentSelectionIndex = Mathf.Clamp(CurrentSelectionIndex, 0, tmpList.Count - 1);
                currentSceneScroll.y += 23f;
            }
            if (Event.current.keyCode == KeyCode.UpArrow)
            {
                CurrentSelectionIndex--;
                Event.current.Use();
                CurrentSelectionIndex = Mathf.Clamp(CurrentSelectionIndex, 0, tmpList.Count - 1);
                currentSceneScroll.y -= 23f;
            }
            if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter)
            {
                if (tmpList.Count > 0)
                {
                    OnSelect(tmpList[CurrentSelectionIndex]);
                    Event.current.Use();
                }
            }
        }
        EditorGUILayout.BeginVertical();

        GUI.SetNextControlName("QuickFileSearchField");
        searchReg = EditorGUILayout.TextField("Search: ", searchReg);
        currentSceneScroll = EditorGUILayout.BeginScrollView(currentSceneScroll, false, false);
        for (int i = 0; i < tmpList.Count; ++i)
        {
            GUI.color = i == CurrentSelectionIndex ? Color.white : Color.grey;
            if (GUILayout.Button(tmpList[i], GUILayout.MinHeight(20f)))
            {
                OnSelect(tmpList[CurrentSelectionIndex]);
            }
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();

        GUI.FocusControl("QuickFileSearchField");
    }

    public void OnSelect(string path)
    {
        bool IsShift = ((Event.current.modifiers & EventModifiers.Shift) == EventModifiers.Shift);
        if(IsShift)
        {
            OpenFile(path);
        }
        else
        {
            SelectFile(path);
        }
        this.Close();
    }

    public void SelectFile(string path)
    {
        Object o = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
        Selection.activeObject = o;
    }

    public void OpenFile(string path)
    {
        Object o = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
        AssetDatabase.OpenAsset(o);
    }
} 
