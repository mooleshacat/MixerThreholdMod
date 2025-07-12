using System;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Casino.UI
{
	// Token: 0x020007B2 RID: 1970
	public class CasinoGamePlayerDisplay : MonoBehaviour
	{
		// Token: 0x06003593 RID: 13715 RVA: 0x000E0348 File Offset: 0x000DE548
		public void RefreshPlayers()
		{
			int currentPlayerCount = this.BindedPlayers.CurrentPlayerCount;
			this.TitleLabel.text = string.Concat(new string[]
			{
				"Players (",
				currentPlayerCount.ToString(),
				"/",
				this.BindedPlayers.PlayerLimit.ToString(),
				")"
			});
			for (int i = 0; i < this.PlayerEntries.Length; i++)
			{
				Player player = this.BindedPlayers.GetPlayer(i);
				if (player != null)
				{
					this.PlayerEntries[i].Find("Container/Name").GetComponent<TextMeshProUGUI>().text = player.PlayerName;
					this.PlayerEntries[i].Find("Container").gameObject.SetActive(true);
				}
				else
				{
					this.PlayerEntries[i].Find("Container").gameObject.SetActive(false);
				}
			}
			this.RefreshScores();
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x000E043C File Offset: 0x000DE63C
		public void RefreshScores()
		{
			int currentPlayerCount = this.BindedPlayers.CurrentPlayerCount;
			for (int i = 0; i < this.PlayerEntries.Length; i++)
			{
				if (currentPlayerCount > i)
				{
					this.PlayerEntries[i].Find("Container/Score").GetComponent<TextMeshProUGUI>().text = MoneyManager.FormatAmount((float)this.BindedPlayers.GetPlayerScore(this.BindedPlayers.GetPlayer(i)), false, false);
				}
			}
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x000E04A8 File Offset: 0x000DE6A8
		public void Bind(CasinoGamePlayers players)
		{
			this.BindedPlayers = players;
			this.BindedPlayers.onPlayerListChanged.AddListener(new UnityAction(this.RefreshPlayers));
			this.BindedPlayers.onPlayerScoresChanged.AddListener(new UnityAction(this.RefreshScores));
			this.RefreshPlayers();
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x000E04FC File Offset: 0x000DE6FC
		public void Unbind()
		{
			if (this.BindedPlayers == null)
			{
				return;
			}
			this.BindedPlayers.onPlayerListChanged.RemoveListener(new UnityAction(this.RefreshPlayers));
			this.BindedPlayers.onPlayerScoresChanged.RemoveListener(new UnityAction(this.RefreshScores));
			this.BindedPlayers = null;
		}

		// Token: 0x040025F0 RID: 9712
		public CasinoGamePlayers BindedPlayers;

		// Token: 0x040025F1 RID: 9713
		[Header("References")]
		public TextMeshProUGUI TitleLabel;

		// Token: 0x040025F2 RID: 9714
		public RectTransform[] PlayerEntries;
	}
}
