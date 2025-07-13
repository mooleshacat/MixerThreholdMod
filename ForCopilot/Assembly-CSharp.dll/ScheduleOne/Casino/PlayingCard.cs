using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using ScheduleOne.Audio;
using UnityEngine;

namespace ScheduleOne.Casino
{
	// Token: 0x0200079C RID: 1948
	public class PlayingCard : MonoBehaviour
	{
		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x060034CA RID: 13514 RVA: 0x000DCBA1 File Offset: 0x000DADA1
		// (set) Token: 0x060034CB RID: 13515 RVA: 0x000DCBA9 File Offset: 0x000DADA9
		public bool IsFaceUp { get; private set; }

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x060034CC RID: 13516 RVA: 0x000DCBB2 File Offset: 0x000DADB2
		// (set) Token: 0x060034CD RID: 13517 RVA: 0x000DCBBA File Offset: 0x000DADBA
		public PlayingCard.ECardSuit Suit { get; private set; }

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x060034CE RID: 13518 RVA: 0x000DCBC3 File Offset: 0x000DADC3
		// (set) Token: 0x060034CF RID: 13519 RVA: 0x000DCBCB File Offset: 0x000DADCB
		public PlayingCard.ECardValue Value { get; private set; }

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x060034D0 RID: 13520 RVA: 0x000DCBD4 File Offset: 0x000DADD4
		// (set) Token: 0x060034D1 RID: 13521 RVA: 0x000DCBDC File Offset: 0x000DADDC
		public CardController CardController { get; private set; }

