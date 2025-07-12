using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Interaction
{
	// Token: 0x0200064A RID: 1610
	public class InteractableObject : MonoBehaviour
	{
		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x060029C3 RID: 10691 RVA: 0x000ACA60 File Offset: 0x000AAC60
		public InteractableObject.EInteractionType _interactionType
		{
			get
			{
				return this.interactionType;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x060029C4 RID: 10692 RVA: 0x000ACA68 File Offset: 0x000AAC68
		public InteractableObject.EInteractableState _interactionState
		{
			get
			{
				return this.interactionState;
			}
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x000ACA70 File Offset: 0x000AAC70
		public void SetInteractionType(InteractableObject.EInteractionType type)
		{
			this.interactionType = type;
		}

		// Token: 0x060029C6 RID: 10694 RVA: 0x000ACA79 File Offset: 0x000AAC79
		public void SetInteractableState(InteractableObject.EInteractableState state)
		{
			this.interactionState = state;
		}

		// Token: 0x060029C7 RID: 10695 RVA: 0x000ACA82 File Offset: 0x000AAC82
		public void SetMessage(string _message)
		{
			this.message = _message;
		}

		// Token: 0x060029C8 RID: 10696 RVA: 0x000ACA8B File Offset: 0x000AAC8B
		public virtual void Hovered()
		{
			if (this.onHovered != null)
			{
				this.onHovered.Invoke();
			}
			if (this.interactionState != InteractableObject.EInteractableState.Disabled)
			{
				this.ShowMessage();
			}
		}

		// Token: 0x060029C9 RID: 10697 RVA: 0x000ACAAF File Offset: 0x000AACAF
		public virtual void StartInteract()
		{
			if (this.interactionState == InteractableObject.EInteractableState.Invalid)
			{
				return;
			}
			if (this.onInteractStart != null)
			{
				this.onInteractStart.Invoke();
			}
			Singleton<InteractionManager>.Instance.LerpDisplayScale(0.9f);
		}

		// Token: 0x060029CA RID: 10698 RVA: 0x000ACADD File Offset: 0x000AACDD
		public virtual void EndInteract()
		{
			if (this.onInteractEnd != null)
			{
				this.onInteractEnd.Invoke();
			}
			Singleton<InteractionManager>.Instance.LerpDisplayScale(1f);
		}

		// Token: 0x060029CB RID: 10699 RVA: 0x000ACB04 File Offset: 0x000AAD04
		protected virtual void ShowMessage()
		{
			Vector3 pos = base.transform.position;
			if (this.displayLocationCollider != null)
			{
				pos = this.displayLocationCollider.ClosestPoint(PlayerSingleton<PlayerCamera>.Instance.transform.position);
			}
			else if (this.displayLocationPoint != null)
			{
				pos = this.displayLocationPoint.position;
			}
			Sprite icon = null;
			string spriteText = string.Empty;
			Color iconColor = Color.white;
			Color messageColor = Color.white;
			switch (this.interactionState)
			{
			case InteractableObject.EInteractableState.Default:
			{
				messageColor = Singleton<InteractionManager>.Instance.messageColor_Default;
				InteractableObject.EInteractionType einteractionType = this.interactionType;
				if (einteractionType != InteractableObject.EInteractionType.Key_Press)
				{
					if (einteractionType != InteractableObject.EInteractionType.LeftMouse_Click)
					{
						Console.LogWarning("EInteractionType not accounted for!", null);
					}
					else
					{
						icon = Singleton<InteractionManager>.Instance.icon_LeftMouse;
						iconColor = Singleton<InteractionManager>.Instance.iconColor_Default;
					}
				}
				else
				{
					icon = Singleton<InteractionManager>.Instance.icon_Key;
					spriteText = Singleton<InteractionManager>.Instance.InteractKey;
					iconColor = Singleton<InteractionManager>.Instance.iconColor_Default_Key;
				}
				break;
			}
			case InteractableObject.EInteractableState.Invalid:
				icon = Singleton<InteractionManager>.Instance.icon_Cross;
				iconColor = Singleton<InteractionManager>.Instance.iconColor_Invalid;
				messageColor = Singleton<InteractionManager>.Instance.messageColor_Invalid;
				break;
			case InteractableObject.EInteractableState.Disabled:
				return;
			case InteractableObject.EInteractableState.Label:
				icon = null;
				messageColor = Singleton<InteractionManager>.Instance.messageColor_Default;
				break;
			default:
				Console.LogWarning("EInteractableState not accounted for!", null);
				return;
			}
			Singleton<InteractionManager>.Instance.EnableInteractionDisplay(pos, icon, spriteText, this.message, messageColor, iconColor);
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x000ACC5C File Offset: 0x000AAE5C
		public bool CheckAngleLimit(Vector3 interactionSource)
		{
			if (!this.LimitInteractionAngle)
			{
				return true;
			}
			Vector3 normalized = (interactionSource - base.transform.position).normalized;
			return Mathf.Abs(Vector3.SignedAngle(base.transform.forward, normalized, Vector3.up)) < this.AngleLimit;
		}

		// Token: 0x04001E48 RID: 7752
		[Header("Settings")]
		[SerializeField]
		protected string message = "<Message>";

		// Token: 0x04001E49 RID: 7753
		[SerializeField]
		protected InteractableObject.EInteractionType interactionType;

		// Token: 0x04001E4A RID: 7754
		[SerializeField]
		protected InteractableObject.EInteractableState interactionState;

		// Token: 0x04001E4B RID: 7755
		public float MaxInteractionRange = 5f;

		// Token: 0x04001E4C RID: 7756
		public bool RequiresUniqueClick = true;

		// Token: 0x04001E4D RID: 7757
		public int Priority;

		// Token: 0x04001E4E RID: 7758
		[SerializeField]
		protected Collider displayLocationCollider;

		// Token: 0x04001E4F RID: 7759
		public Transform displayLocationPoint;

		// Token: 0x04001E50 RID: 7760
		[Header("Angle Limits")]
		public bool LimitInteractionAngle;

		// Token: 0x04001E51 RID: 7761
		public float AngleLimit = 90f;

		// Token: 0x04001E52 RID: 7762
		[Header("Events")]
		public UnityEvent onHovered = new UnityEvent();

		// Token: 0x04001E53 RID: 7763
		public UnityEvent onInteractStart = new UnityEvent();

		// Token: 0x04001E54 RID: 7764
		public UnityEvent onInteractEnd = new UnityEvent();

		// Token: 0x0200064B RID: 1611
		public enum EInteractionType
		{
			// Token: 0x04001E56 RID: 7766
			Key_Press,
			// Token: 0x04001E57 RID: 7767
			LeftMouse_Click
		}

		// Token: 0x0200064C RID: 1612
		public enum EInteractableState
		{
			// Token: 0x04001E59 RID: 7769
			Default,
			// Token: 0x04001E5A RID: 7770
			Invalid,
			// Token: 0x04001E5B RID: 7771
			Disabled,
			// Token: 0x04001E5C RID: 7772
			Label
		}
	}
}
