using JobPortalApp1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalApp1.Controllers
{
    public class NotificationController : Controller
    {
        private readonly jobPortalNewContext _context;

        public NotificationController(jobPortalNewContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ViewNotifications(string filter = null)
        {
            var notifications = _context.Notifications
                .Include(n => n.Company)
                .Include(n => n.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                notifications = notifications.Where(n => n.Status == filter);
            }

            return View(notifications.ToList());
        }

        // Add actions for MarkAsRead and DeleteNotification here if needed
    }
}

