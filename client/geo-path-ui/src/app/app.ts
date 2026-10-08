import { DatePipe } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';

interface PlatformStatus {
  applicationName: string;
  status: string;
  version: string;
  environment: string;
  timestampUtc: string;
  techStack: string[];
}

@Component({
  selector: 'app-root',
  imports: [DatePipe],
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App implements OnInit {
  protected readonly title = 'GeoPath';
  protected readonly loading = signal(true);
  protected readonly platformStatus = signal<PlatformStatus | null>(null);

  ngOnInit(): void {
    fetch('http://localhost:5229/api/health')
      .then(async (response) => {
        if (!response.ok) {
          throw new Error(`API unavailable (${response.status})`);
        }

        return (await response.json()) as PlatformStatus;
      })
      .then((status) => {
        this.platformStatus.set(status);
      })
      .catch(() => {
        this.platformStatus.set({
          applicationName: 'GeoPath',
          status: 'Unavailable',
          version: '0.1.0',
          environment: 'Development',
          timestampUtc: new Date().toISOString(),
          techStack: ['Angular', 'C#/.NET', 'PostgreSQL'],
        });
      })
      .finally(() => {
        this.loading.set(false);
      });
  }
}
