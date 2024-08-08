﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaby.Services.Models.CommentModels
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public int LikesCount { get; set; }
        public string? Attachments { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}