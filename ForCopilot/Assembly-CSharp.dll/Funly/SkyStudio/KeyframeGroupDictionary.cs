using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001DF RID: 479
	[Serializable]
	public class KeyframeGroupDictionary : ISerializationCallbackReceiver, IEnumerable<string>, IEnumerable
	{
		// Token: 0x1700024D RID: 589
		public IKeyframeGroup this[string aKey]
		{
			get
			{
				return this.m_Groups[aKey];
			}
			set
			{
				this.m_Groups[aKey] = value;
			}
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x0002E512 File Offset: 0x0002C712
		public bool ContainsKey(string key)
		{
			return this.m_Groups.ContainsKey(key);
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0002E520 File Offset: 0x0002C720
		public void Clear()
		{
			this.m_Groups.Clear();
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0002E530 File Offset: 0x0002C730
		public T GetGroup<T>(string propertyName) where T : class
		{
			if (typeof(T) == typeof(ColorKeyframeGroup))
			{
				return this.m_Groups[propertyName] as T;
			}
			if (typeof(T) == typeof(NumberKeyframeGroup))
			{
				return this.m_Groups[propertyName] as T;
			}
			if (typeof(T) == typeof(TextureKeyframeGroup))
			{
				return this.m_Groups[propertyName] as T;
			}
			if (typeof(T) == typeof(SpherePointGroupDictionary))
			{
				return this.m_Groups[propertyName] as T;
			}
			if (typeof(T) == typeof(BoolKeyframeGroup))
			{
				return this.m_Groups[propertyName] as T;
			}
			return default(T);
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0002E640 File Offset: 0x0002C840
		public void OnBeforeSerialize()
		{
			this.m_ColorGroup.Clear();
			this.m_NumberGroup.Clear();
			this.m_TextureGroup.Clear();
			this.m_SpherePointGroup.Clear();
			this.m_BoolGroup.Clear();
			foreach (string text in this.m_Groups.Keys)
			{
				IKeyframeGroup keyframeGroup = this.m_Groups[text];
				if (keyframeGroup is ColorKeyframeGroup)
				{
					this.m_ColorGroup[text] = (keyframeGroup as ColorKeyframeGroup);
				}
				else if (keyframeGroup is NumberKeyframeGroup)
				{
					this.m_NumberGroup[text] = (keyframeGroup as NumberKeyframeGroup);
				}
				else if (keyframeGroup is TextureKeyframeGroup)
				{
					this.m_TextureGroup[text] = (keyframeGroup as TextureKeyframeGroup);
				}
				else if (keyframeGroup is SpherePointKeyframeGroup)
				{
					this.m_SpherePointGroup[text] = (keyframeGroup as SpherePointKeyframeGroup);
				}
				else if (keyframeGroup is BoolKeyframeGroup)
				{
					this.m_BoolGroup[text] = (keyframeGroup as BoolKeyframeGroup);
				}
			}
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0002E768 File Offset: 0x0002C968
		public void OnAfterDeserialize()
		{
			this.m_Groups.Clear();
			foreach (string text in this.m_ColorGroup.dict.Keys)
			{
				this.m_Groups[text] = this.m_ColorGroup[text];
			}
			foreach (string text2 in this.m_NumberGroup.dict.Keys)
			{
				this.m_Groups[text2] = this.m_NumberGroup[text2];
			}
			foreach (string text3 in this.m_TextureGroup.dict.Keys)
			{
				this.m_Groups[text3] = this.m_TextureGroup[text3];
			}
			foreach (string text4 in this.m_SpherePointGroup.dict.Keys)
			{
				this.m_Groups[text4] = this.m_SpherePointGroup[text4];
			}
			foreach (string text5 in this.m_BoolGroup.dict.Keys)
			{
				this.m_Groups[text5] = this.m_BoolGroup[text5];
			}
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x0002E964 File Offset: 0x0002CB64
		public IEnumerator<string> GetEnumerator()
		{
			return this.m_Groups.Keys.GetEnumerator();
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0002E97B File Offset: 0x0002CB7B
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000B63 RID: 2915
		[NonSerialized]
		private Dictionary<string, IKeyframeGroup> m_Groups = new Dictionary<string, IKeyframeGroup>();

		// Token: 0x04000B64 RID: 2916
		[SerializeField]
		private ColorGroupDictionary m_ColorGroup = new ColorGroupDictionary();

		// Token: 0x04000B65 RID: 2917
		[SerializeField]
		private NumberGroupDictionary m_NumberGroup = new NumberGroupDictionary();

		// Token: 0x04000B66 RID: 2918
		[SerializeField]
		private TextureGroupDictionary m_TextureGroup = new TextureGroupDictionary();

		// Token: 0x04000B67 RID: 2919
		[SerializeField]
		private SpherePointGroupDictionary m_SpherePointGroup = new SpherePointGroupDictionary();

		// Token: 0x04000B68 RID: 2920
		[SerializeField]
		private BoolGroupDictionary m_BoolGroup = new BoolGroupDictionary();
	}
}
