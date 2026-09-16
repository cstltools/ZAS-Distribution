# Relationships

**Ground truth now available.** This document was previously written under the false assumption that no
DB schema was reachable, so every relationship was guessed purely from how the C# code joined tables or
named columns. That assumption no longer holds: the live `SalesrollDB_ZAS` schema (213 tables) was directly
inspected — every column, every index, and every physically-enforced foreign key. This rewrite replaces the
old guesswork with that ground truth.

**Headline finding: there is exactly one (1) physically-enforced `FOREIGN KEY` constraint in the entire
213-table database.** Every other relationship below — the overwhelming bulk of this document — is
**logical only**: inferred by matching a column named e.g. `CustomerMasterId` against a table whose primary
key is a column of that same name. Nothing but the one constraint listed in §1 is enforced by SQL Server
itself; referential integrity for everything else lives entirely in application/procedure code (or nowhere).
This is a real, confirmed characteristic of this schema's design, not an extraction gap.

Method: `tables_columns.txt` (every column of every table, with PK flags) and `indexes.txt` (PK/unique/index
definitions) were extracted directly from the live database and scanned programmatically: for every non-PK
column ending in `Id` whose data type is numeric, look for a table whose *single-column primary key* has the
exact same name. A hit is listed below as a candidate logical parent -> child relationship. No physical
constraint backs any of these, so cardinality is inferred (default assumption: parent 1 : child many) and the
label **`logical (inferred from column naming), NOT enforced by a physical FOREIGN KEY constraint`** applies
to every row in §2 onward unless stated otherwise.

Cardinality note: `indexes.txt` shows no UNIQUE or PK index on any of these child-side columns anywhere in the
schema (the only UNIQUE constraint outside primary keys in the whole extract is `tblInvoice.InvoiceNo`), so
every logical relationship below defaults to **1 : N** — nothing in the physical schema suggests any of them
are 1:1.

## 1. The one physical foreign key

| Constraint | Parent (referenced) | Child (parent-side FK column) | On Delete | On Update | Cardinality |
|---|---|---|---|---|---|
| `FK__tblPettyC__Petty__1FB8AE52` | `tblPettyCashMaster.PettyCashmasterId` (PK) | `tblPettyCashDetails.PettyCashmasterId` | NO ACTION | NO ACTION | 1 : N — `PettyCashmasterId` is not part of any unique/PK index on `tblPettyCashDetails` (its own PK is `PettyCashDetailsId`), so a master can have many detail rows; deleting/updating a master row is blocked (NO ACTION) while orphan detail rows exist. Standard master/detail petty-cash ledger. |

This is the *only* row in `sys.foreign_keys` for the whole database. Every relationship from here down is
a naming-convention inference, not a constraint SQL Server enforces.

## 2. Logical relationships by subject area

Format: **Parent table (PK column)** followed by every child table/column found to reference it by exact
column-name match. All of these are `logical (inferred from column naming), NOT enforced by a physical
FOREIGN KEY constraint` and default to 1 : N cardinality (see note above). Tables the extraction found with
**no primary key at all** are marked — a same-named "PK-shaped" column there is not a real target, it just
happens to be first in the table, so those cases are called out explicitly rather than treated as a hit.

### Identity & security

| Parent (PK) | Children (col) referencing it | Notes |
|---|---|---|
| `tblAction.ActionId` | `tblActionStepDetail.ActionId`; `tblActionUserWiseApproval.ActionId` | AMBIGUOUS — PK name `ActionId` is also the PK of `tblActionStatus`; a child column named `ActionId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblActionStatus.ActionId` | `tblActionStepDetail.ActionId`; `tblActionUserWiseApproval.ActionId` | AMBIGUOUS — PK name `ActionId` is also the PK of `tblAction`; a child column named `ActionId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblActionSteps.ASId` | `tblActionPageWiseStep.ASId`; `tblActionStepDetail.ASId` |  |
| `tblRoleType.RoleTypeId` | `tbl_UserRoleInfo.RoleTypeId` |  |
| `tblUser.UserId` | `tblActionUserWiseApproval.UserId`; `tblInvoice.UserId`; `tblLoginLog.UserId`; `tblMenuDistribution.UserId`; `tblPaymentReverseDELInvoice.UserId`; `tblStockConditionPermission.UserId`; `tblTagASMRSMInfo.UserId`; `tblUserCompanyUnit.UserId`; `tblUserDeviceToken.UserId`; `tblUserLog.UserId`; `tbl_Notice_UserDetail.UserId` |  |
| `tblUserType.UserTypeId` | `tblUser.UserTypeId`; `tblUserLog.UserTypeId` |  |
| `tbl_UserRoleInfo.UserRoleID` | `tblMarketAttendance_Master_webapi.UserRoleID`; `tblMarketAttendance_Master_webapi_backup.UserRoleID`; `tblMarketStationDetail.UserRoleID`; `tblTagASMRSMInfo.UserRoleId`; `tblUser.UserRoleID`; `tblUserLog.UserRoleID` |  |

### Customer / geography / field-force hierarchy

