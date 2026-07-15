# Unity GameObject 完全指南

> **GameObject 是 Unity 场景中一切事物的基础容器。** 每个存在于 Hierarchy 中的物体都是 GameObject。

---

## 一、什么是 GameObject

### 1.1 本质

`GameObject` 是 Unity 中最基本的**实体类**，场景里的每个物体本质上都是一个 `GameObject`。它本身不做任何事情——它只是一个**空容器**，通过挂载**组件（Component）**来获得功能和外观。

```
GameObject = 一个空箱子
Component  = 往箱子里放的工具

空的 GameObject → 什么都没有，看不见，没逻辑
+ Transform → 有位置了（这个自动带着，删不掉）
+ MeshRenderer + MeshFilter → 能看见了
+ Collider → 能碰撞了
+ 你的脚本 → 有逻辑了
```

### 1.2 每个 GameObject 至少有一个 Transform

```csharp
// 即使是最"空"的 GameObject，也强制携带 Transform
GameObject obj = new GameObject("空物体");
// obj.transform 已经存在，无需添加
```

### 1.3 GameObject 和 Component 的关系

```csharp
// GameObject 管理着所有绑在它身上的 Component
GameObject obj = this.gameObject;

// 通过 GameObject 获取组件
Transform t = obj.transform;              // 拿 Transform
BoxCollider bc = obj.GetComponent<BoxCollider>();  // 拿碰撞体

// 通过组件可以反查它属于哪个 GameObject
BoxCollider collider = GetComponent<BoxCollider>();
GameObject owner = collider.gameObject;   // 反查：这个碰撞体挂在谁身上
```

---

## 二、GameObject 的成员变量（属性）

### 2.1 `name` — 名字

```csharp
// 语法
public string name { get; set; }
```

```csharp
// 读取名字
print(gameObject.name);    // 比如 "Cube"

// 修改名字
gameObject.name = "我的方块";

// ⚠️ 名字不是唯一标识
// 场景中可以有多个同名的 GameObject，不要依赖名字来找对象
```

### 2.2 `activeSelf` — 自身激活状态（只读）

```csharp
// 语法
public bool activeSelf { get; }   // 只有 get
```

```csharp
print(gameObject.activeSelf);   // true 或 false
// 只读！要修改激活状态必须用 SetActive() 方法
```

> **`activeSelf` vs `activeInHierarchy` 的区别见下文 2.3 节。**

### 2.3 `activeInHierarchy` — 层级激活状态（只读）

```csharp
// 语法
public bool activeInHierarchy { get; }   // 只有 get
```

| 属性 | 含义 | 能改吗 |
|------|------|:---:|
| `activeSelf` | 只看**自身**有没有被 `SetActive(false)` | ❌ 只读 |
| `activeInHierarchy` | 看**自身 + 全部祖先**是否都激活 | ❌ 只读 |

```
父物体 SetActive(false)
    └── 子物体 SetActive(true)
        → activeSelf = true          (自己激活了)
        → activeInHierarchy = false  (但父亲关了，所以实际上不可见)
```

```csharp
// 判断物体是否真正在场景中"活着"
if (gameObject.activeInHierarchy)
{
    // 物体确实在运行（自己和所有祖先都激活了）
}
```

### 2.4 `isStatic` — 是否标记为静态

```csharp
// 语法
public bool isStatic { get; set; }
```

```csharp
gameObject.isStatic = true;   // 标记为静态

// ⚠️ 标记为静态后：
//  - 该物体不能移动
//  - Unity 会对其进行光照预计算（烘焙）
//  - 可以参与静态批处理（优化性能）
//  - 不要对运行时移动的物体勾选此选项！
```

### 2.5 `tag` — 标签

```csharp
// 语法
public string tag { get; set; }
```

