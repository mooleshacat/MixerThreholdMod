using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product;
using ScheduleOne.Tools;
using UnityEngine;

namespace ScheduleOne.Packaging
{
	// Token: 0x020008DA RID: 2266
	public class FunctionalProduct : Draggable
	{
		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06003CF3 RID: 15603 RVA: 0x00100C0D File Offset: 0x000FEE0D
		// (set) Token: 0x06003CF4 RID: 15604 RVA: 0x00100C15 File Offset: 0x000FEE15
		public SmoothedVelocityCalculator VelocityCalculator { get; private set; }

		// Token: 0x06003CF5 RID: 15605 RVA: 0x00100C20 File Offset: 0x000FEE20
		public virtual void Initialize(PackagingStation station, ItemInstance item, Transform alignment, bool align = true)
		{
			if (align)
			{
				this.AlignTo(alignment);
			}
			this.startLocalPos = base.transform.localPosition;
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Task"));
			this.InitializeVisuals(item);
			base.Rb.collisionDetectionMode = 2;
			if (this.VelocityCalculator == null)
			{
				this.VelocityCalculator = base.gameObject.AddComponent<SmoothedVelocityCalculator>();
				this.VelocityCalculator.MaxReasonableVelocity = 2f;
			}
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x00100CA0 File Offset: 0x000FEEA0
		public virtual void Initialize(ItemInstance item)
		{
			this.startLocalPos = base.transform.localPosition;
			LayerUtility.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Task"));
			this.InitializeVisuals(item);
			base.Rb.collisionDetectionMode = 2;
			if (this.VelocityCalculator == null)
			{
				this.VelocityCalculator = base.gameObject.AddComponent<SmoothedVelocityCalculator>();
				this.VelocityCalculator.MaxReasonableVelocity = 2f;
			}
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x00100D18 File Offset: 0x000FEF18
		public virtual void InitializeVisuals(ItemInstance item)
		{
			ProductItemInstance productItemInstance = item as ProductItemInstance;
			if (productItemInstance == null)
			{
				Console.LogError("Item instance is not a product instance!", null);
				return;
			}
			productItemInstance.SetupPackagingVisuals(this.Visuals);
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x00100D48 File Offset: 0x000FEF48
		public void AlignTo(Transform alignment)
		{
			base.transform.rotation = alignment.rotation * (Quaternion.Inverse(this.AlignmentPoint.rotation) * base.transform.rotation);
			base.transform.position = alignment.position + (base.transform.position - this.AlignmentPoint.position);
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x00100DBC File Offset: 0x000FEFBC
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
		}

		// Token: 0x06003CFA RID: 15610 RVA: 0x00100DC4 File Offset: 0x000FEFC4
		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (this.ClampZ)
			{
				this.Clamp();
			}
		}

		// Token: 0x06003CFB RID: 15611 RVA: 0x00100DDC File Offset: 0x000FEFDC
		private void Clamp()
		{
			float num = Mathf.Clamp(Mathf.Abs(base.transform.localPosition.x / this.startLocalPos.x), 0f, 1f);
			float num2 = Mathf.Min(Mathf.Abs(this.startLocalPos.z) * num, this.lowestMaxZ);
			this.lowestMaxZ = num2;
			Vector3 vector = base.transform.parent.InverseTransformPoint(base.originalHitPoint);
			vector.z = Mathf.Clamp(vector.z, -num2, num2);
			Vector3 originalHitPoint = base.transform.parent.TransformPoint(vector);
			base.SetOriginalHitPoint(originalHitPoint);
		}

		// Token: 0x04002BD6 RID: 11222
		public bool ClampZ = true;

		// Token: 0x04002BD7 RID: 11223
		[Header("References")]
		public Transform AlignmentPoint;

		// Token: 0x04002BD8 RID: 11224
		public FilledPackagingVisuals Visuals;

		// Token: 0x04002BD9 RID: 11225
		private Vector3 startLocalPos;

		// Token: 0x04002BDA RID: 11226
		private float lowestMaxZ = 500f;
	}
}
