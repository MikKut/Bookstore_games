  export interface UserDto {
    id: string;
    username: string;
    firstName: string;
    lastName: string;
    dateOfBirth: Date;
    address: string;
  }
  
  export interface UserResponseDto {
    token: string;
    user: UserDto;
  }