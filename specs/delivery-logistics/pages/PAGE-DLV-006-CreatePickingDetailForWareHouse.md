# DLV-006 — CreatePickingDetailForWareHouse

> Reverse-engineered from source code only. Facts below are traceable to the files cited in section 28.
> Anything not determinable from the repository is marked **UNKNOWN** — it is not guessed.

## 1. Page Information

| Field | Value |
|---|---|
| Page ID | `DLV-006` |
| Module | `delivery-logistics` |
| File name | `CreatePickingDetailForWareHouse.aspx` |
| Route | `/SInventory_UI/CreatePickingDetailForWareHouse.aspx` |
| `Title` directive | _not set_ |
| Master page | `~/MasterPages/MainMasterPage.master` |
| Code-behind | `CreatePickingDetailForWareHouse.aspx.cs` |
| Code-behind class | `SInventory_UI_CreatePickingDetailForWareHouse` |
| Markup lines | 350 |
| Code-behind lines | 330 |
| Server controls | 41 |
| GridViews | 1 |
| Code-behind methods | 15 |

## 2. Purpose

Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

(INFERRED from file name, control layout and code-behind; no requirement document exists in the repository.)

## 3. Business Objective

**UNKNOWN (no requirement source in repository).** The implementation supports the following observable objective: Transaction-entry screen. It captures a header plus (where a grid is present) line items, validates them and persists them through the BLL/DAL layers, usually also moving stock and/or posting to accounting. Contains 1 grid(s).

## 4. Navigation

Menu placement is **data-driven** — the application builds its menu from `tblMainMenu` (`ManuName`, `URL`, `ParantId`) filtered by `tblMenuDistribution` for the logged-in user (`Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`). The menu rows are **not** in the repository, therefore the exact menu path for this page is **UNKNOWN**.

Reachability evidence found in code:

- referenced from `SInventory_UI/CreatePickingOnWareHouse.aspx`

Outbound `Response.Redirect` targets: `CreatePickingOnWareHouse.aspx`

## 5. Route

`/SInventory_UI/CreatePickingDetailForWareHouse.aspx` — physical ASP.NET Web Forms route. The solution has no routing table, no Areas, no MVC controllers and no Razor Pages.

Query-string parameters read in code-behind: `ReqId`

## 6. Source Files

