using Employee.Models;
namespace Event.Models;

public class EventModel
{
  /* Construtor padrão (sem parâmetros) necessário para o EF Core
  O EF Core precisa de um construtor sem parâmetros para instanciar o 
  modelo durante as operações de migração e manipulação de dados.
  */
  public EventModel()
  {
    Services = new List<string>();
    Employees = new List<EmployeeModel>();
  }

  public EventModel(string name, decimal price, DateTime date, List<string> services, List<EmployeeModel> employees)
  {
    Id = Guid.NewGuid();
    Contracting = name;
    Price = price;
    Date = date;
    Services = services;
    Employees = employees;
  }

  public Guid Id { get; init; }

  public string Contracting { get; private set; }

  public decimal Price { get; set; }

  public DateTime Date { get; set; }

  public List<string> Services { get; set; }
  public List<EmployeeModel> Employees { get; private set; }

  public void ChangeContractingName(string name)
  {
    Contracting = name;
  }

  public void SetInactive()
  {
    Contracting = "(Inativo)"+Contracting;
  }

  public void AddService(string service)
  {
    if (!Services.Contains(service)) // Verifica se o serviço já está na lista
    {
      Services.Add(service);
    }
  }

  public void AddEmployee(EmployeeModel employee)
  {
    /* Any() : verifica se existe pelo menos um elemento na lista que 
    atenda as condições dentro dos parênteses.

    Existe algum funcionário "e" com mesmo Id do funcionário "employee.Id" que
    estamos tentando adicionar? Se sim, retorna true, que negado fica false. 
    Se não, retorna false que negado fica true.
    */
    if (!Employees.Any(e => e.Id == employee.Id))
    {
      Employees.Add(employee);
    }
  }
}

  /* 
  Guid: identificador global exclusivo, inteiro de 128 bits (16bytes)
  identificador único pra criar Id. Toda vez que executa ele gera um id novo.

  private : A única parte da aplicação responsável por cuidar das propriedades
  que estão no modelo é o próprio modelo.
  */