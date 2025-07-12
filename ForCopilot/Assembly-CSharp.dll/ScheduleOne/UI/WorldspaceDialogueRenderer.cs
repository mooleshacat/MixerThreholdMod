using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A9B RID: 2715
	public class WorldspaceDialogueRenderer : MonoBehaviour
	{
		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x060048E8 RID: 18664 RVA: 0x00131B08 File Offset: 0x0012FD08
		// (set) Token: 0x060048E9 RID: 18665 RVA: 0x00131B10 File Offset: 0x0012FD10
		public string ShownText { get; protected set; } = string.Empty;

		// Token: 0x060048EA RID: 18666 RVA: 0x00131B19 File Offset: 0x0012FD19
		private void Awake()
		{
			this.localOffset = base.transform.localPosition;
			this.SetOpacity(0f);
		}

		// Token: 0x060048EB RID: 18667 RVA: 0x00131B38 File Offset: 0x0012FD38
		private void FixedUpdate()
		{
			if (this.ShownText == string.Empty)
			{
				if (this.CurrentOpacity != 0f)
				{
					this.SetOpacity(0f);
				}
				return;
			}
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				if (this.CurrentOpacity != 0f)
				{
					this.SetOpacity(0f);
				}
				return;
			}
			if (Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position) > this.MaxRange)
			{
				if (this.CurrentOpacity != 0f)
				{
					this.SetOpacity(0f);
				}
				return;
			}
			float num = Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position);
			if (num < this.MaxRange - 2f)
			{
				this.SetOpacity(1f);
			}
			else
			{
				this.SetOpacity(1f - (num - (this.MaxRange - 2f)) / 2f);
			}
			this.Text.text = this.ShownText;
		}

		// Token: 0x060048EC RID: 18668 RVA: 0x00131C3D File Offset: 0x0012FE3D
		private void LateUpdate()
		{
			if (this.CurrentOpacity > 0f)
			{
				this.UpdatePosition();
			}
		}

		// Token: 0x060048ED RID: 18669 RVA: 0x00131C54 File Offset: 0x0012FE54
		private void UpdatePosition()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			float num = this.BaseScale * this.Scale.Evaluate(Vector3.Distance(base.transform.position, PlayerSingleton<PlayerCamera>.Instance.transform.position) / this.MaxRange);
			this.Canvas.transform.localScale = new Vector3(num, num, num);
			this.Background.sizeDelta = new Vector2(this.Text.renderedWidth + this.Padding.x, this.Text.renderedHeight + this.Padding.y);
			this.Canvas.transform.LookAt(PlayerSingleton<PlayerCamera>.Instance.transform.position);
			base.transform.localPosition = this.localOffset;
			base.transform.position = base.transform.position + this.WorldSpaceOffset;
		}

		// Token: 0x060048EE RID: 18670 RVA: 0x00131D4C File Offset: 0x0012FF4C
		public void ShowText(string text, float duration = 0f)
		{
			if (this.hideCoroutine != null)
			{
				base.StopCoroutine(this.hideCoroutine);
				this.hideCoroutine = null;
			}
			this.ShownText = text;
			if (this.ShownText != string.Empty)
			{
				this.Text.text = this.ShownText;
				this.Text.ForceMeshUpdate(false, false);
				this.UpdatePosition();
			}
			if (!this.Canvas.enabled && this.Anim != null)
			{
				this.Anim.Play();
			}
			if (duration > 0f)
			{
				this.hideCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<ShowText>g__Wait|22_0(duration));
			}
		}

		// Token: 0x060048EF RID: 18671 RVA: 0x00131DF7 File Offset: 0x0012FFF7
		public void HideText()
		{
			if (this.hideCoroutine != null)
			{
				base.StopCoroutine(this.hideCoroutine);
				this.hideCoroutine = null;
			}
			this.ShownText = string.Empty;
		}

		// Token: 0x060048F0 RID: 18672 RVA: 0x00131E1F File Offset: 0x0013001F
		private void SetOpacity(float op)
		{
			this.CurrentOpacity = op;
			this.CanvasGroup.alpha = op;
			this.Canvas.enabled = (op > 0f);
		}

		// Token: 0x060048F2 RID: 18674 RVA: 0x00131E86 File Offset: 0x00130086
		[CompilerGenerated]
		private IEnumerator <ShowText>g__Wait|22_0(float dur)
		{
			yield return new WaitForSeconds(dur);
			this.ShownText = string.Empty;
			this.hideCoroutine = null;
			yield break;
		}

		// Token: 0x04003585 RID: 13701
		private const float FadeDist = 2f;

		// Token: 0x04003587 RID: 13703
		[Header("Settings")]
		public float MaxRange = 10f;

		// Token: 0x04003588 RID: 13704
		public float BaseScale = 0.01f;

		// Token: 0x04003589 RID: 13705
		public AnimationCurve Scale;

		// Token: 0x0400358A RID: 13706
		public Vector2 Padding;

		// Token: 0x0400358B RID: 13707
		public Vector3 WorldSpaceOffset = Vector3.zero;

		// Token: 0x0400358C RID: 13708
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x0400358D RID: 13709
		public CanvasGroup CanvasGroup;

		// Token: 0x0400358E RID: 13710
		public RectTransform Background;

		// Token: 0x0400358F RID: 13711
		public TextMeshProUGUI Text;

		// Token: 0x04003590 RID: 13712
		public Animation Anim;

		// Token: 0x04003591 RID: 13713
		private Vector3 localOffset = Vector3.zero;

		// Token: 0x04003592 RID: 13714
		private float CurrentOpacity;

		// Token: 0x04003593 RID: 13715
		private Coroutine hideCoroutine;
	}
}
