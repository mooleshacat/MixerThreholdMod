using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Building
{
	// Token: 0x020007CF RID: 1999
	public class BuildUpdate_Surface : BuildUpdate_Base
	{
		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06003629 RID: 13865 RVA: 0x000E4116 File Offset: 0x000E2316
		private float detectionRange
		{
			get
			{
				return Mathf.Max(this.BuildableItemClass.HoldDistance, 4f);
			}
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x000E412D File Offset: 0x000E232D
		protected virtual void Start()
		{
			this.LateUpdate();
		}

		// Token: 0x0600362B RID: 13867 RVA: 0x000E4135 File Offset: 0x000E2335
		protected virtual void Update()
		{
			this.CheckRotation();
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && this.validPosition)
			{
				this.Place();
			}
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x000E4154 File Offset: 0x000E2354
		protected virtual void LateUpdate()
		{
			this.validPosition = false;
			this.GhostModel.transform.up = Vector3.up;
			this.PositionObjectInFrontOfPlayer(this.BuildableItemClass.HoldDistance, true);
			Surface surface = null;
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast_ExcludeBuildables(this.detectionRange, out raycastHit, this.DetectionMask, true))
			{
				surface = raycastHit.collider.GetComponentInParent<Surface>();
			}
			if (this.IsSurfaceValidForItem(surface, raycastHit.collider, raycastHit.point))
			{
				this.hoveredValidSurface = surface;
				this.validPosition = true;
			}
			else
			{
				this.hoveredValidSurface = null;
			}
			float d;
			float d2;
			float d3;
			if ((!Application.isEditor || !Input.GetKey(KeyCode.LeftAlt)) && this.BuildableItemClass.GetPenetration(out d, out d2, out d3))
			{
				if (Vector3.Distance(this.GhostModel.transform.position - this.GhostModel.transform.right * d, PlayerSingleton<PlayerCamera>.Instance.transform.position) < Vector3.Distance(this.GhostModel.transform.position - this.GhostModel.transform.forward * d2, PlayerSingleton<PlayerCamera>.Instance.transform.position))
				{
					this.GhostModel.transform.position -= this.GhostModel.transform.right * d;
					if (this.BuildableItemClass.GetPenetration(out d, out d2, out d3))
					{
						this.GhostModel.transform.position -= this.GhostModel.transform.forward * d2;
					}
				}
				else
				{
					this.GhostModel.transform.position -= this.GhostModel.transform.forward * d2;
					if (this.BuildableItemClass.GetPenetration(out d, out d2, out d3))
					{
						this.GhostModel.transform.position -= this.GhostModel.transform.right * d;
					}
				}
				this.GhostModel.transform.position -= this.GhostModel.transform.up * d3;
			}
			this.UpdateMaterials();
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x000E43BC File Offset: 0x000E25BC
		protected void PositionObjectInFrontOfPlayer(float dist, bool sanitizeForward)
		{
			Vector3 forward = PlayerSingleton<PlayerCamera>.Instance.transform.forward;
			if (sanitizeForward)
			{
				forward.y = 0f;
			}
			Vector3 vector = PlayerSingleton<PlayerCamera>.Instance.transform.position + forward * dist;
			Vector3 forward2 = (PlayerSingleton<PlayerCamera>.Instance.transform.position - vector).normalized;
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast_ExcludeBuildables(this.detectionRange, out raycastHit, this.DetectionMask, true))
			{
				vector = raycastHit.point;
				forward2 = raycastHit.normal;
			}
			else if (this.BuildableItemClass.MidAirCenterPoint != null)
			{
				vector += -this.GhostModel.transform.InverseTransformPoint(this.BuildableItemClass.MidAirCenterPoint.transform.position);
			}
			Quaternion lhs = Quaternion.LookRotation(forward2, Vector3.up);
			this.GhostModel.transform.rotation = lhs * Quaternion.Inverse(this.BuildableItemClass.BuildPoint.transform.rotation);
			this.GhostModel.transform.RotateAround(this.BuildableItemClass.BuildPoint.transform.position, this.BuildableItemClass.BuildPoint.transform.forward, this.CurrentRotation);
			this.GhostModel.transform.position = vector - this.GhostModel.transform.InverseTransformPoint(this.BuildableItemClass.BuildPoint.transform.position);
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x000E4550 File Offset: 0x000E2750
		private bool IsSurfaceValidForItem(Surface surface, Collider hitCollider, Vector3 hitPoint)
		{
			return !(surface == null) && this.BuildableItemClass.ValidSurfaceTypes.Contains(surface.SurfaceType) && !(surface.ParentProperty == null) && surface.ParentProperty.IsOwned && surface.IsPointValid(hitPoint, hitCollider);
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x000E45AC File Offset: 0x000E27AC
		protected void CheckRotation()
		{
			if (!this.BuildableItemClass.AllowRotation)
			{
				this.CurrentRotation = 0f;
				return;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.RotateLeft) && !GameInput.IsTyping)
			{
				this.CurrentRotation -= this.BuildableItemClass.RotationIncrement;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.RotateRight) && !GameInput.IsTyping)
			{
				this.CurrentRotation += this.BuildableItemClass.RotationIncrement;
			}
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x000E4624 File Offset: 0x000E2824
		protected void UpdateMaterials()
		{
			Material material = Singleton<BuildManager>.Instance.ghostMaterial_White;
			if (!this.validPosition)
			{
				material = Singleton<BuildManager>.Instance.ghostMaterial_Red;
			}
			if (this.currentGhostMaterial != material)
			{
				this.currentGhostMaterial = material;
				Singleton<BuildManager>.Instance.ApplyMaterial(this.GhostModel, material, true);
			}
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x000E4678 File Offset: 0x000E2878
		protected virtual void Place()
		{
			Mathf.RoundToInt(this.CurrentRotation);
			Vector3 relativePosition = this.hoveredValidSurface.GetRelativePosition(this.GhostModel.transform.position);
			Quaternion relativeRotation = this.hoveredValidSurface.GetRelativeRotation(this.GhostModel.transform.rotation);
			Singleton<BuildManager>.Instance.CreateSurfaceItem(this.ItemInstance.GetCopy(1), this.hoveredValidSurface, relativePosition, relativeRotation, "");
			PlayerSingleton<PlayerInventory>.Instance.equippedSlot.ChangeQuantity(-1, false);
			Singleton<BuildManager>.Instance.PlayBuildSound((this.ItemInstance.Definition as BuildableItemDefinition).BuildSoundType, this.GhostModel.transform.position);
		}

		// Token: 0x0400265D RID: 9821
		public GameObject GhostModel;

		// Token: 0x0400265E RID: 9822
		public SurfaceItem BuildableItemClass;

		// Token: 0x0400265F RID: 9823
		public ItemInstance ItemInstance;

		// Token: 0x04002660 RID: 9824
		public float CurrentRotation;

		// Token: 0x04002661 RID: 9825
		[Header("Settings")]
		public LayerMask DetectionMask;

		// Token: 0x04002662 RID: 9826
		protected bool validPosition;

		// Token: 0x04002663 RID: 9827
		protected Material currentGhostMaterial;

		// Token: 0x04002664 RID: 9828
		protected Surface hoveredValidSurface;
	}
}
