# RLCreature for Unity ([日本語](./docs/README_ja.md))

### [Project Page](https://dmv.nico/ja/casestudy/alife/)
### [Paper](https://dmv.nico/assets/img/casestudy/alife/siggraph2018_alife.pdf)

![](./docs/20180801_OSS_showcase.gif)

サンプルデモを動かす
===========================

### 0. Install Unity

- We are using 2018
- Set ScriptingRuntimeVersion to .NET4.X

### 1. Clone Repository

- `git clone --recursive https://github.com/dwango/RLCreature.git`
- git-lfs is required

### 2. Install Dependencies

- MessagePack-CSharp
  - https://github.com/neuecc/MessagePack-CSharp/releases
- UniRX
  - https://assetstore.unity.com/packages/tools/integration/unirx-reactive-extensions-for-unity-17276
- Standard Assets
  - https://assetstore.unity.com/packages/essentials/asset-packs/standard-assets-32351

### 3. Open Demo Scenes

- Assets/RLCreature/Sample/RandomCreatures
- Assets/RLCreature/Sample/DesignedCreatures
- Assets/RLCreature/Sample/VariousHeights
- Assets/RLCreature/Sample/Driving
- Assets/RLCreature/Sample/SimpleHunting

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
