import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BookService } from '../book.service';

@Component({
  selector: 'app-add-book',
  templateUrl: './add-book.component.html',
  styleUrls: ['./add-book.component.scss']
})
export class AddBookComponent implements OnInit {
  addBookForm: FormGroup;
  isLoading = false;
  errorMessage = '';
  genres: string[] = []; // Array to store available genres
  selectedGenre = ''; // Holds the selected genre

  constructor(
    private fb: FormBuilder,
    private booksService: BookService,
    private router: Router
  ) {
    this.addBookForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(100)]],
      publicationYear: ['', [Validators.required, Validators.min(1450), Validators.max(new Date().getFullYear())]],
      genre: ['', [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.loadGenres(); // Load genres when the component initializes
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

  onSubmit(): void {
    if (this.addBookForm.invalid) {
      return;
    }

    this.isLoading = true;
    this.booksService.createBook(this.addBookForm.value).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['/books/list']);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage =  + err?.error + ": "+ err?.error?.message || 'An error occurred while adding the book.';
      }
    });
  }
}
