using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Networking;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Multiplayer
{
	// Token: 0x02000B1D RID: 2845
	public class LobbyInterface : PersistentSingleton<LobbyInterface>
	{
		// Token: 0x06004C14 RID: 19476 RVA: 0x00140168 File Offset: 0x0013E368
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<LobbyInterface>.Instance == null || Singleton<LobbyInterface>.Instance != this)
			{
				return;
			}
			this.InviteButton.onClick.AddListener(new UnityAction(this.InviteClicked));
			this.LeaveButton.onClick.AddListener(new UnityAction(this.LeaveClicked));
			Lobby lobby = this.Lobby;
			lobby.onLobbyChange = (Action)Delegate.Combine(lobby.onLobbyChange, new Action(delegate()
			{
				this.UpdateButtons();
				this.UpdatePlayers();
				this.LobbyTitle.text = string.Concat(new string[]
				{
					"Lobby (",
					this.Lobby.PlayerCount.ToString(),
					"/",
					4.ToString(),
					")"
				});
			}));
		}

		// Token: 0x06004C15 RID: 19477 RVA: 0x001401F8 File Offset: 0x0013E3F8
		protected override void Start()
		{
			base.Start();
			if (Singleton<LobbyInterface>.Instance == null || Singleton<LobbyInterface>.Instance != this)
			{
				return;
			}
			this.UpdateButtons();
			this.UpdatePlayers();
			if (PlayerPrefs.GetInt("InviteHintShown", 0) == 0)
			{
				this.InviteHint.SetActive(true);
				return;
			}
			this.InviteHint.SetActive(false);
		}

		// Token: 0x06004C16 RID: 19478 RVA: 0x00140258 File Offset: 0x0013E458
		private void LateUpdate()
		{
			if (Singleton<PauseMenu>.InstanceExists)
			{
				this.Canvas.enabled = (Singleton<PauseMenu>.Instance.IsPaused && this.Lobby.IsInLobby && !GameManager.IS_TUTORIAL);
				if (this.Canvas.enabled)
				{
					this.LeaveButton.gameObject.SetActive(false);
					return;
				}
			}
			else
			{
				this.Canvas.enabled = true;
				this.LeaveButton.gameObject.SetActive(!this.Lobby.IsHost);
			}
		}

		// Token: 0x06004C17 RID: 19479 RVA: 0x001402E4 File Offset: 0x0013E4E4
		public void SetVisible(bool visible)
		{
			this.Canvas.enabled = visible;
		}

		// Token: 0x06004C18 RID: 19480 RVA: 0x001402F2 File Offset: 0x0013E4F2
		public void LeaveClicked()
		{
			this.Lobby.LeaveLobby();
		}

		// Token: 0x06004C19 RID: 19481 RVA: 0x001402FF File Offset: 0x0013E4FF
		public void InviteClicked()
		{
			PlayerPrefs.SetInt("InviteHintShown", 1);
			this.InviteHint.SetActive(false);
			this.Lobby.TryOpenInviteInterface();
		}

		// Token: 0x06004C1A RID: 19482 RVA: 0x00140324 File Offset: 0x0013E524
		private void UpdateButtons()
		{
			this.InviteButton.gameObject.SetActive(this.Lobby.IsHost && this.Lobby.PlayerCount < 4);
			this.LeaveButton.gameObject.SetActive(!this.Lobby.IsHost);
		}

		// Token: 0x06004C1B RID: 19483 RVA: 0x00140380 File Offset: 0x0013E580
		private void UpdatePlayers()
		{
			if (this.Lobby.IsInLobby)
			{
				for (int i = 0; i < this.PlayerSlots.Length; i++)
				{
					if (this.Lobby.Players[i] != CSteamID.Nil)
					{
						this.SetPlayer(i, this.Lobby.Players[i]);
					}
					else
					{
						this.ClearPlayer(i);
					}
				}
				return;
			}
			this.SetPlayer(0, this.Lobby.LocalPlayerID);
			for (int j = 1; j < this.PlayerSlots.Length; j++)
			{
				this.ClearPlayer(j);
			}
		}

		// Token: 0x06004C1C RID: 19484 RVA: 0x00140418 File Offset: 0x0013E618
		public void SetPlayer(int index, CSteamID player)
		{
			this.Lobby.Players[index] = player;
			this.PlayerSlots[index].Find("Frame/Avatar").GetComponent<RawImage>().texture = this.GetAvatar(player);
			this.PlayerSlots[index].gameObject.SetActive(true);
		}

		// Token: 0x06004C1D RID: 19485 RVA: 0x0014046D File Offset: 0x0013E66D
		public void ClearPlayer(int index)
		{
			this.Lobby.Players[index] = CSteamID.Nil;
			this.PlayerSlots[index].gameObject.SetActive(false);
		}

		// Token: 0x06004C1E RID: 19486 RVA: 0x00140498 File Offset: 0x0013E698
		private Texture2D GetAvatar(CSteamID user)
		{
			if (!SteamManager.Initialized)
			{
				Debug.LogWarning("Steamworks not initialized");
				return new Texture2D(0, 0);
			}
			int mediumFriendAvatar = SteamFriends.GetMediumFriendAvatar(user);
			uint num;
			uint num2;
			if (SteamUtils.GetImageSize(mediumFriendAvatar, ref num, ref num2) && num > 0U && num2 > 0U)
			{
				byte[] array = new byte[num * num2 * 4U];
				Texture2D texture2D = new Texture2D((int)num, (int)num2, TextureFormat.RGBA32, false, false);
				if (SteamUtils.GetImageRGBA(mediumFriendAvatar, array, (int)(num * num2 * 4U)))
				{
					texture2D.LoadRawTextureData(array);
					texture2D.Apply();
				}
				return texture2D;
			}
			Debug.LogWarning("Couldn't get avatar.");
			return new Texture2D(0, 0);
		}

		// Token: 0x040038AD RID: 14509
		[Header("References")]
		public Lobby Lobby;

		// Token: 0x040038AE RID: 14510
		public Canvas Canvas;

		// Token: 0x040038AF RID: 14511
		public TextMeshProUGUI LobbyTitle;

		// Token: 0x040038B0 RID: 14512
		public RectTransform[] PlayerSlots;

		// Token: 0x040038B1 RID: 14513
		public Button InviteButton;

		// Token: 0x040038B2 RID: 14514
		public Button LeaveButton;

		// Token: 0x040038B3 RID: 14515
		public GameObject InviteHint;
	}
}
