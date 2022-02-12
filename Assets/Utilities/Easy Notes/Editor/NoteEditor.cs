using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NoteComponent))]
public class NoteEditor : Editor
{

    #region Menu Items

    [MenuItem("Tools/EasyNotes/View All Notes")]
    public static void ViewObjectNotesWindow()
    {
        EditorWindow.GetWindow(typeof(ObjectNotesWindow));   
    }

    [MenuItem("GameObject/Create Other/Easy Note")]
    public static void AddNoteComponent()
    {
        if(!Selection.activeGameObject) 
        {
            Debug.LogError("No GameObjects selected!");

            return;
        }

        if(Selection.activeGameObject.GetComponent<NoteComponent>() != null)
        {
            Debug.LogError("GameObject already contains a Note Component!");

            return;
        }

        Selection.activeGameObject.AddComponent<NoteComponent>();

        Debug.Log("Successfully added the Note Component to selected GameObject!");
    }

    #endregion


    public override void OnInspectorGUI()
    {
        NoteComponent noteComponent = target as NoteComponent;

        for(int i = 0; i < noteComponent.notes.Count; i++)
        {
            var savedNote = noteComponent.notes[i];

            if(!string.IsNullOrEmpty(savedNote.header)) 
                savedNote.header = EditorGUILayout.TextField(savedNote.header);
            else 
                savedNote.header = EditorGUILayout.TextField($"Note {i + 1}");

            GUI.color = savedNote.color;
            GUIStyle style = new(GUI.skin.textArea);
            style.wordWrap = true;

            savedNote.text = EditorGUILayout.TextArea(savedNote.text, style, GUILayout.Height(50f), GUILayout.ExpandHeight(true));

            GUI.color = new Color(1, 1, 1, 1);
            style.wordWrap = false;

            EditorGUILayout.BeginHorizontal();

            savedNote.color = EditorGUILayout.ColorField(savedNote.color);

            if(GUILayout.Button("Remove"))
            {
                noteComponent.notes.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(3f);
        }

        if(noteComponent.notes.Count != 0) EditorGUILayout.Space(10f);

        if(GUILayout.Button("Add New Note"))
        {
            noteComponent.notes.Add(new NoteComponent.Note());
        }
    }
}
