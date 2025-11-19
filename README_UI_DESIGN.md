# Windows UI デザインガイド集

## 📚 ドキュメント概要

このリポジトリには、Windows アプリケーションを Microsoft 標準のような見た目にするための包括的なガイドラインが含まれています。

## 🎯 対象者

- Windows アプリケーション（WPF）のフロントエンドエンジニア
- UI/UX デザイナー
- Microsoft 標準のデザインに準拠したアプリを開発したい方

## 📖 ドキュメント一覧

### 1. [クイックリファレンス](UI_DESIGN_QUICK_REFERENCE.md) ⚡
**最初に読むべきドキュメント**

すぐに使える設計値とチェックリスト
- カラーパレット（#0078D4 など）
- スペーシング（8px, 16px, 24px）
- タイポグラフィ（Segoe UI, 14px）
- コンポーネントサイズ（ボタン 32px など）
- よくある質問

📄 **ページ数**: 約 4 ページ  
⏱ **読了時間**: 5-10 分

---

### 2. [Windows UI デザインガイドライン](WINDOWS_UI_DESIGN_GUIDELINES.md) 📘
**完全版ガイドライン**

Fluent Design System の詳細説明
- Light, Depth, Motion, Material, Scale の 5 要素
- レイアウトとスペーシングの詳細
- カラーパレット（ライト/ダークテーマ）
- タイポグラフィの完全仕様
- コントロール別の実装例
- アニメーションとトランジション
- アクセシビリティ対応
- パフォーマンス最適化
- WPF での実装コード例
- 推奨ライブラリ
- チェックリスト

📄 **ページ数**: 約 12 ページ  
⏱ **読了時間**: 30-45 分

---

### 3. [レイアウトパターン集](LAYOUT_PATTERNS.md) 🎨
**視覚的なパターンガイド**

ASCII アートで図解された実用的なレイアウト
- ナビゲーションビュー（左サイドバー）
- コマンドバーレイアウト
- カードベースレイアウト
- マスター/詳細レイアウト
- フォームレイアウト（垂直・水平）
- ダイアログパターン
- スペーシングの詳細例
- レスポンシブブレークポイント
- ボタン配置パターン
- テーブル/リストビュー
- 設定画面パターン

📄 **ページ数**: 約 12 ページ  
⏱ **読了時間**: 20-30 分

---

### 4. [マイグレーションガイド](MIGRATION_GUIDE.md) 🔄
**Material Design からの移行手順**

現在の実装から Windows デザインへの移行
- 現状分析（Material Design の使用状況）
- カラーパレットの変更方法
- ナビゲーションパターンの改善
- トップバーの改善
- ボタンスタイルの改善
- カードの改善
- 入力フィールドの改善
- 段階的な移行プラン（3 フェーズ）
- 推奨ライブラリ（WPF UI, ModernWpf）
- Before/After コード例
- テストとバリデーション

📄 **ページ数**: 約 11 ページ  
⏱ **読了時間**: 25-35 分

---

## 🚀 クイックスタート

### 初めての方
1. 👉 [クイックリファレンス](UI_DESIGN_QUICK_REFERENCE.md) を読んで基本を把握
2. 📘 [Windows UI デザインガイドライン](WINDOWS_UI_DESIGN_GUIDELINES.md) で詳細を学習
3. 🎨 [レイアウトパターン集](LAYOUT_PATTERNS.md) で実装イメージを確認

### 既存プロジェクトを改善する方
1. 🔄 [マイグレーションガイド](MIGRATION_GUIDE.md) で移行計画を立案
2. 📘 [Windows UI デザインガイドライン](WINDOWS_UI_DESIGN_GUIDELINES.md) で詳細を確認
3. 👉 [クイックリファレンス](UI_DESIGN_QUICK_REFERENCE.md) で実装時に参照

## 💡 主要なポイント

### Windows UI の 3 大原則

1. **カラー**: Microsoft Blue (#0078D4) を基調とした統一パレット
2. **スペーシング**: 8px の倍数で一貫した余白
3. **タイポグラフィ**: Segoe UI フォント、適切なサイズとウェイト

### よくある質問

**Q: どのドキュメントから読めばいい？**
- 時間がない → [クイックリファレンス](UI_DESIGN_QUICK_REFERENCE.md)
- じっくり学習 → [Windows UI デザインガイドライン](WINDOWS_UI_DESIGN_GUIDELINES.md)
- 具体例を見たい → [レイアウトパターン集](LAYOUT_PATTERNS.md)
- 既存アプリ改善 → [マイグレーションガイド](MIGRATION_GUIDE.md)

**Q: Material Design と併用できる？**
- はい。段階的に移行できます。詳しくは[マイグレーションガイド](MIGRATION_GUIDE.md)を参照。

**Q: Windows 11 専用？**
- いいえ。Windows 10/11 両方に対応した原則です。

## 📐 基本的な設計値（早見表）

```
カラー:
  プライマリ: #0078D4
  ホバー:    #106EBE
  押下:      #005A9E

スペーシング:
  最小: 4px
  標準: 8px, 12px, 16px, 24px
  
フォント:
  ファミリー: Segoe UI / Yu Gothic UI
  本文:      14px, Regular
  見出し:    20px, SemiBold

コンポーネント:
  ボタン高さ:     32px または 40px
  最小タッチ:     44×44 px
  入力フィールド: 40px
```

## 🔧 推奨ツールとライブラリ

### WPF UI（推奨）
Windows 11 Fluent Design 準拠
```bash
dotnet add package WPF-UI
```

### ModernWpf
Windows 10 スタイル
```bash
dotnet add package ModernWpfUI
```

## 📚 参考リンク

### 公式リソース
- [Microsoft Fluent Design System](https://www.microsoft.com/design/fluent/)
- [Windows App Design Guidelines](https://learn.microsoft.com/ja-jp/windows/apps/design/)
- [WPF Design Guidelines](https://learn.microsoft.com/ja-jp/dotnet/desktop/wpf/)

### コミュニティ
- [WPF UI (GitHub)](https://github.com/lepoco/wpfui)
- [ModernWpf (GitHub)](https://github.com/Kinnara/ModernWpf)
- [Material Design In XAML Toolkit (GitHub)](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)

## ✅ 実装チェックリスト

デザインを実装する前に確認：

**基本要件**
- [ ] Microsoft Blue (#0078D4) をプライマリカラーに使用
- [ ] Segoe UI フォントを使用（英数字）
- [ ] Yu Gothic UI フォントを使用（日本語）
- [ ] 8px の倍数でスペーシング
- [ ] ボタンの最小サイズ 32px

**アクセシビリティ**
- [ ] テキストとのコントラスト比 4.5:1 以上
- [ ] キーボードナビゲーション対応
- [ ] フォーカス状態を視覚的に表示
- [ ] スクリーンリーダー対応

**レスポンシブ**
- [ ] 125%～200% スケーリングで確認
- [ ] ライトテーマで確認
- [ ] ダークテーマで確認
- [ ] ウィンドウサイズ変更に対応

## 🤝 フィードバック

このガイドラインに関する質問や改善提案があれば、Issue または Pull Request でお知らせください。

---

**最終更新**: 2025-11-19  
**バージョン**: 1.0  
**対応技術**: WPF (Windows Presentation Foundation)  
**対応OS**: Windows 10/11

## 📄 ライセンス

これらのガイドラインは、Microsoft の公式デザインガイドラインとコミュニティのベストプラクティスに基づいています。

---

**開発の成功を祈っています！** 🎉
