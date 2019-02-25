using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
namespace Photon.Pun.UtilityScripts
{
	public class PlayWithFriendSceneManager : Photon.Pun.MonoBehaviourPun
	{

		public Toggle TwoPlayerToggle, FourPlayerToggle;
		public GameObject TwoPlayerGameObject, FourPlayerGameObject;
		public TPPWFConnectionManager TPPWFconnectionManager;
		public void SelectTwoPlayerGamePlay()
		{
			TPPWFconnectionManager.PlayerLength = 2;
			TPPWFconnectionManager.isSelectedGameType = true;
			TwoPlayerGameObject.SetActive (TwoPlayerToggle.isOn);
			FourPlayerGameObject.SetActive (false);
		}
		public void SelectFourPlayerGamePlay()
		{
			TPPWFconnectionManager.PlayerLength = 4;
			TPPWFconnectionManager.isSelectedGameType = true;
			TwoPlayerGameObject.SetActive (false);
			FourPlayerGameObject.SetActive (FourPlayerToggle.isOn);
		}
		public void TakeRoomNameForTwoPlayers()
		{
			
		}
		public void TakeRoomNameForFourPlayers()
		{
			
		}

		//==============Method to Invite friend and Create Room======================//
		public void InviteFriend()
		{
			
		}

		//=============Method to Join Game=================//
		public void JoinGame()
		{
			
		}
		// Use this for initialization
		void Start () 
		{
		
		}
	}
}
