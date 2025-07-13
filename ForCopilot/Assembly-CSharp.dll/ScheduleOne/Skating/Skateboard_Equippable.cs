using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Skating
{
	// Token: 0x020002E1 RID: 737
	public class Skateboard_Equippable : Equippable_Viewmodel
	{
		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x00046B70 File Offset: 0x00044D70
		// (set) Token: 0x06000FF9 RID: 4089 RVA: 0x00046B78 File Offset: 0x00044D78
		public bool IsRiding { get; private set; }

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000FFA RID: 4090 RVA: 0x00046B81 File Offset: 0x00044D81
		// (set) Token: 0x06000FFB RID: 4091 RVA: 0x00046B89 File Offset: 0x00044D89
		public Skateboard ActiveSkateboard { get; private set; }

		// Token: 0x06000FFC RID: 4092 RVA: 0x00046B92 File Offset: 0x00044D92
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 0);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("heldskateboard");
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x00046BBC File Offset: 0x00044DBC
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (action.exitType != ExitType.Escape)
			{
				return;
			}
			if (!this.IsRiding)
			{
				return;
			}
			action.Used = true;
			this.Dismount();
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x00046BE8 File Offset: 0x00044DE8
		protected override void Update()
		{
			base.Update();
			if (GameInput.GetButton(GameInput.ButtonCode.PrimaryClick) && !this.blockDismount && !Singleton<PauseMenu>.Instance.IsPaused)
			{
				if (this.IsRiding)
				{
					if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
					{
						this.Dismount();
					}
				}
				else if (this.CanMountHere() && !PlayerSingleton<PlayerMovement>.Instance.isCrouched && (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) || this.mountTime > 0f))
				{
					this.mountTime += Time.deltaTime;
					Singleton<HUD>.Instance.ShowRadialIndicator(this.mountTime / 0.33f);
					if (this.mountTime >= 0.33f)
					{
						this.Mount();
					}
				}
				else
				{
					this.mountTime = 0f;
				}
			}
			else
			{
				this.mountTime = 0f;
			}
			if (this.IsRiding && Vector3.Angle(this.ActiveSkateboard.transform.up, Vector3.up) > 80f)
			{
				this.Dismount();
			}
			this.UpdateModel();
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x00046CF0 File Offset: 0x00044EF0
		private void UpdateModel()
		{
			Vector3 b = this.IsRiding ? this.ModelPosition_Lowered.localPosition : this.ModelPosition_Raised.localPosition;
			this.ModelContainer.localPosition = Vector3.Lerp(this.ModelContainer.localPosition, b, Time.deltaTime * 8f);
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x00046D45 File Offset: 0x00044F45
		public override void Unequip()
		{
			base.Unequip();
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			if (this.IsRiding)
			{
				this.Dismount();
			}
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00046D78 File Offset: 0x00044F78
		public void Mount()
		{
			this.IsRiding = true;
			this.mountTime = 0f;
			this.ActiveSkateboard = UnityEngine.Object.Instantiate<GameObject>(this.SkateboardPrefab.gameObject, null).GetComponent<Skateboard>();
			this.ActiveSkateboard.Equippable = this;
			Pose skateboardSpawnPose = this.GetSkateboardSpawnPose();
			this.ActiveSkateboard.transform.position = skateboardSpawnPose.position;
			this.ActiveSkateboard.transform.rotation = skateboardSpawnPose.rotation;
			Player.Local.Spawn(this.ActiveSkateboard.NetworkObject, Player.Local.Connection, default(Scene));
			Vector3 velocity = Player.Local.VelocityCalculator.Velocity;
			this.ActiveSkateboard.SetVelocity(velocity * 1.2f);
			Player.Local.MountSkateboard(this.ActiveSkateboard);
			Player.Local.Avatar.SetEquippable(string.Empty);
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00046E68 File Offset: 0x00045068
		public void Dismount()
		{
			this.IsRiding = false;
			this.mountTime = 0f;
			Vector3 velocity = this.ActiveSkateboard.Rb.velocity;
			float num = 50f;
			float time = 0.7f * Mathf.Clamp01(velocity.magnitude / 9f);
			Vector3 normalized = Vector3.ProjectOnPlane(velocity, Vector3.up).normalized;
			PlayerSingleton<PlayerMovement>.Instance.SetResidualVelocity(normalized, velocity.magnitude * num, time);
			Player.Local.DismountSkateboard();
			Player.Local.Despawn(this.ActiveSkateboard.NetworkObject, null);
			UnityEngine.Object.Destroy(this.ActiveSkateboard.gameObject);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("heldskateboard");
			this.ActiveSkateboard = null;
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x00046F30 File Offset: 0x00045130
		private bool CanMountHere()
		{
			return Vector3.Angle(this.GetSkateboardSpawnPose().rotation * Vector3.up, Vector3.up) <= 30f;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x00046F5C File Offset: 0x0004515C
		private Pose GetSkateboardSpawnPose()
		{
			Vector3 vector = Player.Local.PlayerBasePosition + Player.Local.transform.forward * 0.4f + Vector3.up * 0.4f;
			Vector3 vector2 = Player.Local.PlayerBasePosition - Player.Local.transform.forward * 0.4f + Vector3.up * 0.4f;
			Debug.DrawRay(vector, Vector3.down * 0.7f, Color.cyan, 10f);
			Debug.DrawRay(vector2, Vector3.down * 0.7f, Color.cyan, 10f);
			RaycastHit raycastHit;
			if (!Physics.Raycast(vector, Vector3.down, ref raycastHit, 0.7f, this.SkateboardPrefab.GroundDetectionMask, 1))
			{
				raycastHit.point = vector + Vector3.down * 0.7f;
			}
			RaycastHit raycastHit2;
			if (!Physics.Raycast(vector2, Vector3.down, ref raycastHit2, 0.7f, this.SkateboardPrefab.GroundDetectionMask, 1))
			{
				raycastHit2.point = vector2 + Vector3.down * 0.7f;
			}
			Vector3 position = (raycastHit.point + raycastHit2.point) / 2f + Vector3.up * (0.05f + this.SkateboardPrefab.HoverHeight);
			Vector3 normalized = (raycastHit.point - raycastHit2.point).normalized;
			Vector3 normalized2 = Vector3.Cross(Vector3.up, normalized).normalized;
			Vector3 normalized3 = Vector3.Cross(normalized, normalized2).normalized;
			Quaternion rotation = Quaternion.LookRotation(normalized, normalized3);
			return new Pose
			{
				position = position,
				rotation = rotation
			};
		}

		// Token: 0x04001072 RID: 4210
		public const float ModelLerpSpeed = 8f;

		// Token: 0x04001073 RID: 4211
		public const float SurfaceSampleDistance = 0.4f;

		// Token: 0x04001074 RID: 4212
		public const float SurfaceSampleRayLength = 0.7f;

		// Token: 0x04001075 RID: 4213
		public const float BoardSpawnUpwardsShift = 0.05f;

		// Token: 0x04001076 RID: 4214
		public const float BoardSpawnAngleLimit = 30f;

		// Token: 0x04001077 RID: 4215
		public const float MountTime = 0.33f;

		// Token: 0x04001078 RID: 4216
		public const float BoardMomentumTransfer = 1.2f;

		// Token: 0x04001079 RID: 4217
		public const float DismountAngle = 80f;

		// Token: 0x0400107C RID: 4220
		public Skateboard SkateboardPrefab;

		// Token: 0x0400107D RID: 4221
		public bool blockDismount;

		// Token: 0x0400107E RID: 4222
		[Header("References")]
		public Transform ModelContainer;

		// Token: 0x0400107F RID: 4223
		public Transform ModelPosition_Raised;

		// Token: 0x04001080 RID: 4224
		public Transform ModelPosition_Lowered;

		// Token: 0x04001081 RID: 4225
		private float mountTime;
	}
}
