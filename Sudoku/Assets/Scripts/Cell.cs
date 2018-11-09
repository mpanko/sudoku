using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace mySudoku {
    public class Cell : MonoBehaviourPun {

        public int Value { get; private set; }
        public bool Const { get; private set; }
        public bool Error { get; private set; }

        public int LocalIndex {
            get { return transform.GetSiblingIndex(); }
        }

        public int GlobalIndex {
            get { return transform.parent.GetSiblingIndex(); }
        }

        public int Row {
            get { return LocalIndex/3 + (GlobalIndex/3)*3; }
        }

        public int Col {
            get { return LocalIndex%3 + (GlobalIndex%3)*3; }
        }


        public void InitValue(int val) {
            photonView.RPC("InitValue_RPC", RpcTarget.AllBuffered, val);
        }

        public void SetValue(int val) {
            photonView.RPC("SetValue_RPC", RpcTarget.AllBuffered, val);
        }

        [PunRPC]
        void InitValue_RPC(int val) {            
            Value=val;
            SetError(false);
            if (val==0) {
                GetComponentInChildren<Text>().text = "";
                GetComponentInChildren<Text>().color = Colors.Instance.PlayableFont;
                Const=false;
            }
            else {
                GetComponentInChildren<Text>().text = val.ToString();
                GetComponentInChildren<Text>().color = Colors.Instance.ConstFont;
                Const=true;
            }
        }

        [PunRPC]
        void SetValue_RPC(int val) {
            if (!Const) {
                Value=val;

                if (val==0)
                    GetComponentInChildren<Text>().text = "";
                else
                    GetComponentInChildren<Text>().text = val.ToString();                
            }
        }

        

        public void SetErrorColor() {
            GetComponent<Image>().color=Colors.Instance.ErrorCell;
        }

        public void SetHighlightedColor() {
            GetComponent<Image>().color=Colors.Instance.HighlightedCell;
        }

        public void SetRegularColor() {
            GetComponent<Image>().color=Colors.Instance.RegularCell;
        }


        public void SetError(bool value) {
            photonView.RPC("SetError_RPC", RpcTarget.AllBuffered, value);
        }

        [PunRPC]
        public void SetError_RPC(bool value) {
            Error=value;
            if (value)
                SetErrorColor();
            else
                SetRegularColor();
        }

    }
}