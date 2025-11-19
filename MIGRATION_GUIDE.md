# Material Design から Windows Fluent Design への移行ガイド

## 現状分析

現在の `monitailayout` プロジェクトは **Material Design In XAML Toolkit** を使用しています。これは Google が提唱する Material Design に基づいたデザインシステムです。

### 現在の実装の特徴
- Material Design のカラーパレット（Teal & Amber）
- Material Design のコントロール（Card、DialogHost など）
- サイドバーナビゲーション
- カスタムカラー（#2C3E50、#34495E、#3E5771）

## Windows らしい見た目にするための改善案

### 1. カラーパレットの変更

#### 現在（Material Design）
```xml
PrimaryColor="Teal"         <!-- #009688 -->
SecondaryColor="Amber"      <!-- #FFC107 -->
Sidebar Background="#2C3E50"
TopBar Background="#3E5771"
```

#### 推奨（Windows Fluent Design）
```xml
PrimaryColor="#0078D4"      <!-- Microsoft Blue -->
SecondaryColor="#005A9E"    <!-- Darker Blue for pressed state -->
Sidebar Background="#F3F3F3" (Light) / "#2D2D30" (Dark)
TopBar Background="#FFFFFF" (Light) / "#1E1E1E" (Dark)
```

### 2. ナビゲーションパターン

#### 現在の実装
- 固定幅のサイドバー（240px）
- 折りたたみ機能あり
- Material Design のスタイル

#### Windows 標準パターン（NavigationView）
```
コンパクトモード: 48px（アイコンのみ）
通常モード: 320px（アイコン + テキスト）
自動切替: ウィンドウサイズに応じて自動調整
```

**推奨実装:**
```xml
<!-- WPF UI ライブラリを使用する場合 -->
<ui:NavigationView x:Name="RootNavigation"
                   IsBackButtonVisible="Collapsed"
                   IsPaneToggleButtonVisible="True"
                   PaneDisplayMode="Left">
    <ui:NavigationView.MenuItems>
        <ui:NavigationViewItem Content="ホーム" Icon="Home" Tag="Home"/>
        <ui:NavigationViewItem Content="スケジュール" Icon="Calendar" Tag="Schedule"/>
        <ui:NavigationViewItem Content="レポート" Icon="Document" Tag="Report"/>
    </ui:NavigationView.MenuItems>
</ui:NavigationView>
```

### 3. トップバーの改善

#### 現在の実装
```xml
<TextBlock Text="MonitAI"
           FontSize="32"
           FontWeight="Bold"
           Foreground="White"/>
```

#### Windows 標準パターン
- タイトルバーは通常 32px の高さ
- アプリ名は左寄せ、14-16px のフォントサイズ
- 検索ボックスやユーザーアイコンを右側に配置

**推奨実装:**
```xml
<Grid Height="32" Background="{DynamicResource SystemChromeMediumColor}">
    <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
        <!-- アプリアイコン -->
        <Image Source="icon.png" Width="16" Height="16" Margin="12,0,8,0"/>
        <!-- アプリ名 -->
        <TextBlock Text="MonitAI"
                   FontSize="14"
                   FontWeight="SemiBold"
                   VerticalAlignment="Center"/>
    </StackPanel>
</Grid>
```

### 4. ボタンスタイルの改善

#### 現在の実装（Material Design）
```xml
<Button Style="{StaticResource MaterialDesignRaisedButton}"
        Background="#FFA726"
        BorderBrush="#FFA726"
        Height="110"
        FontSize="26"/>
```

#### Windows 標準スタイル
```xml
<Button Content="スタート"
        Background="#0078D4"
        Foreground="White"
        BorderThickness="1"
        BorderBrush="#0078D4"
        Height="32"
        MinWidth="120"
        Padding="16,8"
        FontSize="14"
        FontFamily="Segoe UI"
        CornerRadius="4">
    <Button.Style>
        <Style TargetType="Button">
            <Style.Triggers>
                <Trigger Property="IsMouseOver" Value="True">
                    <Setter Property="Background" Value="#106EBE"/>
                </Trigger>
                <Trigger Property="IsPressed" Value="True">
                    <Setter Property="Background" Value="#005A9E"/>
                </Trigger>
            </Style.Triggers>
        </Style>
    </Button.Style>
</Button>
```

### 5. カードの改善

#### 現在の実装（Material Design Card）
```xml
<materialDesign:Card Padding="30,20"
                    materialDesign:ElevationAssist.Elevation="Dp4"/>
```

#### Windows スタイル
```xml
<Border Background="{DynamicResource SystemControlBackgroundAltHighBrush}"
        CornerRadius="8"
        Padding="24"
        BorderThickness="1"
        BorderBrush="{DynamicResource SystemControlBackgroundBaseLowBrush}">
    <Border.Effect>
        <DropShadowEffect Color="Black" 
                         Opacity="0.08" 
                         BlurRadius="16" 
                         ShadowDepth="2"/>
    </Border.Effect>
</Border>
```

### 6. 入力フィールドの改善

#### 現在の実装
```xml
<TextBox materialDesign:HintAssist.Hint="目標を入力"
         materialDesign:HintAssist.IsFloating="True"/>
```