```csharp
// 设置标签
gameObject.tag = "Player";

// 通过标签查找对象
GameObject player = GameObject.FindGameObjectWithTag("Player");
GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

// 标签比较（推荐用 CompareTag，性能更好）
if (gameObject.CompareTag("Player"))
{
    // 是玩家
}

// ⚠️ 标签必须在 Unity 的 Tag Manager 中预先定义
// ⚠️ gameObject.tag = "不存在的标签" → 运行时不会报错，但标签不会被设置
```

### 2.6 `layer` — 层级

```csharp
// 语法
public int layer { get; set; }
```

```csharp
// 设置层级
gameObject.layer = 5;               // 用数字
gameObject.layer = LayerMask.NameToLayer("UI");  // 用名字 ✅ 推荐

// 判断是否在某个层级
if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
{
    // ...
}

// ⚠️ Layer 控制物理碰撞（哪些层之间可以碰撞）和摄像机渲染（哪些层被渲染）
// ⚠️ Layer 必须在 Unity 的 Layer Manager 中预先定义（0-7 是内置的，8-31 可自定义）
```

### 2.7 `transform` — 位置组件

```csharp
// 语法（MonoBehaviour 内部可直接用）
public Transform transform { get; }
```

```csharp
// 直接通过属性访问
Transform t = gameObject.transform;

// MonoBehaviour 类里可以省掉 gameObject
Transform t = transform;   // 等价于 this.gameObject.transform
```

### 2.8 `scene` — 所属场景

```csharp
// 语法
public Scene scene { get; }
```

```csharp
print(gameObject.scene.name);       // 所在场景的名字
print(gameObject.scene.isLoaded);   // 场景是否已加载
```

---

## 三、GameObject 的静态方法（类级别的方法）

### 3.1 `CreatePrimitive()` — 创建内置几何体

```csharp
// 语法
public static GameObject CreatePrimitive(PrimitiveType type)
```

```csharp
GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

// ⚠️ 创建的物体自动带有 MeshRenderer 和 Collider
// ⚠️ 创建位置默认在 (0, 0, 0)
```

### 3.2 `Find()` — 按名字查找（不推荐）

```csharp
// 语法
public static GameObject Find(string name)
```

```csharp
GameObject obj = GameObject.Find("玩家");
// ⚠️ 遍历整个场景，性能差
// ⚠️ 找不到返回 null
// ⚠️ 无法找到失活的对象
// ⚠️ 多个同名对象时返回哪个不确定
// ⚠️ 不要在 Update() 中使用！
```

### 3.3 `FindGameObjectWithTag()` — 按标签查找单个

```csharp
// 语法
public static GameObject FindGameObjectWithTag(string tag)
// 或
public static GameObject FindWithTag(string tag)   // 功能相同，名字不同
```

```csharp
GameObject player = GameObject.FindGameObjectWithTag("Player");
if (player != null)
{
    // 找到了
}

// ⚠️ 只能找到激活的对象
// ⚠️ 多个同 Tag 对象时返回第一个（不确定哪个）
// ⚠️ 要确保 Tag 在 Tag Manager 中已定义
```

### 3.4 `FindGameObjectsWithTag()` — 按标签查找多个

```csharp
// 语法
public static GameObject[] FindGameObjectsWithTag(string tag)
```

```csharp
GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
print("找到 " + enemies.Length + " 个敌人");

foreach (GameObject enemy in enemies)
{
    // 处理每个敌人
}

// ⚠️ 找不到时返回空数组（Length = 0），不会返回 null
// ⚠️ 同样找不到失活的对象
```

### 3.5 `FindObjectOfType<T>()` — 按脚本类型查找

```csharp
// 语法
public static T FindObjectOfType<T>() where T : Object
```

```csharp
// 找到场景中第一个挂载了 PlayerController 脚本的对象
PlayerController pc = GameObject.FindObjectOfType<PlayerController>();

// ⚠️ ⚠️ ⚠️ 性能最差的查找方法！
//   遍历每个 GameObject → 遍历每个对象上的所有脚本
//   只在 Start()/Awake() 中用一次
//   绝对不要在 Update() 中用！
```

### 3.6 `FindObjectsOfType<T>()` — 按脚本类型查找多个

