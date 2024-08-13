import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorService } from '../author.service';
import { AuthorDto } from '../../../models/author';

@Component({
  selector: 'app-authors-list',
  templateUrl: './authors-list.component.html',
  styleUrls: ['./authors-list.component.scss']
})
export class AuthorsListComponent implements OnInit {
  authors: AuthorDto[] = [];
  searchFirstName: string = '';
  searchLastName: string = '';
  currentPage: number = 1;
  totalPages: number = 0;
  pageSize: number = 10;

  constructor(private authorService: AuthorService, private router: Router) {}

  ngOnInit(): void {
    this.loadAuthors();
  }

  loadAuthors(): void {
    this.authorService.getAuthors(
      this.searchFirstName,
      this.searchLastName,
      this.currentPage,
      this.pageSize
    ).subscribe({
      next: (response) => {
        this.authors = response.items;
        this.currentPage = response.pageNumber;
        this.pageSize = response.pageSize;
        this.totalPages = Math.ceil(response.totalCount / this.pageSize);
      },
      error: (err) => {
        console.error('Failed to load authors', err);
      }
    });
  }

  filterAuthors(): void {
    this.currentPage = 1; // Reset to the first page on filter change
    this.loadAuthors();
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadAuthors();
    }
  }

  deleteAuthor(id: string): void {
    if (confirm('Are you sure you want to delete this author?')) {
      this.authorService.deleteAuthor(id).subscribe({
        next: () => {
          this.loadAuthors(); // Reload authors after deletion
        },
        error: (err) => {
          console.error('Failed to delete author', err);
        }
      });
    }
  }
  
  addAuthor(): void {
    this.router.navigate(['/authors/add']);
  }
  editAuthor(id: string): void {
    this.router.navigate(['/authors/edit', id]);
  }
}