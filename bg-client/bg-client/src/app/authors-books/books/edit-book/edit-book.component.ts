import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BookService } from '../book.service';
import { BookDto } from '../../../models/book';

@Component({
  selector: 'app-edit-book',
  templateUrl: './edit-book.component.html',
  styleUrls: ['./edit-book.component.scss']
})
export class EditBookComponent implements OnInit {
  bookForm: FormGroup;
  bookId!: string;

  constructor(
    private fb: FormBuilder,
    private bookService: BookService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.bookForm = this.fb.group({
      title: ['', Validators.required],
      publicationYear: ['', Validators.required],
      genre: ['', Validators.required]
    });

    this.bookId = this.route.snapshot.params['id'];
  }

  ngOnInit(): void {
    this.loadBookDetails();
  }

  private loadBookDetails(): void {
    this.bookService.getBookById(this.bookId).subscribe({
      next: (book) => {
        this.bookForm.patchValue(book);
      },
      error: (err) => {
        console.error('Failed to load book details', err);
      }
    });
  }

  onSubmit(): void {
    if (this.bookForm.valid) {
      this.bookService.updateBook(this.bookId, this.bookForm.value).subscribe({
        next: () => {
          this.router.navigate(['/books/list']);
        },
        error: (err) => {
          console.error('Failed to update book', err);
        }
      });
    }
  }
}
