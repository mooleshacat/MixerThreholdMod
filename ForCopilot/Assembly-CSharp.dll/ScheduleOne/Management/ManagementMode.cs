using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.UI.Input;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Management
{
	// Token: 0x020005BC RID: 1468
	public class ManagementMode : Singleton<ManagementMode>
	{
		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x0600242B RID: 9259 RVA: 0x0009496B File Offset: 0x00092B6B
		// (set) Token: 0x0600242C RID: 9260 RVA: 0x00094973 File Offset: 0x00092B73
		public Property CurrentProperty { get; private set; }

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x0600242D RID: 9261 RVA: 0x0009497C File Offset: 0x00092B7C
		public bool isActive
		{
			get
			{
				return this.CurrentProperty != null;
			}
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x0009498A File Offset: 0x00092B8A
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 1);
			this.Canvas.enabled = false;
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x000949B0 File Offset: 0x00092BB0
		private void Update()
		{
			this.UpdateInput();
			if (this.isActive && Player.Local.CurrentProperty != this.CurrentProperty)
			{
				this.ExitManagementMode();
			}
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x000949E0 File Offset: 0x00092BE0
		private void UpdateInput()
		{
			if (!Singleton<GameInput>.InstanceExists)
			{
				return;
			}
			this.ManagementModeInputPrompt.enabled = (this.isActive ? ManagementMode.CanExitManagementMode() : ManagementMode.CanEnterManagementMode());
			this.ManagementModeInputPrompt.Label = (this.isActive ? "Exit Management Mode" : "Enter Management Mode");
			if (GameInput.GetButtonDown(GameInput.ButtonCode.ManagementMode))
			{
				if (this.CurrentProperty != null)
				{
					this.ExitManagementMode();
					return;
				}
				if (Player.Local.CurrentProperty != null && Player.Local.CurrentProperty.IsOwned)
				{
					this.EnterManagementMode(Player.Local.CurrentProperty);
				}
			}
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x00094A84 File Offset: 0x00092C84
		private void Exit(ExitAction exitAction)
		{
			if (!this.isActive)
			{
				return;
			}
			if (exitAction.Used)
			{
				return;
			}
			if (exitAction.exitType == ExitType.Escape)
			{
				this.ExitManagementMode();
				exitAction.Used = true;
			}
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x00094AB0 File Offset: 0x00092CB0
		public void EnterManagementMode(Property property)
		{
			this.CurrentProperty = property;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.Canvas.enabled = true;
			if (this.OnEnterManagementMode != null)
			{
				this.OnEnterManagementMode.Invoke();
			}
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x00094B00 File Offset: 0x00092D00
		public void ExitManagementMode()
		{
			this.CurrentProperty = null;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.Canvas.enabled = false;
			if (this.onExitManagementMode != null)
			{
				this.onExitManagementMode.Invoke();
			}
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x00094B4E File Offset: 0x00092D4E
		public static bool CanEnterManagementMode()
		{
			return !(Player.Local.CurrentProperty == null) && PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount <= 0;
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x000022C9 File Offset: 0x000004C9
		public static bool CanExitManagementMode()
		{
			return true;
		}

		// Token: 0x04001AD9 RID: 6873
		[Header("References")]
		public InputPrompt ManagementModeInputPrompt;

		// Token: 0x04001ADA RID: 6874
		[Header("UI References")]
		public Canvas Canvas;

		// Token: 0x04001ADB RID: 6875
		public UnityEvent OnEnterManagementMode;

		// Token: 0x04001ADC RID: 6876
		public UnityEvent onExitManagementMode;
	}
}
