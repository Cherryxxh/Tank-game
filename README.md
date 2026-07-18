# Tank Game

基于 Unity 开发的 3D 坦克对战游戏，使用自定义 OnGUI UI 系统。

## 项目结构

```
Tank game/
├── Assets/
│   ├── ArtRes/TDSTK/           # 第三方美术资源包
│   │   ├── Audio/              # 音效和背景音乐
│   │   ├── Icons/              # 图标素材
│   │   ├── Model/              # 3D 模型（坦克、炮塔、无人机等）
│   │   ├── Prefabs/            # 预制体（武器、特效、AI 单位等）
│   │   ├── Standard Assets/    # 标准资源（粒子系统等）
│   │   ├── Textures&Materials/ # 贴图和材质
│   │   └── UIAssets/           # UI 素材
│   │
│   ├── control/                # 游戏主要代码
│   │   ├── Prefabs/            # 自制预制体
│   │   │   ├── Weapon/
│   │   │   ├── bullet prefabs/
│   │   │   ├── control prefabs/
│   │   │   ├── effect/
│   │   │   ├── enemy/
│   │   │   ├── rewar pre/
│   │   │   └── wall/
│   │   │
│   │   └── SJXXM/
│   │       ├── GAME/
│   │       │   ├── Beginscene/       # 开始场景
│   │       │   │   ├── BasePanel.cs
│   │       │   │   ├── BeginPanel.cs
│   │       │   │   ├── settingpanel.cs
│   │       │   │   ├── Rankpanel.cs
│   │       │   │   ├── Bkmusic.cs
│   │       │   │   └── TOWER_ROTATE.cs
│   │       │   │
│   │       │   ├── Gamescene/        # 游戏场景
│   │       │   │   ├── UI/
│   │       │   │   │   ├── Gamepanel.cs       # 游戏 HUD
│   │       │   │   │   ├── losepanel.cs       # 失败面板
│   │       │   │   │   ├── winpanel.cs        # 胜利面板
│   │       │   │   │   └── Quitgamepanel.cs   # 退出确认面板
│   │       │   │   ├── Object/
│   │       │   │   │   ├── Tankbase.cs        # 坦克基类
│   │       │   │   │   ├── Playerobj.cs       # 玩家坦克
│   │       │   │   │   ├── Weaponobj.cs       # 武器系统
│   │       │   │   │   ├── bulletobj.cs       # 子弹
│   │       │   │   │   ├── cameramove.cs      # 摄像机跟随
│   │       │   │   │   ├── cube.cs            # 可破坏方块
│   │       │   │   │   └── Destoryself.cs     # 定时自毁
│   │       │   │   ├── Reward/
│   │       │   │   │   ├── endpoint.cs        # 关卡终点
│   │       │   │   │   ├── rewardprop.cs      # 奖励道具
│   │       │   │   │   └── Weaponreward.cs    # 武器拾取
│   │       │   │   └── enemy/
│   │       │   │       ├── Monsterobj.cs      # 敌方怪物
│   │       │   │       └── Monstertower.cs    # 敌方炮塔
│   │       │   │
│   │       │   └── Data/             # 数据层
│   │       │       ├── GameDatamgr.cs
│   │       │       ├── MusicData.cs
│   │       │       ├── Rankinfo.cs
│   │       │       └── RankList.cs
│   │       │
│   │       ├── customgui/            # 自定义 UI 框架
│   │       │   ├── Base/
│   │       │   │   ├── CustomGuiRoot.cs
│   │       │   │   ├── CustomGuicontrol.cs
│   │       │   │   └── CustomGuiPos.cs
│   │       │   └── control/
│   │       │       ├── CustomGuiLab.cs
│   │       │       ├── CustomGuiButton.cs
│   │       │       ├── CustomGuiToggle.cs
│   │       │       ├── CustomGuiToggleGroup.cs
│   │       │       ├── CustomGuiSlider.cs
│   │       │       ├── CustomGuiinput.cs
│   │       │       └── CustomGuiTexture.cs
│   │       │
│   │       └── PlayerprefsdataMgr.cs
│   │
│   ├── script/                  # 学习/教学脚本
│   │   └── 必备知识点/Lesson1/
│   │       └── lesson1.cs
│   │
│   ├── Editor/                  # Editor 工具脚本
│   │   ├── RemoveMissingScripts.cs
│   │   ├── ReorganizeRankPanel.cs
│   │   └── RenameRankPanelItems.cs
│   │
│   └── Scenes/
│       ├── Beginscene.scene     # 主菜单场景
│       └── Gamescene.scene      # 游戏关卡场景
```