| Layer | File |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CreatePickingDetailForWareHouse.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CreatePickingDetailForWareHouse.aspx.cs` |
| Master page | `Solution.Web/MasterPages/MainMasterPage.master(.cs)` |
| Business / data layer | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| Business / data layer | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |

## 7. UI Control Inventory

41 controls declared in markup. `Event` is the server-side handler wired in the `.aspx`.

| # | Control ID | Type | Screen label (nearest preceding text) | Text/Value | AutoPostBack | Enabled / ReadOnly | Visible | Event handler | Line |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `Content1` | asp:Content |  |  |  |  |  |  | 4 |
| 2 | `Content2` | asp:Content |  |  |  |  |  |  | 6 |
| 3 | `UpdatePanel1` | asp:UpdatePanel |  |  |  |  |  |  | 7 |
| 4 | `companyLabel` | asp:Label | Company Name: |  |  | ReadOnly=True |  |  | 41 |
| 5 | `companyIdHiddenField` | asp:HiddenField | Company Name: |  |  |  |  |  | 42 |
| 6 | `warehouseLabel` | asp:Label | Warehouse Name: |  |  | ReadOnly=True |  |  | 48 |
| 7 | `warehouseIdHiddenField` | asp:HiddenField | Warehouse Name: |  |  |  |  |  | 49 |
| 8 | `reqNoTextBox` | asp:Label | Req No: |  |  | ReadOnly=True |  |  | 64 |
| 9 | `reqDateTextBox` | asp:Label | Req Date: |  |  | ReadOnly=True |  |  | 70 |
| 10 | `truckNoTextBox` | asp:TextBox | Truck No: |  |  |  |  |  | 76 |
| 11 | `clnNoTextBox` | asp:Label | Picking No: |  |  | ReadOnly=True |  |  | 84 |
| 12 | `clnDateTextBox` | asp:Label | Picking Date: |  |  | ReadOnly=True |  |  | 90 |
| 13 | `driverNameTextBox` | asp:TextBox | Driver No |  |  |  |  |  | 96 |
| 14 | `issueGridView` | asp:GridView |  |  |  |  |  |  | 122 |
| 15 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 125 |
| 16 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 126 |
| 17 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 127 |
| 18 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 128 |
| 19 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 129 |
| 20 | `(no id)` | asp:BoundField |  |  |  |  |  |  | 130 |
| 21 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 131 |
| 22 | `issueQtyTextBox` | asp:TextBox | issueGridView → col[6] "Pick Qty" |  | True | ReadOnly=True |  | issueQtyTextBox_TextChanged | 133 |
| 23 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 137 |
| 24 | `priceTextBox` | asp:TextBox | issueGridView → col[7] "Price" |  |  | ReadOnly=True |  |  | 139 |
| 25 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 142 |
| 26 | `vatTextBox` | asp:TextBox | issueGridView → col[8] "VAT" |  |  | ReadOnly=True |  |  | 144 |
| 27 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 147 |
| 28 | `totalPriceTextBox` | asp:TextBox | issueGridView → col[9] "Total Value" |  |  | ReadOnly=True |  |  | 149 |
| 29 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 152 |
| 30 | `caseTextBox` | asp:TextBox | issueGridView → col[10] "Case" |  |  | ReadOnly=True |  |  | 154 |
| 31 | `(no id)` | asp:TemplateField |  |  |  |  |  |  | 157 |
| 32 | `chkSelectAll` | asp:CheckBox | issueGridView → col[11] "Pick" |  | True |  |  | chkSelectAll_CheckedChanged | 159 |
| 33 | `issueCheckBox` | asp:CheckBox | issueGridView → col[11] "Pick" |  | True |  |  | issueCheckBox_CheckedChanged | 162 |
| 34 | `totalAllPriceTextBox` | asp:TextBox | Total Amount : |  |  | ReadOnly=True |  |  | 201 |
| 35 | `LinkButton1` | asp:LinkButton |  |  |  |  |  | LinkButton1_Click | 215 |
| 36 | `vatAllPriceTextBox` | asp:TextBox | Total VAT : |  |  | ReadOnly=True |  |  | 221 |
| 37 | `grandTotalTextBox` | asp:TextBox | Grand Total Amount : |  |  | ReadOnly=True |  |  | 241 |
| 38 | `hdReqId` | asp:HiddenField |  |  |  |  |  |  | 273 |
| 39 | `bmitButton` | asp:Button |  | Submit |  |  |  | bmitButton_Click | 282 |
| 40 | `UpdateProgress1` | asp:UpdateProgress |  |  |  |  |  |  | 341 |
| 41 | `imgWait` | asp:Image |  | ~/images/progress-bar-opt.gif |  |  |  |  | 344 |

The **Screen label** column is the nearest human-readable text preceding the control in the markup (this application lays out forms in `<table>` rows as `<td>Label:</td><td>control</td>`). It is a heuristic, not an `AssociatedControlID` binding — no control on this page uses `<asp:Label AssociatedControlID>`.

### 7.1 Grid definitions

#### `issueGridView`

| Attribute | Value |
|---|---|
| datakeynames | `ReqChildId` |
| autogeneratecolumns | `False` |
| cssclass | `gridview` |

| Idx | Column type | Header | DataField | Bound expression(s) | Controls in template | SortExpression |
|---|---|---|---|---|---|---|
| 0 | BoundField | Code | ProductCode | — | — | — |
| 1 | BoundField | Product Name | ProductName | — | — | — |
| 2 | BoundField | Pack Size | PackSize | — | — | — |
| 3 | BoundField | Curr. Stock | CurrentStockQty | — | — | — |
| 4 | BoundField | Unit Price | ProductUnitPrice | — | — | — |
| 5 | BoundField | Req Qty | ReqQty | — | — | — |
| 6 | TemplateField | Pick Qty | — | `ReqQty` | `TextBox:issueQtyTextBox` | — |
| 7 | TemplateField | Price | — | — | `TextBox:priceTextBox` | — |
| 8 | TemplateField | VAT | — | — | `TextBox:vatTextBox` | — |
| 9 | TemplateField | Total Value | — | — | `TextBox:totalPriceTextBox` | — |
| 10 | TemplateField | Case | — | — | `TextBox:caseTextBox` | — |
| 11 | TemplateField | Pick | — | — | `CheckBox:chkSelectAll`, `CheckBox:issueCheckBox` | — |

_Column index matters:_ the code-behind of this application addresses grid cells positionally (`grid.Rows[i].Cells[n].FindControl("id")`). Inserting or reordering a column silently breaks the code-behind.


## 8. Field Mapping (UI control → DTO → BLL → DAL → SP/SQL → DB)

This solution has **no DTO/ViewModel binding layer**. Code-behind reads control values directly (`x.Text`, `x.SelectedValue`) and either assigns them to a DAO object in `Library.DAO` or concatenates them straight into a SQL string in the DAL. The chain below is therefore reconstructed per call, not per field.

| UI event handler | Call in code-behind | Resolved entry point | Full chain (transitive) | SQL ops | Tables touched | Stored procedures |
|---|---|---|---|---|---|---|
| `Page_Load` | `aRequisitionBll.GetRequisitionInfoTransferReqId(...)` | `RequisitionBLL.GetRequisitionInfoTransferReqId(string reqId)` <br/>`Library.BLL/SInventory_BLL/RequisitionBLL.cs` | `RequisitionBLL.GetRequisitionInfoTransferReqId` → `RequisitionDAL.GetRequisitionInfofromTransferDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblStockInTransfar` | — |
| `LoadMainInfo` | `aRequisitionBll.GetRequisitionInfoByReqId(...)` | `RequisitionBLL.GetRequisitionInfoByReqId(string reqId)` <br/>`Library.BLL/SInventory_BLL/RequisitionBLL.cs` | `RequisitionBLL.GetRequisitionInfoByReqId` → `RequisitionDAL.GetRequisitionInfoByReqIdDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblCompanyInfo`, `tblRequisition`, `tblWearhouse` | — |
| `LoadMainInfo` | `aRequisitionBll.PickingNo(...)` | `RequisitionBLL.PickingNo(string reqNo)` <br/>`Library.BLL/SInventory_BLL/RequisitionBLL.cs` | `RequisitionBLL.PickingNo` | — | — | — |
| `LoadGrid` | `aRequisitionBll.GetPickingDataForIssue(...)` | `RequisitionBLL.GetPickingDataForIssue(string reqId, string warehouseid)` <br/>`Library.BLL/SInventory_BLL/RequisitionBLL.cs` | `RequisitionBLL.GetPickingDataForIssue` → `RequisitionDAL.GetPickingDataForIssueDAL` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `View_CentralStoreCurrentStock`, `tblRequsitionChild`, `tblUnitPrice` | — |
| `PriceCalculation` | `aProductBll.ProductPriceDetailWithCaseBLL(...)` | `ProductBLL.ProductPriceDetailWithCaseBLL(string productCode)` <br/>`Library.BLL/SInventory_BLL/ProductBLL.cs` | `ProductBLL.ProductPriceDetailWithCaseBLL` → `ProductDAL.ProductPriceDetailWithCase` → `ClsCommonInternalDAL.DataContainerDataTable` | SELECT | `tblProduct`, `tblProductCase`, `tblUnitPrice` | — |
| `SaveData` | `aRequisitionBll.UpdateIssueInformationOnRequisitionChild(...)` | `RequisitionBLL.UpdateIssueInformationOnRequisitionChild(List<RequsitionChild> aRequsitionChildrenList)` <br/>`Library.BLL/SInventory_BLL/RequisitionBLL.cs` | `RequisitionBLL.UpdateIssueInformationOnRequisitionChild` → `ProductBLL.ProductPriceDetailWithCaseBLL` → `ProductDAL.ProductPriceDetailWithCase` → `ClsCommonInternalDAL.DataContainerDataTable` → `RequisitionDAL.UpdateIssueInformationOnRequisitionChildDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` → `RequisitionBLL.UpdateCentralStockAndStockInTransfar` → `ClsPrimaryKeyFind.PrimaryKeyMax` | INSERT, SELECT, UPDATE | `tblCentralStore`, `tblProduct`, `tblProductCase`, `tblRequsitionChild`, `tblStockInTransfar`, `tblUnitPrice` | `sp_DeliveryConfirmationFullPosting` |
| `SaveData` | `aRequisitionBll.UpdatePickingInformationOnRequisition(...)` | `RequisitionBLL.UpdatePickingInformationOnRequisition(Requesition aRequesition)` <br/>`Library.BLL/SInventory_BLL/RequisitionBLL.cs` | `RequisitionBLL.UpdatePickingInformationOnRequisition` → `RequisitionDAL.UpdatePickingInformationOnRequisitionDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` | UPDATE | `tblRequisition` | — |

Field-level column mapping (individual textbox → individual DB column) is **UNKNOWN for most fields**: the DAL builds SQL by string concatenation, so a reliable automated mapping is not possible. Where an entity class exists in `Library.DAO`, its property names match the column names used in the concatenated SQL (verified for `Invoice` → `tblInvoice`, `Product` → `tblProduct`).

## 9. Dropdown Mapping

_No dropdown, checkbox-list or radio-button-list controls on this page._

## 10. Button / Action Mapping

| Button ID | Type | Text | Client-side (`OnClientClick`) | Server handler | Handler size | Calls | SPs |
|---|---|---|---|---|---|---|---|
| `LinkButton1` | asp:LinkButton |  | — | `LinkButton1_Click` | 3 lines | — | — |
| `bmitButton` | asp:Button | Submit | return confirm('Are you sure you want Save ?'); | `bmitButton_Click` | 6 lines | — | — |

Per-button visibility/enabled conditions and permissions: **UNKNOWN unless shown in the `Visible`/`Enabled` column of section 7** — this application controls access at page level through `tblMenuDistribution`, not at button level, except on approval screens which use `ClsApprovalAction.LoadActionControlByUser` (see `specs/00-system/role-permission.md`).

## 11. Frontend Flow

Rendering model: **ASP.NET Web Forms postback**. There is no SPA, no client-side router and (on this page) no REST call unless listed below. State travels in `__VIEWSTATE`; partial rendering uses `UpdatePanel`.

- UpdatePanels: 1
- AJAX Control Toolkit extenders: none

Server-side notification pattern used by this codebase: `ScriptManager.RegisterStartupScript(..., "alert('message');", true)`, usually wrapped in a local `showMessageBox(string)` / `ShowMessageBox(string)` helper.

## 12. Backend Flow

| # | Method | Signature | Lines | Layer calls | Tables (inline SQL) | SPs | Session keys |
|---|---|---|---|---|---|---|---|
| 1 | `Page_Load` | `void Page_Load(object sender, EventArgs e)` | 23 | `aRequisitionBll.GetRequisitionInfoTransferReqId` | — | — | — |
| 2 | `LoadMainInfo` | `void LoadMainInfo(string reqId)` | 17 | `aRequisitionBll.GetRequisitionInfoByReqId`, `aRequisitionBll.PickingNo` | — | — | — |
| 3 | `LoadGrid` | `void LoadGrid(string reqId)` | 29 | `aRequisitionBll.GetPickingDataForIssue` | — | — | — |
| 4 | `chkSelectAll_CheckedChanged` | `void chkSelectAll_CheckedChanged(object sender, EventArgs e)` | 19 | — | — | — | — |
| 5 | `issueCheckBox_CheckedChanged` | `void issueCheckBox_CheckedChanged(object sender, EventArgs e)` | 18 | — | — | — | — |
| 6 | `issueQtyTextBox_TextChanged` | `void issueQtyTextBox_TextChanged(object sender, EventArgs e)` | 7 | — | — | — | — |
| 7 | `PriceCalculation` | `void PriceCalculation(int rowIndex)` | 41 | `aProductBll.ProductPriceDetailWithCaseBLL` | — | — | — |
| 8 | `GetCalculationValue` | `void GetCalculationValue()` | 23 | — | — | — | — |
| 9 | `SaveData` | `void SaveData()` | 58 | `aRequisitionBll.UpdateIssueInformationOnRequisitionChild`, `aRequisitionBll.UpdatePickingInformationOnRequisition` | — | — | — |
| 10 | `Clear` | `void Clear()` | 21 | — | — | — | — |
| 11 | `Validation` | `bool Validation()` | 17 | — | — | — | — |
| 12 | `bmitButton_Click` | `void bmitButton_Click(object sender, EventArgs e)` | 6 | — | — | — | — |
| 13 | `showMessageBox` | `void showMessageBox(string message)` | 6 | — | — | — | — |
| 14 | `LinkButton1_Click` | `void LinkButton1_Click(object sender, EventArgs e)` | 3 | — | — | — | — |
| 15 | `cancelButton_Click` | `void cancelButton_Click(object sender, EventArgs e)` | 3 | — | — | — | — |

Call chain shape for every persisting action on this page:

```
CreatePickingDetailForWareHouse.aspx (markup)
  -> SInventory_UI_CreatePickingDetailForWareHouse (code-behind event handler)
     -> Library.BLL.SInventory_BLL.*BLL          (thin pass-through + calculations)
        -> Library.DAL.SInventory_DAL.*DAL       (builds SQL string / SqlParameter list)
           -> Library.DAL.InternalCls.ClsCommonInternalDAL
              -> EXEC ExecuteAllSqlQueryByStoreProcedure @Query   (for inline SQL)
              -> EXEC <named stored procedure>                    (for parameterised calls)
                 -> SQL Server: SalesrollDB_ZAS
