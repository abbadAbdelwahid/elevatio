// Controllers/AuthController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AuthService.Models;
using AuthService.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Endpoint pour réinitialiser le mot de passe
        [Authorize]  // Autorise uniquement les utilisateurs authentifiés
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound("Utilisateur non trouvé");

            // Vérifier que l'ancien mot de passe est correct
            var result = await _signInManager.PasswordSignInAsync(user, model.OldPassword, false, false);
            if (!result.Succeeded)
                return BadRequest("Ancien mot de passe incorrect");

            // Réinitialiser le mot de passe
            var resetResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (resetResult.Succeeded)
                return Ok("Mot de passe réinitialisé avec succès");

            return BadRequest("Erreur lors de la réinitialisation du mot de passe");
        }
        
        // [Authorize]
        [HttpGet("getIdAndRole")]
        public async Task<IActionResult> GetIdAndRole()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Récupérer l'ID depuis le token

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Utilisateur non trouvé");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault(); // Supposé avoir un seul rôle

            return Ok(new
            {
                userId = user.Id,
                role = role  // Chaîne simple, pas un tableau
            });
        }

        
        [Authorize]
        [HttpGet("isTokenValid")]
        public IActionResult IsTokenValid()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Si l'ID utilisateur est présent dans le token, le token est valide
            if (!string.IsNullOrEmpty(userId))
                return Ok(new { isValid = true, userId });

            return Ok(new { isValid = false });
        }



        
    }
}