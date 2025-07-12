using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A1A RID: 2586
	public class DialogueCanvas : Singleton<DialogueCanvas>
	{
		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06004597 RID: 17815 RVA: 0x001240E7 File Offset: 0x001222E7
		public bool isActive
		{
			get
			{
				return this.currentHandler != null;
			}
		}

		// Token: 0x06004598 RID: 17816 RVA: 0x001240F5 File Offset: 0x001222F5
		protected override void Awake()
		{
			base.Awake();
			this.canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 0);
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x0012412C File Offset: 0x0012232C
		public void DisplayDialogueNode(DialogueHandler diag, DialogueNodeData node, string dialogueText, List<string> choices)
		{
			if (diag != this.currentHandler)
			{
				this.StartDialogue(diag);
			}
			if (this.dialogueRollout != null)
			{
				base.StopCoroutine(this.dialogueRollout);
			}
			this.currentNode = node;
			this.dialogueRollout = base.StartCoroutine(this.RolloutDialogue(dialogueText, choices));
		}

		// Token: 0x0600459A RID: 17818 RVA: 0x00124180 File Offset: 0x00122380
		public void OverrideText(string text)
		{
			this.overrideText = text;
			if (this.dialogueRollout != null)
			{
				base.StopCoroutine(this.dialogueRollout);
			}
			this.dialogueText.text = this.overrideText;
			this.canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x001241D6 File Offset: 0x001223D6
		public void StopTextOverride()
		{
			this.overrideText = string.Empty;
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x001241E3 File Offset: 0x001223E3
		private void Update()
		{
			if (this.isActive)
			{
				if (Input.GetKeyDown(KeyCode.Space))
				{
					this.spaceDownThisFrame = true;
				}
				else
				{
					this.spaceDownThisFrame = false;
				}
				if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
				{
					this.leftClickThisFrame = true;
					return;
				}
				this.leftClickThisFrame = false;
			}
		}

		// Token: 0x0600459D RID: 17821 RVA: 0x0012421D File Offset: 0x0012241D
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.isActive)
			{
				return;
			}
			if (action.exitType != ExitType.Escape)
			{
				return;
			}
			if (!DialogueHandler.activeDialogue.AllowExit)
			{
				return;
			}
			action.Used = true;
			this.currentHandler.EndDialogue();
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x0012425A File Offset: 0x0012245A
		protected IEnumerator RolloutDialogue(string text, List<string> choices)
		{
			List<int> activeDialogueChoices = new List<int>();
			this.dialogueText.maxVisibleCharacters = 0;
			this.dialogueText.text = text;
			this.canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			float rolloutTime = (float)text.Length * 0.015f;
			if (this.SkipNextRollout)
			{
				this.SkipNextRollout = false;
				rolloutTime = 0f;
			}
			float i = 0f;
			while (i < rolloutTime && !this.spaceDownThisFrame && !this.leftClickThisFrame)
			{
				int maxVisibleCharacters = (int)(i / 0.015f);
				this.dialogueText.maxVisibleCharacters = maxVisibleCharacters;
				yield return new WaitForEndOfFrame();
				i += Time.deltaTime;
			}
			this.dialogueText.maxVisibleCharacters = text.Length;
			this.spaceDownThisFrame = false;
			this.leftClickThisFrame = false;
			this.hasChoiceBeenSelected = false;
			if (this.choiceSelectionResidualCoroutine != null)
			{
				base.StopCoroutine(this.choiceSelectionResidualCoroutine);
			}
			this.continuePopup.gameObject.SetActive(false);
			for (int j = 0; j < this.dialogueChoices.Count; j++)
			{
				this.dialogueChoices[j].gameObject.SetActive(false);
				this.dialogueChoices[j].canvasGroup.alpha = 1f;
				if (choices.Count > j)
				{
					this.dialogueChoices[j].text.text = choices[j];
					this.dialogueChoices[j].button.interactable = true;
					string empty = string.Empty;
					if (this.IsChoiceValid(j, out empty))
					{
						this.dialogueChoices[j].notPossibleGameObject.SetActive(false);
						this.dialogueChoices[j].button.interactable = true;
						ColorBlock colors = this.dialogueChoices[j].button.colors;
						colors.disabledColor = colors.pressedColor;
						this.dialogueChoices[j].button.colors = colors;
						this.dialogueChoices[j].text.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
					}
					else
					{
						this.dialogueChoices[j].notPossibleText.text = empty.ToUpper();
						this.dialogueChoices[j].notPossibleGameObject.SetActive(true);
						ColorBlock colors2 = this.dialogueChoices[j].button.colors;
						colors2.disabledColor = colors2.normalColor;
						this.dialogueChoices[j].button.colors = colors2;
						this.dialogueChoices[j].button.interactable = false;
						this.dialogueChoices[j].notPossibleText.ForceMeshUpdate(false, false);
						this.dialogueChoices[j].text.GetComponent<RectTransform>().offsetMax = new Vector2(-(this.dialogueChoices[j].notPossibleText.preferredWidth + 20f), 0f);
					}
					activeDialogueChoices.Add(j);
				}
			}
			if (activeDialogueChoices.Count == 0 || (activeDialogueChoices.Count == 1 && choices[0] == ""))
			{
				this.continuePopup.gameObject.SetActive(true);
				yield return new WaitUntil(() => this.spaceDownThisFrame || this.leftClickThisFrame);
				this.continuePopup.gameObject.SetActive(false);
				this.spaceDownThisFrame = false;
				this.leftClickThisFrame = false;
				this.currentHandler.ContinueSubmitted();
			}
			else
			{
				for (int k = 0; k < activeDialogueChoices.Count; k++)
				{
					this.dialogueChoices[activeDialogueChoices[k]].gameObject.SetActive(true);
				}
				while (!this.hasChoiceBeenSelected)
				{
					string empty2 = string.Empty;
					if (Input.GetKey(KeyCode.Alpha1) && this.IsChoiceValid(0, out empty2))
					{
						this.ChoiceSelected(0);
					}
					else if (Input.GetKey(KeyCode.Alpha2) && this.IsChoiceValid(1, out empty2))
					{
						this.ChoiceSelected(1);
					}
					else if (Input.GetKey(KeyCode.Alpha3) && this.IsChoiceValid(2, out empty2))
					{
						this.ChoiceSelected(2);
					}
					else if (Input.GetKey(KeyCode.Alpha4) && this.IsChoiceValid(3, out empty2))
					{
						this.ChoiceSelected(3);
					}
					else if (Input.GetKey(KeyCode.Alpha5) && this.IsChoiceValid(4, out empty2))
					{
						this.ChoiceSelected(4);
					}
					else if (Input.GetKey(KeyCode.Alpha6) && this.IsChoiceValid(5, out empty2))
					{
						this.ChoiceSelected(5);
					}
					else if (Input.GetKey(KeyCode.Alpha6) && this.IsChoiceValid(6, out empty2))
					{
						this.ChoiceSelected(6);
					}
					else if (Input.GetKey(KeyCode.Alpha6) && this.IsChoiceValid(7, out empty2))
					{
						this.ChoiceSelected(7);
					}
					else if (Input.GetKey(KeyCode.Alpha6) && this.IsChoiceValid(8, out empty2))
					{
						this.ChoiceSelected(8);
					}
					yield return new WaitForEndOfFrame();
				}
			}
			yield break;
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x00124277 File Offset: 0x00122477
		private IEnumerator ChoiceSelectionResidual(DialogueChoiceEntry choice, float fadeTime)
		{
			yield return new WaitForSeconds(0.25f);
			float realFadeTime = fadeTime - 0.25f;
			for (float i = 0f; i < realFadeTime; i += Time.deltaTime)
			{
				choice.canvasGroup.alpha = Mathf.Sqrt(Mathf.Lerp(1f, 0f, i / realFadeTime));
				yield return new WaitForEndOfFrame();
			}
			choice.gameObject.SetActive(false);
			this.choiceSelectionResidualCoroutine = null;
			yield break;
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x00124294 File Offset: 0x00122494
		private void StartDialogue(DialogueHandler handler)
		{
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.currentHandler = handler;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			Vector3 normalized = (this.currentHandler.LookPosition.transform.position - PlayerSingleton<PlayerCamera>.Instance.transform.position).normalized;
			Quaternion quaternion = Quaternion.LookRotation(new Vector3(normalized.x, 0f, normalized.z), Vector3.up);
			PlayerSingleton<PlayerMovement>.Instance.LerpPlayerRotation(quaternion, 0.3f);
			Vector3 vector = new Vector3(Mathf.Sqrt(Mathf.Pow(normalized.x, 2f) + Mathf.Pow(normalized.z, 2f)), normalized.y, 0f);
			float x = -Mathf.Atan2(vector.y, vector.x) * 57.295776f;
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(PlayerSingleton<PlayerCamera>.Instance.transform.position, quaternion * Quaternion.Euler(x, 0f, 0f), 0.3f, true);
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x001243D0 File Offset: 0x001225D0
		public void EndDialogue()
		{
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.continuePopup.gameObject.SetActive(false);
			for (int i = 0; i < this.dialogueChoices.Count; i++)
			{
				this.dialogueChoices[i].gameObject.SetActive(false);
			}
			if (this.dialogueRollout != null)
			{
				base.StopCoroutine(this.dialogueRollout);
			}
			if (this.choiceSelectionResidualCoroutine != null)
			{
				base.StopCoroutine(this.choiceSelectionResidualCoroutine);
			}
			this.canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			this.currentHandler = null;
			this.currentNode = null;
			if (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount == 0)
			{
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, true, false);
				return;
			}
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, false, false);
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x001244D8 File Offset: 0x001226D8
		public void ChoiceSelected(int choiceIndex)
		{
			string empty = string.Empty;
			if (!this.IsChoiceValid(choiceIndex, out empty))
			{
				return;
			}
			this.hasChoiceBeenSelected = true;
			for (int i = 0; i < this.dialogueChoices.Count; i++)
			{
				if (i == choiceIndex)
				{
					this.dialogueChoices[i].button.interactable = false;
					if (this.choiceSelectionResidualCoroutine != null)
					{
						base.StopCoroutine(this.choiceSelectionResidualCoroutine);
					}
					this.choiceSelectionResidualCoroutine = base.StartCoroutine(this.ChoiceSelectionResidual(this.dialogueChoices[i], 0.75f));
				}
				else
				{
					this.dialogueChoices[i].gameObject.SetActive(false);
				}
			}
			this.currentHandler.ChoiceSelected(choiceIndex);
		}

		// Token: 0x060045A3 RID: 17827 RVA: 0x0012458C File Offset: 0x0012278C
		private bool IsChoiceValid(int choiceIndex, out string reason)
		{
			if (this.currentNode != null && this.currentHandler.CurrentChoices.Count > choiceIndex)
			{
				return this.currentHandler.CheckChoice(this.currentHandler.CurrentChoices[choiceIndex].ChoiceLabel, out reason);
			}
			reason = string.Empty;
			return false;
		}

		// Token: 0x04003257 RID: 12887
		public const float TIME_PER_CHAR = 0.015f;

		// Token: 0x04003258 RID: 12888
		public bool SkipNextRollout;

		// Token: 0x04003259 RID: 12889
		[Header("References")]
		[SerializeField]
		protected Canvas canvas;

		// Token: 0x0400325A RID: 12890
		public RectTransform Container;

		// Token: 0x0400325B RID: 12891
		[SerializeField]
		protected TextMeshProUGUI dialogueText;

		// Token: 0x0400325C RID: 12892
		[SerializeField]
		protected GameObject continuePopup;

		// Token: 0x0400325D RID: 12893
		[SerializeField]
		protected List<DialogueChoiceEntry> dialogueChoices = new List<DialogueChoiceEntry>();

		// Token: 0x0400325E RID: 12894
		private DialogueHandler currentHandler;

		// Token: 0x0400325F RID: 12895
		private DialogueNodeData currentNode;

		// Token: 0x04003260 RID: 12896
		private bool spaceDownThisFrame;

		// Token: 0x04003261 RID: 12897
		private bool leftClickThisFrame;

		// Token: 0x04003262 RID: 12898
		private string overrideText = string.Empty;

		// Token: 0x04003263 RID: 12899
		private Coroutine dialogueRollout;

		// Token: 0x04003264 RID: 12900
		private Coroutine choiceSelectionResidualCoroutine;

		// Token: 0x04003265 RID: 12901
		private bool hasChoiceBeenSelected;
	}
}
