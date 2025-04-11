import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ApiService } from '../../service/api.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './employees.component.html',
  styleUrl: './employees.component.scss'
})
export class EmployeesComponent {
  employees: any[] = [];
  mostrarFuncionarios = false;

  constructor(private api: ApiService) {}

  listarFuncionarios() {
    if (this.employees.length === 0) {
      this.api.getAllEmployees().subscribe({
        next: (res) => {
          this.employees = res;
          this.mostrarFuncionarios = true;
        },
        error: (err) => {
          console.error(err);
          alert('Erro ao buscar funcionários');
        }
      });
    } else {
      this.mostrarFuncionarios = !this.mostrarFuncionarios;
    }
  }

  carregarEventos(funcionario: any) {
    if (!funcionario.events) {
      this.api.getEventsByEmployeeId(funcionario.id).subscribe({
        next: (eventos) => {
          funcionario.events = eventos;
        },
        error: (err) => {
          console.error(err);
          alert(`Erro ao buscar eventos do funcionário ${funcionario.name}`);
        }
      });
    } else {
      funcionario.events = null; // Oculta eventos se já estiverem visíveis
    }
  }
}
