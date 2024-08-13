import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { AuthorRoutingModule } from './authors-routing.module';
import { AddAuthorComponent } from './add-author/add-author.component';
import { EditAuthorComponent } from './edit-author/edit-author.component';
import { AuthorsListComponent } from './authors-list/authors-list.component';
import { SharedModule } from '../../shared/shared.module';
import { CoreModule } from '../../core/core.module';

@NgModule({
  declarations: [
    AddAuthorComponent,
    EditAuthorComponent,
    AuthorsListComponent
  ],
  imports: [
    FormsModule,
    CommonModule,
    CoreModule,
    ReactiveFormsModule,
    AuthorRoutingModule,
    SharedModule
  ],
  providers: []
})
export class AuthorModule { }
