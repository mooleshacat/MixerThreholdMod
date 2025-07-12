using System;
using System.Text.RegularExpressions;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.ScriptableObjects;
using ScheduleOne.UI.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000AE0 RID: 2784
	public class CallInterface : Singleton<CallInterface>
	{
		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06004AAA RID: 19114 RVA: 0x00139AF0 File Offset: 0x00137CF0
		// (set) Token: 0x06004AAB RID: 19115 RVA: 0x00139AF8 File Offset: 0x00137CF8
		public PhoneCallData ActiveCallData { get; private set; }

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06004AAC RID: 19116 RVA: 0x00139B01 File Offset: 0x00137D01
		// (set) Token: 0x06004AAD RID: 19117 RVA: 0x00139B09 File Offset: 0x00137D09
		public bool IsOpen { get; protected set; }

		// Token: 0x06004AAE RID: 19118 RVA: 0x00139B14 File Offset: 0x00137D14
		protected override void Awake()
		{
			base.Awake();
			this.highlight1Hex = ColorUtility.ToHtmlStringRGB(this.Highlight1Color);
			this.ContinuePrompt.gameObject.SetActive(false);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 1);
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x06004AAF RID: 19119 RVA: 0x00139B78 File Offset: 0x00137D78
		private void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Submit) || GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) || GameInput.GetButtonDown(GameInput.ButtonCode.Jump))
			{
				if (this.rolloutRoutine != null)
				{
					this.skipRollout = true;
					return;
				}
				this.Continue();
			}
		}

		// Token: 0x06004AB0 RID: 19120 RVA: 0x00139BB2 File Offset: 0x00137DB2
		private void Exit(ExitAction exit)
		{
			if (exit.Used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (exit.exitType == ExitType.Escape)
			{
				exit.Used = true;
				this.Close();
			}
		}

		// Token: 0x06004AB1 RID: 19121 RVA: 0x00139BDC File Offset: 0x00137DDC
		public void StartCall(PhoneCallData data, CallerID caller, int startStage = 0)
		{
			if (this.IsOpen)
			{
				Debug.LogWarning("CallInterface: There is already a call in progress; existing call will be forced complete");
				for (int i = this.currentCallStage; i < this.ActiveCallData.Stages.Length; i++)
				{
					if (i > this.currentCallStage)
					{
						this.ActiveCallData.Stages[i].OnStageStart();
					}
					this.ActiveCallData.Stages[i].OnStageEnd();
				}
				this.ActiveCallData.Completed();
			}
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0.2f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			this.ActiveCallData = data;
			this.IsOpen = true;
			this.ProfilePicture.sprite = caller.ProfilePicture;
			this.MainText.text = string.Empty;
			this.NameLabel.text = caller.Name;
			this.currentCallStage = startStage;
			this.SetIsVisible(true);
			this.ShowStage(0, 0.25f);
		}

		// Token: 0x06004AB2 RID: 19122 RVA: 0x00139D04 File Offset: 0x00137F04
		public void EndCall()
		{
			if (!this.IsOpen)
			{
				Debug.LogWarning("CallInterface: Attempted to end a call while no call was in progress.");
				return;
			}
			if (this.ActiveCallData != null)
			{
				this.ActiveCallData.Completed();
			}
			if (this.CallCompleted != null)
			{
				this.CallCompleted(this.ActiveCallData);
			}
			this.Close();
		}

		// Token: 0x06004AB3 RID: 19123 RVA: 0x00139D5C File Offset: 0x00137F5C
		private void Close()
		{
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.2f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.2f);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0.2f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			if (this.rolloutRoutine != null)
			{
				base.StopCoroutine(this.rolloutRoutine);
				this.rolloutRoutine = null;
			}
			this.ActiveCallData = null;
			this.IsOpen = false;
			this.SetIsVisible(false);
		}

		// Token: 0x06004AB4 RID: 19124 RVA: 0x00139E10 File Offset: 0x00138010
		public void Continue()
		{
			if (this.currentCallStage != -1)
			{
				this.ActiveCallData.Stages[this.currentCallStage].OnStageEnd();
			}
			if (this.currentCallStage == this.ActiveCallData.Stages.Length - 1)
			{
				this.EndCall();
				return;
			}
			this.ShowStage(this.currentCallStage + 1, 0f);
		}

		// Token: 0x06004AB5 RID: 19125 RVA: 0x00139E70 File Offset: 0x00138070
		private void ShowStage(int stageIndex, float initialDelay = 0f)
		{
			CallInterface.<>c__DisplayClass32_0 CS$<>8__locals1 = new CallInterface.<>c__DisplayClass32_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.initialDelay = initialDelay;
			CS$<>8__locals1.stageIndex = stageIndex;
			this.currentCallStage = CS$<>8__locals1.stageIndex;
			this.ActiveCallData.Stages[CS$<>8__locals1.stageIndex].OnStageStart();
			this.rolloutRoutine = base.StartCoroutine(CS$<>8__locals1.<ShowStage>g__Routine|0());
		}

		// Token: 0x06004AB6 RID: 19126 RVA: 0x00139ED0 File Offset: 0x001380D0
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
					return string.Concat(new string[]
					{
						"<color=#",
						this.highlight1Hex,
						">",
						displayNameForControlPath,
						"</color>"
					});
				}
				return match.Value;
			};
			return Regex.Replace(text, pattern, evaluator).Replace("<h1>", "<color=#" + this.highlight1Hex + ">").Replace("</h>", "</color>");
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x00139F28 File Offset: 0x00138128
		private string GetVisibleText(int charactersShown, string fullText)
		{
			bool flag = false;
			string text = fullText.Substring(0, charactersShown);
			char[] array = text.ToCharArray();
			if ((array[charactersShown - 1] != '<' && !flag) || array[charactersShown - 1] == '>')
			{
			}
			return text;
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x00139F64 File Offset: 0x00138164
		private void SetIsVisible(bool visible)
		{
			if (this.slideRoutine != null)
			{
				base.StopCoroutine(this.slideRoutine);
			}
			if (visible)
			{
				this.CanvasGroup.alpha = 0f;
				this.Canvas.enabled = true;
				this.Container.gameObject.SetActive(true);
				this.OpenAnim.Play();
				return;
			}
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x04003702 RID: 14082
		public const float TIME_PER_CHAR = 0.015f;

		// Token: 0x04003705 RID: 14085
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003706 RID: 14086
		public RectTransform Container;

		// Token: 0x04003707 RID: 14087
		public Image ProfilePicture;

		// Token: 0x04003708 RID: 14088
		public TextMeshProUGUI NameLabel;

		// Token: 0x04003709 RID: 14089
		public TextMeshProUGUI MainText;

		// Token: 0x0400370A RID: 14090
		public RectTransform ContinuePrompt;

		// Token: 0x0400370B RID: 14091
		public Animation OpenAnim;

		// Token: 0x0400370C RID: 14092
		public AudioSourceController TypewriterEffectSound;

		// Token: 0x0400370D RID: 14093
		public CanvasGroup CanvasGroup;

		// Token: 0x0400370E RID: 14094
		[Header("Settings")]
		public Color Highlight1Color;

		// Token: 0x0400370F RID: 14095
		private int currentCallStage = -1;

		// Token: 0x04003710 RID: 14096
		private Coroutine slideRoutine;

		// Token: 0x04003711 RID: 14097
		private bool skipRollout;

		// Token: 0x04003712 RID: 14098
		private Coroutine rolloutRoutine;

		// Token: 0x04003713 RID: 14099
		private string highlight1Hex;

		// Token: 0x04003714 RID: 14100
		public Action<PhoneCallData> CallCompleted;
	}
}
