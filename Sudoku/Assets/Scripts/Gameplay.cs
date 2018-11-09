using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mySudoku {
    public class Gameplay : MonoBehaviour {

        public KeyCode[] Alpha;
        public KeyCode[] Keypad;

        int GetInput() {
            for (int i = 0; i<Alpha.Length; i++)
                if (Input.GetKeyDown(Alpha[i]))
                    return i;

            for (int i = 0; i<Keypad.Length; i++)
                if (Input.GetKeyDown(Keypad[i]))
                    return i;

            if (Input.GetKeyDown(KeyCode.Delete) ||
                Input.GetKeyDown(KeyCode.Backspace))
                return 0;

            return -1;
        }

        void Update() {

            int input = GetInput();
            if (Selection.SelectedCell && input!=-1) 
                Move(input);
            
        }

        void Move(int input) {
            Selection.SelectedCell.SetValue(input);
            Board.Instance.CheckValidity();
        }

    }
}