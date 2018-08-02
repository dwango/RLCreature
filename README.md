# RLCreature

- [Project Page](https://dmv.nico/ja/casestudy/alife/)
- [Paper](https://dmv.nico/assets/img/casestudy/alife/siggraph2018_alife.pdf)

![](./docs/20180801_OSS_showcase.gif)

サンプルデモを動かす
===========================

0. Unityを用意

- 2018が推奨
- ScriptingRuntimeVersionを必ず.NET4.Xにして動かして下さい

1. レポジトリをクローン

- git-lfsが必要です

2. 依存ライブラリをインストール

- MessagePack-CSharp
  - https://github.com/neuecc/MessagePack-CSharp/releases
    - unitypackageをダウンロードしてインストールしてください
- UniRX
  - https://assetstore.unity.com/packages/tools/integration/unirx-reactive-extensions-for-unity-17276
- Standard Assets
  - https://assetstore.unity.com/packages/essentials/asset-packs/standard-assets-32351

3. デモシーンを開く

- ひたすらキューブが進むだけ
  - Assets/Sample/SimpleHunting


Reference
===============

```bibtex
@inproceedings{ogaki2018,
  author = {Keisuke Ogaki and Masayoshi Nakamura},
  title = {Real-Time Motion Generation for Imaginary Creatures Using Hierarchical Reinforcement Learning},
  booktitle = {ACM SIGGRAPH 2018 Studio},
  year = {2018},
  publisher = {ACM}
}
```
