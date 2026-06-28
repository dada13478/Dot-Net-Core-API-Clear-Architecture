# .NET Web API 學習架構

> 專案：`Dotnet_beginner_api` — 以商品管理 CRUD 為主軸，從單層架構逐步演進至 Clean Architecture

---

## 學習進度總覽

| 主題 | 狀態 |
|------|------|
| Web API 基礎 + Controller | ✅ 完成 |
| 資料模型 + 資料驗證 Attributes | ✅ 完成 |
| Entity Framework Core + Migrations | ✅ 完成 |
| Repository Pattern（Generic） | ✅ 完成 |
| Service Layer + 依賴注入 DI | ✅ 完成 |
| DTO（Data Transfer Object） | ✅ 完成 |
| Middleware（錯誤處理） | ✅ 完成 |
| CORS 設定 | ✅ 完成 |
| Clean Architecture 分層重構 | ✅ 完成 |
| 單元測試 / 整合測試 | ⬜ 尚未開始 |
| JWT 驗證授權 | ⬜ 尚未開始 |
| 回應封裝（ApiResponse wrapper） | ⬜ 尚未開始 |

---

## 目前專案結構（Clean Architecture）

```
Dotnet_beginner_api/
├── Dotnet_beginner_api.sln
└── src/
    ├── Domain/                          # 第 1 層：核心，零依賴
    │   ├── Entities/
    │   │   └── Product.cs               # 領域實體
    │   └── Interfaces/
    │       └── IGenericRepository.cs    # Repository 抽象介面
    │
    ├── Application/                     # 第 2 層：商業邏輯，只依賴 Domain
    │   ├── DTOs/
    │   │   ├── ProductDto.cs            # 回應用 DTO
    │   │   ├── CreateProductDto.cs      # 新增請求 DTO
    │   │   └── UpdateProductDto.cs      # 修改請求 DTO
    │   ├── Interfaces/
    │   │   └── IProductService.cs       # Service 抽象介面
    │   ├── Services/
    │   │   └── ProductService.cs        # Service 實作（含 Entity↔DTO 轉換）
    │   └── DependencyInjection.cs       # AddApplication()
    │
    ├── Infrastructure/                  # 第 3 層：技術實作，只依賴 Domain
    │   ├── Data/
    │   │   └── AppDbContext.cs          # EF Core DbContext
    │   ├── Repositories/
    │   │   └── GenericRepository.cs     # Generic Repository 實作
    │   ├── Migrations/                  # EF Core 自動產生的遷移檔
    │   └── DependencyInjection.cs       # AddInfrastructure()
    │
    └── API/                             # 第 4 層：進入點，依賴 Application + Infrastructure
        ├── Controllers/
        │   ├── ProductController.cs     # 商品 CRUD 端點
        │   └── WeatherForecastController.cs
        ├── Middleware/
        │   └── ErrorHandlingMiddleware.cs  # 全域例外處理
        ├── Program.cs                   # 應用程式啟動設定
        └── appsettings.json             # 連線字串等設定
```

---

## 依賴關係圖

```
API  ──────────────→  Application  ──→  Domain
 │                                        ↑
 └────────────────→  Infrastructure  ────┘

規則：箭頭方向 = 依賴方向
      內層（Domain）完全不知道外層的存在
```

---

## 各階段學習內容

### 階段 1｜Web API 基礎

**學到的概念**
- `[ApiController]`、`[Route]`、HTTP 動詞 Attribute（`[HttpGet]`、`[HttpPost]` 等）
- `IActionResult` 回傳型別（`Ok()`、`NotFound()`、`Created()`、`NoContent()`）
- `[FromBody]`、`[FromRoute]` 參數來源

**對應檔案**
- [src/API/Controllers/ProductController.cs](src/API/Controllers/ProductController.cs)

**API 端點**

| Method | Route | 功能 |
|--------|-------|------|
| GET | `/api/product` | 取得所有商品 |
| GET | `/api/product/{id}` | 取得單一商品 |
| POST | `/api/product` | 新增商品 |
| PUT | `/api/product/{id}` | 修改商品 |
| DELETE | `/api/product/{id}` | 刪除商品 |

---

### 階段 2｜資料模型與驗證

**學到的概念**
- Data Annotations：`[Required]`、`[StringLength]`、`[Range]`、`[Key]`、`[DatabaseGenerated]`
- Model Validation 自動在 Controller 觸發（`[ApiController]` 內建）

**對應檔案**
- [src/Domain/Entities/Product.cs](src/Domain/Entities/Product.cs)

```csharp
public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "商品名稱不能為空")]
    [StringLength(100)]
    public string Name { get; set; }

    [Range(1, 999999, ErrorMessage = "價格必須在 1 ~ 999999 之間")]
    public int Price { get; set; }
}
```

---

### 階段 3｜Entity Framework Core

**學到的概念**
- `DbContext`、`DbSet<T>` 代表資料表
- Code-First 開發流程（先寫 C# 類別，再產生資料庫）
- Migration 指令：`dotnet ef migrations add`、`dotnet ef database update`
- 連線字串設定在 `appsettings.json`

**對應檔案**
- [src/Infrastructure/Data/AppDbContext.cs](src/Infrastructure/Data/AppDbContext.cs)
- [src/Infrastructure/Migrations/](src/Infrastructure/Migrations/)

**EF Core 指令**（在 solution 根目錄執行）
```bash
# 新增 migration
dotnet ef migrations add <名稱> --project src/Infrastructure --startup-project src/API

# 更新資料庫
dotnet ef database update --project src/Infrastructure --startup-project src/API
```

