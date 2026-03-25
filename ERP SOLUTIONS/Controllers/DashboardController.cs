
using ERP_SOLUTIONS.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP_SOLUTIONS.Controllers
{
    [Authorize]
    //[NoCache]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            var sections = new List<DashboardSection>
    {
        new DashboardSection
        {
            Title = "Student Management",
            Icon = "bi-people",
            Color = "blue",
            Subtitle = "Tools for Manage student admission",
            Items = new List<DashboardItem>
            {
                new DashboardItem { Description="Complete acedamic and personal record management", Title="Student Record", Icon="bi-person-lines-fill", Url="/Student/Index" },
                new DashboardItem { Description="",Title="Enrollment Management", Icon="bi-journal-check", Url="/Enrollment/Index" },
                new DashboardItem { Description="",Title="Parent Portal", Icon="bi-people-fill", Url="/Parent/Index" }
            }
        },

        new DashboardSection
        {
            Title = "Academic Management",
            Icon = "bi-mortarboard",
            Color = "purple",
            Subtitle = "Academic operations & setup",
            Items = new List<DashboardItem>
            {
                new DashboardItem { Title="Classes", Icon="bi-door-open", Url="/Class/Index" },
                new DashboardItem { Title="Subject Assignment", Icon="bi-book", Url="/Subject/Assign" },
                new DashboardItem { Title="Academic Year Setup", Icon="bi-calendar", Url="/AcademicYear/Index" },
                new DashboardItem { Title="Grade Management", Icon="bi-award", Url="/Grade/Index" }
            }
        },

        new DashboardSection
        {
            Title = "Examination System",
            Icon = "bi-file-earmark-text",
            Color = "orange",
            Subtitle = "Manage exams & results",
            Items = new List<DashboardItem>
            {
                new DashboardItem { Title="Exams", Icon="bi-pencil-square", Url="/Exam/Index" },
                new DashboardItem { Title="Results", Icon="bi-bar-chart", Url="/Result/Index" }
            }
        },

        new DashboardSection
        {
            Title = "Attendance & Timetable",
            Icon = "bi-clock",
            Color = "green",
            Subtitle = "Track attendance & schedules",
            Items = new List<DashboardItem>
            {
                new DashboardItem { Title="Attendance", Icon="bi-check2-square", Url="/Attendance/Index" },
                new DashboardItem { Title="Timetable", Icon="bi-calendar3", Url="/Timetable/Index" }
            }
        },

        new DashboardSection
        {
            Title = "Faculty Management",
            Icon = "bi-person-badge",
            Color = "teal",
            Subtitle = "Manage faculty and reports",
            Items = new List<DashboardItem>
            {
                new DashboardItem { Title="Faculty Management", Icon="bi-person-workspace", Url="/Faculty/Index" },
                new DashboardItem { Title="Attendance Report", Icon="bi-graph-up", Url="/Faculty/Report" }
            }
        },

        new DashboardSection
        {
            Title = "Financial Management",
            Icon = "bi-cash-stack",
            Color = "red",
            Subtitle = "Fees & payment tracking",
            Items = new List<DashboardItem>
            {
                new DashboardItem { Title="Fee Management", Icon="bi-wallet2", Url="/Fees/Index" },
                new DashboardItem { Title="Payment Tracking", Icon="bi-credit-card", Url="/Payment/Tracking" },
                new DashboardItem { Title="Payment Reminder", Icon="bi-bell", Url="/Payment/Reminder" }
            }
        },

        new DashboardSection
        {
            Title = "Security & Administration",
            Icon = "bi-shield-lock",
            Color = "dark",
            Subtitle = "User roles & permissions",
            Items = new List<DashboardItem>
            {
                new DashboardItem { Title="Role", Icon="bi-person-lock", Url="/Role/Index" },
                new DashboardItem { Title="Assign Role", Icon="bi-person-plus", Url="/Role/Assign" },
                new DashboardItem { Title="Manage User", Icon="bi-people", Url="/User/Index" }
            }
        }
    };

            return View(sections);
        }
    }
}
