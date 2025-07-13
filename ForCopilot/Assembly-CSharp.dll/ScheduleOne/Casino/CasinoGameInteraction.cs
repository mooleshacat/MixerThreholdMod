using System;
using ScheduleOne.Interaction;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Casino
{
	// Token: 0x02000798 RID: 1944
	public class CasinoGameInteraction : MonoBehaviour
	{
		// Token: 0x0600347C RID: 13436 RVA: 0x000DB204 File Offset: 0x000D9404
		private void Awake()
		{
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x000DB240 File Offset: 0x000D9440
		private void Hovered()
		{
			if (this.Players.CurrentPlayerCount < this.Players.PlayerLimit)
			{
				this.IntObj.SetMessage("Play " + this.GameName);
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.IntObj.SetMessage("Table is full");
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
		}

		// Token: 0x0600347E RID: 13438 RVA: 0x000DB2A9 File Offset: 0x000D94A9
		private void Interacted()
		{
			if (this.Players.CurrentPlayerCount < this.Players.PlayerLimit && this.onLocalPlayerRequestJoin != null)
			{
				this.onLocalPlayerRequestJoin(Player.Local);
			}
		}

		// Token: 0x04002529 RID: 9513
		public string GameName;

		// Token: 0x0400252A RID: 9514
		[Header("References")]
		public CasinoGamePlayers Players;

		// Token: 0x0400252B RID: 9515
		public InteractableObject IntObj;

		// Token: 0x0400252C RID: 9516
		public Action<Player> onLocalPlayerRequestJoin;
	}
}