#### Windows 標準スタイル
```xml
<Grid>
    <TextBlock Text="目標を入力してください"
               Foreground="#757575"
               Margin="12,8"
               IsHitTestVisible="False"
               Visibility="{Binding Text, ElementName=InputBox, 
                           Converter={StaticResource EmptyStringToVisibilityConverter}}"/>
    <TextBox x:Name="InputBox"
             Padding="12,8"
             BorderThickness="2"
             BorderBrush="#8E8E8E"
             Background="Transparent">
        <TextBox.Style>
            <Style TargetType="TextBox">
                <Style.Triggers>
                    <Trigger Property="IsFocused" Value="True">
                        <Setter Property="BorderBrush" Value="#0078D4"/>
                    </Trigger>
                </Style.Triggers>
            </Style>
        </TextBox.Style>
    </TextBox>
</Grid>
```

## 段階的な移行プラン

### フェーズ 1: カラーパレットの調整（最小限の変更）
1. Material Design のテーマカラーを維持しつつ、補助色を Windows Blue に変更
2. サイドバーとトップバーの背景色を調整
3. アクセントカラーを Microsoft Blue に変更

**変更箇所:**
- `App.xaml`: テーマカラーの調整
- `MainWindow.xaml`: サイドバーとトップバーの背景色

**推定作業時間:** 1-2時間

### フェーズ 2: コントロールの最適化
1. ボタンのサイズと padding を Windows 標準に調整
2. フォントサイズを見直し（大きすぎるものを縮小）
3. スペーシングを 8px の倍数に調整

**変更箇所:**
- `MainWindow.xaml`: ナビゲーションボタンのスタイル
- `HomePage.xaml`: 各種コントロールのサイズ調整

**推定作業時間:** 2-3時間

### フェーズ 3: WPF UI への完全移行（大規模変更）
1. Material Design In XAML Toolkit を削除
2. WPF UI または ModernWpf をインストール
3. すべてのコントロールを Windows 標準スタイルに置き換え
4. NavigationView を実装

**変更箇所:**
- `monitailayout.csproj`: パッケージ参照の変更
- `App.xaml`: テーマ定義の全面刷新
- すべての XAML ファイル: コントロールの置き換え

**推定作業時間:** 1-2週間

## 推奨ライブラリ

### Option 1: WPF UI（推奨）
**メリット:**
- Windows 11 Fluent Design に完全準拠
- 継続的に開発されている
- 豊富なドキュメント
- ダークモード自動対応

**インストール:**
```bash
dotnet add package WPF-UI
```

**基本設定:**
```xml
<!-- App.xaml -->
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ui:ThemesDictionary Theme="Light" />
            <ui:ControlsDictionary />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

### Option 2: ModernWpf
**メリット:**
- Windows 10 スタイル
- 軽量
- Material Design からの移行が比較的容易

**インストール:**
```bash
dotnet add package ModernWpfUI
```

### Option 3: 段階的アプローチ（推奨）
Material Design を維持しつつ、カラーパレットとサイズだけを Windows 標準に近づける。

## 実装例：HomePage の改善

### Before (現在の実装)
```xml
<Button x:Name="StartButton"
        Content="スタート"
        Height="110"
        FontSize="26"
        FontWeight="Bold"
        Background="#FFA726"
        BorderBrush="#FFA726"/>
```

### After (Windows スタイル)
```xml
<Button x:Name="StartButton"
        Content="スタート"
        Height="40"
        MinWidth="200"
        FontSize="16"
        FontWeight="SemiBold"
        Background="#0078D4"
        Foreground="White"
        BorderThickness="0"
        CornerRadius="4"
        Padding="24,8">
    <Button.Effect>
        <DropShadowEffect Color="#0078D4" 
                         Opacity="0.3" 
                         BlurRadius="12" 
                         ShadowDepth="0"/>
    </Button.Effect>
</Button>
```

## テストとバリデーション

移行後に確認すべき項目：

### 視覚的な確認
- [ ] ウィンドウサイズを変更しても正しく表示される
- [ ] ライト/ダークテーマ両方で見やすい
- [ ] 色のコントラストが十分（WCAG 2.1 AA 準拠）
- [ ] Windows 10 と Windows 11 で表示を確認

### 機能的な確認
- [ ] すべてのボタンとコントロールが正常に動作する
- [ ] ナビゲーションが直感的
- [ ] キーボードナビゲーションが機能する
- [ ] タッチ操作が快適（タッチスクリーンがある場合）

### パフォーマンス
- [ ] 起動時間が遅くなっていない
- [ ] アニメーションが滑らか
- [ ] メモリ使用量が増加していない

## まとめ

現在の `monitailayout` を Windows らしい見た目にするには：

1. **最小限の変更（推奨）**: カラーパレットとサイズの調整
   - Material Design を維持しつつ、Microsoft Blue に変更
   - ボタンとフォントサイズを Windows 標準に調整
   - 作業時間: 数時間

2. **完全な移行**: WPF UI への移行
   - Windows 11 Fluent Design を完全に採用
   - すべてのコントロールを置き換え
   - 作業時間: 1-2週間

どちらのアプローチも、`WINDOWS_UI_DESIGN_GUIDELINES.md` に記載された原則に従うことで、Windows ユーザーに馴染みのある UI を実現できます。
