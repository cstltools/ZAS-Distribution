# Module: master-data

Reference data for everything else: company and company unit (sales/distribution centre), the geography chain `Region → Area → Territory → Market`, the sales-force hierarchy (`RSM → ASM → MIO → MIA`, plus slave/assistant records and targets), products with category, type, brand, pack size, case and UOM, unit prices and their approval, customers with category and credit limit, and employees.

Most entities follow the Entry / View / Edit triple described in [../00-system/conventions.md](../00-system/conventions.md), with soft delete via `IsActive` and an approval step via `ActionStatus`.

Two chains matter downstream and are documented as business rules:

- `BR-CUS-001` — a customer's owning sales centre is *derived* through `tblCustMaster → tblMarket → tblTerritory → tblArea → tblDcWiseAreaInfo → tblCompanyUnit`, all INNER JOINs. A break anywhere hides the customer from invoicing.
- `BR-PRC-001` — a price is only usable when `tblUnitPrice.ActionStatus = 'Accepted' AND IsActive = 'TRUE'`.

## Pages (75)

| Page ID | Page | Route | Kind | Controls | Grids | Methods | Master page | Spec |
|---|---|---|---|---|---|---|---|---|
| `MST-001` | `ASMInfo.aspx` | `/SInventory_UI/ASMInfo.aspx` | Entry / transaction | 37 | 1 | 17 | yes | [spec](pages/PAGE-MST-001-ASMInfo.md) |
| `MST-002` | `AreaInformation.aspx` | `/SInventory_UI/AreaInformation.aspx` | Entry / transaction | 15 | 0 | 12 | yes | [spec](pages/PAGE-MST-002-AreaInformation.md) |
| `MST-003` | `AreaInformationEdit.aspx` | `/SInventory_UI/AreaInformationEdit.aspx` | Edit | 18 | 0 | 10 | **no** | [spec](pages/PAGE-MST-003-AreaInformationEdit.md) |
| `MST-004` | `AreaInformationView.aspx` | `/SInventory_UI/AreaInformationView.aspx` | List / view | 18 | 1 | 6 | yes | [spec](pages/PAGE-MST-004-AreaInformationView.md) |
| `MST-005` | `CompanyInformation.aspx` | `/SInventory_UI/CompanyInformation.aspx` | Entry / transaction | 15 | 0 | 11 | yes | [spec](pages/PAGE-MST-005-CompanyInformation.md) |
| `MST-006` | `CompanyInformationEdit.aspx` | `/SInventory_UI/CompanyInformationEdit.aspx` | Edit | 15 | 0 | 9 | **no** | [spec](pages/PAGE-MST-006-CompanyInformationEdit.md) |
| `MST-007` | `CompanyInformationView.aspx` | `/SInventory_UI/CompanyInformationView.aspx` | List / view | 16 | 1 | 6 | yes | [spec](pages/PAGE-MST-007-CompanyInformationView.md) |
| `MST-008` | `CustCategoryEdit.aspx` | `/SInventory_UI/CustCategoryEdit.aspx` | Edit | 7 | 0 | 4 | **no** | [spec](pages/PAGE-MST-008-CustCategoryEdit.md) |
| `MST-009` | `CustCategoryView.aspx` | `/SInventory_UI/CustCategoryView.aspx` | List / view | 11 | 1 | 6 | yes | [spec](pages/PAGE-MST-009-CustCategoryView.md) |
| `MST-010` | `CustMasterEdit.aspx` | `/SInventory_UI/CustMasterEdit.aspx` | Edit | 29 | 0 | 12 | **no** | [spec](pages/PAGE-MST-010-CustMasterEdit.md) |
| `MST-011` | `CustMasterEntry.aspx` | `/SInventory_UI/CustMasterEntry.aspx` | Entry / transaction | 37 | 0 | 18 | yes | [spec](pages/PAGE-MST-011-CustMasterEntry.md) |
| `MST-012` | `CustMasterView.aspx` | `/SInventory_UI/CustMasterView.aspx` | List / view | 33 | 1 | 16 | yes | [spec](pages/PAGE-MST-012-CustMasterView.md) |
| `MST-013` | `CustomerCategoryEntry.aspx` | `/SInventory_UI/CustomerCategoryEntry.aspx` | Entry / transaction | 9 | 0 | 4 | yes | [spec](pages/PAGE-MST-013-CustomerCategoryEntry.md) |
| `MST-014` | `CustomerCreditLimitApproval.aspx` | `/SInventory_UI/CustomerCreditLimitApproval.aspx` | Approval | 16 | 1 | 7 | yes | [spec](pages/PAGE-MST-014-CustomerCreditLimitApproval.md) |
| `MST-015` | `CustomerCreditLimitEntry.aspx` | `/SInventory_UI/CustomerCreditLimitEntry.aspx` | Entry / transaction | 23 | 0 | 10 | yes | [spec](pages/PAGE-MST-015-CustomerCreditLimitEntry.md) |
| `MST-016` | `CustomerCreditView.aspx` | `/SInventory_UI/CustomerCreditView.aspx` | List / view | 28 | 1 | 10 | yes | [spec](pages/PAGE-MST-016-CustomerCreditView.md) |
| `MST-017` | `CustomerMasterEdit.aspx` | `/SInventory_UI/CustomerMasterEdit.aspx` | Edit | 26 | 0 | 12 | **no** | [spec](pages/PAGE-MST-017-CustomerMasterEdit.md) |
| `MST-018` | `DcWiseAreaAlocation.aspx` | `/SInventory_UI/DcWiseAreaAlocation.aspx` | Entry / transaction | 10 | 0 | 12 | yes | [spec](pages/PAGE-MST-018-DcWiseAreaAlocation.md) |
| `MST-019` | `DeliveryPersonEntry.aspx` | `/SInventory_UI/DeliveryPersonEntry.aspx` | Entry / transaction | 22 | 0 | 15 | yes | [spec](pages/PAGE-MST-019-DeliveryPersonEntry.md) |
| `MST-020` | `DeliveryPersonView.aspx` | `/SInventory_UI/DeliveryPersonView.aspx` | List / view | 15 | 1 | 5 | yes | [spec](pages/PAGE-MST-020-DeliveryPersonView.md) |
| `MST-021` | `DepartmentEdit.aspx` | `/SInventory_UI/DepartmentEdit.aspx` | Edit | 8 | 0 | 7 | **no** | [spec](pages/PAGE-MST-021-DepartmentEdit.md) |
| `MST-022` | `DepartmentEntry.aspx` | `/SInventory_UI/DepartmentEntry.aspx` | Entry / transaction | 10 | 0 | 8 | yes | [spec](pages/PAGE-MST-022-DepartmentEntry.md) |
| `MST-023` | `DepartmentView.aspx` | `/SInventory_UI/DepartmentView.aspx` | List / view | 11 | 1 | 6 | yes | [spec](pages/PAGE-MST-023-DepartmentView.md) |
| `MST-024` | `DesignationInfo.aspx` | `/SInventory_UI/DesignationInfo.aspx` | Entry / transaction | 12 | 0 | 10 | yes | [spec](pages/PAGE-MST-024-DesignationInfo.md) |
| `MST-025` | `DesignationView.aspx` | `/SInventory_UI/DesignationView.aspx` | List / view | 11 | 1 | 5 | yes | [spec](pages/PAGE-MST-025-DesignationView.md) |
| `MST-026` | `DistributionCenterInfoEdit.aspx` | `/SInventory_UI/DistributionCenterInfoEdit.aspx` | Edit | 20 | 0 | 9 | **no** | [spec](pages/PAGE-MST-026-DistributionCenterInfoEdit.md) |
| `MST-027` | `DistributionCenterInfoView.aspx` | `/SInventory_UI/DistributionCenterInfoView.aspx` | List / view | 18 | 1 | 6 | yes | [spec](pages/PAGE-MST-027-DistributionCenterInfoView.md) |
| `MST-028` | `DistributionCenterInformation.aspx` | `/SInventory_UI/DistributionCenterInformation.aspx` | Entry / transaction | 20 | 0 | 12 | yes | [spec](pages/PAGE-MST-028-DistributionCenterInformation.md) |
| `MST-029` | `EmpGenInfView.aspx` | `/SInventory_UI/EmpGenInfView.aspx` | List / view | 18 | 1 | 6 | yes | [spec](pages/PAGE-MST-029-EmpGenInfView.md) |
| `MST-030` | `EmpGeneralInfoEntry.aspx` | `/SInventory_UI/EmpGeneralInfoEntry.aspx` | Entry / transaction | 63 | 0 | 20 | yes | [spec](pages/PAGE-MST-030-EmpGeneralInfoEntry.md) |
| `MST-031` | `MIOSetup.aspx` | `/SInventory_UI/MIOSetup.aspx` | Entry / transaction | 37 | 1 | 18 | yes | [spec](pages/PAGE-MST-031-MIOSetup.md) |
| `MST-032` | `MIOTargetEntryByProduct.aspx` | `/SInventory_UI/MIOTargetEntryByProduct.aspx` | Entry / transaction | 41 | 1 | 19 | yes | [spec](pages/PAGE-MST-032-MIOTargetEntryByProduct.md) |
| `MST-033` | `ManufacturerEdit.aspx` | `/SInventory_UI/ManufacturerEdit.aspx` | Edit | 9 | 0 | 8 | **no** | [spec](pages/PAGE-MST-033-ManufacturerEdit.md) |
| `MST-034` | `ManufacturerEntry.aspx` | `/SInventory_UI/ManufacturerEntry.aspx` | Entry / transaction | 10 | 0 | 4 | yes | [spec](pages/PAGE-MST-034-ManufacturerEntry.md) |
| `MST-035` | `ManufacturerView.aspx` | `/SInventory_UI/ManufacturerView.aspx` | List / view | 10 | 1 | 6 | yes | [spec](pages/PAGE-MST-035-ManufacturerView.md) |
| `MST-036` | `MarketInfoEdit.aspx` | `/SInventory_UI/MarketInfoEdit.aspx` | Edit | 18 | 0 | 12 | **no** | [spec](pages/PAGE-MST-036-MarketInfoEdit.md) |
| `MST-037` | `MarketInfoEntry.aspx` | `/SInventory_UI/MarketInfoEntry.aspx` | Entry / transaction | 18 | 0 | 14 | yes | [spec](pages/PAGE-MST-037-MarketInfoEntry.md) |
| `MST-038` | `MarketInfoView.aspx` | `/SInventory_UI/MarketInfoView.aspx` | List / view | 22 | 1 | 6 | yes | [spec](pages/PAGE-MST-038-MarketInfoView.md) |
| `MST-039` | `MiaTargetEdit.aspx` | `/SInventory_UI/MiaTargetEdit.aspx` | Edit | 24 | 0 | 6 | **no** | [spec](pages/PAGE-MST-039-MiaTargetEdit.md) |
| `MST-040` | `MiaTargetEntry.aspx` | `/SInventory_UI/MiaTargetEntry.aspx` | Entry / transaction | 27 | 0 | 9 | yes | [spec](pages/PAGE-MST-040-MiaTargetEntry.md) |
| `MST-041` | `MiaTargetView.aspx` | `/SInventory_UI/MiaTargetView.aspx` | List / view | 16 | 1 | 6 | yes | [spec](pages/PAGE-MST-041-MiaTargetView.md) |
| `MST-042` | `PackSizeEdit.aspx` | `/SInventory_UI/PackSizeEdit.aspx` | Edit | 7 | 0 | 5 | **no** | [spec](pages/PAGE-MST-042-PackSizeEdit.md) |
| `MST-043` | `PackSizeEntry.aspx` | `/SInventory_UI/PackSizeEntry.aspx` | Entry / transaction | 9 | 0 | 4 | yes | [spec](pages/PAGE-MST-043-PackSizeEntry.md) |
| `MST-044` | `PackSizeView.aspx` | `/SInventory_UI/PackSizeView.aspx` | List / view | 11 | 1 | 6 | yes | [spec](pages/PAGE-MST-044-PackSizeView.md) |
| `MST-045` | `ProCategoryEdit.aspx` | `/SInventory_UI/ProCategoryEdit.aspx` | Edit | 7 | 0 | 5 | **no** | [spec](pages/PAGE-MST-045-ProCategoryEdit.md) |
| `MST-046` | `ProCategoryEntry.aspx` | `/SInventory_UI/ProCategoryEntry.aspx` | Entry / transaction | 7 | 0 | 4 | yes | [spec](pages/PAGE-MST-046-ProCategoryEntry.md) |
| `MST-047` | `ProCategoryView.aspx` | `/SInventory_UI/ProCategoryView.aspx` | List / view | 9 | 1 | 6 | yes | [spec](pages/PAGE-MST-047-ProCategoryView.md) |
| `MST-048` | `ProTypeEdit.aspx` | `/SInventory_UI/ProTypeEdit.aspx` | Edit | 7 | 0 | 5 | **no** | [spec](pages/PAGE-MST-048-ProTypeEdit.md) |
| `MST-049` | `ProTypeEntry.aspx` | `/SInventory_UI/ProTypeEntry.aspx` | Entry / transaction | 7 | 0 | 4 | yes | [spec](pages/PAGE-MST-049-ProTypeEntry.md) |
| `MST-050` | `ProTypeView.aspx` | `/SInventory_UI/ProTypeView.aspx` | List / view | 9 | 1 | 6 | yes | [spec](pages/PAGE-MST-050-ProTypeView.md) |
| `MST-051` | `ProUnitPriceApprove.aspx` | `/SInventory_UI/ProUnitPriceApprove.aspx` | Approval | 19 | 1 | 8 | yes | [spec](pages/PAGE-MST-051-ProUnitPriceApprove.md) |
| `MST-052` | `ProUnitPriceEdit.aspx` | `/SInventory_UI/ProUnitPriceEdit.aspx` | Edit | 18 | 0 | 6 | **no** | [spec](pages/PAGE-MST-052-ProUnitPriceEdit.md) |
| `MST-053` | `ProUnitPriceEntry.aspx` | `/SInventory_UI/ProUnitPriceEntry.aspx` | Entry / transaction | 45 | 0 | 12 | yes | [spec](pages/PAGE-MST-053-ProUnitPriceEntry.md) |
| `MST-054` | `ProUnitPriceView.aspx` | `/SInventory_UI/ProUnitPriceView.aspx` | List / view | 21 | 1 | 10 | yes | [spec](pages/PAGE-MST-054-ProUnitPriceView.md) |
| `MST-055` | `ProductEdit.aspx` | `/SInventory_UI/ProductEdit.aspx` | Edit | 23 | 0 | 13 | **no** | [spec](pages/PAGE-MST-055-ProductEdit.md) |
| `MST-056` | `ProductEntry.aspx` | `/SInventory_UI/ProductEntry.aspx` | Entry / transaction | 24 | 0 | 13 | yes | [spec](pages/PAGE-MST-056-ProductEntry.md) |
| `MST-057` | `ProductSQEdit.aspx` | `/SInventory_UI/ProductSQEdit.aspx` | Edit | 8 | 0 | 6 | **no** | [spec](pages/PAGE-MST-057-ProductSQEdit.md) |
| `MST-058` | `ProductSQEntry.aspx` | `/SInventory_UI/ProductSQEntry.aspx` | Entry / transaction | 8 | 0 | 5 | yes | [spec](pages/PAGE-MST-058-ProductSQEntry.md) |
| `MST-059` | `ProductSQView.aspx` | `/SInventory_UI/ProductSQView.aspx` | List / view | 9 | 1 | 6 | yes | [spec](pages/PAGE-MST-059-ProductSQView.md) |
| `MST-060` | `ProductView.aspx` | `/SInventory_UI/ProductView.aspx` | List / view | 19 | 1 | 6 | yes | [spec](pages/PAGE-MST-060-ProductView.md) |
| `MST-061` | `RSMSetup.aspx` | `/SInventory_UI/RSMSetup.aspx` | Entry / transaction | 35 | 1 | 17 | yes | [spec](pages/PAGE-MST-061-RSMSetup.md) |
| `MST-062` | `RegionInformation.aspx` | `/SInventory_UI/RegionInformation.aspx` | Entry / transaction | 14 | 0 | 11 | yes | [spec](pages/PAGE-MST-062-RegionInformation.md) |
| `MST-063` | `RegionInformationEdit.aspx` | `/SInventory_UI/RegionInformationEdit.aspx` | Edit | 15 | 0 | 9 | **no** | [spec](pages/PAGE-MST-063-RegionInformationEdit.md) |
| `MST-064` | `RegionInformationViw.aspx` | `/SInventory_UI/RegionInformationViw.aspx` | List / view | 16 | 1 | 6 | yes | [spec](pages/PAGE-MST-064-RegionInformationViw.md) |
| `MST-065` | `ShippingCartonSizeEdit.aspx` | `/SInventory_UI/ShippingCartonSizeEdit.aspx` | Edit | 7 | 0 | 5 | **no** | [spec](pages/PAGE-MST-065-ShippingCartonSizeEdit.md) |
| `MST-066` | `ShippingCartonSizeEntry.aspx` | `/SInventory_UI/ShippingCartonSizeEntry.aspx` | Entry / transaction | 9 | 0 | 5 | yes | [spec](pages/PAGE-MST-066-ShippingCartonSizeEntry.md) |
| `MST-067` | `ShippingCartonSizeView.aspx` | `/SInventory_UI/ShippingCartonSizeView.aspx` | List / view | 10 | 1 | 6 | yes | [spec](pages/PAGE-MST-067-ShippingCartonSizeView.md) |
| `MST-068` | `SlaveInfoApproval.aspx` | `/SInventory_UI/SlaveInfoApproval.aspx` | Approval | 15 | 1 | 7 | yes | [spec](pages/PAGE-MST-068-SlaveInfoApproval.md) |
| `MST-069` | `SlaveInformation.aspx` | `/SInventory_UI/SlaveInformation.aspx` | Entry / transaction | 32 | 0 | 22 | yes | [spec](pages/PAGE-MST-069-SlaveInformation.md) |
| `MST-070` | `SlaveInformationView.aspx` | `/SInventory_UI/SlaveInformationView.aspx` | List / view | 17 | 1 | 5 | yes | [spec](pages/PAGE-MST-070-SlaveInformationView.md) |
| `MST-071` | `SupplierInfoView.aspx` | `/SInventory_UI/SupplierInfoView.aspx` | List / view | 10 | 1 | 5 | yes | [spec](pages/PAGE-MST-071-SupplierInfoView.md) |
| `MST-072` | `SupplierInformation.aspx` | `/SInventory_UI/SupplierInformation.aspx` | Entry / transaction | 12 | 0 | 9 | yes | [spec](pages/PAGE-MST-072-SupplierInformation.md) |
| `MST-073` | `TerritoryInformation.aspx` | `/SInventory_UI/TerritoryInformation.aspx` | Entry / transaction | 16 | 0 | 13 | yes | [spec](pages/PAGE-MST-073-TerritoryInformation.md) |
| `MST-074` | `TerritoryInformationEdit.aspx` | `/SInventory_UI/TerritoryInformationEdit.aspx` | Edit | 17 | 0 | 11 | **no** | [spec](pages/PAGE-MST-074-TerritoryInformationEdit.md) |
| `MST-075` | `TerritoryInformationView.aspx` | `/SInventory_UI/TerritoryInformationView.aspx` | List / view | 20 | 1 | 6 | yes | [spec](pages/PAGE-MST-075-TerritoryInformationView.md) |

