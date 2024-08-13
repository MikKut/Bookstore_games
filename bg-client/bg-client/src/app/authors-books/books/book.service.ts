import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BookDto, BooksResponse } from '../../models/book';
import { environment } from '../../../assets/environments/environment.prod';
import { PagedResult } from '../../models/paged-result';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private apiUrl = `${environment.apiBookUrl}`;

  constructor(private http: HttpClient) {}

  getBooks(
    title: string = '',
    genre: string = '',
    pageNumber: number = 1,
    pageSize: number = 10
  ): Observable<PagedResult<BookDto>> {

    const params = new HttpParams()
      .set('title', title)
      .set('genre', genre)
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

      return this.http.get<PagedResult<BookDto>>(this.apiUrl, { params });
  }

  getBookGenres(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/genres`);
  }

  getBookById(id: string): Observable<BookDto> {
    return this.http.get<BookDto>(`${this.apiUrl}/${id}`);
  }

  createBook(book: BookDto): Observable<BookDto> {
    return this.http.post<BookDto>(this.apiUrl, book);
  }

  updateBook(id: string, book: BookDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, book);
  }

  deleteBook(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
