using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.UI.Compass;
using UnityEngine;

namespace ScheduleOne.PlayerTasks
{
	// Token: 0x02000357 RID: 855
	public class Task
	{
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x0600132A RID: 4906 RVA: 0x000532B9 File Offset: 0x000514B9
		// (set) Token: 0x0600132B RID: 4907 RVA: 0x000532C1 File Offset: 0x000514C1
		public virtual string TaskName { get; protected set; }

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x0600132C RID: 4908 RVA: 0x000532CA File Offset: 0x000514CA
		// (set) Token: 0x0600132D RID: 4909 RVA: 0x000532D2 File Offset: 0x000514D2
		public string CurrentInstruction { get; protected set; } = string.Empty;

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x0600132E RID: 4910 RVA: 0x000532DB File Offset: 0x000514DB
		// (set) Token: 0x0600132F RID: 4911 RVA: 0x000532E3 File Offset: 0x000514E3
		public bool TaskActive { get; private set; }

		// Token: 0x06001330 RID: 4912 RVA: 0x000532EC File Offset: 0x000514EC
		public Task()
		{
			this.TaskActive = true;
			Singleton<TaskManager>.Instance.StartTask(this);
			Singleton<CompassManager>.Instance.SetVisible(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(this.TaskName);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0005336C File Offset: 0x0005156C
		public virtual void StopTask()
		{
			Singleton<TaskManager>.Instance.currentTask = null;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(this.TaskName);
			Singleton<CompassManager>.Instance.SetVisible(true);
			Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Default);
			this.TaskActive = false;
			if (this.clickable != null)
			{
				this.clickable.EndClick();
			}
			if (this.onTaskStop != null)
			{
				this.onTaskStop();
			}
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x000533DD File Offset: 0x000515DD
		public virtual void Success()
		{
			this.Outcome = Task.EOutcome.Success;
			this.StopTask();
			Singleton<TaskManager>.Instance.PlayTaskCompleteSound();
			if (this.onTaskSuccess != null)
			{
				this.onTaskSuccess();
			}
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x00053409 File Offset: 0x00051609
		public virtual void Fail()
		{
			this.Outcome = Task.EOutcome.Fail;
			this.StopTask();
			if (this.onTaskFail != null)
			{
				this.onTaskFail();
			}
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x0005342C File Offset: 0x0005162C
		public virtual void Update()
		{
			if (this.ClickDetectionEnabled && !this.isMultiDragging)
			{
				if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
				{
					RaycastHit hit;
					this.clickable = this.GetClickable(out hit);
					if (this.clickable != null)
					{
						this.clickable.StartClick(hit);
					}
					if (this.clickable is Draggable)
					{
						this.draggable = (this.clickable as Draggable);
						this.constraint = this.draggable.GetComponent<DraggableConstraint>();
					}
				}
				if (this.clickable != null && (!GameInput.GetButton(GameInput.ButtonCode.PrimaryClick) || !this.clickable.ClickableEnabled) && !this.forcedClickables.Contains(this.clickable))
				{
					this.clickable.EndClick();
					this.clickable = null;
					this.draggable = null;
				}
			}
			else if (this.clickable != null)
			{
				this.clickable.EndClick();
				this.clickable = null;
			}
			this.UpdateCursor();
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x00053528 File Offset: 0x00051728
		protected virtual void UpdateCursor()
		{
			if (this.draggable != null || this.isMultiDragging)
			{
				Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Grab);
				return;
			}
			RaycastHit raycastHit;
			Clickable clickable = this.GetClickable(out raycastHit);
			if (clickable != null)
			{
				Singleton<CursorManager>.Instance.SetCursorAppearance(clickable.HoveredCursor);
				return;
			}
			Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Default);
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x00053588 File Offset: 0x00051788
		public virtual void LateUpdate()
		{
			if (this.isMultiDragging)
			{
				Singleton<TaskManagerUI>.Instance.multiGrabIndicator.position = Input.mousePosition;
				Vector3 multiDragOrigin = this.GetMultiDragOrigin();
				Vector3 a = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(multiDragOrigin);
				Vector3 position = multiDragOrigin + PlayerSingleton<PlayerCamera>.Instance.transform.right * this.MultiGrabRadius;
				Vector3 b = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(position);
				float num = Vector3.Distance(a, b) / Singleton<TaskManagerUI>.Instance.canvas.scaleFactor;
				Singleton<TaskManagerUI>.Instance.multiGrabIndicator.sizeDelta = new Vector2(num * 2f, num * 2f);
				Singleton<TaskManagerUI>.Instance.multiGrabIndicator.gameObject.SetActive(true);
				return;
			}
			Singleton<TaskManagerUI>.Instance.multiGrabIndicator.gameObject.SetActive(false);
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x00053664 File Offset: 0x00051864
		private Vector3 GetMultiDragOrigin()
		{
			Ray ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
			Plane plane = new Plane(this.multiGrabProjectionPlane.forward, this.multiGrabProjectionPlane.position);
			float num;
			plane.Raycast(ray, out num);
			LayerMask layerMask = default(LayerMask) | 1 << LayerMask.NameToLayer("Default");
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(num, out raycastHit, layerMask, false, 0f))
			{
				return raycastHit.point;
			}
			return ray.GetPoint(num);
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x000536F8 File Offset: 0x000518F8
		public virtual void FixedUpdate()
		{
			this.UpdateDraggablePhysics();
			if (this.ClickDetectionEnabled && this.multiDraggingEnabled && this.multiGrabProjectionPlane != null && GameInput.GetButton(GameInput.ButtonCode.SecondaryClick) && this.draggable == null)
			{
				this.isMultiDragging = true;
				Vector3 multiDragOrigin = this.GetMultiDragOrigin();
				Collider[] array = Physics.OverlapSphere(multiDragOrigin, this.MultiGrabRadius, LayerMask.GetMask(new string[]
				{
					"Task"
				}));
				List<Draggable> list = new List<Draggable>();
				Collider[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					Draggable componentInParent = array2[i].GetComponentInParent<Draggable>();
					if (componentInParent != null && componentInParent.ClickableEnabled && componentInParent.CanBeMultiDragged)
					{
						list.Add(componentInParent);
					}
				}
				foreach (Draggable draggable in list)
				{
					if (!this.multiDragTargets.Contains(draggable))
					{
						this.multiDragTargets.Add(draggable);
						draggable.StartClick(default(RaycastHit));
						draggable.Rb.useGravity = false;
					}
					Vector3 vector = (multiDragOrigin - draggable.transform.position) * 10f * draggable.DragForceMultiplier * 1.25f;
					draggable.Rb.AddForce(vector, 5);
				}
				foreach (Draggable draggable2 in this.multiDragTargets.ToArray())
				{
					if (!list.Contains(draggable2))
					{
						this.multiDragTargets.Remove(draggable2);
						draggable2.EndClick();
						draggable2.Rb.useGravity = true;
					}
				}
				return;
			}
			this.isMultiDragging = false;
			foreach (Draggable draggable3 in this.multiDragTargets.ToArray())
			{
				this.multiDragTargets.Remove(draggable3);
				draggable3.EndClick();
				draggable3.Rb.useGravity = true;
			}
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x00053918 File Offset: 0x00051B18
		public void ForceStartClick(Clickable _clickable)
		{
			if (!this.forcedClickables.Contains(_clickable))
			{
				this.forcedClickables.Add(_clickable);
			}
			_clickable.StartClick(default(RaycastHit));
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0005394E File Offset: 0x00051B4E
		public void ForceEndClick(Clickable _clickable)
		{
			if (_clickable != null)
			{
				_clickable.EndClick();
				this.forcedClickables.Remove(_clickable);
			}
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x0005396C File Offset: 0x00051B6C
		private void UpdateDraggablePhysics()
		{
			if (this.draggable != null)
			{
				Vector3 normalized = Vector3.ProjectOnPlane(PlayerSingleton<PlayerCamera>.Instance.Camera.transform.forward, Vector3.up).normalized;
				Vector3 inNormal = (this.draggable.DragProjectionMode == Draggable.EDragProjectionMode.CameraForward) ? PlayerSingleton<PlayerCamera>.Instance.transform.forward : normalized;
				if (this.constraint != null && this.constraint.ProportionalZClamp)
				{
					inNormal = this.constraint.Container.forward;
				}
				Plane plane = new Plane(inNormal, this.draggable.originalHitPoint);
				Ray ray = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenPointToRay(Input.mousePosition);
				float distance;
				plane.Raycast(ray, out distance);
				Vector3 vector = (ray.GetPoint(distance) - this.draggable.transform.TransformPoint(this.relativeHitOffset)) * 10f * this.draggable.DragForceMultiplier;
				if (this.draggable.DragForceOrigin != null)
				{
					this.draggable.Rb.AddForceAtPosition(vector, this.draggable.DragForceOrigin.position, 5);
				}
				else
				{
					this.draggable.Rb.AddForce(vector, 5);
				}
				if (this.draggable.RotationEnabled)
				{
					float x = GameInput.MotionAxis.x;
					Vector3 a = normalized;
					this.draggable.Rb.AddTorque(a * -x * this.draggable.TorqueMultiplier, 5);
				}
				this.draggable.PostFixedUpdate();
			}
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x00053B10 File Offset: 0x00051D10
		protected virtual Clickable GetClickable(out RaycastHit hit)
		{
			LayerMask layerMask = default(LayerMask) | 1 << LayerMask.NameToLayer("Task");
			layerMask |= 1 << LayerMask.NameToLayer("Temporary");
			if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(3f, out hit, layerMask, true, this.ClickDetectionRadius))
			{
				Clickable componentInParent = hit.collider.GetComponentInParent<Clickable>();
				if (componentInParent != null)
				{
					if (!componentInParent.enabled)
					{
						return null;
					}
					if (!componentInParent.ClickableEnabled)
					{
						return null;
					}
					if (componentInParent.IsHeld)
					{
						return null;
					}
					this.hitDistance = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, hit.point);
					componentInParent.SetOriginalHitPoint(hit.point);
					if (componentInParent.AutoCalculateOffset)
					{
						this.relativeHitOffset = componentInParent.transform.InverseTransformPoint(hit.point);
						if (componentInParent.FlattenZOffset)
						{
							this.relativeHitOffset.z = 0f;
						}
					}
					else
					{
						this.relativeHitOffset = Vector3.zero;
					}
				}
				return componentInParent;
			}
			return null;
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x00053C23 File Offset: 0x00051E23
		protected void EnableMultiDragging(Transform projectionPlane, float radius = 0.08f)
		{
			this.multiDraggingEnabled = true;
			this.multiGrabProjectionPlane = projectionPlane;
			this.MultiGrabRadius = radius;
		}

		// Token: 0x0400125A RID: 4698
		public const float ClickDetectionRange = 3f;

		// Token: 0x0400125B RID: 4699
		public float ClickDetectionRadius;

		// Token: 0x0400125C RID: 4700
		protected float MultiGrabRadius = 0.08f;

		// Token: 0x0400125D RID: 4701
		public const float MultiGrabForceMultiplier = 1.25f;

		// Token: 0x04001261 RID: 4705
		public bool ClickDetectionEnabled = true;

		// Token: 0x04001262 RID: 4706
		public Task.EOutcome Outcome;

		// Token: 0x04001263 RID: 4707
		public Action onTaskSuccess;

		// Token: 0x04001264 RID: 4708
		public Action onTaskFail;

		// Token: 0x04001265 RID: 4709
		public Action onTaskStop;

		// Token: 0x04001266 RID: 4710
		protected Clickable clickable;

		// Token: 0x04001267 RID: 4711
		protected Draggable draggable;

		// Token: 0x04001268 RID: 4712
		protected DraggableConstraint constraint;

		// Token: 0x04001269 RID: 4713
		protected float hitDistance;

		// Token: 0x0400126A RID: 4714
		protected Vector3 relativeHitOffset = Vector3.zero;

		// Token: 0x0400126B RID: 4715
		private bool multiDraggingEnabled;

		// Token: 0x0400126C RID: 4716
		private Transform multiGrabProjectionPlane;

		// Token: 0x0400126D RID: 4717
		private List<Draggable> multiDragTargets = new List<Draggable>();

		// Token: 0x0400126E RID: 4718
		private bool isMultiDragging;

		// Token: 0x0400126F RID: 4719
		private List<Clickable> forcedClickables = new List<Clickable>();

		// Token: 0x02000358 RID: 856
		public enum EOutcome
		{
			// Token: 0x04001271 RID: 4721
			Cancelled,
			// Token: 0x04001272 RID: 4722
			Success,
			// Token: 0x04001273 RID: 4723
			Fail
		}
	}
}
