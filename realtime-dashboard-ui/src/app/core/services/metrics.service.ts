import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';
import { Metric } from '../models/metric.model';

@Injectable({ providedIn: 'root' })
export class MetricsService {
  // private readonly apiUrl = 'https://localhost:7170/api/metrics';
  // private readonly hubUrl = 'https://localhost:7170/hubs/metrics';
  private readonly apiUrl = 'http://localhost:5049/api/metrics';
  private readonly hubUrl = 'http://localhost:5049/hubs/metrics';

  // Signals — реактивное хранилище состояния (Angular 17+)
  // проще NgRx для такого случая
  private readonly _metrics = signal<Metric[]>([]);
  private readonly _connected = signal(false);

  // Публичные computed — компоненты читают отсюда
  readonly metrics = this._metrics.asReadonly();
  readonly connected = this._connected.asReadonly();

  // Фильтры по категории — computed пересчитывается автоматически
  readonly userMetrics = computed(() =>
    this._metrics().filter(m => m.category === 'Users'));
  readonly orderMetrics = computed(() =>
    this._metrics().filter(m => m.category === 'Orders'));
  readonly aiMetrics = computed(() =>
    this._metrics().filter(m => m.category === 'AI'));
  readonly systemMetrics = computed(() =>
    this._metrics().filter(m => m.category === 'System'));

  private hub!: signalR.HubConnection;

  constructor(private http: HttpClient) { }

  // Вызвать один раз при старте приложения
  async initialize(): Promise<void> {
    // 1. Сначала загружаем текущие данные по HTTP
    this.http.get<Metric[]>(this.apiUrl).subscribe(metrics => {
      this._metrics.set(metrics);
    });

    // 2. Потом подключаемся к SignalR для обновлений
    await this.connectHub();
  }

  private async connectHub(): Promise<void> {
    this.hub = new signalR.HubConnectionBuilder()
      .withUrl(this.hubUrl, {
        // Порядок транспортов: WebSocket → SSE → LongPolling
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect([0, 2000, 5000, 10000]) // реконнект
      .configureLogging(signalR.LogLevel.Warning)
      .build();

    // Слушаем батч-обновления от воркера
    this.hub.on('MetricsBatchUpdated', (updated: Metric[]) => {
      this._metrics.update(current => {
        // Мержим обновления в текущий список
        const map = new Map(current.map(m => [m.id, m]));
        updated.forEach(m => map.set(m.id, m));
        return Array.from(map.values());
      });
    });

    this.hub.onreconnecting(() => this._connected.set(false));
    this.hub.onreconnected(() => this._connected.set(true));
    this.hub.onclose(() => this._connected.set(false));

    await this.hub.start();
    this._connected.set(true);
  }

  async disconnect(): Promise<void> {
  // Останавливаем только если соединение установлено
  if (this.hub &&
      this.hub.state === signalR.HubConnectionState.Connected) {
    await this.hub.stop();
  }
}
}
