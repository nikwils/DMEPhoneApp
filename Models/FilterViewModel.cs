using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DMEPhoneApp.Models
{
    public class FilterViewModel
    {
        public FilterViewModel(string lastName, DateTime dateOfBirthFrom, DateTime dateOfBirthTo) { 
            // устанавливаем начальный элемент, который позволит выбрать всех
            SelectedLastName = lastName;
            SelectedDateOfBirthFrom = dateOfBirthFrom;
            SelectedDateOfBirthTo = dateOfBirthTo;
        }
        public DateTime SelectedDateOfBirthFrom { get; } // выбранный возраст
        public DateTime SelectedDateOfBirthTo { get; } // выбранный возраст

        public string SelectedLastName { get; } // введенное имя
    }
}
