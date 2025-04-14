using Xunit;
using Event.Models;
using Employee.Models;

namespace UnitTests.Tests;

/* AAA Pattern --> Arrange - Act - Assert
   Arrange: Prepara o cenário para o teste, criando os objetos necessários.
   Act: Executa a ação que está sendo testada.
   Assert: Verifica se o resultado esperado ocorreu.
*/

public class EventModelTests
{
    [Fact]
    public void AddEmployee() // Deve adicionar apenas uma vez
    {
        var evento = new EventModel("Evento", 1000, DateTime.Now, new List<string>(), new List<EmployeeModel>());
        var funcionário = new EmployeeModel("Joãozinho");

        evento.AddEmployee(funcionário);
        evento.AddEmployee(funcionário); // Adiciona novamente o mesmo funcionário

        Assert.Single(evento.Employees); // Verifica se o funcionário foi adicionado apenas uma vez
    }
}