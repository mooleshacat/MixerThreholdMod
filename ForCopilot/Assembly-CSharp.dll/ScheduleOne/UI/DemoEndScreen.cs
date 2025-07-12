using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using Steamworks;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A16 RID: 2582
	public class DemoEndScreen : MonoBehaviour
	{
		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x0600457A RID: 17786 RVA: 0x00123B13 File Offset: 0x00121D13
		// (set) Token: 0x0600457B RID: 17787 RVA: 0x00123B1B File Offset: 0x00121D1B
		public bool IsOpen { get; private set; }

		// Token: 0x0600457C RID: 17788 RVA: 0x00123B24 File Offset: 0x00121D24
		public void Awake()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x00123B55 File Offset: 0x00121D55
		private void OnDestroy()
		{
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x000045B1 File Offset: 0x000027B1
		[Button]
		public void Open()
		{
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x00123B68 File Offset: 0x00121D68
		private void Update()
		{
			if (this.IsOpen)
			{
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			}
		}

		// Token: 0x06004580 RID: 17792 RVA: 0x00123B80 File Offset: 0x00121D80
		public void Close()
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0.25f);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
		}

		// Token: 0x06004581 RID: 17793 RVA: 0x00123C11 File Offset: 0x00121E11
		private void Exit(ExitAction action)
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (action.Used)
			{
				return;
			}
			if (action.exitType != ExitType.Escape)
			{
				return;
			}
			action.Used = true;
			this.Close();
		}

		// Token: 0x06004582 RID: 17794 RVA: 0x00123C3C File Offset: 0x00121E3C
		public void LinkClicked()
		{
			if (SteamManager.Initialized)
			{
				SteamFriends.ActivateGameOverlayToStore(new AppId_t(3164500U), 0);
			}
		}

		// Token: 0x0400323B RID: 12859
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x0400323C RID: 12860
		public RectTransform Container;
	}
}
