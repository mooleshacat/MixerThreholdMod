using System;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000215 RID: 533
	public static class CharacterSystemUpdater
	{
		// Token: 0x06000BB8 RID: 3000 RVA: 0x0003606C File Offset: 0x0003426C
		[RuntimeInitializeOnLoadMethod]
		private static void updateCharacters()
		{
			CharacterSystemUpdater.UpdateCharactersOnScene(false, null);
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00036078 File Offset: 0x00034278
		public static void UpdateCharactersOnScene(bool revertPrefabs = false, CharacterCustomization reverbObject = null)
		{
			CharacterCustomization[] array = UnityEngine.Object.FindObjectsOfType<CharacterCustomization>();
			if (array == null)
			{
				return;
			}
			foreach (CharacterCustomization characterCustomization in array)
			{
				if (!(characterCustomization == null))
				{
					characterCustomization.InitColors();
				}
			}
		}
	}
}
