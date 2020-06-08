using Dapper;
using OmsClient.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.DataAccess
{
    public class FileDa : RepositoryBase
    {
        public List<Attachment> GetAttachmentListById(List<Guid> attachmentIds)
        {
            return Db(db => db.Query<Attachment>(@"SELECT * FROM dbo.Attachment a WHERE a.AttachmentId IN @AttachmentIds;", new { AttachmentIds = attachmentIds }).ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Upload(Attachment model)
        {
            Db(db => db.Insert<Guid, Attachment>(model));
        }

        public Attachment GetAttachment(Guid attachmentId)
        {
            return Db(db => db.Get<Attachment>(attachmentId));
        }
    }
}
