# Code Inventory — assemblies, classes and methods

Phase-1 inventory of everything that is not a page: services, repositories, entities, helpers and infrastructure. Counts are from parsing the C# source.

## Assemblies

| Project | Kind | Files | Classes | Methods | Role |
|---|---|---|---|---|---|
| `Library.BLL` | class library | 77 | 76 | 923 | Business logic. Mostly 1-line pass-through to the DAL; real logic in a handful of classes. |
| `Library.DAL` | class library | 145 | 144 | 1554 | Data access. Builds SQL strings or SqlParameter lists; the only layer that talks to SQL Server. |
| `Library.DAO` | class library | 99 | 119 | 0 (auto-properties only) | Entities. Plain property bags — no attributes, no validation, no ORM mapping. |
| `Library.CrystalReports` | class library | 42 `.rpt` + 114 typed DataSets (`.xsd`) | — | — | Report definitions and their schemas. |
| `Solution.Web` | ASP.NET **Web Site** (not Web Application) | 383 `.aspx` + code-behind | — | 3,659 | Presentation. Also holds most of the orchestration and calculation logic. |

## Infrastructure classes (`Library.DAL`)

These five classes are the plumbing every other class routes through. Read [architecture.md](architecture.md) for how they fit together.

| Class | File | Responsibility |
|---|---|---|
| `ClsCommonInternalDAL` | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs` | The main data gateway. Executes inline SQL by passing it as `@Query` to `ExecuteAllSqlQueryByStoreProcedure`, and executes named procedures with `SqlParameter` lists. **No transaction.** |
| `DB_Manager` | `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` | Alternative gateway that *does* open a transaction on connect and commit on close. Swallows exceptions and returns `0`. |
| `DB_Connection` / `DB_Authentication` / `DB_Names` | `Library.DAL/MAIN_FUNCTION/` | Build the connection string. **Server, `sa` user and password are hard-coded here** (`GAP-SEC-003`). |
| `ClsPrimaryKeyFind` | `Library.DAL/InternalCls/ClsPrimaryKeyFind.cs` | Generates keys as `MAX(id)+1` or `IDENT_CURRENT(table)` (`BR-SYS-003`). |
| `ClsApprovalAction` | `Library.DAL/InternalCls/ClsApprovalAction.cs` | The whole approval-permission engine — see [workflows/WF-approval.md](workflows/WF-approval.md). |
| `clsNum2Word` | `Library.DAL/InternalCls/clsNum2Word.cs` | Amount-in-words for printed documents. |

**There is no middleware, no HTTP module, no filter, no interceptor, no background job, no scheduler and no DI container.** `Global.asax` declares the five standard events and all five bodies are empty.

## Business layer (BLL) classes (77 files)

The BLL is thin. A class here typically wraps one DAL class one-to-one. `Methods` counts public and private members parsed from the file; `Used by` counts page code-behinds that call it by a resolvable method name.

| Class | File | Methods | Tables reached | SPs called | Used by (pages) |
|---|---|---|---|---|---|
| `PanalBLL` | `Library.BLL/Panal_BLL/PanalBLL.cs` | 23 | — | — | 5 |
| — | `Library.BLL/Properties/AssemblyInfo.cs` | 0 | — | — | 0 |
| `ASMInfoBll` | `Library.BLL/SInventory_BLL/ASMInfoBll.cs` | 12 | — | — | 0 |
| `AreaBLL` | `Library.BLL/SInventory_BLL/AreaBLL.cs` | 12 | — | — | 0 |
| `AreaWiseMonthlyInventoryReportBll` | `Library.BLL/SInventory_BLL/AreaWiseMonthlyInventoryReportBll.cs` | 2 | — | — | 0 |
| `AuditReportOneBll` | `Library.BLL/SInventory_BLL/AuditReportOneBll.cs` | 4 | — | — | 0 |
| `BatchWiseTopSheetBll` | `Library.BLL/SInventory_BLL/BatchWiseTopSheetBll.cs` | 7 | — | — | 0 |
| `CampaignBLL` | `Library.BLL/SInventory_BLL/CampaignBLL.cs` | 2 | — | — | 1 |
| `CentralStoreBLL` | `Library.BLL/SInventory_BLL/CentralStoreBLL.cs` | 7 | — | — | 1 |
| `ChalanBLL` | `Library.BLL/SInventory_BLL/ChalanBLL.cs` | 23 | — | — | 7 |
| `ComUnitSalesReportBLL` | `Library.BLL/SInventory_BLL/ComUnitSalesReportBLL.cs` | 4 | — | — | 0 |
| `CompanyInfoBLL` | `Library.BLL/SInventory_BLL/CompanyInfoBLL.cs` | 8 | — | — | 4 |
| `CompanySalesReportBLL` | `Library.BLL/SInventory_BLL/CompanySalesReportBLL.cs` | 11 | — | — | 5 |
| `CustPaymentBLL` | `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | 33 | — | — | 9 |
| `CustomerCategoryBLL` | `Library.BLL/SInventory_BLL/CustomerCategoryBLL.cs` | 5 | — | — | 1 |
| `CustomerCoverageReportBll` | `Library.BLL/SInventory_BLL/CustomerCoverageReportBll.cs` | 2 | — | — | 0 |
| `CustomerLedgerBLL` | `Library.BLL/SInventory_BLL/CustomerLedgerBLL.cs` | 1 | — | — | 0 |
| `CustomerMasterBLL` | `Library.BLL/SInventory_BLL/CustomerMasterBLL.cs` | 38 | — | — | 8 |
| `CustomerMasterInfoBll` | `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs` | 35 | — | — | 0 |
| `CustomerReturnBLL` | `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` | 11 | — | — | 3 |
| `DCStockReportBLL` | `Library.BLL/SInventory_BLL/DCStockReportBLL.cs` | 7 | — | — | 4 |
| `DCStoreBLL` | `Library.BLL/SInventory_BLL/DCStoreBLL.cs` | 8 | — | — | 0 |
| `DCUserPermissionBll` | `Library.BLL/SInventory_BLL/DCUserPermissionBll.cs` | 4 | — | — | 2 |
| `DcWiseAreaAloBll` | `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs` | 8 | — | — | 0 |
| `DeleteInvoiceReportBLL` | `Library.BLL/SInventory_BLL/DeleteInvoiceReportBLL.cs` | 4 | — | — | 0 |
| `DeliveryPersonBLL` | `Library.BLL/SInventory_BLL/DeliveryPersonBLL.cs` | 10 | — | — | 2 |
| `DepartmentBLL` | `Library.BLL/SInventory_BLL/DepartmentBLL.cs` | 6 | — | — | 3 |
| `DesignationBll` | `Library.BLL/SInventory_BLL/DesignationBll.cs` | 6 | — | — | 2 |
| `DistributionCeneterInfoBll` | `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs` | 12 | — | — | 0 |
| `DistributionCenterStockMonitoringBll` | `Library.BLL/SInventory_BLL/DistributionCenterStockMonitoringBll.cs` | 2 | — | — | 0 |
| `EmpGeneralInfoBLL` | `Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs` | 10 | — | — | 0 |
| `EmployeeInformationBLL` | `Library.BLL/SInventory_BLL/EmployeeInformationBLL.cs` | 4 | — | — | 2 |
| `ExcelUpForMIGOBLL` | `Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs` | 27 | — | — | 0 |
| `InvoiceBLL` | `Library.BLL/SInventory_BLL/InvoiceBLL.cs` | 97 | — | — | 27 |
| `InvoiceBatchBLL` | `Library.BLL/SInventory_BLL/InvoiceBatchBLL.cs` | 10 | — | — | 10 |
| `MIOSetupBll` | `Library.BLL/SInventory_BLL/MIOSetupBll.cs` | 14 | — | — | 0 |
| `ManufacturerBLL` | `Library.BLL/SInventory_BLL/ManufacturerBLL.cs` | 6 | — | — | 4 |
| `MarketInfoBLL` | `Library.BLL/SInventory_BLL/MarketInfoBLL.cs` | 18 | — | — | 3 |
| `MiaSalesReportBLL` | `Library.BLL/SInventory_BLL/MiaSalesReportBLL.cs` | 5 | — | — | 2 |
| `MiaTargetBLL` | `Library.BLL/SInventory_BLL/MiaTargetBLL.cs` | 5 | — | — | 3 |
| `OrderInfoBLL` | `Library.BLL/SInventory_BLL/OrderInfoBLL.cs` | 42 | — | — | 32 |
| `OrderListBLL` | `Library.BLL/SInventory_BLL/OrderListBLL.cs` | 33 | — | — | 3 |
| `OrderStatusBll` | `Library.BLL/SInventory_BLL/OrderStatusBll.cs` | 6 | — | — | 1 |
| `OtherStockActionBLL` | `Library.BLL/SInventory_BLL/OtherStockActionBLL.cs` | 15 | — | — | 19 |
| `PackSizeBLL` | `Library.BLL/SInventory_BLL/PackSizeBLL.cs` | 4 | — | — | 3 |
| `PettyCashBLL` | `Library.BLL/SInventory_BLL/PettyCashBLL.cs` | 21 | — | — | 0 |
| `ProTypeBLL` | `Library.BLL/SInventory_BLL/ProTypeBLL.cs` | 4 | — | — | 3 |
| `ProductBLL` | `Library.BLL/SInventory_BLL/ProductBLL.cs` | 23 | — | — | 5 |
| `ProductCategoriesBLL` | `Library.BLL/SInventory_BLL/ProductCategoriesBLL.cs` | 5 | — | — | 2 |
| `ProductDiscountBLL` | `Library.BLL/SInventory_BLL/ProductDiscountBLL.cs` | 33 | — | — | 6 |
| `ProductSQBLL` | `Library.BLL/SInventory_BLL/ProductSQBLL.cs` | 5 | — | — | 3 |
| `ProformaOrInvoiceReturnBLL` | `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs` | 10 | — | — | 0 |
| `RSMSetupBll` | `Library.BLL/SInventory_BLL/RSMSetupBll.cs` | 10 | — | — | 0 |
| `RegionInfoBLL` | `Library.BLL/SInventory_BLL/RegionInfoBLL.cs` | 9 | — | — | 3 |
| `RequisitionBLL` | `Library.BLL/SInventory_BLL/RequisitionBLL.cs` | 44 | — | — | 24 |
| `RollBackBLL` | `Library.BLL/SInventory_BLL/RollBackBLL.cs` | 8 | — | — | 1 |
| `SalesAdministrationReportBll` | `Library.BLL/SInventory_BLL/SalesAdministrationReportBll.cs` | 4 | — | — | 0 |
| `SalesHubBLL` | `Library.BLL/SInventory_BLL/SalesHubBLL.cs` | 7 | — | — | 2 |
| `ShippingCartonSizeBLL` | `Library.BLL/SInventory_BLL/ShippingCartonSizeBLL.cs` | 5 | — | — | 3 |
| `SlaveinformationBll` | `Library.BLL/SInventory_BLL/SlaveinformationBll.cs` | 6 | — | — | 0 |
| `StockConditionFreezeBLL` | `Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs` | 16 | — | — | 3 |
| `StockConditionPermissionBll` | `Library.BLL/SInventory_BLL/StockConditionPermissionBll.cs` | 5 | — | — | 0 |
| `StockTransportOrderReportBLL` | `Library.BLL/SInventory_BLL/StockTransportOrderReportBLL.cs` | 3 | — | — | 0 |
| `StockUOMBLL` | `Library.BLL/SInventory_BLL/StockUOMBLL.cs` | 4 | — | — | 0 |
| `TerritoryInfoBll` | `Library.BLL/SInventory_BLL/TerritoryInfoBll.cs` | 12 | — | — | 0 |
| `TotalSummaryBLL` | `Library.BLL/SInventory_BLL/TotalSummaryBLL.cs` | 1 | — | — | 0 |
| `UnitPriceBLL` | `Library.BLL/SInventory_BLL/UnitPriceBLL.cs` | 17 | — | — | 6 |
| `UserBLL` | `Library.BLL/SInventory_BLL/UserBLL.cs` | 8 | — | — | 3 |
| `WHStockInReportBll` | `Library.BLL/SInventory_BLL/WHStockInReportBll.cs` | 6 | — | — | 0 |
| `WHStockListBLL` | `Library.BLL/SInventory_BLL/WHStockListBLL.cs` | 7 | — | — | 2 |
| `WarehousePickingBLL` | `Library.BLL/SInventory_BLL/WarehousePickingBLL.cs` | 1 | — | — | 1 |
| `WarehouseStockInApprovalBll` | `Library.BLL/SInventory_BLL/WarehouseStockInApprovalBll.cs` | 6 | — | — | 0 |
| `WarehouseStockInBll` | `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs` | 17 | — | — | 7 |
| `WhFreezeStockReleaseBll` | `Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs` | 8 | — | — | 0 |
| `WhStockConditionFreezeBll` | `Library.BLL/SInventory_BLL/WhStockConditionFreezeBll.cs` | 9 | — | — | 0 |
| `WhStockMonitoringReportBll` | `Library.BLL/SInventory_BLL/WhStockMonitoringReportBll.cs` | 2 | — | — | 0 |
| `WingsSalesAndCollectionReportBll` | `Library.BLL/SInventory_BLL/WingsSalesAndCollectionReportBll.cs` | 4 | — | — | 0 |

