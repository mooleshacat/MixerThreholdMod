using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.Lighting;
using ScheduleOne.PlayerScripts;
using ScheduleOne.ScriptableObjects;
using ScheduleOne.UI.Phone;
using UnityEngine;

namespace ScheduleOne.Calling
{
	// Token: 0x020007B7 RID: 1975
	public class PayPhone : MonoBehaviour
	{
		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x060035BD RID: 13757 RVA: 0x000E0F80 File Offset: 0x000DF180
		public PhoneCallData QueuedCall
		{
			get
			{
				return Singleton<CallManager>.Instance.QueuedCallData;
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x060035BE RID: 13758 RVA: 0x000E0F8C File Offset: 0x000DF18C
		public PhoneCallData ActiveCall
		{
			get
			{
				return Singleton<CallInterface>.Instance.ActiveCallData;
			}
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x000E0F98 File Offset: 0x000DF198
		public void FixedUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			this.timeSinceLastRing += Time.fixedDeltaTime;
			float num = Vector3.SqrMagnitude(PlayerSingleton<PlayerCamera>.Instance.transform.position - base.transform.position);
			this.Light.IsOn = (this.QueuedCall != null && this.ActiveCall == null);
			if (num < 81f && this.QueuedCall != null && this.timeSinceLastRing >= 4f && this.ActiveCall == null)
			{
				this.timeSinceLastRing = 0f;
				this.RingSound.Play();
			}
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x000E1051 File Offset: 0x000DF251
		public void Hovered()
		{
			if (this.CanInteract())
			{
				this.IntObj.SetMessage("Answer phone");
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x000E1084 File Offset: 0x000DF284
		public void Interacted()
		{
			if (!this.CanInteract())
			{
				return;
			}
			Singleton<CallInterface>.Instance.StartCall(this.QueuedCall, this.QueuedCall.CallerID, 0);
			this.RingSound.Stop();
			this.AnswerSound.Play();
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(this.CameraPosition.position, this.CameraPosition.rotation, 0.2f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(60f, 0.2f);
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x000E1106 File Offset: 0x000DF306
		private bool CanInteract()
		{
			return !(this.QueuedCall == null) && !(this.ActiveCall != null) && !Singleton<CallInterface>.Instance.IsOpen;
		}

		// Token: 0x04002613 RID: 9747
		public const float RING_INTERVAL = 4f;

		// Token: 0x04002614 RID: 9748
		public const float RING_RANGE = 9f;

		// Token: 0x04002615 RID: 9749
		public BlinkingLight Light;

		// Token: 0x04002616 RID: 9750
		public AudioSourceController RingSound;

		// Token: 0x04002617 RID: 9751
		public AudioSourceController AnswerSound;

		// Token: 0x04002618 RID: 9752
		public InteractableObject IntObj;

		// Token: 0x04002619 RID: 9753
		public Transform CameraPosition;

		// Token: 0x0400261A RID: 9754
		private float timeSinceLastRing = 100f;

		// Token: 0x0400261B RID: 9755
		private const float ringRangeSquared = 81f;
	}
}