| Parent (PK) | Children (col) referencing it | Notes |
|---|---|---|
| `tblASMInfo.ASMId` | `tblOrder.ASMId`; `tblSampleIssue.ASMId` |  |
| `tblArea.AreaId` | `tblASMInfo.AreaId`; `tblDcWiseAreaInfo.AreaId`; `tblMarketAttendance_Master_webapi.AreaId`; `tblMarketAttendance_Master_webapi_backup.AreaId`; `tblOrder.AreaId`; `tblSampleIssue.AreaId`; `tblTerritory.AreaId`; `tbl_DCRCustomerInfo.AreaId`; `tbl_DCRInfo.AreaId`; `tbl_DoctorTourPlanDetail.AreaId`; `tbl_Notice_MarketDetails.AreaId`; `tbl_PrescriptionMaster.AreaId`; `tbl_TourPlanInfo.AreaId`; `tbl_UserTracking.AreaId` |  |
| `tblCustCategory.CategoryId` | `tblCustMaster.CategoryId` | AMBIGUOUS — PK name `CategoryId` is also the PK of `tblDoctorCategory`, `tblProCategory`; a child column named `CategoryId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblCustMaster.CustomerMasterId` | `Table_DueInvoice.CustomerMasterId`; `tblCreditAdjustment.CustomerMasterId`; `tblCustomerCreditLimit.CustomerMasterId`; `tblCustomerPay.CustomerMasterId`; `tblCustomerPayDeleteLog.CustomerMasterId`; `tblInvoice.CustomerMasterId`; `tblNonTranscationalInvoiceMaster.CustomerMasterId`; `tblOrder.CustomerMasterId`; `tblPaymentReverseDELInvoice.CustomerMasterId`; `tblPaymentTranscationMaster.CustomerMasterId`; `tblProductDiscount.CustomerMasterId`; `tblProductQuotedPrice.CustomerMasterId`; `tblSampleIssue.CustomerMasterId`; `tblSampleIssueTranscation.CustomerMasterId`; `tbl_TEMP_InvoiceWiseCustomerDue.CustomerMasterId`; `tbl_TourPlanInfo.CustomerMasterId` |  |
| `tblDeliveryManInfo.DeliveryManId` | `PaymentBachMaster.DeliveryManId`; `tblInvoice.DeliveryManId`; `tblInvoiceBatch.DeliveryManId`; `tblPaymentReverseDELInvoice.DeliveryManId` |  |
| `tblGroupInformation.GroupId` | `tblCompanyInfo.GroupId`; `tblMarketAttendance_Master_webapi.GroupId`; `tblMarketAttendance_Master_webapi_backup.GroupId`; `tblNSMInfo.GroupId`; `tblRegion.GroupId`; `tbl_DCRCustomerInfo.GroupId`; `tbl_DCRInfo.GroupId`; `tbl_DoctorTourPlanDetail.GroupId`; `tbl_PrescriptionMaster.GroupId`; `tbl_TourPlanInfo.GroupId`; `tbl_UserTracking.GroupId` | AMBIGUOUS — PK name `GroupId` is also the PK of `tbl_Group`, `tblProductGroup`; a child column named `GroupId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblMIOInfo.MIOId` | `tblGroupWisePromoQty.MIOId`; `tblMarketAttendance_Master_webapi.MIOId`; `tblMarketAttendance_Master_webapi_backup.MIOId`; `tblOrder.MIOId`; `tblSampleIssue.MIOId` |  |
| `tblMarket.MarketId` | `Table_CustomerUpload.MarketId`; `Table_DueInvoice.MarketId`; `tblCustMaster.MarketId`; `tblDoctorMarketDetail.MarketId`; `tblDoctorMaster.MarketId`; `tblMarketStationDetail.MarketId`; `tblOrder.MarketId`; `tblSampleIssue.MarketId`; `tblTempCustomer.MarketId`; `tbl_DCRCustomerInfo.MarketId`; `tbl_DCRInfo.MarketId`; `tbl_DoctorTourPlanDetail.MarketId`; `tbl_Notice_MarketDetails.MarketId`; `tbl_SubMarket.MarketId`; `tbl_TourPlanInfo.MarketId` |  |
| `tblRSMInfo.RSMId` | `tblOrder.RSMId`; `tblSampleIssue.RSMId` |  |
| `tblRegion.RegionId` | `tblArea.RegionId`; `tblMarketAttendance_Master_webapi.RegionId`; `tblMarketAttendance_Master_webapi_backup.RegionId`; `tblOrder.RegionId`; `tblRSMInfo.RegionId`; `tblSampleIssue.RegionId`; `tbl_DCRCustomerInfo.RegionId`; `tbl_DCRInfo.RegionId`; `tbl_DoctorTourPlanDetail.RegionId`; `tbl_Notice_MarketDetails.RegionId`; `tbl_PrescriptionMaster.RegionId`; `tbl_TourPlanInfo.RegionId`; `tbl_UserTracking.RegionId` |  |
| `tblTerritory.TerritoryId` | `tblGroupWisePromoQty.TerritoryId`; `tblMIOInfo.TerritoryId`; `tblMarket.TerritoryId`; `tblMarketAttendance_Master_webapi.TerritoryId`; `tblMarketAttendance_Master_webapi_backup.TerritoryId`; `tblOrder.TerritoryId`; `tblSampleIssue.TerritoryId`; `tbl_DCRCustomerInfo.TerritoryId`; `tbl_DCRInfo.TerritoryId`; `tbl_DoctorTourPlanDetail.TerritoryId`; `tbl_Notice_MarketDetails.TerritoryId`; `tbl_TourPlanInfo.TerritoryId`; `tbl_UserTracking.TerritoryId` |  |
| `tbl_Group.GroupId` | `tblCompanyInfo.GroupId`; `tblMarketAttendance_Master_webapi.GroupId`; `tblMarketAttendance_Master_webapi_backup.GroupId`; `tblNSMInfo.GroupId`; `tblRegion.GroupId`; `tbl_DCRCustomerInfo.GroupId`; `tbl_DCRInfo.GroupId`; `tbl_DoctorTourPlanDetail.GroupId`; `tbl_PrescriptionMaster.GroupId`; `tbl_TourPlanInfo.GroupId`; `tbl_UserTracking.GroupId` | AMBIGUOUS — PK name `GroupId` is also the PK of `tblGroupInformation`, `tblProductGroup`; a child column named `GroupId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tbl_SubMarket.SMId` | `tbl_DoctorTourPlanDetail.SMId`; `tbl_TourPlanInfo.SMId` |  |

