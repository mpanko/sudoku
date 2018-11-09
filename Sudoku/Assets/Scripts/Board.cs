using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace mySudoku {
    public class Board : MonoBehaviour {

        public Transform transf;
        public static Board Instance { get; private set; }
        void Awake() {
            Instance = this;
        }
        void Start() {
            Init();
        }

        Cell[][] row = new Cell[9][];
        Cell[][] column = new Cell[9][];

        public IEnumerable<int> cells {
            get {
                return row
                    .SelectMany(x => x)
                    .Select(p => p.Value);
            }
            set {
                var rowIt = row
                    .SelectMany(x => x)
                    .GetEnumerator();

                var valueIt = value.GetEnumerator();

                while (rowIt.MoveNext() && valueIt.MoveNext())
                    rowIt.Current.SetValue(valueIt.Current);
            }
        }

        public IEnumerable<Cell> GetConstrained(Cell active) {

            IEnumerable<Cell> box = transf.GetChild(active.GlobalIndex)
                .GetComponentsInChildren<Cell>();

            return box
                .Concat(column[active.Col])
                .Concat(row[active.Row]);
        }


        void Init() {

            Transform Board = GameObject.FindGameObjectWithTag("Board").transform;

            for (int i = 0; i<9; i++) {
                row[i] = new Cell[9];
                column[i] = new Cell[9];

                for (int j = 0; j<9; j++) {

                    int k = (j/3) + (i/3)*3;
                    int l = (j%3) + (i%3)*3;
                    int m = (i/3) + (j/3)*3;
                    int n = (i%3) + (j%3)*3;

                    row[i][j]=Board.GetChild(k).GetChild(l).GetComponent<Cell>();
                    column[i][j]=Board.GetChild(m).GetChild(n).GetComponent<Cell>();
                }

            }

        }

        IEnumerable<Cell> GetInvalid(IEnumerable<Cell> cells) {

            var InvalidCells = cells
                .Where(f => f.Value != 0)
                .Select((v, i) => new { Index = i, cell = v })
                .GroupBy(g => g.cell.Value)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g, (g, x) => x.cell);

            return InvalidCells;
        }

        public void CheckValidity() {

            List<Cell> invalid = new List<Cell>();

            foreach (Cell[] r in row)
                invalid.AddRange(GetInvalid(r));

            foreach (Cell[] c in column)
                invalid.AddRange(GetInvalid(c));

            foreach (Transform b in transf)
                invalid.AddRange(GetInvalid(b.GetComponentsInChildren<Cell>(false)));


            foreach (GameObject Cell in GameObject.FindGameObjectsWithTag("Cell"))
                Cell.GetComponent<Cell>().SetError(false);

            foreach (Cell Cell in invalid)
                Cell.SetError(true);


        }
    }
}
