import { Component } from '@angular/core';
import { ApiService } from '../../service/api.service';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-events',
  imports: [RouterModule, CommonModule],
  templateUrl: './events.component.html',
  styleUrl: './events.component.scss'
})
  
export class EventsComponent {

  eventos: any[] = [];
  mostrarEventos: boolean = false;

  constructor(private api: ApiService) { }

  listarEventos() {
    if (this.eventos.length === 0) { 
      this.api.getAllEvents().subscribe({
        next: (res) => {
          this.eventos = res,
            this.mostrarEventos = true;
        },
        error: (err) => {
          console.error(err);
          alert('Erro ao buscar eventos');
        }
      });
    }
    else {
      this.mostrarEventos = !this.mostrarEventos;
    }

  }

}
