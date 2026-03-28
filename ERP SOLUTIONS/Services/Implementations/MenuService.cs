using ERP_SOLUTIONS.Data;
using ERP_SOLUTIONS.Models.Entities;
using ERP_SOLUTIONS.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ERP_SOLUTIONS.Services.Implementations
{
    public class MenuService
    {
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _context;

        public MenuService(IMemoryCache cache, AppDbContext context)
        {
            _cache = cache;
            _context = context;
        }

        public MenuPageVM GetAllMenus()
        {
            var sections = _context.MenuSections
                .Include(s => s.Items)
                .Select(s => new MenuSectionVM
                {
                    Id = s.Id,
                    Title = s.Title,
                    Icon = s.Icon,
                    Color = s.Color,
                    Subtitle = s.Subtitle,
                    IsActive = s.IsActive,
                    ExistingItems = s.Items.Select(i => new MenuItemVM
                    {
                        Id = i.Id,
                        Title = i.Title,
                        Description = i.Description,
                        Icon = i.Icon,
                        Url = i.Url,
                        IsActive = i.IsActive
                    }).ToList()
                }).ToList();

            var vm = new MenuPageVM
            {
                ExistingSections = sections
            };

            return vm;
        }


        public List<MenuSection> GetActiveMenus()
        {
            return _cache.GetOrCreate("ActiveMenus", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                return _context.MenuSections
                    .Where(s => s.IsActive)
                    .Include(s => s.Items.Where(i => i.IsActive))
                    .ToList();
            });
        }

        /*
         must clear the cache when: ( _cache.Remove("ActiveMenus"))
         1. ✅ 1. Add / Create
         2. ✅ 2. Update
         3. ✅ 3. Delete
         */
        public void AddSection(MenuSection section)
        {
            _context.MenuSections.Add(section);
            _context.SaveChanges();

            // 🔥 Clear cache after change
            _cache.Remove("ActiveMenus");
        }

        public void UpdateSection(MenuSection section)
        {
            _context.MenuSections.Update(section);
            _context.SaveChanges();

            _cache.Remove("ActiveMenus");
        }

        public void DeleteSection(int id)
        {
            var section = _context.MenuSections.Find(id);
            if (section != null)
            {
                _context.MenuSections.Remove(section);
                _context.SaveChanges();

                _cache.Remove("ActiveMenus");
            }
        }

        public string CreateMenu(MenuSectionVM vm)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // ✅ 1. Duplicate Section Check
                bool sectionExists = _context.MenuSections
                    .Any(s => s.Title.ToLower() == vm.Title.ToLower());

                if (sectionExists)
                {
                    return "Section already exists.";
                }

                // 🔹 Save Section
                var section = new MenuSection
                {
                    Title = vm.Title,
                    Icon = vm.Icon,
                    Color = vm.Color,
                    Subtitle = vm.Subtitle,
                    IsActive = vm.IsActive
                };

                _context.MenuSections.Add(section);
                _context.SaveChanges();

                // ✅ 2. Remove duplicate items from request (same title)
                var uniqueItems = vm.Items
                    .GroupBy(i => i.Title?.Trim().ToLower())
                    .Select(g => g.First())
                    .ToList();

                // 🔹 Save Items
                foreach (var item in vm.Items)
                {
                    // ✅ 3. Duplicate check in DB (same section + title)
                    bool itemExists = _context.MenuItems.Any(i =>
                        i.SectionId == section.Id &&
                        i.Title == item.Title);

                    if (itemExists)
                        continue; // skip duplicate

                    var menuItem = new MenuItem
                    {
                        SectionId = section.Id,
                        Title = item.Title,
                        Description = item.Description,
                        Icon = item.Icon,
                        Url = item.Url,
                        IsActive = item.IsActive
                    };

                    _context.MenuItems.Add(menuItem);
                }

                _context.SaveChanges();

                // ✅ Commit transaction
                transaction.Commit();

                // 🔥 Clear cache
                _cache.Remove("ActiveMenus");

                return "Menu added.";
            }
            catch (Exception ex)
            {
                // ❌ Rollback everything if error
                transaction.Rollback();
                return ex.Message;
            }

            
        }

    }
}