---

## 架构设计

### UI 系统

项目使用**自定义 OnGUI 框架**而非 Unity UGUI，所有 UI 控件继承自 `CustomGuicontrol`，由 `CustomGuiRoot` 每帧驱动绘制。

```
CustomGuiRoot (OnGUI 入口)
  └── CustomGuicontrol (抽象基类)
        ├── CustomGuiLab       (标签)
        ├── CustomGuiButton    (按钮)
        ├── CustomGuiToggle    (开关)
        ├── CustomGuiSlider    (滑块)
        ├── CustomGuiinput     (输入框)
        └── CustomGuiTexture   (纹理)
```

### 面板系统

所有 UI 面板继承 `BasePanel<T>`，泛型单例模式——子类声明即获得全局访问能力：

```csharp
public class BeginPanel : BasePanel<BeginPanel> { }
// 任意地方调用: BeginPanel.Instance.ShowMe();
```

| 面板 | 所在场景 | 功能 |
|---|---|---|
| `BeginPanel` | Beginscene | 主菜单（开始游戏、设置、排行榜、退出） |
| `settingpanel` | 两个场景 | 音量/音效设置 |
| `Rankpanel` | Beginscene | 排行榜 |
| `Gamepanel` | Gamescene | 游戏内 HUD（血量、分数、用时） |
| `losepanel` | Gamescene | 失败界面 |
| `winpanel` | Gamescene | 胜利界面 |
| `Quitgamepanel` | Gamescene | 退出确认弹窗 |

### 数据持久化

`PlayerprefsdataMgr` 使用**反射**自动序列化/反序列化任意对象的 `public` 字段到 `PlayerPrefs`，支持基础类型、List、Dictionary 和嵌套对象。

```
设置面板 UI → GameDatamgr (内存) → PlayerprefsdataMgr (反射) → PlayerPrefs (磁盘)
```

### 场景流程

```
Beginscene (主菜单)
  ├── "开始游戏" → Gamescene (坦克对战)
  ├── "设置"     → settingpanel (音量/音效)
  ├── "排行榜"   → Rankpanel
  └── "退出"     → 关闭应用
```

---

## 运行方式

1. 使用 **Unity Hub** 打开项目（Unity 版本：5.0.0f4+）
2. 打开 `Assets/Scenes/Beginscene.scene`
3. 点击 Play 按钮运行

---

## 核心系统

### 坦克战斗

- `Tankbase` — 坦克抽象基类，定义攻击/受伤/死亡逻辑
- `Playerobj` — 玩家坦克，键盘移动 + 鼠标旋转炮塔 + 左键射击
- `Weaponobj` — 武器系统，支持多射击点（`shootpos[]`）
- `bulletobj` — 子弹，碰撞检测 + 特效生成

### 音频

- `Bkmusic` — 背景音乐管理器，自动从 `GameDatamgr` 读取设置
- 通过设置面板调节音乐/音效的开关和音量，数据自动持久化

### 排行榜

- `RankList` 存储最多 9 条 `Rankinfo` 记录（姓名、分数、用时）
- 按用时升序排序，超出 9 条自动截断
- 数据通过 `PlayerprefsdataMgr` 持久化

---

## 核心设计模式

| 模式 | 应用 |
|---|---|
| **泛型单例** | `BasePanel<T>` — 面板基类 |
| **抽象工厂** | `Tankbase` — 坦克基类，子类实现具体行为 |
| **观察者模式** | `UnityAction` 事件 — 按钮点击、滑块变化、开关切换 |
| **反射序列化** | `PlayerprefsdataMgr` — 任意对象 ↔ PlayerPrefs |
| **模板方法** | `CustomGuicontrol.DrawGUI()` — 控制绘制流程，子类实现具体绘制 |
