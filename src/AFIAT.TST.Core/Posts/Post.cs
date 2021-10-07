using AFIAT.TST.Posts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace AFIAT.TST.Posts
{
    [Table("Posts")]
    public class Post : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Title { get; set; }

        [Required]
        public virtual string Description { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int Views { get; set; }

        public virtual int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category CategoryFk { get; set; }

    }
}