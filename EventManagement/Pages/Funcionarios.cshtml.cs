using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class FuncionariosModel : PageModel
{
    private readonly HttpClient _httpClient;

    public List<Funcionario> Funcionarios { get; set; } = new();
    [BindProperty]
    public Funcionario NovoFuncionario { get; set; } = new();

    public FuncionariosModel(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5029"); // Ajuste a URL se necessário
    }

    public async Task OnGetAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/employees");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Funcionarios = JsonSerializer.Deserialize<List<Funcionario>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Funcionario>();
            }
        }
        catch
        {
            Funcionarios = new List<Funcionario>();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var json = JsonSerializer.Serialize(NovoFuncionario);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        await _httpClient.PostAsync("/employees", content);

        return RedirectToPage();
    }

    public class Funcionario
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
