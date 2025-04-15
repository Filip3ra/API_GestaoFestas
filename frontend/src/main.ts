import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';

/* Inicia o Angular com AppComponent, e appConfig fornece
todos os serviços, rotas e módulos necessários para a aplicação. */
bootstrapApplication(AppComponent, appConfig).catch((err) =>
  console.error(err)
);