### Doctor / medical-rep (DCR) & field marketing

| Parent (PK) | Children (col) referencing it | Notes |
|---|---|---|
| `tblDoctorCategory.CategoryId` | `tblCustMaster.CategoryId` | AMBIGUOUS — PK name `CategoryId` is also the PK of `tblCustCategory`, `tblProCategory`; a child column named `CategoryId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblDoctorChemberDetail.ChemberId` | `tbl_DCRInfo.ChemberId`; `tbl_PrescriptionMaster.ChemberId` |  |
| `tblDoctorDegree.DegreeId` | `tblDoctorMaster.DegreeId` |  |
| `tblDoctorMaster.DoctorId` | `tblDoctorBrandDetail.DoctorId`; `tblDoctorChemberDetail.DoctorId`; `tblDoctorContactDetail.DoctorId`; `tblDoctorCustomerDetail.DoctorId`; `tblDoctorDegreeDetail.DoctorId`; `tblDoctorInstitutionDetail.DoctorId`; `tblDoctorMarketDetail.DoctorId`; `tblDoctorProgramTypeDetail.DoctorId`; `tblDoctorSpecialDayDetail.DoctorId`; `tblDoctorSpecialityDetail.DoctorId`; `tblDoctor_CustomerDetail.DoctorId`; `tbl_DCRInfo.DoctorId`; `tbl_DoctorTourPlanDetail.DoctorId`; `tbl_PrescriptionMaster.DoctorId` |  |
| `tblDoctorProgramType.ProgramTypeId` | `tblCustMaster.ProgramTypeId`; `tblDoctorMaster.ProgramTypeId`; `tblDoctorProgramTypeDetail.ProgramTypeId` | AMBIGUOUS — PK name `ProgramTypeId` is also the PK of `tblProgramType`; a child column named `ProgramTypeId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblDoctorSpecialDay.SpecialDayId` | `tblDoctorSpecialDayDetail.SpecialDayId` |  |
| `tblDoctorType.DoctorTypeId` | `tblDoctorDegree.DoctorTypeId`; `tblDoctorMaster.DoctorTypeId` |  |
| `tblInstitutionInfo.InstitutionId` | `tblDoctorInstitutionDetail.InstitutionId` |  |
| `tblMarketAttendance_Master_webapi.AttendanceId` | `tblMarketAttendace_Tracking_webapi.AttendanceId` | AMBIGUOUS — PK name `AttendanceId` is also the PK of `tblMarketAttendance_Master_webapi_backup`; a child column named `AttendanceId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblMarketAttendance_Master_webapi_backup.AttendanceId` | `tblMarketAttendace_Tracking_webapi.AttendanceId` | AMBIGUOUS — PK name `AttendanceId` is also the PK of `tblMarketAttendance_Master_webapi`; a child column named `AttendanceId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblNonEffectiveReason.ReasonId` | `tbl_DCRInfo.ReasonId` |  |
| `tblProgramType.ProgramTypeId` | `tblCustMaster.ProgramTypeId`; `tblDoctorMaster.ProgramTypeId`; `tblDoctorProgramTypeDetail.ProgramTypeId` | AMBIGUOUS — PK name `ProgramTypeId` is also the PK of `tblDoctorProgramType`; a child column named `ProgramTypeId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblSMCType.SMCTypeId` | `tblCustMaster.SMCTypeId`; `tblDoctorMaster.SMCTypeId` |  |
| `tbl_ContactType.ContactTypeId` | `tblDoctorContactDetail.ContactTypeId` |  |
| `tbl_DCRInfo.DcrId` | `tbl_DcrBrandDetails.DcrId`; `tbl_DcrDetails.DcrId`; `tbl_DcrVisitedWithDetails.DcrId` |  |
| `tbl_DoctorTourPlanDetail.DocTPDetailsId` | `tbl_DCRInfo.DocTPDetailsId` |  |
| `tbl_Notice_MarketMaster.NoticeId` | `tbl_Notice_MarketDetails.NoticeId` |  |
| `tbl_PrescriptionMaster.PrescriptionId` | `tbl_PrescriptionProductDetail.PrescriptionId` |  |
| `tbl_PrescriptionType.PrescriptionTypeId` | `tbl_PrescriptionMaster.PrescriptionTypeId` |  |
| `tbl_TourPlanPurpose.TPId` | `tbl_DoctorTourPlanDetail.TPId`; `tbl_TourPlanInfo.TPId` |  |
| `tbl_TourPlanType.TourTypeId` | `tbl_DCRInfo.TourTypeId`; `tbl_DoctorTourPlanDetail.TourTypeId`; `tbl_TourPlanInfo.TourTypeId` |  |

