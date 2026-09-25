import { Component, OnInit, OnDestroy } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MetricsService } from './core/services/metrics.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: `<router-outlet />`
})
export class App implements OnInit, OnDestroy {
  constructor(private metricsService: MetricsService) {}

  async ngOnInit() {
    // Инициализируем один раз на старте
    await this.metricsService.initialize();
  }

  async ngOnDestroy() {
    await this.metricsService.disconnect();
  }
}
