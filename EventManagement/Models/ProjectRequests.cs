using Employee.Models;
namespace Event.Models;

public record EventRequest(
  string Name,
  decimal Price,
  DateTime Date,
  List<string> Services,
  List<EmployeeRequest> Employees
);

public record EmployeeRequest(
  string Name
);