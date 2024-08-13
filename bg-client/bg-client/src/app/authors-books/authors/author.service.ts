import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthorDto } from '../../models/author';
import { environment } from '../../../assets/environments/environment.prod';
import { PagedResult } from '../../models/paged-result';
@Injectable({
  providedIn: 'root'
})
export class AuthorService {
  private apiUrl = `${environment.apiAuthorUrl}`;

  constructor(private http: HttpClient) {}

  getAuthors(
    firstName?: string, 
    lastName?: string, 
    pageNumber: number = 1, 
    pageSize: number = 10
  ): Observable<PagedResult<AuthorDto>> {
    let params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());

    if (firstName) {
      params = params.set('FirstName', firstName);
    }
    if (lastName) {
      params = params.set('LastName', lastName);
    }

    return this.http.get<PagedResult<AuthorDto>>(this.apiUrl, { params });
  }

  getAuthorById(id: string): Observable<AuthorDto> {
    return this.http.get<AuthorDto>(`${this.apiUrl}/${id}`);
  }

  createAuthor(author: AuthorDto): Observable<AuthorDto> {
    return this.http.post<AuthorDto>(this.apiUrl, author);
  }

  updateAuthor(id: string, author: AuthorDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, author);
  }

  deleteAuthor(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
