﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Patronymic { get; set; }

        [Column(TypeName ="decimal(18,2)")]
        public int Age { get; set; }
    }
    //public record Employee2(int Id, string FirstName, string LastName, string Patronymic, int Age);
}
