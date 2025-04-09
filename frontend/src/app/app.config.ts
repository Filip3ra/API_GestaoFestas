import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
//import { provideHttpClient } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './interceptors/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(), // Adiciona suporte a requisições HTTP
    FormsModule, // Adiciona suporte a formulários
    ReactiveFormsModule, // Adiciona suporte a formulários reativos
    provideHttpClient(withInterceptors([authInterceptor])),
    provideRouter(routes),
  ],
};
