# Windows UI デザインガイドライン

## 概要
このドキュメントは、Microsoft標準のアプリのような見た目を実現するためのUIデザインの約束事とベストプラクティスをまとめたものです。

## 1. Fluent Design System（フルーエントデザインシステム）

Microsoft の現代的なデザイン言語である Fluent Design System に従うことで、統一感のある Windows アプリケーションを作成できます。

### 1.1 主要な5つの要素

#### Light（光）
- 要素にフォーカスを当て、階層を示すために光を使用
- ホバー時やフォーカス時に視覚的なフィードバックを提供

#### Depth（奥行き）
- レイヤーと影を使って要素間の関係性を表現
- 重要な要素ほど手前に配置し、影を濃くする

#### Motion（動き）
- スムーズで自然なアニメーションを使用
- 状態変化や画面遷移を滑らかに表現
- アニメーションは 200-500ms が推奨

#### Material（素材）
- アクリル効果などの半透明な背景を活用
- 奥行き感を演出するために素材感を表現

#### Scale（スケール）
- タッチ、マウス、ペン、ゲームパッドなど、あらゆる入力方法に対応
- レスポンシブデザインで様々な画面サイズに対応

## 2. レイアウトとスペーシング

### 2.1 グリッドシステム
```
基準単位: 4px
推奨マージン: 8px, 12px, 16px, 20px, 24px
推奨パディング: 8px, 12px, 16px, 20px
```

### 2.2 標準的な余白
- **最小タッチターゲット**: 44×44 px（指でのタッチ操作を考慮）
- **要素間の余白**: 8px または 12px
- **セクション間の余白**: 16px または 24px
- **画面端からの余白**: 20px～40px（画面サイズに応じて調整）

### 2.3 レイアウトパターン
- **ナビゲーションビュー**: 左側にサイドバー、右側にコンテンツ
- **コマンドバー**: 上部に主要なアクション
- **コンテンツエリア**: スクロール可能なメインコンテンツ
- **ダイアログ**: 中央に配置、背景は半透明のオーバーレイ

## 3. カラーパレット

### 3.1 システムカラー
Microsoft が推奨するシステムカラーを使用することで、Windows のテーマ（ライト/ダーク）に自動的に対応できます。

#### 基本カラー
```
プライマリカラー: #0078D4（Microsoft Blue）
ホバー: #106EBE
押下時: #005A9E
無効化: #CCCCCC
```

#### セマンティックカラー
```
成功: #107C10（緑）
警告: #FFA500（オレンジ）
エラー: #D13438（赤）
情報: #0078D4（青）
```

### 3.2 背景とテキスト
```
ライトテーマ:
  - 背景: #FFFFFF, #F3F3F3, #FAFAFA
  - テキスト: #000000 (本文), #666666 (サブテキスト)

ダークテーマ:
  - 背景: #1E1E1E, #252526, #2D2D30
  - テキスト: #FFFFFF (本文), #CCCCCC (サブテキスト)
```

### 3.3 アクセントカラー
ユーザーが設定したアクセントカラーを尊重し、システム設定から取得することを推奨。

## 4. タイポグラフィ

### 4.1 推奨フォント
```
Windows 11: Segoe UI Variable
Windows 10: Segoe UI
日本語: Yu Gothic UI, Meiryo UI
```

### 4.2 フォントサイズとウェイト
```
見出し1: 28px, SemiBold
見出し2: 24px, SemiBold
見出し3: 20px, SemiBold
見出し4: 16px, SemiBold
本文（大）: 15px, Regular
本文（標準）: 14px, Regular
本文（小）: 12px, Regular
キャプション: 11px, Regular
```

### 4.3 行間
- 本文: 1.5（フォントサイズの150%）
- 見出し: 1.2～1.3

## 5. コントロールとコンポーネント

### 5.1 ボタン
- **プライマリボタン**: 主要なアクション用（背景色あり）
- **セカンダリボタン**: 補助的なアクション用（アウトライン）
- **テキストボタン**: 軽微なアクション用（背景色なし）
- **最小サイズ**: 32px 高さ、120px 幅
- **パディング**: 左右 16px、上下 8px

