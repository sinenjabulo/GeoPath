import { Routes } from '@angular/router';
import { AuthGuard } from './auth/auth-guard';
import { AuthPageComponent } from './auth/auth-page';
import { ExampleDetailComponent } from './content/example-detail';
import { ExampleLibraryComponent } from './content/example-library';
import { ReferenceDetailComponent } from './content/reference-detail';
import { ReferenceLibraryComponent } from './content/reference-library';
import { DashboardComponent } from './dashboard/dashboard';
import { LandingPageComponent } from './landing/landing-page';

/** Defines the learner entry, content library, and dashboard routes. */
export const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'auth', component: AuthPageComponent },
  { path: 'examples', component: ExampleLibraryComponent },
  { path: 'examples/:id', component: ExampleDetailComponent },
  { path: 'theorems', component: ReferenceLibraryComponent, data: { kind: 'theorem' } },
  { path: 'theorems/:id', component: ReferenceDetailComponent, data: { kind: 'theorem' } },
  { path: 'definitions', component: ReferenceLibraryComponent, data: { kind: 'definition' } },
  { path: 'definitions/:id', component: ReferenceDetailComponent, data: { kind: 'definition' } },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '' },
];
