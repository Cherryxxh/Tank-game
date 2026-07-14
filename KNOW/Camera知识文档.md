# Unity Camera 代码相关完全指南

> 摄像机（Camera）是玩家看到游戏世界的"眼睛"。掌握代码控制摄像机是 Unity 开发的基本功。

---

## 一、重要静态成员

### 1.1 `Camera.main` — 获取主摄像机

```csharp
// 语法
public static Camera main { get; }
```

```csharp
Camera cam = Camera.main;
print(cam.name);   // 比如 "Main Camera"
```

| 说明 | 细节 |
|------|------|
| **原理** | 遍历场景中所有 Camera，找到第一个 `tag = "MainCamera"` 的 |
| **前提** | 场景中必须有一个摄像机的 Tag 设为 `MainCamera` |
| **性能** | ⚠️ 内部使用 `FindGameObjectWithTag("MainCamera")`，**不要在 Update 中每帧调用** |

```csharp
// ❌ 错误：每帧调用，浪费性能
void Update()
{
    Camera.main.transform.Rotate(...);  // 每帧都查找一次
}

// ✅ 正确：在 Start 中缓存
private Camera mainCam;
void Start()
{
    mainCam = Camera.main;
}
void Update()
{
    mainCam.transform.Rotate(...);  // 直接用缓存
}
```

### 1.2 `Camera.allCamerasCount` — 摄像机数量

```csharp
// 语法
public static int allCamerasCount { get; }
```

```csharp
int count = Camera.allCamerasCount;
print("场景中有 " + count + " 个摄像机");
```

### 1.3 `Camera.allCameras` — 获取所有摄像机

```csharp
// 语法
public static Camera[] allCameras { get; }
```

```csharp
Camera[] allCams = Camera.allCameras;
foreach (Camera cam in allCams)
{
    print(cam.name + " → depth: " + cam.depth);
}
```

> ⚠️ 返回的是**当前帧的快照**，如果场景中增删了摄像机，需要重新获取。

---

### 1.4 渲染相关委托 — 摄像机生命周期钩子

```csharp
Camera.onPreCull  += (c) => { };   // ① 剔除之前
Camera.onPreRender += (c) => { };  // ② 渲染之前
Camera.onPostRender += (c) => { }; // ③ 渲染之后
```

#### 每帧执行顺序

```
每个摄像机、每一帧：

  ① onPreCull    → "哪些物体需要被渲染？" —— 在这之后进行视锥剔除
  ② onPreRender  → "我要开始画这一帧了！" —— 画面渲染之前的最后时机
      [Unity 执行实际渲染管线]
  ③ onPostRender → "这一帧画完了" —— 可以在这里截图、记录数据等
```

#### 使用示例

```csharp
void OnEnable()
{
    Camera.onPreRender += OnCameraPreRender;
    Camera.onPostRender += OnCameraPostRender;
}

void OnDisable()
{
    // ⚠️ 必须取消订阅！否则切换场景后委托残留会导致内存泄漏
    Camera.onPreRender -= OnCameraPreRender;
    Camera.onPostRender -= OnCameraPostRender;
}

void OnCameraPreRender(Camera cam)
{
    if (cam == Camera.main)
    {
        // 主摄像机渲染前的处理，如调整后处理参数
        Debug.Log("主摄像机开始渲染");
    }
}

void OnCameraPostRender(Camera cam)
{
    // 画面渲染完毕，可以做截图等操作
}
```

> ⚠️ ⚠️ 最关键注意事项：**必须用 `OnEnable`/`OnDisable` 或 `OnDestroy` 取消订阅**，否则对象销毁后委托仍持有引用，造成内存泄漏。

#### 三个委托的区别速查

| 委托 | 时机 | 典型用途 |
|------|------|---------|
| `onPreCull` | 视锥剔除前 | 修改剔除参数、动态调整可见范围 |
| `onPreRender` | 开始渲染前 | 调整摄像机参数（FOV、位置）、动态换 Shader |
| `onPostRender` | 渲染完成后 | 截图、帧统计、渲染后处理标记 |

---

## 二、摄像机深度 `depth`

```csharp
// 语法
public float depth { get; set; }
```

```csharp
Camera.main.depth = 10;
```

