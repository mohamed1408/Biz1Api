namespace Biz1PosApi.Models
{
    public class SP
    {
        public string OrderWiseReport = @"
        SELECT o.Id, o.BillAmount, o.CustomerId, OrderedDateTime OrderedDate, dateadd(s, convert(bigint, o.CreatedTimeStamp) / 1000, convert(datetime, '1-1-1970 05:30:00')) as OrderedDate1, o.StoreId, o.SourceId, o.OrderTypeId, s.StoreName as Store,
        o.Tax1+o.Tax2+o.Tax3 as TotalTax,o.DiscAmount,o.PaidAmount,o.InvoiceNo, '[]' ChargeJson, isnull(o.ItemJson, dbo.rptitmjsn(o.Id)) ItemJson, isnull(o.OrderJson, dbo.rptordjsn(o.Id)) OrderJson, 
        STRING_AGG(CAST(t.Amount * (CASE WHEN t.TransTypeId = 1 THEN 1 ELSE -1 END) AS NVARCHAR(MAX)) + ' | ' + isnull(spt.Name, case WHEN o.OrderTypeId = 6 THEN 'SW_ZM' WHEN o.OrderTypeId = 7 THEN 'FB' ELSE 'SPT NA' END) + ' | ' + cast(t.TransDateTime AS NVARCHAR(MAX)), ', ') TransactionDetails FROM POSOrder o
        JOIN GetStores(@storeId,@companyId) s ON o.StoreId = s.Id
        LEFT JOIN Transactions t ON t.OrderId = o.OdrsId
        LEFT JOIN StorePaymentTypes spt ON spt.Id = t.StorePaymentTypeId and spt.StoreId = o.StoreId
        WHERE 
        --(o.StoreId = @storeId OR @storeId = 0) and 
        (o.OrderedDate >= @fromDate AND o.OrderedDate<=@todate)
        and (@sourceId=o.SourceId or @sourceId =0) 
        and (@cancelorder = 0 AND o.OrderStatusId!=-1)
        and o.OrderTypeId in (1,2,3,4,5)
        --  OR (@cancelorder = 1 AND o.OrderStatusId = -1)) 
        and o.CompanyId = @companyId AND o.OrderTypeId <= 7
        --  GROUP BY o.Id, o.BillAmount, o.OrderedDate, o.StoreId, s.Name,o.DiscAmount,o.PaidAmount,o.InvoiceNo, o.ChargeJson, o.ItemJson
        GROUP BY o.Id, o.BillAmount, o.CreatedTimeStamp, o.CustomerId, o.OrderedDateTime, o.StoreId, o.SourceId, s.StoreName,o.Tax1, o.Tax2, o.Tax3, o.DiscAmount,o.PaidAmount,o.InvoiceNo, o.ChargeJson, o.ItemJson, o.OrderJson, o.OrderTypeId

        SELECT 'nodata' nodata
        SELECT 'nodata' nodata
        SELECT 'nodata' nodata";
        public string WiseReport { get; set;}
    }
}
