# Windows UI デザインガイドライン - クイックリファレンス

> **完全版**: 詳細なガイドラインは [WINDOWS_UI_DESIGN_GUIDELINES.md](WINDOWS_UI_DESIGN_GUIDELINES.md) を参照してください

## 📖 概要

このドキュメントは、Microsoft 標準のアプリのような見た目を実現するためのクイックリファレンスです。

## 🎯 主要な設計原則

### 1. カラーパレット
```
プライマリ: #0078D4 (Microsoft Blue)
ホバー:    #106EBE
押下:      #005A9E
成功:      #107C10
警告:      #FFA500
エラー:    #D13438
```

### 2. スペーシング
```
基準単位:         4px
推奨マージン:     8px, 12px, 16px, 20px, 24px
最小タッチ:       44×44 px
要素間の余白:     8px または 12px
セクション間:     16px または 24px
```

### 3. タイポグラフィ
```
フォント:         Segoe UI (英数) / Yu Gothic UI (日本語)
見出し1:         28px, SemiBold
見出し2:         24px, SemiBold
本文:            14px, Regular
キャプション:     12px, Regular
行間:            1.5 (本文)
```

### 4. コンポーネントサイズ
```
ボタン高さ:       32px または 40px
ボタン幅(最小):   120px
入力フィールド:   40px
ナビゲーション:   48px (コンパクト) / 320px (展開)
```

## 🚀 クイックスタート

### Material Design から Windows へ移行する場合

#### Step 1: カラーを変更
```xml
<!-- Before -->
<Button Background="#FFA726" />

<!-- After -->
<Button Background="#0078D4" />
```

#### Step 2: サイズを調整
```xml
<!-- Before -->
<Button Height="110" FontSize="26" />

<!-- After -->
<Button Height="40" FontSize="14" />
```

#### Step 3: フォントを変更
```xml
<Window FontFamily="Segoe UI" FontSize="14">
```

## 📚 ドキュメント一覧

1. **[WINDOWS_UI_DESIGN_GUIDELINES.md](WINDOWS_UI_DESIGN_GUIDELINES.md)**
   - 完全なデザインガイドライン
   - Fluent Design System の詳細
   - コンポーネント別の実装例
   - アクセシビリティとパフォーマンス

2. **[MIGRATION_GUIDE.md](MIGRATION_GUIDE.md)**
   - 現在の実装分析
   - Material Design から Windows への移行方法
   - 段階的な移行プラン
   - Before/After のコード例

## ✅ デザインチェックリスト

実装前の確認項目：

**基本**
- [ ] Microsoft Blue (#0078D4) を使用
- [ ] Segoe UI フォントを使用
- [ ] 8px の倍数でスペーシング
- [ ] ボタンサイズは 32px 以上

**アクセシビリティ**
- [ ] コントラスト比 4.5:1 以上
- [ ] キーボードナビゲーション対応
- [ ] フォーカス状態が明確

**レスポンシブ**
- [ ] 125%～200% スケールで確認
- [ ] ライト/ダークテーマ対応
- [ ] ウィンドウサイズ変更に対応

## 🎨 色の使い分け

| 用途 | ライトモード | ダークモード |
|------|------------|------------|
| 背景 | #FFFFFF | #1E1E1E |
| カード背景 | #F3F3F3 | #2D2D30 |
| テキスト | #000000 | #FFFFFF |
| サブテキスト | #666666 | #CCCCCC |
| ボーダー | #8E8E8E | #3F3F46 |
| アクセント | #0078D4 | #0078D4 |

## 🔧 推奨ライブラリ

### WPF UI (推奨)
```bash
dotnet add package WPF-UI
```
- Windows 11 Fluent Design 準拠
- ダークモード自動対応
- 豊富なコントロール

### ModernWpf
```bash
dotnet add package ModernWpfUI
```
- Windows 10 スタイル
- 軽量でシンプル

## 💡 よくある質問

**Q: Material Design と併用できますか？**  
A: はい。カラーパレットとサイズだけを Windows 標準に近づけることで、段階的に移行できます。

**Q: どのくらいの作業時間が必要ですか？**  
A: カラー調整のみなら数時間。完全移行なら 1-2 週間程度です。

**Q: Windows 10 と Windows 11 で見た目が異なりますか？**  
A: 基本的な原則は共通ですが、Windows 11 では角丸が多用されます。

**Q: ダークモードに対応する必要がありますか？**  
A: 推奨されます。ユーザーの約 40% がダークモードを使用しています。

## 📖 参考リンク

- [Microsoft Fluent Design System](https://www.microsoft.com/design/fluent/)
- [Windows App Design Guidelines](https://learn.microsoft.com/ja-jp/windows/apps/design/)
- [WPF UI Library (GitHub)](https://github.com/lepoco/wpfui)

## 🤝 コントリビューション

このガイドラインの改善案や追加情報があれば、Issue または Pull Request でお知らせください。

---

**作成日**: 2025-11-19  
**対象**: WPF アプリケーション開発者  
**目的**: Windows 標準アプリのような見た目の実現
