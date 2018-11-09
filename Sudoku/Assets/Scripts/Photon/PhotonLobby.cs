using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mySudoku {
    public class PhotonLobby : MonoBehaviourPunCallbacks {

        public GameObject StartButton;
        public GameObject CancelButton;
        public GameObject Landing;

        void Start() {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster() {
            print("Player has connected to Master server.");
            StartButton.SetActive(true);
        }

        public void OnStartButtonClick() {
            print("Start button was clicked.");
            StartButton.SetActive(false);
            CancelButton.SetActive(true);
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message) {
            print("Tried to join a random game but failed. There are no open games available.");
            CreateRoom();
        }

        void CreateRoom() {
            print("Trying to create a new room.");
            int randName = Random.Range(0, int.MaxValue);
            RoomOptions roomOptions = new RoomOptions() { IsVisible=true, IsOpen=true, MaxPlayers=2 };
            PhotonNetwork.CreateRoom("Room" + randName, roomOptions);
        }

        public override void OnJoinedRoom() {
            print("Just joined a room.");

            if (PhotonNetwork.CountOfPlayersInRooms==0)
                Game.Instance.LoadGame1();

            Landing.SetActive(false);
            Game.Instance.ExportBoard();
        }

        public override void OnCreateRoomFailed(short returnCode, string message) {
            print("Tried to create a new room but failed. The room with the same name probably exists already.");
            CreateRoom();
        }


        public void OnCancelButtonClick() {
            print("Cancel button was clicked.");
            CancelButton.SetActive(false);
            StartButton.SetActive(true);
            PhotonNetwork.LeaveRoom();
        }

    }
}