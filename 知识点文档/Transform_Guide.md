# Unity Transform 完全指南

> Transform 是 Unity 中**最重要的组件**。每一个 GameObject 都有一个 Transform，不可移除。

---

## 一、什么是 Transform

### 1.1 本质

`Transform` 是一个**组件（Component）**，记录了物体在游戏世界中的**位置、旋转和缩放**信息，以及**父子层级关系**。

```
每一个 GameObject 都强制携带一个 Transform 组件，无法删除。
```

### 1.2 如何获取

```csharp
// 方式一：MonoBehaviour 中直接访问（最常用）
Transform t = this.transform;           // 因为太常用，Unity 直接给了快捷属性

// 方式二：通过 gameObject 获取
Transform t = this.gameObject.transform;

// 方式三：从另一个 GameObject 获取
Transform t = otherGameObject.transform;

// 方式四：GetComponent（几乎不需要，因为 transform 属性已经是最快路径）
Transform t = this.GetComponent<Transform>();
```

> **注意：** `this.transform` 和 `this.gameObject.transform` 指向**完全同一个对象**，性能无差别。直接用 `transform` 即可。

### 1.3 Transform 的层级结构

```
Transform
├── 位置信息
│   ├── position        （世界坐标）
│   └── localPosition   （相对于父物体的坐标）
├── 旋转信息
│   ├── rotation        （世界旋转，四元数）
│   ├── localRotation   （相对于父物体的旋转，四元数）
│   ├── eulerAngles     （世界旋转，欧拉角）
│   └── localEulerAngles（相对于父物体的旋转，欧拉角）
├── 缩放信息
│   ├── lossyScale      （世界缩放，只读）
│   └── localScale      （相对于父物体的缩放）
├── 层级关系
│   ├── parent          （父 Transform）
│   ├── childCount      （子物体数量）
│   ├── root            （根 Transform）
│   └── GetChild()      （获取第 N 个子物体）
├── 方向向量（只读）
│   ├── forward         （前方，蓝色轴）
│   ├── up              （上方，绿色轴）
│   └── right           （右方，红色轴）
└── 方法
    ├── Translate()     （移动）
    ├── Rotate()        （旋转）
    ├── LookAt()        （看向目标）
    ├── SetParent()     （设置父物体）
    ├── TransformPoint()（坐标转换）
    └── Find()          （按名称查找子物体）
```

---

## 二、位置（Position）

### 2.1 `position` — 世界空间位置

```csharp
// 语法
public Vector3 position { get; set; }
```

物体的**世界坐标**，即物体在场景中的绝对位置。

```csharp
// 读取
Vector3 pos = transform.position;
float x = pos.x;
float y = pos.y;
float z = pos.z;

// 修改（必须整体赋值，不能单独改一个分量）
transform.position = new Vector3(0, 5, 0);      // ✅ 正确

// 错误写法：
// transform.position.x = 5;                      // ❌ 编译错误！x 是只读的
// transform.position = new Vector3(0, 5, 0) + new Vector3(1, 0, 0);  // ✅ 这样可以
```

> **为什么不能直接 `position.x = 5`？** 因为 `position` 是一个**属性（Property）**，不是字段。它返回的是 `Vector3` 的**值拷贝**，修改这个拷贝不会影响原值。所以 C# 编译器禁止给值拷贝的属性赋值。

### 2.2 `localPosition` — 相对于父物体的位置

```csharp
// 语法
public Vector3 localPosition { get; set; }
```

物体的**局部坐标**，即相对于**父物体 pivot** 的偏移量。

```
世界原点 (0,0,0)
    └── 父物体 position = (5, 0, 0)
           └── 子物体 localPosition = (2, 0, 0)
                → 子物体的世界 position = (7, 0, 0)     // 5 + 2
```

```csharp
GameObject child = new GameObject("子物体");
child.transform.parent = this.transform;         // 把 child 设为当前物体的子物体
child.transform.localPosition = new Vector3(0, 2, 0);  // 在父物体上方 2 单位

// 等同关系：
// child.transform.position == this.transform.position + child.transform.localPosition
// （严格来说要考虑旋转和缩放）
```