### 5.2 入力フィールド
- **高さ**: 32px または 40px
- **ラベル**: フィールドの上部に配置
- **プレースホルダー**: グレー系の色（#757575）
- **フォーカス時**: アクセントカラーのボーダー（2px）
- **エラー時**: 赤いボーダーとエラーメッセージ

### 5.3 ナビゲーション
- **NavigationView**: 標準的なナビゲーションパターン
  - コンパクトモード: 48px 幅（アイコンのみ）
  - 展開モード: 320px 幅（アイコン＋テキスト）
  - 選択されたアイテムは背景色で強調
  - ホバー時は半透明の背景

### 5.4 カード
- **影**: 軽い影（Elevation 4 または 8）
- **角丸**: 4px または 8px
- **パディング**: 16px～24px
- **余白**: カード間は 16px

### 5.5 ダイアログ
- **最大幅**: 600px
- **背景**: 半透明のオーバーレイ（#000000, 50% opacity）
- **パディング**: 24px
- **ボタン配置**: 右下に配置（OK/キャンセルの順）

## 6. アイコン

### 6.1 アイコンのスタイル
- **Segoe Fluent Icons** または **Segoe MDL2 Assets** を使用
- 一貫したスタイルを保つ（アウトライン or フィル）
- サイズ: 16px, 20px, 24px, 32px

### 6.2 アイコンの使い方
- 明確で認識しやすいアイコンを選択
- テキストラベルと組み合わせる（特に重要なアクション）
- ツールチップを提供してアイコンの意味を補完

## 7. アニメーションとトランジション

### 7.1 推奨アニメーション
```
フェードイン/アウト: 200ms
スライド: 300ms
展開/折りたたみ: 250ms
```

### 7.2 イージング関数
```
標準: Cubic Bezier (0.8, 0, 0.2, 1)
加速: Cubic Bezier (0.4, 0, 1, 1)
減速: Cubic Bezier (0, 0, 0.2, 1)
```

### 7.3 原則
- 過度なアニメーションは避ける
- ユーザーの操作に対する即座のフィードバック
- アニメーションは無効化できるようにする（アクセシビリティ）

## 8. アクセシビリティ

### 8.1 色のコントラスト比
```
テキスト（標準）: 4.5:1 以上
テキスト（大）: 3:1 以上
UIコンポーネント: 3:1 以上
```

### 8.2 キーボードナビゲーション
- すべてのインタラクティブ要素にキーボードアクセス
- Tab キーで論理的な順序で移動
- フォーカス状態を明確に表示
- ショートカットキーの提供（Ctrl+S など）

### 8.3 スクリーンリーダー対応
- 意味のある要素名を設定（AutomationProperties.Name）
- ライブリージョンで動的な変更を通知
- 装飾的な要素は読み上げから除外

### 8.4 スケーリング対応
- システムのテキストサイズ設定に対応
- 125%, 150%, 200% のスケールで確認
- ハイコントラストモードに対応

## 9. パフォーマンス

### 9.1 起動時間
- 初回起動: 3秒以内
- 2回目以降: 1秒以内
- スプラッシュスクリーンで待機時間を短く感じさせる

### 9.2 応答性
- UI操作への反応: 100ms以内
- 重い処理は非同期で実行
- プログレスインジケーターで進捗を表示

### 9.3 メモリ使用量
- 必要最小限のリソース使用
- 不要な要素は適切に解放
- 大きな画像は適切にリサイズ

## 10. WPF での実装例

### 10.1 システムカラーの使用
```xml
<!-- システムアクセントカラーを使用 -->
<Button Background="{DynamicResource SystemAccentColorBrush}"
        Foreground="White"/>
```

### 10.2 標準的なボタンスタイル
```xml
<Button Content="保存"
        Height="32"
        MinWidth="120"
        Padding="16,0"
        Background="#0078D4"
        Foreground="White"
        BorderThickness="0"
        FontSize="14"
        FontFamily="Segoe UI"/>
```

