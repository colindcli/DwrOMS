using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Attachment
    {
        /// <summary>
        /// ԭͼ·��
        /// </summary>
        [NotMapped]
        public string Path => $"/Files/{Folder}/{AttachmentId}{FileExt}";

        /// <summary>
        /// Сͼ·��
        /// </summary>
        [NotMapped]
        public string PathSmall
        {
            get
            {
                var path = $"/Files/{Folder}/{AttachmentId}_small{FileExt}";
                if (File.Exists(path))
                {
                    return path;
                }
                return Path;
            }
        }

        /// <summary>
        /// ��ͼ·��
        /// </summary>
        [NotMapped]
        public string PathBig
        {
            get
            {
                var path = $"/Files/{Folder}/{AttachmentId}_big{FileExt}";
                if (File.Exists(path))
                {
                    return path;
                }
                return Path;
            }
        }
    }
}