```csharp
// 语法
public static T[] FindObjectsOfType<T>()
```

```csharp
EnemyAI[] allEnemies = GameObject.FindObjectsOfType<EnemyAI>();
```

### 3.7 `Instantiate()` — 克隆对象

```csharp
// 语法（常用重载）
public static Object Instantiate(Object original)
public static Object Instantiate(Object original, Vector3 position, Quaternion rotation)
public static Object Instantiate(Object original, Transform parent)
public static Object Instantiate(Object original, Transform parent, bool instantiateInWorldSpace)
```

```csharp
// 基础克隆（在原对象的位置生成）
GameObject clone = Instantiate(prefab);

// 指定位置和旋转
GameObject clone = Instantiate(prefab, new Vector3(0, 5, 0), Quaternion.identity);

// 克隆并直接设为子物体
GameObject clone = Instantiate(prefab, parentTransform);

// 克隆并设为子物体，同时指定是否保持世界位置
GameObject clone = Instantiate(prefab, parentTransform, true);
//  instantiateInWorldSpace = true：克隆体保持 prefab 的世界位置
//  instantiateInWorldSpace = false：克隆体的局部位置使用 prefab 的局部位置

// ⚠️ Instantiate 可以克隆 Prefab（预设体），也可以克隆场景中已存在的对象
// ⚠️ 克隆出来的对象名字会加 "(Clone)" 后缀
```

### 3.8 `Destroy()` — 删除对象

```csharp
// 语法
public static void Destroy(Object obj)
public static void Destroy(Object obj, float t)   // 延迟 t 秒后删除
```

```csharp
// 立即标记删除（下一帧真正移除）
Destroy(gameObject);

// 延迟 3 秒后删除
Destroy(gameObject, 3f);

// 也可以删除组件
Destroy(GetComponent<Rigidbody>());

// ⚠️ Destroy 不是立即移除！
//   它给对象加一个"删除标记"，在下一帧末尾真正从内存移除
// ⚠️ 如果真的要立即删除，用 DestroyImmediate（但编辑器外极少使用）
```

### 3.9 `DestroyImmediate()` — 立即删除

```csharp
// 语法
public static void DestroyImmediate(Object obj)
```

```csharp
// ⚠️ 仅在编辑器脚本中使用！运行时不要用！
// 会导致资源立即释放，可能破坏依赖关系
```

### 3.10 `DontDestroyOnLoad()` — 切换场景不销毁

```csharp
// 语法
public static void DontDestroyOnLoad(Object target)
```

```csharp
// 让这个 GameObject 在切换场景时不被自动删除
DontDestroyOnLoad(gameObject);

// ⚠️ 通常用于：
//  - 背景音乐（Audio Manager）
//  - 全局管理器（Game Manager）
//  - 玩家数据（在整个游戏过程中保留）
//
// ⚠️ 注意：
//  1. 只能保证"场景根物体"不被销毁
//  2. 如果物体有父物体，先要 SetParent(null) 变成根物体
//  3. 每个 DontDestroyOnLoad 的物体都会创建新场景保存它们
```

---

## 四、GameObject 的实例方法

### 4.1 `SetActive()` — 激活/失活

```csharp
// 语法
public void SetActive(bool value)
```

```csharp
// 激活
gameObject.SetActive(true);

// 失活（物体和所有子物体都不会渲染、不会运行脚本）
gameObject.SetActive(false);

// ⚠️ 失活时：
//  - 该物体上所有脚本的 Update() 不会被调用
//  - Start()/Awake() 也不会重新执行（和 enabled 不同）
//  - 子物体也全部不运行
//  - 子物体的 activeSelf 值不变，但 activeInHierarchy 变为 false
```

### 4.2 `AddComponent<T>()` — 动态添加脚本/组件

```csharp
// 语法
public T AddComponent<T>() where T : Component
public Component AddComponent(Type componentType)
```

