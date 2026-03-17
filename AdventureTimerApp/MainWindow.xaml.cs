using System;
using System.Windows;
using System.Timers;
using Timer = System.Timers.Timer;

namespace AdventureTimerApp
{
	public partial class MainWindow : Window
	{
		private int stamina = 10;          // Player stamina points
		private int secondsPassed = 0;     // Time tracker
		private Timer gameTimer;           // Game timer

		public MainWindow()
		{
			InitializeComponent();
			SetupTimer();
		}

		// Create a timer that ticks every second
		private void SetupTimer()
		{
			gameTimer = new Timer(1000);
			gameTimer.Elapsed += GameLoop;   // Calls GameLoop every 1 second
			gameTimer.Start();
		}

		// Function called each second
		private void GameLoop(object sender, ElapsedEventArgs e)
		{
			secondsPassed++;

			// Every 5 seconds, stamina drops by 1
			if (secondsPassed % 5 == 0)
			{
				stamina--;
				Dispatcher.Invoke(() =>
				{
					LogList.Items.Add($"Time passes... stamina now {stamina}");
					UpdateStamina();
				});
			}

			// Check if stamina has reached 0
			if (stamina <= 0)
			{
				gameTimer.Stop();
				Dispatcher.Invoke(() =>
				{
					ResultLabel.Text = "You have collapsed from exhaustion!";
					LogList.Items.Add("Game Over.");
				});
			}
		}

		// Called when Explore button is pressed
		private void ExploreButton_Click(object sender, RoutedEventArgs e)
		{
			int reward = ExploreArea();   // Call a function that returns a value
			stamina -= 2;                 // Exploring costs stamina
			stamina += reward;            // Add any reward found
			LogList.Items.Add($"You explored and found {reward} stamina!");
			UpdateStamina();
		}

		// Function that returns a random reward between 0 and 3
		private int ExploreArea()
		{
			Random rnd = new Random();
			return rnd.Next(0, 4);
		}

		// Called when Rest button is pressed
		private void RestButton_Click(object sender, RoutedEventArgs e)
		{
			stamina += 3;
			LogList.Items.Add("You rested and regained energy.");
			UpdateStamina();
		}

		// Function updates label text safely
		private void UpdateStamina()
		{
			StaminaLabel.Text = $"Stamina: {stamina}";
		}
	}
}