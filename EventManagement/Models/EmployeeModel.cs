namespace Employee.Models;

using System.Text.Json.Serialization;
using Event.Models;
public class EmployeeModel
{
  public EmployeeModel(string name)
  {
    Id = Guid.NewGuid();
    Name = name;
  }

  /*init : o valor pode ser definido apenas no construtor ou 
  na inicialização do objeto, mas não pode ser alterado depois.*/
  public Guid Id { get; init; }
  public string Name { get; private set; }

  [JsonIgnore] // Pra evitar circular reference no JSON
  public List<EventModel> Events { get; set; } = new();
  public void ChangeEmployeeName(string name)
  {
    Name = name;
  }

  public void SetInactive()
  {
    Name = "(Inativo)" + Name;
  }
}