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

// ��������� ������� ��� ��������������
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // ��������� ���� � �������� �����
    });

// ��������� ������� ��� ����������� � Razor Pages
builder.Services.AddRazorPages()
    .AddViewLocalization();

var app = builder.Build();

// ��������� �������� �� ���������
var supportedCultures = new[]
{
    new CultureInfo("en-US"), // ��������� ����������� �����
    new CultureInfo("ru-RU"), // ��������� �������� �����
    new CultureInfo("kz-KZ") // ��������� ���������� �����
};

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ru-RU"), // ������������� ������� ���� ��� �������� �� ���������
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);

// ��������� ���������� � HTTPS-���������������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ��������� middleware �������������� � �����������
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

// Run the application.
app.Run();

// ���������� � ������ 
public class HotelsController : Controller
{
    // GET: /Hotels
    public IActionResult Index()
    {
        // ��������� ������ �� ������ (��������, �� ���� ������)
        var hotels = GetHotelsFromDatabase();

        // ����������� ������ � �������������
        return View(hotels);
    }

    // ����� ��� ��������� ������ �� ������ (��������)
    private List<Hotel> GetHotelsFromDatabase()
    {
        // � �������� ���������� ����� ����� ������ � ���� ������
        return new List<Hotel>
        {
            new Hotel { Id = 1, Name = "����� 1", Address = "����� ����� 1", PhoneNumber = "123-456-7890" },
            new Hotel { Id = 2, Name = "����� 2", Address = "����� ����� 2", PhoneNumber = "456-789-0123" },
            new Hotel { Id = 3, Name = "����� 3", Address = "����� ����� 3", PhoneNumber = "789-012-3456" }
        };
    }

    // GET: /Hotels/SendMessage
    [ServiceFilter(typeof(AuthFilter))] // ��������� ������ �������������� � ��������
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
            // ��������� ���������
            var message = model.Message;

            // ����� �� ������ ��������� ������ �������� � ���������� (��������, ��������� ��� � ���� ������)

            return RedirectToAction("Success"); // ��������������� �� �������� �������� �������� ���������
        }
        else
        {
            return View("SendMessage", model); // ����������� ����� ����� � ����������� �� ������� ���������
        }
    }

    // GET: /Hotels/Success
    public IActionResult Success()
    {
        // ������� ��������� �� �������� ��������
        TempData["SuccessMessage"] = "�������� ������� ���������!";

        // ������������� ������������ �� �������� ��������
        return RedirectToAction("Index", "Home");
    }
}

// ������ ��������������
public class AuthFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // ���������, ���������������� �� ������������
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            // ���� ���, ���������� ��� �� �������� �����
            context.Result = new RedirectToActionResult("Login", "Account", null);
        }
    }
}