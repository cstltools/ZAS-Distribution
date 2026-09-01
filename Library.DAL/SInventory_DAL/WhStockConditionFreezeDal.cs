using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class WhStockConditionFreezeDal
    {
        ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

         public void GetWhInfoOnDropDownList(DropDownList ddl)
        {
            string query = @"SELECT WearhouseId, WearhouseCode + ':' + WearhouseName AS WearhouseName  FROM tblWearhouse";
            aCommonInternalDal.LoadDropDownValue(ddl, "WearhouseName", "WearhouseId", query, "SSIDB");
        }

         public void GetCompanyDropdownList(DropDownList ddl)
         {
             const string queryStr = @"SELECT CompanyId,CompanyName AS CompanyName FROM tblCompanyInfo";
             aCommonInternalDal.LoadDropDownValue(ddl, "CompanyName", "CompanyId", queryStr, "SSIDB");
         }

         public void GetWhInformation(DropDownList ddl, string companyId)
         {
             string queryStr = @"SELECT WearhouseId, WearhouseCode + ' : ' + WearhouseName AS WearhouseName FROM tblWearhouse WHERE CompanyId = " + companyId;
             aCommonInternalDal.LoadDropDownValue(ddl, "WearhouseName", "WearhouseId", queryStr, "SSIDB");
         }

        public DataTable GetWhStockInformation(string warehouseId)
        {
            string query = @"SELECT DISTINCT tblCentralStore.*,ReceiveId AS nomanslandID,tblCentralStore.ProductCode,tblCentralStore.ProductName,tblCentralStore.BatchNo,ExpDate,ReceiveDate,StockInQty AS TotalQuantity,Quantity AS StockQty,StockCondition
                           FROM dbo.tblCentralStore where tblCentralStore.WarehouseId = '" + warehouseId + "' AND StockCondition = 'Available'  AND  Quantity > 0  ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable LoadWHData(int ReceiveId)
        {
            string query = @"SELECT  tblCentralStore.*,ReceiveId AS nomanslandID,tblCentralStore.ProductCode,tblCentralStore.ProductName,tblCentralStore.BatchNo,ExpDate,ReceiveDate,StockInQty AS TotalQuantity,Quantity AS StockQty,StockCondition
                           FROM dbo.tblCentralStore where StockCondition = 'Available' AND ReceiveId= '" + ReceiveId + "'";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public void LoadStockConditionBll(DropDownList aDownList, string userid)
        {
            string StockCondition = "select * from tblStockCondition WHERE StockCondition<>'Available' AND StockConId IN (SELECT StockConId FROM tblStockConditionPermission where Permission=1 AND UserId='" + userid + "')";
            aCommonInternalDal.LoadDropDownValue(aDownList, "StockCondition", "StockConId", StockCondition, "SSIDB");
        }

        public bool SaveforWh(WhStockConditionFreezeDao aConditionFreezeDao)
        {
            string insertQuery = @"insert into tblWhStockConditionFreeze (WhStockConditionFreezeID,ReceiveId,WarehouseId,FreezeQty,EntryBy,EntryDate) 
            values (" + aConditionFreezeDao.WhStockConditionFreezeID + "," + aConditionFreezeDao.ReceiveId + ",'" + aConditionFreezeDao.WarehouseId + "','" + aConditionFreezeDao.FreezeQty + "','" + aConditionFreezeDao.EntryBy + "','" + aConditionFreezeDao.EntryDate + "')";

            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool SaveWhStoreFreeze(WhStoreFreezeDao aWhStoreFreezeDao)
        {
            string insertQuery = @"insert into tblWhStoreFreeze (TotalQuantity,ProductId,ProductName,PackSize,BatchNo,ExpDate,ReceiveDate,StockQty,DamageQty,StockRcvDate,StockCondition,Remarks,ReceiveId,WhStockConditionFreezeID) 
            values (" + "'" + aWhStoreFreezeDao.TotalQuantity + "','" + aWhStoreFreezeDao.ProductId + "','" + aWhStoreFreezeDao.ProductName + "','" + aWhStoreFreezeDao.PackSize + "','" + aWhStoreFreezeDao.BatchNo + "','" + aWhStoreFreezeDao.ExpDate + "','" + aWhStoreFreezeDao.ReceiveDate  + "','" + aWhStoreFreezeDao.StockQty + "','" + aWhStoreFreezeDao.DamageQty + "','" + aWhStoreFreezeDao.StockRcvDate + "','" + aWhStoreFreezeDao.StockCondition + "','" + aWhStoreFreezeDao.Remarks + "','" + aWhStoreFreezeDao.ReceiveId + "','" + aWhStoreFreezeDao.WhStockConditionFreezeID + "'" + ")";
            return aCommonInternalDal.SaveDataByInsertCommand(insertQuery, "SSIDB");
        }

        public bool UpdateCentralStore(decimal StockQty, int ReceiveId)
        {
            string query = @"UPDATE tblCentralStore SET Quantity='" + StockQty + "' WHERE ReceiveId=" + ReceiveId + "";
            return aCommonInternalDal.UpdateDataByUpdateCommand(query, "SSIDB");
        }

        public DataTable LoadNationalStockInfo(string parameter)
        {
            string query = @"SELECT  
    CASE 
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '371' THEN 'ZAS Dermal'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '372' THEN 'ZAS Critical Care'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '373' THEN 'ZAS Ophthalmic'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '379' THEN 'ZAS Active' 
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '375' THEN 'ZAS Dental ( Clinic )'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '381' THEN 'ZAS Dental ( Lab  )'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '377' THEN 'ZAS Gloves & Instruments'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '376' THEN 'ZAS Gyno'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '378' THEN 'ZAS Nutrition' 
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '374' THEN 'ZAS Onco'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '382' THEN 'ZAS OTC'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '383' THEN 'ZAS NOC'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '384' THEN 'ZAS Baby Care'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '385' THEN 'ZAS Neurology'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '386' THEN 'ZAS Hematology'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '387' THEN 'Aqvida Bangladesh'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '391' THEN 'Z A S Nutrition'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '392' THEN 'ZAS I V F'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '393' THEN 'ZAS Bioplasma'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '394' THEN 'Esha Enterprise'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '389' THEN 'Z A S Industries'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '396' THEN 'Dermal-Devision-1'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '397' THEN 'ZAS Distribution Sales & Marketing'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '398' THEN 'ZAS Rare Deasese'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '399' THEN 'ZAS Nutrition-IV'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '411' THEN 'ZAS Named Patient Access'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '412' THEN 'ZAS Biotech'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '413' THEN 'ZAS Anesthesia'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '414' THEN 'Pharma Parts  International'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '415' THEN 'ZAS Dermatology'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '416' THEN 'ZAS Pharma LTD ( New )'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '388' THEN 'ZAS Pharma Ltd.'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '417' THEN 'ZAS Regenerative'
        WHEN SUBSTRING(vTblTotal.ProductCode, 1, 3) = '418' THEN 'ZAS Cardiology'
        ELSE ''  
    END AS CompanyName,

    vTblTotal.ProductCode,
    vTblTotal.ProductName,
    vTblTotal.PackSize,
    MAX(ISNULL(WA.Avg_Rate, 0)) AS Avg_Rate,

    SUM(vTblTotal.AvailableQty) + SUM(vTblTotal.FZQty) AS AQty,
    SUM(vTblTotal.BookFDel) AS BookFDel,
    SUM(vTblTotal.Tqty) AS Tqty,
    SUM(vTblTotal.RQty) AS RQty,
    SUM(vTblTotal.BQty) AS BQty,
    SUM(vTblTotal.WHQty) AS WHQty,
    SUM(vTblTotal.FZQty) AS FZQty,
    SUM(vTblTotal.ReturnRcvQty) AS ReturnRcvQty,

    SUM
    (
        vTblTotal.WHQty
        + vTblTotal.AvailableQty
        + vTblTotal.RQty
        + vTblTotal.BQty
        + vTblTotal.FZQty
        + vTblTotal.ReturnRcvQty
    ) AS TotalQty,

    SUM(vTblTotal.AvailableQty * ISNULL(WA.Avg_Rate, vTblTotal.Price)) AS AvialableValue,
    SUM(vTblTotal.RQty * ISNULL(WA.Avg_Rate, vTblTotal.Price)) AS RestrictedValue,
    SUM(vTblTotal.Tqty * ISNULL(WA.Avg_Rate, vTblTotal.Price)) AS TransitValue,
    SUM(vTblTotal.BQty * ISNULL(WA.Avg_Rate, vTblTotal.Price)) AS BlockedValue,
    SUM(vTblTotal.WHQty * ISNULL(WA.Avg_Rate, vTblTotal.Price)) AS WhStockValue,
    SUM(vTblTotal.ReturnRcvQty * ISNULL(WA.Avg_Rate, vTblTotal.Price)) AS ReturnRcvValue,

    (
        SUM(vTblTotal.RQty * ISNULL(WA.Avg_Rate, vTblTotal.Price))
        + SUM(vTblTotal.BQty * ISNULL(WA.Avg_Rate, vTblTotal.Price))
        + SUM(vTblTotal.AvailableQty * ISNULL(WA.Avg_Rate, vTblTotal.Price))
        + SUM(vTblTotal.WHQty * ISNULL(WA.Avg_Rate, vTblTotal.Price))
        + SUM(vTblTotal.ReturnRcvQty * ISNULL(WA.Avg_Rate, vTblTotal.Price))
    ) AS GrandTotal

FROM 
(
    /* =====================================================
       1. Available DC Stock - Central Store Based
       ===================================================== */
    SELECT 
        p.ProductCode,
        p.ProductName,
        DCS.PackSize,
        DCS.BatchNo,
        DCS.MfgDate,
        DCS.ExpDate,
        DCS.StockQty AS AvailableQty,
        0 AS BookFDel,
        0 AS Tqty,
        0 AS RQty,
        0 AS BQty,
        0 AS FZQty,
        UP.UnitPrice AS Price,
        DCS.ComUnitId,
        0 AS WHQty,
        0 AS ReturnRcvQty
    FROM dbo.tblDCStore DCS
    INNER JOIN dbo.tblProduct p 
        ON DCS.ProductCode = p.ProductCode
    INNER JOIN dbo.tblUnitPrice AS UP 
        ON p.ProductId = UP.ProductId
    INNER JOIN tblStockInTransfar ST 
        ON ST.StockInTransfarId = DCS.StockInTransfarId
    INNER JOIN dbo.tblCentralStore CS 
        ON CS.ReceiveId = ST.ReceiveId
    INNER JOIN dbo.tblWHStockInDetail MD 
        ON MD.WHStockInDetailID = CS.MigoDetailID
    WHERE 
        DCS.StockQty > 0
        AND DCS.StockCondition = 'Available'
        AND UP.IsActive = 1

    UNION ALL

    /* =====================================================
       2. Available DC Stock - Non-Transactional Invoice Return
       ReceiveId = 0, TempReceiveId = 0
       Price = NonTrans Detail Price + VAT
       ===================================================== */
    SELECT 
        p.ProductCode,
        p.ProductName,
        DCS.PackSize,
        DCS.BatchNo,
        DCS.MfgDate,
        DCS.ExpDate,
        0 AS AvailableQty,
        0 AS BookFDel,
        0 AS Tqty,
        0 AS RQty,
        0 AS BQty,
        0 AS FZQty,
        (ISNULL(NTD.Price, 0) + ISNULL(NTD.VAT, 0)) AS Price,
        DCS.ComUnitId,
        0 AS WHQty,
        DCS.StockQty AS ReturnRcvQty
    FROM dbo.tblDCStore DCS
    INNER JOIN dbo.tblProduct p 
        ON LTRIM(RTRIM(UPPER(DCS.ProductCode))) = LTRIM(RTRIM(UPPER(p.ProductCode)))
    INNER JOIN dbo.tblNonTranscationalInvoiceDetail NTD
        ON NTD.InvReturnDetailID = DCS.NonTranscationalInvReturnDetailID
    WHERE 
        DCS.StockQty > 0
        AND DCS.StockCondition = 'Available'
        AND ISNULL(DCS.NonTranscationalInvReturnDetailID, 0) > 0
        AND ISNULL(DCS.ReceiveId, 0) = 0
        AND ISNULL(DCS.TempReceiveId, 0) = 0

    UNION ALL

    /* =====================================================
       3. Available DC Stock - Chalan Receive Based
       ===================================================== */
    SELECT 
        p.ProductCode,
        p.ProductName,
        DCS.PackSize,
        DCS.BatchNo,
        DCS.MfgDate,
        DCS.ExpDate,
        DCS.StockQty AS AvailableQty,
        0 AS BookFDel,
        0 AS Tqty,
        0 AS RQty,
        0 AS BQty,
        0 AS FZQty,
        UP.UnitPrice AS Price,
        DCS.ComUnitId,
        0 AS WHQty,
        0 AS ReturnRcvQty
    FROM dbo.tblDCStore DCS
    INNER JOIN dbo.tblChalanDetail cd 
        ON DCS.ChalanDetailsId = cd.ChalanDetailsId
    INNER JOIN dbo.tblProduct p 
        ON DCS.ProductCode = p.ProductCode
    INNER JOIN dbo.tblUnitPrice AS UP 
        ON p.ProductId = UP.ProductId
    WHERE 
        DCS.StockQty > 0
        AND DCS.StockCondition = 'Available'
        AND UP.IsActive = 1

    UNION ALL

    /* =====================================================
       4. Booked For Delivery - Invoice
       ===================================================== */
    SELECT 
        p.ProductCode,
        p.ProductName,
        ID.PackSize,
        ID.BatchNo,
        DCS.MfgDate,
        DCS.ExpDate,
        0 AS AvailableQty,
        ID.TotalQuantity AS BookFDel,
        0 AS Tqty,
        0 AS RQty,
        0 AS BQty,
        0 AS FZQty,
        UP.UnitPrice AS Price,
        I.ComUnitId,
        0 AS WHQty,
        0 AS ReturnRcvQty
    FROM dbo.tblInvoice I
    INNER JOIN dbo.tblInvoiceDetail ID 
        ON ID.InvoiceId = I.InvoiceId
    INNER JOIN dbo.tblDCStore DCS 
        ON DCS.DCStoreId = ID.DCStoreId
    INNER JOIN dbo.tblProduct p 
        ON DCS.ProductCode = p.ProductCode
    INNER JOIN dbo.tblUnitPrice AS UP 
        ON p.ProductId = UP.ProductId
    WHERE 
        I.DelivaryInvoiceNo IS NULL
        AND UP.IsActive = 1

    UNION ALL

    /* =====================================================
       5. Booked For Delivery - Chalan
       ===================================================== */
    SELECT 
        p.ProductCode,
        p.ProductName,
        p.PackSize,
        ID.BatchNo,
        DCS.MfgDate,
        DCS.ExpDate,
        0 AS AvailableQty,
        ID.Quantity AS BookFDel,
        0 AS Tqty,
        0 AS RQty,
        0 AS BQty,
        0 AS FZQty,
        UP.UnitPrice AS Price,
        CU.ComUnitId,
        0 AS WHQty,
        0 AS ReturnRcvQty
    FROM dbo.tblChalanInfo I
    INNER JOIN dbo.tblChalanDetail ID 
        ON ID.ChalanId = I.ChalanId
    INNER JOIN dbo.tblDCStore DCS 
        ON DCS.DCStoreId = ID.DCStoreId
    INNER JOIN dbo.tblCompanyUnit CU 
        ON I.FromComUnitCode = CU.ComUnitCode
    INNER JOIN dbo.tblProduct p 
        ON DCS.ProductCode = p.ProductCode
    INNER JOIN dbo.tblUnitPrice AS UP 
        ON p.ProductId = UP.ProductId
    INNER JOIN tblStockInTransfar ST 
        ON ST.StockInTransfarId = DCS.StockInTransfarId
    INNER JOIN dbo.tblCentralStore CS 
        ON CS.ReceiveId = ST.ReceiveId
    INNER JOIN dbo.tblWHStockInDetail MD 
        ON MD.WHStockInDetailID = CS.MigoDetailID
    WHERE 
        I.IsDeliver IS NULL
        AND UP.IsActive = 1

    UNION ALL

    /* =====================================================
       6. Freeze / Return Stock
       ===================================================== */
    SELECT 
        p.ProductCode,
        p.ProductName,
        DCS.PackSize,
        DCS.BatchNo,
        ISNULL(MD.MfgDate, DS.MfgDate) AS MfgDate,
        DCS.ExpDate,
        0 AS AvailableQty,
        0 AS BookFDel,
        0 AS Tqty,
        0 AS RQty,
        0 AS BQty,
        DCS.StockQty AS FZQty,
        UP.UnitPrice AS Price,
        DCS.ComUnitId,
        0 AS WHQty,
        0 AS ReturnRcvQty
    FROM dbo.tblDCStoreFreeze DCS
    INNER JOIN dbo.tblDCStore DS 
        ON DS.DCStoreId = DCS.DCStoreId
    INNER JOIN dbo.tblProduct p 
        ON DCS.ProductCode = p.ProductCode
    INNER JOIN dbo.tblUnitPrice AS UP 
        ON p.ProductId = UP.ProductId
    LEFT JOIN tblStockInTransfar ST 
        ON ST.StockInTransfarId = DS.StockInTransfarId
    LEFT JOIN dbo.tblCentralStore CS 
        ON CS.ReceiveId = ST.ReceiveId
    LEFT JOIN dbo.tblWHStockInDetail MD 
        ON MD.WHStockInDetailID = CS.MigoDetailID
    WHERE 
        DCS.StockQty > 0
        AND (DCS.StockCondition IS NULL OR DCS.StockCondition = 'ReturnStock')
        AND UP.IsActive = 1

    UNION ALL

    /* =====================================================
       7. Restricted Stock
       ===================================================== */
    SELECT 
        p.ProductCode,
        p.ProductName,
        DCS.PackSize,
        DCS.BatchNo,
        ISNULL(MD.MfgDate, DS.MfgDate) AS MfgDate,
        DCS.ExpDate,
        0 AS AvailableQty,
        0 AS BookFDel,
        0 AS Tqty,
        DCS.StockQty AS RQty,
        0 AS BQty,
        0 AS FZQty,
        UP.UnitPrice AS Price,
        DCS.ComUnitId,
        0 AS WHQty,
        0 AS ReturnRcvQty
    FROM dbo.tblDCStoreFreeze DCS
    INNER JOIN dbo.tblDCStore DS 
        ON DS.DCStoreId = DCS.DCStoreId
    INNER JOIN dbo.tblProduct p 
        ON DCS.ProductCode = p.ProductCode
    INNER JOIN dbo.tblUnitPrice AS UP 
        ON p.ProductId = UP.ProductId
    LEFT JOIN tblStockInTransfar ST 
        ON ST.StockInTransfarId = DS.StockInTransfarId
    LEFT JOIN dbo.tblCentralStore CS 
        ON CS.ReceiveId = ST.ReceiveId
    LEFT JOIN dbo.tblWHStockInDetail MD 
        ON MD.WHStockInDetailID = CS.MigoDetailID
    WHERE 
        DCS.StockQty > 0
        AND DCS.StockCondition = 'Restricted'
        AND UP.IsActive = 1

    UNION ALL

    /* =====================================================
       8. Blocked Stock
       ===================================================== */
    SELECT 
        p.ProductCode,
        p.ProductName,
        DCS.PackSize,
        DCS.BatchNo,
        ISNULL(MD.MfgDate, DS.MfgDate) AS MfgDate,
        DCS.ExpDate,
        0 AS AvailableQty,
        0 AS BookFDel,
        0 AS Tqty,
        0 AS RQty,
        DCS.StockQty AS BQty,
        0 AS FZQty,
        UP.UnitPrice AS Price,
        DCS.ComUnitId,
        0 AS WHQty,
        0 AS ReturnRcvQty
    FROM dbo.tblDCStoreFreeze DCS
    INNER JOIN dbo.tblDCStore DS 
        ON DS.DCStoreId = DCS.DCStoreId
    INNER JOIN dbo.tblProduct p 
        ON DCS.ProductCode = p.ProductCode
    INNER JOIN dbo.tblUnitPrice AS UP 
        ON p.ProductId = UP.ProductId
    LEFT JOIN tblStockInTransfar ST 
        ON ST.StockInTransfarId = DS.StockInTransfarId
    LEFT JOIN dbo.tblCentralStore CS 
        ON CS.ReceiveId = ST.ReceiveId
    LEFT JOIN dbo.tblWHStockInDetail MD 
        ON MD.WHStockInDetailID = CS.MigoDetailID
    WHERE 
        DCS.StockQty > 0
        AND DCS.StockCondition = 'Blocked'
        AND UP.IsActive = 1

    UNION ALL

    /* =====================================================
       9. Transit From Requisition
       ===================================================== */
    SELECT 
        p.ProductCode,
        p.ProductName,
        ST.PackSize,
        ST.BatchNo,
        ST.MfgDate,
        ST.ExpDate,
        0 AS AvailableQty,
        0 AS BookFDel,
        ST.Quantity AS Tqty,
        0 AS RQty,
        0 AS BQty,
        0 AS FZQty,
        UP.UnitPrice AS Price,
        R.ComUnitId,
        0 AS WHQty,
        0 AS ReturnRcvQty
    FROM dbo.tblRequisition R
    INNER JOIN dbo.tblStockInTransfar ST 
        ON ST.ReqId = R.ReqId
    INNER JOIN dbo.tblProduct p 
        ON p.ProductCode = ST.ProductCode
    INNER JOIN dbo.tblUnitPrice AS UP 
        ON p.ProductId = UP.ProductId
    INNER JOIN dbo.tblCentralStore CS 
        ON CS.ReceiveId = ST.ReceiveId
    WHERE 
        R.ReceiveIssue IS NULL
        AND UP.IsActive = 1

    UNION ALL

    /* =====================================================
       10. Transit From Chalan
       ===================================================== */
    SELECT 
        p.ProductCode,
        p.ProductName,
        D.PackSize,
        D.BatchNo,
        D.MfgDate,
        D.ExpDate,
        0 AS AvailableQty,
        0 AS BookFDel,
        CD.Quantity AS Tqty,
        0 AS RQty,
        0 AS BQty,
        0 AS FZQty,
        UP.UnitPrice AS Price,
        CU.ComUnitId,
        0 AS WHQty,
        0 AS ReturnRcvQty
    FROM dbo.tblChalanInfo C
    INNER JOIN dbo.tblChalanDetail CD 
        ON CD.ChalanId = C.ChalanId
    INNER JOIN dbo.tblDCStore D 
        ON D.DCStoreId = CD.DCStoreId
    INNER JOIN dbo.tblCompanyUnit CU 
        ON C.ToComUnitCode = CU.ComUnitCode
    INNER JOIN dbo.tblProduct p 
        ON p.ProductCode = CD.ProductCode
    INNER JOIN dbo.tblUnitPrice AS UP 
        ON p.ProductId = UP.ProductId
    WHERE 
        C.IsDeliver = 'False'
        AND UP.IsActive = 1

    UNION ALL

    /* =====================================================
       11. WH Stock
       ===================================================== */
    SELECT 
        ProductCode,
        ProductName,
        PackSize,
        BatchNo,
        MfgDate,
        ExpDate,
        0 AS AvailableQty,
        0 AS BookFDel,
        0 AS Tqty,
        0 AS RQty,
        0 AS BQty,
        0 AS FZQty,
        UnitPrice AS Price,
        0 AS ComUnitId,
        SUM(Quantity) AS WHQty,
        0 AS ReturnRcvQty
    FROM dbo.tblCentralStore
    WHERE Quantity > 0
    GROUP BY 
        ProductCode,
        ProductName,
        PackSize,
        BatchNo,
        MfgDate,
        ExpDate,
        UnitPrice

) AS vTblTotal

LEFT JOIN dbo.tblCompanyUnit Cu 
    ON Cu.ComUnitId = vTblTotal.ComUnitId 
LEFT JOIN dbo.tblProduct P_OUT
    ON P_OUT.ProductCode = vTblTotal.ProductCode
LEFT JOIN (
    SELECT cs1.ProductCode, cs1.Company_Id, cs1.Avg_Rate
    FROM dbo.tblCentralStore cs1
    INNER JOIN (
        SELECT Company_Id, ProductCode, MAX(ReceiveId) AS MaxReceiveId
        FROM dbo.tblCentralStore
        GROUP BY Company_Id, ProductCode
    ) x
        ON x.Company_Id = cs1.Company_Id
       AND x.ProductCode = cs1.ProductCode
       AND x.MaxReceiveId = cs1.ReceiveId
) WA ON vTblTotal.ProductCode = WA.ProductCode AND WA.Company_Id = P_OUT.CompanyId " + parameter + @" GROUP BY vTblTotal.ProductCode, vTblTotal.ProductName, vTblTotal.PackSize HAVING SUM(ISNULL(vTblTotal.AvailableQty, 0)) > 0
    OR SUM(ISNULL(vTblTotal.FZQty, 0)) > 0
    OR SUM(ISNULL(vTblTotal.BookFDel, 0)) > 0
    OR SUM(ISNULL(vTblTotal.Tqty, 0)) > 0
    OR SUM(ISNULL(vTblTotal.RQty, 0)) > 0
    OR SUM(ISNULL(vTblTotal.BQty, 0)) > 0
    OR SUM(ISNULL(vTblTotal.WHQty, 0)) > 0
    OR SUM(ISNULL(vTblTotal.ReturnRcvQty, 0)) > 0

ORDER BY 
    vTblTotal.ProductCode";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