### 2.3 `position` vs `localPosition` 对照表

| 场景 | `position` | `localPosition` |
|------|-----------|----------------|
| 没有父物体时 | 相同 | 相同 |
| 有父物体时 | 世界绝对位置 | 相对于父物体的偏移 |
| **谁去修改？** | 通常读，少改 | 通常改这个 |

---

## 三、旋转（Rotation）

### 3.1 欧拉角 vs 四元数

Transform 同时支持两种旋转表示：

| | 欧拉角（Euler Angles） | 四元数（Quaternion） |
|---|---|---|
| **类型** | `Vector3` (x, y, z) | `Quaternion` (x, y, z, w) |
| **人可读** | ✅ 直观（0°~360°） | ❌ 不直观 |
| **万向节死锁** | ❌ 有 | ✅ 无 |
| **用于计算** | ❌ | ✅ 插值、乘法 |
| **存储方式** | Unity 内部用四元数，欧拉角只是方便人类阅读的"翻译" |

### 3.2 `eulerAngles` — 世界欧拉角

```csharp
// 语法
public Vector3 eulerAngles { get; set; }
```

```csharp
// 读取
print(transform.eulerAngles);    // 比如 (0, 90, 0)

// 设置
transform.eulerAngles = new Vector3(0, 90, 0);   // 绕 Y 轴转 90 度

// ⚠️ 常见坑：不要单独比较 eulerAngles 的某个分量
// 因为 Unity 内部存在多种欧拉角表示同一旋转的情况
// 比如：(180, 0, 180) 和 (0, 180, 0) 可能表示同一个旋转
```

### 3.3 `localEulerAngles` — 局部欧拉角

```csharp
transform.localEulerAngles = new Vector3(0, 45, 0);
```

相对于父物体的旋转角度，用法和 `eulerAngles` 一样，区别只是坐标系参照不同。

### 3.4 `rotation` — 世界四元数

```csharp
// 语法
public Quaternion rotation { get; set; }
```

```csharp
// 一般通过 Quaternion 的静态方法来创建和操作
transform.rotation = Quaternion.identity;           // 无旋转（相当于 (0,0,0)）
transform.rotation = Quaternion.Euler(0, 90, 0);    // 从欧拉角创建
transform.rotation = Quaternion.LookRotation(Vector3.forward);  // 看向前方
```

### 3.5 选择建议

| 什么时候用 | 用哪个 |
|-----------|--------|
| Inspector 中调旋转 | 欧拉角（自动的） |
| 代码中设置固定角度 | `eulerAngles` |
| 做插值动画 | `Quaternion.Lerp/Slerp` |
| 做旋转运算（叠加、朝向） | `Quaternion` |
| 朝向某个目标 | `Quaternion.LookRotation` |

---

## 四、缩放（Scale）

### 4.1 `localScale` — 局部缩放

```csharp
// 语法
public Vector3 localScale { get; set; }
```

```csharp
transform.localScale = new Vector3(2, 2, 2);  // 放大 2 倍
transform.localScale = Vector3.one;            // (1, 1, 1)，原始大小
transform.localScale = new Vector3(-1, 1, 1);  // 沿 X 轴镜像翻转
```

> **注意：** 缩放不要设为 0，会导致渲染和物理计算异常。

### 4.2 `lossyScale` — 世界缩放（只读）

```csharp
// 语法
public Vector3 lossyScale { get; }   // 只有 get，不能 set
```

当物体有父物体时，`lossyScale` 会考虑到所有上级物体的缩放。

```
父物体 localScale = (2, 2, 2)
    └── 子物体 localScale = (3, 3, 3)
        → 子物体的 lossyScale = (6, 6, 6)    // 2 × 3
```

```csharp
print(transform.lossyScale);  // 只读，不能改

// ⚠️　不能直接设置！
// transform.lossyScale = new Vector3(2, 2, 2);  // ❌ 编译错误，只读属性
```

> **重要：** `lossyScale` 不一定等于 `parent.lossyScale * localScale`（旋转后的缩放不是简单的乘积），而且**不能修改**，只能作为参考值。

---

## 五、父子层级

### 5.1 `parent` — 获取/设置父物体

