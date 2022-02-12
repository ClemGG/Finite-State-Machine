using UnityEditor;
using UnityEngine;

public class ObjectNotesWindow : EditorWindow
{
    private protected string _searchName, _searchText, _searchObject;
    private protected Vector2 _scrollPos;

    [MenuItem("Tools/EasyNotes/View All Notes")]
    public static void ViewObjectNotesWindow()
    {
        EditorWindow.GetWindow(typeof(ObjectNotesWindow), false, "Easy Notes: All Notes");   
    }

    public void OnGUI()
    {
        NoteComponent[] noteComponents = GameObject.FindObjectsOfType<NoteComponent>();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Search by Note Name: ");
        _searchName = EditorGUILayout.TextField(_searchName);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Search by Note Text: ");
        _searchText = EditorGUILayout.TextField(_searchText);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Search by Object Name: ");
        _searchObject = EditorGUILayout.TextField(_searchObject);

        EditorGUILayout.EndHorizontal();

        if(noteComponents.Length == 0)
        {
            EditorGUILayout.Space(15f);

            EditorGUILayout.LabelField("<b><color=white>You don't have any notes :(</color></b>", new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 20,
                richText = true
            });

            return;
        }

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

        foreach(var noteComponent in noteComponents)
        {
            for(int i = 0; i < noteComponent.notes.Count; i++)
            {
                var savedNote = noteComponent.notes[i];

                if(!string.IsNullOrEmpty(_searchName))
                {
                    if(string.IsNullOrEmpty(savedNote.header)) continue;

                    if(!savedNote.header.ToLower().Contains(_searchName.ToLower())) continue;
                }

                if(!string.IsNullOrEmpty(_searchText))
                {
                    if(string.IsNullOrEmpty(savedNote.text)) continue;

                    if(!savedNote.text.ToLower().Contains(_searchText.ToLower())) continue;
                }

                if(!string.IsNullOrEmpty(_searchObject))
                {
                    if(!noteComponent.name.ToLower().Contains(_searchObject.ToLower())) continue;
                }

                if(!string.IsNullOrEmpty(savedNote.header)) savedNote.header = EditorGUILayout.TextField(savedNote.header);
                else savedNote.header = EditorGUILayout.TextField($"Note {i + 1}");

                GUI.color = savedNote.color;

                savedNote.text = EditorGUILayout.TextArea(savedNote.text, GUILayout.Height(50f));

                GUI.color = new Color(1, 1, 1, 1);

                EditorGUILayout.BeginHorizontal();

                savedNote.color = EditorGUILayout.ColorField(savedNote.color);

                if(GUILayout.Button("Remove"))
                {
                    noteComponent.notes.RemoveAt(i);
                }

                if(GUILayout.Button("Find in Scene"))
                {
                    Selection.activeGameObject = noteComponent.gameObject;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(3f);
            }
        }

        EditorGUILayout.EndScrollView();
    }
}
