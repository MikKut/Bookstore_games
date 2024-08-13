import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BookService } from '../book.service';
import { BookDto } from '../../../models/book';

@Component({
  selector: 'app-books-list',
  templateUrl: './books-list.component.html',
  styleUrls: ['./books-list.component.scss']
})
export class BooksListComponent implements OnInit {
  books: BookDto[] = [];
  genres: string[] = [];
  selectedGenre: string = '';
  searchTitle: string = '';
  currentPage: number = 1;
  totalPages: number = 0;
  totalItems: number = 0;
  pageSize: number = 10;

  constructor(
    private booksService: BookService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadGenres();
    this.loadBooks();
  }

  private loadBooks(): void {
    this.booksService.getBooks(this.searchTitle, this.selectedGenre, this.currentPage, this.pageSize).subscribe({
      next: (response) => {
        this.books = response.items;
        this.currentPage = response.pageNumber;
        this.pageSize = response.pageSize;
        this.totalItems = response.totalCount;
        this.updateTotalPages();
      },
      error: (err) => {
        console.error('Failed to load books', err);
      }
    });
  }

  private loadGenres(): void {
    this.booksService.getBookGenres().subscribe({
      next: (genres) => {
        this.genres = genres;
      },
      error: (err) => {
        console.error('Failed to load genres', err);
      }
    });
  }

  updateTotalPages(): void {
    this.totalPages = Math.ceil(this.totalItems / this.pageSize);
  }

  filterBooks(): void {
    this.currentPage = 1; // Reset to the first page on filter change
    this.loadBooks();
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadBooks();
    }
  }

  deleteBook(id: string): void {
    if (confirm('Are you sure you want to delete this book?')) {
      this.booksService.deleteBook(id).subscribe({
        next: () => {
          this.books = this.books.filter(book => book.id !== id);
          this.loadBooks(); // Reload books after deletion
        },
        error: (err) => {
          console.error('Failed to delete book', err);
        }
      });
    }
  }

  editBook(id: string): void {
    this.router.navigate(['/books/edit', id]);
  }

  addBook(): void {
    this.router.navigate(['/books/add']);
  }
}
