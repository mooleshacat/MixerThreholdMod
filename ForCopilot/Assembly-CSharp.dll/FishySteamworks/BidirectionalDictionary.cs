using System;
using System.Collections;
using System.Collections.Generic;

namespace FishySteamworks
{
	// Token: 0x02000C95 RID: 3221
	public class BidirectionalDictionary<T1, T2> : IEnumerable
	{
		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06005A4C RID: 23116 RVA: 0x0017CB07 File Offset: 0x0017AD07
		public IEnumerable<T1> FirstTypes
		{
			get
			{
				return this.t1ToT2Dict.Keys;
			}
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06005A4D RID: 23117 RVA: 0x0017CB14 File Offset: 0x0017AD14
		public IEnumerable<T2> SecondTypes
		{
			get
			{
				return this.t2ToT1Dict.Keys;
			}
		}

		// Token: 0x06005A4E RID: 23118 RVA: 0x0017CB21 File Offset: 0x0017AD21
		public IEnumerator GetEnumerator()
		{
			return this.t1ToT2Dict.GetEnumerator();
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06005A4F RID: 23119 RVA: 0x0017CB33 File Offset: 0x0017AD33
		public int Count
		{
			get
			{
				return this.t1ToT2Dict.Count;
			}
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06005A50 RID: 23120 RVA: 0x0017CB40 File Offset: 0x0017AD40
		public Dictionary<T1, T2> First
		{
			get
			{
				return this.t1ToT2Dict;
			}
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x06005A51 RID: 23121 RVA: 0x0017CB48 File Offset: 0x0017AD48
		public Dictionary<T2, T1> Second
		{
			get
			{
				return this.t2ToT1Dict;
			}
		}

		// Token: 0x06005A52 RID: 23122 RVA: 0x0017CB50 File Offset: 0x0017AD50
		public void Add(T1 key, T2 value)
		{
			if (this.t1ToT2Dict.ContainsKey(key))
			{
				this.Remove(key);
			}
			this.t1ToT2Dict[key] = value;
			this.t2ToT1Dict[value] = key;
		}

		// Token: 0x06005A53 RID: 23123 RVA: 0x0017CB81 File Offset: 0x0017AD81
		public void Add(T2 key, T1 value)
		{
			if (this.t2ToT1Dict.ContainsKey(key))
			{
				this.Remove(key);
			}
			this.t2ToT1Dict[key] = value;
			this.t1ToT2Dict[value] = key;
		}

		// Token: 0x06005A54 RID: 23124 RVA: 0x0017CBB2 File Offset: 0x0017ADB2
		public T2 Get(T1 key)
		{
			return this.t1ToT2Dict[key];
		}

		// Token: 0x06005A55 RID: 23125 RVA: 0x0017CBC0 File Offset: 0x0017ADC0
		public T1 Get(T2 key)
		{
			return this.t2ToT1Dict[key];
		}

		// Token: 0x06005A56 RID: 23126 RVA: 0x0017CBCE File Offset: 0x0017ADCE
		public bool TryGetValue(T1 key, out T2 value)
		{
			return this.t1ToT2Dict.TryGetValue(key, out value);
		}

		// Token: 0x06005A57 RID: 23127 RVA: 0x0017CBDD File Offset: 0x0017ADDD
		public bool TryGetValue(T2 key, out T1 value)
		{
			return this.t2ToT1Dict.TryGetValue(key, out value);
		}

		// Token: 0x06005A58 RID: 23128 RVA: 0x0017CBEC File Offset: 0x0017ADEC
		public bool Contains(T1 key)
		{
			return this.t1ToT2Dict.ContainsKey(key);
		}

		// Token: 0x06005A59 RID: 23129 RVA: 0x0017CBFA File Offset: 0x0017ADFA
		public bool Contains(T2 key)
		{
			return this.t2ToT1Dict.ContainsKey(key);
		}

		// Token: 0x06005A5A RID: 23130 RVA: 0x0017CC08 File Offset: 0x0017AE08
		public void Remove(T1 key)
		{
			if (this.Contains(key))
			{
				T2 key2 = this.t1ToT2Dict[key];
				this.t1ToT2Dict.Remove(key);
				this.t2ToT1Dict.Remove(key2);
			}
		}

		// Token: 0x06005A5B RID: 23131 RVA: 0x0017CC48 File Offset: 0x0017AE48
		public void Remove(T2 key)
		{
			if (this.Contains(key))
			{
				T1 key2 = this.t2ToT1Dict[key];
				this.t1ToT2Dict.Remove(key2);
				this.t2ToT1Dict.Remove(key);
			}
		}

		// Token: 0x17000C8C RID: 3212
		public T1 this[T2 key]
		{
			get
			{
				return this.t2ToT1Dict[key];
			}
			set
			{
				this.Add(key, value);
			}
		}

		// Token: 0x17000C8D RID: 3213
		public T2 this[T1 key]
		{
			get
			{
				return this.t1ToT2Dict[key];
			}
			set
			{
				this.Add(key, value);
			}
		}

		// Token: 0x04004268 RID: 17000
		private Dictionary<T1, T2> t1ToT2Dict = new Dictionary<T1, T2>();

		// Token: 0x04004269 RID: 17001
		private Dictionary<T2, T1> t2ToT1Dict = new Dictionary<T2, T1>();
	}
}
