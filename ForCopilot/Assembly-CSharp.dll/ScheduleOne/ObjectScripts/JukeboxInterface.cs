using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BF0 RID: 3056
	public class JukeboxInterface : MonoBehaviour
	{
		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x060051C0 RID: 20928 RVA: 0x001597F7 File Offset: 0x001579F7
		// (set) Token: 0x060051C1 RID: 20929 RVA: 0x001597FF File Offset: 0x001579FF
		public bool IsOpen { get; private set; }

		// Token: 0x060051C2 RID: 20930 RVA: 0x00159808 File Offset: 0x00157A08
		private void Awake()
		{
			this.Canvas.enabled = false;
			Jukebox jukebox = this.Jukebox;
			jukebox.onStateChanged = (Action)Delegate.Combine(jukebox.onStateChanged, new Action(this.RefreshUI));
			Jukebox jukebox2 = this.Jukebox;
			jukebox2.onStateChanged = (Action)Delegate.Combine(jukebox2.onStateChanged, new Action(this.RefreshSongEntries));
			Jukebox jukebox3 = this.Jukebox;
			jukebox3.onStateChanged = (Action)Delegate.Combine(jukebox3.onStateChanged, new Action(this.RefreshAmbientDisplay));
			this.SetupSongEntries();
			this.RefreshUI();
		}

		// Token: 0x060051C3 RID: 20931 RVA: 0x001598A2 File Offset: 0x00157AA2
		private void FixedUpdate()
		{
			this.UpdateAmbientDisplay();
		}

		// Token: 0x060051C4 RID: 20932 RVA: 0x001598AC File Offset: 0x00157AAC
		private void UpdateAmbientDisplay()
		{
			this.AmbientDisplaySongLabel.text = this.Jukebox.currentTrack.TrackName;
			float currentTrackTime = this.Jukebox.CurrentTrackTime;
			float length = this.Jukebox.currentTrack.Clip.length;
			int num = Mathf.FloorToInt(currentTrackTime / 60f);
			int num2 = Mathf.FloorToInt(currentTrackTime % 60f);
			int num3 = Mathf.FloorToInt(length / 60f);
			int num4 = Mathf.FloorToInt(length % 60f);
			this.AmbientDisplayTimeLabel.text = string.Format("{0:D2}:{1:D2} / {2:D2}:{3:D2}", new object[]
			{
				num,
				num2,
				num3,
				num4
			});
		}

		// Token: 0x060051C5 RID: 20933 RVA: 0x0015996C File Offset: 0x00157B6C
		private void SetupSongEntries()
		{
			Jukebox.Track[] trackList = this.Jukebox.TrackList;
			for (int i = 0; i < trackList.Length; i++)
			{
				Jukebox.Track track = trackList[i];
				GameObject entry = UnityEngine.Object.Instantiate<GameObject>(this.SongEntryPrefab, this.EntryContainer);
				entry.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = track.TrackName;
				entry.transform.Find("Artist").GetComponent<TextMeshProUGUI>().text = track.ArtistName;
				entry.transform.SetAsLastSibling();
				entry.transform.Find("PlayPause").GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.SongEntryClicked(entry.GetComponent<RectTransform>());
				});
				this.songEntries.Add(entry.GetComponent<RectTransform>());
			}
		}

		// Token: 0x060051C6 RID: 20934 RVA: 0x00159A64 File Offset: 0x00157C64
		public void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 2);
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x060051C7 RID: 20935 RVA: 0x00159ABB File Offset: 0x00157CBB
		private void OnDestroy()
		{
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x00159ACE File Offset: 0x00157CCE
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			action.Used = true;
			this.Close();
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x00159AF0 File Offset: 0x00157CF0
		public void Open()
		{
			this.RefreshUI();
			this.RefreshSongEntries();
			this.IsOpen = true;
			this.Canvas.enabled = true;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition.position, this.CameraPosition.rotation, 0.15f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0.15f);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			this.RefreshAmbientDisplay();
		}

		// Token: 0x060051CA RID: 20938 RVA: 0x00159BA4 File Offset: 0x00157DA4
		public void Close()
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.15f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.15f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			this.RefreshAmbientDisplay();
		}

		// Token: 0x060051CB RID: 20939 RVA: 0x00159C25 File Offset: 0x00157E25
		private void Hovered()
		{
			if (!this.IsOpen)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				this.IntObj.SetMessage("Use jukebox");
				return;
			}
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x00159C58 File Offset: 0x00157E58
		private void Interacted()
		{
			if (!this.IsOpen)
			{
				this.Open();
			}
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x00159C68 File Offset: 0x00157E68
		public void PlayPausePressed()
		{
			this.Jukebox.TogglePlay();
		}

		// Token: 0x060051CE RID: 20942 RVA: 0x00159C75 File Offset: 0x00157E75
		public void BackPressed()
		{
			this.Jukebox.Back();
		}

		// Token: 0x060051CF RID: 20943 RVA: 0x00159C82 File Offset: 0x00157E82
		public void NextPressed()
		{
			this.Jukebox.Next();
		}

		// Token: 0x060051D0 RID: 20944 RVA: 0x00159C8F File Offset: 0x00157E8F
		public void ShufflePressed()
		{
			this.Jukebox.ToggleShuffle();
		}

		// Token: 0x060051D1 RID: 20945 RVA: 0x00159C9C File Offset: 0x00157E9C
		public void RepeatPressed()
		{
			this.Jukebox.ToggleRepeatMode();
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x00159CA9 File Offset: 0x00157EA9
		public void SyncPressed()
		{
			this.Jukebox.ToggleSync();
		}

		// Token: 0x060051D3 RID: 20947 RVA: 0x00159CB8 File Offset: 0x00157EB8
		public void SongEntryClicked(RectTransform entry)
		{
			int num = this.songEntries.IndexOf(entry);
			if (this.Jukebox.currentTrack == this.Jukebox.TrackList[num])
			{
				this.Jukebox.TogglePlay();
				return;
			}
			this.Jukebox.PlayTrack(num);
		}

		// Token: 0x060051D4 RID: 20948 RVA: 0x00159D04 File Offset: 0x00157F04
		private void RefreshSongEntries()
		{
			for (int i = 0; i < this.songEntries.Count; i++)
			{
				Jukebox.Track track = this.Jukebox.TrackList[i];
				if (this.Jukebox.currentTrack == track && this.Jukebox.IsPlaying)
				{
					this.songEntries[i].Find("PlayPause/Icon").GetComponent<Image>().sprite = this.SongEntryPauseSprite;
				}
				else
				{
					this.songEntries[i].Find("PlayPause/Icon").GetComponent<Image>().sprite = this.SongEntryPlaySprite;
				}
			}
		}

		// Token: 0x060051D5 RID: 20949 RVA: 0x00159DA4 File Offset: 0x00157FA4
		private void RefreshUI()
		{
			this.PausePlayImage.sprite = (this.Jukebox.IsPlaying ? this.PauseSprite : this.PlaySprite);
			this.ShuffleButton.targetGraphic.color = (this.Jukebox.Shuffle ? this.SelectedColor : this.DeselectedColor);
			this.SyncButton.targetGraphic.color = (this.Jukebox.Sync ? this.SelectedColor : this.DeselectedColor);
			Sprite sprite = this.RepeatModeSprite_None;
			switch (this.Jukebox.RepeatMode)
			{
			case Jukebox.ERepeatMode.None:
				sprite = this.RepeatModeSprite_None;
				break;
			case Jukebox.ERepeatMode.RepeatQueue:
				sprite = this.RepeatModeSprite_Queue;
				break;
			case Jukebox.ERepeatMode.RepeatTrack:
				sprite = this.RepeatModeSprite_Track;
				break;
			}
			(this.RepeatButton.targetGraphic as Image).sprite = sprite;
			this.RepeatButton.targetGraphic.color = ((this.Jukebox.RepeatMode == Jukebox.ERepeatMode.None) ? this.DeselectedColor : this.SelectedColor);
		}

		// Token: 0x060051D6 RID: 20950 RVA: 0x00159EAE File Offset: 0x001580AE
		private void RefreshAmbientDisplay()
		{
			this.AmbientDisplayContainer.gameObject.SetActive(!this.IsOpen && this.Jukebox.IsPlaying);
			if (this.AmbientDisplayContainer.activeSelf)
			{
				this.UpdateAmbientDisplay();
			}
		}

		// Token: 0x04003D42 RID: 15682
		public const float OPEN_TIME = 0.15f;

		// Token: 0x04003D44 RID: 15684
		[Header("References")]
		public Jukebox Jukebox;

		// Token: 0x04003D45 RID: 15685
		public Canvas Canvas;

		// Token: 0x04003D46 RID: 15686
		public Transform CameraPosition;

		// Token: 0x04003D47 RID: 15687
		public InteractableObject IntObj;

		// Token: 0x04003D48 RID: 15688
		public Image PausePlayImage;

		// Token: 0x04003D49 RID: 15689
		public Button ShuffleButton;

		// Token: 0x04003D4A RID: 15690
		public Button RepeatButton;

		// Token: 0x04003D4B RID: 15691
		public Button SyncButton;

		// Token: 0x04003D4C RID: 15692
		public RectTransform EntryContainer;

		// Token: 0x04003D4D RID: 15693
		public GameObject AmbientDisplayContainer;

		// Token: 0x04003D4E RID: 15694
		public TextMeshPro AmbientDisplaySongLabel;

		// Token: 0x04003D4F RID: 15695
		public TextMeshPro AmbientDisplayTimeLabel;

		// Token: 0x04003D50 RID: 15696
		[Header("Settings")]
		public Sprite PlaySprite;

		// Token: 0x04003D51 RID: 15697
		public Sprite PauseSprite;

		// Token: 0x04003D52 RID: 15698
		public Sprite SongEntryPlaySprite;

		// Token: 0x04003D53 RID: 15699
		public Sprite SongEntryPauseSprite;

		// Token: 0x04003D54 RID: 15700
		public Sprite RepeatModeSprite_None;

		// Token: 0x04003D55 RID: 15701
		public Sprite RepeatModeSprite_Track;

		// Token: 0x04003D56 RID: 15702
		public Sprite RepeatModeSprite_Queue;

		// Token: 0x04003D57 RID: 15703
		public Color DeselectedColor;

		// Token: 0x04003D58 RID: 15704
		public Color SelectedColor;

		// Token: 0x04003D59 RID: 15705
		public GameObject SongEntryPrefab;

		// Token: 0x04003D5A RID: 15706
		private List<RectTransform> songEntries = new List<RectTransform>();
	}
}
