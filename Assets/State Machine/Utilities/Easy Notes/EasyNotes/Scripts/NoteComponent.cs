using System.Collections.Generic;
using UnityEngine;

public class NoteComponent : MonoBehaviour
{
    public List<Note> notes = new List<Note>();

    [System.Serializable]
    public class Note 
    {
        public Color color = new Color(1, 1, 1, 1);
        public string text, header;
    }
}
