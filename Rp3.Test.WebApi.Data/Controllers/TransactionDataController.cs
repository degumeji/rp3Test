using Rp3.Test.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Rp3.Test.Common.Models;

namespace Rp3.Test.WebApi.Data.Controllers
{
    public class TransactionDataController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {            
            List<Rp3.Test.Common.Models.TransactionView> commonModel = new List<Common.Models.TransactionView>();

            using (DataService service = new DataService())
            {
                IEnumerable<Rp3.Test.Data.Models.Transaction> 
                    dataModel = service.Transactions.Get(                   
                    includeProperties: "Category,TransactionType", 
                    orderBy: p=> p.OrderByDescending(o=>o.RegisterDate) );

                //Para incluir una condición, puede usar el primer parametro de Get
                /*
                 * Ejemplo
                 IEnumerable<Rp3.Test.Data.Models.Transaction>
                    dataModel = service.Transactions.Get(p=> p.TransactionId > 0
                    includeProperties: "Category,TransactionType",
                    orderBy: p => p.OrderByDescending(o => o.RegisterDate));

                 */

                commonModel = dataModel.Select(p => new Common.Models.TransactionView()
                {
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    Notes = p.Notes,
                    Amount = p.Amount,
                    RegisterDate = p.RegisterDate,
                    ShortDescription = p.ShortDescription,
                    TransactionId = p.TransactionId,
                    TransactionType = p.TransactionType.Name,
                    TransactionTypeId = p.TransactionTypeId
                }).ToList();
            }

            return Ok(commonModel);
        }

        [HttpGet]
        public IHttpActionResult GetBalance()
        {
            List<Balance> commonModel = new List<Balance>();

            using (DataService service = new DataService())
            {
                var model = service.Transactions.GetBalance();

                commonModel = model.Select(p => new Balance()
                {
                    CATEGORY = p.CATEGORY,
                    SALDO = p.SALDO
                }).ToList();
            }
            return Ok(commonModel);
        }

        [HttpGet]
        public IHttpActionResult GetById(int transactionId)
        {
            Transaction commonModel = null;
            using (DataService service = new DataService())
            {
                var model = service.Transactions.GetByID(transactionId);

                commonModel = new Transaction()
                {
                    TransactionId = model.TransactionId,
                    TransactionTypeId = model.TransactionTypeId,
                    CategoryId = model.CategoryId,
                    RegisterDate = model.RegisterDate,
                    Amount = model.Amount,
                    ShortDescription = model.ShortDescription,
                    Notes = model.Notes
                };
            }
            return Ok(commonModel);
        }

        [HttpPost]
        public IHttpActionResult Insert(Transaction transaction)
        {
            using (DataService service = new DataService())
            {
                Rp3.Test.Data.Models.Transaction transactionModel = new Rp3.Test.Data.Models.Transaction();

                transactionModel.TransactionTypeId = transaction.TransactionTypeId;
                transactionModel.CategoryId = transaction.CategoryId;
                transactionModel.RegisterDate = transaction.RegisterDate;
                transactionModel.Amount = transaction.Amount;
                transactionModel.ShortDescription = transaction.ShortDescription;
                transactionModel.Notes = transaction.Notes;

                transactionModel.TransactionId = service.Transactions.GetMaxValue<int>(p => p.TransactionId, 0) + 1;

                service.Transactions.Insert(transactionModel);
                service.SaveChanges();
            }

            return Ok(true);
        }

        [HttpPost]
        public IHttpActionResult Update(Transaction transaction)
        {
            //Complete the code
            using (DataService service = new DataService())
            {
                Rp3.Test.Data.Models.Transaction model = service.Transactions.GetByID(transaction.TransactionId);

                model.TransactionTypeId = transaction.TransactionTypeId;
                model.CategoryId = transaction.CategoryId;
                model.RegisterDate = transaction.RegisterDate;
                model.Amount = transaction.Amount;
                model.ShortDescription = transaction.ShortDescription;
                model.Notes = transaction.Notes;

                service.Transactions.Update(model);
                service.SaveChanges();
            }

            return Ok(true);
        }
    }
}