```

## 13. API Mapping

**None.** This page exposes and consumes no HTTP API; all interaction is full or partial (UpdatePanel) postback. See `specs/00-system/api-inventory.md` for the complete, application-wide API surface.

## 14. Service (BLL) Mapping

| Caller (code-behind) | BLL class | Method | Params | Return | SQL ops | Tables | SPs | File |
|---|---|---|---|---|---|---|---|---|
| `Page_Load` | `RequisitionBLL` | `GetRequisitionInfoTransferReqId` | `string reqId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| `LoadMainInfo` | `RequisitionBLL` | `GetRequisitionInfoByReqId` | `string reqId` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| `LoadMainInfo` | `RequisitionBLL` | `PickingNo` | `string reqNo` | `string` | — | — | — | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| `LoadGrid` | `RequisitionBLL` | `GetPickingDataForIssue` | `string reqId, string warehouseid` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| `PriceCalculation` | `ProductBLL` | `ProductPriceDetailWithCaseBLL` | `string productCode` | `DataTable` | — | — | — | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| `SaveData` | `RequisitionBLL` | `UpdateIssueInformationOnRequisitionChild` | `List<RequsitionChild> aRequsitionChildrenList` | `string` | — | — | — | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| `SaveData` | `RequisitionBLL` | `UpdatePickingInformationOnRequisition` | `Requesition aRequesition` | `bool` | — | — | — | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |

