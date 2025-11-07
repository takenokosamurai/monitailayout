using System.Windows;
using System.Windows.Controls;

namespace monitailayout.Views
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void NewTask_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("新規タスク作成", "MonitAI");
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("データを更新しました", "MonitAI");
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("データをエクスポートします", "MonitAI");
        }
    }
}