```csharp
// 泛型方式 ✅ 推荐
Rigidbody rb = gameObject.AddComponent<Rigidbody>();
rb.mass = 2f;
rb.drag = 0.5f;

// 动态添加自己的脚本
MyScript script = gameObject.AddComponent<MyScript>();
script.speed = 10;

// Type 方式
MyScript ms = gameObject.AddComponent(typeof(MyScript)) as MyScript;

// ⚠️ MonoBehaviour 脚本不能 new，必须用 AddComponent 创建
// ⚠️ AddComponent 返回刚创建的组件引用，拿到就能直接配置
// ⚠️ 不能重复添加同类型组件（如只能有一个 Rigidbody）
```

### 4.3 `GetComponent<T>()` — 获取组件

```csharp
// 语法（在 Component/MonoBehaviour 内部）
public T GetComponent<T>()
public Component GetComponent(Type type)
public Component GetComponent(string typeName)
```

```csharp
// 获取自身某个组件
BoxCollider bc = GetComponent<BoxCollider>();
if (bc != null)
{
    bc.size = new Vector3(2, 2, 2);
}

// ⚠️ 泛型方式速度最快，也最安全
// ⚠️ 找不到返回 null，必须判空
// ⚠️ Gameobject 本身没有 GetComponent，是通过 Component 基类提供的
//    但你在 MonoBehaviour 里可以直接用（因为 MonoBehaviour 继承自 Component）
```

### 4.4 `GetComponents<T>()` — 获取多个同类型组件

```csharp
// 语法
public T[] GetComponents<T>()
public void GetComponents<T>(List<T> results)
```

```csharp
// 返回数组
MyScript[] scripts = GetComponents<MyScript>();

// 填充到已有 List（不产生 GC，推荐在 Update 中这样用）
List<MyScript> results = new List<MyScript>();
GetComponents<MyScript>(results);   // results 现在装着所有 MyScript
```

### 4.5 `GetComponentInChildren<T>()` — 从子物体获取组件

```csharp
// 语法
public T GetComponentInChildren<T>()
public T GetComponentInChildren<T>(bool includeInactive)
```

```csharp
// 查找自己 + 所有子物体
MyScript s = GetComponentInChildren<MyScript>();

// 包括失活的子物体
MyScript s = GetComponentInChildren<MyScript>(true);

// ⚠️ 默认 includeInactive = false，跳过失活子物体
// ⚠️ 也会查找自己身上
// ⚠️ 返回找到的第一个（深度优先遍历）
```

### 4.6 `GetComponentsInChildren<T>()` — 从子物体获取多个组件

```csharp
// 语法
public T[] GetComponentsInChildren<T>()
public T[] GetComponentsInChildren<T>(bool includeInactive)
```

```csharp
// 拿到所有子物体上的碰撞体
Collider[] colliders = GetComponentsInChildren<Collider>();
```

### 4.7 `GetComponentInParent<T>()` — 从父物体获取组件

```csharp
// 语法
public T GetComponentInParent<T>()
public T GetComponentInParent<T>(bool includeInactive)
```

```csharp
// 向上查找（父 → 祖父 → ...）直到找到或到头
Rigidbody rb = GetComponentInParent<Rigidbody>();

// ⚠️ 也会查找自己身上
// ⚠️ 一般不需要传 includeInactive，因为父物体失活时子物体的逻辑也不会跑
```

### 4.8 `TryGetComponent<T>()` — 安全获取组件

```csharp
// 语法
public bool TryGetComponent<T>(out T component)
```

```csharp
// 安全方式：不需要手动判空
if (TryGetComponent<Rigidbody>(out Rigidbody rb))
{
    rb.AddForce(Vector3.up * 10);   // 一定能用，不是 null
}

// ⚠️ 比 GetComponent + 判空性能略好
// ⚠️ Unity 2019.3+ 可用
```

### 4.9 `CompareTag()` — 标签比较（推荐）

```csharp
// 语法
public bool CompareTag(string tag)
```

