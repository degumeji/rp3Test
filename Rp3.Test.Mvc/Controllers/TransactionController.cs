using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rp3.Test.Proxies;
using Rp3.Test.Mvc.Models;
using Rp3.Test.Common.Models;

namespace Rp3.Test.Mvc.Controllers
{
    public class TransactionController : Controller
    {

        public ActionResult Index()
        {
            Proxy proxy = new Proxy();
            var data = new List<TransactionView>();

            if (Session["user"] == null)
            {
                data = proxy.GetTransactions();
            }
            else
            {
                data = proxy.GetTransactions().OrderBy(p => p.CategoryId).ToList();
            }

            List<Rp3.Test.Mvc.Models.TransactionViewModel> model = new List<Models.TransactionViewModel>();

            foreach (var item in data)
            {
                model.Add(new Models.TransactionViewModel()
                {
                    Amount = item.Amount,
                    CategoryId = item.CategoryId,
                    CategoryName = item.CategoryName,
                    Notes = item.Notes,
                    RegisterDate = item.RegisterDate,
                    ShortDescription = item.ShortDescription,
                    TransactionId = item.TransactionId,
                    TransactionType = item.TransactionType,
                    TransactionTypeId = item.TransactionTypeId
                });
            }

            return View(model);
        }

        public ActionResult Create()
        {

            Proxy proxy = new Proxy();

            TransactionCreateModel createModel = new TransactionCreateModel();
            List<SelectListItem> listCategories = new List<SelectListItem>();
            List<SelectListItem> listTransactionType = new List<SelectListItem>();

            foreach (var item in proxy.GetCategories().OrderBy(p => p.CategoryId))
            {
                listCategories.Add(new SelectListItem() { Text = item.Name, Value = (item.CategoryId.ToString()) });
            }

            foreach (var item in proxy.GetTransactionTypes().OrderBy(p => p.TransactionTypeId))
            {
                listTransactionType.Add(new SelectListItem() { Text = item.Name, Value = (item.TransactionTypeId.ToString()) });
            }

            createModel.RegisterDate = DateTime.Now;
            createModel.CategorySelectList = new SelectList(listCategories, "Value", "Text");
            createModel.TransactionTypeSelectList = new SelectList(listTransactionType, "Value", "Text");

            ViewBag.Message = "";

            return View(createModel);
        }

        [HttpPost]
        public ActionResult Create(TransactionCreateModel createModel)
        {
            Proxy proxy = new Proxy();
            Transaction commonModel = new Transaction();

            List<SelectListItem> listCategories = new List<SelectListItem>();
            List<SelectListItem> listTransactionType = new List<SelectListItem>();

            foreach (var item in proxy.GetCategories().OrderBy(p => p.CategoryId))
            {
                listCategories.Add(new SelectListItem() { Text = item.Name, Value = (item.CategoryId.ToString()) });
            }

            foreach (var item in proxy.GetTransactionTypes().OrderBy(p => p.TransactionTypeId))
            {
                listTransactionType.Add(new SelectListItem() { Text = item.Name, Value = (item.TransactionTypeId.ToString()) });
            }

            createModel.CategorySelectList = new SelectList(listCategories, "Value", "Text");
            createModel.TransactionTypeSelectList = new SelectList(listTransactionType, "Value", "Text");

            commonModel.TransactionTypeId = createModel.TransactionTypeId;
            commonModel.CategoryId = createModel.CategoryId;

            DateTime dt1 = DateTime.Parse(DateTime.Now.AddDays(-30).ToString());
            DateTime dt2 = createModel.RegisterDate;

            if (dt2 < dt1)
            {
                ViewBag.Message = "Fecha ingresada es menor a 30 dias !!";
                return View(createModel);
            }

            commonModel.RegisterDate = createModel.RegisterDate;

            if (createModel.Amount <= 0)
            {
                ViewBag.Message = "Monto debe ser mayor a 0 !!!";
                return View(createModel);
            }

            commonModel.Amount = createModel.Amount;
            commonModel.ShortDescription = createModel.ShortDescription;
            commonModel.Notes = createModel.Notes;

            bool respondeOk = proxy.CreateTransaction(commonModel);

            if (respondeOk)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Error al crear !!!";
                return View(createModel);
            }
        }

        public ActionResult Edit(int id)
        {

            Proxy proxy = new Proxy();

            TransactionEditModel editModel = new TransactionEditModel();
            List<SelectListItem> listCategories = new List<SelectListItem>();
            List<SelectListItem> listTransactionType = new List<SelectListItem>();

            var commonModel = proxy.GetTransactionById(id);

            foreach (var item in proxy.GetCategories().OrderBy(p => p.CategoryId))
            {
                listCategories.Add(new SelectListItem() { Text = item.Name, Value = (item.CategoryId.ToString()) });
            }

            foreach (var item in proxy.GetTransactionTypes().OrderBy(p => p.TransactionTypeId))
            {
                listTransactionType.Add(new SelectListItem() { Text = item.Name, Value = (item.TransactionTypeId.ToString()) });
            }

            editModel.TransactionId = commonModel.TransactionId;
            editModel.TransactionTypeId = commonModel.TransactionTypeId;
            editModel.CategoryId = commonModel.CategoryId;
            editModel.RegisterDate = commonModel.RegisterDate;
            editModel.Amount = commonModel.Amount;
            editModel.ShortDescription = commonModel.ShortDescription;
            editModel.Notes = commonModel.Notes;
            editModel.CategorySelectList = new SelectList(listCategories, "Value", "Text", commonModel.CategoryId);
            editModel.TransactionTypeSelectList = new SelectList(listTransactionType, "Value", "Text", commonModel.TransactionTypeId);

            ViewBag.Message = "";

            return View(editModel);
        }

