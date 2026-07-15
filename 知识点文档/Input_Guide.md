# Unity Input 输入系统完全指南

> Input 是 **Unity 读取玩家键盘和鼠标输入**的核心类。所有键盘按键、鼠标点击、鼠标位置都通过它获取。

---

## 一、Input 类概述

`Input` 是 UnityEngine 提供的**全是静态方法**的类，不需要挂脚本也能在任何地方调用。

```csharp
using UnityEngine;

public class InputTest : MonoBehaviour
{
    void Update()
    {
        // ⚠️ 所有 Input 检测都必须在 Update() 中进行，不要在 Start() 或 Awake() 中
    }
}
```

---

## 二、鼠标输入

### 2.1 鼠标按键

```csharp
// 语法
public static bool GetMouseButton(int button)
public static bool GetMouseButtonDown(int button)
public static bool GetMouseButtonUp(int button)
```

| 参数 `button` | 对应按键 |
|:---:|---|
| `0` | 鼠标左键 |
| `1` | 鼠标右键 |
| `2` | 鼠标中键（滚轮按下） |

```csharp
void Update()
{
    // ====== 三种检测时机 ======

    // 持续按下（按住不放时每帧都是 true）
    if (Input.GetMouseButton(0))
        print("左键按住中...");

    // 按下瞬间（按下的那一帧才为 true）⭐ 最常用
    if (Input.GetMouseButtonDown(0))
        print("左键刚按下！");

    // 抬起瞬间（松开的那一帧才为 true）
    if (Input.GetMouseButtonUp(0))
        print("左键刚松开！");

    // 右键和中键同理
    if (Input.GetMouseButtonDown(1)) print("右键点击");
    if (Input.GetMouseButtonDown(2)) print("中键点击");
}
```

**`GetMouseButtonDown` vs `GetMouseButton` 的关键区别：**

```
时间线：    ─────────按下─────────松开─────→
           ↑                    ↑
           Down = true          Up = true
           (只这一帧)          (只这一帧)

GetMouseButton： 按下期间每帧都是 true
GetMouseButtonDown：只在下按的第一帧是 true
GetMouseButtonUp：  只在松开的第一帧是 true
```

```csharp
// ❌ 错误：在 FixedUpdate 中检测鼠标按键
void FixedUpdate()
{
    if (Input.GetMouseButtonDown(0))  // 可能漏掉！FixedUpdate 不是每帧执行
        Fire();
}

// ✅ 正确：在 Update 中检测
void Update()
{
    if (Input.GetMouseButtonDown(0))
        Fire();
}
```

### 2.2 鼠标位置

```csharp
// 语法
public static Vector3 mousePosition { get; }   // 屏幕上的像素坐标
```

```csharp
// 获取鼠标在屏幕上的位置
Vector3 mousePos = Input.mousePosition;
// (0, 0)             → 屏幕左下角
// (Screen.width, Screen.height) → 屏幕右上角

print("鼠标 X：" + mousePos.x + "，鼠标 Y：" + mousePos.y);

// ⚠️ mousePosition 返回的是屏幕坐标系的像素坐标
//  - 左下角是原点 (0, 0)
//  - 不是世界坐标！
//  - Z 值始终为 0
```

### 2.3 鼠标滚轮

```csharp
// 语法
public static float mouseScrollDelta { get; }

// 或者用 Axis（见下文）
Input.GetAxis("Mouse ScrollWheel")
```

```csharp
void Update()
{
    float scroll = Input.mouseScrollDelta.y;
    // 向上滚 → 正值（通常约 0.1~1）
    // 向下滚 → 负值

    if (scroll > 0)
        print("滚轮上滚，拉近视野");
    else if (scroll < 0)
        print("滚轮下滚，拉远视野");

    // 也可以直接当缩放值用
    Camera.main.fieldOfView -= scroll * 10f;
}
```

---

## 三、键盘输入

### 3.1 按键检测

```csharp
// 语法
public static bool GetKey(KeyCode key)           // 持续按下
public static bool GetKeyDown(KeyCode key)       // 按下瞬间
public static bool GetKeyUp(KeyCode key)         // 抬起瞬间

// 字符串版本（少用）
public static bool GetKey(string name)           // 按字母名，如 "a", "space"
```

```csharp
void Update()
{
    // ====== 持续按下 ======
    if (Input.GetKey(KeyCode.W))
        print("W 键按住中...每帧都打印");   // 按住时每帧 true

    // ====== 按下瞬间 ⭐ 最常用（跳跃、开火、技能）======
    if (Input.GetKeyDown(KeyCode.Space))
        print("空格键刚按下！");              // 只触发一次

    // ====== 抬起瞬间 ======
    if (Input.GetKeyUp(KeyCode.Space))
        print("空格键刚松开！");              // 只触发一次

    // ====== 字符串方式（不推荐）======
    if (Input.GetKey("a"))
        print("A 键被按住");
}
```

