using GerenciarSenhas.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace GerenciarSenhas.Controllers
{
	public class ContaController : Controller
    {
		[AllowAnonymous]
		public ActionResult Login()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Senhas");
			}
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult Login(Usuario usuario)
		{
			
			if (!ModelState.IsValid)
			{
				return View();
			}

			using (GerenciaSenhasEntities bd = new GerenciaSenhasEntities())
			{
				var v = bd.Usuario.Where(a => a.Login.Equals(usuario.Login) && a.Senha.Equals(usuario.Senha)).FirstOrDefault();
				if (v != null)
				{
					Session.Add("ID_usuario", v.ID);
					FormsAuthentication.SetAuthCookie(usuario.Login, false);
					return RedirectToAction("Index", "Senhas");
				}
				else
				{
					ModelState.AddModelError("", "*Insira um login e uma senha válida!");
				}
			}

			return View();
		}

		[Authorize]
		public ActionResult Deslogar()
		{
			Session.RemoveAll();
			FormsAuthentication.SignOut();
			return RedirectToAction("Login", "Conta");
		}

		[AllowAnonymous]
		public ActionResult Cadastro()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Senhas");
			}
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult Cadastro(Usuario usuario)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			using (GerenciaSenhasEntities bd = new GerenciaSenhasEntities())
			{
				var v = bd.Usuario.Where(a => a.Login.Equals(usuario.Login) && a.Senha.Equals(usuario.Senha)).FirstOrDefault();
				if (v != null)
				{
					ModelState.AddModelError("", "*Usuário já cadastrado!");
				}
				else
				{
					Response.Write("<script>alert('Cadastro efetuado com sucesso');</script>");
					bd.Usuario.Add(usuario);
					bd.SaveChanges();
					return RedirectToAction("Login", "Conta");
				}
			}
			return View();
		}
	}
}