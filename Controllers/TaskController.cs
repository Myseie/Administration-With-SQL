using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Administration.Controllers
{
   
    public class TaskController : Controller
    {
        // En enkel lista för att lagra uppgifter
        private static List<string> Tasks = new List<string>();

        public IActionResult Index()
        {
            return View(Tasks); // Returnerar vyn med listan av uppgifter
        }

        [HttpPost]
        public IActionResult AddTask(string newTask)
        {
            if (!string.IsNullOrWhiteSpace(newTask))
            {
                Tasks.Add(newTask);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveTask(int index)
        {
            if (index >= 0 && index < Tasks.Count)
            {
                Tasks.RemoveAt(index);
            }

            return RedirectToAction("Index");
        }
    }
}