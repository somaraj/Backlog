﻿using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Masters
{
    public class Document : BaseEntity
    {
        public string FileName { get; set; }

        public byte[] FileData { get; set; }

        public string ContentType { get; set; }

        public long FileSize { get; set; }

        public string Extension { get; set; }
    }
}