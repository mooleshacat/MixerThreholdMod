using System;
using System.Collections.Generic;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Casino
{
	// Token: 0x02000799 RID: 1945
	public class CasinoGamePlayerData
	{
		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06003480 RID: 13440 RVA: 0x000DB2DB File Offset: 0x000D94DB
		// (set) Token: 0x06003481 RID: 13441 RVA: 0x000DB2E3 File Offset: 0x000D94E3
		public CasinoGamePlayers Parent { get; private set; }

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06003482 RID: 13442 RVA: 0x000DB2EC File Offset: 0x000D94EC
		// (set) Token: 0x06003483 RID: 13443 RVA: 0x000DB2F4 File Offset: 0x000D94F4
		public Player Player { get; private set; }

		// Token: 0x06003484 RID: 13444 RVA: 0x000DB300 File Offset: 0x000D9500
		public CasinoGamePlayerData(CasinoGamePlayers parent, Player player)
		{
			this.Parent = parent;
			this.Player = player;
			this.bools = new Dictionary<string, bool>();
			this.floats = new Dictionary<string, float>();
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x000DB350 File Offset: 0x000D9550
		public T GetData<T>(string key)
		{
			if (typeof(T) == typeof(bool))
			{
				if (this.bools.ContainsKey(key))
				{
					return (T)((object)this.bools[key]);
				}
			}
			else if (typeof(T) == typeof(float) && this.floats.ContainsKey(key))
			{
				return (T)((object)this.floats[key]);
			}
			return default(T);
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x000DB3E8 File Offset: 0x000D95E8
		public void SetData<T>(string key, T value, bool network = true)
		{
			if (network)
			{
				if (typeof(T) == typeof(bool))
				{
					this.Parent.SendPlayerBool(this.Player.NetworkObject, key, (bool)((object)value));
				}
				else if (typeof(T) == typeof(float))
				{
					this.Parent.SendPlayerFloat(this.Player.NetworkObject, key, (float)((object)value));
				}
			}
			if (!(typeof(T) == typeof(bool)))
			{
				if (typeof(T) == typeof(float))
				{
					if (this.floats.ContainsKey(key))
					{
						this.floats[key] = (float)((object)value);
						return;
					}
					this.floats.Add(key, (float)((object)value));
				}
				return;
			}
			if (this.bools.ContainsKey(key))
			{
				this.bools[key] = (bool)((object)value);
				return;
			}
			this.bools.Add(key, (bool)((object)value));
		}

		// Token: 0x0400252F RID: 9519
		protected Dictionary<string, bool> bools = new Dictionary<string, bool>();

		// Token: 0x04002530 RID: 9520
		protected Dictionary<string, float> floats = new Dictionary<string, float>();
	}
}
