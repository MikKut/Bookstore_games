import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthorService } from '../author.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-author',
  templateUrl: './add-author.component.html',
  styleUrls: ['./add-author.component.scss']
})
export class AddAuthorComponent {
  addAuthorForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authorsService: AuthorService,
    private router: Router
  ) {
    this.addAuthorForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      dateOfBirth: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.addAuthorForm.valid) {
      this.authorsService.createAuthor(this.addAuthorForm.value).subscribe({
        next: () => this.router.navigate(['/authors/list']),
        error: (err) => console.error('Error creating author: ', err)
      });
    }
  }
}
