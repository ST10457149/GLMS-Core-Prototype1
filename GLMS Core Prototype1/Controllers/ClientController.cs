using GLMS_Core_Prototype.Data; 
using GLMS_Core_Prototype.Models;
using GLMS_Core_Prototype.Services.Validation;
using Microsoft.AspNetCore.Mvc;

namespace GLMS_Core_Prototype.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IModelValidator<Client> _validator;

        public ClientController(ApplicationDbContext context, IModelValidator<Client> validator)
        {
            _context = context;
            _validator = validator;
        }

        public IActionResult Index() => View(_context.Clients.ToList());

        public IActionResult Details(int id)
        {
            var client = _context.Clients.FirstOrDefault(c => c.ClientId == id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Client client)
        {
            foreach (var error in _validator.Validate(client))
            {
                ModelState.AddModelError(string.Empty, error);
            }
            if (ModelState.IsValid)
            {
                _context.Clients.Add(client);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost]
        public IActionResult Edit(Client client)
        {
            foreach (var error in _validator.Validate(client))
            {
                ModelState.AddModelError(string.Empty, error);
            }
            if (ModelState.IsValid)
            {
                _context.Clients.Update(client);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var client = _context.Clients.Find(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}