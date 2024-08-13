import { NgModule } from '@angular/core';   
import { RouterModule } from '@angular/router';
import { routes } from './app.routes';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
@NgModule({
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    RouterModule.forRoot(routes)
  ],
  bootstrap: []  // Empty because AppComponent is standalone
})
export class AppModule { }