```csharp
// 语法
public Transform parent { get; set; }
```

```csharp
// 获取父物体
Transform p = transform.parent;
if (p != null)
{
    print("父物体是：" + p.name);
}

// 设置父物体
transform.parent = otherTransform;   // 设为 otherTransform 的子物体
transform.parent = null;             // 断开父子关系（变成根物体）
```

### 5.2 `SetParent()` — 设置父物体（推荐）

```csharp
// 语法
public void SetParent(Transform parent)
public void SetParent(Transform parent, bool worldPositionStays)
```

**这是设置父物体的推荐方式**，因为它可以控制是否保持世界位置。

```csharp
// ====== 等 SetParent 的第二个参数 worldPositionStays ======

// worldPositionStays = false（默认值）
// → 保留原来的 localPosition，世界位置会跟着父物体"跳"
transform.SetParent(otherTransform, false);
// 等价于：transform.parent = otherTransform
// 例如：父物体在 (10,0,0)，你原来在世界 (5,0,0)，localPosition = (5,0,0)
//       设父物体后 localPosition 还是 (5,0,0)
//       世界位置变成 10+5 = (15,0,0) → 跳走了！

// worldPositionStays = true ⭐ 推荐
// → 保留世界位置不动，Unity 自动调整 localPosition 来补偿
transform.SetParent(otherTransform, true);
// 例如：父物体在 (10,0,0)，你原来在世界 (5,0,0)
//       设父物体后世界位置还是 (5,0,0) → 原地不动
//       Unity 自动修正 localPosition = (-5,0,0)，因为 10+(-5)=5 ✓

// 无参数的 SetParent → 默认 worldPositionStays = true
transform.SetParent(otherTransform);      // 保持世界位置
```

```csharp
// 示例：让手枪"跟随"角色的手，但不改变手枪当前世界位置
GameObject pistol = GameObject.Find("Pistol");
Transform handBone = GameObject.Find("RightHand").transform;
pistol.transform.SetParent(handBone, true);  // 世界位置不跳
```

### 5.3 `DetachChildren()` — 断开所有子物体

```csharp
// 语法
public void DetachChildren()
```

```csharp
transform.DetachChildren();
// 等价于给所有子物体执行 child.parent = null
```

### 5.4 `GetChild()` — 获取第 N 个子物体

```csharp
// 语法
public Transform GetChild(int index)
```

```csharp
int count = transform.childCount;

// 遍历所有子物体
for (int i = 0; i < count; i++)
{
    Transform child = transform.GetChild(i);
    print("第 " + i + " 个子物体：" + child.name);
}

// ⚠️ 索引从 0 开始，越界会抛异常
// ⚠️ childCount 指的是直接子物体，不包括孙子
```

### 5.5 `Find()` — 按名称查找子物体（深层）

```csharp
// 语法
public Transform Find(string name)
```

```csharp
Transform found = transform.Find("左手/食指/指尖");
// 可以按路径查找（用 / 分隔），不限于直接子物体

// ⚠️ Find 性能较差，不要每帧调用
// ⚠️ 找不到返回 null
```

### 5.6 `root` — 获取根物体

```csharp
Transform root = transform.root;  // 沿着父物体链一直追溯到最顶层
```

### 5.7 迭代器遍历

```csharp
// foreach 遍历所有直接子物体
foreach (Transform child in transform)
{
    print("子物体：" + child.name);
}
```

### 5.8 子物体操作父物体 — 层级关系是双向的

**父子关系不是单向的**，子物体可以自由访问和操作父物体、祖辈乃至整个层级链上的任何节点。

#### 5.8.1 访问父物体

```csharp
// 获取父物体的 Transform
Transform parent = transform.parent;

// 获取父物体的 GameObject
GameObject parentObj = transform.parent.gameObject;

// 判断有没有父物体
if (transform.parent != null)
{
    print("我有父物体：" + transform.parent.name);
}
else
{
    print("我是根物体，没有父物体");
}
```

#### 5.8.2 操作父物体

