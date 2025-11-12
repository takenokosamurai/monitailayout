using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace monitailayout.Views
{
    public partial class HomePage : Page
    {
        private bool isDurationMode = true;
        private DispatcherTimer updateTimer;
        private Stack<TimeInputState> undoStack = new Stack<TimeInputState>();
        private List<HistoryItem> historyList = new List<HistoryItem>();

        // 履歴ファイルのパス
        private static readonly string AppDataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "MonitAI"
        );
        private static readonly string HistoryFilePath = Path.Combine(AppDataFolder, "history.json");

        public HomePage()
        {
            InitializeComponent();
            LoadHistory();

            DaysTextBox.Text = "00";
            HoursTextBox.Text = "00";
            MinutesTextBox.Text = "00";

            this.Loaded += (s, e) =>
            {
                UpdateCalculatedTime();

                updateTimer = new DispatcherTimer();
                updateTimer.Interval = TimeSpan.FromSeconds(1);
                updateTimer.Tick += (sender, args) => UpdateCalculatedTime();
                updateTimer.Start();
            };
        }

        // 履歴データモデル
        public class HistoryItem
        {
            public string Goal { get; set; }
            public int Days { get; set; }
            public int Hours { get; set; }
            public int Minutes { get; set; }
            public string TimeDisplay => $"{Days}日 {Hours}時間 {Minutes}分";
            public DateTime CreatedAt { get; set; }
        }

        // Undo用の状態保存
        private class TimeInputState
        {
            public int Days { get; set; }
            public int Hours { get; set; }
            public int Minutes { get; set; }
        }

        // 履歴を読み込む（JSONファイルから）
        private void LoadHistory()
        {
            try
            {
                // ファイルが存在する場合
                if (File.Exists(HistoryFilePath))
                {
                    string json = File.ReadAllText(HistoryFilePath);
                    historyList = JsonSerializer.Deserialize<List<HistoryItem>>(json) ?? new List<HistoryItem>();

                    // 作成日時で降順ソート（最新が上）
                    historyList = historyList.OrderByDescending(x => x.CreatedAt).ToList();
                }
                else
                {
                    // 初回起動時は空のリスト
                    historyList = new List<HistoryItem>();
                }
            }
            catch (Exception ex)
            {
                // エラー時は空のリストで継続
                MessageBox.Show($"履歴の読み込みに失敗しました: {ex.Message}", "MonitAI",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                historyList = new List<HistoryItem>();
            }
        }

        // 履歴を保存（JSONファイルへ）
        private void SaveHistory(HistoryItem item)
        {
            try
            {
                // リストの先頭に追加
                historyList.Insert(0, item);

                // 最大10件まで保持
                if (historyList.Count > 10)
                {
                    historyList = historyList.Take(10).ToList();
                }

                // フォルダが存在しない場合は作成
                if (!Directory.Exists(AppDataFolder))
                {
                    Directory.CreateDirectory(AppDataFolder);
                }

                // JSONにシリアライズして保存
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,  // 読みやすい形式
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping  // 日本語をそのまま保存
                };

                string json = JsonSerializer.Serialize(historyList, options);
                File.WriteAllText(HistoryFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"履歴の保存に失敗しました: {ex.Message}", "MonitAI",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // 以下、既存のメソッドはそのまま...

        // 履歴ボタンクリック
        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryListBox.ItemsSource = null;

            if (historyList.Count > 0)
            {
                HistoryListBox.ItemsSource = historyList;
                NoHistoryText.Visibility = Visibility.Collapsed;
            }
            else
            {
                NoHistoryText.Visibility = Visibility.Visible;
            }

            HistoryDialogHost.IsOpen = true;
        }

        // 履歴ダイアログを閉じる
        private void CloseHistoryDialog_Click(object sender, RoutedEventArgs e)
        {
            HistoryDialogHost.IsOpen = false;
        }

        // 履歴アイテムをクリック
        private void HistoryItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is HistoryItem item)
            {
                GoalTextBox.Text = item.Goal;

                if (isDurationMode)
                {
                    DaysTextBox.Text = item.Days.ToString("00");
                    HoursTextBox.Text = item.Hours.ToString("00");
                    MinutesTextBox.Text = item.Minutes.ToString("00");
                }
                else
                {
                    var targetTime = DateTime.Now.AddDays(item.Days).AddHours(item.Hours).AddMinutes(item.Minutes);
                    DaysTextBox.Text = targetTime.Day.ToString("00");
                    HoursTextBox.Text = targetTime.Hour.ToString("00");
                    MinutesTextBox.Text = targetTime.Minute.ToString("00");
                    ValidateEndTime();
                }

                HistoryDialogHost.IsOpen = false;
            }
        }

        // モード切り替え
        private void SwitchMode_Click(object sender, RoutedEventArgs e)
        {
            isDurationMode = !isDurationMode;

            if (isDurationMode)
            {
                // 期間入力モードに切り替え
                TimeModeLabel.Text = "やる";
                ResultLabel.Text = "完了予定時刻";
                SwitchModeText.Text = "終了時刻で入力";

                if (int.TryParse(DaysTextBox.Text, out int day) &&
                    int.TryParse(HoursTextBox.Text, out int hour) &&
                    int.TryParse(MinutesTextBox.Text, out int minute))
                {
                    var targetTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day, hour, minute, 0);
                    var duration = targetTime - DateTime.Now;

                    if (duration.TotalMinutes > 0)
                    {
                        DaysTextBox.Text = ((int)duration.TotalDays).ToString("00");
                        HoursTextBox.Text = duration.Hours.ToString("00");
                        MinutesTextBox.Text = duration.Minutes.ToString("00");
                    }
                    else
                    {
                        DaysTextBox.Text = "00";
                        HoursTextBox.Text = "00";
                        MinutesTextBox.Text = "00";
                    }
                }
            }
            else
            {
                // 終了時刻入力モードに切り替え
                TimeModeLabel.Text = "まで";
                ResultLabel.Text = "実行時間";
                SwitchModeText.Text = "期間で入力";

                var now = DateTime.Now;
                DaysTextBox.Text = now.Day.ToString("00");
                HoursTextBox.Text = now.Hour.ToString("00");
                MinutesTextBox.Text = now.Minute.ToString("00");
            }

            UpdateCalculatedTime();
        }

        // 月の最終日を取得
        private int GetLastDayOfMonth()
        {
            var now = DateTime.Now;
            return DateTime.DaysInMonth(now.Year, now.Month);
        }

        // 終了時刻の妥当性チェックと自動補正
        private void ValidateEndTime()
        {
            if (!isDurationMode)
            {
                var now = DateTime.Now;
                int day = int.Parse(DaysTextBox.Text);
                int hour = int.Parse(HoursTextBox.Text);
                int minute = int.Parse(MinutesTextBox.Text);

                int lastDay = GetLastDayOfMonth();

                // 月末を超えた場合
                if (day > lastDay)
                {
                    DaysTextBox.Text = lastDay.ToString("00");
                    MonthEndDialogHost.IsOpen = true;
                    return;
                }

                // 現在日より前の場合
                if (day < now.Day)
                {
                    DaysTextBox.Text = now.Day.ToString("00");
                }

                var targetTime = new DateTime(now.Year, now.Month, day, hour, minute, 0);

                // 過去の日時の場合は現在時刻に補正
                if (targetTime < now)
                {
                    DaysTextBox.Text = now.Day.ToString("00");
                    HoursTextBox.Text = now.Hour.ToString("00");
                    MinutesTextBox.Text = now.Minute.ToString("00");
                }
            }
        }

        // 日のフォーカス喪失時のバリデーション
        private void DaysTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DaysTextBox.Text))
            {
                DaysTextBox.Text = "00";
            }

            int value = int.Parse(DaysTextBox.Text);

            if (isDurationMode)
            {
                // 期間モード: 月の最終日まで
                int lastDay = GetLastDayOfMonth();
                if (value > lastDay)
                {
                    DaysTextBox.Text = lastDay.ToString("00");
                }
            }
            else
            {
                ValidateEndTime();
            }
        }

        // 時のフォーカス喪失時のバリデーション
        private void HoursTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(HoursTextBox.Text))
            {
                HoursTextBox.Text = "00";
            }

            int value = int.Parse(HoursTextBox.Text);

            if (value > 23)
            {
                HoursTextBox.Text = "23";
            }

            if (!isDurationMode)
            {
                ValidateEndTime();
            }
        }

        // 分のフォーカス喪失時のバリデーション
        private void MinutesTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MinutesTextBox.Text))
            {
                MinutesTextBox.Text = "00";
            }

            int value = int.Parse(MinutesTextBox.Text);

            if (value > 59)
            {
                MinutesTextBox.Text = "59";
            }

            if (!isDurationMode)
            {
                ValidateEndTime();
            }
        }

        // ステッパーボタン: 日を増やす
        private void IncrementDays_Click(object sender, RoutedEventArgs e)
        {
            int value = int.Parse(DaysTextBox.Text);

            if (isDurationMode)
            {
                // 期間モード: 月の最終日まで
                int lastDay = GetLastDayOfMonth();
                if (value < lastDay)
                {
                    DaysTextBox.Text = (value + 1).ToString("00");
                }
                else
                {
                    DaysTextBox.Text = lastDay.ToString("00");
                }
            }
            else
            {
                // 終了時刻モード: 現在日〜月末
                int lastDay = GetLastDayOfMonth();
                if (value < lastDay)
                {
                    DaysTextBox.Text = (value + 1).ToString("00");
                }
                else
                {
                    MonthEndDialogHost.IsOpen = true;
                }
            }
        }

        // ステッパーボタン: 日を減らす
        private void DecrementDays_Click(object sender, RoutedEventArgs e)
        {
            int value = int.Parse(DaysTextBox.Text);

            if (isDurationMode)
            {
                // 期間モード: 0まで
                if (value > 0)
                {
                    DaysTextBox.Text = (value - 1).ToString("00");
                }
            }
            else
            {
                // 終了時刻モード: 現在日まで
                var now = DateTime.Now;
                if (value > now.Day)
                {
                    DaysTextBox.Text = (value - 1).ToString("00");
                }
            }
        }

        // ステッパーボタン: 時を増やす
        private void IncrementHours_Click(object sender, RoutedEventArgs e)
        {
            int days = int.Parse(DaysTextBox.Text);
            int hours = int.Parse(HoursTextBox.Text);

            if (isDurationMode)
            {
                // 期間モード: 繰り上がり処理
                if (hours == 23)
                {
                    int lastDay = GetLastDayOfMonth();
                    if (days < lastDay)
                    {
                        DaysTextBox.Text = (days + 1).ToString("00");
                        HoursTextBox.Text = "00";
                    }
                    else
                    {
                        // 最終日で止める
                        HoursTextBox.Text = "23";
                    }
                }
                else
                {
                    HoursTextBox.Text = (hours + 1).ToString("00");
                }
            }
            else
            {
                // 終了時刻モード: 0-23で循環
                HoursTextBox.Text = ((hours + 1) % 24).ToString("00");
                ValidateEndTime();
            }
        }

        // ステッパーボタン: 時を減らす
        private void DecrementHours_Click(object sender, RoutedEventArgs e)
        {
            int days = int.Parse(DaysTextBox.Text);
            int hours = int.Parse(HoursTextBox.Text);

            if (isDurationMode)
            {
                // 期間モード: 繰り下がり処理
                if (hours == 0)
                {
                    if (days > 0)
                    {
                        DaysTextBox.Text = (days - 1).ToString("00");
                        HoursTextBox.Text = "23";
                    }
                }
                else
                {
                    HoursTextBox.Text = (hours - 1).ToString("00");
                }
            }
            else
            {
                // 終了時刻モード: 0-23で循環
                HoursTextBox.Text = ((hours - 1 + 24) % 24).ToString("00");
                ValidateEndTime();
            }
        }

        // ステッパーボタン: 分を増やす
        private void IncrementMinutes_Click(object sender, RoutedEventArgs e)
        {
            int days = int.Parse(DaysTextBox.Text);
            int hours = int.Parse(HoursTextBox.Text);
            int minutes = int.Parse(MinutesTextBox.Text);

            if (isDurationMode)
            {
                // 期間モード: 繰り上がり処理
                if (minutes == 59)
                {
                    MinutesTextBox.Text = "00";

                    // 時を増やす
                    if (hours == 23)
                    {
                        int lastDay = GetLastDayOfMonth();
                        if (days < lastDay)
                        {
                            DaysTextBox.Text = (days + 1).ToString("00");
                            HoursTextBox.Text = "00";
                        }
                        else
                        {
                            // 最終日で止める
                            HoursTextBox.Text = "23";
                            MinutesTextBox.Text = "59";
                        }
                    }
                    else
                    {
                        HoursTextBox.Text = (hours + 1).ToString("00");
                    }
                }
                else
                {
                    MinutesTextBox.Text = (minutes + 1).ToString("00");
                }
            }
            else
            {
                // 終了時刻モード: 0-59で循環
                MinutesTextBox.Text = ((minutes + 1) % 60).ToString("00");
                ValidateEndTime();
            }
        }

        // ステッパーボタン: 分を減らす
        private void DecrementMinutes_Click(object sender, RoutedEventArgs e)
        {
            int days = int.Parse(DaysTextBox.Text);
            int hours = int.Parse(HoursTextBox.Text);
            int minutes = int.Parse(MinutesTextBox.Text);

            if (isDurationMode)
            {
                // 期間モード: 繰り下がり処理
                if (minutes == 0)
                {
                    MinutesTextBox.Text = "59";

                    // 時を減らす
                    if (hours == 0)
                    {
                        if (days > 0)
                        {
                            DaysTextBox.Text = (days - 1).ToString("00");
                            HoursTextBox.Text = "23";
                        }
                        else
                        {
                            // 0で止める
                            HoursTextBox.Text = "00";
                            MinutesTextBox.Text = "00";
                        }
                    }
                    else
                    {
                        HoursTextBox.Text = (hours - 1).ToString("00");
                    }
                }
                else
                {
                    MinutesTextBox.Text = (minutes - 1).ToString("00");
                }
            }
            else
            {
                // 終了時刻モード: 0-59で循環
                MinutesTextBox.Text = ((minutes - 1 + 60) % 60).ToString("00");
                ValidateEndTime();
            }
        }

        // 月末超過ダイアログを閉じる
        private void CloseMonthEndDialog_Click(object sender, RoutedEventArgs e)
        {
            MonthEndDialogHost.IsOpen = false;
        }

        // スケジュール設定へ遷移
        private void GoToSchedule_Click(object sender, RoutedEventArgs e)
        {
            MonthEndDialogHost.IsOpen = false;
            Services.NavigationService.Instance.NavigateTo<SchedulePage>();
        }

        // クイック時間追加
        private void QuickAdd_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Tag.ToString(), out int minutesToAdd))
            {
                SaveCurrentStateForUndo();

                int currentDays = int.Parse(DaysTextBox.Text);
                int currentHours = int.Parse(HoursTextBox.Text);
                int currentMinutes = int.Parse(MinutesTextBox.Text);

                if (isDurationMode)
                {
                    // 期間モード: 時間を追加（繰り上がり処理）
                    currentMinutes += minutesToAdd;
                    currentHours += currentMinutes / 60;
                    currentMinutes %= 60;
                    currentDays += currentHours / 24;
                    currentHours %= 24;

                    // 月末チェック
                    int lastDay = GetLastDayOfMonth();
                    if (currentDays > lastDay)
                    {
                        currentDays = lastDay;
                        currentHours = 23;
                        currentMinutes = 59;
                        MonthEndDialogHost.IsOpen = true;
                    }

                    DaysTextBox.Text = currentDays.ToString("00");
                    HoursTextBox.Text = currentHours.ToString("00");
                    MinutesTextBox.Text = currentMinutes.ToString("00");
                }
                else
                {
                    // 終了時刻モード: 終了時刻を遅らせる
                    var now = DateTime.Now;
                    var targetTime = new DateTime(now.Year, now.Month, currentDays, currentHours, currentMinutes, 0)
                        .AddMinutes(minutesToAdd);

                    if (targetTime.Month != now.Month)
                    {
                        int lastDay = GetLastDayOfMonth();
                        targetTime = new DateTime(now.Year, now.Month, lastDay, 23, 59, 0);
                        MonthEndDialogHost.IsOpen = true;
                    }

                    DaysTextBox.Text = targetTime.Day.ToString("00");
                    HoursTextBox.Text = targetTime.Hour.ToString("00");
                    MinutesTextBox.Text = targetTime.Minute.ToString("00");
                }
            }
        }

        // 数値入力のみ許可
        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }

        // キーボード入力処理
        private void TimeInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;

            if (e.Key == Key.Back && textBox != null)
            {
                SaveCurrentStateForUndo();
                textBox.Text = "00";
                textBox.SelectAll();
                e.Handled = true;
            }
            else if (e.Key == Key.Enter && textBox != null)
            {
                // Enterキーでバリデーション実行
                if (textBox == DaysTextBox)
                {
                    DaysTextBox_LostFocus(sender, e);
                }
                else if (textBox == HoursTextBox)
                {
                    HoursTextBox_LostFocus(sender, e);
                }
                else if (textBox == MinutesTextBox)
                {
                    MinutesTextBox_LostFocus(sender, e);
                }
            }
            else if (e.Key == Key.Z && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (undoStack.Count > 0)
                {
                    var state = undoStack.Pop();
                    DaysTextBox.Text = state.Days.ToString("00");
                    HoursTextBox.Text = state.Hours.ToString("00");
                    MinutesTextBox.Text = state.Minutes.ToString("00");
                }
                e.Handled = true;
            }
        }

        // フォーカス時に全選択
        private void TimeInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        // テキスト変更時
        private void TimeInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CalculatedTimeText != null)
            {
                UpdateCalculatedTime();
            }
        }

        // Undo用に現在の状態を保存
        private void SaveCurrentStateForUndo()
        {
            var state = new TimeInputState
            {
                Days = int.TryParse(DaysTextBox.Text, out int d) ? d : 0,
                Hours = int.TryParse(HoursTextBox.Text, out int h) ? h : 0,
                Minutes = int.TryParse(MinutesTextBox.Text, out int m) ? m : 0
            };

            undoStack.Push(state);

            if (undoStack.Count > 3)
            {
                var tempStack = new Stack<TimeInputState>(undoStack.Reverse().Take(3).Reverse());
                undoStack = tempStack;
            }
        }

        // 計算結果を更新
        private void UpdateCalculatedTime()
        {
            if (DaysTextBox == null || HoursTextBox == null || MinutesTextBox == null ||
                CalculatedTimeText == null || TimeModeLabel == null)
            {
                return;
            }

            int days = int.TryParse(DaysTextBox.Text, out int d) ? d : 0;
            int hours = int.TryParse(HoursTextBox.Text, out int h) ? h : 0;
            int minutes = int.TryParse(MinutesTextBox.Text, out int m) ? m : 0;

            if (isDurationMode)
            {
                // 期間 → 終了時刻を計算
                var endTime = DateTime.Now.AddDays(days).AddHours(hours).AddMinutes(minutes);
                CalculatedTimeText.Text = endTime.ToString("yyyy年MM月dd日 HH時mm分");
                CalculatedTimeText.Foreground = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#00897B"));
            }
            else
            {
                // 終了時刻 → 期間を計算
                var now = DateTime.Now;

                try
                {
                    var targetTime = new DateTime(now.Year, now.Month, days, hours, minutes, 0);
                    var duration = targetTime - now;

                    if (duration.TotalMinutes < 0)
                    {
                        CalculatedTimeText.Text = "過去の時刻です";
                        CalculatedTimeText.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                    }
                    else
                    {
                        int durationDays = (int)duration.TotalDays;
                        int durationHours = duration.Hours;
                        int durationMinutes = duration.Minutes;
                        CalculatedTimeText.Text = $"{durationDays}日 {durationHours}時間 {durationMinutes}分やる";
                        CalculatedTimeText.Foreground = new System.Windows.Media.SolidColorBrush(
                            (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#00897B"));
                    }
                }
                catch
                {
                    CalculatedTimeText.Text = "無効な日付です";
                    CalculatedTimeText.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                }
            }
        }

        // スタートボタン
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GoalTextBox.Text))
            {
                MessageBox.Show("目標を入力してください", "MonitAI", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int days = int.Parse(DaysTextBox.Text);
            int hours = int.Parse(HoursTextBox.Text);
            int minutes = int.Parse(MinutesTextBox.Text);

            int actualDays, actualHours, actualMinutes;

            if (isDurationMode)
            {
                actualDays = days;
                actualHours = hours;
                actualMinutes = minutes;
            }
            else
            {
                var now = DateTime.Now;
                var targetTime = new DateTime(now.Year, now.Month, days, hours, minutes, 0);
                var duration = targetTime - now;

                if (duration.TotalMinutes <= 0)
                {
                    MessageBox.Show("過去の時刻は設定できません", "MonitAI", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                actualDays = (int)duration.TotalDays;
                actualHours = duration.Hours;
                actualMinutes = duration.Minutes;
            }

            if (actualDays == 0 && actualHours == 0 && actualMinutes == 0)
            {
                MessageBox.Show("時間を設定してください", "MonitAI", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveHistory(new HistoryItem
            {
                Goal = GoalTextBox.Text,
                Days = actualDays,
                Hours = actualHours,
                Minutes = actualMinutes,
                CreatedAt = DateTime.Now
            });

            var runningPage = new RunningPage(GoalTextBox.Text, actualDays, actualHours, actualMinutes);
            NavigationService.Navigate(runningPage);
        }
    }
}