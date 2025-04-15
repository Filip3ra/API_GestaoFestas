import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FooterComponent } from './layout/footer/footer.component';
import { NavbarComponent } from "./layout/navbar/navbar.component";

/**
 * Component raiz da aplicação, primeiro a ser carregado.
 * selector: 'app-root' - renderiza esse componente onde tiver <app-root> no index.html
 * templateUrl: './app.component.html' - arquivo HTML que renderiza o componente
 * styleUrls: ['./app.component.scss'] - arquivo CSS que estiliza o componente
 * imports: [RouterOutlet] - importa o RouterOutlet, que é responsável por renderizar os componentes de acordo com as rotas definidas
 * @standalone: true - indica que esse componente é um componente autônomo, ou seja, não precisa ser declarado em um módulo Angular
 */

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FooterComponent, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'frontend';
}
