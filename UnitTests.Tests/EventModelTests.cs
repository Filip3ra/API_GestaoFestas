using Xunit;
using Event.Models;
using Employee.Models;

namespace UnitTests.Tests;

/* AAA Pattern --> Arrange - Act - Assert
   Arrange: Prepara o cenário para o teste, criando os objetos necessários.
   Act: Executa a ação que está sendo testada.
   Assert: Verifica se o resultado esperado ocorreu.

   Para testar todos: dotnet test
   Para testar um método específico: dotnet test --filter "Nome do método"
*/

public class EventModelTests
{
    [Fact]
    public void AddEmployee() // Deve adicionar apenas uma vez
    {
        // Arrange
        var evento = new EventModel("Evento", 1000, DateTime.Now, new List<string>(), new List<EmployeeModel>());
        var funcionário = new EmployeeModel("Joãozinho");

        // Act
        evento.AddEmployee(funcionário);
        evento.AddEmployee(funcionário); // Adiciona novamente o mesmo funcionário

        // Assert
        Assert.Single(evento.Employees); // Verifica se o funcionário foi adicionado apenas uma vez
    }

    [Fact]
    public void SetInactive() // Deve adicionar prefixo inativo ao contratante
    {
        // Arrange
        var evento = new EventModel("Evento Teste", 1000, DateTime.Now, new List<string>(), new List<EmployeeModel>());

        // Act
        evento.SetInactive();

        // Assert
        Assert.StartsWith("(Inativo)", evento.Contracting); // Verifica se o nome do contratante começa com "(Inativo)"
    }
}