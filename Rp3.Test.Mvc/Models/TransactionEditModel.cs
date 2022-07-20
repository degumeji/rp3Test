using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rp3.Test.Mvc.Models
{
    public class TransactionEditModel
    {
        public int TransactionId { get; set; }
        public short TransactionTypeId { get; set; }
        public int CategoryId { get; set; }

        [Display(Name = "RegisterDate"), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RegisterDate { get; set; }
        public string RegisterDateString { get; set; }

        [Display(Name = "Amount"), Required]
        [Range(1, 100), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public string ShortDescription { get; set; }
        public string Notes { get; set; }
        public SelectList CategorySelectList { get; set; }
        public SelectList TransactionTypeSelectList { get; set; }
    }
}