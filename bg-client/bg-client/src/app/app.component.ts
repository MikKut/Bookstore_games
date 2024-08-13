import { Component } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { AuthorModule } from './authors-books/authors/authors.module';
import { BookModule } from './authors-books/books/books.module';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    SharedModule,
    CoreModule,
    CommonModule,
    RouterModule,
    AuthorModule,
    BookModule
    ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'My Angular App';
}