### Order -> invoice -> delivery -> return

| Parent (PK) | Children (col) referencing it | Notes |
|---|---|---|
| `tblChalanDetail.ChalanDetailsId` | `tblDCStore.ChalanDetailsId`; `tblDCStoreFreeze.ChalanDetailsId`; `tblDCStore_OpeningBalance.ChalanDetailsId` |  |
| `tblChalanInfo.ChalanId` | `tblChalanDetail.ChalanId` |  |
| `tblCustomerReturnDetail.CustReturnDetailId` | `tblDCStoreFreeze.CustReturnDetailId` |  |
| `tblCustomerReturnMaster.CustReturnId` | `tblCustomerReturnDetail.CustReturnId` |  |
| `tblInvoice.InvoiceId` | `Table_DueInvoice.InvoiceId`; `Table_IVF.InvoiceId`; `Table_Temp_CustPayDetails.InvoiceId`; `tblCreditAdjustment.InvoiceId`; `tblCustPayDetail.InvoiceId`; `tblCustPayDetailDeleteLog.InvoiceId`; `tblCustPayForLastcursor.InvoiceId`; `tblCustomerReturnMaster.InvoiceId`; `tblInvoiceDeleteLog.InvoiceId`; `tblInvoiceDetail.InvoiceId`; `tblInvoiceDetail_DeleterRecord.InvoiceId`; `tblNonTranscationalInvoiceMaster.InvoiceId`; `tblPaymentReverseDELInvoice.InvoiceId`; `tblPaymentTranscationDetail.InvoiceId`; `tblProInvoiceReturnTrack.InvoiceId` |  |
| `tblInvoiceBatch.BatchId` | `tblCustomerPay.BatchId`; `tblInvoice.BatchId`; `tblMoneyReceipt.BatchId`; `tblPaymentReverseDELInvoice.BatchId` |  |
| `tblInvoiceDetail.InvoiceDetailId` | `tblCustomerReturnDetail.InvoiceDetailId`; `tblDCStoreFreeze.InvoiceDetailId`; `tblInvoiceDetail_DeleterRecord.InvoiceDetailId`; `tblProInvoiceReturnTrack.InvoiceDetailId`; `tbl_INV_BatchWiseStock.InvoiceDetailId` |  |
| `tblNonTranscationalInvoiceMaster.InvReturnMasterID` | `tblNonTranscationalInvoiceDetail.InvReturnMasterID` |  |
| `tblOrder.OrderId` | `tblInvoice.OrderId`; `tblOrderDel.OrderId`; `tblOrderDetail.OrderId`; `tblOrderDetailDel.OrderId`; `tblPaymentReverseDELInvoice.OrderId`; `tblSampleIssueDetail.OrderId` | AMBIGUOUS — PK name `OrderId` is also the PK of `tblSampleIssue`; a child column named `OrderId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblOrderDetail.OrderDetailId` | `tblOrderDetailDel.OrderDetailId` | AMBIGUOUS — PK name `OrderDetailId` is also the PK of `tblSampleIssueDetail`; a child column named `OrderDetailId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblSampleIssue.OrderId` | `tblInvoice.OrderId`; `tblOrderDel.OrderId`; `tblOrderDetail.OrderId`; `tblOrderDetailDel.OrderId`; `tblPaymentReverseDELInvoice.OrderId`; `tblSampleIssueDetail.OrderId` | AMBIGUOUS — PK name `OrderId` is also the PK of `tblOrder`; a child column named `OrderId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblSampleIssueDetail.OrderDetailId` | `tblOrderDetailDel.OrderDetailId` | AMBIGUOUS — PK name `OrderDetailId` is also the PK of `tblOrderDetail`; a child column named `OrderDetailId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |

### Stock, warehouse & DC/central store

| Parent (PK) | Children (col) referencing it | Notes |
|---|---|---|
| `tblCentralStore.ReceiveId` | `tblCentralStore_OpeninigBalance.ReceiveId`; `tblDCStore.ReceiveId`; `tblDCStoreFreeze.ReceiveId`; `tblStockConditionFreeze.ReceiveId`; `tblStockInTransfar.ReceiveId`; `tblWHStockOutDetail.ReceiveId`; `tblWhStockConditionFreeze.ReceiveId`; `tblWhStoreFreeze.ReceiveId` |  |
| `tblCompanyUnit.ComUnitId` | `tblDCStore.ComUnitId`; `tblDCStoreFreeze.ComUnitId`; `tblDCStore_OpeningBalance.ComUnitId`; `tblInvoice.ComUnitId`; `tblNonTranscationalInvoiceMaster.ComUnitId`; `tblOrder.ComUnitId`; `tblOrderDel.ComUnitId`; `tblPaymentReverseDELInvoice.ComUnitId`; `tblRequisition.ComUnitId`; `tblSampleIssue.ComUnitId`; `tblSampleIssueTranscation.ComUnitId` |  |
| `tblDCStore.DCStoreId` | `tblChalanDetail.DCStoreId`; `tblDCStoreFreeze.DCStoreId`; `tblDCStore_OpeningBalance.DCStoreId`; `tblDirectStockOut.DCStoreId`; `tblInvoiceDetail.DCStoreId`; `tblInvoiceDetail_DeleterRecord.DCStoreId`; `tblProInvoiceReturnTrack.DCStoreId`; `tblSampleIssueTranscation.DCStoreId`; `tblStockConditionFreeze.DCStoreId`; `tbl_INV_BatchWiseStock.DCStoreId` |  |
| `tblDCStoreFreeze.DCStoreFreezeId` | `tbl_INV_BatchWiseStock.DCStoreFreezeId` |  |
| `tblDestroyProduct.DestroyId` | `tbl_INV_BatchWiseStock.DestroyId` |  |
| `tblDistributionType.DcTypeId` | `tblCompanyUnit.DcTypeId` |  |
| `tblRequisition.ReqId` | `tblDCStore.ReqId`; `tblDCStoreFreeze.ReqId`; `tblDCStore_OpeningBalance.ReqId`; `tblRequsitionChild.ReqId`; `tblStockInTransfar.ReqId` |  |
| `tblRequsitionChild.ReqChildId` | `tblDCStore.ReqChildId`; `tblDCStoreFreeze.ReqChildId`; `tblDCStore_OpeningBalance.ReqChildId`; `tblStockInTransfar.ReqChildId` |  |
| `tblStockCondition.StockConId` | `tblStockConditionPermission.StockConId` |  |
| `tblStockConditionFreeze.StockConditionFreezeID` | `tblDCStoreFreeze.StockConditionFreezeID` |  |
| `tblStockInTransfar.StockInTransfarId` | `tblDCStore.StockInTransfarId`; `tblDCStoreFreeze.StockInTransfarId`; `tblDCStore_OpeningBalance.StockInTransfarId`; `tbl_INV_Central_BatchWiseStock.StockInTransfarId` |  |
| `tblSupplierInformation.SupplierId` | `tblWHStockInMaster.SupplierId`; `tblWHStockInMasterDeleteLog.SupplierId` |  |
| `tblWHStockInDetail.WHStockInDetailID` | `tblWHStockInDetailDeleteLog.WHStockInDetailID`; `tblWHStockOutDetail.WHStockInDetailID`; `tbl_INV_Central_BatchWiseStock.WHStockInDetailId` |  |
| `tblWHStockInMaster.WHStockInMasterID` | `tblWHStockInDetail.WHStockInMasterID`; `tblWHStockInDetailDeleteLog.WHStockInMasterID`; `tblWHStockInMasterDeleteLog.WHStockInMasterID`; `tblWHStockOutMaster.WHStockInMasterID` |  |
| `tblWHStockOutMaster.WHStockOutMasterID` | `tblWHStockOutDetail.WHStockOutMasterID` |  |
| `tblWhStockConditionFreeze.WhStockConditionFreezeID` | `tblWhStoreFreeze.WhStockConditionFreezeID` |  |
| `tbl_INV_CentralStore_WeightedAvg.Weighted_Avg_Id` | `tbl_INV_BatchWiseStock.Weighted_Avg_Id`; `tbl_INV_Central_BatchWiseStock.Weighted_Avg_Id` | AMBIGUOUS — PK name `Weighted_Avg_Id` is also the PK of `tbl_INV_WeightedAvg`; a child column named `Weighted_Avg_Id` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tbl_INV_WeightedAvg.Weighted_Avg_Id` | `tbl_INV_BatchWiseStock.Weighted_Avg_Id`; `tbl_INV_Central_BatchWiseStock.Weighted_Avg_Id` | AMBIGUOUS — PK name `Weighted_Avg_Id` is also the PK of `tbl_INV_CentralStore_WeightedAvg`; a child column named `Weighted_Avg_Id` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |

