using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using voorhent.Models;
using Voorhent.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы для аутентификации
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Указываем путь к странице входа
    });

// Добавляем сервисы для локализации и Razor Pages
builder.Services.AddRazorPages()
    .AddViewLocalization();

var app = builder.Build();

// Установка культуры по умолчанию
var supportedCultures = new[]
{
    new CultureInfo("en-US"), // Поддержка английского языка
    new CultureInfo("ru-RU"), // Поддержка русского языка
    new CultureInfo("kz-KZ") // Поддержка казахского языка
};

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ru-RU"), // Устанавливаем русский язык как культуру по умолчанию
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);

// Обработка исключений и HTTPS-перенаправление
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Добавляем middleware аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

// Run the application.
app.Run();

// Контроллер и фильтр 
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