```csharp
// 禁用父物体
transform.parent.gameObject.SetActive(false);

// 修改父物体的位置
transform.parent.position = new Vector3(0, 10, 0);

// 获取父物体身上的组件
HealthBar bar = transform.parent.GetComponent<HealthBar>();
if (bar != null)
{
    bar.UpdateHealth(50);
}

// 更简洁：直接向上查找组件
HealthBar bar = GetComponentInParent<HealthBar>();
// GetComponentInParent 会沿着 自身 → 父 → 祖父 → ... 一直查找
```

#### 5.8.3 访问祖辈（一路向上追）

```csharp
// 获取祖父
Transform grandparent = transform.parent.parent;

// 获取最顶层根物体
Transform root = transform.root;
print("根物体是：" + root.name);

// 判断自己是不是根物体
bool isRoot = transform.parent == null;

// 向上遍历打印整条祖先链
Transform current = transform;
while (current != null)
{
    print(current.name);
    current = current.parent;
}
```

#### 5.8.4 子找父 vs 父找子 对照

```csharp
// 父操作子
transform.GetChild(0);                        // 拿第 0 个子
transform.Find("名字");                       // 按名找子物体
Transform child = transform.GetChild(2);
child.gameObject.SetActive(false);            // 禁用第 2 个子

// 子操作父（一样可以！）
transform.parent;                             // 拿父物体
transform.root;                               // 拿根物体
GetComponentInParent<Rigidbody>();            // 向上找组件
transform.parent.gameObject.SetActive(false); // 禁用父物体

// 子操作兄弟（通过父中转）
transform.parent.GetChild(1);                 // 拿兄弟（第 1 个子）
for (int i = 0; i < transform.parent.childCount; i++)
{
    if (transform.parent.GetChild(i) != transform)
        print("兄弟：" + transform.parent.GetChild(i).name);
}
```

#### 5.8.5 常见模式：子物体通知父物体

```csharp
// 场景：碰撞检测中，子物体（触发器）通知父物体处理
void OnTriggerEnter(Collider other)
{
    // 方式一：一路向上找有特定组件的父物体
    EnemyAI enemy = GetComponentInParent<EnemyAI>();
    if (enemy != null)
    {
        enemy.TakeDamage(10);
    }

    // 方式二：直接拿直接父物体
    transform.parent.GetComponent<PlayerController>().OnHit();

    // 方式三：拿根物体（整棵树的管理者）
    transform.root.GetComponent<UnitManager>().ReportDamage(10);
}
```

#### 5.8.6 安全断开父子关系

```csharp
// 断开自己与父物体的关系（自己变成根物体，世界位置不变）
transform.SetParent(null);
// 或
transform.parent = null;

// 断开自己所有子物体
transform.DetachChildren();

// ⚠️ 断父子关系前，如果需要保持世界位置，用 SetParent(null, true)
transform.SetParent(null, true);
// 等价于 transform.parent = null;
```

### 5.9 父子层级总结

```
层级关系的方向性：

  父 → 子          ✅ 可以（GetChild, Find, foreach）
  子 → 父          ✅ 可以（parent, GetComponentInParent, root）
  子 → 兄弟        ✅ 可以（parent.GetChild(n)）
  任何节点 → 根    ✅ 可以（root）
  
结论：层级关系完全是双向的，没有任何访问方向的限制。
```

---

## 六、方向向量（只读属性）

Transform 提供了物体的三个**局部坐标系方向**在世界空间中的向量表示。

```csharp
// 语法（都是只读的）
public Vector3 forward { get; }   // 前方（世界空间）
public Vector3 up      { get; }   // 上方（世界空间）
public Vector3 right   { get; }   // 右方（世界空间）
```

### 6.1 基本用法

```csharp
// 向前移动
transform.position += transform.forward * Time.deltaTime * speed;

// 向上移动（跳跃方向）
transform.position += transform.up * Time.deltaTime * jumpSpeed;

// 向右移动（侧移）
transform.position += transform.right * Time.deltaTime * sideSpeed;
```

### 6.2 场景对比

| 代码 | 物体正前方是 +Z 时 | 物体正前方是 +X 时 |
|------|:---:|:---:|
| `transform.forward` | `(0, 0, 1)` | `(1, 0, 0)` |
| `Vector3.forward` | `(0, 0, 1)` | `(0, 0, 1)` |

