import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthorService } from '../author.service';
import { AuthorDto } from '../../../models/author';

@Component({
  selector: 'app-edit-author',
  templateUrl: './edit-author.component.html',
  styleUrls: ['./edit-author.component.scss']
})
export class EditAuthorComponent implements OnInit {
  editAuthorForm: FormGroup;
  authorId!: string;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private authorsService: AuthorService,
    private router: Router
  ) {
    this.editAuthorForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      dateOfBirth: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.authorId = this.route.snapshot.paramMap.get('id')!;
    this.authorsService.getAuthorById(this.authorId).subscribe({
      next: (author: AuthorDto) => {
        this.editAuthorForm.patchValue({
          firstName: author.firstName,
          lastName: author.lastName,
          dateOfBirth: this.formatDate(author.dateOfBirth)
        });
      },
      error: (err) => console.error('Error fetching author: ', err)
    });
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toISOString().substring(0, 10);
  }

  onSubmit() {
    if (this.editAuthorForm.valid) {
      this.authorsService.updateAuthor(this.authorId, this.editAuthorForm.value).subscribe({
        next: () => this.router.navigate(['/authors/list']),
        error: (err) => console.error('Error updating author: ', err)
      });
    }
  }
}
