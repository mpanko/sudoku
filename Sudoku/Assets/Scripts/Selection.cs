using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace mySudoku {
    public class Selection : MonoBehaviourPun {

        public static Cell SelectedCell { get; private set; } = null;

        public IEnumerable<Cell> ErrorCells {
            get {
                return Board.Instance.transf.GetComponentsInChildren<Cell>()
                    .Where(f => f.Error==true);
            }
        }

        public static Selection Instance { get; private set; }
        void Awake() {
            Instance = this;
        }
        
        void NewCellSelected() {

            SelectedCell = EventSystem.current.currentSelectedGameObject.GetComponent<Cell>();

            foreach (Cell cell in Board.Instance.transf.GetComponentsInChildren<Cell>())
                if (!cell.Error)
                    cell.SetRegularColor();

            foreach (Cell cell in Board.Instance.GetConstrained(SelectedCell))
                if (!cell.Error)
                    cell.SetHighlightedColor();
        }


        void Update() {

            if (!EventSystem.current.currentSelectedGameObject) 
                EventSystem.current.SetSelectedGameObject(SelectedCell.gameObject);
            

            if (EventSystem.current.currentSelectedGameObject &&
                SelectedCell != EventSystem.current.currentSelectedGameObject.GetComponent<Cell>())
                NewCellSelected();

        }

    }
}