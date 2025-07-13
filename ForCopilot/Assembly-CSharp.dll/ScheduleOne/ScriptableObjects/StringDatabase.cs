using System;
using UnityEngine;

namespace ScheduleOne.ScriptableObjects
{
	// Token: 0x020007BB RID: 1979
	[CreateAssetMenu(fileName = "StringDatabase", menuName = "ScriptableObjects/StringDatabase", order = 1)]
	[Serializable]
	public class StringDatabase : ScriptableObject
	{
		// Token: 0x04002624 RID: 9764
		[TextArea(2, 10)]
		public string[] Strings;
	}
}
