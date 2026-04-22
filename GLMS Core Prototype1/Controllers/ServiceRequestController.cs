using GLMS_Core_Prototype.Data;
using GLMS_Core_Prototype.Models;
using GLMS_Core_Prototype.Services;
using GLMS_Core_Prototype.Services.Validation;
using GLMS_Core_Prototype.Patterns.Factory_Pattern;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLMS_Core_Prototype.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CurrencyService _currencyService;
        private readonly ServiceRequestValidator _validator;

        public ServiceRequestController(ApplicationDbContext context, CurrencyService currencyService, ServiceRequestValidator validator)
        {
            _context = context;
            _currencyService = currencyService;
            _validator = validator;
        }

        [HttpGet]
        public IActionResult Create(int contractId = 0)
        {
            if (contractId != 0 && _context.Contracts.Find(contractId) == null)
            {
                return NotFound();
            }

            ViewData["Contracts"] = _context.Contracts
                .Select(c => new
                {
                    c.ContractId,
                    Display = $"{c.ContractId} - {c.Client.Name}"
                })
                .ToList();
            return View(new ServiceRequest { ContractId = contractId });
        }

        [HttpGet]
        public async Task<IActionResult> UsdToZarRate()
        {
            try
            {
                var rate = await _currencyService.GetUsdToZarRateAsync();
                return Json(new { rate });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceRequest request, string handlerType = "Standard")
        {
            var contract = _context.Contracts.Find(request.ContractId);
            var errors = _validator.Validate(request, contract).ToList();
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            if (!ModelState.IsValid)
            {
                ViewData["Contracts"] = _context.Contracts
                    .Select(c => new
                    {
                        c.ContractId,
                        Display = $"{c.ContractId} - {c.Client.Name}"
                    })
                    .ToList();
                return View(request);
            }

            // Currency conversion
            try
            {
                var rate = await _currencyService.GetUsdToZarRateAsync();
                request.CostZAR = request.CostUSD * rate;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Currency service unavailable: {ex.Message}");
            }

            // Factory Pattern
            var handler = RequestHandlerFactory.Create(handlerType);
            handler.Handle(request);

            if (ModelState.IsValid)
            {
                _context.ServiceRequests.Add(request);
                _context.SaveChanges();
                return RedirectToAction("Details", new { id = request.ServiceRequestId });
            }
            ViewData["Contracts"] = _context.Contracts
                .Select(c => new
                {
                    c.ContractId,
                    Display = $"{c.ContractId} - {c.Client.Name}"
                })
                .ToList();
            return View(request);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var requests = _context.ServiceRequests
                .Include(r => r.Contract)
                .ThenInclude(c => c.Client)
                .ToList();
            return View(requests);
        }

        public IActionResult Details(int id)
        {
            var request = _context.ServiceRequests.FirstOrDefault(r => r.ServiceRequestId == id);
            if (request == null) return NotFound();
            return View(request);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var request = _context.ServiceRequests.Find(id);
            if (request == null) return NotFound();
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ServiceRequest request)
        {
            if (ModelState.IsValid)
            {
                var existing = _context.ServiceRequests.Find(request.ServiceRequestId);
                if (existing == null) return NotFound();

                var contract = _context.Contracts.Find(existing.ContractId);
                var errors = _validator.Validate(request, contract).ToList();
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                existing.Description = request.Description;
                existing.CostUSD = request.CostUSD;
                existing.Status = request.Status;

                try
                {
                    var rate = await _currencyService.GetUsdToZarRateAsync();
                    existing.CostZAR = existing.CostUSD * rate;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Currency service unavailable: {ex.Message}");
                    return View(request);
                }

                _context.SaveChanges();
                return RedirectToAction("Details", new { id = existing.ServiceRequestId });
            }
            return View(request);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var request = _context.ServiceRequests.Find(id);
            if (request == null) return NotFound();
            return View(request);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var request = _context.ServiceRequests.Find(id);
            if (request != null)
            {
                _context.ServiceRequests.Remove(request);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Client");
        }
    }
}