**三个方法的区别（和鼠标完全一样）：**

```
按下 W：  GetKeyDown = true（仅这一帧）
按住 W：  GetKey = true（每帧都是）
松开 W：  GetKeyUp = true（仅这一帧）
```

### 3.2 常用 KeyCode 对照表

| 类别 | KeyCode 枚举 |
|------|-------------|
| **字母键** | `KeyCode.A` ~ `KeyCode.Z` |
| **数字键** | `KeyCode.Alpha0` ~ `KeyCode.Alpha9` |
| **小键盘数字** | `KeyCode.Keypad0` ~ `KeyCode.Keypad9` |
| **功能键** | `KeyCode.F1` ~ `KeyCode.F12` |
| **方向键** | `KeyCode.UpArrow`、`DownArrow`、`LeftArrow`、`RightArrow` |
| **修饰键** | `KeyCode.LeftShift`、`KeyCode.RightShift`、`KeyCode.LeftControl`、`KeyCode.RightControl`、`KeyCode.LeftAlt`、`KeyCode.RightAlt` |
| **特殊键** | `KeyCode.Space`（空格）、`KeyCode.Return`（回车）、`KeyCode.Escape`（ESC）、`KeyCode.Tab`、`KeyCode.Backspace`、`KeyCode.Delete` |
| **鼠标** | `KeyCode.Mouse0`（= 左键）、`KeyCode.Mouse1`（= 右键）、`KeyCode.Mouse2`（= 中键） |

```csharp
// 组合键
if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
{
    print("Ctrl+S：保存");   // 实际上 Unity 已经占用了 Ctrl+S
}

if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W))
{
    print("Shift+W：加速跑");
}
```

---

## 四、虚拟轴（Axis）— 最常用的移动输入方式

虚拟轴在 Unity 的 **Edit → Project Settings → Input Manager** 中预设好，你直接使用即可。

### 4.1 `GetAxis()` — 平滑渐变值

```csharp
// 语法
public static float GetAxis(string axisName)
```

```csharp
void Update()
{
    // 水平和垂直轴（默认 WASD 或 方向键）
    float horizontal = Input.GetAxis("Horizontal");   // -1 ← 0 → 1，平滑过渡
    float vertical = Input.GetAxis("Vertical");       // -1 ← 0 → 1，平滑过渡

    // 用于移动
    Vector3 move = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
    transform.Translate(move, Space.World);

    // 鼠标移动轴（鼠标在屏幕上的移动量）
    float mouseX = Input.GetAxis("Mouse X");   // 鼠标横向移动量
    float mouseY = Input.GetAxis("Mouse Y");   // 鼠标纵向移动量
    // 用于旋转视野
    transform.Rotate(0, mouseX * sensitivity, 0);

    // 鼠标滚轮
    float scroll = Input.GetAxis("Mouse ScrollWheel");
}
```

**`GetAxis` 返回值的平滑特性：**

```
按下 W 键：
  GetAxis("Vertical")  →  0.0 → 0.1 → 0.3 → 0.6 → 0.9 → 1.0
                         （不是瞬间跳到 1，而是一个渐变过程）

松开 W 键：
  GetAxis("Vertical")  →  1.0 → 0.8 → 0.5 → 0.2 → 0.0
                         （不是瞬间跳到 0，而是慢慢回落）
```

这个平滑效果让移动和旋转更自然，适合摇杆、手柄、人物移动。

### 4.2 `GetAxisRaw()` — 瞬间跳变值

```csharp
// 语法
public static float GetAxisRaw(string axisName)
```

```csharp
// 和 GetAxis 一样用，但值只有整数：-1、0、1（无平滑过渡）
float rawH = Input.GetAxisRaw("Horizontal");  // 按下立刻是 1 或 -1，松开立刻是 0
float rawV = Input.GetAxisRaw("Vertical");
```

**`GetAxis` vs `GetAxisRaw` 对比：**

```
时间线：    ────────按住 W─────────────────────────松开──→

GetAxis:    0 → 0.3 → 0.8 → 1.0 →→→→→→→ 0.7 → 0.3 → 0
           （渐变上升/下降）

GetAxisRaw: 0 → 1 →→→→→→→→→→→→→→→→→→→→ 0
           （瞬间跳变）
```

| 场景 | 用哪个 |
|------|--------|
| 人物移动（WASD） | `GetAxis`（平滑） |
| 平台跳跃（需要立刻响应） | `GetAxisRaw`（瞬发） |
| 菜单选择（上下移动项） | `GetAxisRaw`（防止连发） |
| 赛车加速 | `GetAxis`（平滑） |

### 4.3 预设轴速查表