```
transform.forward  = 物体的"前方"，会跟着旋转而变
Vector3.forward    = 世界的"前方"，永远指向 (0, 0, 1)
```

> **关键区别：** `transform.forward` 会随着物体旋转而变化。想"让物体朝自己前方走"就用它，想"让物体朝世界 Z 轴走"就用 `Vector3.forward`。

---

## 七、移动方法 — `Translate()`

```csharp
// 语法
public void Translate(Vector3 translation)
public void Translate(Vector3 translation, Space relativeTo)
public void Translate(Vector3 translation, Transform relativeTo)
public void Translate(float x, float y, float z)
public void Translate(Vector3 translation, Space relativeTo = Space.Self)
```

### 7.1 基本用法

```csharp
// ====== 理解 Translate 的关键：第二个参数 Space ======
//
// Translate(Vector3, Space) 的第二个参数决定了坐标系：
// - Space.Self（默认）：把传入的向量"当作物体的局部方向"来理解
// - Space.World：      把传入的向量"当作世界的方向"来理解

// 按自身坐标系移动（默认 Space.Self）
transform.Translate(Vector3.forward * Time.deltaTime * speed);
// ↑ Vector3.forward 虽然是 (0,0,1)，但因为用的是 Space.Self，
//   这个 (0,0,1) 被解释为"物体自己的 Z 轴"——即物体自己的前方
//   transform.forward = 物体前方在世界中的方向
//   Vector3.forward = 世界 Z 轴永远不变的 (0,0,1)

// 按世界坐标系移动
transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);
// ↑ Space.World：Vector3.forward 就是世界 Z 轴方向，不受物体旋转影响

// ⚠️ 容易混淆的点：
// Vector3.forward 永远是 (0, 0, 1)，它本身不"知道"物体朝哪
// 是 Space.Self 参数让它被当作"物体的局部方向"来移动
// 而不是 Vector3.forward 自己变了

// 按另一个物体的坐标系移动
transform.Translate(Vector3.forward, otherTransform);
// 沿着 otherTransform 的"前方"走
```

### 7.2 `Translate` vs 直接改 position

```csharp
// Translate：相对移动（加法）
transform.Translate(new Vector3(1, 0, 0));   // 从当前位置向右移 1 单位

// position：绝对设置
transform.position = new Vector3(1, 0, 0);   // 直接跳到 (1, 0, 0)
```

---

## 八、旋转方法 — `Rotate()`

```csharp
// 语法
public void Rotate(Vector3 eulers)
public void Rotate(Vector3 eulers, Space relativeTo)
public void Rotate(float xAngle, float yAngle, float zAngle)
```

```csharp
// 自身坐标系旋转（默认）
transform.Rotate(new Vector3(0, 90, 0));            // 绕自己的 Y 轴转 90 度

// 世界坐标系旋转
transform.Rotate(new Vector3(0, 90, 0), Space.World);  // 绕世界 Y 轴转 90 度

// 持续旋转（每帧调用）
transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
```

---

## 九、朝向目标 — `LookAt()`

```csharp
// 语法
public void LookAt(Transform target)
public void LookAt(Vector3 worldPosition)
public void LookAt(Transform target, Vector3 worldUp)
public void LookAt(Vector3 worldPosition, Vector3 worldUp)
```

```csharp
// 看向另一个物体
transform.LookAt(enemy.transform);

// 看向一个坐标点
transform.LookAt(new Vector3(0, 0, 10));

// 看向目标，同时指定哪个方向是"上"
transform.LookAt(target, Vector3.up);  // 默认就是 Vector3.up

// ⚠️ LookAt 会让物体的 forward (+Z) 轴指向目标
// 如果你的模型的"前方"是 +X 或其他方向就会有问题
```

---

## 十、坐标转换方法

### 10.1 核心概念：两套坐标系

在你理解转换方法之前，先区分两个坐标系：