### Payment & finance

| Parent (PK) | Children (col) referencing it | Notes |
|---|---|---|
| `PaymentBachMaster.BatchMasterId` | `PaymentBatchDetails.BatchMasterId` |  |
| `tblBankInfo.BankId` | `tblCompanyWiseDeposit.BankId` |  |
| `tblCustPayDetail.CustPayDetailId` | `tblCustPayDetailDeleteLog.CustPayDetailId` | AMBIGUOUS — PK name `CustPayDetailId` is also the PK of `tblPaymentTranscationDetail`; a child column named `CustPayDetailId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblCustomerPay.CustPayId` | `Table_Temp_CustPayDetails.CustPayId`; `tblCustPayDetail.CustPayId`; `tblCustPayDetailDeleteLog.CustPayId`; `tblPaymentTranscationDetail.CustPayId` | AMBIGUOUS — PK name `CustPayId` is also the PK of `tblCustomerPayDeleteLog`, `tblPaymentTranscationMaster`; a child column named `CustPayId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblCustomerPayDeleteLog.CustPayId` | `Table_Temp_CustPayDetails.CustPayId`; `tblCustPayDetail.CustPayId`; `tblCustPayDetailDeleteLog.CustPayId`; `tblPaymentTranscationDetail.CustPayId` | AMBIGUOUS — PK name `CustPayId` is also the PK of `tblCustomerPay`, `tblPaymentTranscationMaster`; a child column named `CustPayId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblPaymentTranscationDetail.CustPayDetailId` | `tblCustPayDetailDeleteLog.CustPayDetailId` | AMBIGUOUS — PK name `CustPayDetailId` is also the PK of `tblCustPayDetail`; a child column named `CustPayDetailId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblPaymentTranscationMaster.CustPayId` | `Table_Temp_CustPayDetails.CustPayId`; `tblCustPayDetail.CustPayId`; `tblCustPayDetailDeleteLog.CustPayId`; `tblPaymentTranscationDetail.CustPayId` | AMBIGUOUS — PK name `CustPayId` is also the PK of `tblCustomerPay`, `tblCustomerPayDeleteLog`; a child column named `CustPayId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblPaymentType.PaymentTypeId` | `tblInvoice.PaymentTypeId`; `tblPaymentReverseDELInvoice.PaymentTypeId` |  |
| `tblPettyCashMaster.PettyCashmasterId` | `tblPettyCashDetails.PettyCashmasterId` |  |

