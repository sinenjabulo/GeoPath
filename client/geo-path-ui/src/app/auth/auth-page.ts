import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

interface AuthFormState {
  name: string;
  email: string;
  password: string;
}

@Component({
  selector: 'app-auth-page',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './auth-page.html',
  styleUrl: './auth-page.scss',
})
export class AuthPageComponent {
  readonly form: AuthFormState = {
    name: '',
    email: '',
    password: '',
  };

  mode: 'sign-in' | 'register' = 'sign-in';
  isSubmitting = false;
  errorMessage = '';

  constructor(private readonly router: Router) {}

  submit(): void {
    this.errorMessage = '';
    this.isSubmitting = true;

    const payload =
      this.mode === 'register'
        ? {
            name: this.form.name,
            email: this.form.email,
            password: this.form.password,
          }
        : {
            email: this.form.email,
            password: this.form.password,
          };

    const endpoint = this.mode === 'register' ? '/api/auth/register' : '/api/auth/login';

    fetch(`http://localhost:5229${endpoint}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payload),
    })
      .then(async (response) => {
        if (!response.ok) {
          const body = await response.json().catch(() => ({}));
          throw new Error((body as { message?: string })?.message ?? 'Unable to sign in right now.');
        }

        return response.json() as Promise<{ token: string; user?: { name?: string } }>;
      })
      .then((result) => {
        localStorage.setItem('geo-path-token', result.token);
        this.router.navigateByUrl('/dashboard');
      })
      .catch((error: Error) => {
        this.errorMessage = error.message;
      })
      .finally(() => {
        this.isSubmitting = false;
      });
  }

  toggleMode(mode: 'sign-in' | 'register'): void {
    this.mode = mode;
    this.errorMessage = '';
  }
}
