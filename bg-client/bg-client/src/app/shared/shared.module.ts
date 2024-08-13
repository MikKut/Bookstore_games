import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormValidationDirective } from './directives/form-validation.directive';
import { DateFormatPipe } from './pipes/date-format.pipe';
import { DatePipe } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { HttpClientModule } from '@angular/common/http';
import { ErrorDialogComponent } from './error-dialog/error-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { LoadingSpinnerComponent } from './loading-spinner/loading-spinner.component';

@NgModule({
  declarations: [
    FormValidationDirective,
    DateFormatPipe,
    HeaderComponent,
    FooterComponent,
    ErrorDialogComponent,
    LoadingSpinnerComponent
  ],
  imports: [
    HttpClientModule,
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule    
  ],
  providers: [DatePipe],
  exports: [
    FormValidationDirective,
    DateFormatPipe,
    HeaderComponent,
    FooterComponent,
    ErrorDialogComponent,
    LoadingSpinnerComponent
  ]
})
export class SharedModule { }
