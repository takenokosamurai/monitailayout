using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace monitailayout.Views
{
    public partial class RunningPage : Page
    {
        private string goal;
        private DateTime startTime;
        private DateTime endTime;
        private DispatcherTimer timer;

        // レベルシステム (表示用のサンプル)
        private int level = 5;
        private int lcount = 3;

        public RunningPage(string goal, int days, int hours, int minutes)
        {
            InitializeComponent();

            this.goal = goal;
            this.startTime = DateTime.Now;
            this.endTime = startTime.AddDays(days).AddHours(hours).AddMinutes(minutes);

            GoalText.Text = goal;
            LevelText.Text = $"Level {level}";
            LevelProgressText.Text = $"レベルUPまで {lcount}回";

            // タイマー開始
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateRemainingTime();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            UpdateRemainingTime();
        }

        private void UpdateRemainingTime()
        {
            TimeSpan remaining = endTime - DateTime.Now;

            if (remaining.TotalSeconds <= 0)
            {
                timer.Stop();
                MessageBox.Show("お疲れ様でした!目標を達成しました!", "MonitAI", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.GoBack();
                return;
            }

            // 残り時間の計算
            int days = (int)remaining.TotalDays;
            int hours = remaining.Hours;
            int minutes = remaining.Minutes;
            int seconds = remaining.Seconds;

            // HH:MM:SS形式
            if (days > 0)
            {
                RemainingTimeText.Text = $"{days * 24 + hours:00}:{minutes:00}:{seconds:00}";
            }
            else
            {
                RemainingTimeText.Text = $"{hours:00}:{minutes:00}:{seconds:00}";
            }
        }
    }
}