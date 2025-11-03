using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    //Datos requeridos para el registro de un nuevo usuario
    [Required]
    public string DisplayName { get; set; } = "";
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";

}
