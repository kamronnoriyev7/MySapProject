import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  
  const sessionId = authService.getSessionId();
  const routeId = authService.getRouteId();
  
  if (sessionId && routeId && !req.url.includes('/auth/login')) {
    const authReq = req.clone({
      setHeaders: {
        'Cookie': `B1SESSION=${sessionId}; ROUTEID=${routeId}`
      }
    });
    return next(authReq);
  }
  
  return next(req);
};