## 15. Repository (DAL) Mapping

_No DAL method resolved for this page._

Note: many pages in this application skip the BLL and call `Library.DAL` directly (the BLL project has 74 classes against 130 DAL classes).

## 16. Stored Procedure Mapping

| Stored procedure | Invoked from | Body available in repo? |
|---|---|---|
| `sp_DeliveryConfirmationFullPosting` | SaveData → `RequisitionBLL.UpdateIssueInformationOnRequisitionChild → ProductBLL.ProductPriceDetailWithCaseBLL → ProductDAL.ProductPriceDetailWithCase → ClsCommonInternalDAL.DataContainerDataTable → RequisitionDAL.UpdateIssueInformationOnRequisitionChildDAL → ClsCommonInternalDAL.UpdateDataByUpdateCommand` | **No** — see note |

**The repository contains no `.sql` files, no `database/` directory and no DDL.** Stored procedure bodies, parameters, transactions, TRY/CATCH blocks, temp tables, CTEs, dynamic SQL and result-set shapes are therefore **UNKNOWN** and cannot be documented from this repository. Only the *names* and the *parameters the C# passes* are recoverable. See `specs/00-system/stored-procedures.md` and `specs/SPEC-GAPS.md` (gap `GAP-DB-001`).

Every inline SQL statement in this application also reaches SQL Server through one generic stored procedure, `ExecuteAllSqlQueryByStoreProcedure(@Query nvarchar)`, defined in the database and invoked from `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`.

