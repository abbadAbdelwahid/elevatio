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
        
        [Authorize]
        [HttpGet("getRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new { roles });
        }

        
    }
}