import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const AuthGuard: CanActivateFn = () => {
  const router = inject(Router);
  const token = localStorage.getItem('geo-path-token');

  if (!token) {
    return router.createUrlTree(['/auth']);
  }

  return true;
};
