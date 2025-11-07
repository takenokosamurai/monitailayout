using monitailayout.Services;
using monitailayout.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace monitailayout
{
    public partial class MainWindow : Window
    {
        private bool isSidebarCollapsed = false;

        public MainWindow()
        {
            InitializeComponent();

            // NavigationService の初期化
            NavigationService.Instance.Initialize(MainFrame);

            // 初期ページをホームに設定
            NavigationService.Instance.NavigateTo<HomePage>();

            // 初期状態でのツールチップ設定
            UpdateMenuButtonToolTip();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleSidebar();
        }

        private void ToggleSidebar()
        {
            // アニメーションの設定
            var duration = TimeSpan.FromSeconds(0.3);
            var ease = new CubicEase { EasingMode = EasingMode.EaseOut };

            if (isSidebarCollapsed)
            {
                // サイドバーを開く
                AnimateSidebarWidth(240, duration, ease);
                AnimateIcon(false, duration);
                ShowMenuText(true, duration);
                isSidebarCollapsed = false;
            }
            else
            {
                // サイドバーを閉じる
                AnimateSidebarWidth(80, duration, ease);
                AnimateIcon(true, duration);
                ShowMenuText(false, duration);
                isSidebarCollapsed = true;
            }

            // ツールチップを更新
            UpdateMenuButtonToolTip();
        }

        // メニューボタンのツールチップを更新
        private void UpdateMenuButtonToolTip()
        {
            if (MenuButton != null && MenuButton.ToolTip is ToolTip tooltip)
            {
                tooltip.Content = isSidebarCollapsed ? "メニューを開く" : "メニューを閉じる";
            }
        }

        // サイドバーの幅をアニメーション
        private void AnimateSidebarWidth(double toWidth, TimeSpan duration, IEasingFunction ease)
        {
            var animation = new GridLengthAnimation
            {
                From = SidebarColumn.Width,
                To = new GridLength(toWidth),
                Duration = duration,
                EasingFunction = ease
            };

            SidebarColumn.BeginAnimation(ColumnDefinition.WidthProperty, animation);
        }

        // メニューアイコンの変形アニメーション（×↔ ハンバーガー）
        private void AnimateIcon(bool toClose, TimeSpan duration)
        {
            var closeOpacity = new DoubleAnimation
            {
                To = toClose ? 0 : 1,
                Duration = duration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            var hamburgerOpacity = new DoubleAnimation
            {
                To = toClose ? 1 : 0,
                Duration = duration,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            HamburgerIcon.BeginAnimation(UIElement.OpacityProperty, hamburgerOpacity);
            CloseIcon.BeginAnimation(UIElement.OpacityProperty, closeOpacity);
        }

        // メニューテキストの表示/非表示
        private void ShowMenuText(bool show, TimeSpan duration)
        {
            var animation = new DoubleAnimation
            {
                To = show ? 1 : 0,
                Duration = duration
            };

            HomeText.BeginAnimation(UIElement.OpacityProperty, animation);
            ScheduleText.BeginAnimation(UIElement.OpacityProperty, animation);
            ReportText.BeginAnimation(UIElement.OpacityProperty, animation);
            HistoryText.BeginAnimation(UIElement.OpacityProperty, animation);
            SettingsText.BeginAnimation(UIElement.OpacityProperty, animation);
            HelpText.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                // すべてのボタンスタイルをリセット
                ResetNavigationButtons();

                // クリックされたボタンを選択状態に
                button.Style = (Style)FindResource("SelectedNavigationButtonStyle");

                // ページ遷移
                string pageTag = button.Tag?.ToString();
                NavigateToPage(pageTag);
            }
        }

        private void ResetNavigationButtons()
        {
            var normalStyle = (Style)FindResource("NavigationButtonStyle");
            HomeButton.Style = normalStyle;
            ScheduleButton.Style = normalStyle;
            ReportButton.Style = normalStyle;
            HistoryButton.Style = normalStyle;
            SettingsButton.Style = normalStyle;
            HelpButton.Style = normalStyle;
        }

        private void NavigateToPage(string pageName)
        {
            switch (pageName)
            {
                case "Home":
                    NavigationService.Instance.NavigateTo<HomePage>();
                    break;
                case "Schedule":
                    NavigationService.Instance.NavigateTo<SchedulePage>();
                    break;
                case "Report":
                    MessageBox.Show("レポートページは準備中です", "MonitAI");
                    break;
                case "History":
                    MessageBox.Show("履歴ページは準備中です", "MonitAI");
                    break;
                case "Settings":
                    MessageBox.Show("設定ページは準備中です", "MonitAI");
                    break;
                case "Help":
                    MessageBox.Show("ヘルプページは準備中です", "MonitAI");
                    break;
            }
        }
    }

    // GridLengthAnimation クラス（WPF には標準で存在しないため自作）
    public class GridLengthAnimation : AnimationTimeline
    {
        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From", typeof(GridLength), typeof(GridLengthAnimation));

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof(GridLength), typeof(GridLengthAnimation));

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(GridLengthAnimation));

        public GridLength From
        {
            get => (GridLength)GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        public GridLength To
        {
            get => (GridLength)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        public override Type TargetPropertyType => typeof(GridLength);

        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            double fromValue = From.Value;
            double toValue = To.Value;

            if (animationClock.CurrentProgress.HasValue)
            {
                double progress = animationClock.CurrentProgress.Value;

                if (EasingFunction != null)
                {
                    progress = EasingFunction.Ease(progress);
                }

                double currentValue = fromValue + (toValue - fromValue) * progress;
                return new GridLength(currentValue);
            }

            return From;
        }
    }
}