        [HttpPost]
        public ActionResult Edit(TransactionEditModel editModel)
        {
            Proxy proxy = new Proxy();
            Transaction commonModel = new Transaction();

            List<SelectListItem> listCategories = new List<SelectListItem>();
            List<SelectListItem> listTransactionType = new List<SelectListItem>();

            foreach (var item in proxy.GetCategories().OrderBy(p => p.CategoryId))
            {
                listCategories.Add(new SelectListItem() { Text = item.Name, Value = (item.CategoryId.ToString()) });
            }

            foreach (var item in proxy.GetTransactionTypes().OrderBy(p => p.TransactionTypeId))
            {
                listTransactionType.Add(new SelectListItem() { Text = item.Name, Value = (item.TransactionTypeId.ToString()) });
            }

            editModel.CategorySelectList = new SelectList(listCategories, "Value", "Text", commonModel.CategoryId);
            editModel.TransactionTypeSelectList = new SelectList(listTransactionType, "Value", "Text", commonModel.TransactionTypeId);

            commonModel.TransactionId = editModel.TransactionId;
            commonModel.TransactionTypeId = editModel.TransactionTypeId;
            commonModel.CategoryId = editModel.CategoryId;
            commonModel.RegisterDate = editModel.RegisterDate;

            if (editModel.Amount <= 0)
            {
                ViewBag.Message = "Monto debe ser mayor a 0 !!!";
                return View(editModel);
            }

            commonModel.Amount = editModel.Amount;
            commonModel.ShortDescription = editModel.ShortDescription;
            commonModel.Notes = editModel.Notes;

            bool respondeOk = proxy.UpdateTransaction(commonModel);

            if (respondeOk)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Error al Editar !!!";
                return View(editModel);
            }
        }

        public ActionResult Balance()
        {
            Proxy proxy = new Proxy();
            var data = new List<Balance>();

            if (Session["user"] != null)
            {
                data = proxy.GetBalances();
            }

            List<BalanceViewModel> model = new List<BalanceViewModel>();

            foreach (var item in data)
            {
                model.Add(new BalanceViewModel()
                {
                    CATEGORY = item.CATEGORY,
                    SALDO = item.SALDO
                });
            }

            return View(model);
        }


        #region "POP UP"

        [HttpGet]
        public ActionResult GetPOPUP(int id)
        {

            Proxy proxy = new Proxy();

            TransactionEditModel editModel = new TransactionEditModel();
            List<SelectListItem> listCategories = new List<SelectListItem>();
            List<SelectListItem> listTransactionType = new List<SelectListItem>();

            var commonModel = proxy.GetTransactionById(id);

            foreach (var item in proxy.GetCategories().OrderBy(p => p.CategoryId))
            {
                listCategories.Add(new SelectListItem() { Text = item.Name, Value = (item.CategoryId.ToString()) });
            }

            foreach (var item in proxy.GetTransactionTypes().OrderBy(p => p.TransactionTypeId))
            {
                listTransactionType.Add(new SelectListItem() { Text = item.Name, Value = (item.TransactionTypeId.ToString()) });
            }

            editModel.TransactionId = commonModel.TransactionId;
            editModel.TransactionTypeId = commonModel.TransactionTypeId;
            editModel.CategoryId = commonModel.CategoryId;
            editModel.RegisterDateString = commonModel.RegisterDate.ToString("dd/M/yyyy H:mm:ss");
            editModel.Amount = commonModel.Amount;
            editModel.ShortDescription = commonModel.ShortDescription;
            editModel.Notes = commonModel.Notes;
            editModel.CategorySelectList = new SelectList(listCategories, "Value", "Text", commonModel.CategoryId);
            editModel.TransactionTypeSelectList = new SelectList(listTransactionType, "Value", "Text", commonModel.TransactionTypeId);

            ViewBag.Message = "";

            return Json(editModel, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult EditPOPUP(TransactionEditModel editModel)
        {
            Proxy proxy = new Proxy();
            Transaction commonModel = new Transaction();

            commonModel.TransactionId = editModel.TransactionId;
            commonModel.TransactionTypeId = editModel.TransactionTypeId;
            commonModel.CategoryId = editModel.CategoryId;
            commonModel.RegisterDate = DateTime.Parse(editModel.RegisterDateString);

            if (editModel.Amount <= 0)
            {
                return RedirectToAction("Index");
            }

            commonModel.Amount = editModel.Amount;
            commonModel.ShortDescription = editModel.ShortDescription;
            commonModel.Notes = editModel.Notes;

            bool respondeOk = proxy.UpdateTransaction(commonModel);

            return RedirectToAction("Index");
        }
        #endregion
    }
}
