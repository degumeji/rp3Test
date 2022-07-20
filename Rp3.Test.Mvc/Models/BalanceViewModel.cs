using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rp3.Test.Mvc.Models
{
    public class BalanceViewModel
    {
        public string CATEGORY { get; set; }

        [DisplayFormat(DataFormatString = "{0:n0}")]
        public decimal SALDO { get; set; }
    }
}