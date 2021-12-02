using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebStore.ViewModels
{
    public class EmployeeViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Не указана фамилия")]
        [StringLength(200, MinimumLength =2, ErrorMessage = "Длина фамилии должна быть от 2 до 200 символов")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)| ([A-Z][a-z]+)", ErrorMessage = "Строка имеет неверный формат (Что-то пошло не так) ")]
        public string LastName { get; set; }
        [Display(Name = "Имя")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Длина имени должна быть от 2 до 200 символов")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)| ([A-Z][a-z]+)", ErrorMessage = "Строка имеет неверный формат (Что-то пошло не так) ")]
        public string Name { get; set; }
        [Display(Name = "Отчество")]
        [StringLength(200, ErrorMessage = "Длина отчества должна быть от 2 до 200 символов")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)| ([A-Z][a-z]+)", ErrorMessage = "Строка имеет неверный формат (Что-то пошло не так) ")]
        public string Patronymic { get; set; }
        [Display(Name = "Возраст")]
        [Range(18, 80, ErrorMessage = "Возраст должен быть от 18 до 80 лет")]
        public int Age { get; set; }

    }
}
