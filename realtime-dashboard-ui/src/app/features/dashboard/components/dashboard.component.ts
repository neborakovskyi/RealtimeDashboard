import {
  Component, ChangeDetectionStrategy
} from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatBadgeModule } from '@angular/material/badge';
import { MatIconModule } from '@angular/material/icon';
import { MetricsService } from '../../../core/services/metrics.service';
import { MetricCardComponent } from './metric-card.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    MatToolbarModule, MatBadgeModule,
    MatIconModule,
    MetricCardComponent
  ],
template: `
  <mat-toolbar color="primary">
    <span>Realtime Dashboard</span>
    <span style="flex:1"></span>
    <mat-icon
      [style.color]="metrics.connected() ? '#4caf50' : '#f44336'">
      {{ metrics.connected() ? 'wifi' : 'wifi_off' }}
    </mat-icon>
  </mat-toolbar>

  <div class="dashboard">

    @if (metrics.userMetrics().length) {
      <section>
        <h2>👥 Users</h2>
        <div class="grid">
          @for (m of metrics.userMetrics(); track m.id) {
            <app-metric-card [metric]="m" />
          }
        </div>
      </section>
    }

    @if (metrics.orderMetrics().length) {
      <section>
        <h2>🛒 Orders</h2>
        <div class="grid">
          @for (m of metrics.orderMetrics(); track m.id) {
            <app-metric-card [metric]="m" />
          }
        </div>
      </section>
    }

    @if (metrics.aiMetrics().length) {
      <section>
        <h2>🤖 AI Tasks</h2>
        <div class="grid">
          @for (m of metrics.aiMetrics(); track m.id) {
            <app-metric-card [metric]="m" />
          }
        </div>
      </section>
    }

    @if (metrics.systemMetrics().length) {
      <section>
        <h2>⚙️ System</h2>
        <div class="grid">
          @for (m of metrics.systemMetrics(); track m.id) {
            <app-metric-card [metric]="m" />
          }
        </div>
      </section>
    }

  </div>
`,
  styles: [`
    .dashboard {
      padding: 24px;
      max-width: 1200px;
      margin: 0 auto;
    }
    section { margin-bottom: 32px; }
    h2 {
      font-size: 1.1rem;
      font-weight: 600;
      margin-bottom: 12px;
      color: var(--mat-sys-on-surface-variant);
    }
    .grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
      gap: 16px;
    }
  `]
})
export class DashboardComponent {
  constructor(readonly metrics: MetricsService) {}
}
