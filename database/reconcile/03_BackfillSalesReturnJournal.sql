-- Backfill: post the Sales Return journal for accepted returns that never got one
-- (approved before 02_DeliverySalesReturnPosting_CentralStoreJoin.sql was applied).
-- Run on SalesrollDB_ZAS. Edit the @ids list first.
-- Returns 2,3,5,6,8 (2019-2024) are deliberately excluded: posting them dates
-- vouchers into closed financial years. Add them only with accounting sign-off.
USE SalesrollDB_ZAS;
SET NOCOUNT ON;

DECLARE @ids TABLE (CustReturnId INT);
INSERT @ids VALUES (3200), (3204);

DECLARE @id INT;
DECLARE c CURSOR FAST_FORWARD FOR SELECT CustReturnId FROM @ids;
OPEN c; FETCH NEXT FROM c INTO @id;
WHILE @@FETCH_STATUS = 0
BEGIN
    -- SP guard: any voucher keyed by the return id (incl. a COGS one) makes it exit silently
    IF EXISTS (SELECT 1 FROM CSTL_ACCDB..tblDebitCreditVoucherMaster WHERE TableID = 2018 AND CommonPK_ID = @id)
        PRINT 'Return ' + CAST(@id AS VARCHAR(20)) + ': SKIPPED - voucher keyed by return id exists, SP guard would block';
    ELSE
    BEGIN
        EXEC sp_Sales_DeliverySalesReturnPosting @ReturnId = @id;
        PRINT 'Return ' + CAST(@id AS VARCHAR(20)) + ': EXEC done';
    END
    FETCH NEXT FROM c INTO @id;
END
CLOSE c; DEALLOCATE c;

-- Verify: should list none of the ids above
SELECT R.CustReturnId, R.ApprovedDate, ID.InvoiceId, SUM(D.ReturnAmount) AS ReturnAmount
FROM tblCustomerReturnMaster R
JOIN tblCustomerReturnDetail D ON D.CustReturnId = R.CustReturnId
JOIN tblInvoiceDetail ID ON ID.InvoiceDetailId = D.InvoiceDetailId
WHERE R.ActionStatus = 'Accepted'
  AND R.CustReturnId IN (SELECT CustReturnId FROM @ids)
  AND NOT EXISTS (SELECT 1 FROM CSTL_ACCDB..tblDebitCreditVoucherMaster M
                  WHERE M.TableID = 2018 AND ISNULL(M.IsInventoryValuation,0) = 0
                    AND M.CommonPK_ID = ID.InvoiceId)
GROUP BY R.CustReturnId, R.ApprovedDate, ID.InvoiceId;

-- Duplicate COGS check: the SP re-runs COGS posting at the end; any count > 1 is a double post
SELECT CommonPK_ID AS CustReturnId, COUNT(*) AS CogsVouchers
FROM CSTL_ACCDB..tblDebitCreditVoucherMaster
WHERE TableID = 2018 AND IsInventoryValuation = 1
  AND CommonPK_ID IN (SELECT CustReturnId FROM @ids)
GROUP BY CommonPK_ID HAVING COUNT(*) > 1;
