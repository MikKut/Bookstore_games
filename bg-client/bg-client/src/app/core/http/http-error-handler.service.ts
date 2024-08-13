import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorHandlerService {

  constructor(private snackBar: MatSnackBar) {}

  handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred!';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Backend returned an unsuccessful response code
      if (error.status === 0) {
        errorMessage = 'Network error. Please check your internet connection.';
      } else if (error.status >= 400 && error.status < 500) {
        errorMessage = `Client-side error: ${error.error?.message || error.message}`;
      } else if (error.status >= 500) {
        errorMessage = 'Server error. Please try again later.';
      }
    }

    this.showErrorMessage(errorMessage);
    return throwError(() => new Error(errorMessage));
  }

  private showErrorMessage(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 5000,
    });
  }
}