### Product / pricing / promotion

| Parent (PK) | Children (col) referencing it | Notes |
|---|---|---|
| `Table_CPM_BonusCampaignMaster.CampgainMasterId` | `Table_CPM_BonusCampaignDetail.CampgainMasterId`; `tblBonusCampgainDetail.CampgainMasterId` | AMBIGUOUS — PK name `CampgainMasterId` is also the PK of `tblBonusCampaignMaster`; a child column named `CampgainMasterId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblBonusCampaignMaster.CampgainMasterId` | `Table_CPM_BonusCampaignDetail.CampgainMasterId`; `tblBonusCampgainDetail.CampgainMasterId` | AMBIGUOUS — PK name `CampgainMasterId` is also the PK of `Table_CPM_BonusCampaignMaster`; a child column named `CampgainMasterId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblGroupWisePromoQty.GWPromoQtyId` | `tbl_DcrDetails.GWPromoQtyId` |  |
| `tblIngridents.IngridentsId` | `tblProductSQ.IngridentsId` |  |
| `tblManufacturer.ManufacId` | `tblChalanInfo.ManufacId`; `tblOrder.ManufacId`; `tblOrderDel.ManufacId`; `tblProduct.ManufacId`; `tblSampleIssue.ManufacId`; `tblStockConditionFreeze.ManufacId`; `tblTradePolicyNew.ManufacId` |  |
| `tblPackSize.PackSizeId` | `tblProduct.PackSizeId` |  |
| `tblProCategory.CategoryId` | `tblCustMaster.CategoryId` | AMBIGUOUS — PK name `CategoryId` is also the PK of `tblCustCategory`, `tblDoctorCategory`; a child column named `CategoryId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblProType.ProTypeId` | `tblProduct.ProTypeId` |  |
| `tblProduct.ProductId` | `tblCentralStore.ProductId`; `tblCentralStore_OpeninigBalance.ProductId`; `tblGroupWisePromoQty.ProductId`; `tblMIATargetProductWise.ProductId`; `tblNonTranscationalInvoiceDetail.ProductId`; `tblOrderDetail.ProductId`; `tblOrderDetailDel.ProductId`; `tblProductDiscount.ProductId`; `tblProductQuotedPrice.ProductId`; `tblProductUpload.ProductId`; `tblSampleIssueDetail.ProductId`; `tblUnitPrice.ProductId`; `tblWHStockInDetail.ProductId`; `tblWHStockInDetailDeleteLog.ProductId`; `tblWHStockOutDetail.ProductId`; `tblWhStoreFreeze.ProductId`; `tbl_DcrDetails.ProductId`; `tbl_MC_ProductRawMaterialDetails.ProductId`; `tbl_PrescriptionProductDetail.ProductId` |  |
| `tblProductCase.CaseId` | `tblProduct.CaseId` |  |
| `tblProductGroup.GroupId` | `tblCompanyInfo.GroupId`; `tblMarketAttendance_Master_webapi.GroupId`; `tblMarketAttendance_Master_webapi_backup.GroupId`; `tblNSMInfo.GroupId`; `tblRegion.GroupId`; `tbl_DCRCustomerInfo.GroupId`; `tbl_DCRInfo.GroupId`; `tbl_DoctorTourPlanDetail.GroupId`; `tbl_PrescriptionMaster.GroupId`; `tbl_TourPlanInfo.GroupId`; `tbl_UserTracking.GroupId` | AMBIGUOUS — PK name `GroupId` is also the PK of `tbl_Group`, `tblGroupInformation`; a child column named `GroupId` could logically reference any of them. UNKNOWN — HUMAN VERIFICATION REQUIRED to confirm which. |
| `tblProductSQ.ProductBrandId` | `tblProduct.ProductBrandId` |  |

