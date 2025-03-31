using Employee.Models;
using Event.Models;
using Event.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Event.Routes;

public static class EventRoute
{
  /* this : extension method, uso a própria instância como método de extensão.
    onde tenho um método base e extendo ele a outros recursos e métodos.

    async : Sempre que mexer em banco de dados tem que ser assíncrono,
    ou seja, a requisição aqui é assíncrona

    await : usado para esperar a conclusão de uma operação assíncrona sem
    bloquear a thread pricnipal.
  */
  public static void EventRoutes(this WebApplication app)
  {

    var route = app.MapGroup("event");

    route.MapPost("", async (EventRequest req, EventContext context) =>
      {
        List<EmployeeModel> listEmployees = new List<EmployeeModel>();

        if (req.EmployeesId != null && req.EmployeesId.Any()) // Verifica se a lista de Id não está vazia 
        {
          // Contains : busca todos os funcionários cujos Id estão na lista passada
          listEmployees = await context.EmployeeModel
         .Where(e => req.EmployeesId.Contains(e.Id))
         .ToListAsync();

          // Verifica se todos os funcionários informados existem 
          if (listEmployees.Count != req.EmployeesId.Count)
          {
            return Results.BadRequest("Um ou mais funcionários informados não existem.");
          }
        }

        if (req.Name == null || req.Price == null || req.Date == null || req.Services == null)
        {
          return Results.BadRequest("Algum campo nulo fornecido.");
        }

        var event_ = new EventModel(req.Name, req.Price.Value, req.Date.Value, req.Services, listEmployees);
        await context.AddAsync(event_); // Adiciona esse evento no context
        await context.SaveChangesAsync(); // Commit para adicionar de fato no banco de dados
        return Results.Ok($"O evento da {event_.Contracting} foi cadastrado com sucesso.");
      });

    // Get dos eventos com funcionários alocados para os eventos
    route.MapGet("", async (EventContext context) =>
    {
      // Include : inclui os funcionários cadastrados no evento
      var events_ = await context.Events.Include(e => e.Employees).ToListAsync();
      return Results.Ok(events_);
    });

    // Edita nome do contratante, mas deveria ser patch
    /*route.MapPut("{id:guid}", async (Guid id, EventRequest req, EventContext context) =>
    {
      //var event_ = await context.Events.FindAsync(id);
      var event_ = await context.Events.FirstOrDefaultAsync(event_ => event_.Id == id);

      if (event_ == null)
      {
        return Results.NotFound();
      }

      event_.ChangeContractingName(req.Name);
      await context.SaveChangesAsync();
      return Results.Ok(event_);

    });*/

    // Edita atributos do evento
    route.MapPatch("", async (Guid id, EventRequest req, EventContext context) =>
    {
      // Verifica existânci de um evento
      var event_ = await context.Events.FirstOrDefaultAsync(e => e.Id == id);

      if (event_ == null)
      {
        return Results.NotFound();
      }

      // Se foi fornecido id válido de um funcionário
      if (req.EmployeesId != null && req.EmployeesId.Any())
      {
        var listEmployees = await context.EmployeeModel
        .Where(e => req.EmployeesId.Contains(e.Id))
        .ToListAsync();

        // Se a quantidade de funcionários encontrados for diferente da quantidade de ID enviado, algum deles não existe
        if (listEmployees.Count != req.EmployeesId.Count)
        {
          return Results.BadRequest("Um ou mais funcionários informados não existem.");
        }

        // Pra cada funcionário da lista, adiciona ele ao evento
        listEmployees.ForEach(employee => event_.AddEmployee(employee));
      }

      // Só altera se o valor foi passado na requisição
        if (!string.IsNullOrWhiteSpace(req.Name))
        {
          event_.ChangeContractingName(req.Name);
        }

      // Se Price foi fornecido, altera
      if (req.Price.HasValue)
      {
        event_.Price = req.Price.Value;
      }

      // Se Date foi fornecido, altera
      if (req.Date.HasValue)
      {
        event_.Date = req.Date.Value;
      }

      // Verifica se a lista está na requisição e tem algum valor antes de atualizar
      if (req.Services != null && req.Services.Any())
      {
        event_.Services = req.Services;
      }

      await context.SaveChangesAsync();
      return Results.Ok(event_);
    });

    // Soft Delete
    route.MapDelete("{id:guid}/soft", async (Guid id, EventContext context) =>
    {
      var event_ = await context.Events.FirstOrDefaultAsync(event_ => event_.Id == id);

      if (event_ == null)
      {
        return Results.NotFound();
      }

      event_.SetInactive();
      await context.SaveChangesAsync();
      return Results.Ok(event_);

    });

    // Hard Delete 
    route.MapDelete("{id:guid}/hard", async (Guid id, EventContext context) =>
    {
      //var event_ = await context.Events.FirstOrDefaultAsync(event_ => event_.Id == id);

      var event_ = await context.Events
      .Include(e => e.Employees)
      .FirstOrDefaultAsync(e => e.Id == id);

      if (event_ == null)
      {
        return Results.NotFound($"Evento {id} não encontrado.");
      }

      event_.Employees.Clear(); // Remove associação de funcionários ao evento
      await context.SaveChangesAsync(); // Garante remover a associação antes de remover o evento

      context.Events.Remove(event_); // Remove o evento
      await context.SaveChangesAsync();

      return Results.Ok($"Evento {id} removido com sucesso.");
    });

    // Remove funcionário de um evento
    route.MapDelete("{eventId:guid}/employee/{employeeId:guid}", async (Guid eventId, Guid employeeId, EventContext context) =>
    {
      // Buscar o evento e incluir a lista de funcionários 
      var event_ = await context.Events
      .Include(e => e.Employees.Where(emp => emp.Id == employeeId))
      .FirstOrDefaultAsync(e => e.Id == eventId);

      if (event_ == null)
      {
        return Results.NotFound($"Evento com ID {eventId} não encontrado.");
      }

      // Se o funcionário não estiver na lista, ele não faz parte do evento
      var employee = event_.Employees.FirstOrDefault();
      if (employee == null)
      {
        return Results.NotFound($"O funcionário com ID {employeeId} não está associado a este evento.");
      }

      // Remove a associação
      event_.Employees.Remove(employee);
      await context.SaveChangesAsync();
      return Results.Ok($"Funcionário {employee.Name} foi removido do evento {event_.Contracting} com sucesso.");
    });


    //----------------------------------------EMPLOYEE----------------------------------------

    var routeEmployee = app.MapGroup("employee");

    // Post de funcionários
    routeEmployee.MapPost("", async (EmployeeRequest req, EventContext context) =>
    {
      // Busca no banco todos que atendem a pelo menos uma das condições
      var listEmployees = await context.EmployeeModel
      .Where(e => e.Name == req.Name)
      .ToListAsync();

      // dentro de listEmployees tem algum funcionário de mesmo nome?
      var existingEmployee = listEmployees.FirstOrDefault(e => e.Name == req.Name);

      // Se tem funcionário com mesmo nome = BadRequest
      if (existingEmployee != null)
      {
        return Results.BadRequest("Já existe um funcionário com esse nome.");
      }

      var employee_ = new EmployeeModel(req.Name);
      await context.AddAsync(employee_);
      await context.SaveChangesAsync();
      return Results.Ok(employee_);
    });

    // Get todos os funcionários
    routeEmployee.MapGet("", async (EventContext context) =>
    {
      var employees_ = await context.EmployeeModel.ToListAsync();
      return Results.Ok(employees_);
    });

    // Get eventos de um funcionário
    routeEmployee.MapGet("{id:guid}/events", async (Guid id, EventContext context) =>
    {
      var employee = await context.EmployeeModel
      .Include(e => e.Events)
      .FirstOrDefaultAsync(e => e.Id == id);

      if (employee == null)
      {
        return Results.NotFound($"Funcionário com id {id} não encontrado.");
      }

      return Results.Ok(employee.Events);
    });

    // Edita nome funcionário (não pode ter mesmo nome que outro já cadastrado)
    routeEmployee.MapPut("{id:guid}", async (Guid id, EmployeeRequest req, EventContext context) =>
    {
      // Busca no banco todos que atendem a pelo menos uma das condições
      var employee_ = await context.EmployeeModel
      .Where(e => e.Id == id || e.Name == req.Name)
      .ToListAsync();

      // dentro de employee tem algum elemento que satisfaça essas condições?
      var existingEmployee = employee_.FirstOrDefault(e => e.Name == req.Name && e.Id != id);
      var employeeToUpdate = employee_.FirstOrDefault(e => e.Id == id);

      // Se tem funcionário com mesmo nome = BadRequest
      if (existingEmployee != null)
      {
        return Results.BadRequest("Já existe um funcionário com esse nome.");
      }

      // Se não tem funcionário com mesmo id que passei = NotFound
      if (employeeToUpdate == null)
      {
        return Results.NotFound($"O funcionário com id: {id} não foi encontrado.");
      }

      // Se tem funcionário com id que passei = altera o nome
      employeeToUpdate.ChangeEmployeeName(req.Name);
      await context.SaveChangesAsync();
      return Results.Ok(employee_);
    });
    
    // Soft Delete 
    routeEmployee.MapDelete("{id:guid}/soft", async (Guid id, EventContext context) =>
    {
      var employee_ = await context.EmployeeModel.FirstOrDefaultAsync(event_ => event_.Id == id);

      if (employee_ == null)
      {
        return Results.NotFound($"O funcionário com id: {id} não foi encontrado.");
      }

      employee_.SetInactive();
      await context.SaveChangesAsync();
      return Results.Ok($"Funcionário(a) {employee_.Name} INATIVADO com sucesso.");
    });

    // Hard Delete 
    routeEmployee.MapDelete("{id:guid}/hard", async (Guid id, EventContext context) =>
    {
      var employee_ = await context.EmployeeModel.FirstOrDefaultAsync(event_ => event_.Id == id);

      if (employee_ == null)
      {
        return Results.NotFound($"O funcionário com id: {id} não foi encontrado.");
      }

      context.EmployeeModel.Remove(employee_); // Remove funcionário
      await context.SaveChangesAsync();
      return Results.Ok($"Funcionário(a) {employee_.Name} REMOVIDO com sucesso.");
    });

  }
}

/*
              PARA MEDIR PERFORMANCE NO FUTURO

              FirstOrDefaultAsync("expressão") : O EF realiza uma consulta no bd e verifica se existe
              algum registro na tabela "EmployeeModel" que atenda a condição da "expressão".      


              var existingEmployee = await context.EmployeeModel.
              FirstOrDefaultAsync(existingEmployee => existingEmployee.Name == req.Name && existingEmployee.Id != id);

              if (existingEmployee != null)
              {
                return Results.BadRequest("Já existe um funcionário com esse nome.");
              }

              var employee_ = await context.EmployeeModel.FirstOrDefaultAsync(employee_ => employee_.Id == id);

              if (employee_ == null)
              {
                return Results.NotFound();
              }

              employee_.ChangeEmployeeName(req.Name);
              await context.SaveChangesAsync();
              return Results.Ok(employee_);


        */