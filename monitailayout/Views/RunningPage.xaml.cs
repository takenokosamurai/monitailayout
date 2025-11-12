using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace monitailayout.Views
{
    public partial class RunningPage : Page
    {
        private string goal;
        private DateTime endTime;
        private DispatcherTimer timer;
        private bool isPaused = false;
        private TimeSpan pausedRemaining;

        // レベルシステム (表示用のサンプル)
        private int level = 5;
        private int lcount = 3;

        public RunningPage(string goal, int days, int hours, int minutes)
        {
            InitializeComponent();

            this.goal = goal;
            this.endTime = DateTime.Now.AddDays(days).AddHours(hours).AddMinutes(minutes);

            GoalText.Text = goal;
            LevelText.Text = $"Level {level} - レベルUPまで {lcount}回";

            // タイマー開始
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateRemainingTime();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                UpdateRemainingTime();
            }
        }

        private void UpdateRemainingTime()
        {
            TimeSpan remaining;

            if (isPaused)
            {
                remaining = pausedRemaining;
            }
            else
            {
                remaining = endTime - DateTime.Now;
            }

            if (remaining.TotalSeconds <= 0)
            {
                timer.Stop();
                MessageBox.Show("お疲れ様でした!目標を達成しました!", "MonitAI", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
                return;
            }

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

            // 詳細表示
            RemainingDetailText.Text = $"{days}日 {hours}時間 {minutes}分 {seconds}秒";
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPaused)
            {
                // 再開
                isPaused = false;
                endTime = DateTime.Now.Add(pausedRemaining);
                PauseButton.Content = "一時停止";
            }
            else
            {
                // 一時停止
                isPaused = true;
                pausedRemaining = endTime - DateTime.Now;
                PauseButton.Content = "再開";
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("本当に中止しますか?", "MonitAI", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                timer.Stop();
                NavigationService.GoBack();
            }
        }
    }
}