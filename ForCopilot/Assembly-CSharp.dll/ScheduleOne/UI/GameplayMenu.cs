using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using ScheduleOne.UI.Phone;
using ScheduleOne.UI.Phone.Map;
using ScheduleOne.UI.Phone.Messages;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A24 RID: 2596
	public class GameplayMenu : Singleton<GameplayMenu>
	{
		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x060045DF RID: 17887 RVA: 0x00125687 File Offset: 0x00123887
		// (set) Token: 0x060045E0 RID: 17888 RVA: 0x0012568F File Offset: 0x0012388F
		public bool IsOpen { get; protected set; }

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x060045E1 RID: 17889 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CharacterScreenEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x060045E2 RID: 17890 RVA: 0x00125698 File Offset: 0x00123898
		// (set) Token: 0x060045E3 RID: 17891 RVA: 0x001256A0 File Offset: 0x001238A0
		public GameplayMenu.EGameplayScreen CurrentScreen { get; protected set; }

		// Token: 0x060045E4 RID: 17892 RVA: 0x001256AC File Offset: 0x001238AC
		protected override void Start()
		{
			base.Start();
			this.OverlayCamera.enabled = false;
			this.OverlayLight.enabled = false;
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, -2f, base.transform.localPosition.z);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 0);
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x00125720 File Offset: 0x00123920
		public void Exit(ExitAction exit)
		{
			if (exit.Used)
			{
				return;
			}
			if (exit.exitType == ExitType.RightClick && Singleton<ItemUIManager>.InstanceExists && Singleton<ItemUIManager>.Instance.CanDragFromSlot(Singleton<ItemUIManager>.Instance.HoveredSlot))
			{
				return;
			}
			if (this.IsOpen)
			{
				exit.Used = true;
				this.SetIsOpen(false);
			}
		}

		// Token: 0x060045E6 RID: 17894 RVA: 0x00125774 File Offset: 0x00123974
		protected virtual void Update()
		{
			if (!GameInput.IsTyping && !Singleton<PauseMenu>.Instance.IsPaused && (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount == 0 || this.IsOpen))
			{
				if (GameInput.GetButtonDown(GameInput.ButtonCode.TogglePhone))
				{
					this.SetIsOpen(!this.IsOpen);
				}
				if (GameInput.GetButtonDown(GameInput.ButtonCode.OpenMap) && !GameManager.IS_TUTORIAL)
				{
					if (PlayerSingleton<MapApp>.Instance.isOpen && this.IsOpen && this.CurrentScreen == GameplayMenu.EGameplayScreen.Phone)
					{
						this.SetIsOpen(false);
					}
					else
					{
						this.<Update>g__PrepAppOpen|22_0();
						PlayerSingleton<MapApp>.Instance.SetOpen(true);
					}
				}
				if (GameInput.GetButtonDown(GameInput.ButtonCode.OpenJournal))
				{
					if (PlayerSingleton<JournalApp>.Instance.isOpen && this.IsOpen && this.CurrentScreen == GameplayMenu.EGameplayScreen.Phone)
					{
						this.SetIsOpen(false);
					}
					else
					{
						this.<Update>g__PrepAppOpen|22_0();
						PlayerSingleton<JournalApp>.Instance.SetOpen(true);
					}
				}
				if (GameInput.GetButtonDown(GameInput.ButtonCode.OpenTexts))
				{
					if (PlayerSingleton<MessagesApp>.Instance.isOpen && this.IsOpen && this.CurrentScreen == GameplayMenu.EGameplayScreen.Phone)
					{
						this.SetIsOpen(false);
					}
					else
					{
						this.<Update>g__PrepAppOpen|22_0();
						PlayerSingleton<MessagesApp>.Instance.SetOpen(true);
					}
				}
				if (this.IsOpen)
				{
					bool characterScreenEnabled = this.CharacterScreenEnabled;
				}
			}
		}

		// Token: 0x060045E7 RID: 17895 RVA: 0x0012589C File Offset: 0x00123A9C
		public void SetScreen(GameplayMenu.EGameplayScreen screen)
		{
			GameplayMenu.<>c__DisplayClass23_0 CS$<>8__locals1 = new GameplayMenu.<>c__DisplayClass23_0();
			CS$<>8__locals1.screen = screen;
			CS$<>8__locals1.<>4__this = this;
			if (this.CurrentScreen == CS$<>8__locals1.screen)
			{
				return;
			}
			CS$<>8__locals1.previousScreen = this.CurrentScreen;
			this.CurrentScreen = CS$<>8__locals1.screen;
			if (CS$<>8__locals1.screen == GameplayMenu.EGameplayScreen.Phone)
			{
				PlayerSingleton<Phone>.Instance.SetIsOpen(true);
			}
			else if (CS$<>8__locals1.screen == GameplayMenu.EGameplayScreen.Character)
			{
				Singleton<CharacterDisplay>.Instance.SetOpen(true);
			}
			if (this.screenChangeRoutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.screenChangeRoutine);
			}
			Singleton<GameplayMenuInterface>.Instance.SetSelected(CS$<>8__locals1.screen);
			this.screenChangeRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SetScreen>g__ScreenChange|0());
		}

		// Token: 0x060045E8 RID: 17896 RVA: 0x0012594C File Offset: 0x00123B4C
		public void SetIsOpen(bool open)
		{
			this.IsOpen = open;
			if (open)
			{
				this.OverlayLight.enabled = true;
			}
			if (this.CurrentScreen == GameplayMenu.EGameplayScreen.Phone)
			{
				if (open)
				{
					PlayerSingleton<Phone>.Instance.SetIsOpen(true);
				}
				else
				{
					PlayerSingleton<Phone>.Instance.SetIsOpen(false);
				}
			}
			else if (this.CurrentScreen == GameplayMenu.EGameplayScreen.Character)
			{
				if (open)
				{
					Singleton<CharacterDisplay>.Instance.SetOpen(true);
				}
				else
				{
					Singleton<CharacterDisplay>.Instance.SetOpen(false);
				}
			}
			if (this.IsOpen)
			{
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
				PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
				PlayerSingleton<PlayerMovement>.Instance.canMove = false;
				Singleton<ItemUIManager>.Instance.SetDraggingEnabled(true, false);
				Singleton<CompassManager>.Instance.SetVisible(false);
				Player.Local.SendEquippable_Networked("Avatar/Equippables/Phone_Lowered");
				Singleton<InputPromptsCanvas>.Instance.LoadModule("phone");
				Singleton<GameplayMenuInterface>.Instance.Open();
			}
			else
			{
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				if (Player.Local.CurrentVehicle == null)
				{
					PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
					PlayerSingleton<PlayerMovement>.Instance.canMove = true;
					PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
				}
				else
				{
					Singleton<HUD>.Instance.SetCrosshairVisible(false);
				}
				Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
				Singleton<CompassManager>.Instance.SetVisible(true);
				Player.Local.SendEquippable_Networked(string.Empty);
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
				Singleton<GameplayMenuInterface>.Instance.Close();
			}
			if (this.openCloseRoutine != null)
			{
				base.StopCoroutine(this.openCloseRoutine);
			}
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			if (this.IsOpen)
			{
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			}
			this.openCloseRoutine = base.StartCoroutine(this.<SetIsOpen>g__SetIsOpenRoutine|24_0(open));
		}

		// Token: 0x060045EA RID: 17898 RVA: 0x00125B15 File Offset: 0x00123D15
		[CompilerGenerated]
		private void <Update>g__PrepAppOpen|22_0()
		{
			if (!this.IsOpen)
			{
				this.SetIsOpen(true);
			}
			if (this.CurrentScreen != GameplayMenu.EGameplayScreen.Phone)
			{
				this.SetScreen(GameplayMenu.EGameplayScreen.Phone);
			}
			if (Phone.ActiveApp != null)
			{
				PlayerSingleton<Phone>.Instance.RequestCloseApp();
			}
		}

		// Token: 0x060045EB RID: 17899 RVA: 0x00125B4C File Offset: 0x00123D4C
		[CompilerGenerated]
		private IEnumerator <SetIsOpen>g__SetIsOpenRoutine|24_0(bool open)
		{
			if (open)
			{
				this.OverlayCamera.enabled = true;
			}
			float num = 1f - base.transform.localPosition.y / -2f;
			float adjustedLerpTime = 0.06f;
			float startVert = base.transform.localPosition.y;
			float endVert = 0f;
			if (open)
			{
				adjustedLerpTime *= 1f - num;
				endVert = 0.02f;
			}
			else
			{
				adjustedLerpTime *= num;
				endVert = -2f;
			}
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(open, adjustedLerpTime);
			for (float i = 0f; i < adjustedLerpTime; i += Time.deltaTime)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.Lerp(startVert, endVert, i / adjustedLerpTime), base.transform.localPosition.z);
				yield return new WaitForEndOfFrame();
			}
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, endVert, base.transform.localPosition.z);
			if (!open)
			{
				this.OverlayCamera.enabled = false;
				this.OverlayLight.enabled = false;
			}
			this.openCloseRoutine = null;
			yield break;
		}

		// Token: 0x04003291 RID: 12945
		public const float OpenVerticalOffset = 0.02f;

		// Token: 0x04003292 RID: 12946
		public const float ClosedVerticalOffset = -2f;

		// Token: 0x04003293 RID: 12947
		public const float OpenTime = 0.06f;

		// Token: 0x04003294 RID: 12948
		public const float SlideTime = 0.12f;

		// Token: 0x04003297 RID: 12951
		[Header("References")]
		public Camera OverlayCamera;

		// Token: 0x04003298 RID: 12952
		public Light OverlayLight;

		// Token: 0x04003299 RID: 12953
		[Header("Settings")]
		public float ContainerOffset_PhoneScreen = -0.1f;

		// Token: 0x0400329A RID: 12954
		private Coroutine openCloseRoutine;

		// Token: 0x0400329B RID: 12955
		private Coroutine screenChangeRoutine;

		// Token: 0x02000A25 RID: 2597
		public enum EGameplayScreen
		{
			// Token: 0x0400329D RID: 12957
			Phone,
			// Token: 0x0400329E RID: 12958
			Character
		}
	}
}
