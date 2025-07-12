using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product;
using ScheduleOne.Properties;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A4B RID: 2635
	public class NewMixScreen : Singleton<NewMixScreen>
	{
		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x060046CA RID: 18122 RVA: 0x00128E4C File Offset: 0x0012704C
		public bool IsOpen
		{
			get
			{
				return this.canvas.enabled;
			}
		}

		// Token: 0x060046CB RID: 18123 RVA: 0x00128E5C File Offset: 0x0012705C
		protected override void Awake()
		{
			base.Awake();
			this.nameInputField.onValueChanged.AddListener(new UnityAction<string>(this.OnNameValueChanged));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 3);
			this.canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x060046CC RID: 18124 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Exit(ExitAction action)
		{
		}

		// Token: 0x060046CD RID: 18125 RVA: 0x00128EBA File Offset: 0x001270BA
		protected virtual void Update()
		{
			if (this.IsOpen && this.confirmButton.interactable && GameInput.GetButtonDown(GameInput.ButtonCode.Submit))
			{
				this.ConfirmButtonClicked();
			}
		}

		// Token: 0x060046CE RID: 18126 RVA: 0x00128EE0 File Offset: 0x001270E0
		public void Open(List<Property> properties, EDrugType drugType, float productMarketValue)
		{
			this.canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.nameInputField.text = this.GenerateUniqueName(properties.ToArray(), drugType);
			Singleton<TaskManager>.Instance.PlayTaskCompleteSound();
			this.PropertiesLabel.text = string.Empty;
			for (int i = 0; i < properties.Count; i++)
			{
				Property property = properties[i];
				if (this.PropertiesLabel.text.Length > 0)
				{
					TextMeshProUGUI propertiesLabel = this.PropertiesLabel;
					propertiesLabel.text += "\n";
				}
				if (i == 4 && properties.Count > 5)
				{
					int num = properties.Count - 5 + 1;
					TextMeshProUGUI propertiesLabel2 = this.PropertiesLabel;
					propertiesLabel2.text = propertiesLabel2.text + "+ " + num.ToString() + " more...";
					break;
				}
				TextMeshProUGUI propertiesLabel3 = this.PropertiesLabel;
				propertiesLabel3.text = string.Concat(new string[]
				{
					propertiesLabel3.text,
					"<color=#",
					ColorUtility.ToHtmlStringRGBA(property.LabelColor),
					">• ",
					property.Name,
					"</color>"
				});
			}
			this.MarketValueLabel.text = "Market Value: <color=#54E717>" + MoneyManager.FormatAmount(productMarketValue, false, false) + "</color>";
		}

		// Token: 0x060046CF RID: 18127 RVA: 0x0012904C File Offset: 0x0012724C
		public void Close()
		{
			this.canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x0012907B File Offset: 0x0012727B
		public void RandomizeButtonClicked()
		{
			this.nameInputField.text = this.GenerateUniqueName(null, EDrugType.Marijuana);
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x00129090 File Offset: 0x00127290
		public void ConfirmButtonClicked()
		{
			if (this.onMixNamed != null)
			{
				this.onMixNamed(this.nameInputField.text);
			}
			this.Sound.Play();
			this.RandomizeButtonClicked();
			this.Close();
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x001290C8 File Offset: 0x001272C8
		public string GenerateUniqueName(Property[] properties = null, EDrugType drugType = EDrugType.Marijuana)
		{
			UnityEngine.Random.InitState((int)(Time.timeSinceLevelLoad * 10f));
			string str = this.name1Library[UnityEngine.Random.Range(0, this.name1Library.Count)];
			string str2 = this.name2Library[UnityEngine.Random.Range(0, this.name2Library.Count)];
			if (properties != null)
			{
				int num = 0;
				foreach (Property property in properties)
				{
					num += property.Name.GetHashCode() / 2000;
				}
				num += drugType.GetHashCode() / 1000;
				int value = Mathf.Abs(num % this.name1Library.Count);
				int value2 = Mathf.Abs(num / 2 % this.name2Library.Count);
				str = this.name1Library[Mathf.Clamp(value, 0, this.name1Library.Count)];
				str2 = this.name2Library[Mathf.Clamp(value2, 0, this.name2Library.Count)];
			}
			while (NetworkSingleton<ProductManager>.Instance.ProductNames.Contains(str + " " + str2))
			{
				str = this.name1Library[UnityEngine.Random.Range(0, this.name1Library.Count)];
				str2 = this.name2Library[UnityEngine.Random.Range(0, this.name2Library.Count)];
			}
			return str + " " + str2;
		}

		// Token: 0x060046D3 RID: 18131 RVA: 0x0012923C File Offset: 0x0012743C
		protected void RefreshNameButtons()
		{
			float num = this.nameInputField.textComponent.preferredWidth / 2f;
			float num2 = 20f;
			this.editIcon.anchoredPosition = new Vector2(num + num2, this.editIcon.anchoredPosition.y);
			this.randomizeNameButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-num - num2, this.randomizeNameButton.GetComponent<RectTransform>().anchoredPosition.y);
		}

		// Token: 0x060046D4 RID: 18132 RVA: 0x001292B8 File Offset: 0x001274B8
		public void OnNameValueChanged(string newVal)
		{
			if (NetworkSingleton<ProductManager>.Instance.ProductNames.Contains(this.nameInputField.text) || !ProductManager.IsMixNameValid(this.nameInputField.text))
			{
				this.mixAlreadyExistsText.gameObject.SetActive(true);
				this.confirmButton.interactable = false;
			}
			else
			{
				this.mixAlreadyExistsText.gameObject.SetActive(false);
				this.confirmButton.interactable = true;
			}
			this.RefreshNameButtons();
			base.Invoke("RefreshNameButtons", 0.016666668f);
		}

		// Token: 0x04003389 RID: 13193
		public const int MAX_PROPERTIES_DISPLAYED = 5;

		// Token: 0x0400338A RID: 13194
		[Header("References")]
		[SerializeField]
		protected Canvas canvas;

		// Token: 0x0400338B RID: 13195
		public RectTransform Container;

		// Token: 0x0400338C RID: 13196
		[SerializeField]
		protected TMP_InputField nameInputField;

		// Token: 0x0400338D RID: 13197
		[SerializeField]
		protected GameObject mixAlreadyExistsText;

		// Token: 0x0400338E RID: 13198
		[SerializeField]
		protected RectTransform editIcon;

		// Token: 0x0400338F RID: 13199
		[SerializeField]
		protected Button randomizeNameButton;

		// Token: 0x04003390 RID: 13200
		[SerializeField]
		protected Button confirmButton;

		// Token: 0x04003391 RID: 13201
		[SerializeField]
		protected TextMeshProUGUI PropertiesLabel;

		// Token: 0x04003392 RID: 13202
		[SerializeField]
		protected TextMeshProUGUI MarketValueLabel;

		// Token: 0x04003393 RID: 13203
		public AudioSourceController Sound;

		// Token: 0x04003394 RID: 13204
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject attributeEntryPrefab;

		// Token: 0x04003395 RID: 13205
		[Header("Name Library")]
		[SerializeField]
		protected List<string> name1Library = new List<string>();

		// Token: 0x04003396 RID: 13206
		[SerializeField]
		protected List<string> name2Library = new List<string>();

		// Token: 0x04003397 RID: 13207
		public Action<string> onMixNamed;
	}
}