```csharp
// ✅ 推荐：性能好
if (gameObject.CompareTag("Player"))
{
    // ...
}

// ⚠️ 和 gameObject.tag == "Player" 效果一样，但 CompareTag 不会产生 GC
//    （tag == "..." 每次比较都分配内存）
```

### 4.10 `SendMessage()` / `BroadcastMessage()` / `SendMessageUpwards()` — 消息广播

```csharp
// ⚠️ 都不建议使用！性能极差！

// 给自己身上所有脚本发消息，有指定函数名的就执行
gameObject.SendMessage("DoSomething");

// 广播给所有子物体
gameObject.BroadcastMessage("DoSomething");

// 发送给父物体
gameObject.SendMessageUpwards("DoSomething");

// ⚠️ 这些方法通过字符串匹配函数名，极慢，且容易拼写错误不报错
// ⚠️ 查找所有脚本 → 比对函数名 → 反射调用 → 极慢
// ⚠️ 用接口或者直接拿组件调方法代替！
```

---

## 五、创建和删除 GameObject

### 5.1 创建空 GameObject

```csharp
// 方式一：无参数 → 名字是默认的 "GameObject"
GameObject obj1 = new GameObject();

// 方式二：指定名字
GameObject obj2 = new GameObject("我的空物体");

// 方式三：指定名字 + 同时添加组件
GameObject obj3 = new GameObject("带脚本的空物体", typeof(MyScript), typeof(BoxCollider));
// ⚠️ 第三个参数是 params Type[]，可以传任意多个组件类型
```

### 5.2 通过 Prefab 创建

```csharp
// Prefab 必须先在 Unity 编辑器中创建好，然后拖到脚本的 public 字段里

public GameObject bulletPrefab;   // Inspector 拖入

void Fire()
{
    Instantiate(bulletPrefab, transform.position, transform.rotation);
}
```

### 5.3 通过 Resources 加载

```csharp
// 把 Prefab 放在 Assets/Resources 文件夹下
GameObject prefab = Resources.Load<GameObject>("Enemy");
Instantiate(prefab);

// ⚠️ Resources 加载的资源会一直占用内存
// ⚠️ 大型项目建议使用 Addressables 系统代替
```

### 5.4 删除对象

```csharp
// 销毁自身所在的 GameObject
Destroy(gameObject);

// 销毁另一个对象
Destroy(targetObject);

// 延迟 5 秒销毁
Destroy(gameObject, 5f);

// ⚠️ 重要：Destroy 是异步的，不会立即生效！
//    调用 Destroy(obj) 后，在当前帧的剩余逻辑中 obj 仍然存在
//    真正销毁发生在当前帧末尾或下一帧

// 验证：
Destroy(gameObject);
print(gameObject == null);   // 仍然是 false！（虽然打了删除标记）

// ⚠️ 可以用这个特性做延迟检查：
// if(obj == null) → Destroy 后仍然 false，直到帧结束才变 true
```

---

## 六、GameObject 的层次结构关系

### 6.1 父子关系的影响

```
父物体
  ├── 位置：子物体的世界位置 = 父位置 + 子局部位置
  ├── 旋转：子物体的世界旋转 = 父旋转 × 子局部旋转
  ├── 缩放：子物体的世界缩放 = 父缩放 × 子局部缩放（结果 ≈ lossyScale）
  └── 失活：父物体 SetActive(false) → 所有子物体也不可见
```

### 6.2 找父/子/根

```csharp
// 通过 Transform 访问
Transform parent = transform.parent;      // 父 Transform
Transform root = transform.root;          // 根 Transform（追溯到最顶层）
int count = transform.childCount;         // 直接子物体数量
Transform child = transform.GetChild(0);  // 第 0 号子物体

// 深层查找（不推荐频繁使用）
Transform found = transform.Find("身体/手臂/手/指尖");

// 遍历所有直接子物体
foreach (Transform child in transform)
{
    print(child.name);
}
```

---

## 七、GameObject 常用模式

### 7.1 单例管理器

```csharp
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);   // 已经有了，销毁自己
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);   // 切场景不销毁
    }
}
```

