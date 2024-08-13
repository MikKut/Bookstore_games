import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, finalize, tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthService } from '../../auth/auth.service';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { LoadingService } from '../../shared/loading.service';
import { MatDialog } from '@angular/material/dialog';
import { ErrorDialogComponent } from '../../shared/error-dialog/error-dialog.component';

@Injectable()
export class HttpInterceptorService implements HttpInterceptor {

  constructor(
    private authService: AuthService,
    private httpErrorHandler: HttpErrorHandlerService,
    private loadingService: LoadingService,
    private router: Router,
    private dialog: MatDialog
  ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let authReq = req;

    const authToken = this.authService.getAuthToken();
    if (authToken) {
      authReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${authToken}`
        }
      });
    }

    this.loadingService.show();

    console.log('HTTP Request:', {
      url: authReq.url,
      method: authReq.method,
      headers: authReq.headers,
      body: authReq.body
    });

    return next.handle(authReq).pipe(
      tap((event: HttpEvent<any>) => {
        // Log the response
        if (event instanceof HttpErrorResponse) {
          console.log('HTTP Error Response:', event);
        } else {
          console.log('HTTP Response:', event);
        }
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 404) {
          this.router.navigate(['/error'], { state: { message: 'Resource not found' } });
        } else  {
          console.log(error);
          this.dialog.open(ErrorDialogComponent, {
            data: { message: error?.error + ":\n" + error?.message + ":\n" + error?.error?.title}
          });
        }
        return this.httpErrorHandler.handleError(error);
      }),
      finalize(() => this.loadingService.hide())
    );
  }
}
