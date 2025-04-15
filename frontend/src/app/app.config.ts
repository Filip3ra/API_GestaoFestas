import { ApplicationConfig, importProvidersFrom  } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './interceptors/auth.interceptor';

/* Configuração dos providers globais da aplicação 
  - provideHttpClient: fornece o HttpClient para fazer requisições HTTP
  - withInterceptors: adiciona interceptores para manipular as requisições e respostas HTTP
  - provideRouter: fornece o roteador para gerenciar as rotas da aplicação
  - importProvidersFrom: importa módulos adicionais, como FormsModule e ReactiveFormsModule, para usar formulários reativos e template-driven na aplicação
*/

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([authInterceptor])),
    provideRouter(routes),
    importProvidersFrom(FormsModule, ReactiveFormsModule),
  ],
};
