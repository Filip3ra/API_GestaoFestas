namespace Employee.Models;

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
}