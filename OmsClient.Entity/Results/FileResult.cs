using System;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Results
{
    public class FileResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }

    public class FileUploadResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Original { get; set; }
        public string Small { get; set; }
        public string Big { get; set; }
    }
}
