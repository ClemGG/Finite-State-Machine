using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(Note))]
public class NoteEditor : Editor
{
    private Note note;

    private Color noteTextColor = new Color(.85f, .85f, .85f, 1f);
    private Vector2 scrollPos;
    private Vector2 scrollRectSize = new Vector2(200f, 100f);

    private void Init()
    {
        note = target as Note;
    }

    public override void OnInspectorGUI()
    {
        Init();

        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(scrollRectSize.y));
        GUIStyle myStyle = new GUIStyle();
        myStyle.wordWrap = true;
        myStyle.stretchWidth = false;
        myStyle.normal.textColor = noteTextColor;


        //Spécifie la taille du bloc de texte dans la scrollView, pas la vue en elle-même
        note.text = GUILayout.TextArea(note.text, myStyle, GUILayout.Width(scrollRectSize.x), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.EndScrollView();
        Repaint();
    }
}