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
  */
  public static void EventRoutes(this WebApplication app)
  {

    var route = app.MapGroup("event");

    route.MapPost("",
      async (EventRequest req, EventContext context) =>
      {
        var event_ = new EventModel(
          req.Name, req.Price, req.Date, req.Services,
          req.Employees.Select(e => new EmployeeModel(e.Name)).ToList()
          ); // Conversão de EmployeeRequest para EmployeeModel

        await context.AddAsync(event_); // Adiciona esse evento no context = banco de dados
        await context.SaveChangesAsync(); // Commit para adicionar de fato no banco de dados
      }
    );

    // Get dos eventos com funcionários alocados para os eventos
    route.MapGet("", async (EventContext context) =>
    {
      // Include : inclui os funcionários cadastrados no evento
      var events_ = await context.Events.Include(e => e.Employees).ToListAsync();
      return Results.Ok(events_);
    });

    // Get somente da tabela de funcionários
    route.MapGet("employees", async (EventContext context) =>
    {
      var employees_ = await context.EmployeeModel.ToListAsync();
      return Results.Ok(employees_);
    }
    );

    // Edita nome do contratante, e se fosse patch? testar
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





  }
}

/*

    app.MapGet("event", () =>
    new EventModel(
      "Filipi",
      500,
      DateTime.Now,
      new List<string> { "Animação", "Pintura Facial" },
      new List<EmployeeModel>
      {
        new EmployeeModel("Wesley"),
        new EmployeeModel("Jade")
      }
    )    
    );

*/