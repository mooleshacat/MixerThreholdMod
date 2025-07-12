using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ScheduleOne.UI.Input
{
	// Token: 0x02000B70 RID: 2928
	[ExecuteInEditMode]
	public class InputPrompt : MonoBehaviour
	{
		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06004DC1 RID: 19905 RVA: 0x00147367 File Offset: 0x00145567
		private InputPromptsManager manager
		{
			get
			{
				if (!Singleton<InputPromptsManager>.InstanceExists)
				{
					return GameObject.Find("@InputPromptsManager").GetComponent<InputPromptsManager>();
				}
				return Singleton<InputPromptsManager>.Instance;
			}
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x00147385 File Offset: 0x00145585
		private void OnEnable()
		{
			this.RefreshPromptImages();
			this.Container.gameObject.SetActive(true);
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x0014739E File Offset: 0x0014559E
		private void OnDisable()
		{
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Update()
		{
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x001473B4 File Offset: 0x001455B4
		private void RefreshPromptImages()
		{
			this.AppliedAlignment = this.Alignment;
			this.displayedActions.Clear();
			this.displayedActions.AddRange(this.Actions);
			int childCount = this.ImagesContainer.childCount;
			Transform[] array = new Transform[childCount];
			for (int i = 0; i < childCount; i++)
			{
				array[i] = this.ImagesContainer.GetChild(i);
			}
			for (int j = 0; j < childCount; j++)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(array[j].gameObject);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(array[j].gameObject);
				}
			}
			this.promptImages.Clear();
			float num = 0f;
			for (int k = 0; k < this.Actions.Count; k++)
			{
				string text;
				string controlPath;
				InputActionRebindingExtensions.GetBindingDisplayString(this.Actions[k].action, 0, ref text, ref controlPath, 0);
				PromptImage promptImage = this.manager.GetPromptImage(controlPath, this.ImagesContainer);
				if (!(promptImage == null))
				{
					num += promptImage.Width;
					foreach (Image image in promptImage.transform.GetComponentsInChildren<Image>())
					{
						if (this.OverridePromptImageColor)
						{
							image.color = this.PromptImageColor;
						}
					}
					this.promptImages.Add(promptImage);
				}
			}
			num += InputPrompt.Spacing * (float)this.Actions.Count;
			this.LabelComponent.text = this.Label;
			this.LabelComponent.ForceMeshUpdate(false, false);
			num += this.LabelComponent.preferredWidth;
			float num2 = 0f;
			if (this.Alignment == InputPrompt.EInputPromptAlignment.Left)
			{
				num2 = -InputPrompt.Spacing;
			}
			else if (this.Alignment == InputPrompt.EInputPromptAlignment.Middle)
			{
				num2 = -num / 2f;
			}
			else if (this.Alignment == InputPrompt.EInputPromptAlignment.Right)
			{
				num2 = InputPrompt.Spacing;
			}
			float num3 = 1f;
			if (this.Alignment == InputPrompt.EInputPromptAlignment.Left)
			{
				this.LabelComponent.alignment = 8196;
				num3 = -1f;
			}
			else
			{
				this.LabelComponent.alignment = 8193;
			}
			float num4 = 0f;
			for (int m = 0; m < this.promptImages.Count; m++)
			{
				this.promptImages[m].GetComponent<RectTransform>().anchoredPosition = new Vector2(num2 + num4 * num3 + this.promptImages[m].Width * 0.5f * num3, 0f);
				num4 += this.promptImages[m].Width + InputPrompt.Spacing;
			}
			this.LabelComponent.GetComponent<RectTransform>().anchoredPosition = new Vector2(num2 + num4 * num3 + this.LabelComponent.GetComponent<RectTransform>().sizeDelta.x * 0.5f * num3, 0f);
			this.UpdateShade();
		}

		// Token: 0x06004DC6 RID: 19910 RVA: 0x0014768F File Offset: 0x0014588F
		public void SetLabel(string label)
		{
			this.Label = label;
			this.LabelComponent.text = this.Label;
			this.UpdateShade();
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x001476B0 File Offset: 0x001458B0
		private void UpdateShade()
		{
			float num = this.LabelComponent.preferredWidth + 90f;
			this.Shade.sizeDelta = new Vector2(num, this.Shade.sizeDelta.y);
			if (this.Alignment == InputPrompt.EInputPromptAlignment.Left)
			{
				this.Shade.anchoredPosition = new Vector2(-num / 2f, 0f);
				return;
			}
			if (this.Alignment == InputPrompt.EInputPromptAlignment.Middle)
			{
				this.Shade.anchoredPosition = new Vector2(0f, 0f);
				return;
			}
			if (this.Alignment == InputPrompt.EInputPromptAlignment.Right)
			{
				this.Shade.anchoredPosition = new Vector2(num / 2f, 0f);
			}
		}

		// Token: 0x040039F5 RID: 14837
		public static float Spacing = 10f;

		// Token: 0x040039F6 RID: 14838
		[Header("Settings")]
		public List<InputActionReference> Actions = new List<InputActionReference>();

		// Token: 0x040039F7 RID: 14839
		public string Label;

		// Token: 0x040039F8 RID: 14840
		public InputPrompt.EInputPromptAlignment Alignment;

		// Token: 0x040039F9 RID: 14841
		[Header("References")]
		public RectTransform Container;

		// Token: 0x040039FA RID: 14842
		public RectTransform ImagesContainer;

		// Token: 0x040039FB RID: 14843
		public TextMeshProUGUI LabelComponent;

		// Token: 0x040039FC RID: 14844
		public RectTransform Shade;

		// Token: 0x040039FD RID: 14845
		[Header("Settings")]
		public bool OverridePromptImageColor;

		// Token: 0x040039FE RID: 14846
		public Color PromptImageColor = Color.white;

		// Token: 0x040039FF RID: 14847
		[SerializeField]
		private List<PromptImage> promptImages = new List<PromptImage>();

		// Token: 0x04003A00 RID: 14848
		private List<InputActionReference> displayedActions = new List<InputActionReference>();

		// Token: 0x04003A01 RID: 14849
		private InputPrompt.EInputPromptAlignment AppliedAlignment;

		// Token: 0x02000B71 RID: 2929
		public enum EInputPromptAlignment
		{
			// Token: 0x04003A03 RID: 14851
			Left,
			// Token: 0x04003A04 RID: 14852
			Middle,
			// Token: 0x04003A05 RID: 14853
			Right
		}
	}
}
