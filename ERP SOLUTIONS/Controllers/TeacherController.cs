using ERP_SOLUTIONS.Data;
using ERP_SOLUTIONS.Models.Entities;
using ERP_SOLUTIONS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;


namespace ERP_SOLUTIONS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly AppDbContext _context;

        public TeacherController(ITeacherService teacherService, AppDbContext context)
        {
            _teacherService = teacherService;
            _context = context;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var teachers = await _teacherService.GetAllAsync();
        //    return View(teachers);
        //}

        public async  Task<IActionResult> Index(string search, int page = 1, int pageSize = 10)
        {
            // Start with all teachers
            var teachers = await _teacherService.GetAllAsync();
            //var teachers = _context.Teachers.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                teachers = teachers.Where(t =>
                    t.TeacherName.Contains(search) ||
                    t.Phone.Contains(search) ||
                    t.EmailID.Contains(search) ||
                    t.Qualification.Contains(search));
            }

            // Count total records after filter
            int totalRecords = teachers.Count();

            // Apply paging
            var pagedTeachers = teachers
                .OrderBy(t => t.TeacherName)           // Sort by name
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Pass data + pagination info to view
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            ViewBag.Search = search;

            return View(pagedTeachers);
        }

        public IActionResult Create()
        {
            return View(new Teacher());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Teacher model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var data = await _teacherService.CreateTeacherWithUserAsync(model);
                    if(data.username == "")
                    {
                        ModelState.AddModelError("", "Teacher already exists.");
                        return View(model);
                    }
                    TempData["SuccessMessage"] = $"Teacher {model.FullName} added successfully! Username: {data.username}, and Password: {data.password}";
                    return RedirectToAction("Index");
                }
                catch ( Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await _teacherService.GetByIdAsync(id);
            return View("Create", teacher);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Teacher model)
        {
            var result = await _teacherService.UpdateAsync(model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            await _teacherService.DeactivateAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateStatus([FromBody] Teacher model)
        {
            var teacher = _context.Teachers.Find(model.TeacherID);

            if (teacher == null)
                return Json(new { success = false });

            teacher.Status = model.Status;
            teacher.StatusRemark = model.StatusRemark;
            teacher.StatusUpdatedOn = DateTime.Now;

            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}