| 轴名称 | 对应的输入 | 返回 |
|--------|----------|------|
| `"Horizontal"` | A/D 或 ←→ 键 | -1 ~ 1 |
| `"Vertical"` | W/S 或 ↑↓ 键 | -1 ~ 1 |
| `"Mouse X"` | 鼠标横向移动速度 | 变化值 |
| `"Mouse Y"` | 鼠标纵向移动速度 | 变化值 |
| `"Mouse ScrollWheel"` | 鼠标滚轮 | 变化值 |
| `"Fire1"` | 左 Ctrl 或鼠标左键 | 0 ~ 1 |
| `"Fire2"` | 左 Alt 或鼠标右键 | 0 ~ 1 |
| `"Fire3"` | 左 Shift 或鼠标中键 | 0 ~ 1 |
| `"Jump"` | 空格键 | 0 ~ 1 |

```csharp
// Jump 轴的典型用法
if (Input.GetButtonDown("Jump"))
    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
```

---

## 五、虚拟按钮（Button）— 用名字代替键位

### 5.1 `GetButton()` / `GetButtonDown()` / `GetButtonUp()`

```csharp
// 语法（和 GetKey 系列一样，只是参数是"按钮名"而不是键位）
public static bool GetButton(string buttonName)
public static bool GetButtonDown(string buttonName)
public static bool GetButtonUp(string buttonName)
```

```csharp
void Update()
{
    // 按住
    if (Input.GetButton("Fire1"))
        Debug.Log("持续开火中...");

    // 按下瞬间 ⭐ 推荐（比 GetMouseButtonDown 更灵活）
    if (Input.GetButtonDown("Jump"))
        Debug.Log("跳跃！");

    // 抬起瞬间
    if (Input.GetButtonUp("Fire1"))
        Debug.Log("停止开火");

    // ⚠️ "Fire1" 默认绑定左 Ctrl 和鼠标左键——多个物理按键对应同一个逻辑按钮
    // 这就是用 Button 的好处：一次检测，自动支持多种输入设备
}
```

**为什么用 `GetButtonDown("Jump")` 而不是 `GetKeyDown(KeyCode.Space)`？**

```
GetKeyDown(KeyCode.Space)     → 硬编码了空格键，换键要改代码
GetButtonDown("Jump")         → 通过 Input Manager 配置，不改代码就能换键
                                 还自动支持手柄的跳跃按钮
```

---

## 六、`anyKey` / `anyKeyDown` — 任意按键

```csharp
// 检测是否有任何按键或鼠标点击被按下
if (Input.anyKeyDown)
{
    print("检测到任意按键被按下");
    print("具体是：" + Input.inputString);  // 按下的字符
}

// 持续检测
if (Input.anyKey)
{
    print("有键被按住中...");
}

// ⚠️ 这两个检测不到"松开"，只能检测按下和按住
// ⚠️ 鼠标点击也算"键"（因为 Mouse0 是一个 virtual key）
```

---

## 七、`inputString` — 输入文本

```csharp
// 语法
public static string inputString { get; }   // 本帧按下的字符
```

```csharp
void Update()
{
    if (Input.anyKeyDown)
    {
        print("本帧输入字符：" + Input.inputString);
    }
}
// 按 A → 打印 "a"
// 按 Shift+A → 打印 "A"
// ⚠️ 只会返回本帧内按下的字符，不会累积
```

---

## 八、锁定/隐藏鼠标（第一人称必备）

```csharp
// 语法
Cursor.lockState = CursorLockMode.Locked;     // 锁定在屏幕中央并隐藏
Cursor.lockState = CursorLockMode.Confined;   // 限制在窗口范围内
Cursor.lockState = CursorLockMode.None;       // 不限制（默认）
Cursor.visible = false;                        // 隐藏鼠标指针
```

```csharp
// 典型 FPS 模式
void Start()
{
    Cursor.lockState = CursorLockMode.Locked;  // 锁定鼠标
    Cursor.visible = false;                     // 隐藏光标
}

void Update()
{
    // 用鼠标移动量旋转摄像机
    float mouseX = Input.GetAxis("Mouse X");
    float mouseY = Input.GetAxis("Mouse Y");
    transform.Rotate(0, mouseX * sensitivity, 0);

    // 按 ESC 释放鼠标
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
```

---

## 九、常见错误和注意事项

### ❌ 错误 1：在 Update 外用 Input

```csharp
// ❌ Start 中不适用——这时还没任何输入产生
void Start()
{
    if (Input.GetKeyDown(KeyCode.Space))  // 永远不会触发
        Jump();
}

// ✅ Input 必须在 Update 中
void Update()
{
    if (Input.GetKeyDown(KeyCode.Space))
        Jump();
}
```

### ❌ 错误 2：在 FixedUpdate 中检测按键

