using System;
using System.Collections;
using ScheduleOne.DevUtilities;
using ScheduleOne.Misc;
using ScheduleOne.PlayerTasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000C30 RID: 3120
	public class LabOvenButton : MonoBehaviour
	{
		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x0600564C RID: 22092 RVA: 0x0016D592 File Offset: 0x0016B792
		// (set) Token: 0x0600564D RID: 22093 RVA: 0x0016D59A File Offset: 0x0016B79A
		public bool Pressed { get; private set; }

		// Token: 0x0600564E RID: 22094 RVA: 0x0016D5A3 File Offset: 0x0016B7A3
		private void Start()
		{
			this.SetInteractable(false);
			this.Clickable.onClickStart.AddListener(new UnityAction<RaycastHit>(this.Press));
		}

		// Token: 0x0600564F RID: 22095 RVA: 0x0016D5C8 File Offset: 0x0016B7C8
		public void SetInteractable(bool interactable)
		{
			this.Clickable.ClickableEnabled = interactable;
		}

		// Token: 0x06005650 RID: 22096 RVA: 0x0016D5D6 File Offset: 0x0016B7D6
		public void Press(RaycastHit hit)
		{
			this.SetPressed(true);
		}

		// Token: 0x06005651 RID: 22097 RVA: 0x0016D5E0 File Offset: 0x0016B7E0
		public void SetPressed(bool pressed)
		{
			if (this.Pressed == pressed)
			{
				return;
			}
			this.Pressed = pressed;
			this.Light.isOn = pressed;
			if (this.Pressed)
			{
				if (this.pressCoroutine != null)
				{
					base.StopCoroutine(this.pressCoroutine);
				}
				this.pressCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.MoveButton(this.PressedTransform));
				return;
			}
			if (this.pressCoroutine != null)
			{
				base.StopCoroutine(this.pressCoroutine);
			}
			this.pressCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.MoveButton(this.DepressedTransform));
		}

		// Token: 0x06005652 RID: 22098 RVA: 0x0016D673 File Offset: 0x0016B873
		private IEnumerator MoveButton(Transform destination)
		{
			Vector3 startPos = this.Button.localPosition;
			Vector3 endPos = destination.localPosition;
			float lerpTime = 0.2f;
			for (float t = 0f; t < lerpTime; t += Time.deltaTime)
			{
				this.Button.localPosition = Vector3.Lerp(startPos, endPos, t / lerpTime);
				yield return null;
			}
			this.Button.localPosition = endPos;
			this.pressCoroutine = null;
			yield break;
		}

		// Token: 0x04003FC5 RID: 16325
		public Transform Button;

		// Token: 0x04003FC6 RID: 16326
		public Transform PressedTransform;

		// Token: 0x04003FC7 RID: 16327
		public Transform DepressedTransform;

		// Token: 0x04003FC8 RID: 16328
		public ToggleableLight Light;

		// Token: 0x04003FC9 RID: 16329
		public Clickable Clickable;

		// Token: 0x04003FCA RID: 16330
		private Coroutine pressCoroutine;
	}
}
