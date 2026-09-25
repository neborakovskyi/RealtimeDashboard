import {
  Component, Input, ChangeDetectionStrategy
} from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { DatePipe, DecimalPipe } from '@angular/common';
import { Metric } from '../../../core/models/metric.model';

@Component({
  selector: 'app-metric-card',
  standalone: true,
  imports: [MatCardModule, DatePipe, DecimalPipe],
  changeDetection: ChangeDetectionStrategy.OnPush, // перерисовка только при новом Input
  template: `
    <mat-card class="metric-card">
      <mat-card-header>
        <mat-card-subtitle>{{ metric.label }}</mat-card-subtitle>
      </mat-card-header>
      <mat-card-content>
        <div class="value">
          {{ metric.value | number:'1.0-0' }}
          <span class="unit">{{ metric.unit }}</span>
        </div>
        <div class="updated">
          {{ metric.updatedAt | date:'HH:mm:ss' }}
        </div>
      </mat-card-content>
    </mat-card>
  `,
  styles: [`
    .metric-card {
      height: 140px;
      display: flex;
      flex-direction: column;
      justify-content: center;
    }
    .value {
      font-size: 2.5rem;
      font-weight: 700;
      color: var(--mat-sys-primary);
      line-height: 1;
      margin: 8px 0 4px;
    }
    .unit {
      font-size: 1rem;
      font-weight: 400;
      color: var(--mat-sys-outline);
      margin-left: 4px;
    }
    .updated {
      font-size: 0.75rem;
      color: var(--mat-sys-outline);
    }
  `]
})
export class MetricCardComponent {
  @Input({ required: true }) metric!: Metric;
}
