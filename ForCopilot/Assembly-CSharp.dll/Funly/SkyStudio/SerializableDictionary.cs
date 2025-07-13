using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001E0 RID: 480
	[Serializable]
	public class SerializableDictionary<K, V> : ISerializationCallbackReceiver
	{
		// Token: 0x06000A83 RID: 2691 RVA: 0x0002E9D9 File Offset: 0x0002CBD9
		public void Clear()
		{
			this.dict.Clear();
		}

		// Token: 0x1700024E RID: 590
		public V this[K aKey]
		{
			get
			{
				return this.dict[aKey];
			}
			set
			{
				this.dict[aKey] = value;
			}
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x0002EA04 File Offset: 0x0002CC04
		public void OnBeforeSerialize()
		{
			this.m_Keys.Clear();
			this.m_Values.Clear();
			foreach (K k in this.dict.Keys)
			{
				this.m_Keys.Add(k);
				this.m_Values.Add(this.dict[k]);
			}
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0002EA90 File Offset: 0x0002CC90
		public void OnAfterDeserialize()
		{
			if (this.m_Keys.Count != this.m_Values.Count)
			{
				Debug.LogError("Can't restore dictionry with unbalaned key/values");
				return;
			}
			this.dict.Clear();
			for (int i = 0; i < this.m_Keys.Count; i++)
			{
				this.dict[this.m_Keys[i]] = this.m_Values[i];
			}
		}

		// Token: 0x04000B69 RID: 2921
		[NonSerialized]
		public Dictionary<K, V> dict = new Dictionary<K, V>();

		// Token: 0x04000B6A RID: 2922
		[SerializeField]
		public List<K> m_Keys = new List<K>();

		// Token: 0x04000B6B RID: 2923
		[SerializeField]
		public List<V> m_Values = new List<V>();
	}
}