```
世界坐标系：场景中固定不变的坐标系
  - 原点在 (0, 0, 0)
  - X 右、Y 上、Z 前
  - 不管哪个物体，世界的方向始终不变

物体局部坐标系：每个物体自己的"私人坐标系"
  - 原点在物体自己的 pivot 点
  - +Z = 物体自己的前方（forward）
  - +Y = 物体自己的上方（up）
  - +X = 物体自己的右方（right）
  - 旋转物体时，它的局部坐标轴也跟着转！
```

**坐标转换方法的作用就是在世界坐标和局部坐标之间来回翻译。**

---

### 10.2 `TransformPoint()` — 局部坐标 → 世界坐标

```csharp
// 语法
public Vector3 TransformPoint(Vector3 localPoint)
public Vector3 TransformPoint(float x, float y, float z)
```

**"在物体自己看来是 (x,y,z) 的那个点，在世界中是什么位置？"**

```csharp
// "我头顶 2 米的位置"在世界坐标中是哪？
Vector3 aboveHead = transform.TransformPoint(new Vector3(0, 2, 0));

// "我正前方 5 米的位置"在世界坐标中是哪？
Vector3 inFront = transform.TransformPoint(new Vector3(0, 0, 5));
```

#### 深入理解：考虑了位置、旋转和缩放

```
假设物体：
  世界位置 = (10, 0, 0)
  绕 Y 轴转了 90°（物体前方 = 世界 +X 方向）
  缩放 = (1, 1, 1)

transform.TransformPoint(0, 0, 5)：
  (0, 0, 5) 是物体局部坐标 → "物体前方 5 米"
  物体前方 = 世界 +X
  结果 = 物体位置 + 5 × 物体前方方向
       = (10, 0, 0) + 5 × (1, 0, 0)
       = (15, 0, 0)
```

#### 实际应用：生成物体前方固定距离的东西

```csharp
// 在物体前方 3 米处生成一个特效（不随物体旋转会歪掉）
Vector3 spawnPos = transform.TransformPoint(0, 0, 3);
Instantiate(effect, spawnPos, Quaternion.identity);
```

---

### 10.3 `InverseTransformPoint()` — 世界坐标 → 局部坐标

```csharp
// 语法
public Vector3 InverseTransformPoint(Vector3 worldPoint)
public Vector3 InverseTransformPoint(float x, float y, float z)
```

**"敌人在世界的那个位置，在'我看来'是在前方还是后方？"**

这在 AI、敌人检测、UI 定位中非常常用。

#### 最实用的场景：判断目标的相对方位

```csharp
Vector3 relative = transform.InverseTransformPoint(enemy.position);

if (relative.z > 0)     print("敌人在我前方");
else                     print("敌人在我后方");

if (relative.x > 0)     print("敌人在我右侧");
else                     print("敌人在我左侧");

if (relative.y > 0)     print("敌人在我上方");
else                     print("敌人在我下方");

// 还可以精确判断距离和方向
float distance = relative.magnitude;    // 敌人离我多远
float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;  // 方位角
```

#### 没用这个方法之前的问题

```csharp
// ❌ 简单差值不考虑旋转
float diffZ = enemy.position.z - transform.position.z;
// diffZ > 0 → 敌人在世界 +Z 方向，不一定是你"面朝的前方"

// ✅ InverseTransformPoint 考虑了你自身的旋转
Vector3 local = transform.InverseTransformPoint(enemy.position);
// local.z > 0 → 敌人真的在你的正前方
```

#### 另一个场景：计算自己在别人视野中的位置

```csharp
// 我在敌人眼中的方位（敌人是否能看到我）
Vector3 theirView = enemy.transform.InverseTransformPoint(transform.position);
if (theirView.z > 0 && Mathf.Abs(theirView.x) < 5f)
    print("我在敌人前方 5 米宽的视野范围内");
```

---

### 10.4 `TransformDirection()` — 局部方向 → 世界方向

```csharp
// 语法
public Vector3 TransformDirection(Vector3 localDirection)
public Vector3 TransformDirection(float x, float y, float z)
```

**"物体自己认为的某个方向，在世界中是什么方向？"**

> ⚠️ 它**只考虑旋转**，不考虑位置和缩放。转换的是"方向"不是"位置"。

#### 与 `TransformPoint` 的关键区别

