using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace voorhent.Pages
{
    public class ContactModel : PageModel
    {
        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат адреса электронной почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Сообщение обязательно")]
        public string Message { get; set; }

        // Добавленное свойство PhoneNumber
        [Required(ErrorMessage = "Номер телефона обязателен")]
        [Phone(ErrorMessage = "Неверный формат номера телефона")]
        public string PhoneNumber { get; set; }
    }
}
