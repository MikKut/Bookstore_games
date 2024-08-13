import { NgModule, Optional, SkipSelf } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AuthService } from '../auth/auth.service';
import { JwtInterceptorService } from './interceptors/jwt-interceptor-service';
import { HttpErrorHandlerService } from './http/http-error-handler.service';
import { HttpInterceptorService } from './http/http-interceptor.service';
import { BookService } from '../authors-books/books/book.service';
import { AuthorService } from '../authors-books/authors/author.service';
@NgModule({
  imports: [
    HttpClientModule
  ],
  providers: [
    AuthService,
    BookService,
    AuthorService,
    HttpErrorHandlerService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptorService,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpInterceptorService,
      multi: true
    }
  ]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error('CoreModule is already loaded. Import it in the AppModule only.');
    }
  }
}