### 7.2 对象池（Object Pool）

```csharp
// 避免频繁 Instantiate/Destroy（会产生 GC）

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    private Queue<GameObject> pool = new Queue<GameObject>();

    public GameObject GetBullet()
    {
        if (pool.Count > 0)
        {
            GameObject bullet = pool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        return Instantiate(bulletPrefab);   // 池子空了，才真正创建
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);   // 回收进池子，不销毁
    }
}
```

### 7.3 拖拽引用 vs 代码查找

```csharp
// ✅ 方式一：Inspector 拖拽（推荐！明确、高效）
public GameObject target;
public Transform spawnPoint;

// ⚠️  方式二：代码查找（仅在无法拖拽时使用）
GameObject target = GameObject.Find("敌人");
// 只应在 Start()/Awake() 中用一次，然后缓存起来
```

---

## 八、GameObject 和 Component 的区别与关系

| | GameObject | Component |
|---|---|---|
| **是什么** | 场景中的实体 | 附着在实体上的功能模块 |
| **你能看到它** | Hierarchy 中的一行 | Inspector 中的各个折叠栏 |
| **可以独立存在** | ✅ 可以（空物体） | ❌ 必须挂载在 GameObject 上 |
| **有 name** | ✅ | ❌（组件的名字是类型名） |
| **有 transform** | ✅ 强制携带 | ❌（但可以通过 gameObject.transform 访问） |
| **可以被 SetActive** | ✅ | ❌（但可以用 enabled 控制） |
| **可以 Instantiate** | ✅ | ❌ 不能单独克隆 |
| **创建方式** | `new GameObject()` | `AddComponent<T>()` |
| **销毁方式** | `Destroy(gameObject)` | `Destroy(this)` 只删脚本 |

---

## 九、常见注意事项汇总

### ❌ 错误 1：在 Update() 中使用 Find

```csharp
// ❌ 每帧遍历整个场景，卡死
void Update()
{
    GameObject player = GameObject.Find("Player");
}

// ✅ 在 Start 中找一次，缓存起来
GameObject player;
void Start()
{
    player = GameObject.Find("Player");
}
```

### ❌ 错误 2：直接 new MonoBehaviour

```csharp
// ❌ 编译能过，但 Start/Update 不会执行
MyScript s = new MyScript();

// ✅ 必须通过 AddComponent
MyScript s = gameObject.AddComponent<MyScript>();
```

### ❌ 错误 3：Destroy 后立即判断 null

```csharp
Destroy(gameObject);
// ❌ 此时 gameObject 还没真正销毁，不会触发 null
if (gameObject == null) { }     // → false

// ✅ 用 Unity 的隐式 null 检查（Unity 重载了 == 运算符）
// Destroy 后虽然 C# 对象还在，但 Unity 让 == null 返回 true
// （仅限 UnityEngine.Object 的子类）
```

### ❌ 错误 4：GetComponent 不判空

```csharp
// ❌ 脚本可能没挂，GetComponent 返回 null，然后崩了
Rigidbody rb = GetComponent<Rigidbody>();
rb.AddForce(Vector3.up * 10);    // NullReferenceException!

// ✅ 先判空
Rigidbody rb = GetComponent<Rigidbody>();
if (rb != null)
    rb.AddForce(Vector3.up * 10);

// ✅ 或用 TryGetComponent
if (TryGetComponent<Rigidbody>(out Rigidbody rb2))
    rb2.AddForce(Vector3.up * 10);
```

### ❌ 错误 5：忘记 Instantiate 不会自动设位置

```csharp
// Instantiate 的位置参数如果不传，使用 prefab 本身的原始位置
Instantiate(prefab);                                // 在 prefab 默认位置
Instantiate(prefab, transform.position, Quaternion.identity);  // ✅ 明确指定
```

### ❌ 错误 6：依赖名字来区分对象

