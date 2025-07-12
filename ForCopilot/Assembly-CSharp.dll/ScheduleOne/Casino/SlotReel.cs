using System;
using ScheduleOne.Audio;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Casino
{
	// Token: 0x020007AF RID: 1967
	public class SlotReel : MonoBehaviour
	{
		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06003570 RID: 13680 RVA: 0x000DF8C1 File Offset: 0x000DDAC1
		// (set) Token: 0x06003571 RID: 13681 RVA: 0x000DF8C9 File Offset: 0x000DDAC9
		public bool IsSpinning { get; private set; }

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06003572 RID: 13682 RVA: 0x000DF8D2 File Offset: 0x000DDAD2
		// (set) Token: 0x06003573 RID: 13683 RVA: 0x000DF8DA File Offset: 0x000DDADA
		public SlotMachine.ESymbol CurrentSymbol { get; private set; } = SlotMachine.ESymbol.Seven;

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06003574 RID: 13684 RVA: 0x000DF8E3 File Offset: 0x000DDAE3
		// (set) Token: 0x06003575 RID: 13685 RVA: 0x000DF8EB File Offset: 0x000DDAEB
		public float CurrentRotation { get; private set; }

		// Token: 0x06003576 RID: 13686 RVA: 0x000DF8F4 File Offset: 0x000DDAF4
		private void Awake()
		{
			this.SetSymbol(SlotMachine.GetRandomSymbol());
		}

		// Token: 0x06003577 RID: 13687 RVA: 0x000DF901 File Offset: 0x000DDB01
		private void Update()
		{
			if (this.IsSpinning)
			{
				this.SetReelRotation(this.CurrentRotation + this.SpinSpeed * Time.deltaTime);
				return;
			}
			this.SetReelRotation(this.GetSymbolRotation(this.CurrentSymbol));
		}

		// Token: 0x06003578 RID: 13688 RVA: 0x000DF937 File Offset: 0x000DDB37
		public void Spin()
		{
			this.IsSpinning = true;
			if (this.onStart != null)
			{
				this.onStart.Invoke();
			}
		}

		// Token: 0x06003579 RID: 13689 RVA: 0x000DF953 File Offset: 0x000DDB53
		public void Stop(SlotMachine.ESymbol endSymbol)
		{
			this.CurrentSymbol = endSymbol;
			this.IsSpinning = false;
			this.StopSound.Play();
			if (this.onStop != null)
			{
				this.onStop.Invoke();
			}
		}

		// Token: 0x0600357A RID: 13690 RVA: 0x000DF981 File Offset: 0x000DDB81
		public void SetSymbol(SlotMachine.ESymbol symbol)
		{
			this.CurrentSymbol = symbol;
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x000DF98A File Offset: 0x000DDB8A
		private void SetReelRotation(float rotation)
		{
			base.transform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
			this.CurrentRotation = rotation % 360f;
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x000DF9B4 File Offset: 0x000DDBB4
		private float GetSymbolRotation(SlotMachine.ESymbol symbol)
		{
			foreach (SlotReel.SymbolRotation symbolRotation in this.SymbolRotations)
			{
				if (symbolRotation.Symbol == symbol)
				{
					return symbolRotation.Rotation;
				}
			}
			Console.LogWarning("SlotReel.GetSymbolRotation: Symbol not found: " + symbol.ToString(), null);
			return 0f;
		}

		// Token: 0x040025CC RID: 9676
		[Header("Settings")]
		public SlotReel.SymbolRotation[] SymbolRotations;

		// Token: 0x040025CD RID: 9677
		public float SpinSpeed = 1000f;

		// Token: 0x040025CE RID: 9678
		[Header("References")]
		public AudioSourceController StopSound;

		// Token: 0x040025CF RID: 9679
		public UnityEvent onStart;

		// Token: 0x040025D0 RID: 9680
		public UnityEvent onStop;

		// Token: 0x020007B0 RID: 1968
		[Serializable]
		public class SymbolRotation
		{
			// Token: 0x040025D1 RID: 9681
			public SlotMachine.ESymbol Symbol;

			// Token: 0x040025D2 RID: 9682
			public float Rotation;
		}
	}
}
