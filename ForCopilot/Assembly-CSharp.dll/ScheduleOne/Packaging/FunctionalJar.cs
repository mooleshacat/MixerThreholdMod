using System;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using UnityEngine;

namespace ScheduleOne.Packaging
{
	// Token: 0x020008CA RID: 2250
	public class FunctionalJar : FunctionalPackaging
	{
		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06003CA8 RID: 15528 RVA: 0x000FF4E5 File Offset: 0x000FD6E5
		// (set) Token: 0x06003CA9 RID: 15529 RVA: 0x000FF4ED File Offset: 0x000FD6ED
		public override CursorManager.ECursorType HoveredCursor { get; protected set; } = CursorManager.ECursorType.Finger;

		// Token: 0x06003CAA RID: 15530 RVA: 0x000FF4F8 File Offset: 0x000FD6F8
		public override void Initialize(PackagingStation _station, Transform alignment, bool align = false)
		{
			base.Initialize(_station, alignment, align);
			this.lidPosition = base.transform.InverseTransformPoint(this.Lid.transform.position);
			this.LidObject = this.Lid.gameObject;
			this.Lid.transform.SetParent(_station.Container);
			this.Lid.transform.position = this.LidStartPoint.position;
			this.Lid.transform.rotation = this.LidStartPoint.rotation;
			this.LidSensor.enabled = false;
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x000FF598 File Offset: 0x000FD798
		public override void Destroy()
		{
			UnityEngine.Object.Destroy(this.LidObject);
			base.Destroy();
		}

		// Token: 0x06003CAC RID: 15532 RVA: 0x000FF5AB File Offset: 0x000FD7AB
		protected override void EnableSealing()
		{
			base.EnableSealing();
			this.Lid.enabled = true;
			this.Lid.ClickableEnabled = true;
			this.Lid.Rb.isKinematic = false;
			this.LidSensor.enabled = true;
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x000FF5E8 File Offset: 0x000FD7E8
		protected override void LateUpdate()
		{
			base.LateUpdate();
		}

		// Token: 0x06003CAE RID: 15534 RVA: 0x000FF5F0 File Offset: 0x000FD7F0
		protected override void OnTriggerStay(Collider other)
		{
			base.OnTriggerStay(other);
			if (this.Lid != null && this.Lid.enabled && other.gameObject.name == "LidTrigger")
			{
				this.Seal();
			}
		}

		// Token: 0x06003CAF RID: 15535 RVA: 0x000FF63C File Offset: 0x000FD83C
		public override void Seal()
		{
			base.Seal();
			this.Lid.enabled = false;
			this.Lid.ClickableEnabled = false;
			this.Lid.transform.SetParent(base.transform);
			UnityEngine.Object.Destroy(this.Lid.Rb);
			UnityEngine.Object.Destroy(this.Lid);
			UnityEngine.Object.Destroy(this.LidCollider);
			this.Lid.transform.position = base.transform.TransformPoint(this.lidPosition);
			this.LidSensor.enabled = false;
		}

		// Token: 0x06003CB0 RID: 15536 RVA: 0x000FF6D0 File Offset: 0x000FD8D0
		protected override void FullyPacked()
		{
			base.FullyPacked();
			this.FullyPackedBlocker.SetActive(true);
		}

		// Token: 0x04002B72 RID: 11122
		[Header("References")]
		public Draggable Lid;

		// Token: 0x04002B73 RID: 11123
		public Transform LidStartPoint;

		// Token: 0x04002B74 RID: 11124
		public Collider LidSensor;

		// Token: 0x04002B75 RID: 11125
		public Collider LidCollider;

		// Token: 0x04002B76 RID: 11126
		public GameObject FullyPackedBlocker;

		// Token: 0x04002B77 RID: 11127
		private GameObject LidObject;

		// Token: 0x04002B78 RID: 11128
		private Vector3 lidPosition = Vector3.zero;
	}
}
