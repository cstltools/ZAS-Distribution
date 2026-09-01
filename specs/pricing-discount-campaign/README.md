# Module: pricing-discount-campaign

Commercial policy: campaigns and bonus campaigns (buy-X-get-Y, `tblFocMaster`/`tblFocDetails`, `tblBonusCampaignMaster`/`tblBonusCampgainDetail`), product discounts, customer fixed discounts, and customer-specific quoted prices — each with its own approval screen.

How these are actually applied at invoicing time is documented as `BR-PRC-002` (quoted price overrides list price, date-bounded and approval-gated) and `BR-PRC-003` (product discount by customer + product, **not** date-bounded despite the date being passed — see `GAP-APP-003`).

## Pages (15)

| Page ID | Page | Route | Kind | Controls | Grids | Methods | Master page | Spec |
|---|---|---|---|---|---|---|---|---|
| `PRC-001` | `BonusCampaignSetup.aspx` | `/SInventory_UI/BonusCampaignSetup.aspx` | Entry / transaction | 35 | 1 | 22 | yes | [spec](pages/PAGE-PRC-001-BonusCampaignSetup.md) |
| `PRC-002` | `BonusCampaignSetupView.aspx` | `/SInventory_UI/BonusCampaignSetupView.aspx` | List / view | 29 | 1 | 9 | yes | [spec](pages/PAGE-PRC-002-BonusCampaignSetupView.md) |
| `PRC-003` | `CampaignSetup.aspx` | `/SInventory_UI/CampaignSetup.aspx` | Entry / transaction | 45 | 1 | 20 | yes | [spec](pages/PAGE-PRC-003-CampaignSetup.md) |
| `PRC-004` | `CampaignView.aspx` | `/SInventory_UI/CampaignView.aspx` | List / view | 23 | 1 | 9 | yes | [spec](pages/PAGE-PRC-004-CampaignView.md) |
| `PRC-005` | `CustomerFixedDiscount.aspx` | `/SInventory_UI/CustomerFixedDiscount.aspx` | Entry / transaction | 28 | 0 | 10 | yes | [spec](pages/PAGE-PRC-005-CustomerFixedDiscount.md) |
| `PRC-006` | `CustomerFixedDiscountView.aspx` | `/SInventory_UI/CustomerFixedDiscountView.aspx` | List / view | 12 | 1 | 6 | yes | [spec](pages/PAGE-PRC-006-CustomerFixedDiscountView.md) |
| `PRC-007` | `DiscountApproval.aspx` | `/SInventory_UI/DiscountApproval.aspx` | Approval | 28 | 2 | 11 | yes | [spec](pages/PAGE-PRC-007-DiscountApproval.md) |
| `PRC-008` | `FixedDiscountApproval.aspx` | `/SInventory_UI/FixedDiscountApproval.aspx` | Approval | 15 | 1 | 8 | yes | [spec](pages/PAGE-PRC-008-FixedDiscountApproval.md) |
| `PRC-009` | `FixedDiscountEntry.aspx` | `/SInventory_UI/FixedDiscountEntry.aspx` | Entry / transaction | 14 | 0 | 12 | yes | [spec](pages/PAGE-PRC-009-FixedDiscountEntry.md) |
| `PRC-010` | `FixedDiscountView.aspx` | `/SInventory_UI/FixedDiscountView.aspx` | List / view | 18 | 1 | 10 | yes | [spec](pages/PAGE-PRC-010-FixedDiscountView.md) |
| `PRC-011` | `ProductDiscountEntry.aspx` | `/SInventory_UI/ProductDiscountEntry.aspx` | Entry / transaction | 33 | 0 | 26 | yes | [spec](pages/PAGE-PRC-011-ProductDiscountEntry.md) |
| `PRC-012` | `ProductDiscountView.aspx` | `/SInventory_UI/ProductDiscountView.aspx` | List / view | 23 | 1 | 13 | yes | [spec](pages/PAGE-PRC-012-ProductDiscountView.md) |
| `PRC-013` | `ProductQuotedPriceApproval.aspx` | `/SInventory_UI/ProductQuotedPriceApproval.aspx` | Approval | 30 | 2 | 11 | yes | [spec](pages/PAGE-PRC-013-ProductQuotedPriceApproval.md) |
| `PRC-014` | `ProductQuotedPriceSetup.aspx` | `/SInventory_UI/ProductQuotedPriceSetup.aspx` | Entry / transaction | 31 | 0 | 26 | yes | [spec](pages/PAGE-PRC-014-ProductQuotedPriceSetup.md) |
| `PRC-015` | `ProductQuotedPriceView.aspx` | `/SInventory_UI/ProductQuotedPriceView.aspx` | List / view | 22 | 1 | 12 | yes | [spec](pages/PAGE-PRC-015-ProductQuotedPriceView.md) |

## Database objects this module touches

Aggregated from every page above by following code-behind → BLL → DAL.

| Table / view | Call sites in this module |
|---|---|
| `View_CustomerMaster` | 2 |
| `View_InvoiceWiseDue` | 2 |
| `tblArea` | 7 |
| `tblBonusCampaignMaster` | 5 |
| `tblBonusCampgainDetail` | 6 |
| `tblCompanyInfo` | 18 |
| `tblCompanyUnit` | 4 |
| `tblCustMaster` | 16 |
| `tblCustomerFixedDiscount` | 5 |
| `tblDcWiseAreaInfo` | 2 |
| `tblEmpGeneralInfo` | 2 |
| `tblGroupInformation` | 4 |
| `tblInvoice` | 6 |
| `tblMIOInfo` | 8 |
| `tblManufacturer` | 1 |
| `tblMarket` | 6 |
| `tblProduct` | 14 |
| `tblProductDiscount` | 10 |
| `tblProductQuotedPrice` | 6 |
| `tblRegion` | 4 |
| `tblSupplierInformation` | 1 |
| `tblTerritory` | 8 |
| `tblUnitPrice` | 1 |

## Stored procedures this module calls

| Procedure | Call sites in this module |
|---|---|
| `sp_CPM_BonusCampaignDetailInsert` | 1 |
| `sp_CPM_BonusCampaigntMasterInsert` | 1 |
| `sp_CPM_BonusCampaigntMasterUpdate` | 1 |
| `sp_CPM_DeleteBonusCampaign` | 1 |
| `sp_DEL_ProductDiscount` | 2 |

## Cross-cutting references

- [../00-system/architecture.md](../00-system/architecture.md) — the call chain every page here follows
- [../00-system/conventions.md](../00-system/conventions.md) — the grid, dropdown, validation and message patterns
- [../00-system/business-rules.md](../00-system/business-rules.md) — application-wide rules
- [../00-system/status-lifecycle.md](../00-system/status-lifecycle.md) — status values and transitions
- [../SPEC-GAPS.md](../SPEC-GAPS.md) — what is still unknown
