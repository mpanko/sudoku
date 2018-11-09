using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mySudoku {
    public class Colors : MonoBehaviour {

        public Color ConstFont;
        public Color PlayableFont;
        public Color ErrorFont;
        public Color HighlightedCell;
        public Color RegularCell;
        public Color SelectedCell;
        public Color ErrorCell;

        public static Colors Instance { get; private set; }
        void Awake() {
            Instance = this;
        }


    }
}