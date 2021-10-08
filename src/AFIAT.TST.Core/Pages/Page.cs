using AFIAT.TST.Posts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace AFIAT.TST.Pages
{
    [Table("Pages")]
    public class Page : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public virtual double Views { get; set; }

        public virtual string ImageSrc { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category CategoryFk { get; set; }

    }
}