| | TransformPoint | TransformDirection |
|---|---|---|
| **转换什么** | 一个点 / 位置 | 一个方向 / 向量 |
| **考虑位置** | ✅ 会加上物体世界位置 | ❌ 不考虑 |
| **考虑缩放** | ✅ 会乘以缩放 | ❌ 一般不考虑 |
| **输入示例** | "我头顶 2 米处" | "我头顶的方向" |
| **输出** | 世界中的一个坐标点 | 世界中的一个方向向量 |

```csharp
// TransformPoint：转换一个位置
// "在我自己坐标系中 (0, 2, 0) 这个点" → 世界坐标中的点
Vector3 abovePoint = transform.TransformPoint(0, 2, 0);
// 物体在 (0,0,0)：结果是 (0, 2, 0)
// 物体在 (10,0,0)：结果是 (10, 2, 0) ← 受物体位置影响

// TransformDirection：转换一个方向
// "我自己上方"方向 → 世界中的方向向量
Vector3 upDir = transform.TransformDirection(Vector3.up);
// 物体在哪都返回 (0, 1, 0)（假设没旋转）
// 效果等价于 transform.up
// ↑ 不受物体位置影响，只受旋转影响
```

#### 实际应用

```csharp
// 让一个力沿着物体自己的前方施加
Vector3 worldForward = transform.TransformDirection(Vector3.forward);
rb.AddForce(worldForward * thrustPower);

// 等价于：
rb.AddForce(transform.forward * thrustPower);
```

---

### 10.5 `InverseTransformDirection()` — 世界方向 → 局部方向

```csharp
// 语法
public Vector3 InverseTransformDirection(Vector3 worldDirection)
```

和 `InverseTransformPoint` 类似，但只考虑旋转，不考虑位置。把世界的方向转换为物体坐标系中的方向。

---

### 10.6 综合示例：物体前方地面的敌人

```csharp
// 问题：物体面前 10 米远、下方 1 米处的那个点，在世界中的坐标？
Vector3 localSpot = new Vector3(0, -1, 10);
Vector3 worldSpot = transform.TransformPoint(localSpot);

// 在这个位置生成一个标记
Instantiate(marker, worldSpot, Quaternion.identity);

// 反过来：这个点移到了哪里，在物体自己看来是什么方位？
Vector3 relative = transform.InverseTransformPoint(worldSpot);
// relative.z 应该接近 10
// relative.y 应该接近 -1
```

---

### 10.7 速查表

| 我想知道... | 用什么 |
|-----------|--------|
| 敌人在我前方还是后方？ | `InverseTransformPoint(enemy.position)` |
| 我头顶 3 米处的世界坐标？ | `TransformPoint(0, 3, 0)` |
| 子弹沿着我枪口方向飞向哪？ | `TransformDirection(Vector3.forward)` |
| 世界中的重力方向在我看来是朝哪？ | `InverseTransformDirection(Vector3.down)` |
| 敌人离我多远、在什么方位角？ | `InverseTransformPoint` 后取 magnitude 和 Atan2 |

---

## 十一、常用 Vector3 静态常量

```csharp
Vector3.zero     // (0, 0, 0)    — 原点
Vector3.one      // (1, 1, 1)    — 全 1
Vector3.forward  // (0, 0, 1)    — +Z
Vector3.back     // (0, 0, -1)   — -Z
Vector3.up       // (0, 1, 0)    — +Y
Vector3.down     // (0, -1, 0)   — -Y
Vector3.right    // (1, 0, 0)    — +X
Vector3.left     // (-1, 0, 0)   — -X
```

---

## 十二、Transform 初始化的两种方式

### 12.1 创建空物体时自动获得

```csharp
GameObject obj = new GameObject("新物体");
// Transform 已经自动创建，无需手动添加
print(obj.transform.position);  // (0, 0, 0)
```

### 12.2 通过 Instantiate 克隆时自动获得

```csharp
GameObject clone = Instantiate(originalPrefab, position, rotation);
// clone.transform 就是原 prefab transform 的副本
```

---

## 十三、常见注意事项汇总

### ❌ 错误 1：逐分量修改 position

```csharp
// ❌ 错误
transform.position.x = 5;

// ✅ 正确
Vector3 pos = transform.position;
pos.x = 5;
transform.position = pos;
```

