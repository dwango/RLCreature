# RLCreature for Unity

スマートフォンで人工生命数十体がリアルタイムに学習可能な深層強化学習

- [Project Page](https://dmv.nico/ja/casestudy/alife/)
- [Paper](https://dmv.nico/assets/img/casestudy/alife/siggraph2018_alife.pdf)

![](./20180801_OSS_showcase.gif)

サンプルデモを動かす
===========================

### 0. Unityを用意

- 2018が推奨
- ScriptingRuntimeVersionを必ず.NET4.Xにして動かして下さい

### 1. レポジトリをクローン

- git-lfsが必要です

### 2. 依存ライブラリをインストール

- MessagePack-CSharp
  - https://github.com/neuecc/MessagePack-CSharp/releases
    - unitypackageをダウンロードしてインストールしてください
- UniRX
  - https://assetstore.unity.com/packages/tools/integration/unirx-reactive-extensions-for-unity-17276
- Standard Assets
  - https://assetstore.unity.com/packages/essentials/asset-packs/standard-assets-32351

### 3. デモシーンを開く

- ひたすらキューブが進むだけ
  - Assets/Sample/SimpleHunting


Create Original Creatures
===========================

see  `Assets/RLCreature/Sample/DesignedCreatures/DesignedCreaturesEntryPoint.cs`

```
// Instantiate prefab (see Assets/RLCreature/Sample/DesignedCreatures/CreaturePrefabs)
var centralBody = Instantiate(creaturePrefab);

// Add Sensor and Mouth for food
Sensor.CreateComponent(centralBody, typeof(Food), State.BasicKeys.RelativeFoodPosition, range: 100f);
Mouth.CreateComponent(centralBody, typeof(Food));

// Initialize Brain
var actions = LocomotionAction.EightDirections();
var sequenceMaker = new EvolutionarySequenceMaker(epsilon: 0.1f, minimumCandidates: 30);
var decisionMaker = new ReinforcementDecisionMaker();
var souls = new List<ISoul>() {new GluttonySoul()};

var brain = new Brain(decisionMaker, sequenceMaker);
var agent = Agent.CreateComponent(creatureRootGameObject, brain, new Body(centralBody), actions, souls);
```


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
