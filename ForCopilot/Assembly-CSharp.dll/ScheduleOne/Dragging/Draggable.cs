using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Dragging
{
	// Token: 0x020006BE RID: 1726
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(InteractableObject))]
	public class Draggable : MonoBehaviour, IGUIDRegisterable
	{
		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002F62 RID: 12130 RVA: 0x000C6FBB File Offset: 0x000C51BB
		public bool IsBeingDragged
		{
			get
			{
				return this.CurrentDragger != null;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002F63 RID: 12131 RVA: 0x000C6FC9 File Offset: 0x000C51C9
		// (set) Token: 0x06002F64 RID: 12132 RVA: 0x000C6FD1 File Offset: 0x000C51D1
		public Player CurrentDragger { get; protected set; }

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002F65 RID: 12133 RVA: 0x000C6FDA File Offset: 0x000C51DA
		// (set) Token: 0x06002F66 RID: 12134 RVA: 0x000C6FE2 File Offset: 0x000C51E2
		public Guid GUID { get; protected set; }

		// Token: 0x06002F67 RID: 12135 RVA: 0x000C6FEC File Offset: 0x000C51EC
		[Button]
		public void RegenerateGUID()
		{
			this.BakedGUID = Guid.NewGuid().ToString();
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002F68 RID: 12136 RVA: 0x000C7012 File Offset: 0x000C5212
		// (set) Token: 0x06002F69 RID: 12137 RVA: 0x000C701A File Offset: 0x000C521A
		public Vector3 initialPosition { get; private set; }

		// Token: 0x06002F6A RID: 12138 RVA: 0x000C7024 File Offset: 0x000C5224
		protected virtual void Awake()
		{
			this.IntObj.MaxInteractionRange = 2.5f;
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
			this.IntObj.SetMessage("Pick up");
			this.initialPosition = base.transform.position;
			if (this.CreateCoM)
			{
				Transform transform = new GameObject("CenterOfMass").transform;
				transform.SetParent(base.transform);
				transform.localPosition = this.Rigidbody.centerOfMass;
				this.IntObj.displayLocationPoint = transform;
				this.DragOrigin = transform;
			}
			if (!string.IsNullOrEmpty(this.BakedGUID))
			{
				this.GUID = new Guid(this.BakedGUID);
				GUIDManager.RegisterObject(this);
			}
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x000C7108 File Offset: 0x000C5308
		protected virtual void Start()
		{
			NetworkSingleton<DragManager>.Instance.RegisterDraggable(this);
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x000C7115 File Offset: 0x000C5315
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x000C7124 File Offset: 0x000C5324
		protected void OnValidate()
		{
			if (this.IntObj == null)
			{
				this.IntObj = base.GetComponent<InteractableObject>();
			}
			if (this.Rigidbody == null)
			{
				this.Rigidbody = base.GetComponent<Rigidbody>();
			}
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x000C715A File Offset: 0x000C535A
		protected void OnDestroy()
		{
			if (NetworkSingleton<DragManager>.InstanceExists)
			{
				if (this.IsBeingDragged && NetworkSingleton<DragManager>.Instance.CurrentDraggable == this)
				{
					NetworkSingleton<DragManager>.Instance.StopDragging(Vector3.zero);
				}
				NetworkSingleton<DragManager>.Instance.Deregister(this);
			}
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x000C7198 File Offset: 0x000C5398
		private void FixedUpdate()
		{
			if (this.IsBeingDragged)
			{
				this.timeSinceLastDrag = 0f;
			}
			else if (this.timeSinceLastDrag < 1f)
			{
				this.timeSinceLastDrag += Time.fixedDeltaTime;
			}
			if (this.IsBeingDragged && this.CurrentDragger != Player.Local)
			{
				Vector3 targetPosition = this.CurrentDragger.MimicCamera.position + this.CurrentDragger.MimicCamera.forward * 1.25f * this.HoldDistanceMultiplier;
				this.ApplyDragForces(targetPosition);
			}
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x000C7238 File Offset: 0x000C5438
		public void ApplyDragForces(Vector3 targetPosition)
		{
			Vector3 vector = targetPosition - base.transform.position;
			if (this.DragOrigin != null)
			{
				vector = targetPosition - this.DragOrigin.position;
			}
			float magnitude = vector.magnitude;
			Vector3 a = vector.normalized * NetworkSingleton<DragManager>.Instance.DragForce * magnitude;
			a -= this.Rigidbody.velocity * NetworkSingleton<DragManager>.Instance.DampingFactor;
			this.Rigidbody.AddForce(a * this.DragForceMultiplier, 5);
			Vector3 a2 = Vector3.Cross(base.transform.up, Vector3.up);
			a2 -= this.Rigidbody.angularVelocity * NetworkSingleton<DragManager>.Instance.TorqueDampingFactor;
			this.Rigidbody.AddTorque(a2 * NetworkSingleton<DragManager>.Instance.TorqueForce, 5);
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x000C7328 File Offset: 0x000C5528
		protected virtual void Hovered()
		{
			if (this.CanInteract() && base.enabled)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				this.IntObj.SetMessage("Pick up");
			}
			else
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
			}
			if (this.onHovered != null)
			{
				this.onHovered.Invoke();
			}
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x000C7382 File Offset: 0x000C5582
		protected virtual void Interacted()
		{
			if (!base.enabled)
			{
				return;
			}
			if (this.onInteracted != null)
			{
				this.onInteracted.Invoke();
			}
			if (!this.CanInteract())
			{
				return;
			}
			NetworkSingleton<DragManager>.Instance.StartDragging(this);
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x000C73B4 File Offset: 0x000C55B4
		private bool CanInteract()
		{
			return !this.IsBeingDragged && this.timeSinceLastDrag >= 0.1f && !NetworkSingleton<DragManager>.Instance.IsDragging && NetworkSingleton<DragManager>.Instance.IsDraggingAllowed();
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x000C73EC File Offset: 0x000C55EC
		public void StartDragging(Player dragger)
		{
			if (this.IsBeingDragged)
			{
				return;
			}
			this.CurrentDragger = dragger;
			this.Rigidbody.useGravity = false;
			if (this.onDragStart != null)
			{
				this.onDragStart.Invoke();
			}
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x000C741D File Offset: 0x000C561D
		public void StopDragging()
		{
			if (!this.IsBeingDragged)
			{
				return;
			}
			this.CurrentDragger = null;
			this.Rigidbody.useGravity = true;
			if (this.onDragEnd != null)
			{
				this.onDragEnd.Invoke();
			}
		}

		// Token: 0x04002150 RID: 8528
		public const float INITIAL_REPLICATION_DISTANCE = 1f;

		// Token: 0x04002151 RID: 8529
		public const float MAX_DRAG_START_RANGE = 2.5f;

		// Token: 0x04002152 RID: 8530
		public const float MAX_TARGET_OFFSET = 1.5f;

		// Token: 0x04002155 RID: 8533
		[SerializeField]
		protected string BakedGUID = string.Empty;

		// Token: 0x04002156 RID: 8534
		[Header("References")]
		public Rigidbody Rigidbody;

		// Token: 0x04002157 RID: 8535
		public InteractableObject IntObj;

		// Token: 0x04002158 RID: 8536
		public Transform DragOrigin;

		// Token: 0x04002159 RID: 8537
		[Header("Settings")]
		public bool CreateCoM = true;

		// Token: 0x0400215A RID: 8538
		[Range(0.5f, 2f)]
		public float HoldDistanceMultiplier = 1f;

		// Token: 0x0400215B RID: 8539
		[Range(0f, 5f)]
		public float DragForceMultiplier = 1f;

		// Token: 0x0400215C RID: 8540
		public Draggable.EInitialReplicationMode InitialReplicationMode;

		// Token: 0x0400215D RID: 8541
		private float timeSinceLastDrag = 100f;

		// Token: 0x0400215E RID: 8542
		public UnityEvent onDragStart;

		// Token: 0x0400215F RID: 8543
		public UnityEvent onDragEnd;

		// Token: 0x04002160 RID: 8544
		public UnityEvent onHovered;

		// Token: 0x04002161 RID: 8545
		public UnityEvent onInteracted;

		// Token: 0x020006BF RID: 1727
		public enum EInitialReplicationMode
		{
			// Token: 0x04002164 RID: 8548
			Off,
			// Token: 0x04002165 RID: 8549
			OnlyIfMoved,
			// Token: 0x04002166 RID: 8550
			Full
		}
	}
}
