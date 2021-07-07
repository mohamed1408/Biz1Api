using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biz1PosApi.Controllers
{
    public class ElectronController : Controller
    {
        // GET: ElectronController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ElectronController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ElectronController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ElectronController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ElectronController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ElectronController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ElectronController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ElectronController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
