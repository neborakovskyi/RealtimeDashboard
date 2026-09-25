export interface Metric {
  id: string;
  key: string;
  label: string;
  value: number;
  unit: string;
  category: 'Users' | 'Orders' | 'AI' | 'System';
  updatedAt: string
}
