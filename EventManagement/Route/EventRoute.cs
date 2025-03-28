using Employee.Models;
using Event.Models;
using Event.Data;
using Microsoft.EntityFrameworkCore;

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

        if (req.EmployeesId.Any()) // Verifica se a lista de Id não está vazia 
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

        var event_ = new EventModel(req.Name, req.Price, req.Date, req.Services, listEmployees);
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
    route.MapPut("{id:guid}", async (Guid id, EventRequest req, EventContext context) =>
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

    });

    // Edita somente os funcionários de um evento

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

    // Hard Delete (não funciona)
    route.MapDelete("{id:guid}/hard", async (Guid id, EventContext context) =>
    {
      var event_ = await context.Events.FirstOrDefaultAsync(event_ => event_.Id == id);

      if (event_ == null)
      {
        return Results.NotFound();
      }

      context.RemoveRange(event_.Employees); // Remove funcionários associados ao evento
      context.Events.Remove(event_); // Remove o evento

      await context.SaveChangesAsync();
      return Results.Ok($"Evento {id} removido com sucesso.");
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
      return Results.Ok($"Funcionário {id} removido com sucesso.");
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
      return Results.Ok($"Funcionário {id} removido com sucesso.");
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