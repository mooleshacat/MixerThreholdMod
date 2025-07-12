using System;
using FishNet.Object;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Compass;
using UnityEngine;

namespace ScheduleOne.Casino
{
	// Token: 0x02000797 RID: 1943
	public class CasinoGameController : NetworkBehaviour
	{
		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x0600346D RID: 13421 RVA: 0x000DAFEC File Offset: 0x000D91EC
		// (set) Token: 0x0600346E RID: 13422 RVA: 0x000DAFF4 File Offset: 0x000D91F4
		public bool IsOpen { get; private set; }

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x0600346F RID: 13423 RVA: 0x000DAFFD File Offset: 0x000D91FD
		public CasinoGamePlayerData LocalPlayerData
		{
			get
			{
				return this.Players.GetPlayerData();
			}
		}

		// Token: 0x06003470 RID: 13424 RVA: 0x000DB00A File Offset: 0x000D920A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Casino.CasinoGameController_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003471 RID: 13425 RVA: 0x000DB01E File Offset: 0x000D921E
		protected virtual void OnLocalPlayerRequestJoin(Player player)
		{
			this.Open();
		}

		// Token: 0x06003472 RID: 13426 RVA: 0x000DB026 File Offset: 0x000D9226
		protected virtual void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				this.Close();
				action.Used = true;
			}
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Update()
		{
		}

		// Token: 0x06003474 RID: 13428 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void FixedUpdate()
		{
		}

		// Token: 0x06003475 RID: 13429 RVA: 0x000DB050 File Offset: 0x000D9250
		protected virtual void Open()
		{
			this.IsOpen = true;
			this.Players.AddPlayer(Player.Local);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.localDefaultCameraTransform = this.DefaultCameraTransforms[this.Players.GetPlayerIndex(Player.Local)];
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.localDefaultCameraTransform.position, this.localDefaultCameraTransform.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(65f, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			Singleton<CompassManager>.Instance.SetVisible(false);
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x000DB114 File Offset: 0x000D9314
		protected virtual void Close()
		{
			this.IsOpen = false;
			this.Players.RemovePlayer(Player.Local);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			Singleton<CompassManager>.Instance.SetVisible(true);
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x000DB193 File Offset: 0x000D9393
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Casino.CasinoGameControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Casino.CasinoGameControllerAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x000DB1A6 File Offset: 0x000D93A6
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Casino.CasinoGameControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Casino.CasinoGameControllerAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x000DB1B9 File Offset: 0x000D93B9
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x000DB1C7 File Offset: 0x000D93C7
		protected virtual void dll()
		{
			CasinoGameInteraction interaction = this.Interaction;
			interaction.onLocalPlayerRequestJoin = (Action<Player>)Delegate.Combine(interaction.onLocalPlayerRequestJoin, new Action<Player>(this.OnLocalPlayerRequestJoin));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 0);
		}

		// Token: 0x04002520 RID: 9504
		public const float FOV = 65f;

		// Token: 0x04002521 RID: 9505
		public const float CAMERA_LERP_TIME = 0.2f;

		// Token: 0x04002523 RID: 9507
		[Header("References")]
		public CasinoGamePlayers Players;

		// Token: 0x04002524 RID: 9508
		public CasinoGameInteraction Interaction;

		// Token: 0x04002525 RID: 9509
		public Transform[] DefaultCameraTransforms;

		// Token: 0x04002526 RID: 9510
		protected Transform localDefaultCameraTransform;

		// Token: 0x04002527 RID: 9511
		private bool dll_Excuted;

		// Token: 0x04002528 RID: 9512
		private bool dll_Excuted;
	}
}