## Database objects this module touches

Aggregated from every page above by following code-behind → BLL → DAL.

| Table / view | Call sites in this module |
|---|---|
| `tblAction` | 1 |
| `tblActionPageWiseStep` | 2 |
| `tblActionStepDetail` | 2 |
| `tblActionSteps` | 2 |
| `tblActionUserWiseApproval` | 2 |
| `tblArea` | 20 |
| `tblCompanyInfo` | 32 |
| `tblCompanyUnit` | 8 |
| `tblCustCategory` | 8 |
| `tblCustMaster` | 12 |
| `tblCustomerCreditLimit` | 8 |
| `tblCustomerFixedDiscount` | 1 |
| `tblDcWiseAreaInfo` | 8 |
| `tblDeliveryManInfo` | 5 |
| `tblDepartment` | 12 |
| `tblDesignation` | 5 |
| `tblDistributionType` | 1 |
| `tblDistrict` | 2 |
| `tblEmpGeneralInfo` | 14 |
| `tblGroupInformation` | 5 |
| `tblIngridents` | 3 |
| `tblInvoice` | 3 |
| `tblMIAInfo` | 2 |
| `tblMIATarget` | 4 |
| `tblMIATargetProductWise` | 3 |
| `tblMIOInfo` | 5 |
| `tblMainMenu` | 2 |
| `tblManufacturer` | 6 |
| `tblMarket` | 19 |
| `tblOrder` | 2 |
| `tblPackSize` | 10 |
| `tblProCategory` | 9 |
| `tblProType` | 7 |
| `tblProduct` | 16 |
| `tblProductCase` | 7 |
| `tblProductSQ` | 7 |
| `tblRegion` | 26 |
| `tblStockUOM` | 3 |
| `tblSupplierInformation` | 3 |
| `tblTerritory` | 17 |
| `tblUnitPrice` | 10 |
| `tblUnitPriceUpdate` | 1 |
| `tblUserCompanyUnit` | 1 |
| `tbl_MC_ProductRawMaterialDetails` | 3 |

## Stored procedures this module calls

| Procedure | Call sites in this module |
|---|---|
| `SP_DataMigration_Item` | 1 |
| `sp_I_CustomerGl` | 2 |
| `sp_I_ProductGl` | 1 |
| `sp_I_SupplierGl` | 1 |
| `sp_Integration_CustomerSubsidaryInsert` | 2 |

## Cross-cutting references

- [../00-system/architecture.md](../00-system/architecture.md) — the call chain every page here follows
- [../00-system/conventions.md](../00-system/conventions.md) — the grid, dropdown, validation and message patterns
- [../00-system/business-rules.md](../00-system/business-rules.md) — application-wide rules
- [../00-system/status-lifecycle.md](../00-system/status-lifecycle.md) — status values and transitions
- [../SPEC-GAPS.md](../SPEC-GAPS.md) — what is still unknown
