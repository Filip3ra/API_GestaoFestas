namespace Event.Models;

public class EventModel
{

  public EventModel(string name)
  {
    Name = name;
    Id = Guid.NewGuid();
  }

  /* Guid: identificador global exclusivo, inteiro de 128 bits (16bytes)
  identificador único pra criar Id. 

  "string?" :  a ? é um marcador de nulo, pra avisar que a variável pode ser nula.
  Mas também posso resolver isso colocando " = String.Empty;" ao final.
  Ou, se tenho um construtor eu não preciso de marcador nulo, pois já tenho inicialização que é o construtor.

  init : Vai funcionar somente quando tiver construtor, só posso alterar ele uma vez.

  private : a única parte da aplicação responsável por cuidar das propriedades
  que estão no modelo é o próprio modelo.

  */
  public Guid Id { get; init; }
  public string Name { get; private set; }

}