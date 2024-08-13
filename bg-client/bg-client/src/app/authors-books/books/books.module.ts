import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BookRoutingModule } from './books-routing.module';
import { AddBookComponent } from './add-book/add-book.component';
import { EditBookComponent } from './edit-book/edit-book.component';
import { BooksListComponent } from './books-list/books-list.component';
import { SharedModule } from '../../shared/shared.module';
import { CoreModule } from '../../core/core.module';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
@NgModule({
  declarations: [
    AddBookComponent,
    EditBookComponent,
    BooksListComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    CoreModule,
    ReactiveFormsModule,
    BookRoutingModule
  ],
  providers: []
})
export class BookModule { }
