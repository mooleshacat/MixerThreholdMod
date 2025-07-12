using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management.Presets;
using ScheduleOne.Management.Presets.Options.SetterScreens;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.Management
{
	// Token: 0x020005BF RID: 1471
	public class PresetEditScreen : MonoBehaviour
	{
		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06002441 RID: 9281 RVA: 0x00094CD5 File Offset: 0x00092ED5
		public bool isOpen
		{
			get
			{
				return this.EditedPreset != null;
			}
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x00094CE0 File Offset: 0x00092EE0
		protected virtual void Awake()
		{
			this.ReturnButton.onClick.AddListener(new UnityAction(this.ReturnButtonClicked));
			this.DeleteButton.onClick.AddListener(new UnityAction(this.DeleteButtonClicked));
			this.InputField.onValueChanged.AddListener(new UnityAction<string>(this.NameFieldChange));
			this.InputField.onEndEdit.AddListener(new UnityAction<string>(this.NameFieldDone));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x00094D6F File Offset: 0x00092F6F
		private void Exit(ExitAction action)
		{
			if (action.Used)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.Used = true;
				this.Close();
			}
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x00094D9C File Offset: 0x00092F9C
		public virtual void Open(Preset preset)
		{
			this.EditedPreset = preset;
			this.InputField.text = this.EditedPreset.PresetName;
			Canvas.ForceUpdateCanvases();
			this.RefreshIcon();
			this.RefreshTransforms();
			base.gameObject.SetActive(true);
			base.StartCoroutine(this.<Open>g__Delay|13_0());
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x00094DF0 File Offset: 0x00092FF0
		public void Close()
		{
			this.EditedPreset = null;
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x00094E05 File Offset: 0x00093005
		private void RefreshIcon()
		{
			this.IconBackground.color = this.EditedPreset.PresetColor;
		}

		// Token: 0x06002447 RID: 9287 RVA: 0x00094E24 File Offset: 0x00093024
		private void RefreshTransforms()
		{
			this.InputField.ForceLabelUpdate();
			this.InputField.textComponent.ForceMeshUpdate(true, true);
			float renderedWidth = this.InputField.textComponent.renderedWidth;
			if (this.InputField.text == string.Empty)
			{
				renderedWidth = ((TextMeshProUGUI)this.InputField.placeholder).renderedWidth;
			}
			this.InputFieldRect.sizeDelta = new Vector2(renderedWidth + 3f, this.InputFieldRect.sizeDelta.y);
			this.InputFieldRect.anchoredPosition = new Vector2(1.5f, this.InputFieldRect.anchoredPosition.y);
			float num = 1.75f;
			float min = 5f;
			this.IconBackgroundRect.anchoredPosition = new Vector2(-Mathf.Clamp(renderedWidth / 2f + num, min, float.MaxValue), this.IconBackgroundRect.anchoredPosition.y);
			this.EditButtonRect.anchoredPosition = new Vector2(Mathf.Clamp(renderedWidth / 2f + num, min, float.MaxValue), this.IconBackgroundRect.anchoredPosition.y);
		}

		// Token: 0x06002448 RID: 9288 RVA: 0x00094F4D File Offset: 0x0009314D
		private void NameFieldChange(string newVal)
		{
			this.RefreshTransforms();
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x00094F55 File Offset: 0x00093155
		private void NameFieldDone(string piss)
		{
			if (this.IsNameAppropriate(piss))
			{
				this.EditedPreset.SetName(piss);
				return;
			}
			this.InputField.text = this.EditedPreset.PresetName;
			this.RefreshTransforms();
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x00094F89 File Offset: 0x00093189
		private bool IsNameAppropriate(string name)
		{
			return !string.IsNullOrWhiteSpace(name) && !(name == string.Empty) && !(name == "Pablo");
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x00094FB4 File Offset: 0x000931B4
		public void DeleteButtonClicked()
		{
			this.EditedPreset.DeletePreset(Preset.GetDefault(this.EditedPreset.ObjectType));
			this.Close();
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x00094FD7 File Offset: 0x000931D7
		public void ReturnButtonClicked()
		{
			this.Close();
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x00094FDF File Offset: 0x000931DF
		[CompilerGenerated]
		private IEnumerator <Open>g__Delay|13_0()
		{
			yield return new WaitForEndOfFrame();
			this.RefreshTransforms();
			yield break;
		}

		// Token: 0x04001AE3 RID: 6883
		public Preset EditedPreset;

		// Token: 0x04001AE4 RID: 6884
		[Header("References")]
		public RectTransform IconBackgroundRect;

		// Token: 0x04001AE5 RID: 6885
		public Image IconBackground;

		// Token: 0x04001AE6 RID: 6886
		public RectTransform InputFieldRect;

		// Token: 0x04001AE7 RID: 6887
		public TMP_InputField InputField;

		// Token: 0x04001AE8 RID: 6888
		public RectTransform EditButtonRect;

		// Token: 0x04001AE9 RID: 6889
		public Button ReturnButton;

		// Token: 0x04001AEA RID: 6890
		public Button DeleteButton;

		// Token: 0x020005C0 RID: 1472
		[Serializable]
		public class OptionData
		{
			// Token: 0x04001AEB RID: 6891
			public GameObject OptionEntryPrefab;

			// Token: 0x04001AEC RID: 6892
			public OptionSetterScreen OptionSetterScreen;
		}
	}
}