### 深度决定渲染顺序

```
depth 值 → 渲染顺序 → 最终画面中的层级：

  depth = -1  先画（最先渲染）
  depth = 0   覆盖在上
  depth = 5   覆盖在上
  depth = 99  最后画（显示在最上层）

  场景摄像机 depth = 0
  UI 摄像机 depth = 10（UI 永远在画面最上层）
```

**这就是为什么 UI 摄像机的 depth 通常设为较大值——** UI 永远显示在 3D 场景画面之上，不被场景物体遮挡。

---

## 三、坐标转换（最常用的功能 ⭐）

### 3.1 三种坐标系的区别

| 坐标系 | 单位 | 原点 | 范围 |
|--------|------|------|------|
| **世界坐标** | 米（Unity 单位） | 场景原点 `(0,0,0)` | 无限 |
| **屏幕坐标** | 像素（pixel） | 屏幕左下角 `(0,0)` | `Screen.width × Screen.height` |
| **视口坐标** | 比例（0~1） | 屏幕左下角 `(0,0)` | `(0,0)` ~ `(1,1)` |

---

### 3.2 `WorldToScreenPoint()` — 世界坐标 → 屏幕坐标

```csharp
// 语法
public Vector3 WorldToScreenPoint(Vector3 position)
```

```csharp
Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.position);
```

#### 返回值详解

| 分量 | 含义 | 范围 |
|------|------|------|
| `screenPos.x` | 水平像素坐标 | `0` ~ `Screen.width` |
| `screenPos.y` | 垂直像素坐标 | `0` ~ `Screen.height` |
| `screenPos.z` | 物体到摄像机的**距离** | 正=在前方，负=在后方 |

#### 示例：头顶血条

```csharp
public Transform enemy;
public RectTransform healthBar;  // UI 中的血条

void Update()
{
    Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.position);

    // 如果 z > 0，说明物体在摄像机前方（可见）
    // 如果 z < 0，说明物体在摄像机后方（不应显示血条）
    if (screenPos.z > 0)
    {
        healthBar.position = screenPos;
        healthBar.gameObject.SetActive(true);
    }
    else
    {
        healthBar.gameObject.SetActive(false);
    }
}
```

> ⚠️ **注意：** `WorldToScreenPoint` 只做坐标转换，**不管物体是否被其他物体遮挡**。物体藏在墙后依然返回屏幕坐标。

---

### 3.3 `ScreenToWorldPoint()` — 屏幕坐标 → 世界坐标

```csharp
// 语法
public Vector3 ScreenToWorldPoint(Vector3 position)
```

#### Z 值是理解的关键 ⚠️

```csharp
Vector3 mouseScreen = Input.mousePosition;
mouseScreen.z = 5f;   // ← 必须设置！代表"离摄像机多远的横截面"
Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreen);
```

```
Z 的含义：

  Z = 0  → 就是摄像机自己所在的位置（没意义）
  Z = 5  → 摄像机正前方 5 米处的横截面上的点
  Z = 10 → 摄像机正前方 10 米处横截面上的点

                    ← Z = 10（远处截面）
                  ← Z = 5（近处截面）
              ↗
    摄像机 ●
```

```csharp
// 完整示例：物体跟随鼠标在固定平面上移动
void Update()
{
    Vector3 mouseScreen = Input.mousePosition;
    mouseScreen.z = 10f;  // 离摄像机 10 米的平面上
    Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreen);
    transform.position = worldPos;
}
```

---

### 3.4 `WorldToViewportPoint()` — 世界坐标 → 视口坐标

```csharp
// 语法
public Vector3 WorldToViewportPoint(Vector3 position)
```

| 分量 | 范围 |
|------|------|
| `x`, `y` | `0.0` ~ `1.0`（左下 → 右上） |
| `z` | 物体到摄像机的距离 |

```
(0, 1) ───────── (1, 1)
  │                 │
  │    屏幕区域      │
  │                 │
(0, 0) ───────── (1, 0)
```

```csharp
// 判断物体是否在屏幕内
Vector3 vp = Camera.main.WorldToViewportPoint(enemy.position);
bool isOnScreen = vp.x > 0 && vp.x < 1
               && vp.y > 0 && vp.y < 1
               && vp.z > 0;  // 在摄像机前方

if (!isOnScreen)
{
    // 敌人在屏幕外，可以关闭它的 AI 逻辑来省性能
}
```