```csharp
// ❌ FixedUpdate 不是每帧执行，可能漏掉按下事件
void FixedUpdate()
{
    if (Input.GetKeyDown(KeyCode.Space))  // 不稳定！
        Jump();
}

// ✅ 在 Update 中检测，在 FixedUpdate 中施加物理
void Update()
{
    if (Input.GetKeyDown(KeyCode.Space))
        shouldJump = true;              // 在 Update 中检测
}

void FixedUpdate()
{
    if (shouldJump)
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        shouldJump = false;
    }
}
```

### ❌ 错误 3：混淆 `GetAxis` 和 `GetAxisRaw`

```csharp
// 平台跳跃：需要立刻响应，用 GetAxisRaw
float move = Input.GetAxisRaw("Horizontal");   // ✅ -1 或 1，不拖泥带水

// 赛车加速/人物移动：需要平滑感，用 GetAxis
float move = Input.GetAxis("Horizontal");      // ✅ 渐变过渡
```

### ❌ 错误 4：鼠标位置当成世界坐标

```csharp
// ❌ Input.mousePosition 是屏幕像素坐标，不能直接当世界坐标用
transform.position = Input.mousePosition;   // 物体会跑到屏幕像素位置

// ✅ 用 Camera.ScreenToWorldPoint 转换
Vector3 mouseScreenPos = Input.mousePosition;
mouseScreenPos.z = 10f;   // 距离摄像机的深度
Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
transform.position = worldPos;
```

### ❌ 错误 5：忘记锁鼠标导致旋转出界

```csharp
// 做第一人称视角时忘记锁鼠标 → 鼠标移出窗口 → 不再响应移动
void Start()
{
    Cursor.lockState = CursorLockMode.Locked;   // 必须锁定
}
```

### ❌ 错误 6：用 `==` 比较 `GetAxis` 的值

```csharp
// ❌ GetAxis 几乎不会刚好等于 -1 或 1（因为有平滑过程）
if (Input.GetAxis("Horizontal") == 1) { }   // 很少命中

// ✅ 用范围判断
float h = Input.GetAxis("Horizontal");
if (Mathf.Abs(h) > 0.1f)    // 有输入（不关心具体值）
    Move(h);

// ✅ 或者直接用 GetAxisRaw（整数 -1/0/1）
if (Input.GetAxisRaw("Horizontal") == 1)    // 稳定命中
```

### ❌ 错误 7：中文字符串比较 KeyCode

```csharp
// ❌ GetKey 的字符串版本区分大小写，且只支持英文
Input.GetKey("空格");        // 无效！

// ✅ 用 KeyCode 枚举
Input.GetKeyDown(KeyCode.Space);
```

### ❌ 错误 8：每一帧多次调用同一个 Input

```csharp
// ❌ 没必要每帧检测 3 次
if (Input.GetKeyDown(KeyCode.Space)) DoA();
if (Input.GetKeyDown(KeyCode.Space)) DoB();
// DoA 和 DoB 都会执行，因为同一帧内两次 GetKeyDown 都返回 true

// ✅ 只检测一次，存到变量里
bool jumpPressed = Input.GetKeyDown(KeyCode.Space);
if (jumpPressed) DoA();   // A 和 B 只会执行一个，根据你的逻辑选择
```

---

## 十、完整示例

### 10.1 第一人称角色控制器（鼠标 + 键盘）

```csharp
public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSensitivity = 2f;
    private float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 鼠标旋转视角
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);   // 限制上下角度
        Camera.main.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
        transform.Rotate(Vector3.up * mouseX);

        // 键盘移动
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

        // 跳跃
        if (Input.GetButtonDown("Jump"))
            print("跳跃！");

        // 释放鼠标
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
```

### 10.2 简单的射击检测

```csharp
void Update()
{
    if (Input.GetMouseButtonDown(0))   // 左键点击
    {
        Fire();
    }

    if (Input.GetMouseButton(0))       // 左键按住（连发）
    {
        // 配合计时器控制射速
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 0f;
        }
    }
}
```

---

## 十一、检测时机速查表

| 你需要... | 用哪个 |
|----------|--------|
| 按住 W 持续前进 | `Input.GetAxis("Vertical")` 或 `Input.GetKey(KeyCode.W)` |
| 空格只跳一次 | `Input.GetKeyDown(KeyCode.Space)` |
| 鼠标点一下开火 | `Input.GetMouseButtonDown(0)` |
| 按住左键连发 | `Input.GetMouseButton(0)` + 计时器 |
| 检测松开技能键 | `Input.GetKeyUp(KeyCode.E)` |
| 鼠标移动旋转视角 | `Input.GetAxis("Mouse X")` / `"Mouse Y"` |
| W 键瞬间前冲（无平滑） | `Input.GetAxisRaw("Vertical")` |
| 任意按键跳过动画 | `Input.anyKeyDown` |
