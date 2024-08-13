import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../assets/environments/environment.prod';
import { UserDto } from '../models/user';
import { UserResponseDto } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiAuthUrl}`;
  private apiUserUrl = `${environment.apiUserUrl}`;
  private userSubject: BehaviorSubject<UserDto | null>;
  public user$: Observable<UserDto | null>;
  
  constructor(private http: HttpClient)  {
    const storedUser = this.getUser();
    this.userSubject = new BehaviorSubject<UserDto | null>(storedUser);
    this.user$ = this.userSubject.asObservable();
  }

  login(credentials: { username: string; password: string }): Observable<void> {
    console.log("(before login "); 
    return this.http.post<UserResponseDto>(`${this.apiUrl}/login`, credentials).pipe(
      map(response => {
        console.log("(after login auth token: "); console.log(localStorage.getItem('authToken'));
        localStorage.setItem('authToken', response.token);
        localStorage.setItem('user', JSON.stringify(response.user));
        this.userSubject.next(response.user);
      })
    );
  }
  
  register(user: any): Observable<void> {
    console.log("befor reg "); 
    return this.http.post<UserResponseDto>(`${this.apiUrl}/register`, user).pipe(
      map(response => {
          console.log("iside reg before toke "); 
          localStorage.setItem('authToken', response.token);
          localStorage.setItem('user', JSON.stringify(response.user));
          this.userSubject.next(response.user);
      })
    );
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('authToken');
  }

  logout(): void {
    localStorage.removeItem('authToken');
    localStorage.removeItem('user');
    this.userSubject.next(null);
  }

  getAuthToken(): string | null {
    console.log("(bef) auth token: "); console.log(localStorage.getItem('authToken'));
    return localStorage.getItem('authToken');
  }

  getUser(): UserDto | null {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }
}