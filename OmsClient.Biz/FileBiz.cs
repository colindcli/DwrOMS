using OmsClient.DataAccess;
using OmsClient.Entity;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz
{
    public class FileBiz
    {
        private static readonly FileDa Da = new FileDa();

        public List<FileResult> GetAttachmentListById(List<Guid> attachmentIds)
        {
            var list = Da.GetAttachmentListById(attachmentIds);
            return attachmentIds.Join(list, p => p, p => p.AttachmentId, (p, q) => q).Select(p => new FileResult()
            {
                Id = p.AttachmentId,
                Title = p.Title
            }).ToList();
        }

        /// <summary>
        /// 获取附件
        /// </summary>
        /// <param name="attachmentIds"></param>
        /// <returns></returns>
        public List<Attachment> GetAttachmentByBulk(List<Guid> attachmentIds)
        {
            return Da.GetAttachmentListById(attachmentIds);
        }

        public Attachment GetAttachment(Guid attachmentId)
        {
            return Da.GetAttachment(attachmentId);
        }
    }
}
