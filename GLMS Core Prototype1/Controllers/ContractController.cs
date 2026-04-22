using GLMS_Core_Prototype.Data;
using GLMS_Core_Prototype.Services;
using GLMS_Core_Prototype.Models;
using GLMS_Core_Prototype.Patterns.Observer_Pattern;
using GLMS_Core_Prototype.Services.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS_Core_Prototype.Controllers
{
    public class ContractController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly FileService _fileService;
        private readonly IModelValidator<Contract> _validator;
        private readonly List<IObserver> _observers;

        public ContractController(ApplicationDbContext context, FileService fileService, IModelValidator<Contract> validator)
        {
            _context = context;
            _fileService = fileService;
            _validator = validator;
            _observers = new List<IObserver> { new NotificationService(), new AuditService() };
        }

        [HttpGet]
        public IActionResult Index(DateTime? date, DateTime? startDate, DateTime? endDate, ContractStatus? status)
        {
            var query = _context.Contracts.Include(c => c.Client).AsQueryable();

            if (date.HasValue)
            {
                var targetDate = date.Value.Date;
                query = query.Where(c => c.StartDate.Date <= targetDate && c.EndDate.Date >= targetDate);
            }

            if (startDate.HasValue)
            {
                var rangeStart = startDate.Value.Date;
                query = query.Where(c => c.StartDate.Date >= rangeStart);
            }

            if (endDate.HasValue)
            {
                var rangeEnd = endDate.Value.Date;
                query = query.Where(c => c.EndDate.Date <= rangeEnd);
            }

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            ViewData["FilterDate"] = date?.ToString("yyyy-MM-dd");
            ViewData["FilterStartDate"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["FilterEndDate"] = endDate?.ToString("yyyy-MM-dd");
            ViewData["FilterStatus"] = status?.ToString();

            return View(query.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Clients"] = new SelectList(_context.Clients, "ClientId", "Name");
            return View(new Contract
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today
            });
        }

        [HttpPost]
        public IActionResult Create(Contract contract, IFormFile? signedAgreement)
        {
            foreach (var error in _validator.Validate(contract))
            {
                ModelState.AddModelError(string.Empty, error);
            }
            if (ModelState.IsValid)
            {
                _context.Contracts.Add(contract);
                _context.SaveChanges();

                if (signedAgreement != null)
                {
                    try
                    {
                        contract.SignedAgreementPath = _fileService.SavePdfForContract(signedAgreement, contract.ContractId, null);
                        _context.SaveChanges();
                    }
                    catch (InvalidOperationException ex)
                    {
                        TempData["AgreementUploadError"] = ex.Message;
                        ViewData["Clients"] = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
                        return View(contract);
                    }
                }

                NotifyObservers(contract);
                return RedirectToAction("Details", new { id = contract.ContractId });
            }

            ViewData["Clients"] = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        public IActionResult Details(int id)
        {
            var contract = _context.Contracts.Include(c => c.Client).FirstOrDefault(c => c.ContractId == id);
            if (contract == null) return NotFound();
            return View(contract);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var contract = _context.Contracts.Find(id);
            if (contract == null) return NotFound();

            ViewData["Clients"] = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        [HttpPost]
        public IActionResult Edit(Contract contract)
        {
            foreach (var error in _validator.Validate(contract))
            {
                ModelState.AddModelError(string.Empty, error);
            }
            if (ModelState.IsValid)
            {
                var existing = _context.Contracts.Find(contract.ContractId);
                if (existing == null) return NotFound();

                existing.ClientId = contract.ClientId;
                existing.ServiceLevel = contract.ServiceLevel;
                existing.Status = contract.Status;
                existing.StartDate = contract.StartDate;
                existing.EndDate = contract.EndDate;
                existing.Region = contract.Region;

                _context.SaveChanges();
                NotifyObservers(existing);
                return RedirectToAction("Details", new { id = existing.ContractId });
            }

            ViewData["Clients"] = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var contract = _context.Contracts.Find(id);
            if (contract == null) return NotFound();
            return View(contract);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var contract = _context.Contracts.Find(id);
            if (contract != null)
            {
                _context.Contracts.Remove(contract);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UploadAgreement(int id, IFormFile file)
        {
            var contract = _context.Contracts.Find(id);
            if (contract == null) return NotFound();

            try
            {
                var path = _fileService.SavePdfForContract(file, contract.ContractId, contract.SignedAgreementPath);
                contract.SignedAgreementPath = path;
                _context.SaveChanges();
                NotifyObservers(contract);
            }
            catch (InvalidOperationException ex)
            {
                TempData["AgreementUploadError"] = ex.Message;
            }

            return RedirectToAction("Details", new { id });
        }

        private void NotifyObservers(Contract contract)
        {
            foreach (var observer in _observers)
                observer.Update(contract);
        }
    }
}
