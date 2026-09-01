# Integrations

Every integration in this application is **database-to-database**, inside one SQL Server instance, using
three-part names or a second connection string. There is **no message queue, no HTTP client, no file drop, no
scheduled job and no outbound web service call anywhere in the codebase** — verified by the absence of
`HttpClient`, `WebRequest`, `WebClient`, `SmtpClient`, `ServiceReference`, `System.IO.File` writes to an
interface folder, and any `Quartz`/`Hangfire`/`TaskScheduler` reference.

## Map

```
                    SalesrollDB_ZAS   (the application's own database)
                            │
   ┌────────────────────────┼─────────────────────────┬──────────────────────┐
   │                        │                         │                      │
   ▼                        ▼                         ▼                      ▼
ZAS_ACCDB              CSTL_ACCDB               CSTL_ERPDB              ZASHRIS
(ZAS accounting)     (group accounting)        (group ERP)            (group HR)
 write: postings      read: chart of accounts,  read: items, batch      read: employees,
 read: COA, ledgers         vouchers, financial       stock, material         job locations
                            year, bank/cash            conversion, recipes
```

## 1. Accounting — `ZAS_ACCDB` and `CSTL_ACCDB`

**Direction:** SalesRoll → accounting (write, via procedures) and accounting → SalesRoll (read, via three-part
names).

**Connection:** `SolutionConnectionStringACCDB` for `ZAS_ACCDB`. `CSTL_ACCDB` has **no connection string at
all** — it is reached only as `CSTL_ACCDB..<table>` from inside queries running on `SalesrollDB_ZAS`, so it must
live on the same instance (or a linked server) as the application database.

**Objects read directly:**

| Catalog | Object |
|---|---|
| `ZAS_ACCDB` | `tblChartOfAccounts`, `tblControlLedger`, `tblLedgerHead`, `tblSubsidiaryLedger`, `tblSubsubsidiaryLadger` (sic), `tblBankAccount`, `tblBankInformation`, `tblCashAccount`, `tblMultipleCompanySameBankAccount`, `tblMultipleCompanySameCashAccount` |
| `CSTL_ACCDB` | `tblChartOfAccounts`, `tblFinancialYear`, `tblDebitCreditVoucherMaster`, `tblDebitCreditVoucherDetail`, `tblBankAccount`, `tblBankAccountUnitdtl`, `tblBankInformation`, `tblCashAccount`, `tblMultipleCompanySameCashAccount` |

**Posting procedures (write side — bodies UNKNOWN):**

| Trigger in the UI | Procedure | Parameters passed |
|---|---|---|
| Proforma invoice save | `sp_ProformaPosting` | `@InvoiceId` |
| Proforma full return | `sp_ProformaFullReturnPosting` | `@InvoiceId` |
| Proforma partial return | `sp_ProformaReturnPosting` | `@InvoiceId` |
| Delivery confirmation (full) | `sp_Sales_ExecuteDeliveryPipeline` | `@InvoiceId` |
| Delivery confirmation (partial) | `sp_Sales_DeliverySalesPosting` | `@InvoiceId` |
| Delivery invoice delete | `sp_DeliveryInvoiceDeletePosting` | `@InvoiceId` |
| Delivery return | `sp_Sales_DeliverySalesReturnPosting` | (see `CustomerReturnDAL`) |
| Customer payment | `sp_Sales_CustomerPaymentPosting`, `sp_CustomerPaymentPosting` | — |
| Payment reversal | `sp_Sales_CustomerPaymentReversePosting` | — |
| Product destroy approval | `sp_Sales_ProductDestroyPosting` | destroy id |
| Warehouse stock-in approval | `sp_WHStockInPosting`, `sp_Acc_MaterialConversionReceive` | `@MasterId`, `@ApprovalBy`, `@ApprovalDate`, `@ApprovalStatus` |
| Sample invoice | `sp_SampleInvoicePosting` | — |
| Freeze / release | `sp_FreezeStockPosting`, `sp_FreezeStockReleasePosting` | — |
| Trade material transfer | `sp_Process_TradeMaterialTransferFullProcess` | — |
| Customer / supplier / product GL rows | `sp_I_CustomerGl`, `sp_I_SupplierGl`, `sp_I_ProductGl`, `sp_UD_CustomerGl`, `sp_Integration_CustomerSubsidaryInsert` | — |

**Preconditions the application checks before posting:**

- an **Active** `CSTL_ACCDB..tblFinancialYear` row for the company covering the document date
  (`InvoiceDAL.GetFinancialYearInfo`);
- a `ZAS_ACCDB..tblChartOfAccounts` row for the customer (`InvoiceDAL.CheckChartOfAccount`).

**Failure handling:** none in the application. `ClsCommonInternalDAL.UpdateAction` returns a `bool`; several
call sites ignore it. There is no retry, no dead-letter, no reconciliation report and no posting log visible in
the repository. **Whether a posting procedure is idempotent is UNKNOWN**, so the effect of pressing an approve
button twice cannot be stated.

## 2. Group ERP — `CSTL_ERPDB`

**Direction:** ERP → SalesRoll, read-only. Nothing in the repository writes to `CSTL_ERPDB`.