## 17. Database Mapping

| Table / view | Referenced by |
|---|---|
| `View_CentralStoreCurrentStock` | `LoadGrid()` → `RequisitionBLL.GetPickingDataForIssue` → `RequisitionDAL.GetPickingDataForIssueDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCentralStore` | `SaveData()` → `RequisitionBLL.UpdateIssueInformationOnRequisitionChild` → `ProductBLL.ProductPriceDetailWithCaseBLL` → `ProductDAL.ProductPriceDetailWithCase` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblCompanyInfo` | `LoadMainInfo()` → `RequisitionBLL.GetRequisitionInfoByReqId` → `RequisitionDAL.GetRequisitionInfoByReqIdDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProduct` | `PriceCalculation()` → `ProductBLL.ProductPriceDetailWithCaseBLL` → `ProductDAL.ProductPriceDetailWithCase` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveData()` → `RequisitionBLL.UpdateIssueInformationOnRequisitionChild` → `ProductBLL.ProductPriceDetailWithCaseBLL` → `ProductDAL.ProductPriceDetailWithCase` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblProductCase` | `PriceCalculation()` → `ProductBLL.ProductPriceDetailWithCaseBLL` → `ProductDAL.ProductPriceDetailWithCase` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveData()` → `RequisitionBLL.UpdateIssueInformationOnRequisitionChild` → `ProductBLL.ProductPriceDetailWithCaseBLL` → `ProductDAL.ProductPriceDetailWithCase` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblRequisition` | `LoadMainInfo()` → `RequisitionBLL.GetRequisitionInfoByReqId` → `RequisitionDAL.GetRequisitionInfoByReqIdDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveData()` → `RequisitionBLL.UpdatePickingInformationOnRequisition` → `RequisitionDAL.UpdatePickingInformationOnRequisitionDAL` → `ClsCommonInternalDAL.UpdateDataByUpdateCommand` |
| `tblRequsitionChild` | `LoadGrid()` → `RequisitionBLL.GetPickingDataForIssue` → `RequisitionDAL.GetPickingDataForIssueDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveData()` → `RequisitionBLL.UpdateIssueInformationOnRequisitionChild` → `ProductBLL.ProductPriceDetailWithCaseBLL` → `ProductDAL.ProductPriceDetailWithCase` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblStockInTransfar` | `Page_Load()` → `RequisitionBLL.GetRequisitionInfoTransferReqId` → `RequisitionDAL.GetRequisitionInfofromTransferDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveData()` → `RequisitionBLL.UpdateIssueInformationOnRequisitionChild` → `ProductBLL.ProductPriceDetailWithCaseBLL` → `ProductDAL.ProductPriceDetailWithCase` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblUnitPrice` | `LoadGrid()` → `RequisitionBLL.GetPickingDataForIssue` → `RequisitionDAL.GetPickingDataForIssueDAL` → `ClsCommonInternalDAL.DataContainerDataTable`, `PriceCalculation()` → `ProductBLL.ProductPriceDetailWithCaseBLL` → `ProductDAL.ProductPriceDetailWithCase` → `ClsCommonInternalDAL.DataContainerDataTable`, `SaveData()` → `RequisitionBLL.UpdateIssueInformationOnRequisitionChild` → `ProductBLL.ProductPriceDetailWithCaseBLL` → `ProductDAL.ProductPriceDetailWithCase` → `ClsCommonInternalDAL.DataContainerDataTable` |
| `tblWearhouse` | `LoadMainInfo()` → `RequisitionBLL.GetRequisitionInfoByReqId` → `RequisitionDAL.GetRequisitionInfoByReqIdDAL` → `ClsCommonInternalDAL.DataContainerDataTable` |

Database: `SalesrollDB_ZAS` (connection string `SolutionConnectionStringSSIDB`). Two further catalogs are referenced by three-part names elsewhere in the application: `ZAS_ACCDB` (`SolutionConnectionStringACCDB`) and `CSTL_ERPDB` (`SolutionConnectionStringERPDB`); `CSTL_ACCDB` and `ZASHRIS` also appear in SQL literals.

Column-level detail, data types, nullability, defaults, primary keys, foreign keys, indexes, constraints and triggers are **UNKNOWN** — no schema artefact exists in the repository.

## 18. Validation Matrix

| Source method | Message shown | Frontend | Backend | DB | Kind |
|---|---|---|---|---|---|
| `showMessageBox` | {0} | No | Yes | UNKNOWN | Notification |

Database-level validation is **UNKNOWN** (no constraints available in the repository).

## 19. Business Rules

| Rule ID | Description | Source | File / Method | Trigger | Expected result | Message | Confidence |
|---|---|---|---|---|---|---|---|
| `BR-DLV-006-001` | {0} | Code | `Solution.Web/SInventory_UI/CreatePickingDetailForWareHouse.aspx.cs` → `showMessageBox()` | user action on this page | operation is refused | "{0}" | CONFIRMED |

Rules enforced inside the stored procedures this page calls are **UNKNOWN** (SP bodies absent). Application-wide rules that also apply here are listed in `specs/00-system/business-rules.md`.

## 20. Permission Matrix

| Role | View | Create | Edit | Delete | Submit | Approve | Reject | Cancel | Export | Print |
|---|---|---|---|---|---|---|---|---|---|---|
| _any role_ | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN | UNKNOWN |

**Why UNKNOWN:** this application has no static role model. Access is a per-user, per-menu-row grant stored in `tblMenuDistribution (UserId, MenuSL, Status)`; `tblUser.UserType` exists but the only value the code branches on is `"Admin"`. Approval buttons are additionally gated per user and per page by `tblActionUserWiseApproval` + `tblActionPageWiseStep` + `tblActionSteps` + `tblActionStepDetail` (`Library.DAL/InternalCls/ClsApprovalAction.cs`). All of these are **data**, not code, so the effective matrix cannot be derived from the repository. See `specs/00-system/role-permission.md`.

Authentication check: performed by the master page (`MainMasterPage.master.cs` `Page_Load` redirects to `../Login.aspx` when `Session["UserId"]` is null). This page uses that master page, so the check applies.

## 21. Status Lifecycle

No status token appears in this page's own code.

Application-wide status vocabulary and the transitions that are actually implemented are documented in `specs/00-system/status-lifecycle.md`. Transitions executed inside stored procedures are **UNKNOWN**.

## 22. Error Handling

- Page level: no `try`/`catch` found in the code-behind — an exception surfaces as an ASP.NET yellow screen
- DAL level: `ClsCommonInternalDAL` re-throws (`throw ex;`, which resets the stack trace). `DB_Manager.ExecuteNonQueryAction` **swallows** the exception, rolls back the transaction, closes the connection and returns `0`, so the caller sees "no rows affected" rather than the error.
- Application level: `Global.asax` `Application_Error` is **empty** — there is no global error handler, no logging and no custom error page (`<customErrors>` is not configured in `Web.config`).
- User-visible failures are surfaced as `alert()` boxes injected with `ScriptManager.RegisterStartupScript`.

## 23. Audit

No audit column is written by this page (INFERRED — none appears in its code-behind).

Application-wide auditing: only `tblLoginLog (UserId, LoginName, LoginTime)`, written on successful login (`Library.DAL/PanalCls/PanalClsDAL.cs::LoginLog`). There is no logout log, no change log, no row-version history and no application logging framework wired up (log4net is referenced in `Web.config` `<assemblies>` but never configured or called).

## 24. Integration

- Accounting posting into `ZAS_ACCDB` via `sp_*Posting` / `sp_Sales_*` procedures.

See `specs/00-system/integrations.md` for the full list.

## 25. Reports

_This page opens no report._

## 26. Dependencies

| Kind | Dependency |
|---|---|
| Master page | `MasterPages/MainMasterPage.master` (auth guard + menu) |
| Session | `Session["UserId"]` must be set (login) |
| Assembly source | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| Assembly source | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| Database object | `View_CentralStoreCurrentStock` |
| Database object | `tblCentralStore` |
| Database object | `tblCompanyInfo` |
| Database object | `tblProduct` |
| Database object | `tblProductCase` |
| Database object | `tblRequisition` |
| Database object | `tblRequsitionChild` |
| Database object | `tblStockInTransfar` |
| Database object | `tblUnitPrice` |
| Database object | `tblWearhouse` |
| Stored procedure | `sp_DeliveryConfirmationFullPosting` |
| Runtime | .NET Framework 4.0, ASP.NET Web Forms, Enterprise Library Data Access Block, AJAX Control Toolkit |

## 27. Test Cases

| ID | Scenario | Steps | Expected | Confidence |
|---|---|---|---|---|
| `TC-DLV-006-001` | Unauthenticated access | Request `/SInventory_UI/CreatePickingDetailForWareHouse.aspx` with no session | Redirect to `/Login.aspx` (master-page guard) | CONFIRMED |
| `TC-DLV-006-002` | Validation: {0} | Trigger `showMessageBox()` in the state that produces this message | alert "{0}" and the operation is not performed | CONFIRMED |
| `TC-DLV-006-003` | Grid loads | Open the page with data present | Grid `issueGridView` binds and shows its columns | CONFIRMED |

Expected values that depend on stored-procedure behaviour are **UNKNOWN** until the SP bodies are supplied.

## 28. Reverse Engineering References

| Evidence | Location |
|---|---|
| Markup | `Solution.Web/SInventory_UI/CreatePickingDetailForWareHouse.aspx` |
| Code-behind | `Solution.Web/SInventory_UI/CreatePickingDetailForWareHouse.aspx.cs` |
| Layer source | `Library.BLL/SInventory_BLL/ProductBLL.cs` |
| Layer source | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` |
| Connection strings | `Solution.Web/Web.config` |
| Data-access plumbing | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`, `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` |
| Auth + menu | `Solution.Web/Login.aspx.cs`, `Solution.Web/MasterPages/MainMasterPage.master.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` |

## 29. Spec vs Actual

| Area | Prior spec | Code | Database | Status | Reference |
|---|---|---|---|---|---|
| Page existence | absent | present | n/a | **MISSING → now documented** | section 1 |
| UI controls | absent | 41 controls enumerated | n/a | **MISSING → now documented** | section 7 |
| Frontend behaviour | absent | postback model documented | n/a | **MISSING → now documented** | section 11 |
| Backend flow | absent | 15 methods traced | n/a | **MISSING → now documented** | section 12 |
| API | absent | none | n/a | MATCH (none expected) | section 13 |
| Stored procedures | absent | 1 names found | bodies **not in repo** | **PARTIAL** | section 16 |
| Tables | absent | 10 names found | DDL **not in repo** | **PARTIAL** | section 17 |
| Business rules | absent | messages extracted | rules inside SPs unknown | **PARTIAL** | section 19 |
| Permissions | absent | page-level grant only | grant rows **not in repo** | **UNKNOWN** | section 20 |
| Status lifecycle | absent | tokens extracted | transitions inside SPs unknown | **PARTIAL** | section 21 |

_"Prior spec" is `absent` for every row: the repository contained no `specs/`, `docs/`, `.claude/` or `database/` directory before this rebuild — verified at the repository root._

## 30. Known Gaps

- Stored-procedure bodies are not in the repository → business rules, transactions and result sets executed server-side are UNKNOWN (`GAP-DB-001`).
- No DDL → column names, types, keys, indexes, FKs, triggers UNKNOWN (`GAP-DB-002`).
- Menu path and permission grants live in `tblMainMenu`/`tblMenuDistribution` data → navigation and role matrix UNKNOWN (`GAP-SEC-001`).

## 31. Change Impact

Changing this page affects:

- `Library.BLL/SInventory_BLL/ProductBLL.cs` is **shared with 8 other page(s)** (e.g. `DcWiseAreaAlocation.aspx`, `DistributionCenterInformation.aspx`, `EmpGeneralInfoEntry.aspx`, `IssueRequisitionProducts.aspx`, `ProductEdit.aspx`) — a change there is not local.
- `Library.BLL/SInventory_BLL/RequisitionBLL.cs` is **shared with 29 other page(s)** (e.g. `Dashboard.aspx`, `B2BReceiveViewer.aspx`, `StockReceiveReportViewer.aspx`, `TransferStockReceiveReportViewer.aspx`, `AllSalesReport.aspx`) — a change there is not local.
- Writes/reads `View_CentralStoreCurrentStock` — any schema change to it breaks this page.
- Writes/reads `tblCentralStore` — any schema change to it breaks this page.
- Writes/reads `tblCompanyInfo` — any schema change to it breaks this page.
- Writes/reads `tblProduct` — any schema change to it breaks this page.
- Writes/reads `tblProductCase` — any schema change to it breaks this page.
- Writes/reads `tblRequisition` — any schema change to it breaks this page.
- Writes/reads `tblRequsitionChild` — any schema change to it breaks this page.
- Writes/reads `tblStockInTransfar` — any schema change to it breaks this page.
- Writes/reads `tblUnitPrice` — any schema change to it breaks this page.
- Writes/reads `tblWearhouse` — any schema change to it breaks this page.

Because the DAL builds SQL by string concatenation, a renamed column silently breaks at runtime, not at compile time. There are no automated tests in the repository.
