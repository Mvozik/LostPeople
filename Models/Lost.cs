using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zadanko3.Models
{
    public class Lost
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Nie podano Imienia!")]
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Wiek { get; set; }
        public PlecList Plec { get; set; }
        public enum PlecList { Brak, M, K }
        public double Wzrost { get; set; }
        public string Miejscowość { get; set; }
        public string Ulica { get; set; }
        public string NumerKontaktowy { get; set; }
        public enum WojewództwoList { Wszystkie, Dolnośląskie, KujawskoPomorskie, Lubelskie, Lubuskie, Łódzkie, Małopolskie, Mazowieckie, Opolskie, Podkarpackie, Podlaskie, Pomorskie, Śląskie, Świętkorzyskie, WarmińskoMazurskie, Wielkopolskie, Zachodniopomorskie }
        public WojewództwoList WojewództwoLista { get; set; }
        public string Img { get; set; }
        public string NazwaUser { get; set; }
        public string Opis { get; set; }
        
    }
}
