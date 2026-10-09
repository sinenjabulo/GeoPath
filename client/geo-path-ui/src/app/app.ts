import { DatePipe } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';

/** Describes the status payload returned by the API health endpoint. */
interface PlatformStatus {
  /** Name of the platform reported by the API. */
  applicationName: string;
  /** Current operational status reported by the API. */
  status: string;
  /** Deployed platform version. */
  version: string;
  /** Hosting environment serving the API. */
  environment: string;
  /** UTC time when the status was generated. */
  timestampUtc: string;
  /** Technology names used by the platform. */
  techStack: string[];
}

/** Root component that displays the platform shell and API health status. */
@Component({
  selector: 'app-root',
  imports: [DatePipe],
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App implements OnInit {
  /** Product name displayed in the application shell. */
  protected readonly title = 'GeoPath';
  /** Indicates whether the initial API health request is pending. */
  protected readonly loading = signal(true);
  /** Holds the API health response or a fallback status when the API is unavailable. */
  protected readonly platformStatus = signal<PlatformStatus | null>(null);

  /** Loads API health details and displays a fallback when the request fails. */
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
