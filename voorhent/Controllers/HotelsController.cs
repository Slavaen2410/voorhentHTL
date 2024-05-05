using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using voorhent.Models;
using Voorhent.ViewModels;

namespace voorhent.Controllers
{
    public class HotelsController : Controller
    {
        // GET: /Hotels
        public IActionResult Index()
        {
            // Получение данных из модели (например, из базы данных)
            var hotels = GetHotelsFromDatabase();

            // Возвращение данных в представление
            return View(hotels);
        }

        // Метод для получения данных об отелях (заглушка)
        private List<Hotel> GetHotelsFromDatabase()
        {
            // В реальном приложении здесь будет запрос к базе данных
            return new List<Hotel>
            {
                new Hotel { Id = 1, Name = "Отель 1", Address = "Адрес отеля 1", PhoneNumber = "123-456-7890" },
                new Hotel { Id = 2, Name = "Отель 2", Address = "Адрес отеля 2", PhoneNumber = "456-789-0123" },
                new Hotel { Id = 3, Name = "Отель 3", Address = "Адрес отеля 3", PhoneNumber = "789-012-3456" }
            };
        }

        // GET: /Hotels/SendMessage
        [ServiceFilter(typeof(AuthFilter))] // Применяем фильтр аутентификации к действию
        public IActionResult SendMessage()
        {
            return View();
        }

        // POST: /Hotels/ProcessMessage
        [HttpPost]
        public IActionResult ProcessMessage(SendMessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Обработка сообщения
                var message = model.Message;

                // Здесь вы можете выполнить нужные действия с сообщением (например, сохранить его в базу данных)

                return RedirectToAction("Success"); // Перенаправление на страницу успешной отправки сообщения
            }
            else
            {
                return View("SendMessage", model); // Отображение формы снова с сообщениями об ошибках валидации
            }
        }

        // GET: /Hotels/Success
        public IActionResult Success()
        {
            // Вывести сообщение об успешной операции
            TempData["SuccessMessage"] = "Операция успешно выполнена!";

            // Перенаправить пользователя на домашнюю страницу
            return RedirectToAction("Index", "Home");
        }
    }

    // Фильтр аутентификации
    public class AuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Проверяем, аутентифицирован ли пользователь
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                // Если нет, отправляем его на страницу входа
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}

