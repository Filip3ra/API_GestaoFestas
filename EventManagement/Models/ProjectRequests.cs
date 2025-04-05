/* 
Esse aqrquivo de requisição é uma camada intermediária entre 
a API(requisições do usuário) e o modelo de domínio (EventModel)

> Facilita validação e transformação antes de salvar no banco.
> Separar a estrutura da API da estrutura do banco é uma boa prática.
*/

using Employee.Models;
namespace Event.Models;

public record EventRequest(
  string? Name,
  decimal? Price,
  DateTime? Date,
  List<string>? Services,
  List<Guid>? EmployeesId
);

public record EmployeeRequest(
  string Name
);