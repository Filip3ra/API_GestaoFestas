/* 
Esse aqrquivo de requisição é uma camada intermediária entre 
a API(requisições do usuário) e o modelo de domínio (EventModel)

> Facilita validação e transformação antes de salvar no banco.
> Separar a estrutura da API da estrutura do banco é uma boa prática.
*/

namespace Event.Request;

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

public record LoginRequest(string Username, string Password);
public record UserUpdateRequest(string? Username, string? Password);