```csharp
// ❌ 名字不是唯一标识，容易冲突
GameObject obj = GameObject.Find("Cube");  // 场景里可能有 10 个 Cube

// ✅ 用 Tag、Layer 或者 public 字段拖拽
public GameObject myCube;   // Inspector 中精确指定想要的那个
```

### ❌ 错误 7：SetActive(false) 后通过 Find 找不到

```csharp
// 失活的对象对所有 Find 方法都不可见
gameObject.SetActive(false);
GameObject obj = GameObject.Find("我的物体");  // 找不到了！

// ✅ 如果要从代码找，保持激活，用 enabled=false 禁用脚本即可
// ✅ 或者使用 GetComponentInChildren(true) 传 true 来查找失活子物体
```

### ❌ 错误 8：DontDestroyOnLoad 后忘记处理重复

```csharp
// ❌ 如果场景重新加载，会创建第二个单例
void Awake()
{
    DontDestroyOnLoad(gameObject);   // 切场景回来又多一个
}

// ✅ 必须加重复检查
void Awake()
{
    if (FindObjectsOfType<MyManager>().Length > 1)
    {
        Destroy(gameObject);
        return;
    }
    DontDestroyOnLoad(gameObject);
}
```

---

## 十、性能参考

| 操作 | 开销 | 建议 |
|------|:---:|------|
| `gameObject.name` | 低 | 自由使用 |
| `gameObject.tag` / `CompareTag` | 低 | `CompareTag` 更佳（无 GC） |
| `gameObject.SetActive()` | **高** | 不要每帧大量调 |
| `GameObject.Find("name")` | **极高** | 只在 Start 中用一次 |
| `GameObject.FindWithTag("tag")` | 高 | 缓存结果，不要每帧调用 |
| `GameObject.FindObjectOfType<T>()` | **极高** | 只在 Start/Awake 中用 |
| `Instantiate()` | **高** | 频繁创建用对象池代替 |
| `Destroy()` | 中 | 批量销毁优于逐个销毁 |
| `AddComponent<T>()` | **高** | 不要在 Update 中调用 |
| `GetComponent<T>()` | 低 | 自由使用，但缓存重复访问 |
| `GetComponentInChildren<T>()` | 中高 | 缓存结果，不要每帧调用 |

---

## 十一、速查表

| 想要做的事 | 代码 |
|-----------|------|
| 获取自己的名字 | `gameObject.name` |
| 改名 | `gameObject.name = "新名字";` |
| 失活自己 | `gameObject.SetActive(false);` |
| 激活自己 | `gameObject.SetActive(true);` |
| 判断是否激活 | `if (gameObject.activeInHierarchy)` |
| 设置标签 | `gameObject.tag = "Player";` |
| 标签比较 | `if (gameObject.CompareTag("Player"))` |
| 设置层级 | `gameObject.layer = LayerMask.NameToLayer("UI");` |
| 找单个对象（按名） | `GameObject.Find("名字");` |
| 找单个对象（按标签） | `GameObject.FindGameObjectWithTag("Tag");` |
| 找多个对象（按标签） | `GameObject.FindGameObjectsWithTag("Tag");` |
| 找多个对象（按脚本） | `GameObject.FindObjectsOfType<脚本>();` |
| 克隆 Prefab | `Instantiate(prefab);` |
| 克隆并指定位置 | `Instantiate(prefab, pos, rot);` |
| 删除对象 | `Destroy(gameObject);` |
| 延迟删除 | `Destroy(gameObject, 3f);` |
| 切场景不销毁 | `DontDestroyOnLoad(gameObject);` |
| 创建空物体 | `new GameObject("名字");` |
| 创建几何体 | `GameObject.CreatePrimitive(PrimitiveType.Cube);` |
| 动态添加脚本 | `AddComponent<MyScript>();` |
| 获取组件 | `GetComponent<Rigidbody>();` |
| 安全获取组件 | `if (TryGetComponent<T>(out T c)) { }` |
| 获取子物体上的组件 | `GetComponentInChildren<T>();` |
| 获取父物体上的组件 | `GetComponentInParent<T>();` |
