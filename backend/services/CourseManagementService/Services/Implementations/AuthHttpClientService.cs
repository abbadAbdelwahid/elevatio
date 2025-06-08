using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CourseManagementService.ExternalServices
{
    public class AuthHttpClientService
    {
        private readonly HttpClient _http;
        private readonly string _authServiceBaseUrl;

        public AuthHttpClientService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _authServiceBaseUrl = config["Other-Microservices-EndPoints:AuthService-EndPoint"]
                                  ?? throw new ArgumentNullException("AuthService-EndPoint missing from config");
        }

        public async Task<string> GetTeacherFullNameAsync(int teacherId)
        {
            try
            {
                var response = await _http.GetAsync($"{_authServiceBaseUrl}/api/Enseignants/{teacherId}/fullname");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                return doc.RootElement.GetProperty("fullName").GetString() ?? "Inconnu";
            }
            catch
            {
                return "Inconnu";
            }
        }
        public async Task<string> GetStudentFullNameAsync(int studentId)
        {
            try
            {
                var response = await _http.GetAsync($"{_authServiceBaseUrl}/api/Etudiants/{studentId}/fullname");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                return doc.RootElement.GetProperty("fullName").GetString() ?? "Inconnu";
            }
            catch
            {
                return "Inconnu";
            }
        }

        
    }
}