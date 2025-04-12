import { Component } from '@angular/core';
import { ApiService } from '../../service/api.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: true,
  imports: [FormsModule],
})
export class LoginComponent {
  username = '';
  password = '';

  constructor(private api: ApiService, private router: Router) {}

  login() {    
    this.api.login({ username: this.username, password: this.password }).subscribe({
        next: (res) => {
          localStorage.setItem('token', res.token);
          alert('Login realizado com sucesso!');
          this.router.navigate(['/dashboard']);
        },
        error: () => {
          alert('Usuário ou senha inválidos');
        },
      });
  }
}