### ❌ 错误 2：每帧使用 GetChild 遍历大量子物体

```csharp
// ❌ 不好：每帧都访问
void Update()
{
    for (int i = 0; i < transform.childCount; i++)
    {
        // ...
    }
}

// ✅ 更好：缓存到数组中
Transform[] children;
void Start()
{
    children = new Transform[transform.childCount];
    for (int i = 0; i < children.Length; i++)
        children[i] = transform.GetChild(i);
}
```

### ❌ 错误 3：混淆 `transform.forward` 和 `Vector3.forward`

```csharp
transform.Translate(Vector3.forward, Space.Self);   // 世界 Z 轴方向移动 → 可能不是前方
transform.Translate(transform.forward, Space.World); // 物体的前方在世界中 → 这才是"向前走"
```

### ❌ 错误 4：欧拉角直接用于旋转插值

```csharp
// ❌ 欧拉角插值不平滑，可能有万向节问题
Vector3 euler = Vector3.Lerp(from, to, t);

// ✅ 使用四元数插值
Quaternion rot = Quaternion.Slerp(fromRotation, toRotation, t);
transform.rotation = rot;
```

### ❌ 错误 5：频繁使用 `Transform.Find()`

```csharp
// ❌ 每次都要搜索子物体树
void Update()
{
    Transform t = transform.Find("某个深层子物体");
}

// ✅ 只在 Start/Awake 中找一次，缓存起来
Transform cachedChild;
void Start()
{
    cachedChild = transform.Find("某个深层子物体");
}
```

### ❌ 错误 6：`lossyScale` 用于计算

```csharp
// ❌ lossyScale 是近似值，不精确
float w = originalWidth * transform.lossyScale.x;

// ✅ 使用渲染器 bounds 或自己维护缩放值
float w = originalWidth * transform.localScale.x;
```

### ❌ 错误 7：设父物体时不传 `worldPositionStays`

```csharp
// ❌ 物体可能会"跳"到意外位置
transform.SetParent(newParent);

// ✅ 明确你想不想要保持位置
transform.SetParent(newParent, true);   // 保持世界位置
transform.SetParent(newParent, false);  // 不保持
```

---

## 十四、性能参考

| 操作 | 开销 | 建议 |
|------|:---:|------|
| `transform.position` | 低 | 自由使用 |
| `transform.localPosition` | 低 | 自由使用 |
| `transform.forward/up/right` | 低 | 缓存到变量，不用每次重新读取 |
| `GetChild(index)` | 中 | 避免每帧大量调用 |
| `transform.Find("name")` | **高** | 只在 Start/Awake 中使用 |
| `transform.childCount` | 低 | 但每次修改子物体会标记 dirty |
| `transform.SetParent()` | **高** | 避免频繁调用 |

---

## 十五、速查表

| 想要做的事 | 代码 |
|-----------|------|
| 读取世界位置 | `transform.position` |
| 设置世界位置 | `transform.position = new Vector3(x, y, z);` |
| 设置局部位置 | `transform.localPosition = new Vector3(x, y, z);` |
| 读取旋转（欧拉角） | `transform.eulerAngles` |
| 设置旋转 | `transform.eulerAngles = new Vector3(x, y, z);` |
| 旋转插值 | `Quaternion.Slerp(from, to, t)` |
| 归零旋转 | `transform.rotation = Quaternion.identity;` |
| 设置缩放 | `transform.localScale = new Vector3(x, y, z);` |
| 前方移动 | `transform.Translate(Vector3.forward * speed * Time.deltaTime);` |
| 持续旋转 | `transform.Rotate(Vector3.up * speed * Time.deltaTime);` |
| 看向目标 | `transform.LookAt(target.position);` |
| 设为子物体 | `transform.SetParent(parent, true);` |
| 断开父子 | `transform.SetParent(null);` |
| 获取子物体数量 | `transform.childCount` |
| 获取第 i 个子 | `transform.GetChild(i)` |
| 查找深层子物体 | `transform.Find("路径/名称")` |
| 敌人相对于自己的位置 | `transform.InverseTransformPoint(enemy.position)` |
