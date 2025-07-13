using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.UI.Compass;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.TV
{
	// Token: 0x020002B5 RID: 693
	public class TVInterface : MonoBehaviour
	{
		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x000405B4 File Offset: 0x0003E7B4
		// (set) Token: 0x06000E86 RID: 3718 RVA: 0x000405BC File Offset: 0x0003E7BC
		public bool IsOpen { get; private set; }

		// Token: 0x06000E87 RID: 3719 RVA: 0x000405C5 File Offset: 0x0003E7C5
		public void Awake()
		{
			this.Canvas.enabled = false;
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x000405F9 File Offset: 0x0003E7F9
		public void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 2);
			this.MinPass();
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00040613 File Offset: 0x0003E813
		private void OnDestroy()
		{
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00040644 File Offset: 0x0003E844
		private void MinPass()
		{
			this.TimeLabel.text = TimeManager.Get12HourTime((float)NetworkSingleton<TimeManager>.Instance.CurrentTime, true);
			this.Daylabel.text = NetworkSingleton<TimeManager>.Instance.CurrentDay.ToString();
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00040690 File Offset: 0x0003E890
		public void Open()
		{
			if (this.IsOpen)
			{
				return;
			}
			this.IsOpen = true;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition.position, this.CameraPosition.rotation, 0.15f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0.15f);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			Singleton<HUD>.Instance.SetCrosshairVisible(false);
			Singleton<CompassManager>.Instance.SetVisible(false);
			this.AddPlayer(Player.Local);
			this.Canvas.enabled = true;
			this.TimeLabel.gameObject.SetActive(false);
			this.HomeScreen.Open();
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x0004076C File Offset: 0x0003E96C
		public void Close()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.IsOpen = false;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.15f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.15f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			this.RemovePlayer(Player.Local);
			this.Canvas.enabled = false;
			this.TimeLabel.gameObject.SetActive(true);
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			Singleton<CompassManager>.Instance.SetVisible(true);
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00040817 File Offset: 0x0003EA17
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

		// Token: 0x06000E8E RID: 3726 RVA: 0x00040838 File Offset: 0x0003EA38
		public bool CanOpen()
		{
			return !this.IsOpen;
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00040843 File Offset: 0x0003EA43
		public void AddPlayer(Player player)
		{
			if (!this.Players.Contains(player))
			{
				this.Players.Add(player);
				if (this.onPlayerAdded != null)
				{
					this.onPlayerAdded.Invoke(player);
				}
			}
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00040873 File Offset: 0x0003EA73
		public void RemovePlayer(Player player)
		{
			if (this.Players.Contains(player))
			{
				this.Players.Remove(player);
				if (this.onPlayerRemoved != null)
				{
					this.onPlayerRemoved.Invoke(player);
				}
			}
		}

		// Token: 0x04000F04 RID: 3844
		public const float OPEN_TIME = 0.15f;

		// Token: 0x04000F05 RID: 3845
		public const float FOV = 60f;

		// Token: 0x04000F07 RID: 3847
		public List<Player> Players = new List<Player>();

		// Token: 0x04000F08 RID: 3848
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04000F09 RID: 3849
		public Transform CameraPosition;

		// Token: 0x04000F0A RID: 3850
		public TVHomeScreen HomeScreen;

		// Token: 0x04000F0B RID: 3851
		public TextMeshPro TimeLabel;

		// Token: 0x04000F0C RID: 3852
		public TextMeshPro Daylabel;

		// Token: 0x04000F0D RID: 3853
		public UnityEvent<Player> onPlayerAdded = new UnityEvent<Player>();

		// Token: 0x04000F0E RID: 3854
		public UnityEvent<Player> onPlayerRemoved = new UnityEvent<Player>();
	}
}
