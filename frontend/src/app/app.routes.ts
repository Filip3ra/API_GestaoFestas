import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { authGuard } from './guards/auth.guard';
import { EventsComponent } from './pages/events/events.component';
import { EmployeesComponent } from './pages/employees/employees.component';
import { AboutComponent } from './pages/about/about.component';

/* Define todas as rotas da aplicação, ou seja, 
qual componente será carregada para cada URL */
export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent }, // Rota para o componente de login
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] }, // Rota para o componente de dashboard, mas só pode ser acessada se o usuário estiver autenticado
  { path: 'events', component: EventsComponent, canActivate: [authGuard] },
  { path: 'employees', component: EmployeesComponent, canActivate: [authGuard] },
  { path: 'about', component: AboutComponent },
];

