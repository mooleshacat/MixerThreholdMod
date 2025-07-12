using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.TV
{
	// Token: 0x020002AA RID: 682
	public class RunnerGame : TVApp
	{
		// Token: 0x06000E33 RID: 3635 RVA: 0x0003EE24 File Offset: 0x0003D024
		protected override void Awake()
		{
			base.Awake();
			this.defaultCharacterY = this.Character.anchoredPosition.y;
			this.CloudSpawner.OnSpawn.AddListener(new UnityAction<GameObject>(this.CloudSpawned));
			this.ObstacleSpawner.OnSpawn.AddListener(new UnityAction<GameObject>(this.ObstacleSpawned));
			this.StartScreen.SetActive(true);
			this.isReady = true;
			this.GameSpeed = 0f;
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x0003EEA3 File Offset: 0x0003D0A3
		public override void Open()
		{
			base.Open();
			this.RefreshHighScore();
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x0003EEB1 File Offset: 0x0003D0B1
		protected override void TryPause()
		{
			if (this.isReady)
			{
				this.Close();
				return;
			}
			base.TryPause();
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x0003EEC8 File Offset: 0x0003D0C8
		public void Update()
		{
			if (!base.IsOpen)
			{
				return;
			}
			this.Ground.SpeedMultiplier = (base.IsPaused ? 0f : this.GameSpeed);
			this.CharacterFlipboard.SpeedMultiplier = (base.IsPaused ? 0f : this.GameSpeed);
			this.ScoreLabel.text = this.score.ToString("00000");
			for (int i = 0; i < this.clouds.Count; i++)
			{
				if (this.clouds[i] == null || this.clouds[i].gameObject == null)
				{
					this.clouds.RemoveAt(i);
					i--;
				}
				else
				{
					this.clouds[i].SpeedMultiplier = (base.IsPaused ? 0f : this.GameSpeed);
				}
			}
			for (int j = 0; j < this.obstacles.Count; j++)
			{
				if (this.obstacles[j] == null || this.obstacles[j].gameObject == null)
				{
					this.obstacles.RemoveAt(j);
					j--;
				}
				else
				{
					this.obstacles[j].SpeedMultiplier = (base.IsPaused ? 0f : this.GameSpeed);
				}
			}
			float spawnRateMultiplier = Mathf.Sqrt(this.GameSpeed);
			this.ObstacleSpawner.SpawnRateMultiplier = spawnRateMultiplier;
			this.CloudSpawner.SpawnRateMultiplier = spawnRateMultiplier;
			if (this.isReady && (GameInput.GetButtonDown(GameInput.ButtonCode.Jump) || GameInput.GetButtonDown(GameInput.ButtonCode.Forward)))
			{
				this.StartGame();
			}
			if (base.IsPaused || this.GameSpeed == 0f)
			{
				return;
			}
			this.score += (float)this.ScoreRate * Time.deltaTime;
			this.GameSpeed = Mathf.Clamp(this.GameSpeed + this.SpeedIncreaseRate * Time.deltaTime, this.MinGameSpeed, this.MaxGameSpeed);
			if (this.Character.anchoredPosition.y - this.defaultCharacterY > 10f)
			{
				this.CharacterFlipboard.Image.sprite = this.JumpSprite;
				this.CharacterFlipboard.enabled = false;
			}
			else
			{
				this.CharacterFlipboard.enabled = true;
			}
			this.yVelocity -= this.Gravity * this.GlobalForceMultiplier * Time.deltaTime;
			if (this.isJumping && (GameInput.GetButton(GameInput.ButtonCode.Crouch) || GameInput.GetButton(GameInput.ButtonCode.Backward)))
			{
				this.yVelocity -= this.DropForce * this.GlobalForceMultiplier * Time.deltaTime;
			}
			if (this.Character.anchoredPosition.y + this.yVelocity * Time.deltaTime <= this.defaultCharacterY)
			{
				if (this.isJumping)
				{
					this.CharacterFlipboard.SetIndex(0);
				}
				this.Character.anchoredPosition = new Vector2(this.Character.anchoredPosition.x, this.defaultCharacterY);
				this.yVelocity = 0f;
				this.isJumping = false;
				this.isGrounded = true;
			}
			else
			{
				this.Character.anchoredPosition = new Vector2(this.Character.anchoredPosition.x, this.Character.anchoredPosition.y + this.yVelocity * Time.deltaTime);
			}
			if ((GameInput.GetButtonDown(GameInput.ButtonCode.Jump) || GameInput.GetButtonDown(GameInput.ButtonCode.Forward)) && this.isGrounded)
			{
				this.Jump();
			}
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0003F246 File Offset: 0x0003D446
		private void Jump()
		{
			this.isGrounded = false;
			this.isJumping = true;
			this.yVelocity = this.JumpForce * this.GlobalForceMultiplier;
			if (this.onJump != null)
			{
				this.onJump.Invoke();
			}
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0003F27C File Offset: 0x0003D47C
		private void CloudSpawned(GameObject cloud)
		{
			this.clouds.Add(cloud.GetComponent<UIMover>());
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x0003F28F File Offset: 0x0003D48F
		private void ObstacleSpawned(GameObject obstacle)
		{
			this.obstacles.Add(obstacle.GetComponent<UIMover>());
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x0003F2A4 File Offset: 0x0003D4A4
		private void RefreshHighScore()
		{
			float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("RunGameHighScore");
			this.HighScoreLabel.text = value.ToString("00000");
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x0003F2D8 File Offset: 0x0003D4D8
		public void PlayerCollided()
		{
			if (this.isReady)
			{
				return;
			}
			this.EndGame();
			if (this.onHit != null)
			{
				this.onHit.Invoke();
			}
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0003F2FC File Offset: 0x0003D4FC
		private void EndGame()
		{
			float value = NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("RunGameHighScore");
			if (this.score > value)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("RunGameHighScore", this.score.ToString(), true);
				this.NewHighScoreAnimation.Play();
				if (this.onNewHighScore != null)
				{
					this.onNewHighScore.Invoke();
				}
			}
			this.GameOverScreen.SetActive(true);
			this.RefreshHighScore();
			this.GameSpeed = 0f;
			this.isReady = true;
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0003F380 File Offset: 0x0003D580
		private void StartGame()
		{
			this.ResetGame();
			this.GameSpeed = this.MinGameSpeed;
			this.GameOverScreen.SetActive(false);
			this.StartScreen.SetActive(false);
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0003F3AC File Offset: 0x0003D5AC
		private void ResetGame()
		{
			this.score = 0f;
			this.GameSpeed = this.MinGameSpeed;
			this.yVelocity = 0f;
			this.isJumping = false;
			this.isGrounded = true;
			this.isReady = false;
			this.Character.anchoredPosition = new Vector2(this.Character.anchoredPosition.x, this.defaultCharacterY);
			for (int i = 0; i < this.clouds.Count; i++)
			{
				if (this.clouds[i] == null || this.clouds[i].gameObject == null)
				{
					this.clouds.RemoveAt(i);
					i--;
				}
				else
				{
					UnityEngine.Object.Destroy(this.clouds[i].gameObject);
				}
			}
			this.clouds.Clear();
			for (int j = 0; j < this.obstacles.Count; j++)
			{
				if (this.obstacles[j] == null || this.obstacles[j].gameObject == null)
				{
					this.obstacles.RemoveAt(j);
					j--;
				}
				else
				{
					UnityEngine.Object.Destroy(this.obstacles[j].gameObject);
				}
			}
			this.obstacles.Clear();
		}

		// Token: 0x04000EA0 RID: 3744
		public float GameSpeed = 1f;

		// Token: 0x04000EA1 RID: 3745
		public float MinGameSpeed = 1.5f;

		// Token: 0x04000EA2 RID: 3746
		public float MaxGameSpeed = 4f;

		// Token: 0x04000EA3 RID: 3747
		public float SpeedIncreaseRate = 0.1f;

		// Token: 0x04000EA4 RID: 3748
		public int ScoreRate = 50;

		// Token: 0x04000EA5 RID: 3749
		public float Gravity = 9.8f;

		// Token: 0x04000EA6 RID: 3750
		public float JumpForce = 10f;

		// Token: 0x04000EA7 RID: 3751
		public float GlobalForceMultiplier = 20f;

		// Token: 0x04000EA8 RID: 3752
		public float DropForce = 1f;

		// Token: 0x04000EA9 RID: 3753
		public RectTransform Character;

		// Token: 0x04000EAA RID: 3754
		public Flipboard CharacterFlipboard;

		// Token: 0x04000EAB RID: 3755
		public SlidingRect Ground;

		// Token: 0x04000EAC RID: 3756
		public UISpawner CloudSpawner;

		// Token: 0x04000EAD RID: 3757
		public UISpawner ObstacleSpawner;

		// Token: 0x04000EAE RID: 3758
		public TextMeshProUGUI ScoreLabel;

		// Token: 0x04000EAF RID: 3759
		public TextMeshProUGUI HighScoreLabel;

		// Token: 0x04000EB0 RID: 3760
		public GameObject StartScreen;

		// Token: 0x04000EB1 RID: 3761
		public GameObject GameOverScreen;

		// Token: 0x04000EB2 RID: 3762
		public Animation NewHighScoreAnimation;

		// Token: 0x04000EB3 RID: 3763
		public Sprite JumpSprite;

		// Token: 0x04000EB4 RID: 3764
		private bool isJumping;

		// Token: 0x04000EB5 RID: 3765
		private bool isGrounded = true;

		// Token: 0x04000EB6 RID: 3766
		private bool isReady;

		// Token: 0x04000EB7 RID: 3767
		private float score;

		// Token: 0x04000EB8 RID: 3768
		private float yVelocity;

		// Token: 0x04000EB9 RID: 3769
		private float defaultCharacterY;

		// Token: 0x04000EBA RID: 3770
		private List<UIMover> clouds = new List<UIMover>();

		// Token: 0x04000EBB RID: 3771
		private List<UIMover> obstacles = new List<UIMover>();

		// Token: 0x04000EBC RID: 3772
		public UnityEvent onJump;

		// Token: 0x04000EBD RID: 3773
		public UnityEvent onHit;

		// Token: 0x04000EBE RID: 3774
		public UnityEvent onNewHighScore;
	}
}
