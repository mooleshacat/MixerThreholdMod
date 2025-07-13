using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Levelling;
using ScheduleOne.Money;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCustomization
{
	// Token: 0x02000B82 RID: 2946
	public class CharacterCustomizationOption : MonoBehaviour
	{
		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06004E23 RID: 20003 RVA: 0x0014A84B File Offset: 0x00148A4B
		// (set) Token: 0x06004E24 RID: 20004 RVA: 0x0014A853 File Offset: 0x00148A53
		public bool purchased { get; private set; }

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06004E25 RID: 20005 RVA: 0x0014A85C File Offset: 0x00148A5C
		private bool purchaseable
		{
			get
			{
				return !this.RequireLevel || this.RequiredLevel <= NetworkSingleton<LevelManager>.Instance.GetFullRank();
			}
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x0014A880 File Offset: 0x00148A80
		private void Awake()
		{
			this.NameLabel.text = this.Name;
			if (this.Price > 0f)
			{
				this.PriceLabel.text = MoneyManager.FormatAmount(this.Price, false, false);
			}
			else
			{
				this.PriceLabel.text = "Free";
			}
			this.UpdatePriceColor();
			this.LevelLabel.text = this.RequiredLevel.ToString();
			this.MainButton.onClick.AddListener(new UnityAction(this.Selected));
			this.BuyButton.onClick.AddListener(new UnityAction(this.Purchased));
		}

		// Token: 0x06004E27 RID: 20007 RVA: 0x0014A92F File Offset: 0x00148B2F
		private void OnValidate()
		{
			base.gameObject.name = this.Name;
		}

		// Token: 0x06004E28 RID: 20008 RVA: 0x0014A942 File Offset: 0x00148B42
		private void FixedUpdate()
		{
			this.BuyButton.interactable = (NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= this.Price);
		}

		// Token: 0x06004E29 RID: 20009 RVA: 0x0014A964 File Offset: 0x00148B64
		private void Start()
		{
			this.UpdateUI();
		}

		// Token: 0x06004E2A RID: 20010 RVA: 0x0014A96C File Offset: 0x00148B6C
		private void Selected()
		{
			this.SetSelected(true);
		}

		// Token: 0x06004E2B RID: 20011 RVA: 0x0014A978 File Offset: 0x00148B78
		private void Purchased()
		{
			if (!this.purchaseable)
			{
				return;
			}
			if (this.onPurchase != null)
			{
				this.onPurchase.Invoke();
			}
			if (this.Price > 0f)
			{
				NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Character customization", -this.Price, 1f, string.Empty);
			}
			this.SetPurchased(true);
		}

		// Token: 0x06004E2C RID: 20012 RVA: 0x0014A9D8 File Offset: 0x00148BD8
		private void UpdatePriceColor()
		{
			if (this.Price <= 0f)
			{
				Color color;
				this.PriceLabel.color = (ColorUtility.TryParseHtmlString("#4CBFFF", out color) ? color : Color.white);
				return;
			}
			if (NetworkSingleton<MoneyManager>.Instance.cashBalance >= this.Price)
			{
				Color color2;
				this.PriceLabel.color = (ColorUtility.TryParseHtmlString("#4CBFFF", out color2) ? color2 : Color.white);
				return;
			}
			this.PriceLabel.color = new Color32(200, 75, 70, byte.MaxValue);
		}

		// Token: 0x06004E2D RID: 20013 RVA: 0x0014AA6C File Offset: 0x00148C6C
		public void SetSelected(bool _selected)
		{
			this.selected = _selected;
			this.SelectionIndicator.gameObject.SetActive(this.selected);
			this.NameLabel.rectTransform.offsetMin = new Vector2(this.selected ? 30f : 10f, this.NameLabel.rectTransform.offsetMin.y);
			this.UpdateUI();
			if (this.selected)
			{
				if (this.onSelect != null)
				{
					this.onSelect.Invoke();
					return;
				}
			}
			else if (this.onDeselect != null)
			{
				this.onDeselect.Invoke();
			}
		}

		// Token: 0x06004E2E RID: 20014 RVA: 0x0014AB0C File Offset: 0x00148D0C
		public void SetPurchased(bool _purchased)
		{
			this.purchased = _purchased;
			this.BuyButton.gameObject.SetActive(!this.purchased);
			this.PriceLabel.gameObject.SetActive(!this.purchased);
			if (_purchased)
			{
				this.SetSelected(true);
			}
			this.UpdateUI();
		}

		// Token: 0x06004E2F RID: 20015 RVA: 0x0014AB64 File Offset: 0x00148D64
		private void UpdateUI()
		{
			this.LockDisplay.gameObject.SetActive(!this.purchaseable);
			this.PriceLabel.gameObject.SetActive(this.purchaseable && !this.purchased);
			this.BuyButton.gameObject.SetActive(this.purchaseable && !this.purchased);
			this.UpdatePriceColor();
		}

		// Token: 0x06004E30 RID: 20016 RVA: 0x0014ABD8 File Offset: 0x00148DD8
		public void ParentCategoryClosed()
		{
			if (this.selected && !this.purchased)
			{
				this.SetSelected(false);
				return;
			}
			if (this.purchased && !this.selected)
			{
				this.SetSelected(true);
			}
		}

		// Token: 0x06004E31 RID: 20017 RVA: 0x0014AC09 File Offset: 0x00148E09
		public void SiblingOptionSelected(CharacterCustomizationOption option)
		{
			if (option != this && this.selected)
			{
				this.SetSelected(false);
			}
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x0014AC23 File Offset: 0x00148E23
		public void SiblingOptionPurchased(CharacterCustomizationOption option)
		{
			if (option != this && this.purchased)
			{
				this.SetPurchased(false);
			}
		}

		// Token: 0x04003A7E RID: 14974
		public string Name = "Option";

		// Token: 0x04003A7F RID: 14975
		public string Label = "AssetPath or Label";

		// Token: 0x04003A80 RID: 14976
		public float Price;

		// Token: 0x04003A81 RID: 14977
		public bool RequireLevel;

		// Token: 0x04003A82 RID: 14978
		public FullRank RequiredLevel = new FullRank(ERank.Street_Rat, 1);

		// Token: 0x04003A83 RID: 14979
		[Header("References")]
		public TextMeshProUGUI NameLabel;

		// Token: 0x04003A84 RID: 14980
		public TextMeshProUGUI PriceLabel;

		// Token: 0x04003A85 RID: 14981
		public TextMeshProUGUI LevelLabel;

		// Token: 0x04003A86 RID: 14982
		public RectTransform LockDisplay;

		// Token: 0x04003A87 RID: 14983
		public Button MainButton;

		// Token: 0x04003A88 RID: 14984
		public Button BuyButton;

		// Token: 0x04003A89 RID: 14985
		public RectTransform SelectionIndicator;

		// Token: 0x04003A8A RID: 14986
		[Header("Events")]
		public UnityEvent onSelect;

		// Token: 0x04003A8B RID: 14987
		public UnityEvent onDeselect;

		// Token: 0x04003A8C RID: 14988
		public UnityEvent onPurchase;

		// Token: 0x04003A8E RID: 14990
		private bool selected;
	}
}
