namespace Event.Data;

using Employee.Models;
using Event.Models;
using Microsoft.EntityFrameworkCore;

public class EventContext : DbContext
{
  // Os nomes representam o nome da tabela no bd
  public DbSet<EventModel> Events { get; set; }
  public DbSet<EmployeeModel> EmployeeModel { get; set; }

  // Configura quem é o provider do bd, que será o Sqlite
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlite("Data Source=event.sqlite");
    base.OnConfiguring(optionsBuilder);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<EventModel>()
    .HasMany(e => e.Employees)
    .WithMany(e => e.Events)
    .UsingEntity(j => j.ToTable("EventEmployee"));
  }

}

/*  LEIA-ME -- Necessário adicionar pacotes: 

Ao criar esse arquico EventContext.cs preciso instalar e adicionar pacotes:
  > "dotnet tool install --global dotnet-ef --version 8.*"
  
  Obs: "--global": versão global do entity framework, pois vamos usar comandos como: 
  "dotnet ef", "database update", "migrations", etc. Já em "8.*" é a versão 8 mais 
  atualizada que tiver.

  E dentro da pasta do projeto é precio adicionar os pacotes:
  > "dotnet add package Microsoft.EntityFrameworkCore --version 8.*"
  > "dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.*"
  > "dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.*"

  Obs: 
  "Design": como vamos trabalhar com migrations, ele lida com a questão de criar as migrations
  
  "Core":  Significa que estamos trabalhando com code first, não precisamos mexer em codigo sqlite,
  não vamos criar o bd na mão, criamos apenas o código c# e através da migrations pegamos o código
  c# e convertemos ele pra código Sqlite.

  Ao utilizar magrations, tudo deve ser feito automático, não se faz manual, se precisar podemos
  usar um Undo, Down para Dropar a tabela, etc.

  "DbContext": é a representação do banco de dados, e eu sei que 
  nele terei um EventContext. Mas preciso dizer quais são as tabelas
  que vou ter no bd. 

  "DbSet<EventModel>": representa a tabela, que é a tabela EventModel.

Para iniciar a migração: "dotnet ef migrations add Initial"
Para criar o bd: "dotnet ef database update"
*/