using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GerenciarSenhas.Models;

namespace GerenciarSenhas.Controllers
{
	public class SenhasController : Controller
    {
        private GerenciaSenhasEntities db = new GerenciaSenhasEntities();

		// GET: teste
		[Authorize]
		public ActionResult Index()
        {
            var senha = db.Senha.Include(s => s.Usuario);
            return View(senha.ToList());
        }

		// GET: teste/Create
		[Authorize]
		public ActionResult Create()
        {
			ViewBag.IDUsuario = new SelectList(db.Usuario, "ID", "Login");
            return View();
        }

        // POST: teste/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult Create([Bind(Include = "ID,IDUsuario,Senha1,LocalS,LoginS")] Senha senha)
        {
			senha.IDUsuario = Convert.ToInt32( Session["ID_usuario"]);
			if (ModelState.IsValid)
            {
                db.Senha.Add(senha);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDUsuario = new SelectList(db.Usuario, "ID", "Login", senha.IDUsuario);
            return View(senha);
        }

		// GET: teste/Edit/5
		[Authorize]
		public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Senha senha = db.Senha.Find(id);
            if (senha == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDUsuario = new SelectList(db.Usuario, "ID", "Login", senha.IDUsuario);
            return View(senha);
        }

        // POST: teste/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult Edit([Bind(Include = "ID,IDUsuario,Senha1,LocalS,LoginS")] Senha senha)
        {
			if( senha.LocalS == null || senha.LoginS == null || senha.Senha1 == null)
			{
				//ModelState.AddModelError("", "Insira todos os dados!");
			}
            if (ModelState.IsValid)
            {
                db.Entry(senha).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUsuario = new SelectList(db.Usuario, "ID", "Login", senha.IDUsuario);
            return View(senha);
        }

		// GET: teste/Delete/5
		[Authorize]
		public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Senha senha = db.Senha.Find(id);
            if (senha == null)
            {
                return HttpNotFound();
            }
            return View(senha);
        }

        // POST: teste/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult DeleteConfirmed(int id)
        {
            Senha senha = db.Senha.Find(id);
            db.Senha.Remove(senha);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