### HR / employee

| Parent (PK) | Children (col) referencing it | Notes |
|---|---|---|
| `tblEmpGeneralInfo.EmpInfoId` | `tblDeliveryManInfo.EmpInfoId`; `tblEmpLatLong.EmpInfoId`; `tblGroupWisePromoQty.EmpInfoId`; `tblLeaveAvail.EmpInfoId`; `tblLeaveInventory.EmpInfoId`; `tblMarketAttendance_Master_webapi.EmpInfoId`; `tblMarketAttendance_Master_webapi_backup.EmpInfoId`; `tblSynchronizationInfo.EmpInfoId`; `tblTagASMRSMInfo.EmpInfoId`; `tblUser.EmpInfoId`; `tblUserDeviceToken.EmpInfoId`; `tblUserLog.EmpInfoId`; `tbl_DCRCustomerInfo.EmpInfoId`; `tbl_DcrDetails.EmpInfoId`; `tbl_DcrVisitedWithDetails.EmpInfoId`; `tbl_DoctorTourPlanDetail.EmpInfoId`; `tbl_DoctorTourPlanMaster.EmpInfoId`; `tbl_Notification.EmpInfoId`; `tbl_TourPlanInfo.EmpInfoId`; `tbl_TourPlanMaster.EmpInfoId`; `tbl_UserTracking.EmpInfoId` |  |
| `tbl_Shift.ShiftId` | `tblEmpGeneralInfo.ShiftId`; `tblEmpLatLong.ShiftId`; `tblMarketAttendance_Master_webapi.ShiftId`; `tblMarketAttendance_Master_webapi_backup.ShiftId`; `tbl_DoctorTourPlanDetail.ShiftId`; `tbl_TourPlanInfo.ShiftId` |  |

### Other masters (not grouped above)

| Parent (PK) | Children (col) referencing it | Notes |
|---|---|---|
| `tblStockUOM.StockUOMId` | `tblProduct.StockUOMId` |  |

### Known false positive: the `Employee` table

`tblASMInfo.EmployeeId`, `tblMIOInfo.EmployeeId`, `tblNSMInfo.EmployeeId`, `tblRSMInfo.EmployeeId` name-match
the PK of a table called `Employee` (`EmployeeID`) — but `Employee` has only `FirstName`/`LastName`/
`Department`/`Position`/`Salary` columns, which do not match this application's naming or domain conventions
anywhere else in the schema. This is almost certainly an unrelated leftover/tutorial table, not the real
target. The real field-force employee master in this schema is `tblEmpGeneralInfo` (PK `EmpInfoId`), which
uses a *different* column name (`EmpInfoId`, not `EmployeeId`) everywhere it's actually referenced (see the
HR / employee section). **UNKNOWN — HUMAN VERIFICATION REQUIRED**: `EmployeeId` on those four tables may
reference `tblEmpGeneralInfo` by a different implicit key (e.g. `EmpMasterCode`), or an external HR system id
(see `ZASHRIS` cross-database note below) — not the `Employee` demo table.

### Notable orphan `...Id` columns with no matching single-column PK in this schema

These recur across many tables and are clearly meant as foreign keys by convention, but no table in the
213-table extract has a *single-column primary key* of the exact matching name — either because the natural
target table has **no PK constraint at all** (confirmed for the ones listed below) or because the column
refers to a cross-database table (see §3). All: **UNKNOWN — HUMAN VERIFICATION REQUIRED.**

