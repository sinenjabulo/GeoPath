import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

interface UserSummary {
  id: string;
  name: string;
  email: string;
  role: string;
}

interface ClassSummary {
  id: string;
  name: string;
  joinedAtUtc: string;
}

interface AttemptSummary {
  id: string;
  exampleId: string;
  status: string;
  startedAtUtc: string;
  completedAtUtc?: string | null;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class DashboardComponent implements OnInit {
  user: UserSummary | null = null;
  classes: ClassSummary[] = [];
  attempts: AttemptSummary[] = [];
  joinCode = '';
  isLoading = true;
  isJoining = false;
  feedbackMessage = '';
  errorMessage = '';

  constructor(private readonly router: Router) {}

  ngOnInit(): void {
    this.loadData();
  }

  async loadData(): Promise<void> {
    const token = localStorage.getItem('geo-path-token');
    if (!token) {
      this.router.navigateByUrl('/auth');
      return;
    }

    try {
      const [meResponse, classesResponse, attemptsResponse] = await Promise.all([
        fetch('http://localhost:5229/api/auth/me', {
          headers: { Authorization: `Bearer ${token}` },
        }),
        fetch('http://localhost:5229/api/classes', {
          headers: { Authorization: `Bearer ${token}` },
        }),
        fetch('http://localhost:5229/api/attempts', {
          headers: { Authorization: `Bearer ${token}` },
        }),
      ]);

      if (meResponse.status === 401) {
        localStorage.removeItem('geo-path-token');
        this.router.navigateByUrl('/auth');
        return;
      }

      if (!meResponse.ok) {
        throw new Error('Could not load your profile.');
      }

      this.user = (await meResponse.json()) as UserSummary;
      this.classes = (await classesResponse.json()) as ClassSummary[];
      this.attempts = (await attemptsResponse.json()) as AttemptSummary[];
      this.feedbackMessage = '';
      this.errorMessage = '';
    } catch (error) {
      this.errorMessage = (error as Error).message || 'Unable to load learner dashboard.';
    } finally {
      this.isLoading = false;
    }
  }

  async joinClass(): Promise<void> {
    const token = localStorage.getItem('geo-path-token');
    if (!token) {
      this.router.navigateByUrl('/auth');
      return;
    }

    const trimmedCode = this.joinCode.trim();
    if (!trimmedCode) {
      this.errorMessage = 'Enter a class code to continue.';
      return;
    }

    this.isJoining = true;
    this.errorMessage = '';
    this.feedbackMessage = '';

    try {
      const response = await fetch('http://localhost:5229/api/classes/join', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({ joinCode: trimmedCode }),
      });

      const body = (await response.json().catch(() => ({}))) as {
        message?: string;
        class?: { id: string; name: string };
      };

      if (!response.ok) {
        throw new Error(body.message ?? 'That join code could not be used.');
      }

      this.feedbackMessage = body.message ?? 'Class joined successfully.';
      this.joinCode = '';
      await this.loadData();
    } catch (error) {
      this.errorMessage = (error as Error).message || 'Unable to join your class.';
    } finally {
      this.isJoining = false;
    }
  }

  signOut(): void {
    localStorage.removeItem('geo-path-token');
    this.router.navigateByUrl('/');
  }
}
