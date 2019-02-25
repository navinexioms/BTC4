using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VoxelBusters;
using VoxelBusters.NativePlugins;
using SimpleJSON;
using UnityEngine.Networking;

namespace Photon.Pun.UtilityScripts
{
	public class TPPWFConnectionManager : MonoBehaviourPunCallbacks {
		public static bool isMaster,isRemote,JoinedRoomFlag;
		private GameObject QuitPanel;
		public Text RoomName,WarningText;
		public static int PlayMode = 0;
		public bool isSelectedGameType;
		public int PlayerLength=0;
		void Awake()
		{
			DontDestroyOnLoad (this);
		}

		public void CreateOrJoinRoomMethod()
		{
			if (Application.internetReachability == NetworkReachability.NotReachable) {
				print ("no Internet connection is there");
				StartCoroutine (RoomNameWarning ("PLEASE CONNECT TO INTERNET"));
			} else if (!isSelectedGameType) {
				StartCoroutine (RoomNameWarning ("PLEASE SELECT THE GAMETYPE"));
			}else if (RoomName.text.Length > 0 && (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)) {
				if (PhotonNetwork.AuthValues == null) {
					PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues ();
				}
				string PlayerName = PlayerPrefs.GetString ("userid");
				PhotonNetwork.AuthValues.UserId = PlayerName;
				PhotonNetwork.LocalPlayer.NickName = PlayerName;
				PhotonNetwork.ConnectUsingSettings ();
			} else if (RoomName.text.Length == 0 && (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork))  {
				StartCoroutine (RoomNameWarning("PLEASE ENTER THE ROOM NAME"));
			}
		}

		public IEnumerator RoomNameWarning(string warn)
		{
			WarningText.text = warn;
			yield return new WaitForSeconds (1);
			WarningText.text = null;
		}

		private void FinishSharing(eShareResult _result)
		{
			print (_result);
		}

		public override void OnConnectedToMaster()
		{
			print ("Conneced to master server:");
			PhotonNetwork.JoinLobby ();
		}
		public override void OnJoinedLobby()
		{
			print ("Joined lobby");
			if (PlayerLength == 2) {
				PhotonNetwork.JoinOrCreateRoom (RoomName.text, new Photon.Realtime.RoomOptions {
					MaxPlayers = 2,
					PlayerTtl = 300000,
					EmptyRoomTtl = 10000
				}, null);
			} else if (PlayerLength == 4) {
				PhotonNetwork.JoinOrCreateRoom (RoomName.text, new Photon.Realtime.RoomOptions {
					MaxPlayers = 4,
					PlayerTtl = 300000,
					EmptyRoomTtl = 10000
				}, null);
			}
		}
		public override void OnCreatedRoom()
		{
			print ("Room Created Successfully");
			ShareSheet shareSheet = new ShareSheet ();
			shareSheet.Text = RoomName.text;
			NPBinding.Sharing.ShowView (shareSheet, FinishSharing);
			isMaster = true;
		}
		public override void OnCreateRoomFailed(short msg,string msg1)
		{
			print(msg1);

			PhotonNetwork.JoinRoom ("nsd");
		}
		public override void OnJoinedRoom()
		{
			print (PhotonNetwork.MasterClient.NickName);
			print ("Room Joined successfully");
			if (PhotonNetwork.PlayerList.Length == 2) 
			{
				isRemote = true;
			}
			if (!JoinedRoomFlag) {
//				SceneManager.LoadScene ("OneOnOneGameBoard");
				PlayMode=1;
				SceneManager.LoadScene("BettingAmountFor2PlayerPlayWithFriends");
			}
			JoinedRoomFlag = true;
		}
		void OnApplicationQuit()
		{
			if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) 
			{
				print ("Quit the Application When internet connection is There and Logged out");
				StartCoroutine (HitUrl11 (0));
				PlayerPrefs.SetString ("userid", null);
			}
			else if(Application.internetReachability == NetworkReachability.NotReachable)
			{
				print ("no Internet connection is there");
			}
		}
		IEnumerator HitUrl11(int status)
		{
			print ("HitUrl11");
			string id= PlayerPrefs.GetString ("userid");
			UnityWebRequest request11 =new UnityWebRequest("http://apienjoybtc.exioms.me/api/Home/login_session?userid="+id+"&gamesessionid="+status);

			request11.chunkedTransfer = false;

			request11.downloadHandler = new DownloadHandlerBuffer ();

			yield return request11.SendWebRequest ();

			if (request11.error != null) {

			} else {
				print (request11.downloadHandler.text);
			}
		}
	}
}
