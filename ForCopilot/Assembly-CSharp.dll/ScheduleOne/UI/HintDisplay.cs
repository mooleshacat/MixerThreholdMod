using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI.Input;
using ScheduleOne.UI.Phone;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScheduleOne.UI
{
	// Token: 0x02000A2F RID: 2607
	public class HintDisplay : Singleton<HintDisplay>
	{
		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x0600461F RID: 17951 RVA: 0x0012653E File Offset: 0x0012473E
		// (set) Token: 0x06004620 RID: 17952 RVA: 0x00126546 File Offset: 0x00124746
		public bool IsOpen { get; protected set; }

		// Token: 0x06004621 RID: 17953 RVA: 0x0012654F File Offset: 0x0012474F
		protected override void Start()
		{
			base.Start();
			this.Group.alpha = 0f;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x00126578 File Offset: 0x00124778
		public void Update()
		{
			if (!this.IsOpen)
			{
				if (this.hintQueue.Count > 0 && this.Group.alpha == 0f)
				{
					this.ShowHint(this.hintQueue[0].Text, this.hintQueue[0].Duration);
					this.hintQueue.RemoveAt(0);
				}
				return;
			}
			this.timeSinceOpened += Time.deltaTime;
			if (Singleton<CallInterface>.Instance.IsOpen)
			{
				this.Hide();
			}
			this.DismissPrompt.SetLabel((this.hintQueue.Count > 0) ? "Next" : "Dismiss");
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Submit) && !GameInput.IsTyping && this.timeSinceOpened > 0.1f)
			{
				this.Hide();
			}
		}

		// Token: 0x06004623 RID: 17955 RVA: 0x0012664E File Offset: 0x0012484E
		public void ShowHint_10s(string text)
		{
			this.ShowHint(text, 10f);
		}

		// Token: 0x06004624 RID: 17956 RVA: 0x0012665C File Offset: 0x0012485C
		public void ShowHint_20s(string text)
		{
			this.ShowHint(text, 20f);
		}

		// Token: 0x06004625 RID: 17957 RVA: 0x0012666A File Offset: 0x0012486A
		public void ShowHint(string text)
		{
			this.ShowHint(text, 0f);
		}

		// Token: 0x06004626 RID: 17958 RVA: 0x00126678 File Offset: 0x00124878
		public void ShowHint(string text, float autoCloseTime = 0f)
		{
			text = this.ProcessText(text);
			Console.Log("Showing hint: " + text, null);
			this.timeSinceOpened = 0f;
			this.SetAlpha(1f);
			this.FlashAnim.Play();
			this.Label.text = text;
			this.Label.ForceMeshUpdate(false, false);
			this.Container.sizeDelta = new Vector2(this.Label.renderedWidth + this.Padding.x, this.Label.renderedHeight + this.Padding.y);
			this.Container.anchoredPosition = new Vector2(-this.Container.sizeDelta.x / 2f - this.Offset.x, -this.Container.sizeDelta.y / 2f + this.Offset.y);
			if (this.autoCloseRoutine != null)
			{
				base.StopCoroutine(this.autoCloseRoutine);
			}
			if (autoCloseTime > 0f)
			{
				this.autoCloseRoutine = base.StartCoroutine(this.<ShowHint>g__AutoClose|22_0(autoCloseTime));
			}
			this.IsOpen = true;
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x001267A4 File Offset: 0x001249A4
		public void Hide()
		{
			this.SetAlpha(0f);
			this.IsOpen = false;
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x001267B8 File Offset: 0x001249B8
		private void SetAlpha(float alpha)
		{
			HintDisplay.<>c__DisplayClass24_0 CS$<>8__locals1 = new HintDisplay.<>c__DisplayClass24_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.alpha = alpha;
			if (CS$<>8__locals1.alpha > 0f)
			{
				this.Container.gameObject.SetActive(true);
			}
			if (this.fadeRoutine != null)
			{
				base.StopCoroutine(this.fadeRoutine);
			}
			this.fadeRoutine = base.StartCoroutine(CS$<>8__locals1.<SetAlpha>g__Routine|0());
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x0012681D File Offset: 0x00124A1D
		public void QueueHint_10s(string message)
		{
			this.hintQueue.Add(new HintDisplay.Hint(message, 10f));
		}

		// Token: 0x0600462A RID: 17962 RVA: 0x00126835 File Offset: 0x00124A35
		public void QueueHint_20s(string message)
		{
			this.hintQueue.Add(new HintDisplay.Hint(message, 20f));
		}

		// Token: 0x0600462B RID: 17963 RVA: 0x0012684D File Offset: 0x00124A4D
		public void QueueHint(string message, float time)
		{
			this.hintQueue.Add(new HintDisplay.Hint(message, time));
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x00126864 File Offset: 0x00124A64
		private string ProcessText(string text)
		{
			string pattern = "<Input_([a-zA-Z0-9]+)>";
			MatchEvaluator evaluator = delegate(Match match)
			{
				GameInput.ButtonCode code;
				if (Enum.TryParse<GameInput.ButtonCode>(match.Groups[1].Value, out code))
				{
					string text2;
					string controlPath;
					InputActionRebindingExtensions.GetBindingDisplayString(Singleton<GameInput>.Instance.GetAction(code), 0, ref text2, ref controlPath, 0);
					string displayNameForControlPath = Singleton<InputPromptsManager>.Instance.GetDisplayNameForControlPath(controlPath);
					return "<color=#88CBFF>" + displayNameForControlPath + "</color>";
				}
				return match.Value;
			};
			return Regex.Replace(text, pattern, evaluator).Replace("<h1>", "<color=#88CBFF>").Replace("<h2>", "<color=#F86266>").Replace("<h3>", "<color=#46CB4F>").Replace("</h>", "</color>");
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x001268EE File Offset: 0x00124AEE
		[CompilerGenerated]
		private IEnumerator <ShowHint>g__AutoClose|22_0(float time)
		{
			yield return new WaitForSeconds(time);
			this.Hide();
			this.autoCloseRoutine = null;
			yield break;
		}

		// Token: 0x040032D1 RID: 13009
		public const float FadeTime = 0.3f;

		// Token: 0x040032D3 RID: 13011
		[Header("References")]
		public RectTransform Container;

		// Token: 0x040032D4 RID: 13012
		public TextMeshProUGUI Label;

		// Token: 0x040032D5 RID: 13013
		public CanvasGroup Group;

		// Token: 0x040032D6 RID: 13014
		public InputPrompt DismissPrompt;

		// Token: 0x040032D7 RID: 13015
		public Animation FlashAnim;

		// Token: 0x040032D8 RID: 13016
		[Header("Settings")]
		public Vector2 Padding;

		// Token: 0x040032D9 RID: 13017
		public Vector2 Offset;

		// Token: 0x040032DA RID: 13018
		private Coroutine autoCloseRoutine;

		// Token: 0x040032DB RID: 13019
		private Coroutine fadeRoutine;

		// Token: 0x040032DC RID: 13020
		private List<HintDisplay.Hint> hintQueue = new List<HintDisplay.Hint>();

		// Token: 0x040032DD RID: 13021
		private float timeSinceOpened;

		// Token: 0x02000A30 RID: 2608
		private class Hint
		{
			// Token: 0x0600462F RID: 17967 RVA: 0x00126904 File Offset: 0x00124B04
			public Hint(string text, float duration)
			{
				this.Text = text;
				this.Duration = duration;
			}

			// Token: 0x040032DE RID: 13022
			public string Text;

			// Token: 0x040032DF RID: 13023
			public float Duration;
		}
	}
}