## Data layer (DAL) classes (145 files)

Note there are more DAL classes than BLL classes — many pages call the DAL directly.

| Class | File | Methods | Tables reached | SPs called | Used by (pages) |
|---|---|---|---|---|---|
| `ClsApprovalAction` | `Library.DAL/InternalCls/ClsApprovalAction.cs` | 2 | `tblAction`, `tblActionPageWiseStep`, `tblActionStepDetail`, `tblActionSteps`, `tblActionUserWiseApproval`, `tblMainMenu` | — | 0 |
| `ClsCommonInternalDAL` | `Library.DAL/InternalCls/ClsCommonInternalDAL.cs` | 33 | `tblAction` | `sp_`, `usp_` | 0 |
| `ClsPrimaryKeyFind` | `Library.DAL/InternalCls/ClsPrimaryKeyFind.cs` | 3 | — | — | 0 |
| `clsNum2Word` | `Library.DAL/InternalCls/clsNum2Word.cs` | 2 | — | — | 0 |
| `DB_Authentication` | `Library.DAL/MAIN_FUNCTION/DB_Authentication.cs` | 0 | — | — | 0 |
| `DB_Connection` | `Library.DAL/MAIN_FUNCTION/DB_Connection.cs` | 1 | — | — | 0 |
| `DB_Manager` | `Library.DAL/MAIN_FUNCTION/DB_Manager.cs` | 14 | — | — | 0 |
| `DB_Names` | `Library.DAL/MAIN_FUNCTION/DB_Names.cs` | 0 | — | — | 0 |
| `PanalClsDAL` | `Library.DAL/PanalCls/PanalClsDAL.cs` | 11 | `tblLoginLog`, `tblMainMenu`, `tblMenuDistribution`, `tblUser`, `tblUserCompanyUnit` | — | 0 |
| `PanalDAL` | `Library.DAL/Panal_DAL/PanalDAL.cs` | 23 | `tblActionPageWiseStep`, `tblActionSteps`, `tblActionUserWiseApproval`, `tblLoginLog`, `tblMainMenu`, `tblMenuDistribution`, `tblUser` | — | 0 |
| — | `Library.DAL/Properties/AssemblyInfo.cs` | 0 | — | — | 0 |
| `ASMInfoDal` | `Library.DAL/SInventory_DAL/ASMInfoDal.cs` | 12 | `tblASMInfo`, `tblArea`, `tblCompanyInfo`, `tblEmpGeneralInfo`, `tblRegion` | — | 0 |
| `ASMWiseProductSalesDal` | `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs` | 8 | `tblASMInfo`, `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblDcWiseAreaInfo`, `tblEmpGeneralInfo`, `tblGroupInformation`, `tblInvoice` … | — | 0 |
| `AreaDAL` | `Library.DAL/SInventory_DAL/AreaDAL.cs` | 15 | `tblArea`, `tblDistrict`, `tblMIAInfo`, `tblRegion` | `sp_Common_GetUnitByPermissionByModule`, `sp_Common_GetUnitWiseLocation_Zas`, `sp_GET_AllItemDropDown_Zas`, `sp_INV_WeightedAverage_Report` | 1 |
| `AreaWiseMonthlyInventoryReportDal` | `Library.DAL/SInventory_DAL/AreaWiseMonthlyInventoryReportDal.cs` | 3 | `tblCentralStore`, `tblChalanDetail`, `tblChalanInfo`, `tblCompanyUnit`, `tblDCStore`, `tblDCStoreFreeze`, `tblDCStore_OpeningBalance`, `tblInvoice` … | `sp_DCBinCard`, `sp_GetExpireproductList`, `sp_GetExpireproductListNational` | 0 |
| `AuditReportOneDal` | `Library.DAL/SInventory_DAL/AuditReportOneDal.cs` | 4 | `View_CustomerMaster`, `tblCompanyUnit`, `tblCustMaster`, `tblOrderDel`, `tblOrderDetailDel` | — | 0 |
| `BatchWiseCollectionDal` | `Library.DAL/SInventory_DAL/BatchWiseCollectionDal.cs` | 1 | `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice`, `tblInvoiceBatch` | — | 0 |
| `BatchWiseCollectionReportDal` | `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs` | 18 | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoice` … | `sp_CustomerPaymentPosting`, `sp_Sales_CustomerPaymentReversePosting` | 0 |
| `BatchWiseTopSheetDal` | `Library.DAL/SInventory_DAL/BatchWiseTopSheetDal.cs` | 7 | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceBatch`, `tblInvoiceDetail` … | — | 0 |
| `BatchWiseTransitReportDal` | `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs` | 7 | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblDcWiseAreaInfo`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblGroupInformation` … | — | 0 |
| `BonusCampaignDal` | `Library.DAL/SInventory_DAL/BonusCampaignDal.cs` | 9 | `tblCompanyInfo`, `tblProduct`, `tblUnitPrice` | `sp_CPM_BonusCampaignDetailInsert`, `sp_CPM_BonusCampaigntMasterInsert`, `sp_CPM_BonusCampaigntMasterUpdate`, `sp_CPM_DeleteBonusCampaign` | 0 |
| `CampaignDAL` | `Library.DAL/SInventory_DAL/CampaignDAL.cs` | 8 | `tblBonusCampaignMaster`, `tblBonusCampgainDetail`, `tblCompanyInfo`, `tblProduct` | — | 3 |
| `CentralStoreDAL` | `Library.DAL/SInventory_DAL/CentralStoreDAL.cs` | 11 | `tblCentralStore`, `tblCurrentStock`, `tblUnitPrice` | — | 0 |
| `ChalanDAL` | `Library.DAL/SInventory_DAL/ChalanDAL.cs` | 34 | `tblCentralStore`, `tblChalanDetail`, `tblChalanInfo`, `tblCompanyUnit`, `tblDCStock`, `tblDCStore`, `tblDCStoreFreeze`, `tblManufacturer` … | `sp_Process_WeightedAverage`, `sp_Process_WeightedAverageB2BTransfer`, `sp_Process_WeightedAverageFreezeStock` | 0 |
| `ComUnitSalesReportDAL` | `Library.DAL/SInventory_DAL/ComUnitSalesReportDAL.cs` | 4 | `View_MIAWiseSalesReport`, `tblCompanyUnit` | — | 0 |
| `CompanyInfoDAL` | `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs` | 10 | `tblCompanyInfo`, `tblGroupInformation` | — | 0 |
| `CompanySalesReportDAL` | `Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs` | 11 | `View_CustomerMaster`, `View_MIAWiseSalesReport`, `tblArea`, `tblCompanyUnit`, `tblCustPayDetail`, `tblCustomerPay`, `tblDCStore`, `tblDistrict` … | — | 0 |
| `CompanywisebranchDal` | `Library.DAL/SInventory_DAL/CompanywisebranchDal.cs` | 6 | `tblBankInfo`, `tblBranchInfo`, `tblCompanyInfo`, `tblCompanyWiseDeposit` | — | 0 |
| `CreditLimitDAL` | `Library.DAL/SInventory_DAL/CreditLimitDAL.cs` | 11 | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustomerCreditLimit`, `tblInvoice` | — | 2 |
| `CustPaymentDAL` | `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` | 41 | `View_CustomerMaster`, `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCreditAdjustment`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay` … | `sp_CustomerPaymentPosting`, `sp_Sales_CustomerPaymentPosting`, `sp_Save_PayCode`, `sp_Save_SaveCustPayment`, `sp_UD_InvoicePayment` | 2 |
| `CustomerCategoryDAL` | `Library.DAL/SInventory_DAL/CustomerCategoryDAL.cs` | 5 | `tblCustCategory` | — | 0 |
| `CustomerCoverageReportDal` | `Library.DAL/SInventory_DAL/CustomerCoverageReportDal.cs` | 2 | `View_CustomerMaster`, `tblEmpGeneralInfo`, `tblInvoice`, `tblMIOInfo` | — | 0 |
| `CustomerDueReportDAL` | `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs` | 6 | `View_CustomerMaster`, `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblEmpGeneralInfo` … | — | 0 |
| `CustomerFixedDiscountDal` | `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs` | 4 | `tblCompanyInfo`, `tblCustMaster`, `tblCustomerFixedDiscount`, `tblGroupInformation`, `tblMIOInfo`, `tblMarket`, `tblTerritory` | — | 0 |
| `CustomerLedgerDAL` | `Library.DAL/SInventory_DAL/CustomerLedgerDAL.cs` | 1 | `View_CustomerMaster`, `tblInvoice` | — | 0 |
| `CustomerMasterDAL` | `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs` | 39 | `View_CustomerMaster`, `tblArea`, `tblCompanyUnit`, `tblCustCategory`, `tblCustMaster`, `tblDistrict`, `tblInvoice`, `tblMIAInfo` … | `sp_DeleteOrder` | 0 |
| `CustomerMasterInfoDal` | `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs` | 38 | `View_CustomerMaster`, `tblArea`, `tblCompanyUnit`, `tblCustCategory`, `tblCustMaster`, `tblDistrict`, `tblInvoice`, `tblMIAInfo` … | `sp_I_CustomerGl`, `sp_Integration_CustomerSubsidaryInsert`, `sp_UD_CustomerGl` | 0 |
| `CustomerPaymentReportDal` | `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs` | 7 | `View_CustomerMaster`, `tblASMInfo`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblCustomerReturnMaster` … | — | 0 |
| `CustomerReportDal` | `Library.DAL/SInventory_DAL/CustomerReportDal.cs` | 5 | `tblArea`, `tblCustCategory`, `tblCustMaster`, `tblMarket`, `tblRegion`, `tblTerritory` | — | 0 |
| `CustomerReturnBYMioDal` | `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs` | 7 | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblDcWiseAreaInfo`, `tblEmpGeneralInfo`, `tblGroupInformation`, `tblInvoice`, `tblInvoiceDetail` … | — | 0 |
| `CustomerReturnDAL` | `Library.DAL/SInventory_DAL/CustomerReturnDAL.cs` | 11 | `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerReturnDetail`, `tblCustomerReturnMaster`, `tblDCStore`, `tblInvoice`, `tblInvoiceDetail` | `sp_Sales_DeliverySalesReturnPosting` | 0 |
| `CustomerWiseLedgerDal` | `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs` | 9 | `View_CustomerMaster`, `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblDcWiseAreaInfo` … | — | 0 |
| `CustomerWiseProductSalesDal` | `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs` | 6 | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblEmpGeneralInfo`, `tblGroupInformation`, `tblInvoice`, `tblInvoiceDetail` … | — | 0 |
| `CustomerWiseReturnDal` | `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs` | 10 | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblDcWiseAreaInfo`, `tblEmpGeneralInfo`, `tblGroupInformation`, `tblInvoice` … | — | 0 |
| `CustomerWiseSalesReportDAL` | `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs` | 6 | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblEmpGeneralInfo`, `tblGroupInformation`, `tblInvoice` … | — | 0 |
| `DCStockInfoReprotDal` | `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs` | 5 | `View_CustomerMaster`, `tblASMInfo`, `tblCentralStore`, `tblChalanDetail`, `tblChalanInfo`, `tblCompanyInfo`, `tblCompanyUnit`, `tblDCStore` … | — | 0 |
| `DCStockReportDAL` | `Library.DAL/SInventory_DAL/DCStockReportDAL.cs` | 7 | `View_ProDCStock`, `tblCentralStore`, `tblChalanDetail`, `tblChalanInfo`, `tblCompanyUnit`, `tblDCStore`, `tblDCStoreFreeze`, `tblInvoice` … | — | 0 |
| `DCStoreDAL` | `Library.DAL/SInventory_DAL/DCStoreDAL.cs` | 12 | `tblCurrentStock`, `tblDCStore`, `tblProduct`, `tblProductQuotedPrice`, `tblUnitPrice` | — | 0 |
| `DCUserPermissionDal` | `Library.DAL/SInventory_DAL/DCUserPermissionDal.cs` | 6 | `tblCompanyUnit`, `tblEmpGeneralInfo`, `tblUser`, `tblUserCompanyUnit`, `tblWearhouse` | — | 0 |
| `DashboardDal` | `Library.DAL/SInventory_DAL/DashboardDal.cs` | 5 | `tblCompanyUnit`, `tblCustomerCreditLimit`, `tblDCStore`, `tblUser` | — | 0 |
| `DashboardInfoDal` | `Library.DAL/SInventory_DAL/DashboardInfoDal.cs` | 1 | `tblCompanyInfo`, `tblCompanyUnit`, `tblInvoice`, `tblInvoiceDetail` | — | 0 |
| `DcWiseAreaAloDal` | `Library.DAL/SInventory_DAL/DcWiseAreaAloDal.cs` | 7 | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblDcWiseAreaInfo` | — | 0 |
| `DeleteInvoiceReportDAL` | `Library.DAL/SInventory_DAL/DeleteInvoiceReportDAL.cs` | 4 | `tblCompanyUnit`, `tblCustMaster`, `tblInvoiceDeleteLog`, `tblInvoiceDetail_DeleterRecord` | — | 0 |
| `DeliveryInvoiceReportDal` | `Library.DAL/SInventory_DAL/DeliveryInvoiceReportDal.cs` | 8 | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblDcWiseAreaInfo`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoice` … | — | 0 |
| `DeliveryPersonDAL` | `Library.DAL/SInventory_DAL/DeliveryPersonDAL.cs` | 9 | `tblCompanyInfo`, `tblDeliveryManInfo`, `tblEmpGeneralInfo` | — | 0 |
| `DepartmentDAL` | `Library.DAL/SInventory_DAL/DepartmentDAL.cs` | 6 | `tblDepartment` | — | 0 |
| `DepositSlipEntryDal` | `Library.DAL/SInventory_DAL/DepositSlipEntryDal.cs` | 9 | `tblBankInfo`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCompanyWiseDeposit`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice`, `tblUserCompanyUnit` | `sp_I_CompanyWiseDeposit` | 0 |
| `DesignationDal` | `Library.DAL/SInventory_DAL/DesignationDal.cs` | 6 | `tblDesignation` | — | 0 |
| `DestroyProductDal` | `Library.DAL/SInventory_DAL/DestroyProductDal.cs` | 23 | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblDCStore`, `tblDCStoreFreeze`, `tblDestroyProduct`, `tblGroupInformation`, `tblInvoice` … | `sp_NonTranscationalInvoiceApproval`, `sp_NonTranscationalInvoiceApproval_New`, `sp_Sales_ProductDestroyPosting` | 0 |
| `DirectStockOutDal` | `Library.DAL/SInventory_DAL/DirectStockOutDal.cs` | 1 | `tblCompanyUnit`, `tblDCStore`, `tblDirectStockOut` | — | 0 |
| `DistributionCenterInfoDal` | `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs` | 13 | `tblCompanyInfo`, `tblCompanyUnit`, `tblDistributionType`, `tblRegion` | — | 0 |
| `DistributionCenterStockMonitoringDal` | `Library.DAL/SInventory_DAL/DistributionCenterStockMonitoringDal.cs` | 2 | — | `sp_GET_AllStockProductwise`, `sp_GET_EmployeeWiseProductSale` | 0 |
| `DuplicatePaymentCorrectionDal` | `Library.DAL/SInventory_DAL/DuplicatePaymentCorrectionDal.cs` | 2 | `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice` | `sp_DEL_DuplicatePayment` | 0 |
| `EmpGeneralInfoDAL` | `Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs` | 10 | `tblCompanyInfo`, `tblDepartment`, `tblDesignation`, `tblEmpGeneralInfo` | — | 0 |
| `EmployeeInformationDAL` | `Library.DAL/SInventory_DAL/EmployeeInformationDAL.cs` | 4 | `tblCompanyInfo`, `tblDeliveryManInfo`, `tblDepartment`, `tblDesignation`, `tblEmpGeneralInfo`, `tblGroupInformation` | — | 0 |
| `ErrorInvoiceDal` | `Library.DAL/SInventory_DAL/ErrorInvoiceDal.cs` | 1 | `tblCompanyUnit`, `tblCustMaster`, `tblInvoice`, `tblInvoiceDetail` | — | 0 |
| `ExcelUpForMIGODAL` | `Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs` | 25 | `tblCentralStore`, `tblCustomerMasterExcelFileDetail`, `tblCustomerMasterExcelFileMaster`, `tblMIGODetail`, `tblMIGOMaster`, `tblManufacturer`, `tblOrderListDetail`, `tblOrderListMaster` … | `sp_CustomerTransfer`, `sp_I_Customer`, `sp_StockInMIGOtoCentralStore` | 0 |
| `FieldForceSummeryDal` | `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs` | 24 | `View_CustomerMaster`, `tblASMInfo`, `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay` … | `sp_GET_DCStockInfo`, `sp_GET_WingsSalesReport`, `sp_GET_WingsSalesReportByCompany`, `sp_GET_WingsSalesReportByUnit`, `sp_I_DCStockOutInfo` | 0 |
| `FixedDiscountEntryDal` | `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs` | 11 | `tblCompanyInfo`, `tblCustMaster`, `tblCustomerFixedDiscount`, `tblGroupInformation`, `tblMIOInfo`, `tblMarket`, `tblTerritory`, `tblUserCompanyUnit` | — | 0 |
| `FreezeStockReportDal` | `Library.DAL/SInventory_DAL/FreezeStockReportDal.cs` | 1 | `tblCompanyUnit`, `tblDCStoreFreeze` | — | 0 |
| `HREmployeeDataSyncDal` | `Library.DAL/SInventory_DAL/HREmployeeDataSyncDal.cs` | 3 | — | `sp_Employee_EmployeeCompanySyncedWithDistibution`, `sp_Employee_EmployeeListFromHR`, `sp_Employee_EmployeeListSyncedWithDistibution` | 0 |
| `InvoiceBatchDAL` | `Library.DAL/SInventory_DAL/InvoiceBatchDAL.cs` | 10 | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceBatch`, `tblUserCompanyUnit` | `sp_Save_BatchCode` | 0 |
| `InvoiceDAL` | `Library.DAL/SInventory_DAL/InvoiceDAL.cs` | 104 | `View_CustomerMaster`, `View_DCStoreCurrentStock`, `tblArea`, `tblBonusCampgainDetail`, `tblCompanyInfo`, `tblCompanyUnit`, `tblConsumerCustomer`, `tblCreditAdjustment` … | `sp_Deletenvoice`, `sp_DeliveryConfirmationFullPosting`, `sp_DeliveryConfirmationPartiallPosting`, `sp_DeliveryConformationFull`, `sp_DeliveryConformationReject` … | 0 |
| `InvoiceReportDal` | `Library.DAL/SInventory_DAL/InvoiceReportDal.cs` | 10 | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblDcWiseAreaInfo`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoice` … | — | 0 |
| `MIOSalesSummeryReportDal` | `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs` | 8 | `tblASMInfo`, `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblDcWiseAreaInfo`, `tblEmpGeneralInfo`, `tblGroupInformation` … | — | 0 |
| `MIOSetupDal` | `Library.DAL/SInventory_DAL/MIOSetupDal.cs` | 14 | `tblASMInfo`, `tblArea`, `tblCompanyInfo`, `tblEmpGeneralInfo`, `tblMIOInfo`, `tblMioInfo`, `tblRSMInfo`, `tblRegion` … | — | 0 |
| `MIOWiseProformaReportDAL` | `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs` | 5 | `View_CustomerMaster`, `tblCompanyInfo`, `tblCompanyUnit`, `tblDCStore`, `tblEmpGeneralInfo`, `tblGroupInformation`, `tblInvoice`, `tblInvoiceDetail` … | — | 0 |
| `ManualOrderCreationDal` | `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs` | 27 | `tblASMInfo`, `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerCreditLimit`, `tblDcWiseAreaInfo` … | `sp_I_OrderMaster`, `sp_UD_SampleStockIssue` | 0 |
| `ManufacturerDAL` | `Library.DAL/SInventory_DAL/ManufacturerDAL.cs` | 6 | `tblManufacturer` | — | 0 |
| `MarketInfoDAL` | `Library.DAL/SInventory_DAL/MarketInfoDAL.cs` | 17 | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblDistrict`, `tblMarket`, `tblRegion`, `tblTerritory` | — | 0 |
| `MaterialConversionReceiveDal` | `Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs` | 6 | `tblCompanyInfo`, `tblPackSize`, `tblProduct`, `tblWHStockInDetail`, `tblWHStockInMaster`, `tbl_MC_ProductRawMaterialDetails` | `sp_DeliveryConformationFull` | 0 |
| `MiaSalesReportDAL` | `Library.DAL/SInventory_DAL/MiaSalesReportDAL.cs` | 5 | `View_CustomerMaster`, `View_MIAWiseSalesReport`, `tblCompanyUnit`, `tblMIAInfo` | — | 0 |
| `MiaTargetDAL` | `Library.DAL/SInventory_DAL/MiaTargetDAL.cs` | 13 | `tblCompanyInfo`, `tblEmpGeneralInfo`, `tblGroupInformation`, `tblMIAInfo`, `tblMIATarget`, `tblMIATargetProductWise`, `tblMIOInfo`, `tblProduct` … | — | 0 |
| `MioWiseCustomerDal` | `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs` | 6 | `View_CustomerMaster`, `tblCompanyInfo`, `tblCompanyUnit`, `tblEmpGeneralInfo`, `tblGroupInformation`, `tblMIOInfo`, `tblTerritory`, `tblUserCompanyUnit` | — | 0 |
| `MioWiseProductSalesDal` | `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs` | 9 | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblDcWiseAreaInfo`, `tblEmpGeneralInfo`, `tblGroupInformation`, `tblInvoice` … | — | 0 |
| `NationalBeenCaedDal` | `Library.DAL/SInventory_DAL/NationalBeenCaedDal.cs` | 1 | — | `sp_NationalBinCard` | 0 |
| `NationalSalesProductWiseDal` | `Library.DAL/SInventory_DAL/NationalSalesProductWiseDal.cs` | 3 | `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceDetail`, `tblMIOInfo`, `tblOrder` … | — | 0 |
| `OrderInfoDAL` | `Library.DAL/SInventory_DAL/OrderInfoDAL.cs` | 43 | `View_CustomerMaster`, `View_InvoiceWiseDue`, `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustomerCreditLimit`, `tblCustomerFixedDiscount` … | — | 1 |
| `OrderListDAL` | `Library.DAL/SInventory_DAL/OrderListDAL.cs` | 31 | `tblASMInfo`, `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerCreditLimit`, `tblDcWiseAreaInfo` … | — | 0 |
| `OrderStatusDal` | `Library.DAL/SInventory_DAL/OrderStatusDal.cs` | 6 | `tblInvoice`, `tblInvoiceDetail`, `tblOrder`, `tblOrderDetail` | — | 0 |
| `OtherStockActionDAL` | `Library.DAL/SInventory_DAL/OtherStockActionDAL.cs` | 15 | `tblCompanyInfo`, `tblCompanyUnit`, `tblDCStore`, `tblDCStoreFreeze`, `tblInvoice`, `tblInvoiceDetail`, `tblMarket`, `tblProduct` … | `sp_FreezeStockReleasePosting` | 0 |
| `PackSizeDAL` | `Library.DAL/SInventory_DAL/PackSizeDAL.cs` | 5 | `tblPackSize` | — | 0 |
| `PaymentReverseDal` | `Library.DAL/SInventory_DAL/PaymentReverseDal.cs` | 6 | `tblCustPayDetail`, `tblCustomerPay`, `tblInvoice`, `tblOrder` | `sp_I_PaymentDeleteLog` | 0 |
| `PaymentTranscationDal` | `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs` | 27 | `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblInvoice`, `tblInvoiceBatch` … | `sp_CustomerPaymentPosting`, `sp_MRCP_DeletePayment`, `sp_MRCP_MoneyReceiptMasterInsert`, `sp_PMNT_CustomerPaymentDetailInsert`, `sp_PMNT_CustomerPaymentMasterInsert` … | 0 |
| `PettyCashDaL` | `Library.DAL/SInventory_DAL/PettyCashDaL.cs` | 22 | `tblCompanyInfo`, `tblCustMaster`, `tblCustomerCreditLimit`, `tblPettyCashDetails`, `tblPettyCashMaster`, `tblProduct`, `tblProductDiscount` | `sp_DEL_PettyCashDetails`, `sp_DEL_PettyCashMaster`, `sp_GET_PretyCashDetails_ById`, `sp_GET_PretyCashMaster_ById`, `sp_I_PrettyCashDetails` … | 0 |
| `ProTypeDAL` | `Library.DAL/SInventory_DAL/ProTypeDAL.cs` | 5 | `tblProType` | — | 0 |
| `ProductCategoryDAL` | `Library.DAL/SInventory_DAL/ProductCategoryDAL.cs` | 5 | `tblProCategory` | — | 0 |
| `ProductDAL` | `Library.DAL/SInventory_DAL/ProductDAL.cs` | 23 | `tblCompanyInfo`, `tblIngridents`, `tblManufacturer`, `tblPackSize`, `tblProCategory`, `tblProType`, `tblProduct`, `tblProductCase` … | `SP_DataMigration_Item` | 0 |
| `ProductDiscountDAL` | `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs` | 35 | `View_CustomerMaster`, `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblDcWiseAreaInfo`, `tblEmpGeneralInfo`, `tblMIOInfo` … | `sp_DEL_ProductDiscount` | 0 |
| `ProductSQDAL` | `Library.DAL/SInventory_DAL/ProductSQDAL.cs` | 7 | `tblIngridents`, `tblProductSQ` | — | 0 |
| `ProductUnitPriceDAL` | `Library.DAL/SInventory_DAL/ProductUnitPriceDAL.cs` | 20 | `tblCompanyInfo`, `tblPackSize`, `tblProCategory`, `tblProType`, `tblProduct`, `tblProductCase`, `tblProductSQ`, `tblStockUOM` … | `sp_I_ProductGl` | 0 |
| `ProformaOrInvoiceReturn` | `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs` | 10 | `tblDCStoreFreeze`, `tblInvoice`, `tblInvoiceDetail`, `tblSampleIssue`, `tblWHStockInMaster` | `sp_DeleteDeliveryInvoice`, `sp_DeleteDeliveryInvoice_FullRollback`, `sp_Delete_ProformaInvoice`, `sp_DeliveryInvoiceDeletePosting`, `sp_ProformaReturnPosting` | 0 |
| `RSMSetupDal` | `Library.DAL/SInventory_DAL/RSMSetupDal.cs` | 10 | `tblCompanyInfo`, `tblEmpGeneralInfo`, `tblRSMInfo`, `tblRegion` | — | 0 |
| `RSMWiseProductSalesDal` | `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs` | 8 | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblDcWiseAreaInfo`, `tblEmpGeneralInfo`, `tblGroupInformation`, `tblInvoice`, `tblInvoiceDetail` … | — | 0 |
| `RegionDAL` | `Library.DAL/SInventory_DAL/RegionDAL.cs` | 9 | `tblCompanyInfo`, `tblRegion` | — | 0 |
| `RequisitionDAL` | `Library.DAL/SInventory_DAL/RequisitionDAL.cs` | 46 | `View_CentralStoreCurrentStock`, `tblCentralStore`, `tblChalanDetail`, `tblCompanyInfo`, `tblCompanyUnit`, `tblDCStore`, `tblDCStoreFreeze`, `tblMIGODetail` … | `sp_DeliveryConfirmationFullPosting`, `sp_Process_WeightedAverage`, `sp_UD_ReceiveIdInDc` | 0 |
| `ReturnAcknowledgmentNoteDal` | `Library.DAL/SInventory_DAL/ReturnAcknowledgmentNoteDal.cs` | 8 | `tblCompanyInfo`, `tblNonTranscationalInvoiceDetail`, `tblNonTranscationalInvoiceMaster`, `tblRegion` | `sp_RAN_DeleteById`, `sp_RAN_DetailInsert`, `sp_RAN_MasterInsert` | 0 |
| `RollBackDAL` | `Library.DAL/SInventory_DAL/RollBackDAL.cs` | 8 | `tblCentralStore`, `tblRequisition`, `tblRequsitionChild`, `tblStockInTransfar` | — | 0 |
| `SCtoWHTransferDal` | `Library.DAL/SInventory_DAL/SCtoWHTransferDal.cs` | 4 | `tblCompanyInfo`, `tblCompanyUnit`, `tblUserCompanyUnit` | — | 0 |
| `SalesAdministrationReportDal` | `Library.DAL/SInventory_DAL/SalesAdministrationReportDal.cs` | 4 | `tblASMInfo`, `tblArea`, `tblCompanyInfo`, `tblDcWiseAreaInfo`, `tblEmpGeneralInfo`, `tblMIOInfo`, `tblRSMInfo`, `tblRegion` … | — | 0 |
| `SalesDashboardDal` | `Library.DAL/SInventory_DAL/SalesDashboardDal.cs` | 26 | `View_CustomerMaster`, `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay`, `tblDCStore` … | `sp_DSB_AglingReport`, `sp_DSB_BusinessSummery`, `sp_Dashboard_Top50Customer`, `sp_GET_CashFlowSummaryBDTCurrency`, `sp_GET_CompanyInfo` … | 0 |
| `SalesHubDAL` | `Library.DAL/SInventory_DAL/SalesHubDAL.cs` | 7 | `tblArea`, `tblDcWiseAreaInfo`, `tblMarket`, `tblRegion`, `tblTerritory` | — | 0 |
| `SalesPerformanceReportProductWise_Dal` | `Library.DAL/SInventory_DAL/SalesPerformanceReportProductWise_Dal.cs` | 4 | — | — | 0 |
| `SalesSummeryReportForAcc` | `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs` | 6 | `tblCompanyInfo`, `tblCompanyUnit`, `tblGroupInformation`, `tblUserCompanyUnit` | `sp_Summery_SalesSummerReportForAccounts` | 0 |
| `SampleInvoiceDal` | `Library.DAL/SInventory_DAL/SampleInvoiceDal.cs` | 7 | `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblDcWiseAreaInfo`, `tblDeliveryManInfo`, `tblEmpGeneralInfo`, `tblMarket` … | — | 0 |
| `SampleIssueDal` | `Library.DAL/SInventory_DAL/SampleIssueDal.cs` | 33 | `tblASMInfo`, `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerCreditLimit`, `tblDCStore` … | `sp_DEL_SampleStockIssue`, `sp_I_SampleProductIssue`, `sp_I_SampleProductIssueDetail`, `sp_SampleInvoicePosting`, `sp_UD_SampleIssue` … | 0 |
| `ShippingCartonSizeDAL` | `Library.DAL/SInventory_DAL/ShippingCartonSizeDAL.cs` | 6 | `tblProduct`, `tblProductCase` | — | 0 |
| `SlaveApprovalDal` | `Library.DAL/SInventory_DAL/SlaveApprovalDal.cs` | 4 | `tblCompanyInfo`, `tblSlaveInfo` | — | 0 |
| `SlaveInformationDal` | `Library.DAL/SInventory_DAL/SlaveInformationDal.cs` | 6 | `tblCompanyInfo`, `tblSlaveInfo` | — | 0 |
| `StockConditionFreezeDAL` | `Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs` | 16 | `View_CustomerMaster`, `tblCentralStore`, `tblCompanyInfo`, `tblCompanyUnit`, `tblDCStore`, `tblDCStoreFreeze`, `tblInvoice`, `tblInvoiceDetail` … | `sp_FreezeStockPosting` | 0 |
| `StockConditionPermissionDal` | `Library.DAL/SInventory_DAL/StockConditionPermissionDal.cs` | 6 | `tblStockCondition`, `tblStockConditionPermission`, `tblUser` | — | 0 |
| `StockTransportOrderReportDAL` | `Library.DAL/SInventory_DAL/StockTransportOrderReportDAL.cs` | 3 | `tblCompanyInfo`, `tblCompanyUnit`, `tblProduct`, `tblRequisition`, `tblRequsitionChild`, `tblStockInTransfar`, `tblStockUOM`, `tblWearhouse` | — | 0 |
| `StockUOMDAL` | `Library.DAL/SInventory_DAL/StockUOMDAL.cs` | 5 | `tblStockUOM` | — | 0 |
| `SupplierInfoDal` | `Library.DAL/SInventory_DAL/SupplierInfoDal.cs` | 9 | `tblSupplierInformation` | `sp_I_SupplierGl` | 0 |
| `TerritoryInfoDal` | `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs` | 12 | `tblArea`, `tblDistrict`, `tblMIAInfo`, `tblRegion`, `tblTerritory` | — | 0 |
| `TotalSummaryDAL` | `Library.DAL/SInventory_DAL/TotalSummaryDAL.cs` | 1 | `tblCompanyUnit`, `tblInvoice`, `tblInvoiceDetail` | — | 1 |
| `TradeMaterialTransferDal` | `Library.DAL/SInventory_DAL/TradeMaterialTransferDal.cs` | 18 | `tblCompanyInfo`, `tblCompanyUnit`, `tblDCStore`, `tblProduct`, `tblTradeMaterialTransfer`, `tblUnitPrice`, `tbl_INV_WeightedAvg` | `spDeleteTradeMaterialTransfer`, `spInsertTradeMaterialTransfer`, `spUpdateTradeMaterialTransfer`, `sp_Common_GetUnitByPermissionByModule`, `sp_Common_GetUnitWiseLocation_FromErp` … | 3 |
| `UserCredentialChangeDal` | `Library.DAL/SInventory_DAL/UserCredentialChangeDal.cs` | 1 | `tblUser`, `tblUserLog` | — | 0 |
| `UserDAL` | `Library.DAL/SInventory_DAL/UserDAL.cs` | 11 | `tblCompanyUnit`, `tblUser`, `tblUserCompanyUnit`, `tblUserType` | — | 0 |
| `UserMenuPermissionDal` | `Library.DAL/SInventory_DAL/UserMenuPermissionDal.cs` | 2 | `tblMainMenu`, `tblMenuDistribution`, `tblUser` | — | 0 |
| `WHBeenCardReportDal` | `Library.DAL/SInventory_DAL/WHBeenCardReportDal.cs` | 2 | `tblCompanyInfo`, `tblWearhouse` | — | 0 |
| `WHBinCardDal` | `Library.DAL/SInventory_DAL/WHBinCardDal.cs` | 1 | — | `sp_WearhouseBinCard` | 0 |
| `WHStockInReportDal` | `Library.DAL/SInventory_DAL/WHStockInReportDal.cs` | 7 | `tblCompanyInfo`, `tblManufacturer`, `tblPackSize`, `tblProduct`, `tblWHStockInDetail`, `tblWHStockInMaster`, `tblWearhouse` | `sp_Delete_WHStockIn` | 0 |
| `WHStockListDAL` | `Library.DAL/SInventory_DAL/WHStockListDAL.cs` | 6 | `tblCentralStore`, `tblProduct`, `tblWHStockInDetail`, `tblWHStockInMaster`, `tblWHStockOutDetail`, `tblWHStockOutMaster`, `tblWearhouse` | — | 0 |
| `WHStockValuationReprtDal` | `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs` | 7 | `tblCompanyInfo`, `tblCompanyUnit`, `tblGroupInformation`, `tblUserCompanyUnit` | `sp_INV_DCStockValuationReport`, `sp_INV_WHStockValuationReport` | 0 |
| `WarehousePickingDAL` | `Library.DAL/SInventory_DAL/WarehousePickingDAL.cs` | 1 | `tblRequisition` | — | 0 |
| `WarehouseStockInApprovalDal` | `Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs` | 7 | `tblCentralStore`, `tblProduct`, `tblWHStockInDetail`, `tblWHStockInMaster`, `tblWearhouse` | `sp_Acc_MaterialConversionReceive`, `sp_WHStockInPosting` | 0 |
| `WarehouseStockInDal` | `Library.DAL/SInventory_DAL/WarehouseStockInDal.cs` | 19 | `tblCompanyInfo`, `tblManufacturer`, `tblNonTranscationalInvoiceDetail`, `tblNonTranscationalInvoiceMaster`, `tblProduct`, `tblSupplierInformation`, `tblWHStockInDetail`, `tblWHStockInMaster` … | — | 0 |
| `WebServiceDal` | `Library.DAL/SInventory_DAL/WebServiceDal.cs` | 4 | `tblArea`, `tblCustMaster`, `tblMarket`, `tblMoneyReceipt`, `tblProduct`, `tblRegion`, `tblTerritory`, `tblUnitPrice` | — | 0 |
| `WhFreezeStockReleaseDal` | `Library.DAL/SInventory_DAL/WhFreezeStockReleaseDal.cs` | 8 | `tblCentralStore`, `tblCompanyInfo`, `tblProduct`, `tblWearhouse`, `tblWhStockConditionFreeze`, `tblWhStoreFreeze` | — | 0 |
| `WhStockConditionFreezeDal` | `Library.DAL/SInventory_DAL/WhStockConditionFreezeDal.cs` | 10 | `tblCentralStore`, `tblChalanDetail`, `tblChalanInfo`, `tblCompanyInfo`, `tblCompanyUnit`, `tblDCStore`, `tblDCStoreFreeze`, `tblInvoice` … | — | 0 |
| `WhStockMonitoringReportDal` | `Library.DAL/SInventory_DAL/WhStockMonitoringReportDal.cs` | 2 | `tblCentralStore`, `tblCentralStore_OpeninigBalance`, `tblInvoice`, `tblInvoiceDetail`, `tblProduct`, `tblRequisition`, `tblRequsitionChild`, `tblStockInTransfar` | — | 0 |
| `WingsSalesAndCollectionReportDal` | `Library.DAL/SInventory_DAL/WingsSalesAndCollectionReportDal.cs` | 4 | `tblCompanyInfo`, `tblCompanyUnit`, `tblEmpGeneralInfo`, `tblGroupInformation`, `tblInvoice`, `tblMIOInfo`, `tblOrder`, `tblTerritory` | — | 0 |
| `WingsWiseSalesReportDal` | `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs` | 20 | `View_CustomerMaster`, `tblASMInfo`, `tblArea`, `tblCompanyInfo`, `tblCompanyUnit`, `tblCustMaster`, `tblCustPayDetail`, `tblCustomerPay` … | `sp_GET_DCStockInfo`, `sp_GET_WingsSalesReport`, `sp_GET_WingsSalesReportByCompany`, `sp_GET_WingsSalesReportByUnit`, `sp_I_DCStockOutInfo` | 0 |
| `ZASMonitoringDal` | `Library.DAL/SInventory_DAL/ZASMonitoringDal.cs` | 2 | `tblChalanDetail`, `tblCompanyUnit`, `tblCustPayDetail`, `tblCustomerPay`, `tblDCStore`, `tblDCStoreFreeze`, `tblDestroyProduct`, `tblDirectStockOut` … | — | 0 |

## Entity (DAO) classes (99 files)

These are the closest thing to DTOs/ViewModels in the application. They carry public properties only; there is no validation, no data annotation and no mapping configuration. Property names generally match the column names used in the concatenated SQL.

| Class | File | Methods | Tables reached | SPs called | Used by (pages) |
|---|---|---|---|---|---|
| `ObjActionPageWiseStep` | `Library.DAO/Panal_Entities/ObjActionPageWiseStep.cs` | 0 | — | — | 0 |
| `ObjPanal` | `Library.DAO/Panal_Entities/ObjPanal.cs` | 0 | — | — | 0 |
| `ObjUserWiseApprovalPermission` | `Library.DAO/Panal_Entities/ObjUserWiseApprovalPermission.cs` | 0 | — | — | 0 |
| — | `Library.DAO/Properties/AssemblyInfo.cs` | 0 | — | — | 0 |
| `ASMInfoDao` | `Library.DAO/SInventory_Entities/ASMInfoDao.cs` | 0 | — | — | 0 |
| `AreaInfo` | `Library.DAO/SInventory_Entities/AreaInfo.cs` | 0 | — | — | 0 |
| `BonusCampaignDetailsDao` | `Library.DAO/SInventory_Entities/BonusCampaignDetailsDao.cs` | 0 | — | — | 0 |
| `BonusCampaignMasterDao` | `Library.DAO/SInventory_Entities/BonusCampaignMasterDao.cs` | 0 | — | — | 0 |
| `CampaignDAO` | `Library.DAO/SInventory_Entities/CampaignDAO.cs` | 0 | — | — | 0 |
| `CampaignDetailDAO` | `Library.DAO/SInventory_Entities/CampaignDetailDAO.cs` | 0 | — | — | 0 |
| `CentralStore` | `Library.DAO/SInventory_Entities/CentralStore.cs` | 0 | — | — | 0 |
| `ChalanDetail` | `Library.DAO/SInventory_Entities/ChalanDetail.cs` | 0 | — | — | 0 |
| `ChalanInfo` | `Library.DAO/SInventory_Entities/ChalanInfo.cs` | 0 | — | — | 0 |
| `CoaDAO` | `Library.DAO/SInventory_Entities/CoaDAO.cs` | 0 | — | — | 0 |
| `CompanyInformation` | `Library.DAO/SInventory_Entities/CompanyInformation.cs` | 0 | — | — | 0 |
| `CompanyUnit` | `Library.DAO/SInventory_Entities/CompanyUnit.cs` | 0 | — | — | 0 |
| `CompanyWiseDepositDao` | `Library.DAO/SInventory_Entities/CompanyWiseDepositDao.cs` | 0 | — | — | 0 |
| `ConsumerCustomerDAO` | `Library.DAO/SInventory_Entities/ConsumerCustomerDAO.cs` | 0 | — | — | 0 |
| `CreditAdjustmentDao` | `Library.DAO/SInventory_Entities/CreditAdjustmentDao.cs` | 0 | — | — | 0 |
| `CreditLimitDAO` | `Library.DAO/SInventory_Entities/CreditLimitDAO.cs` | 0 | — | — | 0 |
| `CurrentStock` | `Library.DAO/SInventory_Entities/CurrentStock.cs` | 0 | — | — | 0 |
| `CustFixedDiscountDao` | `Library.DAO/SInventory_Entities/CustFixedDiscountDao.cs` | 0 | — | — | 0 |
| `CustPayment` | `Library.DAO/SInventory_Entities/CustPayment.cs` | 0 | — | — | 0 |
| `CustPaymentDetail` | `Library.DAO/SInventory_Entities/CustPaymentDetail.cs` | 0 | — | — | 0 |
| `CustomerCategory` | `Library.DAO/SInventory_Entities/CustomerCategory.cs` | 0 | — | — | 0 |
| `CustomerMaster` | `Library.DAO/SInventory_Entities/CustomerMaster.cs` | 0 | — | — | 0 |
| `CustomerReturnDetailDAO` | `Library.DAO/SInventory_Entities/CustomerReturnDetailDAO.cs` | 0 | — | — | 0 |
| `CustomerReturnMasterDAO` | `Library.DAO/SInventory_Entities/CustomerReturnMasterDAO.cs` | 0 | — | — | 0 |
| `DCPicking` | `Library.DAO/SInventory_Entities/DCPicking.cs` | 0 | — | — | 0 |
| `DCPickingDetail` | `Library.DAO/SInventory_Entities/DCPickingDetail.cs` | 0 | — | — | 0 |
| `DCStockNew` | `Library.DAO/SInventory_Entities/DCStockNew.cs` | 0 | — | — | 0 |
| `DCStore` | `Library.DAO/SInventory_Entities/DCStore.cs` | 0 | — | — | 0 |
| `DCStoreFreezeDAO` | `Library.DAO/SInventory_Entities/DCStoreFreezeDAO.cs` | 0 | — | — | 0 |
| `DcWiseAreaAloDao` | `Library.DAO/SInventory_Entities/DcWiseAreaAloDao.cs` | 0 | — | — | 0 |
| `DeliveryPersonDAO` | `Library.DAO/SInventory_Entities/DeliveryPersonDAO.cs` | 0 | — | — | 0 |
| `Department` | `Library.DAO/SInventory_Entities/Department.cs` | 0 | — | — | 0 |
| `Designation` | `Library.DAO/SInventory_Entities/Designation.cs` | 0 | — | — | 0 |
| `DestroyProductDao` | `Library.DAO/SInventory_Entities/DestroyProductDao.cs` | 0 | — | — | 0 |
| `DirectStockOutDao` | `Library.DAO/SInventory_Entities/DirectStockOutDao.cs` | 0 | — | — | 0 |
| `DistrictInfo` | `Library.DAO/SInventory_Entities/DistrictInfo.cs` | 0 | — | — | 0 |
| `EmpGeneralInfo` | `Library.DAO/SInventory_Entities/EmpGeneralInfo.cs` | 0 | — | — | 0 |
| `FixedDiscountDao` | `Library.DAO/SInventory_Entities/FixedDiscountDao.cs` | 0 | — | — | 0 |
| `IncomeStatementDao`, `IncomeStatementGlDao` | `Library.DAO/SInventory_Entities/IncomeStatementDao.cs` | 0 | — | — | 0 |
| `Ingridents` | `Library.DAO/SInventory_Entities/Ingridents.cs` | 0 | — | — | 0 |
| `Invoice` | `Library.DAO/SInventory_Entities/Invoice.cs` | 0 | — | — | 0 |
| `InvoiceBatchDAO` | `Library.DAO/SInventory_Entities/InvoiceBatchDAO.cs` | 0 | — | — | 0 |
| `InvoiceDetail` | `Library.DAO/SInventory_Entities/InvoiceDetail.cs` | 0 | — | — | 0 |
| `MIOSetupDao` | `Library.DAO/SInventory_Entities/MIOSetupDao.cs` | 0 | — | — | 0 |
| `MIOTargetProductWise` | `Library.DAO/SInventory_Entities/MIOTargetProductWise.cs` | 0 | — | — | 0 |
| `Manufacturer` | `Library.DAO/SInventory_Entities/Manufacturer.cs` | 0 | — | — | 0 |
| `MarketInfo` | `Library.DAO/SInventory_Entities/MarketInfo.cs` | 0 | — | — | 0 |
| `MiaInformation` | `Library.DAO/SInventory_Entities/MiaInformation.cs` | 0 | — | — | 0 |
| `MiaTarget` | `Library.DAO/SInventory_Entities/MiaTarget.cs` | 0 | — | — | 0 |
| `MigoMasterDAO` | `Library.DAO/SInventory_Entities/MigoMasterDAO.cs` | 0 | — | — | 0 |
| `NonTranscationalInvoiceDetailDao` | `Library.DAO/SInventory_Entities/NonTranscationalInvoiceDetailDao.cs` | 0 | — | — | 0 |
| `OrderInfoDetail` | `Library.DAO/SInventory_Entities/OrderInfoDetail.cs` | 0 | — | — | 0 |
| `OrderInfoMaster` | `Library.DAO/SInventory_Entities/OrderInfoMaster.cs` | 0 | — | — | 0 |
| `OrderListMasterDAO` | `Library.DAO/SInventory_Entities/OrderListMasterDAO.cs` | 0 | — | — | 0 |
| `PackSize` | `Library.DAO/SInventory_Entities/PackSize.cs` | 0 | — | — | 0 |
| `PaymentBachMasterDao` | `Library.DAO/SInventory_Entities/PaymentBachMasterDao.cs` | 0 | — | — | 0 |
| `PaymentBatchDetailDao` | `Library.DAO/SInventory_Entities/PaymentBatchDetailDao.cs` | 0 | — | — | 0 |
| `PaymentTranscationDetailDao` | `Library.DAO/SInventory_Entities/PaymentTranscationDetailDao.cs` | 0 | — | — | 0 |
| `PaymentTranscationMasterDao` | `Library.DAO/SInventory_Entities/PaymentTranscationMasterDao.cs` | 0 | — | — | 0 |
| `PettyCashDetailsDao` | `Library.DAO/SInventory_Entities/PettyCashDetailsDao.cs` | 0 | — | — | 0 |
| `PettyCashMasterDao` | `Library.DAO/SInventory_Entities/PettyCashMasterDao.cs` | 0 | — | — | 0 |
| `ProType` | `Library.DAO/SInventory_Entities/ProType.cs` | 0 | — | — | 0 |
| `Product` | `Library.DAO/SInventory_Entities/Product.cs` | 0 | — | — | 0 |
| `ProductCategory` | `Library.DAO/SInventory_Entities/ProductCategory.cs` | 0 | — | — | 0 |
| `ProductDiscount` | `Library.DAO/SInventory_Entities/ProductDiscount.cs` | 0 | — | — | 0 |
| `ProductRawMaterialDetailsDao` | `Library.DAO/SInventory_Entities/ProductRawMaterialDetailsDao.cs` | 0 | — | — | 0 |
| `ProductSQ` | `Library.DAO/SInventory_Entities/ProductSQ.cs` | 0 | — | — | 0 |
| `ProductUnitPrice` | `Library.DAO/SInventory_Entities/ProductUnitPrice.cs` | 0 | — | — | 0 |
| `QuotedPriceSetup` | `Library.DAO/SInventory_Entities/QuotedPriceSetup.cs` | 0 | — | — | 0 |
| `RAN_DetailsDao` | `Library.DAO/SInventory_Entities/RAN_DetailsDao.cs` | 0 | — | — | 0 |
| `RAN_MasterDao` | `Library.DAO/SInventory_Entities/RAN_MasterDao.cs` | 0 | — | — | 0 |
| `RSMSetupDao` | `Library.DAO/SInventory_Entities/RSMSetupDao.cs` | 0 | — | — | 0 |
| `RegionInfo` | `Library.DAO/SInventory_Entities/RegionInfo.cs` | 0 | — | — | 0 |
| `Requesition` | `Library.DAO/SInventory_Entities/Requesition.cs` | 0 | — | — | 0 |
| `RequsitionChild` | `Library.DAO/SInventory_Entities/RequsitionChild.cs` | 0 | — | — | 0 |
| `SalesDashboardDao`, `CompanyWiseSalesDao`, `MioWiseInvoiceDao`, `CompanyWiseInvoiceDao`, `CompanyWiseUnitSalesDao`, `MIODao`, `FinancialYearDao`, `CompanyDao`, `CompanyWiseProductSalesDao`, `GroupSalesCompanyWiseDao`, `RegionDao`, `DepotDao`, `WingsSalesTrend`, `AglingReportDao`, `BusinessSummeryReportDao`, `CashFlowDao`, `IncomeSatementDao`, `TrialBalanceDao`, `ExpireProductDao`, `Top50CustomerDao`, `TopPriorityProductSalesDao` | `Library.DAO/SInventory_Entities/SalesDashboardDao.cs` | 0 | — | — | 0 |
| `ShippingCartonSize` | `Library.DAO/SInventory_Entities/ShippingCartonSize.cs` | 0 | — | — | 0 |
| `SlaveInformation` | `Library.DAO/SInventory_Entities/SlaveInformation.cs` | 0 | — | — | 0 |
| `StockConditionFreezeDAO` | `Library.DAO/SInventory_Entities/StockConditionFreezeDAO.cs` | 0 | — | — | 0 |
| `StockConditionPermissionDao` | `Library.DAO/SInventory_Entities/StockConditionPermissionDao.cs` | 0 | — | — | 0 |
| `StockInTransfar` | `Library.DAO/SInventory_Entities/StockInTransfar.cs` | 0 | — | — | 0 |
| `StockUOM` | `Library.DAO/SInventory_Entities/StockUOM.cs` | 0 | — | — | 0 |
| `SupplierInformation` | `Library.DAO/SInventory_Entities/SupplierInformation.cs` | 0 | — | — | 0 |
| `TerritoryInfoDao` | `Library.DAO/SInventory_Entities/TerritoryInfoDao.cs` | 0 | — | — | 0 |
| `TradeMaterialTransferDao` | `Library.DAO/SInventory_Entities/TradeMaterialTransferDao.cs` | 0 | — | — | 0 |
| `UserCompanyUnit` | `Library.DAO/SInventory_Entities/UserCompanyUnit.cs` | 0 | — | — | 0 |
| `UserCompanyUnitDao` | `Library.DAO/SInventory_Entities/UserCompanyUnitDao.cs` | 0 | — | — | 0 |
| `UserInformation` | `Library.DAO/SInventory_Entities/UserInformation.cs` | 0 | — | — | 0 |
| `WHStockInToCenteralStore` | `Library.DAO/SInventory_Entities/WHStockInToCenteralStore.cs` | 0 | — | — | 0 |
| `WarehouseStockInMasterDao` | `Library.DAO/SInventory_Entities/WarehouseStockInMasterDao.cs` | 0 | — | — | 0 |
| `WhStockConditionFreezeDao` | `Library.DAO/SInventory_Entities/WhStockConditionFreezeDao.cs` | 0 | — | — | 0 |
| `WhStoreFreezeDao` | `Library.DAO/SInventory_Entities/WhStoreFreezeDao.cs` | 0 | — | — | 0 |
| `WharehouseStockInDetailsDao` | `Library.DAO/SInventory_Entities/WharehouseStockInDetailsDao.cs` | 0 | — | — | 0 |
| `ZoneInfo` | `Library.DAO/SInventory_Entities/ZoneInfo.cs` | 0 | — | — | 0 |
| `tblNonTranscationalInvoiceMasterDao` | `Library.DAO/SInventory_Entities/tblNonTranscationalInvoiceMasterDao.cs` | 0 | — | — | 0 |

## Layer classes that no page code-behind calls **directly**

This list is *not* a dead-code list. A DAL class reached only through its BLL wrapper appears here and is very
much alive — that is the intended architecture. The list is useful for the opposite reason: anything here that
also has no BLL caller is a candidate for removal. Verify each one before deleting.

168 of 321 layer files.

- `Library.BLL/SInventory_BLL/ASMInfoBll.cs`
- `Library.BLL/SInventory_BLL/AreaBLL.cs`
- `Library.BLL/SInventory_BLL/AreaWiseMonthlyInventoryReportBll.cs`
- `Library.BLL/SInventory_BLL/AuditReportOneBll.cs`
- `Library.BLL/SInventory_BLL/BatchWiseTopSheetBll.cs`
- `Library.BLL/SInventory_BLL/ComUnitSalesReportBLL.cs`
- `Library.BLL/SInventory_BLL/CustomerCoverageReportBll.cs`
- `Library.BLL/SInventory_BLL/CustomerLedgerBLL.cs`
- `Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs`
- `Library.BLL/SInventory_BLL/DCStoreBLL.cs`
- `Library.BLL/SInventory_BLL/DcWiseAreaAloBll.cs`
- `Library.BLL/SInventory_BLL/DeleteInvoiceReportBLL.cs`
- `Library.BLL/SInventory_BLL/DistributionCeneterInfoBll.cs`
- `Library.BLL/SInventory_BLL/DistributionCenterStockMonitoringBll.cs`
- `Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs`
- `Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs`
- `Library.BLL/SInventory_BLL/MIOSetupBll.cs`
- `Library.BLL/SInventory_BLL/PettyCashBLL.cs`
- `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs`
- `Library.BLL/SInventory_BLL/RSMSetupBll.cs`
- `Library.BLL/SInventory_BLL/SalesAdministrationReportBll.cs`
- `Library.BLL/SInventory_BLL/SlaveinformationBll.cs`
- `Library.BLL/SInventory_BLL/StockConditionPermissionBll.cs`
- `Library.BLL/SInventory_BLL/StockTransportOrderReportBLL.cs`
- `Library.BLL/SInventory_BLL/StockUOMBLL.cs`
- `Library.BLL/SInventory_BLL/TerritoryInfoBll.cs`
- `Library.BLL/SInventory_BLL/TotalSummaryBLL.cs`
- `Library.BLL/SInventory_BLL/WHStockInReportBll.cs`
- `Library.BLL/SInventory_BLL/WarehouseStockInApprovalBll.cs`
- `Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs`
- `Library.BLL/SInventory_BLL/WhStockConditionFreezeBll.cs`
- `Library.BLL/SInventory_BLL/WhStockMonitoringReportBll.cs`
- `Library.BLL/SInventory_BLL/WingsSalesAndCollectionReportBll.cs`
- `Library.DAL/InternalCls/ClsApprovalAction.cs`
- `Library.DAL/InternalCls/ClsCommonInternalDAL.cs`
- `Library.DAL/InternalCls/ClsPrimaryKeyFind.cs`
- `Library.DAL/InternalCls/clsNum2Word.cs`
- `Library.DAL/MAIN_FUNCTION/DB_Connection.cs`
- `Library.DAL/MAIN_FUNCTION/DB_Manager.cs`
- `Library.DAL/PanalCls/PanalClsDAL.cs`
- `Library.DAL/Panal_DAL/PanalDAL.cs`
- `Library.DAL/SInventory_DAL/ASMInfoDal.cs`
- `Library.DAL/SInventory_DAL/ASMWiseProductSalesDal.cs`
- `Library.DAL/SInventory_DAL/AreaWiseMonthlyInventoryReportDal.cs`
- `Library.DAL/SInventory_DAL/AuditReportOneDal.cs`
- `Library.DAL/SInventory_DAL/BatchWiseCollectionDal.cs`
- `Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs`
- `Library.DAL/SInventory_DAL/BatchWiseTopSheetDal.cs`
- `Library.DAL/SInventory_DAL/BatchWiseTransitReportDal.cs`
- `Library.DAL/SInventory_DAL/BonusCampaignDal.cs`
- `Library.DAL/SInventory_DAL/CentralStoreDAL.cs`
- `Library.DAL/SInventory_DAL/ChalanDAL.cs`
- `Library.DAL/SInventory_DAL/ComUnitSalesReportDAL.cs`
- `Library.DAL/SInventory_DAL/CompanyInfoDAL.cs`
- `Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs`
- `Library.DAL/SInventory_DAL/CompanywisebranchDal.cs`
- `Library.DAL/SInventory_DAL/CustomerCategoryDAL.cs`
- `Library.DAL/SInventory_DAL/CustomerCoverageReportDal.cs`
- `Library.DAL/SInventory_DAL/CustomerDueReportDAL.cs`
- `Library.DAL/SInventory_DAL/CustomerFixedDiscountDal.cs`
- `Library.DAL/SInventory_DAL/CustomerLedgerDAL.cs`
- `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs`
- `Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs`
- `Library.DAL/SInventory_DAL/CustomerPaymentReportDal.cs`
- `Library.DAL/SInventory_DAL/CustomerReportDal.cs`
- `Library.DAL/SInventory_DAL/CustomerReturnBYMioDal.cs`
- `Library.DAL/SInventory_DAL/CustomerReturnDAL.cs`
- `Library.DAL/SInventory_DAL/CustomerWiseLedgerDal.cs`
- `Library.DAL/SInventory_DAL/CustomerWiseProductSalesDal.cs`
- `Library.DAL/SInventory_DAL/CustomerWiseReturnDal.cs`
- `Library.DAL/SInventory_DAL/CustomerWiseSalesReportDAL.cs`
- `Library.DAL/SInventory_DAL/DCStockInfoReprotDal.cs`
- `Library.DAL/SInventory_DAL/DCStockReportDAL.cs`
- `Library.DAL/SInventory_DAL/DCStoreDAL.cs`
- `Library.DAL/SInventory_DAL/DCUserPermissionDal.cs`
- `Library.DAL/SInventory_DAL/DashboardDal.cs`
- `Library.DAL/SInventory_DAL/DashboardInfoDal.cs`
- `Library.DAL/SInventory_DAL/DcWiseAreaAloDal.cs`
- `Library.DAL/SInventory_DAL/DeleteInvoiceReportDAL.cs`
- `Library.DAL/SInventory_DAL/DeliveryInvoiceReportDal.cs`
- `Library.DAL/SInventory_DAL/DeliveryPersonDAL.cs`
- `Library.DAL/SInventory_DAL/DepartmentDAL.cs`
- `Library.DAL/SInventory_DAL/DepositSlipEntryDal.cs`
- `Library.DAL/SInventory_DAL/DesignationDal.cs`
- `Library.DAL/SInventory_DAL/DestroyProductDal.cs`
- `Library.DAL/SInventory_DAL/DirectStockOutDal.cs`
- `Library.DAL/SInventory_DAL/DistributionCenterInfoDal.cs`
- `Library.DAL/SInventory_DAL/DistributionCenterStockMonitoringDal.cs`
- `Library.DAL/SInventory_DAL/DuplicatePaymentCorrectionDal.cs`
- `Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs`
- `Library.DAL/SInventory_DAL/EmployeeInformationDAL.cs`
- `Library.DAL/SInventory_DAL/ErrorInvoiceDal.cs`
- `Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs`
- `Library.DAL/SInventory_DAL/FieldForceSummeryDal.cs`
- `Library.DAL/SInventory_DAL/FixedDiscountEntryDal.cs`
- `Library.DAL/SInventory_DAL/FreezeStockReportDal.cs`
- `Library.DAL/SInventory_DAL/HREmployeeDataSyncDal.cs`
- `Library.DAL/SInventory_DAL/InvoiceBatchDAL.cs`
- `Library.DAL/SInventory_DAL/InvoiceDAL.cs`
- `Library.DAL/SInventory_DAL/InvoiceReportDal.cs`
- `Library.DAL/SInventory_DAL/MIOSalesSummeryReportDal.cs`
- `Library.DAL/SInventory_DAL/MIOSetupDal.cs`
- `Library.DAL/SInventory_DAL/MIOWiseProformaReportDAL.cs`
- `Library.DAL/SInventory_DAL/ManualOrderCreationDal.cs`
- `Library.DAL/SInventory_DAL/ManufacturerDAL.cs`
- `Library.DAL/SInventory_DAL/MarketInfoDAL.cs`
- `Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs`
- `Library.DAL/SInventory_DAL/MiaSalesReportDAL.cs`
- `Library.DAL/SInventory_DAL/MiaTargetDAL.cs`
- `Library.DAL/SInventory_DAL/MioWiseCustomerDal.cs`
- `Library.DAL/SInventory_DAL/MioWiseProductSalesDal.cs`
- `Library.DAL/SInventory_DAL/NationalBeenCaedDal.cs`
- `Library.DAL/SInventory_DAL/NationalSalesProductWiseDal.cs`
- `Library.DAL/SInventory_DAL/OrderListDAL.cs`
- `Library.DAL/SInventory_DAL/OrderStatusDal.cs`
- `Library.DAL/SInventory_DAL/OtherStockActionDAL.cs`
- `Library.DAL/SInventory_DAL/PackSizeDAL.cs`
- `Library.DAL/SInventory_DAL/PaymentReverseDal.cs`
- `Library.DAL/SInventory_DAL/PaymentTranscationDal.cs`
- `Library.DAL/SInventory_DAL/PettyCashDaL.cs`
- `Library.DAL/SInventory_DAL/ProTypeDAL.cs`
- `Library.DAL/SInventory_DAL/ProductCategoryDAL.cs`
- `Library.DAL/SInventory_DAL/ProductDAL.cs`
- `Library.DAL/SInventory_DAL/ProductDiscountDAL.cs`
- `Library.DAL/SInventory_DAL/ProductSQDAL.cs`
- `Library.DAL/SInventory_DAL/ProductUnitPriceDAL.cs`
- `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs`
- `Library.DAL/SInventory_DAL/RSMSetupDal.cs`
- `Library.DAL/SInventory_DAL/RSMWiseProductSalesDal.cs`
- `Library.DAL/SInventory_DAL/RegionDAL.cs`
- `Library.DAL/SInventory_DAL/RequisitionDAL.cs`
- `Library.DAL/SInventory_DAL/ReturnAcknowledgmentNoteDal.cs`
- `Library.DAL/SInventory_DAL/RollBackDAL.cs`
- `Library.DAL/SInventory_DAL/SCtoWHTransferDal.cs`
- `Library.DAL/SInventory_DAL/SalesAdministrationReportDal.cs`
- `Library.DAL/SInventory_DAL/SalesDashboardDal.cs`
- `Library.DAL/SInventory_DAL/SalesHubDAL.cs`
- `Library.DAL/SInventory_DAL/SalesPerformanceReportProductWise_Dal.cs`
- `Library.DAL/SInventory_DAL/SalesSummeryReportForAcc.cs`
- `Library.DAL/SInventory_DAL/SampleInvoiceDal.cs`
- `Library.DAL/SInventory_DAL/SampleIssueDal.cs`
- `Library.DAL/SInventory_DAL/ShippingCartonSizeDAL.cs`
- `Library.DAL/SInventory_DAL/SlaveApprovalDal.cs`
- `Library.DAL/SInventory_DAL/SlaveInformationDal.cs`
- `Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs`
- `Library.DAL/SInventory_DAL/StockConditionPermissionDal.cs`
- `Library.DAL/SInventory_DAL/StockTransportOrderReportDAL.cs`
- `Library.DAL/SInventory_DAL/StockUOMDAL.cs`
- `Library.DAL/SInventory_DAL/SupplierInfoDal.cs`
- `Library.DAL/SInventory_DAL/TerritoryInfoDal.cs`
- `Library.DAL/SInventory_DAL/UserCredentialChangeDal.cs`
- `Library.DAL/SInventory_DAL/UserDAL.cs`
- `Library.DAL/SInventory_DAL/UserMenuPermissionDal.cs`
- `Library.DAL/SInventory_DAL/WHBeenCardReportDal.cs`
- `Library.DAL/SInventory_DAL/WHBinCardDal.cs`
- `Library.DAL/SInventory_DAL/WHStockInReportDal.cs`
- `Library.DAL/SInventory_DAL/WHStockListDAL.cs`
- `Library.DAL/SInventory_DAL/WHStockValuationReprtDal.cs`
- `Library.DAL/SInventory_DAL/WarehousePickingDAL.cs`
- `Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs`
- `Library.DAL/SInventory_DAL/WarehouseStockInDal.cs`
- `Library.DAL/SInventory_DAL/WebServiceDal.cs`
- `Library.DAL/SInventory_DAL/WhFreezeStockReleaseDal.cs`
- `Library.DAL/SInventory_DAL/WhStockConditionFreezeDal.cs`
- `Library.DAL/SInventory_DAL/WhStockMonitoringReportDal.cs`
- `Library.DAL/SInventory_DAL/WingsSalesAndCollectionReportDal.cs`
- `Library.DAL/SInventory_DAL/WingsWiseSalesReportDal.cs`
- `Library.DAL/SInventory_DAL/ZASMonitoringDal.cs`

## Client-side assets

| Path | Contents |
|---|---|
| `Solution.Web/scripts` | 21 files: jQuery 1.7.2, jQuery UI 1.8.19, fancybox, flexslider, isotope, jcarousel, layerslider, modernizr, gmap, twitter, easing, selectnav, contact — plus `custom.js`, `datepicker.js`, `switcher.js`. All theme code; **no business logic**. |
| `Solution.Web/select2` | Select2 dropdown library. |
| `Solution.Web/crystalreportviewers13` | Crystal Reports viewer client resources (vendor). |
| `Solution.Web/css`, `PopCss`, `LoginAssets`, `HTML_Report_css` | 211 stylesheets; mostly theme. |
| `Solution.Web/images`, `fonts` | Theme and icon assets, including `~/images/lineAdd.png` and `~/images/lineDelete.png` used by every editable grid. |

Business behaviour lives in the code-behind, not in JavaScript — with the single exception of the dashboard/cash-flow/income-statement screens, which call page `[WebMethod]`s from client script.

## Files excluded from the build

Seven pages and two report artefacts carry a `.exclude` suffix, meaning they were removed from the Web Site project and are **not compiled or served**. They are not counted anywhere in this specification.

| Excluded file |
|---|
| `Solution.Web/Reports/CrystalReports/crpCustomerDueReport.cs.exclude` |
| `Solution.Web/Reports/CrystalReports/rptInvoiceForCustomer.rpt.exclude` |
| `Solution.Web/SInventory_RPTVIEW/AreaWiseMonthlySalesReportViewer.aspx.cs.exclude` |
| `Solution.Web/SInventory_RPTVIEW/AreaWiseMonthlySalesReportViewer.aspx.exclude` |
| `Solution.Web/SInventory_RPTVIEW/SalesTrendReportViewer.aspx.cs.exclude` |
| `Solution.Web/SInventory_UI/CompanyWiseBranch.aspx.cs.exclude` |
| `Solution.Web/SInventory_UI/CompanyWiseBranch.aspx.exclude` |
| `Solution.Web/SInventory_UI/CustomerDueReport.aspx.cs.exclude` |
| `Solution.Web/SInventory_UI/CustomerDueReport.aspx.exclude` |
| `Solution.Web/SInventory_UI/CustomerPayment.aspx.2.exclude` |
| `Solution.Web/SInventory_UI/CustomerPayment.aspx.cs.2.exclude` |
| `Solution.Web/SInventory_UI/MIOWiseCustomer.aspx.cs.exclude` |
| `Solution.Web/SInventory_UI/MIOWiseCustomer.aspx.exclude` |
| `Solution.Web/SInventory_UI/ManualOrderCreation.aspx.cs.exclude` |
| `Solution.Web/SInventory_UI/ManualOrderCreation.aspx.exclude` |
| `Solution.Web/SInventory_UI/WarehouseStockInEdit.aspx.cs.exclude` |
| `Solution.Web/SInventory_UI/WarehouseStockInEdit.aspx.exclude` |
