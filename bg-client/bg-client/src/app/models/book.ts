export interface BookDto {
    id: string;
    title: string;
    publicationYear: number;
    genre: string;
  }

  export interface BooksResponse {
    books: BookDto[];
    totalCount: number;
  }