		// Token: 0x060034D2 RID: 13522 RVA: 0x000DCBE5 File Offset: 0x000DADE5
		private void OnValidate()
		{
			base.gameObject.name = "PlayingCard (" + this.CardID + ")";
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x000DCC07 File Offset: 0x000DAE07
		public void SetCardController(CardController cardController)
		{
			this.CardController = cardController;
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x000DCC10 File Offset: 0x000DAE10
		public void SetCard(PlayingCard.ECardSuit suit, PlayingCard.ECardValue value, bool network = true)
		{
			if (network && this.CardController != null)
			{
				this.CardController.SendCardValue(this.CardID, suit, value);
				return;
			}
			this.Suit = suit;
			this.Value = value;
			PlayingCard.CardSprite cardSprite = this.GetCardSprite(suit, value);
			if (cardSprite != null)
			{
				this.CardSpriteRenderer.sprite = cardSprite.Sprite;
			}
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x000DCC6D File Offset: 0x000DAE6D
		public void ClearCard()
		{
			this.SetCard(PlayingCard.ECardSuit.Spades, PlayingCard.ECardValue.Blank, true);
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x000DCC78 File Offset: 0x000DAE78
		public void SetFaceUp(bool faceUp, bool network = true)
		{
			if (network && this.CardController != null)
			{
				this.CardController.SendCardFaceUp(this.CardID, faceUp);
			}
			if (this.IsFaceUp == faceUp)
			{
				return;
			}
			this.IsFaceUp = faceUp;
			if (this.IsFaceUp)
			{
				this.FlipAnimation.Play(this.FlipFaceUpClip.name);
			}
			else
			{
				this.FlipAnimation.Play(this.FlipFaceDownClip.name);
			}
			this.FlipSound.Play();
		}

		// Token: 0x060034D7 RID: 13527 RVA: 0x000DCCFC File Offset: 0x000DAEFC
		public void GlideTo(Vector3 position, Quaternion rotation, float duration = 0.5f, bool network = true)
		{
			PlayingCard.<>c__DisplayClass35_0 CS$<>8__locals1 = new PlayingCard.<>c__DisplayClass35_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.duration = duration;
			CS$<>8__locals1.position = position;
			CS$<>8__locals1.rotation = rotation;
			if (network && this.CardController != null)
			{
				this.CardController.SendCardGlide(this.CardID, CS$<>8__locals1.position, CS$<>8__locals1.rotation, CS$<>8__locals1.duration);
				return;
			}
			if (this.lastGlideTarget != null && this.lastGlideTarget.Item1.Equals(CS$<>8__locals1.position) && this.lastGlideTarget.Item2.Equals(CS$<>8__locals1.rotation))
			{
				return;
			}
			this.lastGlideTarget = new Tuple<Vector3, Quaternion>(CS$<>8__locals1.position, CS$<>8__locals1.rotation);
			CS$<>8__locals1.verticalOffset = 0.02f;
			if (this.moveRoutine != null)
			{
				base.StopCoroutine(this.moveRoutine);
			}
			this.moveRoutine = base.StartCoroutine(CS$<>8__locals1.<GlideTo>g__MoveRoutine|0());
		}

		// Token: 0x060034D8 RID: 13528 RVA: 0x000DCDE8 File Offset: 0x000DAFE8
		private PlayingCard.CardSprite GetCardSprite(PlayingCard.ECardSuit suit, PlayingCard.ECardValue val)
		{
			return this.CardSprites.FirstOrDefault((PlayingCard.CardSprite x) => x.Suit == suit && x.Value == val);
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x000DCE20 File Offset: 0x000DB020
		[Button]
		public void VerifyCardSprites()
		{
			List<PlayingCard.CardSprite> list = new List<PlayingCard.CardSprite>(this.CardSprites);
			foreach (object obj in Enum.GetValues(typeof(PlayingCard.ECardSuit)))
			{
				PlayingCard.ECardSuit ecardSuit = (PlayingCard.ECardSuit)obj;
				foreach (object obj2 in Enum.GetValues(typeof(PlayingCard.ECardValue)))
				{
					PlayingCard.ECardValue ecardValue = (PlayingCard.ECardValue)obj2;
					PlayingCard.CardSprite cardSprite = this.GetCardSprite(ecardSuit, ecardValue);
					if (cardSprite == null)
					{
						Debug.LogError(string.Format("Card sprite for {0} {1} is missing.", ecardSuit, ecardValue));
					}
					else if (list.Contains(cardSprite))
					{
						Debug.LogError(string.Format("Card sprite for {0} {1} is duplicated.", ecardSuit, ecardValue));
					}
					else
					{
						list.Add(cardSprite);
					}
				}
			}
		}

		// Token: 0x0400253F RID: 9535
		public string CardID = "card_1";

		// Token: 0x04002540 RID: 9536
		[Header("References")]
		public SpriteRenderer CardSpriteRenderer;

		// Token: 0x04002541 RID: 9537
		public PlayingCard.CardSprite[] CardSprites;

		// Token: 0x04002542 RID: 9538
		public Animation FlipAnimation;

		// Token: 0x04002543 RID: 9539
		public AnimationClip FlipFaceUpClip;

		// Token: 0x04002544 RID: 9540
		public AnimationClip FlipFaceDownClip;

		// Token: 0x04002545 RID: 9541
		[Header("Sound")]
		public AudioSourceController FlipSound;

		// Token: 0x04002546 RID: 9542
		public AudioSourceController LandSound;

		// Token: 0x04002547 RID: 9543
		private Coroutine moveRoutine;

		// Token: 0x04002548 RID: 9544
		private Tuple<Vector3, Quaternion> lastGlideTarget;

		// Token: 0x0200079D RID: 1949
		[Serializable]
		public class CardSprite
		{
			// Token: 0x04002549 RID: 9545
			public PlayingCard.ECardSuit Suit;

			// Token: 0x0400254A RID: 9546
			public PlayingCard.ECardValue Value;

			// Token: 0x0400254B RID: 9547
			public Sprite Sprite;
		}

		// Token: 0x0200079E RID: 1950
		public struct CardData
		{
			// Token: 0x060034DC RID: 13532 RVA: 0x000DCF4F File Offset: 0x000DB14F
			public CardData(PlayingCard.ECardSuit suit, PlayingCard.ECardValue value)
			{
				this.Suit = suit;
				this.Value = value;
			}

			// Token: 0x0400254C RID: 9548
			public PlayingCard.ECardSuit Suit;

			// Token: 0x0400254D RID: 9549
			public PlayingCard.ECardValue Value;
		}

		// Token: 0x0200079F RID: 1951
		public enum ECardSuit
		{
			// Token: 0x0400254F RID: 9551
			Spades,
			// Token: 0x04002550 RID: 9552
			Hearts,
			// Token: 0x04002551 RID: 9553
			Diamonds,
			// Token: 0x04002552 RID: 9554
			Clubs
		}

		// Token: 0x020007A0 RID: 1952
		public enum ECardValue
		{
			// Token: 0x04002554 RID: 9556
			Blank,
			// Token: 0x04002555 RID: 9557
			Ace,
			// Token: 0x04002556 RID: 9558
			Two,
			// Token: 0x04002557 RID: 9559
			Three,
			// Token: 0x04002558 RID: 9560
			Four,
			// Token: 0x04002559 RID: 9561
			Five,
			// Token: 0x0400255A RID: 9562
			Six,
			// Token: 0x0400255B RID: 9563
			Seven,
			// Token: 0x0400255C RID: 9564
			Eight,
			// Token: 0x0400255D RID: 9565
			Nine,
			// Token: 0x0400255E RID: 9566
			Ten,
			// Token: 0x0400255F RID: 9567
			Jack,
			// Token: 0x04002560 RID: 9568
			Queen,
			// Token: 0x04002561 RID: 9569
			King
		}
	}
}