---

### 3.5 `ViewportToWorldPoint()` — 视口坐标 → 世界坐标

```csharp
Vector3 vpPos = new Vector3(0.5f, 0.5f, 10f);  // 屏幕正中心，前方 10 米
Vector3 worldPos = Camera.main.ViewportToWorldPoint(vpPos);
```

---

### 3.6 `ScreenPointToRay()` — 屏幕坐标 → 射线（重要！）

```csharp
// 语法
public Ray ScreenPointToRay(Vector3 position)
```

```csharp
// 从鼠标位置发射射线（FPS 射击、点击拾取的核心）
Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
if (Physics.Raycast(ray, out RaycastHit hit, 100f))
{
    Debug.Log("击中：" + hit.collider.name);
    // 在击中点生成特效
    Instantiate(hitEffect, hit.point, Quaternion.identity);
}
```

> `ScreenPointToRay` 是点击拾取物体的核心方法，比 `ScreenToWorldPoint` 更常用。

---

## 四、完整实用示例

### 4.1 鼠标点击地面，物体移动到点击位置

```csharp
void Update()
{
    if (Input.GetMouseButtonDown(0))
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            transform.position = hit.point;  // 移动到鼠标点击的地面位置
        }
    }
}
```

### 4.2 物体在摄像机前方平面上跟随鼠标

```csharp
void Update()
{
    Vector3 mouseScreen = Input.mousePosition;
    mouseScreen.z = 10f;
    Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreen);
    transform.position = worldPos;  // 物体在 Z=10 平面上跟随鼠标
}
```

### 4.3 屏幕外物体指示器（UI 箭头指向敌人）

```csharp
public Transform enemy;
public RectTransform arrow;  // UI 箭头

void Update()
{
    Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.position);

    if (screenPos.z < 0)
    {
        // 敌人在身后，箭头指向反方向
        screenPos.x = Screen.width - screenPos.x;
        screenPos.y = Screen.height - screenPos.y;
    }

    // 钳制箭头在屏幕边缘
    screenPos.x = Mathf.Clamp(screenPos.x, 50, Screen.width - 50);
    screenPos.y = Mathf.Clamp(screenPos.y, 50, Screen.height - 50);

    arrow.position = screenPos;
}
```

---

## 五、坐标转换方法速查表

| 方法 | 输入 | 输出 | 常用场景 |
|------|------|------|---------|
| `WorldToScreenPoint` | 世界坐标 | 屏幕像素坐标 | 头顶血条、UI 跟随 |
| `ScreenToWorldPoint` | 屏幕坐标 + Z | 世界坐标 | 鼠标控制物体位置 |
| `WorldToViewportPoint` | 世界坐标 | 0~1 归一化坐标 | 判断物体在屏幕内/外 |
| `ViewportToWorldPoint` | 0~1 坐标 + Z | 世界坐标 | 固定视口位置生成物体 |
| `ScreenPointToRay` | 屏幕坐标 | Ray 射线 | 鼠标点击检测、FPS 射击 |
| `ViewportPointToRay` | 0~1 坐标 | Ray 射线 | 屏幕中心射线 |

---

## 六、注意事项汇总

| # | 注意事项 | 说明 |
|---|---------|------|
| 1 | **`Camera.main` 要缓存** | 不要在 Update 中直接调用，内部有遍历查找开销 |
| 2 | **`ScreenToWorldPoint` 必须设 Z** | Z=0 永远返回摄像机自身位置，必须设正数 |
| 3 | **`WorldToScreenPoint` 不管遮挡** | 物体在墙后、在身后都返回坐标，要自己判断 z>0 |
| 4 | **渲染委托必须取消订阅** | 在 `OnDisable`/`OnDestroy` 中 `-=`，否则内存泄漏 |
| 5 | **depth 越大越靠前** | UI 摄像机 depth 通常设最大（如 100） |
| 6 | **`allCameras` 是快照** | 场景摄像机变化后需要重新获取 |
| 7 | **屏幕坐标原点在左下角** | `(0,0)` = 左下角，不是左上角（和 GUI 不同） |