### 10.3 入力フィールド
```xml
<TextBox Height="40"
         Padding="12,8"
         FontSize="14"
         BorderThickness="1"
         BorderBrush="#8E8E8E">
    <TextBox.Style>
        <Style TargetType="TextBox">
            <Style.Triggers>
                <Trigger Property="IsFocused" Value="True">
                    <Setter Property="BorderBrush" Value="#0078D4"/>
                    <Setter Property="BorderThickness" Value="2"/>
                </Trigger>
            </Style.Triggers>
        </Style>
    </TextBox.Style>
</TextBox>
```

### 10.4 カードレイアウト
```xml
<Border Background="White"
        CornerRadius="8"
        Padding="24"
        Margin="0,0,0,16">
    <Border.Effect>
        <DropShadowEffect Color="#000000" 
                         Opacity="0.1" 
                         BlurRadius="16" 
                         ShadowDepth="2"/>
    </Border.Effect>
    <!-- カードの内容 -->
</Border>
```

## 11. 推奨ライブラリ

### 11.1 WPF UI ライブラリ
- **WPF UI**: https://github.com/lepoco/wpfui
  - Windows 11 Fluent Design に準拠
  - 豊富なコントロール
  - ダーク/ライトテーマ自動切替

- **ModernWpf**: https://github.com/Kinnara/ModernWpf
  - Windows 10 スタイルのコントロール
  - UWP ライクなデザイン

- **Material Design In XAML Toolkit**: 現在使用中
  - Material Design に準拠（Google の設計）
  - Windows 標準とは異なるが、モダンで統一感のあるデザイン

### 11.2 移行の検討
現在のアプリは Material Design を使用していますが、より Windows らしい見た目にするには **WPF UI** または **ModernWpf** への移行を検討してください。

## 12. チェックリスト

デザイン実装時に確認すべき項目：

- [ ] 一貫したカラーパレットを使用している
- [ ] 適切なフォントサイズと行間を設定している
- [ ] 十分な余白とスペーシングを確保している
- [ ] ボタンやタッチターゲットのサイズが適切（最小 44px）
- [ ] アクセシビリティ（コントラスト比、キーボードナビゲーション）に対応している
- [ ] ホバー時とフォーカス時の視覚的フィードバックがある
- [ ] アニメーションは滑らかで過度でない（200-500ms）
- [ ] エラーメッセージは明確で理解しやすい
- [ ] ローディング状態を適切に表示している
- [ ] ライト/ダークテーマの両方で見やすい
- [ ] 125%～200% のスケーリングで正しく表示される
- [ ] システムのアクセントカラーに対応している

## 13. 参考リンク

### 公式ドキュメント
- [Microsoft Fluent Design System](https://www.microsoft.com/design/fluent/)
- [Windows App Design Guidelines](https://learn.microsoft.com/ja-jp/windows/apps/design/)
- [WPF Design Guidelines](https://learn.microsoft.com/ja-jp/dotnet/desktop/wpf/)

### ツール
- [Fluent UI](https://developer.microsoft.com/en-us/fluentui)
- [Windows UI Library (WinUI)](https://github.com/microsoft/microsoft-ui-xaml)
- [Segoe UI Variable Font](https://learn.microsoft.com/ja-jp/windows/apps/design/signature-experiences/typography)

## 14. まとめ

Microsoft 標準のアプリのような見た目を実現するためには：

1. **Fluent Design System** の原則に従う
2. **一貫性のあるカラーパレット**を使用（特に Microsoft Blue #0078D4）
3. **適切なスペーシング**（8px の倍数）を保つ
4. **Segoe UI フォント**を使用し、読みやすいサイズとウェイトを選択
5. **アクセシビリティ**を考慮（コントラスト、キーボードナビゲーション）
6. **滑らかなアニメーション**で状態変化を表現
7. **標準的なコントロール**とレイアウトパターンを使用
8. **システムテーマ**（ライト/ダーク）に対応

これらの原則を守ることで、ユーザーが使い慣れた Windows アプリケーションの体験を提供できます。
