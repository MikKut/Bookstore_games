import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddAuthorComponent } from './add-author/add-author.component';
import { EditAuthorComponent } from './edit-author/edit-author.component';
import { AuthorsListComponent } from './authors-list/authors-list.component';

const routes: Routes = [
  { path: 'add', component: AddAuthorComponent },
  { path: 'edit/:id', component: EditAuthorComponent },
  { path: 'list', component: AuthorsListComponent },
  { path: '', redirectTo: 'authors', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthorRoutingModule { }
