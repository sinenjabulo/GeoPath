import { Routes } from '@angular/router';
import { AuthGuard } from './auth/auth-guard';
import { AuthPageComponent } from './auth/auth-page';
import { DashboardComponent } from './dashboard/dashboard';
import { LandingPageComponent } from './landing/landing-page';

/** Defines the learner entry and dashboard routes for Sprint 5. */
export const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'auth', component: AuthPageComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '' },
];
