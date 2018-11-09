using Newtonsoft.Json;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SudokuLinq;

namespace mySudoku {
    public class Game : MonoBehaviour {

        public TextAsset input;

        Grids grids;
        class Grids {
            public List<List<int>> Grid_1 { get; set; }
            public List<List<int>> Grid_2 { get; set; }
            public List<List<int>> Grid_3 { get; set; }
        }

        List<List<int>> current;

        public static Game Instance { get; private set; }
        void Awake() {
            Instance = this;
            ImportBoard();            
        }

        void LoadGame(List<List<int>> game) {
            current = game;

            var valueIt = game
                .SelectMany(x => x)
                .GetEnumerator();

            var rowIt = Board.Instance.transf
                .GetComponentsInChildren<Cell>(false).GetEnumerator();

            while (rowIt.MoveNext() && valueIt.MoveNext())
                ((Cell)rowIt.Current).InitValue(valueIt.Current);

        }

        void ImportBoard() {
            grids = JsonConvert.DeserializeObject<Grids>(input.ToString());
            current = grids.Grid_1;
        }

        public void ExportBoard() {

            var export = Board.Instance.transf.GetComponentsInChildren<Cell>(false)
                .Select((x, i) => new { Index = i, x.Value })
                .GroupBy(x => x.Index / 9)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

            var jsonString = JsonConvert.SerializeObject(export, Formatting.None);

            StreamWriter writer = new StreamWriter("output.json", false);
            writer.Write(jsonString);
            writer.Close();
        }

        public void LoadGame1() {
            LoadGame(grids.Grid_1);
        }

        public void LoadGame2() {
            LoadGame(grids.Grid_2);
        }

        public void LoadGame3() {
            LoadGame(grids.Grid_3);
        }

        public void Reset() {
            LoadGame(current);
        }

        public void Solve() {

            var p = new SudokuPuzzle(Board.Instance.cells.ToArray());
            p = p.Solve();
            Board.Instance.cells = p.Cells.Select(c => c[0]);

        }
    }
}