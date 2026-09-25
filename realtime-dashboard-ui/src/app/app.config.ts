import { ApplicationConfig, provideZonelessChangeDetection ,APP_INITIALIZER}
  from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimationsAsync } from
  '@angular/platform-browser/animations/async';
import { routes } from './app.routes';
import { MetricsService } from './core/services/metrics.service';

export const appConfig: ApplicationConfig = {
  providers: [
    // Заменили provideZoneChangeDetection на zoneless
    // Signals работают без Zone.js — это современный подход
    provideZonelessChangeDetection(),
    provideRouter(routes),
    provideHttpClient(),
    provideAnimationsAsync(),
      {
      provide: APP_INITIALIZER,
      // Singleton сервис инициализируется один раз при старте
      // не зависит от lifecycle компонентов
      useFactory: (metrics: MetricsService) => () => metrics.initialize(),
      deps: [MetricsService],
      multi: true
    }
  ]
};