| Orphan column (seen on) | Likely intended target | Why unmatched |
|---|---|---|
| `CompanyId` (`tblCompanyUnit`, `tblProduct`, `tblEmpGeneralInfo`, `tblASMInfo`, `tblRSMInfo`, `tblNSMInfo`, `tblMIOInfo`, `tblCreditAdjustment`, `tblRequisition`, `tblInvoiceBatch`, `tbl_DCRInfo`, `tbl_TourPlanInfo`, `tbl_UserTracking`, and more) | `tblCompanyInfo.CompanyId` | `tblCompanyInfo.CompanyId` is `IDENTITY` but **not marked as a PK** in the extract — the table has no primary key constraint at all |
| `WarehouseId`/`WearhouseId` (`tblCentralStore`, `tblRequisition`, `tblWHStockInMaster`, `tblWhStockConditionFreeze`, etc.) | `tblWearhouse.WearhouseId` | `tblWearhouse.WearhouseId` is not marked as a PK — no PK constraint on that table |
| `DesignationId` (`tblEmpGeneralInfo`, `tblDoctorMaster`, `tblMarketStationDetail`) | `tblDesignation.DesignationId` or `tblDoctorDesignation.DesignationId` | Neither table has a PK constraint; also ambiguous which of the two (general HR designation vs. doctor-facing designation) is meant |
| `GroupId` (`tblCompanyInfo`, `tblMarketAttendance_Master_webapi(_backup)`, `tblNSMInfo`, `tblRegion`, `tbl_DCRCustomerInfo`, `tbl_DCRInfo`, `tbl_DoctorTourPlanDetail`, `tbl_PrescriptionMaster`, `tbl_TourPlanInfo`, `tbl_UserTracking`) | ambiguous between `tblGroupInformation.GroupId`, `tblProductGroup.GroupId`, `tbl_Group.GroupId` | three separate tables share PK name `GroupId` — see ambiguity note in §2 |
| `CategoryId` (`tblCustMaster`) | ambiguous between `tblCustCategory.CategoryId`, `tblDoctorCategory.CategoryId`, `tblProCategory.CategoryId` | three tables share PK name `CategoryId` — see ambiguity note in §2 |
| `BankAccId` / `CashAccId` (`tblCustPayDetail`, `tblPaymentTranscationDetail`, `tblPettyCashMaster`, `Table_Temp_CustPayDetails`) | an account/GL master, likely in `ZAS_ACCDB` | no table named like a bank/cash account master exists in `SalesrollDB_ZAS`; almost certainly a three-part-name reference into the accounting DB |
| `MiaId` / `UnitId` (`tblInvoice`, `Table_DueInvoice`, `Table_IVF`, `tblPaymentReverseDELInvoice`, `tblMIATarget`) | `tblMIAInfo` (referenced in the old doc) | no table literally named `tblMIAInfo` was found in this extract — `tblMIOInfo` (Medical Information Officer?) exists instead; naming drift between `MIA`/`MIO` needs a human to confirm which role table (if any) this really is |
| `DesignationId`/`SoftwareUseId`/`SubTerritoryId`/`DoctorProgramypeId` (various DCR/tour-plan tables) | unclear | no matching PK found anywhere in the extract; likely typos/legacy columns (`DoctorProgramypeId` looks like a misspelling of `DoctorProgramTypeId`) or dead columns |

## 3. Cross-database (three-part name) relationships

**UNKNOWN — HUMAN VERIFICATION REQUIRED (only `SalesrollDB_ZAS` was directly inspected in this pass; the
databases below were not queried).** Carried over from the previous version of this document, which inferred
these from stored-procedure parameter lists (procedures accepting ids from both databases together). Left
largely as-is because no live access to `ZAS_ACCDB`, `CSTL_ACCDB`, `CSTL_ERPDB`, or `ZASHRIS` was available
for this rewrite.

| Relationship | Basis (INFERRED, unverified) |
|---|---|
| `tblCustMaster` (SalesrollDB_ZAS) <-> `ZAS_ACCDB..tblSubsidiaryLedger` | `sp_I_CustomerGl`/`sp_Integration_CustomerSubsidaryInsert` take both `@CustomerId` and `@SubsidiaryLId` |
| `tblSupplierInformation` <-> `ZAS_ACCDB` ledger tables | `sp_I_SupplierGl` takes `@SupplierId` and `@SubsidiaryLId` |
| `tblEmpGeneralInfo` (SalesrollDB_ZAS) <-> `ZASHRIS..tblEmpGeneralInfo` | `sp_Employee_EmployeeListSyncedWithDistibution`/`sp_Employee_EmployeeCompanySyncedWithDistibution` take `@EmployeeMasterCode` and sync by it |
| `tblCompanyUnit` (SalesrollDB_ZAS) <-> `CSTL_ERPDB..tblCompanyUnit` | `sp_Common_GetUnitWiseLocation_FromErp` takes `@CompanyId` scoped to the ERP catalog, paired with `sp_Common_GetUnitWiseLocation_Zas` for the local one |
| `tblTradeMaterialTransfer` <-> `CSTL_ERPDB..tblItemDescription` / `tbl_MC_*` | `TradeMaterialTransferDal` calls both this app's tables and ERP three-part-name material-conversion tables in the same transfer workflow; also note `tblTradeMaterialTransfer.ToItemDescriptionId`/`FromCompanyId`/`ToCompanyId` have no matching PK inside `SalesrollDB_ZAS`, consistent with them pointing at `CSTL_ERPDB` |
| `BankAccId`/`CashAccId` columns (see §2 orphan table) <-> `ZAS_ACCDB` chart-of-accounts | Inferred from column naming only, not from a specific procedure this pass looked at |

## Summary

- **1** physical foreign key (§1).
- **387** logical (naming-convention-inferred, unenforced) parent -> child relationships documented across §2 (covering 99 distinct parent/master tables).
- **176** additional `...Id`-shaped columns found with no matching single-column PK anywhere in the 213-table extract; the business-significant ones are itemized in the orphan-column table above, the full raw list is available in the extraction scratch file for follow-up.
- **17** PK column names are shared by more than one table, making every column that matches them genuinely ambiguous (flagged inline in §2).
- Cross-database relationships (§3) remain **UNKNOWN — HUMAN VERIFICATION REQUIRED**; no live access to those databases was available for this rewrite.
