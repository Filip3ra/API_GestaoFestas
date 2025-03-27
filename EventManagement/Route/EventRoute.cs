using Employee.Models;
using Event.Models;

namespace Event.Routes;

public static class EventRoute
{
  /* this : extension method, uso a própria instância como método de extensão.
    onde tenho um método base e extendo ele a outros recursos e métodos.
  */
  public static void EventRoutes(this WebApplication app)
  {

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
  }
}