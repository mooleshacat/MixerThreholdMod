using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B69 RID: 2921
	public class SaveDisplay : MonoBehaviour
	{
		// Token: 0x06004D9C RID: 19868 RVA: 0x00146A5E File Offset: 0x00144C5E
		public void Awake()
		{
			Singleton<LoadManager>.Instance.onSaveInfoLoaded.AddListener(new UnityAction(this.Refresh));
			this.Refresh();
		}

		// Token: 0x06004D9D RID: 19869 RVA: 0x00146A84 File Offset: 0x00144C84
		public void Refresh()
		{
			for (int i = 0; i < LoadManager.SaveGames.Length; i++)
			{
				this.SetDisplayedSave(i, LoadManager.SaveGames[i]);
			}
		}

		// Token: 0x06004D9E RID: 19870 RVA: 0x00146AB4 File Offset: 0x00144CB4
		public void SetDisplayedSave(int index, SaveInfo info)
		{
			Transform transform = this.Slots[index].Find("Container");
			if (info == null)
			{
				transform.Find("Info").gameObject.SetActive(false);
				return;
			}
			transform.Find("Info/Organisation").GetComponent<TextMeshProUGUI>().text = info.OrganisationName;
			transform.Find("Info/Version").GetComponent<TextMeshProUGUI>().text = "v" + info.SaveVersion;
			float num = info.Networth;
			string text = string.Empty;
			Color color = new Color32(75, byte.MaxValue, 10, byte.MaxValue);
			if (num > 1000000f)
			{
				num /= 1000000f;
				text = "$" + this.RoundToDecimalPlaces(num, 1).ToString() + "M";
				color = new Color32(byte.MaxValue, 225, 10, byte.MaxValue);
			}
			else if (num > 1000f)
			{
				num /= 1000f;
				text = "$" + this.RoundToDecimalPlaces(num, 1).ToString() + "K";
			}
			else
			{
				text = MoneyManager.FormatAmount(num, false, false);
			}
			transform.Find("Info/NetWorth/Text").GetComponent<TextMeshProUGUI>().text = text;
			transform.Find("Info/NetWorth/Text").GetComponent<TextMeshProUGUI>().color = color;
			int hours = Mathf.RoundToInt((float)(DateTime.Now - info.DateCreated).TotalHours);
			transform.Find("Info/Created/Text").GetComponent<TextMeshProUGUI>().text = this.GetTimeLabel(hours);
			int hours2 = Mathf.RoundToInt((float)(DateTime.Now - info.DateLastPlayed).TotalHours);
			transform.Find("Info/LastPlayed/Text").GetComponent<TextMeshProUGUI>().text = this.GetTimeLabel(hours2);
			transform.Find("Info").gameObject.SetActive(true);
		}

		// Token: 0x06004D9F RID: 19871 RVA: 0x00146C9D File Offset: 0x00144E9D
		private float RoundToDecimalPlaces(float value, int decimalPlaces)
		{
			return SaveDisplay.ToSingle(Math.Floor((double)value * Math.Pow(10.0, (double)decimalPlaces)) / Math.Pow(10.0, (double)decimalPlaces));
		}

		// Token: 0x06004DA0 RID: 19872 RVA: 0x00146CCD File Offset: 0x00144ECD
		public static float ToSingle(double value)
		{
			return (float)value;
		}

		// Token: 0x06004DA1 RID: 19873 RVA: 0x00146CD4 File Offset: 0x00144ED4
		private string GetTimeLabel(int hours)
		{
			int num = hours / 24;
			if (num == 0)
			{
				return "Today";
			}
			if (num == 1)
			{
				return "Yesterday";
			}
			if (num > 365)
			{
				return "More than a year ago";
			}
			return num.ToString() + " days ago";
		}

		// Token: 0x040039E2 RID: 14818
		[Header("References")]
		public RectTransform[] Slots;
	}
}