| Object | Used by |
|---|---|
| `tblItemDescription`, `View_ItemDescription` | `TradeMaterialTransferDal`, `ProductDAL` — mapping SalesRoll products to ERP items |
| `tbl_INV_BatchWiseStock` | batch stock look-up |
| `tbl_MC_MaterialConversionMaster`, `tbl_MC_MaterialConversionDetails`, `tbl_MC_RecipeMaster`, `tbl_MC_ProductionUnit` | `MaterialConversionReceiveDal` — receiving converted/produced material |
| `tblCompanyInfo`, `tblCompanyUnit` | company/unit cross-reference |

Supporting procedures: `sp_Common_GetUnitWiseLocation_FromErp (@CompanyId)`,
`sp_GET_AllItemDropDown_Zas`, `SP_DataMigration_Item (@ProductCodePram)` (`ProductDAL.CretaeItem` — pushes a
SalesRoll product into the ERP item master; body UNKNOWN),
`sp_StockInMIGOtoCentralStore` (goods receipt from an ERP MIGO into `tblCentralStore`).

**Screens:** `MaterialConversionReceive.aspx`, `MaterialConversionReceiveView.aspx`,
`TradeMaterialTransfer.aspx` (+ `…Approval`, `…List`), `MIGOReport.aspx`.

## 3. HR — `ZASHRIS`

**Direction:** HR → SalesRoll, read then copy.

**Screen:** `Solution.Web/SInventory_UI/HREmployeeSysnc.aspx` (spelled "Sysnc").
**Layer:** `Library.DAL/SInventory_DAL/HREmployeeDataSyncDal.cs`.

| Step | Procedure | Parameters |
|---|---|---|
| List candidates from HR | `sp_Employee_EmployeeListFromHR` | `@Parameter` |
| Sync one employee | `sp_Employee_EmployeeListSyncedWithDistibution` (sic) | `@EmployeeMasterCode` |
| Sync one employee's company assignment | `sp_Employee_EmployeeCompanySyncedWithDistibution` | `@EmployeeMasterCode` |

Objects read: `ZASHRIS..tblEmpGeneralInfo`, `ZASHRIS..tblJobLocation`. Target: `tblEmpGeneralInfo` in
`SalesrollDB_ZAS`.

**It is manual and one employee at a time** — the user picks rows on the screen and presses sync. There is no
schedule, no delta detection and no deletion propagation. What the procedures actually copy is **UNKNOWN**.

## 4. Excel

Two directions, both file-based through the browser.

**Import.** `Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs` + `ExcelUpForMIGOBLL` load a spreadsheet of MIGO
(goods-receipt) lines. Customer master import uses staging tables
`tblCustomerMasterExcelFileMaster` / `tblCustomerMasterExcelFileDetail` and `tbltempCustMaster`. The only
`asp:FileUpload` control in the entire application is on the import screen.

**Export.** Several Crystal reports exist in an "Excel" variant, exported by choosing a different `.rpt`:
`ASMReportExcel.rpt`, `MIOReportExcel.rpt`, `crpDeliveryPersonExcel.rpt`,
`crpEemployeeInformationExcel.rpt` (sic), `crpRSMInformationExcel.rpt`, `rptNationalBinCardExcel.rpt`,
`rptWHDCStockExcel.rpt`. The viewer chooses between the normal and the Excel report using
`Request.QueryString["reportType"]` / `["rptType"]`, or `Session["Excel"]`.
`Solution.Web/ExcelFiles/` holds sample/output workbooks.

## 5. Reporting engine

Crystal Reports 13 is an in-process integration: `crystalreportviewers13/` ships the client resources,
`CrystalImageHandler.aspx` is registered in `Web.config` as both an `httpHandler` and a `handler`, and
`CrystalImageCleaner-*` app settings control temp-image cleanup (start on, sleep 60 s, age 120 s).
Typed datasets in `Library.CrystalReports/SInventory_DS` (114 `.xsd`) define the report schemas.

## 6. What is *not* integrated

Explicitly checked and absent from the repository:

- No e-mail (`SmtpClient`, `MailMessage`, `<mailSettings>`) — no notifications of any kind.
- No SMS gateway.
- No payment gateway.
- No SAP, e-Pharma, e-Sales or LMS integration (these were asked about; **none exists** in this codebase).
- No mobile/app API — although `tblUser.IsAppsUser` implies app accounts exist, no endpoint serves them here.
- No single sign-on, LDAP or Active Directory.
- No file-based EDI, no FTP, no SFTP.

## Integration risk summary

| Risk | Evidence | Confidence |
|---|---|---|
| Cross-database writes are not in a distributed transaction | postings are separate `EXEC`s after the local insert has already committed | CONFIRMED |
| A failed posting is silent | return value frequently ignored; `DB_Manager` swallows exceptions | CONFIRMED |
| `CSTL_ACCDB` and `ZASHRIS` have no connection string — they must be co-located or linked | three-part names only | CONFIRMED |
| Re-approving may double-post | idempotency lives in the procedures, which are absent | UNKNOWN |
| Coupling is by table name across four databases | 30 three-part-name references | CONFIRMED |
