
interface Token {
  key: string;
}

export interface LoginResponse {
  token: Token;
}
export interface LoginRequest {
  email: string;
  password: string;
}

interface Avatar {
  original_url: string;
  large_url: string;
  medium_url: string;
}

export interface UserResponse {
  admin: boolean;
  allowed_to_order: boolean;

  name: string;
  email: string;
  email_verified: boolean;
  avatar?: Avatar;
  first_name: string;
  last_name: string;
  phone: string;
  birth_date: string;
}