---

### 階段 4｜Repository Pattern

**學到的概念**
- Repository 把「資料存取邏輯」抽離出 Service，讓 Service 不需要知道 EF Core
- Generic Repository `<T>` 讓一份程式碼套用所有 Entity
- `IGenericRepository<T>` 介面讓上層只依賴抽象，不依賴實作

**對應檔案**
- [src/Domain/Interfaces/IGenericRepository.cs](src/Domain/Interfaces/IGenericRepository.cs)（介面）
- [src/Infrastructure/Repositories/GenericRepository.cs](src/Infrastructure/Repositories/GenericRepository.cs)（實作）

```
Controller → Service → IGenericRepository（介面）
                              ↑
                       GenericRepository（EF Core 實作）
```

---

### 階段 5｜Service Layer + 依賴注入

**學到的概念**
- Service 負責處理商業邏輯，Controller 只負責路由
- 依賴注入（DI）：透過建構子注入（Constructor Injection）
- `AddScoped`、`AddSingleton`、`AddTransient` 的生命週期差異
- Extension Method 統一管理服務註冊

**對應檔案**
- [src/Application/Interfaces/IProductService.cs](src/Application/Interfaces/IProductService.cs)
- [src/Application/Services/ProductService.cs](src/Application/Services/ProductService.cs)
- [src/Application/DependencyInjection.cs](src/Application/DependencyInjection.cs)
- [src/Infrastructure/DependencyInjection.cs](src/Infrastructure/DependencyInjection.cs)

**DI 生命週期比較**

| 類型 | 建立時機 | 適用情境 |
|------|----------|----------|
| `AddScoped` | 每個 HTTP 請求一個 | Repository、Service（最常用） |
| `AddSingleton` | 整個應用程式只有一個 | 設定檔、Cache |
| `AddTransient` | 每次注入都建立新的 | 輕量的無狀態服務 |

---

### 階段 6｜DTO（Data Transfer Object）

**學到的概念**
- DTO 用來控制 API 的輸入/輸出格式，避免直接暴露 Entity
- 好處：Entity 改欄位不影響 API 介面；可以針對不同操作設計不同欄位
- 手動 Mapping（Entity ↔ DTO）

**對應檔案**
- [src/Application/DTOs/ProductDto.cs](src/Application/DTOs/ProductDto.cs)（回應）
- [src/Application/DTOs/CreateProductDto.cs](src/Application/DTOs/CreateProductDto.cs)（新增請求）
- [src/Application/DTOs/UpdateProductDto.cs](src/Application/DTOs/UpdateProductDto.cs)（修改請求）

**資料流向**
```
[請求進來]  CreateProductDto  →  ProductService  →  Product (Entity)  →  資料庫
[回應出去]  資料庫           →  Product (Entity)  →  ProductService   →  ProductDto
```

---

### 階段 7｜Middleware

**學到的概念**
- Middleware 是 HTTP 請求管道（Pipeline）的一環
- 可以在請求進來、回應出去時插入自訂邏輯
- 全域例外處理：避免例外直接崩潰，統一回傳結構化錯誤

**對應檔案**
- [src/API/Middleware/ErrorHandlingMiddleware.cs](src/API/Middleware/ErrorHandlingMiddleware.cs)

**Middleware 管道**
```
Request → ErrorHandlingMiddleware → Routing → Controller → Response
              ↑
         (如果後面有例外，這裡攔截並回傳 500)
```

---

### 階段 8｜Clean Architecture 重構

**學到的概念**
- 依賴規則（Dependency Rule）：外層依賴內層，內層不依賴外層
- 各層職責清楚，改 EF Core 換成其他 ORM 只需改 Infrastructure 層
- Project Reference vs NuGet Package 的管理

**依賴規則**
```
Domain        — 零依賴，純 C# 類別和介面
Application   — 只依賴 Domain
Infrastructure — 只依賴 Domain（不知道 Application 的存在）
API           — 依賴 Application + Infrastructure
```

**為什麼 Infrastructure 不依賴 Application？**
> Infrastructure 只負責「怎麼存資料」，Service 的邏輯屬於 Application，
> 兩者分開才能各自替換，例如把 SQL Server 換成 MongoDB 不會影響 Service。

---

## 下一步學習建議

### 優先學習

1. **回應封裝（ApiResponse Wrapper）**
   - 統一 API 回應格式：`{ success, data, message, errors }`
   - 讓前端更容易處理

2. **單元測試**
   - 用 xUnit 測試 `ProductService` 的邏輯
   - Mock `IGenericRepository<T>` 用 Moq 套件

3. **JWT 驗證**
   - 新增登入端點、產生 Token
   - `[Authorize]` 保護 API

### 進階學習

4. **CQRS（Command Query Responsibility Segregation）**
   - 讀（Query）和寫（Command）分開
   - 搭配 MediatR 套件

5. **AutoMapper**
   - 取代手動 `ToDto()` 方法，自動對應屬性

6. **Logging（結構化日誌）**
   - 用 Serilog 記錄到檔案或資料庫

---

## 快速指令參考

```bash
# 執行 API
cd src/API
dotnet run

# 新增 EF Migration
dotnet ef migrations add <名稱> --project src/Infrastructure --startup-project src/API

# 更新資料庫
dotnet ef database update --project src/Infrastructure --startup-project src/API

# Build 整個 solution
dotnet build Dotnet_beginner_api.sln
```

---

*Swagger UI：http://localhost:5168/swagger（開發環境）*
