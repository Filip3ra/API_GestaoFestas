using Employee.Models;

namespace Event.Models;

public class EventModel
{

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

  /* Guid: identificador global exclusivo, inteiro de 128 bits (16bytes)
  identificador único pra criar Id. Toda vez que executa ele gera um id novo.

  "string?" :  a ? é um marcador de nulo, pra avisar que a variável pode ser nula.
  Mas também posso resolver isso colocando " = String.Empty;" ao final.
  Ou, se tenho um construtor eu não preciso de marcador nulo, pois já tenho inicialização que é o construtor.

  init : Vai funcionar somente quando tiver construtor, só posso alterar ele uma vez.

  private : a única parte da aplicação responsável por cuidar das propriedades
  que estão no modelo é o próprio modelo.

  */