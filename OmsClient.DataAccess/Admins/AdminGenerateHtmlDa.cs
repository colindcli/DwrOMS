using Dapper;
using OmsClient.DataAccess.Admins.Bases;
using OmsClient.Entity;
using System.Collections.Generic;
using System.Linq;
using OmsClient.Entity.Models;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.DataAccess.Admins
{
    public class AdminGenerateHtmlDa : BaseAdminGenerateHtmlDa
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<GenerateHtml> GetGenerateHtmls()
        {
            const string sqlStr = @"
SELECT TOP (100) gh.GenerateHtmlId,gh.GenerateHtmlUrl,gh.GenerateHtmlPath INTO #Temp FROM dbo.GenerateHtml gh WHERE gh.IsGenerate=0 ORDER BY gh.GenerateHtmlId ASC;
UPDATE dbo.GenerateHtml SET IsGenerate=1 FROM dbo.GenerateHtml x, #Temp y WHERE x.GenerateHtmlId=y.GenerateHtmlId;
SELECT * FROM #Temp t;
DROP TABLE #Temp;";
            return Db((db, tran) => db.Query<GenerateHtml>(sqlStr, null, transaction: tran).ToList());
        }

        public GenerateHtmlModel GetGenerateHtmlReport()
        {
            const string sqlStr = @"SELECT COUNT(CASE WHEN gh.IsGenerate=1 THEN 1 ELSE 0 END) Finished,COUNT(CASE WHEN gh.IsSuccess=1 THEN 1 ELSE 0 END) Success,COUNT(1) Total FROM dbo.GenerateHtml gh;";
            return Db(db => db.Query<GenerateHtmlModel>(sqlStr).FirstOrDefault());
        }
    }
}