using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.Equipping;
using ScheduleOne.Interaction;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C48 RID: 3144
	public class Dumpster : GridItem
	{
		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x0600586D RID: 22637 RVA: 0x0017632E File Offset: 0x0017452E
		// (set) Token: 0x0600586E RID: 22638 RVA: 0x00176336 File Offset: 0x00174536
		public bool lidOpen { get; protected set; }

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x0600586F RID: 22639 RVA: 0x0017633F File Offset: 0x0017453F
		// (set) Token: 0x06005870 RID: 22640 RVA: 0x00176347 File Offset: 0x00174547
		public float currentTrashLevel { get; protected set; }

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x06005871 RID: 22641 RVA: 0x00176350 File Offset: 0x00174550
		public bool isFull
		{
			get
			{
				return this.currentTrashLevel >= Dumpster.capacity;
			}
		}

		// Token: 0x06005872 RID: 22642 RVA: 0x00176364 File Offset: 0x00174564
		protected virtual void Update()
		{
			if (this.lidOpen)
			{
				this.lid_CurrentAngle = Mathf.Clamp(this.lid_CurrentAngle + Time.deltaTime * 90f * 3f, 0f, 90f);
			}
			else
			{
				this.lid_CurrentAngle = Mathf.Clamp(this.lid_CurrentAngle - Time.deltaTime * 90f * 3f, 0f, 90f);
			}
			this.lid.localRotation = Quaternion.Euler(0f, 0f, -this.lid_CurrentAngle);
		}

		// Token: 0x06005873 RID: 22643 RVA: 0x001763F6 File Offset: 0x001745F6
		public virtual void Lid_Hovered()
		{
			if (this.lidOpen)
			{
				this.lid_IntObj.SetMessage("Close dumpster");
				return;
			}
			this.lid_IntObj.SetMessage("Open dumpster");
		}

		// Token: 0x06005874 RID: 22644 RVA: 0x00176421 File Offset: 0x00174621
		public virtual void Lid_Interacted()
		{
			this.lidOpen = !this.lidOpen;
		}

		// Token: 0x06005875 RID: 22645 RVA: 0x00176432 File Offset: 0x00174632
		protected bool DoesPlayerHaveBinEquipped()
		{
			return PlayerSingleton<PlayerInventory>.Instance.equippedSlot != null && PlayerSingleton<PlayerInventory>.Instance.equippedSlot.Equippable is Equippable_Bin;
		}

		// Token: 0x06005876 RID: 22646 RVA: 0x00176459 File Offset: 0x00174659
		public void ChangeTrashLevel(float change)
		{
			this.SetTrashLevel(this.currentTrashLevel + change);
		}

		// Token: 0x06005877 RID: 22647 RVA: 0x00176469 File Offset: 0x00174669
		public void SetTrashLevel(float trashLevel)
		{
			this.currentTrashLevel = Mathf.Clamp(trashLevel, 0f, Dumpster.capacity);
			this.UpdateTrashVisuals();
		}

		// Token: 0x06005878 RID: 22648 RVA: 0x00176488 File Offset: 0x00174688
		private void UpdateTrashVisuals()
		{
			this.trash.localPosition = new Vector3(this.trash.localPosition.x, this.trash_MinY + this.currentTrashLevel / Dumpster.capacity * (this.trash_MaxY - this.trash_MinY));
			this.trash.gameObject.SetActive(this.currentTrashLevel > 0f);
		}

		// Token: 0x06005879 RID: 22649 RVA: 0x001764F3 File Offset: 0x001746F3
		public override bool CanBeDestroyed(out string reason)
		{
			if (this.currentTrashLevel > 0f)
			{
				reason = "Dumpster is not empty";
				return false;
			}
			return base.CanBeDestroyed(out reason);
		}

		// Token: 0x0600587C RID: 22652 RVA: 0x0017651E File Offset: 0x0017471E
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.DumpsterAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.DumpsterAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600587D RID: 22653 RVA: 0x00176537 File Offset: 0x00174737
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.DumpsterAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.DumpsterAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600587E RID: 22654 RVA: 0x00176550 File Offset: 0x00174750
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600587F RID: 22655 RVA: 0x0017655E File Offset: 0x0017475E
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040040DA RID: 16602
		public static float capacity = 100f;

		// Token: 0x040040DB RID: 16603
		[Header("References")]
		[SerializeField]
		protected InteractableObject lid_IntObj;

		// Token: 0x040040DC RID: 16604
		[SerializeField]
		protected InteractableObject inner_IntObj;

		// Token: 0x040040DD RID: 16605
		[SerializeField]
		protected Transform lid;

		// Token: 0x040040DE RID: 16606
		[SerializeField]
		protected Transform trash;

		// Token: 0x040040DF RID: 16607
		public Transform standPoint;

		// Token: 0x040040E0 RID: 16608
		[Header("Settings")]
		[SerializeField]
		protected float trash_MinY;

		// Token: 0x040040E1 RID: 16609
		[SerializeField]
		protected float trash_MaxY;

		// Token: 0x040040E4 RID: 16612
		private float lid_CurrentAngle;

		// Token: 0x040040E5 RID: 16613
		private bool dll_Excuted;

		// Token: 0x040040E6 RID: 16614
		private bool dll_Excuted;
	}
}
